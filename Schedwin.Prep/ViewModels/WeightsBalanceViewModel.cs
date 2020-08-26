using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Prep.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Schedwin.Prep
{
    public class WeightsBalanceViewModel : ViewModelBase
    {

        private String _status;
        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }
        public List<WeightBalanceLegsView> LegCntrls { get; set; }

        private String _acRegistration;
        public String ACRegistration
        {
            get
            {
                return _acRegistration;
            }
            set
            {
                _acRegistration = value;
                NotifyPropertyChanged("ACRegistration");
            }
        }

        private String _ACType;
        public String ACType
        {
            get
            {
                return _ACType;
            }
            set
            {
                _ACType = value;
                NotifyPropertyChanged("ACType");
            }
        }

        public String Country { get; set; }
        public DateTime ScheduleDate { get; set; }

        public DateTime? SelectedDate { get; set; }

        public ScheduleACPilot SelectedPilot { get; set; }

        public int MaleWeight { get; set; }

        public int FemaleWeight { get; set; }

        public WeightsBalanceView View { get; set; }
        public RangeObservableCollection<ScheduleACPilot> ACPilots { get; set; }

        public Schedule Schedule { get; set; }

   



        public List<PilotInfo> Pilots { get; set; }
        public WeightsBalanceViewModel()
        {
            ScheduleDate = DateTime.Today;
            SelectedDate = null;

            ACPilots = new RangeObservableCollection<ScheduleACPilot>();
            LegCntrls = new List<WeightBalanceLegsView>();
        }


        public void AddLegControl(WeightBalanceLegsView legControl)
        {
            var legcntrlViewModel = legControl.DataContext as WeightBalanceLegsViewModel;
            legcntrlViewModel.WeightBalanceViewModel = this;
            LegCntrls.Add(legControl);
        }

        private void HideAllLegsCntrls()
        {
            foreach (var legCntrl in LegCntrls)
            {
                var viewModel = legCntrl.DataContext as WeightBalanceLegsViewModel;
                viewModel.IsVisible = false;
            }
            
        }
        public async Task Refresh()
        {
            HideAllLegsCntrls();

            ACPilots.Clear();

            if (!String.IsNullOrEmpty(Server) && !String.IsNullOrEmpty(Database))
            {
                Status = "Loading schedule...";
                Schedule = await Schedule.LoadScheduleAsync(SelectedDate.Value, Server, Database);
                Status = "";
                foreach (var schedPilot in Schedule.list_ACPilots)
                {

                    var pilotInfo = Pilots.FirstOrDefault(x => x.IDX_Personnel == schedPilot.IDX_Pilot_1);
                    if (pilotInfo != null)
                    {
                        schedPilot.PilotWeight = pilotInfo.Weight;
                        schedPilot.Pilot1Name = pilotInfo.Name;
                    }
                }

                ACType = "";
                ACRegistration = "";
                ACPilots.AddRange(Schedule.list_ACPilots.OrderBy(x => x.Pilot1Name));
                NotifyPropertyChanged("ACPilots");
                NotifyPropertyChanged("LegCntrlsVis");
            }

        }

        public async Task PilotSelected()
        {
            HideAllLegsCntrls();

            if (SelectedPilot != null)
            {
              

                var allLegIDS = SelectedPilot.Legs.Select(x => x.IDX).ToList();
                var acInfo = AircraftInfo.GetAircraftInfo(SelectedPilot.IDX_Aircraft);
                List<WeightBalancePositionItem> allRowItems = null;
                if (acInfo != null)
                {

                    var acType = AircraftType.GetACType(SelectedPilot.IDX_AircraftType);

                    SelectedPilot.AircraftSpeed = acType.Speed;
                    SelectedPilot.FuelFlow = acType.FuelFlow;
                    SelectedPilot.ReserveFuel = acInfo.ReserveFuel;
                    var legs = SelectedPilot.Legs;

                    foreach (var leg in legs)
                    {


                        leg.FromAP = AirstripInfo.GetAirstripCode(leg.IDX_FromAP);
                        var toAPInfo = AirstripInfo.GetAirstripInfo(leg.IDX_ToAP);
                        leg.ToAP = toAPInfo.Code;

                        var altAPInfo = AirstripInfo.GetAirstripInfo(toAPInfo.IDX_Alt);

                        if (altAPInfo != null)
                        {
                            leg.ToRefuel = altAPInfo.FuelPoint;
                            leg.AltAP = altAPInfo.Code;
                            leg.DistanceToAlt = toAPInfo.AltDistance;
                        }
                    }


                    SelectedPilot.CalculateFuelWTs();


                    ACRegistration = acInfo.Registration;
                    ACType = acType.TypeName;
                    var ldingArrangements = await ACLoadingArrangement.LoadLoadArrangements(acInfo.IDX_AC_Type, Server, Database);

                    Status = "Loading saved data";
                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))     
                        allRowItems= await WeightBalanceSchedule.Load(allLegIDS, Server, Database);
                    Status = "";

                
                    var legCntrlIndx = 0;
                    foreach (var leg in legs)
                    {
                       

                        var wbLegViewModel = LegCntrls[legCntrlIndx++].DataContext as WeightBalanceLegsViewModel;

                        if (wbLegViewModel.Init(acType, leg, Convert.ToInt32(SelectedPilot.PilotWeight), acInfo.EmptyArm, acInfo.EmptyMass))
                            wbLegViewModel.Refresh(allRowItems, ldingArrangements);
                        else
                            FailWindow.Display("Aircraft type not supported");
                    }
                }


            }

        }

        public async Task Init()
        {
            Status = "Initializing...";
            Pilots = PilotInfo.GetPilotList();
            await StandardPassengerWeights.LoadStandardWeights(Server, Database);
            await AircraftType.LoadACTypes(Server, Database,false);
            await AircraftInfo.LoadAircraftList(Server, Database, false);
            await StandardPassengerWeights.LoadStandardWeights(Server, Database);
            var countryIDX = Schedwin.Data.Classes.Country.GetCountry(Country).IDX;
            MaleWeight = StandardPassengerWeights.GetStandardWeight(countryIDX, true, true);
            FemaleWeight = StandardPassengerWeights.GetStandardWeight(countryIDX, false, true);
            Status = "";
        }

        public async Task Save()
        {
            Status = "Saving...";
            var allRowItems = new List<WeightBalancePositionItem>();
            var allLegIDS = SelectedPilot.Legs.Select(x => x.IDX).ToList();
            foreach (var legCntrl in LegCntrls)
            {
                if (legCntrl.IsVisible)
                {
                    var viewModel = legCntrl.DataContext as WeightBalanceLegsViewModel;
                    allRowItems.AddRange(viewModel.Rows);
                }
            }


            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await WeightBalanceSchedule.Clear(allLegIDS, Server, Database);
                await WeightBalanceSchedule.Save(allRowItems, Server, Database);
            }

            Status = "";
        }

        public void Print()
        {
            try
            {

                var wbPrintView = new WeightBalancePrintView();
                var prntWindow = new PrintWindowContainer();
                prntWindow.PrintedControl = wbPrintView;
                var wbPrintViewModel = wbPrintView.DataContext as WeightBalancePrintViewModel;

                wbPrintViewModel.DOF = SelectedDate.Value.ToShortDateString();
                wbPrintViewModel.Pilot = SelectedPilot.Pilot1Name;
                wbPrintViewModel.Aircraft = ACRegistration;
                wbPrintViewModel.AircraftType = this.ACType;
              

                foreach (var legControl in LegCntrls)
                {
                    var legctrlVM = legControl.DataContext as WeightBalanceLegsViewModel;
                    if (legctrlVM.IsVisible)
                    {
                        wbPrintViewModel.AddLeg(legctrlVM.Rows.ToList(), legctrlVM.Leg.FromAP, legctrlVM.Leg.ToAP, legctrlVM.Leg.LegFlightTime.ToString());
                    }
                }

                wbPrintViewModel.Refresh();
                prntWindow.Landscape = true;
                prntWindow.WindowTitle = "Weights and Balances";
                prntWindow.Owner = View;
                prntWindow.Show();
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Initialization error :" + Environment.NewLine + message);
            }
        }
    }
}
