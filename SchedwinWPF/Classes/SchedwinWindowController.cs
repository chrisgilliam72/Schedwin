
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Schedwin.OldMain.VB;
using Schedwin.Common;
using Schedwin.WishIntegration;
using Schedwin.Reservations;
using Schedwin.Scheduling;
using Schedwin.Setup;
using Schedwin.Reporting.Crystal;
using Schedwin.Techlogs;
using Schedwin.Prep;
using Schedwin.GreatPlains;
using Shedwin.Tracking;
using Schedwin.Reports.Classes;

namespace SchedwinWPF
{
    class SchedwinWindowController : IMainWindowController
    {
        

         public SchedwinBaseWindow ShowNewIndigoTrack(System.Windows.Window Parent)
        {
            try
            {
                var trackingView = new IndigoTrackingView();
                var trackingViewModel = trackingView.DataContext as IndigoTrackingViewModel;

                trackingViewModel.Server = SchedwinFrameWindowViewModel.Server;
                trackingViewModel.Database = SchedwinFrameWindowViewModel.Database;

                trackingView.Owner = Parent;
                trackingView.Show();
                return trackingView;
            }
            catch (Exception ex)
            {

                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);

                return null;
            }
        }




        public SchedwinBaseWindow ShowNewGPInvoicing(System.Windows.Window Parent)
        {
            try
            {
                var gpInvoicingView = new GPInvoicingView();
                var viewModel = gpInvoicingView.DataContext as GPInvoicingViewModel;
                viewModel.Database = SchedwinFrameWindowViewModel.Database;
                viewModel.Server = SchedwinFrameWindowViewModel.Server;
                viewModel.CountryID = SchedwinFrameWindowViewModel.CountryID;
                gpInvoicingView.Owner = Parent;
                gpInvoicingView.Show();
                return gpInvoicingView;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);

                return null;
            }
        }

        public void ShowUnlockSchedules(System.Windows.Window parent)
        {
            try
            {
                var lockedSchedsView = new ViewLockedSchedulesView();
                var viewModel = lockedSchedsView.DataContext as ViewLockedSchedulesViewModel;
                viewModel.Database = SchedwinFrameWindowViewModel.Database;
                viewModel.Server = SchedwinFrameWindowViewModel.Server;
                viewModel.CurrentUser = SchedwinFrameWindowViewModel.CurrentUserName;
                lockedSchedsView.Owner = parent;
                lockedSchedsView.ShowDialog();
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                
            }
        
        }

        public SchedwinBaseWindow ShowTicketsNew(System.Windows.Window Parent)
        {
            var ticketView = new TicketsView();
            var viewModel = ticketView.DataContext as TicketsViewModel;
            viewModel.Database = SchedwinFrameWindowViewModel.Database;
            viewModel.Server = SchedwinFrameWindowViewModel.Server;
            viewModel.CompanyID = SchedwinFrameWindowViewModel.CompanyID;
            viewModel.Region = SchedwinFrameWindowViewModel.Region;
            viewModel.CurrentUser = SchedwinFrameWindowViewModel.CurrentUserName;
            ticketView.Owner = Parent;
            ticketView.Show();
            return ticketView;

        }


        public SchedwinBaseWindow ShowNewScheduling(System.Windows.Window parent)
        {
            try
            {
                var newSchedView = new NewSchedulingView();
                var viewModel = newSchedView.DataContext as NewSchedulingViewModel;
                viewModel.Database = SchedwinFrameWindowViewModel.Database;
                viewModel.Server = SchedwinFrameWindowViewModel.Server;
                viewModel.CurrentUser = SchedwinFrameWindowViewModel. CurrentUserName;
                newSchedView.Owner = parent;
                newSchedView.Show();


                return newSchedView;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
        }
        public SchedwinBaseWindow ShowNewTechLogs(System.Windows.Window parent)
        {
            try
            {
                var techlogView = new TechLogListView();
                var techlogViewModel = techlogView.DataContext as TechLogListViewModel;
                techlogViewModel.Database = SchedwinFrameWindowViewModel.Database;
                techlogViewModel.Server = SchedwinFrameWindowViewModel.Server;

                techlogView.Owner = parent;
                techlogView.Show();

                return techlogView;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
        }
        public SchedwinBaseWindow ShowExForSetup(System.Windows.Window parent)
        {
            try
            {
                var airstripSetUpView = new AirstripExForSetupView();
                var airstripSetupViewModel = airstripSetUpView.DataContext as AirstripExForSetupViewModel;

                airstripSetupViewModel.Database = SchedwinFrameWindowViewModel.Database;
                airstripSetupViewModel.Server = SchedwinFrameWindowViewModel.Server;
                airstripSetUpView.Owner = parent;
                airstripSetUpView.Show();

                return airstripSetUpView;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
        }


        public SchedwinBaseWindow ShowNewReservations(System.Windows.Window parent)
        {

            try
            {
                var reslist = new ReservationsListView();
                var resListViewModel = reslist.DataContext as ReservationsListViewModel;

                resListViewModel.Database = SchedwinFrameWindowViewModel.Database;
                resListViewModel.Server = SchedwinFrameWindowViewModel.Server;
                resListViewModel.CurrentUserID = SchedwinFrameWindowViewModel.CurrentUserID;
                resListViewModel.CurrentUserName = SchedwinFrameWindowViewModel.CurrentUserName;
                resListViewModel.RegionName = SchedwinFrameWindowViewModel.Region;
                reslist.Owner = parent;
                reslist.Show();

                return reslist;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
        }


        public SchedwinBaseWindow ShowPilotRoster(System.Windows.Window parent)
        {
            try
            {
                var pilotRosterView = new PilotRosterView();

                var pilotRosterViewModel = pilotRosterView.DataContext as PilotRosterViewModel;
                pilotRosterViewModel.Database = SchedwinFrameWindowViewModel.Database;
                pilotRosterViewModel.Server = SchedwinFrameWindowViewModel.Server;
                pilotRosterView.Owner = parent;
                pilotRosterView.Show();

                return pilotRosterView;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
        }


        public SchedwinBaseWindow ShowSetup(System.Windows.Window parent)
        {
            try
            {
                var newsetupView = new NewSetupView();
                var viewModel = newsetupView.DataContext as NewSetupViewModel;
                viewModel.Database = SchedwinFrameWindowViewModel.Database;
                viewModel.Server = SchedwinFrameWindowViewModel.Server;
                viewModel.Region = SchedwinFrameWindowViewModel.Region;
                newsetupView.Owner = parent;
                newsetupView.Show();

                return newsetupView;


            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }

        }


        public SchedwinBaseWindow ShowWeightBalance(System.Windows.Window parent)
        {
            try
            {
                var weightsBalView = new WeightsBalanceView();
                var viewModel = weightsBalView.DataContext as WeightsBalanceViewModel;
                viewModel.Database= SchedwinFrameWindowViewModel.Database;
                viewModel.Server = SchedwinFrameWindowViewModel.Server;
                viewModel.Country = SchedwinFrameWindowViewModel.Region;
                weightsBalView.Owner = parent;
                weightsBalView.Show();
                return weightsBalView;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }

        }



        public SchedwinBaseWindow ShowWishIntegration(System.Windows.Window parent)
        {
            try
            {
                var wishIntUI = new WishIntegrationUI();
                wishIntUI.Server = SchedwinFrameWindowViewModel.Server;
                wishIntUI.UseGlobalDB = SchedwinFrameWindowViewModel.UseGlobalDB;
                wishIntUI.Database = SchedwinFrameWindowViewModel.Database;
                wishIntUI.UserID = SchedwinFrameWindowViewModel.CurrentUserID;
                wishIntUI.CompanyID = SchedwinFrameWindowViewModel.CompanyID;
                wishIntUI.CountryID = SchedwinFrameWindowViewModel.CountryID;
                wishIntUI.WishPrincipalID = SchedwinFrameWindowViewModel.PrincipalID;
                wishIntUI.WishPrincipaSoleUselID = SchedwinFrameWindowViewModel.SoleUsePrincipalID;
                wishIntUI.Owner = parent;

                //switch (SchedwinFrameWindowViewModel.Region)
                //{
                //    case "Botswana":                   
                //        wishIntUI.WishPrincipalID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WISHSefoBots"));
                //        wishIntUI.WishPrincipaSoleUselID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WISHSefoBots2"));
                //        break;
                //    case "Namibia":                    
                //        wishIntUI.WishPrincipalID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WISHSefoNam"));
                //        break;
                //    case "Zimbabwe":                   
                //        wishIntUI.WishPrincipalID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WISHSefoZim"));
                //        break;
                //    case "Zambia":                     
                //        wishIntUI.WishPrincipalID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WISHSefoZam"));
                //        break;
                //    default:                       
                //        wishIntUI.WishPrincipalID = Convert.ToInt32(ConfigurationManager.AppSettings.Get("WISHSefoBots"));
                //        break;
                //}

                wishIntUI.Show();
                return wishIntUI;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
        }


        public Form ShowGPInvoicing(System.Windows.Forms.Control Parent)
        { try
            {
                var fGPInvoicing = new frmGPInvoicing();
                fGPInvoicing.DatabaseName = SchedwinFrameWindowViewModel.Database;
                fGPInvoicing.ServerName = SchedwinFrameWindowViewModel.Server;
                fGPInvoicing.IDX_Company = SchedwinFrameWindowViewModel.CompanyID;
                fGPInvoicing.TopLevel = false;
                fGPInvoicing.Parent = Parent;
                fGPInvoicing.Left = 0;
                fGPInvoicing.Top = 0;
                fGPInvoicing.Show();
                fGPInvoicing.BringToFront();
                return fGPInvoicing;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }

        }

    public Form ShowReporting(System.Windows.Forms.Control frmParent, System.Windows.Window wndParent, String reportName)
    {
        try
        {
            var mylib = new SchedwinReportLibrary();
            var reportMan = new ReportManager();
            mylib.SchedRegion = SchedwinFrameWindowViewModel.Region;
           reportMan.SchedRegion= SchedwinFrameWindowViewModel.Region;
           switch (reportName)
            {
                case "Pilot Detailed Summary": mylib.Pilot_DetailedSummary_Click(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database);break;
                case "Flight Movements": mylib.Schedules_FlightMovements(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Flight Following": mylib.Schedules_FlightFollowing(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Flight Date Range": mylib.Schedules_FlightDateRange(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Schedule by Airport": mylib.Schedules_ScheduleByAirport(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Schedule by Operator": mylib.Schedules_ScheduleByOperator(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Summary by Aircraft": mylib.Schedules_SummaryByAircraft(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Cargo Manifest Bots": mylib.BotsCargoMan(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Cargo Manifest Nam": mylib.NamibiaCargoMan(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Cargo Manifest Zim": mylib.ZimbabweCargoMan(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Manifest Bots": mylib.PassengerMannifestBotswana(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Manifest Nam": mylib.PassengerMannifestNamibia(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Manifest Zim": mylib.PassengerMannifestZimbabwe(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Load Factor": mylib.Passenger_LoadFactor(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Operator Summary": mylib.Passenger_CountOperatorSummaryCount(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Operator Airstrip": mylib.Passanger_CountOperatorByAirstripPairs(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Operator Date": mylib.Passengers_CountOperatorByDate(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger List Operator": mylib.ExcelReadyPassengerListByOperatorTotals(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Seat KMs": mylib.ExcelReadyPassengerSeatKMSbyMonth(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Count Month": mylib.ExcelReadyPassengerCountByMonth(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger FOC": mylib.ExcelReadyPassengerFOC(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger Operator All One Day": mylib.Passengers_ListAllOperatorsOneDay(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Passenger List Invoicing":mylib.Passenger_ListInvoicing(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database);break;
                case "Passenger Daily Reservation Movement": mylib.DailyReservationMovementByOperator(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database);break;
                case "Aircraft Flying Summary": mylib.Aircraft_FlyingSummary(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Flying Hours": mylib.Aircraft_FlyingHours(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Detailed Route": mylib.Aircraft_DetailedRouteSummary(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Techlog Entries": mylib.Aircraft_TechlogEntries(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Techlog Differing From Schedules": mylib.Aircraft_TechlogsDifferingFromSchedules(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Detailed Tech Summary": mylib.Aircraft_TechnicalDetailedTechnicalSummary(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Last Technical Summary": mylib.Aircraft_TechnicalLastTechlogSummary(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Service Records": mylib.Aircraft_TechnicalServiceRecords(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Time Since Service": mylib.Aircraft_TechnicalTimeSinceService(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Stat Use By Operator": mylib.Aircraft_StatisticalUseByOperator(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Flight Time Count": mylib.Aircraft_StatisticalFlightTimeCount(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Flight Count Date": mylib.Aircraft_StatisticalFlightTimeCountDatePeriod(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Flight Count Date DOW": mylib.Aircraft_StatisticalFlightTimeCountDatePeriodDOW(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Fuel Cost": mylib.Aircraft_FuelCost(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Aircraft Fuel Uplifts": mylib.Aircraft_FuelUplifts(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Flight Duty Period": mylib.PilotFlightDutyPeriod(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database);break;
                case "Airports Departure Tax Domestic": mylib.DepartureTaxDomestic(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database,
                                                                                        SchedwinFrameWindowViewModel.CurrentUserName, ""); break;
                case "Airports Departure Tax Domestic By Airport": mylib.DepartureTaxDomesticByAirport(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database,
                                                                                                        SchedwinFrameWindowViewModel.CurrentUserName,""); break;
                case "Airports Departure Tax International": mylib.DepartureTaxInternational(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database,
                                                                                                SchedwinFrameWindowViewModel.CurrentUserName, ""); break;
                case "Airports Landing Fees": mylib.Airport_LandingFees(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Airports Arrival Log Book": reportMan.AirportArrivalLogBookReport(wndParent,SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database,true); break;
                case "Airports Departure Log Book": reportMan.AirportArrivalLogBookReport(wndParent,SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database, false); break;
                case "Invoicing Flight Movements": mylib.Schedules_InvoicingFlightMovements(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                case "Misc Business KMS":   mylib.Misc_BusinessKMs(SchedwinFrameWindowViewModel.Server, SchedwinFrameWindowViewModel.Database); break;
                      
                }

                return new Form();

        }
        catch (Exception ex)
        {
            var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
            var exMessage = string.Join(Environment.NewLine, messages);
            FailWindow.Display(ex.Message);
            return null;
        }

    }

            //}


            //public Form ShowReporting(System.Windows.Forms.Control Parent) 
            //{
            //    try
            //    {
            //        var fReports = new frmReports();
            //        fReports.DatabaseName = SchedwinFrameWindowViewModel.Database;
            //        fReports.ServerName = SchedwinFrameWindowViewModel.Server;
            //        fReports.SchedRegion = SchedwinFrameWindowViewModel.Region;
            //        fReports.Username = SchedwinFrameWindowViewModel.CurrentUserName;
            //        fReports.TopLevel = false;
            //         fReports.Parent = Parent;
            //        fReports.Left = 0;
            //        fReports.Top = 0;
            //        fReports.Show();
            //        fReports.BringToFront();

            //        return fReports;
            //    }
            //    catch (Exception ex)
            //    {
            //        var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
            //        var exMessage = string.Join(Environment.NewLine, messages);
            //        FailWindow.Display(ex.Message);
            //        return null;
            //    }


            //}

            public Form ShowGPSetup(System.Windows.Forms.Control Parent)
            {
            try
            {

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
            return null;
            }

      

        public Form ShowTickets(System.Windows.Forms.Control Parent) 
        {
            try
            {
                var fTickets = new frmTickets();
                fTickets.Database = SchedwinFrameWindowViewModel.Database;
                fTickets.Server = SchedwinFrameWindowViewModel.Server;
                fTickets.UserName = SchedwinFrameWindowViewModel.CurrentUserName;
                fTickets.TopLevel = false;
                fTickets.Parent = Parent;
                fTickets.Left = 0;
                fTickets.Top = 0;
                fTickets.Show();
                fTickets.BringToFront();

                return fTickets;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;

            }

        }

        public Form ShowIndigoTrack(System.Windows.Forms.Control Parent) 
        {
            try
            {
                var fTracking = new frmTrackingScreen();
                fTracking.TopLevel = false;
                fTracking.DatabaseName = SchedwinFrameWindowViewModel.Database;
                fTracking.ServerName = SchedwinFrameWindowViewModel.Server;
                fTracking.Parent = Parent;
                fTracking.Left = 0;
                fTracking.Top = 0;
                fTracking.Show();
                fTracking.BringToFront();

                return fTracking;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
         
        }


        public Form ShowTicketHistory(System.Windows.Forms.Control Parent) 
        {
            try
            {
                var fTicketHistory = new frmTicketHistory();
                fTicketHistory.Database = SchedwinFrameWindowViewModel.Database;
                fTicketHistory.Server = SchedwinFrameWindowViewModel.Server;
                fTicketHistory.TopLevel = false;
                fTicketHistory.Parent = Parent;
                fTicketHistory.Left = 0;
                fTicketHistory.Top = 0;
                fTicketHistory.Show();
                fTicketHistory.BringToFront();

                return fTicketHistory;

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
           
        }

        public Form ShowAircraftPrep(System.Windows.Forms.Control Parent) 
        {
            try
            {

                //var fFlightprep = new frmFlightPrep();
                //fFlightprep.TopLevel = false;
                //fFlightprep.Parent = Parent;
                //fFlightprep.Left = 0;
                //fFlightprep.Top = 0;
                //fFlightprep.Show();
                //fFlightprep.BringToFront();

                //return fFlightprep;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
            return null;
        }


        public Form ShowTrackingTimes(System.Windows.Forms.Control Parent) 
        {
            try
            {
                var fTrackingTimes = new frmTrackingTimes();
                fTrackingTimes.Database = SchedwinFrameWindowViewModel.Database;
                fTrackingTimes.Server = SchedwinFrameWindowViewModel.Server;
                fTrackingTimes.ShowDialog();
                fTrackingTimes.BringToFront();
                return fTrackingTimes;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
     
        }

        public Form ShowBaggageTags(System.Windows.Forms.Control Parent) 
        {
            try
            {

                var fBaggagTag = new frmBaggageTags();
                fBaggagTag.Username = SchedwinFrameWindowViewModel.CurrentUserName;
                fBaggagTag.Database = SchedwinFrameWindowViewModel.Database;
                fBaggagTag.Server = SchedwinFrameWindowViewModel.Server;
                fBaggagTag.TopLevel = false;
                fBaggagTag.Parent = Parent;
                fBaggagTag.Left = 0;
                fBaggagTag.Top = 0;
                fBaggagTag.Show();
                fBaggagTag.BringToFront();
                return fBaggagTag;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
           
        }



        public Form  ShowUpdateOldSchedule(System.Windows.Forms.Control Parent)
        {
            try
            {

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
                return null;
            }
            return null;
        }
          
    
    }
}
