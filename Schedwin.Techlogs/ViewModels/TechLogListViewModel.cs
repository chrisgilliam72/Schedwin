using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Techlogs
{
    public class TechLogListViewModel :ViewModelBase
    {
        public bool CanViewHistory
        {
            get
            {
                return SelectedAircraft != null;
            }
        }

        public TechLogListView View { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        private String _selectedAircraft;
        public String SelectedAircraft
        {
            get
            {
                return _selectedAircraft;
            }
            set
            {
                _selectedAircraft = value;
                NotifyPropertyChanged("CanViewHistory");
            }
        }

        public RangeObservableCollection<String> AircraftList { get; set; }

        public RangeObservableCollection<Techlog> TechLogList { get; set; }

        public Techlog SelectedTechlog { get; set; }


        public TechLogListViewModel()
        {
            AircraftList = new RangeObservableCollection<string>();
            TechLogList = new RangeObservableCollection<Techlog>();

            EndDate = DateTime.Today;
            StartDate = EndDate.AddDays(-60);
        }

        public async Task<bool> Refresh()
        {
            try
            {
                TechLogList.Clear();

                var aircraftInfo = AircraftInfo.GetAircraftInfo(SelectedAircraft);
                var techlogList = await Techlog.GetTechLogs(StartDate, EndDate, aircraftInfo.IDX, Server, Database);
                TechLogList.AddRange(techlogList.OrderByDescending(x => x.TechLogID).ToList());

                NotifyPropertyChanged("TechLogList");
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

            return true;
        }
        public async Task<bool> Init()
        {
            AircraftList.Clear();
            using (new StackedCursorOverride(Cursors.Wait))
            {
                var tmpList = await AircraftInfo.LoadAircraftList(Server, Database,false);
                AircraftList.AddRange(tmpList.Where(x => x.OwnAircraft).OrderBy(x => x.Registration).Select(x => x.Registration).ToList());
            }


            NotifyPropertyChanged("AircraftList");
            NotifyPropertyChanged("StartDate");
            NotifyPropertyChanged("EndDate");

            return true;
        }

        public async void EditServiceHistory()
        { 
            try
            {
                if (SelectedAircraft != null)
                {
                    var techLogServiceHistoryView = new TechLogServiceHistoryView();
                    var serviceHistoryViewModel = techLogServiceHistoryView.DataContext as TechLogServiceHistoryViewModel;
                    serviceHistoryViewModel.ListTechlogs=this.TechLogList;
                    techLogServiceHistoryView.Owner = View;

                    var aircraftInfo = AircraftInfo.GetAircraftInfo(SelectedAircraft);
                    if (aircraftInfo != null)
                    {
                        var techlogHistory = await TechLogService.GetTechlogServiceHistory(aircraftInfo.IDX, Server, Database);
                        serviceHistoryViewModel.IDX_Aircraft = aircraftInfo.IDX;
                        serviceHistoryViewModel.Database = Database;
                        serviceHistoryViewModel.Server = Server;
                        if (techlogHistory != null)
                        {
                            serviceHistoryViewModel.Refresh(techlogHistory);
                            techLogServiceHistoryView.ShowDialog();
                        }
                        else
                            FailWindow.Display("No techlog history available");
                    }
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

        }

        public async void EditTechLog()
        {
            if (SelectedTechlog!=null)
            {
                var techlogCaptureView = new TechLogCaptureView();
                var techlogViewModel = techlogCaptureView.DataContext as TechlogCaptureViewModel;
                techlogViewModel.Server = Server;
                techlogViewModel.Database = Database;

                techlogCaptureView.Owner = View;
                techlogViewModel.Refresh(SelectedTechlog);
                var dlgResult = techlogCaptureView.ShowDialog();

                if (dlgResult.HasValue && dlgResult.Value)
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        await Refresh();
                    }
                }
            }

        }

        public  void DeleteTechLog()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Tech log ";
            parameters.Content = "Are you sure want to delete this Tech Log Entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnDeleteTechLogConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }

        public async void AddTechLog()
        {
            var techlogCaptureView = new TechLogCaptureView();
            var aircraftInfo = AircraftInfo.GetAircraftInfo(SelectedAircraft);

            var techlogViewModel = techlogCaptureView.DataContext as TechlogCaptureViewModel;
            techlogViewModel.Server = Server;
            techlogViewModel.Database = Database;
            techlogCaptureView.Owner = View;

            if (TechLogList.Count > 0)
            {
                var lastTechLogID = TechLogList.Max(x => x.TechLogID);
                var lastTachEnd = TechLogList.Max(x => x.TachEnd);
                var lastTechLogDate = TechLogList.Max(x => x.TechLogDate);


                techlogViewModel.Init(lastTechLogDate, lastTechLogID, lastTachEnd, aircraftInfo.IDX);
            }
            else
                techlogViewModel.Init(null, 1, 0, aircraftInfo.IDX);

            var dlgResult = techlogCaptureView.ShowDialog();

            if (dlgResult.HasValue && dlgResult.Value)
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await Refresh();
                }
            }
        }

        private async void OnDeleteTechLogConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedTechlog != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {
                            await Techlog.DeleteTechLog(SelectedTechlog.TechLogID, Server, Database);
                            await Refresh();
                        }
                    }
                }
            }
    
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

        }
    }
}
