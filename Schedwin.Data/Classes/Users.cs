using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class User
    {
        public bool IsUser { get; set; }
        public bool IsNew { get; set; }
        public int IDX { get; set; }

        public int IDX_Company { get; set; }

        public int IDX_UserType { get; set; }

        public String FirstName { get; set; }

        public String Surname { get; set; }

        public String FullName
        {
            get
            {
                return FirstName + " " + Surname;
            }
        }

        public String Username { get; set; }

        public String Email { get; set; }

        public bool Active { get; set; }

        public List<ModulePermission> ModulePermissions { get; set; }



        private static List<User> _userCacheList;


        public void CreateDefaultModulePermissions()
        {
            ModulePermissions.Add(new ModulePermission { ModuleName = "Integration", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Invoicing", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Prep", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Reports", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Reservations", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Scheduling", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Setup", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Techlogs", CanView = true });
            ModulePermissions.Add(new ModulePermission { ModuleName = "Tracking", CanView = true });
        }

        public User()
        {
            IsUser = true;
            IsNew = true;
            IDX_Company = 1;
            ModulePermissions = new List<ModulePermission>();
        }

        public static async Task DeactivateUser(int IDX, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetUserItem = await ctx.tset_Personnel.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tsetUserItem != null)
                    tsetUserItem.Active = false;
                await ctx.SaveChangesAsync();


            }
        }


        public static void UpdateCachedObject(User newUserObj)
        {
            if (_userCacheList != null)
            {
                var oldObject = _userCacheList.FirstOrDefault(x => x.IDX == newUserObj.IDX);
                if (oldObject != null)
                    _userCacheList.Remove(oldObject);

                _userCacheList.Add(newUserObj);
            }

        }

        public async Task<bool> Save(String Server, string regionalDBName)
        {
            tset_Personnel tsetPersonnel = null;
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                if (!IsNew)
                {
                    tsetPersonnel = ctx.tset_Personnel.Include("tset_ModulePermissions").FirstOrDefault(x => x.IDX == IDX);
                    tsetPersonnel.Firstname = FirstName;
                    tsetPersonnel.Surname = Surname;
                    tsetPersonnel.EMail = Email;
                    tsetPersonnel.Username = Username;

                    ctx.tset_ModulePermissions.RemoveRange(tsetPersonnel.tset_ModulePermissions);
                    tsetPersonnel.tset_ModulePermissions.Clear();
                    foreach (var modulePermisson in ModulePermissions)
                    {
                        var tsetPermission = (tset_ModulePermissions)modulePermisson;
                        tsetPersonnel.tset_ModulePermissions.Add(tsetPermission);
                    }

                    ctx.tset_ModulePermissions.AddRange(tsetPersonnel.tset_ModulePermissions);
                }
                else
                {
                    tsetPersonnel = (tset_Personnel)this;
                    //tsetPersonnel.Username = "chrisg2";
                    ctx.tset_Personnel.Add(tsetPersonnel);
                    ctx.tset_ModulePermissions.AddRange(tsetPersonnel.tset_ModulePermissions);


                }


                await ctx.SaveChangesAsync();
                IDX = tsetPersonnel.IDX;

            }


            return true;
        }

        public static explicit operator User(UserPrincipal userPrincipal)
        {
            var user = new User();
            user.IsNew = true;
            user.FirstName = userPrincipal.GivenName;
            user.Surname = userPrincipal.Surname;
            user.Username = userPrincipal.SamAccountName;
            user.Email = userPrincipal.EmailAddress;


            return user;
        }

        public static explicit operator tset_Personnel(User user)
        {
            var tsetPersonnel = new tset_Personnel();
            tsetPersonnel.IDX = user.IDX;
            tsetPersonnel.EMail = user.Email;
            tsetPersonnel.Firstname = user.FirstName;
            tsetPersonnel.Surname = user.Surname;
            tsetPersonnel.Username = user.Username;
            tsetPersonnel.Telephone = "";
            tsetPersonnel.DateOfBirth = new DateTime(2000, 01, 01);
            tsetPersonnel.Fax = "";
            tsetPersonnel.IDX_PersonnelType = user.IDX_UserType;
            tsetPersonnel.IDX_Company = user.IDX_Company;
            tsetPersonnel.WebPassword = "";
            tsetPersonnel.Email2 = "";
            tsetPersonnel.Active = user.Active;
            tsetPersonnel.IsUser = user.IsUser;
            var tsetPermissions = user.ModulePermissions.Select(x => (tset_ModulePermissions)x).ToList();
            foreach (var tsetPermission in tsetPermissions)
            {
                tsetPersonnel.tset_ModulePermissions.Add(tsetPermission);

            }

            return tsetPersonnel;
        }


        public static explicit operator User (tbUser tbUser)
        {
            var user = new User();
            user.IsNew = false;
            user.IDX = tbUser.pkUserID;
            user.FirstName = tbUser.Firstname;
            user.Surname = tbUser.Surname;
            user.Username = tbUser.username;
            user.Email = tbUser.Email;
            user.Active = tbUser.Active;
        
            //if (tbUser.tset_Companies != null)
            //    user.IDX_Company = tsetUser.IDX_Company;
            if (tbUser.tbModulePermissions != null && (tbUser.tbModulePermissions.Count > 0))
                user.ModulePermissions = tbUser.tbModulePermissions.Select(x => (ModulePermission)x).ToList();
            else
                user.CreateDefaultModulePermissions();
           
            return user;
        }

        public static explicit operator User(tset_Personnel tsetUser)
        {
            var user = new User();
            user.IsNew = false;
            user.IDX = tsetUser.IDX;
            user.FirstName = tsetUser.Firstname;
            user.Surname = tsetUser.Surname;
            user.Username = tsetUser.Username;
            user.Email = tsetUser.EMail;
            user.IDX_UserType = tsetUser.IDX_PersonnelType;
            user.Active = tsetUser.Active ?? false;
            user.IsUser = tsetUser.IsUser ?? false;
            if (tsetUser.tset_Companies != null)
                user.IDX_Company = tsetUser.IDX_Company;
            if (tsetUser.tset_ModulePermissions != null && tsetUser.tset_ModulePermissions.Count > 0)
                user.ModulePermissions = tsetUser.tset_ModulePermissions.Select(x => (ModulePermission)x).ToList();
            else
                user.CreateDefaultModulePermissions();
            
            return user;
        }
        
        public static List<User> GetUserList()
        {


            if ( _userCacheList != null)
                return _userCacheList.Where(x => x.Active).OrderBy(x=>x.FullName).ToList();

            return _userCacheList;
        }

        static public async Task<List<User>> GetAgentUsersForCompany(int idxCompany, String Server, String dbInstance)
        {
            var conString = RegionalConnectionGenerator.GetConnectionString(Server, dbInstance);
            var ctx = new SchedwinRegionalEntities(conString);

            using (ctx)
            {
                var tmpdbLst = await ctx.tset_Personnel.Where(x => x.tset_Companies.IDX == idxCompany).ToListAsync();
                var tmpUserList = tmpdbLst.Select(x => (User)x).ToList();
                return tmpUserList;


            }
        }


        static public async Task DeleteAgentUser(int userIDX, String Server, string regionalDBName)
        {
            var conString = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(conString);

            using (ctx)
            {
                var tmpUser = new tset_Personnel();
                tmpUser.IDX = userIDX;
                ctx.tset_Personnel.Attach(tmpUser);
                ctx.tset_Personnel.Remove(tmpUser);
                await ctx.SaveChangesAsync();
            }
        }

        static public async Task<List<User>> LoadUserList(bool bForceReload)
        {
            if (_userCacheList != null && !bForceReload)
                return _userCacheList;
            else
            {
             
                var ctx = new SchedwinGlobalEntities();

                using (ctx)
                {
                    var tmpUserLst = await ctx.tbUsers.Include("tbModulePermissions")
                                           .Where(x => x.Active).ToListAsync();
                    _userCacheList = tmpUserLst.Select(x => (User)x).OrderBy(x => x.FullName).ToList();

                }

                return _userCacheList;
            }
        }

        static public async Task<List<User>> LoadUserList(String Server, string regionalDBName, bool bForceReload)
        {
            if (_userCacheList != null && !bForceReload)
                return _userCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var tmpUserLst = await ctx.tset_Personnel.Include("tset_ModulePermissions").Include("tset_Companies")
                                           .Where(x => x.Active.HasValue && x.Active.Value && x.IsUser.HasValue && x.IsUser==true).ToListAsync();
                    _userCacheList= tmpUserLst.Select(x => (User)x).OrderBy(x=>x.FullName).ToList();

                }

                return _userCacheList;
            }

        }
    }
}
