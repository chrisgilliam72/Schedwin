using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class UserInfoCntrlViewModel : ItemCntrlViewModelBase
    {
        private User _selectedUser;
        public User SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                if (value != null)
                {
                    FullName = value.FirstName + " " + value.Surname;
                    Email = value.Email;
                    UserName = value.Username;
                    _selectedUser = value;


                }
                else
                {
                    FullName = "";
                    Email = "";
                    UserName = "";
                    _selectedUser = null;
                }

                NotifyPropertyChanged("SelectedUser");
                NotifyPropertyChanged("Email");
                NotifyPropertyChanged("UserName");
                NotifyPropertyChanged("FullName");

            }
        }


       
        public String UserName { get; set; }

        public String FullName { get; set; }

        public String Email { get; set; }

        public bool IsActive { get; set; }


        public RangeObservableCollection<User> Users { get; set; }

        public RangeObservableCollection<UserType> ListUserTypes { get; set; }

        public RangeObservableCollection<Company> ListAgencies { get; set; }
        public RangeObservableCollection<ModulePermission> ModulePermissions { get; set; }

        public UserType SelectedUserType { get; set; }

        public Company SelectedAgency { get; set; }

        public UserInfoCntrlViewModel()
        {
            ListUserTypes = new RangeObservableCollection<UserType>();
            Users = new RangeObservableCollection<User>();
            ModulePermissions = new RangeObservableCollection<ModulePermission>();
            ListAgencies = new RangeObservableCollection<Company>();
        }

        public override bool Validate()
        {
            return true;
        }

        public override async Task <bool> Save()
        {
            try
            {
                var User = new User();
                User.FirstName = SelectedUser.FirstName;
                User.Surname = SelectedUser.Surname;
                User.Username = SelectedUser.Username;
                User.Email = SelectedUser.Email;
                User.ModulePermissions.AddRange(ModulePermissions);
                User.Active = IsActive;
                User.IsNew = IsNew;
                User.IDX = IDX;
                User.IDX_UserType = SelectedUserType.IDX;
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await User.Save(Server, Database);
                }

                User.UpdateCachedObject(User);

                return true;
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save user details " + Environment.NewLine + exMessage);
                return false;
            }
        }

       

        private List<User> GetWishAgents()
        {
            List<User> userList = null;

            using (var context = new PrincipalContext(ContextType.Domain, "wilderness.co.za"))
            {
                using (var group = GroupPrincipal.FindByIdentity(context, "#APP.rw-WS-WISH"))
                {
                    if (group != null)
                    {
                        var tmplst = group.GetMembers(true).OrderBy(x => x.DisplayName);
                        var tmplst2 = tmplst.Select(x => (UserPrincipal)x).ToList();
                        userList = tmplst2.Select(x => (User)x).OrderBy(x => x.FullName).ToList();
                    }

                }
            }

            return userList;
        }

        private List<User> GetSchedwinADUsers()
        {

            List<User> userList = null;
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, "wilderness.co.za"))
                {
                    using (var group = GroupPrincipal.FindByIdentity(context, "#APP-JNB-dbo-Schedwin"))
                    {
                        if (group != null)
                        {
                            var tmplst = group.GetMembers(true).OrderBy(x => x.DisplayName);
                             var tmplst2 = tmplst.Select(x => (UserPrincipal)x).ToList();
                            userList= tmplst2.Select(x=> (User)x).OrderBy(x=>x.FullName).ToList();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                FailWindow.Display("Erorr accessing Active Directory:" + Environment.NewLine + ex.Message);
            }

            return userList;
        }

        public override void Init()
        {
            IsActive = true;
            Users.Clear();
            var tmplist = GetSchedwinADUsers();
            tmplist.AddRange(GetWishAgents());

            var tmplst2 = tmplist.Select(x=>(User)x).OrderBy(x => x.FullName).ToList();
            Users.AddRange(tmplst2);
        
            ListUserTypes.Clear();
            ListUserTypes.AddRange(UserType.GetUserTypes());
            var User = new User();       
            User.CreateDefaultModulePermissions();
            ModulePermissions.AddRange(User.ModulePermissions);


            ListAgencies.AddRange(Company.GetCompanyList());

            NotifyPropertyChanged("ListAgencies");
            NotifyPropertyChanged("ListUserTypes");
            NotifyPropertyChanged("ModulePermissions");
            NotifyPropertyChanged("Users");
            NotifyPropertyChanged("IsActive");


        }


        
        public void Refresh(User user)
        {
            ModulePermissions.Clear();
            Users.Clear();
            Users.AddRange(GetSchedwinADUsers());
            ListUserTypes.Clear();
            ListUserTypes.AddRange(UserType.GetUserTypes());
            ListAgencies.AddRange(Company.GetCompanyList());

            SelectedUser = Users.FirstOrDefault(x => x.Username == user.Username);
            if (ListAgencies!=null)
                SelectedAgency = ListAgencies.First(x => x.IDX == user.IDX_Company);

            IsActive = user.Active;
            IsNew = false;
            IDX = user.IDX;
            ModulePermissions.AddRange(user.ModulePermissions);
            SelectedUserType = ListUserTypes.FirstOrDefault(x => x.IDX == user.IDX_UserType);

            NotifyPropertyChanged("SelectedAgency");
            NotifyPropertyChanged("SelectedUserType");
            NotifyPropertyChanged("ListUserTypes");
            NotifyPropertyChanged("ModulePermissions");
            NotifyPropertyChanged("Users");
            NotifyPropertyChanged("IsActive");
        }
    }
}
