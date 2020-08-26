using Schedwin.Common;
using Schedwin.Reservations.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using System.Windows.Media;
using System.Windows.Input;
using Schedwin.Common.Classes;
using Schedwin.Reporting.Crystal;

namespace Schedwin.Reservations

{
    public class ReservationsListViewModel : Schedwin.Common.ViewModelBase
    {

        public ReservationsListView View { get; set; }

        public int CurrentUserID { get; set; }

        public String CurrentUserName { get; set; }

        public DateTime SelectedDate { get; set; }

        public String ReservationSearchString { get; set; }

        public bool ShowCancelled { get; set; }


        private String _Status;
        public String Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private GridReservatonItem _selectedReservation;
        public GridReservatonItem SelectedReservation
        {
            get
            {
                return _selectedReservation;
            }
            set
            {
                Status = "";
                _selectedReservation = value;
                NotifyPropertyChanged("ItemSelected");
            }
        }

        public ICommand TextSearchCommand { get; set; }


        public String Title { get; set; }

        private List<GridReservatonItem> _reservationList;
        public RangeObservableCollection<GridReservatonItem> ReservationList
        {
            get
            {
                var observableReservationList = new RangeObservableCollection<GridReservatonItem>();
                if (ShowCancelled)
                    observableReservationList.AddRange(_reservationList);
                else
                    observableReservationList.AddRange(_reservationList.Where(x => x.IsCancelled == false).ToList());

                return observableReservationList;
            }
        }

        public bool ItemSelected
        {
            get
            {
                return SelectedReservation != null ? true : false;
            }
        }

        public ReservationsListViewModel()
        {
            SelectedDate = DateTime.Today;
            _reservationList = new List<GridReservatonItem>();
            TextSearchCommand = new RelayCommand(TextBoxSearch);
        }

        public async void TextBoxSearch(object obj)
        {
            ReservationSearchString = (String)obj;
            await Refresh();
        }

        public async Task Init()
        {
            try
            {
                ShowCancelled = false;
                Title = "Reservation List";
                Status = "Initialising...";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    if (UseGlobalDB)
                        await ReservationInfoViewModel.ForceLookupLoad();
                    else
                         await ReservationInfoViewModel.ForceLookupLoad(Server, Database);
                }
                Status = "";
                NotifyPropertyChanged("ShowCancelled");
                NotifyPropertyChanged("Title");
            }
            catch (Exception ex)
            {
                Status = "Initialization error";
                FailWindow.Display(@"Unable to initialise reservation lookups:" + Environment.NewLine + ex.Message);
            }
        }

        public bool NewReservation()
        {
            var ReservationInfoViewDlg = new ReservationInfoView();
            ReservationInfoViewDlg.Owner = View;
            var ReservationInfoViewDlgModel = ReservationInfoViewDlg.DataContext as ReservationInfoViewModel;
            ReservationInfoViewDlgModel.Server = Server;
            ReservationInfoViewDlgModel.Database = Database;
            ReservationInfoViewDlgModel.CurrentUserID = CurrentUserID;
            ReservationInfoViewDlgModel.RegionName = RegionName;
            ReservationInfoViewDlg.ShowDialog();
            return true;
        }


        public bool EditReservation()
        {
            var ReservationInfoViewDlg = new ReservationInfoView();
            ReservationInfoViewDlg.Owner = View;
            var ReservationInfoViewDlgModel = ReservationInfoViewDlg.DataContext as ReservationInfoViewModel;
            Status = "";

            if (SelectedReservation != null)
            {
                var OriginalReservation = new Reservation(SelectedReservation.Reservation);
                ReservationInfoViewDlgModel.Reservation = SelectedReservation.Reservation;
                ReservationInfoViewDlgModel.Server = Server;
                ReservationInfoViewDlgModel.Database = Database;
                ReservationInfoViewDlgModel.RegionName = RegionName;
                ReservationInfoViewDlg.ShowDialog();
                if (ReservationInfoViewDlg.DialogResult.HasValue && ReservationInfoViewDlg.DialogResult.Value)
                    Status = "Reservation saved";

            }
            return true;
        }

        public async Task SplitReservation()
        {
            try
            {
                if (SelectedReservation != null)
                {
                    var splitResView = new SplitReservationView();
                    var splitResViewModel = splitResView.DataContext as SplitReservationViewModel;
                    splitResView.ShowDialog();
                    if (splitResView.DialogResult.HasValue && splitResView.DialogResult.Value)
                    {
                        await Schedwin.Reservations.Classes.Reservations.SplitReservation(SelectedReservation.Reservation.Header.Res_IDX, splitResViewModel.SplitGuestCount, Server, Database);
                        SuccessWindow.Display("Booking split.");
                    }


                }

            }
            catch (Exception ex)
            {
                FailWindow.Display(@"Failed to split booking: \r\n" + ex.Message);
            }

        }

        public bool ViewBookingItinerary()
        {
            if (SelectedReservation != null)
            {

                var oldVBReports = new ReservationReports();
                oldVBReports.ServerName = Server;
                oldVBReports.DatabaseName = Database;
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    oldVBReports.ViewBookingItineray(SelectedReservation.Reservation.Header.Res_IDX);
                }

            }
            return true;
        }

        public bool ViewItineraryForDate()
        {

            var oldVBReports = new ReservationReports();
            oldVBReports.ServerName = Server;
            oldVBReports.DatabaseName = Database;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                oldVBReports.ViewItineraryForDate(SelectedDate);
            }

            return true;
        }

        public async void AdvanceDate(bool goForward = true)
        {
            if (goForward)
                SelectedDate = SelectedDate.AddDays(1);
            else
                SelectedDate = SelectedDate.AddDays(-1);
            NotifyPropertyChanged("SelectedDate");

            await Refresh();
        }



        public async Task<bool> Refresh()
        {
            var returnVal = false;
            if (UseGlobalDB)
                returnVal=await RefreshFromGlobal();
            else
                returnVal= await RefreshFromRegional();

            return returnVal;
        }

        public async Task<bool> RefreshFromGlobal()
        {
            var reservations = new Schedwin.Reservations.Classes.Reservations();
            List<Reservation> tmpReservationList = null;
            List<int> tmpResIdList = null;
            _reservationList.Clear();

            try
            {
                Status = "Loading...";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    if (String.IsNullOrEmpty(ReservationSearchString))
                    {
                        tmpResIdList = await reservations.GetReservationIDS(SelectedDate, 1);
                        Title = "Reservation List: " + SelectedDate.ToShortDateString();
                    }

                    else
                    {
                        tmpResIdList = await reservations.GetReservationIDS(ReservationSearchString);
                        Title = "Reservation List: " + ReservationSearchString;
                    }


                    if (tmpResIdList == null)
                        return true;

                    tmpReservationList = await reservations.GetReservations(tmpResIdList, 1);
                }

                var tmpGridLst = tmpReservationList.Select(x => new GridReservatonItem { Reservation = x }).ToList();

                tmpGridLst.ForEach(x => x.ConfigureSubLists());
                _reservationList.AddRange(tmpGridLst);
                Status = "";
                NotifyPropertyChanged("ReservationList");
                NotifyPropertyChanged("Title");
                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var errMsg = string.Join(Environment.NewLine, messages);
                Status = "Load error";
                FailWindow.Display(@"Unable to retrieve reservation list:" + Environment.NewLine + errMsg);
                return false;
            }
        }


        public async Task<bool> RefreshFromRegional()
        {
            var reservations = new Schedwin.Reservations.Classes.Reservations();
            List<Reservation> tmpReservationList = null;
            List<int> tmpResIdList = null;
            _reservationList.Clear();

            try
            {
                Status = "Loading...";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    if (String.IsNullOrEmpty(ReservationSearchString))
                    {
                        tmpResIdList = await reservations.GetReservationIDS(SelectedDate, 1, Server, Database);
                        Title = "Reservation List: " + SelectedDate.ToShortDateString();
                    }

                    else
                    {
                        tmpResIdList = await reservations.GetReservationIDS(ReservationSearchString, Server, Database);
                        Title = "Reservation List: " + ReservationSearchString;
                    }


                    if (tmpResIdList == null)
                        return true;

                    tmpReservationList = await reservations.GetReservations(tmpResIdList, 1, Server, Database);
                }

                var tmpGridLst = tmpReservationList.Select(x => new GridReservatonItem { Reservation = x }).ToList();

                tmpGridLst.ForEach(x => x.ConfigureSubLists());
                _reservationList.AddRange(tmpGridLst);
                Status = "";
                NotifyPropertyChanged("ReservationList");
                NotifyPropertyChanged("Title");
                return true;
            }
            catch (Exception ex)
            {
                Status = "Load error";
                FailWindow.Display(@"Unable to retrieve reservation list:"+Environment.NewLine+ ex.Message);
                return false;
            }
        }

        public void ToggleCancelled()
        {
            NotifyPropertyChanged("ReservationList");
        }


    }
}
