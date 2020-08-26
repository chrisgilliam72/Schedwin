using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Common;
using Schedwin.Data.Classes;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Navigation;
using Telerik.Windows;
using System.Windows;
using Schedwin.Setup;
using Schedwin.Scheduling;
using Schedwin.Reporting.Crystal;
using Schedwin.Data;

namespace SchedwinWPF
{
    public class SchedwinFrameWindowViewModel : Schedwin.Common.ViewModelBase
    {
        private String _StatusText;
        public String StatusText
        {
            get
            {
                return _StatusText;
            }
            set
            {
                _StatusText = value;
            }
        }
        private System.Threading.Timer _timer { get; set; }
        private System.Threading.Timer _canContinueTimer { get; set; }

        public ObservableCollection<RadMenuItem> WindowNameList
        {
            get
            {
                var windowNameList = new ObservableCollection<RadMenuItem>();
                var windowNames = GetWindowMenuList();
                foreach (var windowName in windowNames)
                {
                    var menuItem = new RadMenuItem();
                    menuItem.Header = windowName;
                    menuItem.Click += WindowItem_Click;
                    windowNameList.Add(menuItem);
                }


                return windowNameList;
            }
        }


        public static  new bool  UseGlobalDB
        {
            get
            {

                return (Database == "Schedwin_Global");
            }

        }

        public static String Region { get; set; }
        public static new String Database { get; set; }
        public static new  String Server { get; set; }
        public static int CurrentUserID { get; set; }
        public static String CurrentUserName { get; set; }

        public static int CountryID { get;set;}
        public static int CompanyID { get; set; }

        public static int PrincipalID { get; set; }

        public static int SoleUsePrincipalID { get; set; }
        public  List<ModulePermission> UserPermissions { get; set; }

        public List<Form> ChildFormList { get; set; }
        public List<SchedwinBaseWindow> ChildWindowList { get; set; }


        public Control DummyParentControl { get; set; }
        public SchedwinFrameWindowView View { get; set; }
        public IMainWindowController WndCntrler { get; set; }


        private String _title;
        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public bool CanViewIntegration
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Integration");
                if (modPerm!=null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }

        public bool CanViewReservations
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Reservations");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }

        public bool CanViewScheduling
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Scheduling");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }

        public bool CanViewSetup
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Setup");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }

        public bool CanViewInvoicing
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Invoicing");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }


        public bool CanViewPrep
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Prep");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }

        public bool CanViewReports
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Reports");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }



        public bool CanViewTechlogs
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Techlogs");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }


        public bool CanViewFlightFollowing
        {
            get
            {
                var modPerm = UserPermissions.FirstOrDefault(x => x.ModuleName == "Tracking");
                if (modPerm != null)
                {
                    return modPerm.CanModify;
                }
                return false;
            }
        }

        public void RefreshMenus()
        {
            NotifyPropertyChanged("CanViewIntegration");
            NotifyPropertyChanged("CanViewReservations");
            NotifyPropertyChanged("CanViewScheduling");
            NotifyPropertyChanged("CanViewSetup");
            NotifyPropertyChanged("CanViewInvoicing");
            NotifyPropertyChanged("CanViewPrep");
            NotifyPropertyChanged("CanViewReports");
            NotifyPropertyChanged("CanViewTechlogs");
            NotifyPropertyChanged("CanViewFlightFollowing");
        }
        public SchedwinFrameWindowViewModel()
        {
            UserPermissions = new List<ModulePermission>();
             ChildWindowList = new List<SchedwinBaseWindow>();
            ChildFormList =  new List<Form>();
               _timer = new System.Threading.Timer(TimerTick, this, 0, 1000);
            _canContinueTimer = new System.Threading.Timer(CanContinueWorking_Tick, this, 0, 60000);
        }

        public async Task<bool> Init()
        {
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                UpdateStatus("Loading cache");
                try
                {
                    ShowPilotExpiryReport();
                    if (SchedwinFrameWindowViewModel.UseGlobalDB)
                        await DataCache.Init();
                    else
                        await DataCache.Init(Server, Database);              
                }
                catch (Exception ex)
                {
                    var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                    var message = string.Join(Environment.NewLine, messages);
                   FailWindow.Display("Failed to load cache :"+Environment.NewLine+ message);
                    return false;
                    
                }
                UpdateStatus("Online : "+Server+" "+Database);
                return true;
            }
        }

        private  void TimerTick(object ViewModel)
        {
            var schedwinFrameVM = ViewModel as SchedwinFrameWindowViewModel;
            var globalEntites = new SchedwinGlobalEntities();
            using (globalEntites)
            {

            }
        }
        private void CanContinueWorking_Tick(object ViewModel)
        {
            var schedwinFrameVM = ViewModel as SchedwinFrameWindowViewModel;
            RefreshWindowMenu();
        }

        public void ShowPilotExpiryReport()
        {
            if (System.Windows.MessageBox.Show("Do you want to view the pilot expiration dates report now ?","Pilot expiration dates.", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var reporlib = new SchedwinReportLibrary();
                reporlib.SchedRegion = Region;

                reporlib.PilotExpiryreport(Server, Database);
            }

        }
        public void UpdateStatus(String newStatusText)
        {
            StatusText = newStatusText;
            NotifyPropertyChanged("StatusText");
        }
        public void AddWindowName(String WindowName)
        {

            RadMenuItem menuItem = new RadMenuItem();
            menuItem.Click += WindowItem_Click;
            menuItem.Header = WindowName;
            WindowNameList.Add(menuItem);
            NotifyPropertyChanged("WindowNameList");
        }


        public void AddForm(Form childForm, int frmCount)
        {
            if (childForm!=null)
            {
                String frmName = childForm.Text;

                if (frmCount > 0)
                    frmName = childForm.Text + "[" + frmCount.ToString() + "]";

                AddWindowName(frmName);
                ChildFormList.Add(childForm);
            }

        }

        public void AddWindow(String WindowName, SchedwinBaseWindow childWnd)
        {
            
            //AddWindowName(WindowName);
            if (ChildWindowList.FirstOrDefault(x=>x.Title== WindowName)==null)
                ChildWindowList.Add(childWnd);
        }


        SchedwinBaseWindow FindChildWindow(String Name)
        {
            var window = ChildWindowList.FirstOrDefault(x => x.Title == Name);
            return window;
        }


    

        Form FindChildForm(String Name)
        {
            var frm = ChildFormList.FirstOrDefault(x => x.Text == Name);
            return frm;
        }


        private void ActivateForm(String frmName)
        {
            var frm = FindChildForm(frmName);
            if (frm != null)
            {
                frm.BringToFront();
                frm.WindowState = FormWindowState.Maximized;
            }
        }


        private void WindowItem_Click(object sender, RadRoutedEventArgs e)
        {
            var menuItem = e.Source as RadMenuItem;
            if (menuItem!=null)
            {
                var header = menuItem.Header as String;
                var frm = FindChildForm(header);
                if (frm != null)
                {
                    frm.BringToFront();
                    frm.WindowState=FormWindowState.Maximized;
                }
                else 
                {
                    var wnd = FindChildWindow(header);
                    if (wnd!=null)
                    {
                        wnd.Activate();
                        wnd.WindowState = WindowState.Maximized;
                    }
                } 
                   
            }
            
        }   

        public void RefreshWindowMenu()
        {
            NotifyPropertyChanged("WindowNameList");
        }


 

        public void ShowNewReservationsWindow()
        {

            if (WndCntrler != null)
            {
                int wndCnt = WindowNameList.Where(x => ((String)x.Header).Contains("Reservation List")).Count();
                
                var wnd = WndCntrler.ShowNewReservations(View);
               AddWindow("Reservation List", wnd);
              
            }

        }

        public void WishIntegration()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Wish Integration") == null)
                {
                   var wnd= WndCntrler.ShowWishIntegration(View);
                    AddWindow("Wish Integration", wnd); 
                }
                else
                {
                    var wnd = FindChildWindow("Wish Integration");
                    if (wnd != null)
                    {
                        wnd.WindowState = WindowState.Maximized;
                    }
                }
            }
           
        }

        public void GPInvoicing()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "GPInvoicing") == null)
                {
                    var wnd=WndCntrler.ShowGPInvoicing(DummyParentControl);
                    AddForm(wnd,0);
                }
                else
                {
                    var frm = FindChildForm("GPInvoicing");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }

        }

        public void GPInvoicingNew()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "GP Invoicing New") == null)
                {
                    var wnd = WndCntrler.ShowNewGPInvoicing(View);
                    AddWindow("GP Invoicing New", wnd);
                }
                else
                {
                    var Wnd = FindChildWindow("GP Invoicing New");
                    if (Wnd != null)
                    {
                        //Wnd.BringIntoView();
                        Wnd.WindowState = WindowState.Maximized;
                    }
                }
            }

        }

        public void PilotRoster()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Pilot Roster") == null)
                {
                    var wnd=WndCntrler.ShowPilotRoster(View);
                    AddWindow("Pilot Roster", wnd);
                }
                else
                {
                    var Wnd = FindChildWindow("Pilot Roster");
                    if (Wnd != null)
                    {
                        //Wnd.BringIntoView();
                        Wnd.WindowState = WindowState.Maximized;
                    }
                }
            }
        }

        public void ShowAirstripExForWindow()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Airstrip Ex/For") == null)
                {
                    var wnd = WndCntrler.ShowExForSetup(View);
                    AddWindow("Airstrip Ex/For", wnd);
                }
                else
                {
                    var Wnd = FindChildWindow("Airstrip Ex/For");
                    if (Wnd != null)
                    {
                        //Wnd.BringIntoView();
                        Wnd.WindowState = WindowState.Maximized;
                    }
                }
            }

        }


        public void LockSchedules()
        {
            if (WndCntrler != null)
            {
                WndCntrler.ShowUnlockSchedules(this.View);
            }
               
        }

   

        public void UpdateOldSchedule()
        {
            if (WndCntrler != null)
                WndCntrler.ShowUpdateOldSchedule(DummyParentControl);
        }

        public void WeightBalance()
        { 
        
            if (WndCntrler != null)
            {

                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Weight Balance") == null)
                {
                    var wnd = WndCntrler.ShowWeightBalance(View);
                    AddWindow("Weight Balance", wnd);
                }
                else
                {
                    var Wnd = FindChildWindow("Weight Balance");
                    if (Wnd != null)
                    {
                        //Wnd.BringIntoView();
                        Wnd.WindowState = WindowState.Maximized;
                    }
                }

            }

        }

        public void AircraftPrep()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Aircraft Prep") == null)
                {
                    var frm=WndCntrler.ShowAircraftPrep(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Aircraft Prep");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }

            }
        }

        public void TicketsNew()
        {

            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Tickets New") == null)
                {
                    var wnd = WndCntrler.ShowTicketsNew(View);
                    AddWindow("Tickets Newr", wnd);
                }
                else
                {
                    var Wnd = FindChildWindow("Tickets New");
                    if (Wnd != null)
                    {
                        //Wnd.BringIntoView();
                        Wnd.WindowState = WindowState.Maximized;
                    }
                }
            }

        }

        public void Tickets()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Tickets") == null)
                {
                    var frm=WndCntrler.ShowTickets(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Tickets");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }

        }

        public void TicketHistory()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Ticket History") == null)
                {
                    var frm=WndCntrler.ShowTicketHistory(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Ticket History");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }

        }

        public void BaggageTags()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Baggage Tags") == null)
                {
                    var frm=WndCntrler.ShowBaggageTags(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Baggage Tags");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }

        }
        public void IndigoTrack()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Indigo Tracking") == null)
                {
                    var frm = WndCntrler.ShowIndigoTrack(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Indigo Tracking");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }

        }

        public void IndigoTrackNew()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Indigo Tracking") == null)
                {
                    var wnd = WndCntrler.ShowNewIndigoTrack(View);
                    AddWindow("Tracking Times", wnd);
                }
                else
                {
                    var Wnd = FindChildWindow("Indigo Tracking");
                    if (Wnd != null)
                    {
                        //Wnd.BringIntoView();
                        Wnd.WindowState = WindowState.Maximized;
                    }
                }
            }

        }


        public void TrackingTimes()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Tracking Times") == null)
                {
                    var frm = WndCntrler.ShowTrackingTimes(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Tracking Times");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }

        }

        public void ShowNewSchedule()
        {

            if (WndCntrler != null)
            {

                var wnd = WndCntrler.ShowNewScheduling(View);
                AddWindow("Schedule", wnd);     
            }
        }

        public void NewTechLog()
        {
            if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Tech log register") == null)
            {
                var wnd = WndCntrler.ShowNewTechLogs(View);
                AddWindow("Tech log register", wnd);
            }
            else
            {
                var Wnd = FindChildWindow("Tech log register");
                if (Wnd != null)
                {
                    //Wnd.BringIntoView();
                    Wnd.WindowState = WindowState.Maximized;
                }
            }
        }


     

        public void ShowNewSetup()
        {
            var newsetupView = new NewSetupView();
            var viewModel = newsetupView.DataContext as NewSetupViewModel;
            viewModel.Database = Database;
            viewModel.Server = Server;
            newsetupView.Show();
        }

        public void Setup()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Data Setup") == null)
                {
                    var wnd=WndCntrler.ShowSetup(View);
                    AddWindow("Data Setup", wnd);
                }
                else
                {
                    var wnd = FindChildWindow("Data Setup");
                    if (wnd != null)
                    {
                        wnd.WindowState = WindowState.Maximized;
                    }
                }
            }

        }

        public void GPSetup()
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "GP Setup") == null)
                {
                    var frm = WndCntrler.ShowGPSetup(DummyParentControl);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("GP Setup");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }
  
        }

        public void Reporting(String reportName)
        {
            if (WndCntrler != null)
            {
                if (WindowNameList.FirstOrDefault(x => (String)x.Header == "Reporting") == null)
                {
                    var frm = WndCntrler.ShowReporting(DummyParentControl,View, reportName);
                    AddForm(frm,0);
                }
                else
                {
                    var frm = FindChildForm("Reporting");
                    if (frm != null)
                    {
                        frm.BringToFront();
                    }
                }
            }
               
        }

    

        public List<String> GetWindowMenuList()
        {
            var windowTitleLst = new List<String>();
            foreach (var window in ChildWindowList)
            {
                if (!window.IsClosed)
                    windowTitleLst.Add(window.Title);
            }

            foreach (var form in ChildFormList)
            {
                if (form != null && !form.IsDisposed)
                    windowTitleLst.Add(form.Text);
            }

            return windowTitleLst;
        }

     
    
    }
}
