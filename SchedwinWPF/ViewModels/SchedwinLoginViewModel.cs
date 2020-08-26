using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System.DirectoryServices.AccountManagement;
using TelerikControls=Telerik.Windows.Controls;
using System.Reflection;
using System.Data.Entity;

namespace SchedwinWPF
{
   public class SchedwinLoginViewModel : ViewModelBase
    {


        private bool _showIncorrectPass;
        public bool ShowIncorrectPassword
        {
            get
            {
                return _showIncorrectPass;
            }
            set
            {
                _showIncorrectPass = value;
                NotifyPropertyChanged("ShowIncorrectPassword");
            }
        }
        public Guid UserADGuid { get; set; }
        public String Title { get; set; }

        private String _selectedRegion;
        public String SelectedRegion
        {
            get
            {
                return _selectedRegion;
            }
            set
            {
                _selectedRegion = value;
                NotifyPropertyChanged("SelectedRegion");
            }
        }

        public String Username { get; set; }

        public String Password { get; set; }

        public int RegionalDB_IDX { get; set; }

        public tbDBRegionInfo RegoionalInfo { get; set; }

        public RangeObservableCollection<String> Regions { get; set; }

        public List<ModulePermission> UserPermissions { get; set; }



        private bool _useUseCurrentAD;

        public bool UseCurrentAD
        {
            get
            {

                return _useUseCurrentAD;

            }
            set
            {
                _useUseCurrentAD = value;
                NotifyPropertyChanged("EnablePassword");
                NotifyPropertyChanged("UseCurrentAD");
            }

        }

        public bool EnablePassword
        {
            set
            {

            }
            get
            {
                return !UseCurrentAD;
            }
        }

        public SchedwinLoginViewModel() 
        {
            Regions = new RangeObservableCollection<string>();
            ShowIncorrectPassword = false;
        }

        private bool ValidateADCredentials()
        {
            bool isValid = false;

            if (String.IsNullOrEmpty(Username))
            {
                FailWindow.Display("Username can not be empty");
                return false;
            }

        
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "wilderness.co.za"))
            {


                UserPrincipal user = UserPrincipal.FindByIdentity(pc, IdentityType.SamAccountName, Username);
                if (user != null)
                {
                    bool accountLocked = user.IsAccountLockedOut();

                    if (accountLocked)
                    {
                        FailWindow.Display("Your domain account is currently locked out.\r\n Please contact support@wilderness.co.za to have your account unlocked");
                        return false;
                    }
                }

                UserADGuid = user.Guid.Value;
                System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (UseCurrentAD && currentUser.IsAuthenticated)
                    return true;
                else
                 isValid = pc.ValidateCredentials(Username, Password);
        
            }

            if (!isValid)
                //FailWindow.Display("Username or password is incorrect");
                ShowIncorrectPassword = true;
        

            return isValid;
        }

        public  async Task<bool> ValidateRegionalAccess()
        {
            //Roles.Clear();

            var regionalUsers = new RegionalUsers();

            RegionalDB_IDX =  await regionalUsers.GetRegionalIDX(Username, RegoionalInfo.Server, RegoionalInfo.Database);
            if (RegionalDB_IDX == -1)
            {
                FailWindow.Display("You do not have a valid user ID for database: \r\n" + RegoionalInfo.Server + "\\" + RegoionalInfo.Database);
                return false;
            }

            if (RegionalDB_IDX == -999)
            {
                FailWindow.Display("There was an error connecting to regional database :\r\n" + RegoionalInfo.Server + "\\" + RegoionalInfo.Database + "\r\n" + regionalUsers.LastError);
                return false;
            }
           

            //var tbRoles = await cnf.GetRoles(Username, RegoionalInfo.Region);
            //if (tbRoles.Count < 1)
            //{
            //    TelerikControls.RadWindow.Alert("You do not have a valid role for this region");
            //    return false;
            //}
            //Roles.AddRange(tbRoles.Select(x => x.RoleName).ToList());
            return true;
        }

        public async void LogLogon()
        {
            var globalDB = new SchedwinGlobalEntities();
            using (globalDB)
            {
                var logonDetails = new tbUserLogonDetail();
                logonDetails.Username = Username;
                logonDetails.Version =  Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
                logonDetails.Version += "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
                logonDetails.build = Assembly.GetExecutingAssembly().GetName().Version.Build.ToString(); ;
                logonDetails.host = System.Environment.MachineName;
                logonDetails.LogonTime = DateTime.Now;
                globalDB.tbUserLogonDetails.Add(logonDetails);
                await  globalDB.SaveChangesAsync();
            }
        }


        public async void SaveLastSelectedRegion()
        {
            var congfig = new GlobalConfig();
            await congfig.SaveSettings(Username,UserADGuid, SelectedRegion);
        }

        public void ShowMainWindow()
        {
            var mainWindow = new SchedwinFrameWindowView();
            var mainViewModel = mainWindow.DataContext as SchedwinFrameWindowViewModel;
            var wndController = new SchedwinWindowController();

            mainViewModel.WndCntrler = wndController;

            SchedwinFrameWindowViewModel.Database = RegoionalInfo.Database;
            SchedwinFrameWindowViewModel.Server = RegoionalInfo.Server;
            SchedwinFrameWindowViewModel.Region = RegoionalInfo.Region;
            SchedwinFrameWindowViewModel.CurrentUserID = RegionalDB_IDX;
            SchedwinFrameWindowViewModel.CurrentUserName = Username;
            SchedwinFrameWindowViewModel.PrincipalID = RegoionalInfo.PrincipalID.Value;
            SchedwinFrameWindowViewModel.SoleUsePrincipalID = RegoionalInfo.SoleUsePrincipalID ?? 0;
            SchedwinFrameWindowViewModel.CountryID = RegoionalInfo.DefaultCountryID.Value;
            if (SelectedRegion== "ZZZ_Global")
            {
                SchedwinFrameWindowViewModel.CurrentUserID = UserPermissions.First().IDX_User;
                SchedwinFrameWindowViewModel.Region = "Botswana";

            }
            else
            {
                SchedwinFrameWindowViewModel.CompanyID = RegoionalInfo.GPCompanyID.Value;

            }
     


            mainViewModel.UserPermissions = UserPermissions;
            mainViewModel.Title = Title;
            mainViewModel.RefreshMenus();
            mainWindow.Show();

   
        }

        public async Task<bool> CanContinueUsing(bool loggingOn)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
               var logInInfo= await ctx.tbCanLogIns.FirstOrDefaultAsync();
                if (logInInfo!=null && !logInInfo.CanLogIn)
                {
                    FailWindow.Display("Schedwin can not be used at this time :" + Environment.NewLine + logInInfo.ShutdownReason);
                    return false;
                }

            }

            return true;
        }

        public async Task<bool> IsVersionAllowed()
        {
            try
            {

                var versionString = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
                versionString += "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
                int currentBuild = Assembly.GetExecutingAssembly().GetName().Version.Build;
                Double currentVersion = Convert.ToDouble(versionString);

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var ctx = new SchedwinGlobalEntities();
                    using (ctx)
                    {
                        var minVersion = await ctx.tbMininumVersionBuilds.FirstAsync();
                        if (currentVersion < minVersion.Lowest_Version)
                        {
                            FailWindow.Display("This version of Schedwin is no longer supported. Please ensure you are using version " + minVersion.Lowest_Version + " or higher");
                            return false;
                        }
                        if (currentBuild< minVersion.Lowest_Build)
                        {
                            FailWindow.Display("Incompatible build. Please ensure you are using build " + minVersion.Lowest_Build + " or higher");
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errMess = string.Join(Environment.NewLine, messages);
                FailWindow.Display(errMess);
                return false;
            }
        }

        public  async Task<bool> Logon()
        {
            try
            {
                bool canLogin = false;

     
                if (String.IsNullOrEmpty(SelectedRegion))
                {
                    TelerikControls.RadWindow.Alert("Please select a region");
                    return false;

                }
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    if (ValidateADCredentials())
                    {
                        var cnf = new GlobalConfig();
                        RegoionalInfo = await cnf.GetRegionInfo(SelectedRegion);
                        if (SelectedRegion!= "ZZZ_Global")
                        {
                            canLogin = await ValidateRegionalAccess();
                            if (canLogin)
                            {

                                UserPermissions  = await ModulePermission.GetModulePermissions(RegionalDB_IDX, RegoionalInfo.Server, RegoionalInfo.Database);
                    

                            }
                        }
                        else
                        {

                            UserPermissions = await ModulePermission.GetModulePermissions(Username);
                            canLogin = true;
                        }
                    }
                    else
                        return false;


                }


                return canLogin;
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errMess = string.Join(Environment.NewLine, messages);
                FailWindow.Display(errMess);
                return false;
            }
 
        }

        public async void LoadLastRegion()
        {
            var config = new GlobalConfig();
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                SelectedRegion = await config.GetLastRegion(Username);
            }
        }

        public async void RefreshRegions()
        {
            try
            {
                if (Regions.Count==0)
                {
                    var config = new GlobalConfig();
                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                        var regions = await config.GetRegions();
                        Regions.AddRange(regions);
                        NotifyPropertyChanged("Regions");
                    }
                }
    
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errMess = string.Join(Environment.NewLine, messages);
                FailWindow.Display(errMess);
            }
        }

        public void Init()
        {
            
            try
            {
                Title = "Schedwin ";
                Title += Assembly.GetExecutingAssembly().GetName().Version.Major.ToString();
                Title += "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString();
                Title += " ( Build ";
                Title += Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
                Title += ")";



                Username = Environment.UserName.Substring(0, 1) + (Environment.UserName.Substring(1, Environment.UserName.Length - 1));
                Username = Username.ToLower();
                NotifyPropertyChanged("Title");
                NotifyPropertyChanged("Username");

                UseCurrentAD = true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errMess = string.Join(Environment.NewLine, messages);
                FailWindow.Display(errMess);
            }

        }

    }
}
