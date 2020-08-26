using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Setup
{
    public class AircraftTypeCntrlViewModel : ItemCntrlViewModelBase
    {

        public String AircraftType { get; set; }
        public short NumPax { get; set; }

        public int MaxTakeOffWeight { get; set; }

        public short BlockSpeed { get; set; }

        public float Range { get; set; }

        public float RangeHours { get; set; }

        public short Demurrage { get; set; }

        public short TurnAroundTime { get; set; }

        public float Endurance { get; set; }

        public FuelType SelectedFuelType { get; set; }

        public short FuelFlow { get; set; }

        public bool TwinEngine { get; set; }

        public String Description { get; set; }

        public short MaxStops { get; set; }

        public double FuelArm { get; set; }

        public ACLoadingArrangement SelectedArrangement { get; set; }

        public RangeObservableCollection<FuelType> FuelTypes { get; set; }

        public RangeObservableCollection<ACLoadingArrangement> LoadingArrangements { get; set; }


        public AircraftTypeCntrlViewModel()
        {
            FuelTypes = new RangeObservableCollection<FuelType>();
            LoadingArrangements = new RangeObservableCollection<ACLoadingArrangement>();

        }

        public override void Init()
        {
            IsNew = true;
            RefreshFuelTypes();
        }

        public override bool Validate()
        {
            var invalidFieldLst = new List<String>();

            if (SelectedFuelType==null)
                invalidFieldLst.Add("Fuel Type");

            if (AircraftType == null)
                invalidFieldLst.Add("Aircraft type not selected");

            if (MaxTakeOffWeight<=0)
                invalidFieldLst.Add("Max take off weight");

            if (BlockSpeed <= 0)
                invalidFieldLst.Add("Block speed");

            if (NumPax <= 0)
                invalidFieldLst.Add("No of pax");


            if (Range <= 0)
                invalidFieldLst.Add("Range (KM)");

            if (Demurrage <= 0)
                invalidFieldLst.Add("Demurrage");

            if (MaxStops <= 0)
                invalidFieldLst.Add("Max Stops");

            if (invalidFieldLst.Count > 0)
            {
                String validFailMess = "The following fields need to be completed:" + Environment.NewLine;
                String strfieldList = String.Join(Environment.NewLine, invalidFieldLst);
                validFailMess += strfieldList;

                FailWindow.Display(validFailMess);
                return false;

            }

            return true;
        }

        public override async Task<bool> Save()
        {
            try
            {
                var aircraftType = new AircraftType();
                aircraftType.IsNew = IsNew;
                aircraftType.IDX = IDX;
                aircraftType.IDX_Fueltype = SelectedFuelType.IDX;
                aircraftType.TypeName = AircraftType;
                aircraftType.Pax = NumPax;
                aircraftType.FuelFlow = FuelFlow;
                aircraftType.MaxGrossWeight = MaxTakeOffWeight;
                aircraftType.Speed = BlockSpeed;
                aircraftType.RangeKM = Range;
                aircraftType.RangeHours = RangeHours;
                aircraftType.Demurrage = Demurrage;
                aircraftType.TurnAroundTime = TurnAroundTime;
                aircraftType.TwinEngine = TwinEngine;
                aircraftType.Description = Description;
                aircraftType.MaxStops = MaxStops;
                aircraftType.FuelArm = FuelArm;
                aircraftType.IsActive = true;
                await aircraftType.Save(Server, Database);
                Schedwin.Data.Classes.AircraftType.UpdateCachedObject(aircraftType);

                foreach (var loadingArrangement in LoadingArrangements)
                {
                    await  loadingArrangement.Save(Server, Database);
                }

                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save aircraft type :" + Environment.NewLine + exMessage);
                return false;
            }
           
        }
       public async  void Refresh(AircraftType aircraft)
       {
            try
            {
                IsNew = false;
                IDX = aircraft.IDX;
                RefreshFuelTypes();
                LoadingArrangements.Clear();

                AircraftType = aircraft.TypeName;
                NumPax = aircraft.Pax;
                MaxTakeOffWeight = aircraft.MaxGrossWeight;
                BlockSpeed = aircraft.Speed;
                Range = aircraft.RangeKM;
                RangeHours = aircraft.RangeHours;
                Demurrage = aircraft.Demurrage;
                MaxStops = aircraft.MaxStops;
                TurnAroundTime = aircraft.TurnAroundTime;
                TwinEngine = aircraft.TwinEngine;
                Description = aircraft.Description;
                FuelArm = aircraft.FuelArm;
                SelectedFuelType = FuelTypes.FirstOrDefault(x => x.IDX == aircraft.IDX_Fueltype);
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    var tmpLst = await ACLoadingArrangement.LoadLoadArrangements(IDX, Server, Database);
                    LoadingArrangements.AddRange(tmpLst);
                }
                FuelFlow = aircraft.FuelFlow;

                //NumPax = aircraft.Pax;
                //MaxTakeOffWeight = aircraft.r
                //BlockSpeed= aircraft.bl
                NotifyPropertyChanged("LoadingArrangements");
                NotifyPropertyChanged("MaxStops");
                NotifyPropertyChanged("FuelFlow");
                NotifyPropertyChanged("AircraftType");
                NotifyPropertyChanged("NumPax");
                NotifyPropertyChanged("MaxTakeOffWeight");
                NotifyPropertyChanged("BlockSpeed");
                NotifyPropertyChanged("Range");
                NotifyPropertyChanged("Demurrage");
                NotifyPropertyChanged("TurnAroundTime");
                NotifyPropertyChanged("Description");
                NotifyPropertyChanged("TwinEngine");
                NotifyPropertyChanged("SelectedFuelType");
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to refresh aircraft type :" + Environment.NewLine + exMessage);
            }

        }

        private void RefreshFuelTypes()
        {
            FuelTypes.Clear();
            FuelTypes.AddRange(FuelType.GetFuelTypes());
        }

        public void AddLoadingArrangements()
        {
            var nameWindow = new GetTextWindow("New Loading Arrangement", "Arrangement Name:");
            nameWindow.ShowDialog();

            if (nameWindow.DialogResult.HasValue && nameWindow.DialogResult.Value)
            {
                var newArrangement = new ACLoadingArrangement();
                newArrangement.IDX_ACType = IDX;
                newArrangement.Name = nameWindow.InputText;
                LoadingArrangements.Add(newArrangement);
                NotifyPropertyChanged("LoadingArrangements");
            }
        }

        public void AddFreightRow()
        {
            if (SelectedArrangement != null)
            {
                var acStation = new ACLoadingStation();
                acStation.Number = Convert.ToInt16(SelectedArrangement.Stations.Count + 1);
                acStation.Type = "Freight";
                acStation.IDX_Type = ACStationType.GetStationTypeList().FirstOrDefault(x => x.Description == "Freight").IDX;
                acStation.IDX_AC_TYPE = IDX;
                SelectedArrangement.Stations.Add(acStation);
                SelectedArrangement.RefreshStations();
            }

        }

        public void AddPaxRow()
        {
            if (SelectedArrangement!=null)
            {
                var acStation = new ACLoadingStation();
                acStation.Number = Convert.ToInt16(SelectedArrangement.Stations.Count + 1);
                acStation.Type = "Pax";
                acStation.IDX_Type = ACStationType.GetStationTypeList().FirstOrDefault(x => x.Description == "Pax").IDX;
                acStation.IDX_AC_TYPE = IDX;
                SelectedArrangement.Stations.Add(acStation);
                SelectedArrangement.RefreshStations();
            }

        }
    }
}
