using System.Windows;
using Schedwin.Scheduling;
using Schedwin.Prep;

namespace SchedwinWPF
{
    /// <summary>
    /// Interaction logic for SchedwinFrameWindow.xaml
    /// </summary>
    public partial class SchedwinFrameWindowView : Window
    {
        public SchedwinFrameWindowView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            var initResult= await viewModel.Init();
            if (initResult)
            {
                viewModel.View = this;
                viewModel.DummyParentControl = this.wndFrmHost.Child;
            }
            else
                Close();
        }


        private void WishIntegration_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.WishIntegration();
        }

        private void GPInvoicing_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.GPInvoicing();
        }

        private void GPInvoicingNew_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.GPInvoicingNew();

        }


        private void UnlockSchedules_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.LockSchedules();

        }

        private void UpdateOldSchedule_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.UpdateOldSchedule();
        }

        private void WeightBalance_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.WeightBalance();
        }

        private void AircraftPrep_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.AircraftPrep();
        }

        private void Tickets_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Tickets();
        }

        private void TicketHistory_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.TicketHistory();
        }

        private void BaggageTags_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.BaggageTags();
        }
        private void IndigoTrackNew_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.IndigoTrackNew();
        }

        private void IndigoTrack_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.IndigoTrack();
        }

        private void TrackingTimes_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.TrackingTimes();
        }

        private void NewTechlogRegister_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.NewTechLog();
        }

        private void Setup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Setup();
        }

        private void GPSetup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.GPSetup();
        }

        //private void Reporting_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    var viewModel = DataContext as SchedwinFrameWindowViewModel;
        //    viewModel.Reporting();
        //}

        private void PilotRoster_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.PilotRoster();
        }

        private void RadReservationNewTest_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.ShowNewReservationsWindow();
        }


        private void SetupExFor_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.ShowAirstripExForWindow();
        }

        private void NewSetup_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.ShowNewSetup();
        }

        private void SchedTest_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.ShowNewSchedule();
        }

        private void TicketsNew_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.TicketsNew();
        }



        private void RptFlightMovement_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Flight Movements");
        }

        private void RptInvoiceFlightmovement_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Invoicing Flight Movements");

        }

        private void RptFlightFollowing_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Flight Following");
        }

        private void RptFlightDateRange_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Flight Date Range");
        }
        private void RptScheduleByAirport_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Schedule by Airport");
        }

        private void RptScheduleByOperator_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Schedule by Operator");
        }

        private void RptScheduleByAircraft_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Summary by Aircraft");
        }

        private void PassManBots_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Manifest Bots");
        }

        private void PassManNam_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Manifest Nam");
        }

        private void PassManZimClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Manifest Zim");
        }

        private void CargoManBots_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Cargo Manifest Bots");
        }

        private void CargoManNam_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Cargo Manifest Nam");
        }

        private void CargoManZim_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Cargo Manifest Zim");
        }

        private void Pass_OperatorAllOneDay_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Operator All One Day");
        }

        private void PassListOperator_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger List Operator");
        }

        private void PassSeatKMs_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Seat KMs");
        }

        private void PassCountMonth_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Count Month");
        }

        private void PassFOC_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger FOC");
        }


        private void PassListDailyReservation_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Daily Reservation Movement");
        }

        private void Pass_OperatorSummary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Operator Summary");
        }

        private void Pass_Operator_Airstrip_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Operator Airstrip");
        }

        private void Pass_OperatorDate_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Operator Date");
        }

        private void Pass_FlyingSummary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Flying Summary");
        }

        private void Pass_FlyingHours_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Flying Hours");
        }

        private void Pass_DetailedRoute_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Detailed Route");
        }

        private void Pass_TechlogEntry_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger TechLogEntry");
        }

        private void Pass_DifferentSchedule_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger DifferentSchedule");
        }

        private void Pass_AirTechnical_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger AirTechnical");
        }

        private void Pass_ListInvoicing_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger List Invoicing");
        }

        private void AircraftStat_UseByOperator_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Stat Use By Operator");
        }

        private void AircraftFlightTime_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Flight Time Count");
        }

        private void AircraftFlightCountDate_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Flight Count Date");
        }

        private void AircraftFlightCountDateDOW_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Flight Count Date DOW");
        }

        private void Aircraft_FuelCost_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Fuel Cost");
        }

        private void Aircraft_Fuel_Uplifts_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Fuel Uplifts");
        }

        private void FlightDuty_Period_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Flight Duty Period");
        }

        private void Pilot_DetailedSummary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Pilot Detailed Summary");
        }


        private void Airports_DepartureTaxDomestic_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Airports Departure Tax Domestic");
        }

        private void Airports_DepartureTaxDomesticByAirport_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Airports Departure Tax Domestic By Airport");
        }


        private void Airports_DepartureInternational_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Airports Departure Tax International");
        }



        private void Airports_Landing_Fees_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Airports Landing Fees");
        }

        private void Airports_Arrival_Logbook_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Airports Arrival Log Book");
        }

        private void Airports_Departure_Logbook_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Airports Departure Log Book");
        }

        private void Aircaft_Detailed_Tech_Summary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Detailed Tech Summary");
        }

        private void Last_Techlog_Summary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Last Technical Summary");
        }


        private void Aircraft_Service_Records_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Service Records");
        }


        private void Last_Technical_Summary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Last Technical Summary");
        }


        private void Time_Since_Service_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Time Since Service");
        }


        private void Aircraft_Fuel_UpLifts_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Fuel UpLifts");
        }


        private void PassengerLoad_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Passenger Load Factor");
        }


        private void Aircraft_FlyingSummary_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Flying Summary");
        }


        private void Aircraft_FlyingHours_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Flying Hours");
        }

        private void Aircraft_DetailedRoute_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Detailed Route");
        }
        private void Aircraft_TechlogEntry_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Techlog Entries");
        }


        private void Aircraft_DifferentSchedule_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Aircraft Techlog Differing From Schedules");

        }

        private void MiscBusinessKMs_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinFrameWindowViewModel;
            viewModel.Reporting("Misc Business KMS");
        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }
    }
}
