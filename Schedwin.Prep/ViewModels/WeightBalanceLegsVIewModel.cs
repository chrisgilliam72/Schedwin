using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class WeightBalanceLegsViewModel : ViewModelBase
    {
        public ArmGraphControlViewModelBase GraphControlViewModel { get; set; }

        public ArmGraphControlViewBase GraphControlView { get; set; }

        public WeightsBalanceViewModel WeightBalanceViewModel { get; set; }
        public int PilotWeight { get; set; }
        public int IDX_ACType { get; set; }

        public double ACEmptyArm { get; set; }

        public double ACEmptyMass { get; set; }

        public float ACFuelFlow { get; set; }

        public Double FuelArm { get; set; }

        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }


        public String LegName
        {
            get
            {
                if (Leg != null)
                    return Leg.Description;
                else
                    return "";
            }
        }

        public int MaleWeight
        {
            get
            {
                return WeightBalanceViewModel.MaleWeight;
            }
        }

        public int FemaleWeight
        {
            get
            {
                return WeightBalanceViewModel.FemaleWeight;
            }
        }

        public ScheduleLeg Leg { get; set; }
        private ACLoadingArrangement _selectedArrangement;
        public ACLoadingArrangement SelectedArrangement
        {
            get
            {
                return _selectedArrangement;
            }
            set
            {
                _selectedArrangement = value;
                RecalculateWeightPositions();
                NotifyPropertyChanged("SelectedArrangement");
            }
        }
        public RangeObservableCollection<WeightBalancePositionItem> Rows { get; set; }
        public RangeObservableCollection<ACLoadingArrangement> LoadingArrangements { get; set; }

        public int TotalAssignedPax
        {
            get
            {
                int paxTotal = 0;
                if (Rows!=null)
                     paxTotal += Rows.SelectMany(x => x.PaxSeatAssignments).Where(x =>(x.IsMale ||x.IsFemale)).Count();

                return paxTotal;
            }
        }


        public int TotalAssignedMen
        {
            get
            {
                int paxTotal = 0;

                if (Rows != null)
                    paxTotal += Rows.SelectMany(x => x.PaxSeatAssignments).Where(x => x.IsMale).Count();        

                return paxTotal;
            }
        }

        public int TotalAssignedWomen
        {
            get
            {
                int paxTotal = 0;

                if (Rows != null)
                    paxTotal += Rows.SelectMany(x => x.PaxSeatAssignments).Where(x => x.IsFemale).Count();

                return paxTotal;
            }
        }


        public bool Init(AircraftType aircraftType, ScheduleLeg scheuledLeg, int pilotWeight, double acEmptyArm, double acEmptyMass)
        {
            LoadingArrangements.Clear();
            Rows.Clear();
            IDX_ACType = aircraftType.IDX;
            FuelArm = aircraftType.FuelArm;
            ACFuelFlow = aircraftType.FuelFlow;
            ACEmptyArm = acEmptyArm;
            ACEmptyMass = acEmptyMass;

            switch (aircraftType.TypeName)
            {
                case "C206": GraphControlView = new C206ArmGraphControlView()  ; GraphControlViewModel = GraphControlView.DataContext as  C206ArmGraphControlViewModel; break;
                case "C208STOL": GraphControlView = new C208ArmGraphControlView(); GraphControlViewModel = GraphControlViewModel = GraphControlView.DataContext as C208ArmGraphControlViewModel; break;
                case "GA 8": GraphControlView = new GA8ArmGraphControlView (); GraphControlViewModel =  GraphControlView.DataContext as GA8ArmGraphControlViewModel; break;
                case "F406": GraphControlView = new F406ArmGraphControlView(); GraphControlViewModel = GraphControlView.DataContext as F406ArmGraphControlViewModel; break;


                default: return false;
            }

    

            Leg = scheuledLeg;
            PilotWeight = pilotWeight;      
            IsVisible = true;

            NotifyPropertyChanged("GraphControlView");
            NotifyPropertyChanged("LegName");
            NotifyPropertyChanged("Rows");
            NotifyPropertyChanged("LoadingArrangements");
            return true;
        }

        public WeightBalanceLegsViewModel()
        {
            LoadingArrangements = new RangeObservableCollection<ACLoadingArrangement>();
            Rows = new RangeObservableCollection<WeightBalancePositionItem>();
        }

        public void RecalculateZFW()
        {
            var totalWeight = Rows.Where(x=>x.UseForZFW).Sum(x => x.Weight);
            var totalMom = Rows.Where(x =>x.UseForZFW).Sum(x => x.Mom);

            var zfwItemPosition = Rows.FirstOrDefault(x => x.Name == "ZFW");
            zfwItemPosition.Weight = totalWeight;
            zfwItemPosition.Arm = Math.Round((totalMom * 1000) / totalWeight,1);
        }

        public void RecalculateFuelWeights()
        {
            var zfwItemPosition = Rows.FirstOrDefault(x => x.Name == "ZFW");
            var strtFuelPosition = Rows.FirstOrDefault(x => x.Name == "Start Fuel");
            var tOffWghtPos = Rows.FirstOrDefault(x => x.Name == "T/Off Weight");
            var tripFuelPos = Rows.FirstOrDefault(x => x.Name == "Trip Fuel");
            var lndWeight = Rows.FirstOrDefault(x => x.Name == "Land Weight");

            tOffWghtPos.Weight = zfwItemPosition.Weight + strtFuelPosition.Weight;
            var tmpMom = zfwItemPosition.Mom + strtFuelPosition.Mom;
            tOffWghtPos.Arm = Math.Round((tmpMom * 1000) / tOffWghtPos.Weight, 1);

            lndWeight.Weight = tOffWghtPos.Weight - tripFuelPos.Weight;
            tmpMom = tOffWghtPos.Mom - tripFuelPos.Mom;
            lndWeight.Arm=Math.Round((tmpMom * 1000) / tOffWghtPos.Weight, 1);
        }


        public void RecalculateWeightPositions()
        {
            if (SelectedArrangement != null)
            {
                var totalPaxCount = Leg.ResList.Sum(x => x.NumPax);
                Rows.Clear();

                var stations = SelectedArrangement.Stations.OrderBy(x => x.Number).ToList();

                var weightPosItem = new WeightBalancePositionItem(0);
                weightPosItem.IDX_Leg = Leg.IDX;
                weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                weightPosItem.Name = "BEOW";
                weightPosItem.Weight = ACEmptyMass;
                weightPosItem.Arm = ACEmptyArm;
                weightPosItem.CanEdit = false;
                Rows.Add(weightPosItem);
                foreach (var station in stations)
                {

                    weightPosItem = new WeightBalancePositionItem(station.MaxSeats);
                    weightPosItem.IDX_Leg = Leg.IDX;
                    weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                    weightPosItem.Name = station.Name;
                    weightPosItem.Arm = Math.Round(station.Arm, 2);
                    if (station.Number == 1)
                        weightPosItem.AddPilot(PilotWeight);
                    if (station.Type == "Freight")
                    {
                        weightPosItem.MaxSeats = 0;
                        weightPosItem.Freight = true;
                    }
                    Rows.Add(weightPosItem);
                }

                weightPosItem = new WeightBalancePositionItem(0);
                weightPosItem.IDX_Leg = Leg.IDX;
                weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                weightPosItem.Name = "ZFW";
                weightPosItem.UseForZFW = false;
                weightPosItem.CanEdit = false;
                Rows.Add(weightPosItem);

                weightPosItem = new WeightBalancePositionItem(0);
                weightPosItem.IDX_Leg = Leg.IDX;
                weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                weightPosItem.Name = "Start Fuel";
                weightPosItem.Arm = FuelArm;
                weightPosItem.Weight = Leg.TotalTripFuelWT;
                weightPosItem.UseForZFW = false;
                weightPosItem.CanEdit = true;
                Rows.Add(weightPosItem);

                weightPosItem = new WeightBalancePositionItem(0);
                weightPosItem.IDX_Leg = Leg.IDX;
                weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                weightPosItem.Name = "T/Off Weight";
             
                weightPosItem.UseForZFW = false;
                weightPosItem.CanEdit = false;
                Rows.Add(weightPosItem);

                weightPosItem = new WeightBalancePositionItem(0);
                weightPosItem.IDX_Leg = Leg.IDX;
                weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                weightPosItem.Name = "Trip Fuel";
                weightPosItem.Arm = FuelArm;
                weightPosItem.Weight = Leg.TotalFuelWT;
                weightPosItem.UseForZFW = false;
                weightPosItem.CanEdit = false;
                Rows.Add(weightPosItem);


                weightPosItem = new WeightBalancePositionItem(0);
                weightPosItem.IDX_Leg = Leg.IDX;
                weightPosItem.IDX_Loading = SelectedArrangement.IDX;
                weightPosItem.Name = "Land Weight";
                weightPosItem.UseForZFW = false;
                weightPosItem.CanEdit = false;
                Rows.Add(weightPosItem);


                RecalculateZFW();
                RecalculateFuelWeights();
                NotifyPropertyChanged("Rows");
            }

        }

        public void Refresh(List<WeightBalancePositionItem> savedRowItems, List<ACLoadingArrangement> loadingArrangements)
        {
            LoadingArrangements.AddRange(loadingArrangements);


            if (savedRowItems!=null)
            {
                var thisLegRows = savedRowItems.Where(x => x.IDX_Leg == Leg.IDX);
                if (thisLegRows!=null && thisLegRows.Count() >0)
                {
                    var selectedArrangementIDX= thisLegRows.FirstOrDefault().IDX_Loading;
                    _selectedArrangement = LoadingArrangements.FirstOrDefault(x => x.IDX == selectedArrangementIDX);
                    RecalculateWeightPositions();

                    foreach (var item in thisLegRows)
                    {
                        var currentRow = Rows.First(x => x.Name == item.Name);
                        currentRow.Weight = item.Weight;
                        currentRow.SeatingAssignment = item.SeatingAssignment;
                        currentRow.Freight = item.Freight;

                    }

                    RecalculateZFW();
                    RecalculateFuelWeights();
                    NotifyPropertyChanged("Rows");
                }


            }
            NotifyPropertyChanged("LoadingArrangements");
            NotifyPropertyChanged("SelectedArrangement");
        }

        public void UpdateRowWeights(WeightBalancePositionItem selectedItem)
        {
            if (selectedItem != null)
            {
                if (selectedItem.MaxSeats==0)
                {
                    var freightAssignementView = new FreightRowAssignmentView();
                    var viewModel = freightAssignementView.DataContext as FreightRowAssignmentViewModel;
                    viewModel.RowDetails = selectedItem.Name;
                    freightAssignementView.Owner = WeightBalanceViewModel.View;
                    var dlgReslt = freightAssignementView.ShowDialog();
                    if (dlgReslt.HasValue && dlgReslt.Value)
                    {
                        selectedItem.Weight = viewModel.ActualWeight;
                        selectedItem.RefreshSeatingAssignment();
                    }

                }
                else
                {
                    var paxRowAssignmentsView = new PaxRowAssignementView();
                    var viewModel = paxRowAssignmentsView.DataContext as PaxRowAssignementViewModel;

                    paxRowAssignmentsView.Owner = WeightBalanceViewModel.View;           
                    viewModel.WeightBalanceLegViewModel = this;
                    viewModel.ResList = Leg.ResList;
                    viewModel.RowPositionItem = selectedItem;
                    viewModel.RowDetails = selectedItem.Name;
                    viewModel.AdditionalFreight = selectedItem.AdditionalFreightWeight;
                    var dlgReslt = paxRowAssignmentsView.ShowDialog();
                    if (dlgReslt.HasValue && dlgReslt.Value)
                    {
                        selectedItem.Weight = viewModel.RowWeight;
                        if (viewModel.AdditionalFreight>0)
                        {
                            selectedItem.Freight = true;
                            selectedItem.Weight += viewModel.AdditionalFreight;
                            selectedItem.AdditionalFreightWeight= viewModel.AdditionalFreight;

                        }
                       else
                        {
                            selectedItem.Freight = false;
                            selectedItem.AdditionalFreightWeight = 0;
                        }


                        selectedItem.RefreshSeatingAssignment();
                    }

                }
                RecalculateZFW();
                RecalculateFuelWeights();
            }

        }
        public void RefreshGraph()
        {
            if (GraphControlViewModel!=null && Rows!=null && Rows.Count >1)
             {
                var takeOffRow = Rows.FirstOrDefault(x => x.Name == "T/Off Weight");
                var landingRow = Rows.FirstOrDefault(x => x.Name == "Land Weight");
                if (takeOffRow!=null && landingRow!=null)
                    GraphControlViewModel.Refresh(takeOffRow.Weight,takeOffRow.Arm, landingRow.Weight,landingRow.Arm);
            }
        }

    }

 
}
