using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedwin.Scheduling
{
    public class SchedulingPilotListViewModel : ViewModelBase
    {
        private bool _IsReadOnly;
        public bool ReadOnly
        {
            get
            {
                return _IsReadOnly;
            }
            set
            {
                _IsReadOnly = value;
                NotifyPropertyChanged("ReadOnly");
                NotifyPropertyChanged("CanModify");
            }
        }
  
        public bool CanModify
        {
            get
            {
                return !_IsReadOnly;
            }
           
        }

        private DateTime _scheduledDate;
        public DateTime ScheduleDate
        {
            get
            {
                return _scheduledDate;
            }
            set
            {
                _scheduledDate = value;
                NotifyPropertyChanged("ScheduleDate");
            }
        }

        public bool CanRemovePilot
        {
            get
            {
                return SelectedPilot != null;
            }
        }
        public RangeObservableCollection<PilotInfo> Pilots
        {
            get
            {
                var tmpLst = PilotInfo.GetPilotList();
                var pilotsAvailable = DutyTypes.Where(x => x.DutyType == "1").Select(x => x.IDX_Pilot).ToList();            
                var pilots = new RangeObservableCollection<PilotInfo>();
                pilots.AddRange(tmpLst.Where(x => pilotsAvailable.Contains(x.IDX)));
                return pilots;

               
            }
        }
        public RangeObservableCollection<PilotType> PilotTypes { get; set; }

        public List<ScheduleACPilot> DeletedPilots { get; set; }
        public RangeObservableCollection<ScheduleACPilot> ScheduledPilots { get; set; }


        private ScheduleACPilot _selectedPilot;
        public ScheduleACPilot SelectedPilot
        {
            get
            {
                return _selectedPilot;

            }
            set
            {
                _selectedPilot = value;
                NotifyPropertyChanged("SelectedPilot");
                NotifyPropertyChanged("CanRemovePilot");
            }
        }

        public RangeObservableCollection<AircraftInfo> AircraftList { get; set; }

        public RangeObservableCollection<AirstripInfo> Airstrips { get; set; }

        public RangeObservableCollection<AircraftType> AircraftTypes { get; set; }

        public RangeObservableCollection<FuelType> FuelTypes { get; set; }

        public List<PilotDutyType> DutyTypes { get; set; }
        public SchedulingLegsListViewModel LegsViewModel { get; set; }

        public ScheduleTotalViewModel TotalsViewModel { get; set; }

        public SchedulingPilotListViewModel()
        {
            ReadOnly = true;
            ScheduledPilots = new RangeObservableCollection<ScheduleACPilot>();
            DeletedPilots = new List<ScheduleACPilot>();
            DutyTypes = new List<PilotDutyType>();

       
            PilotTypes = new RangeObservableCollection<PilotType>();
            Airstrips = new RangeObservableCollection<AirstripInfo>();
            AircraftList = new RangeObservableCollection<AircraftInfo>();
            AircraftTypes = new RangeObservableCollection<AircraftType>();
            FuelTypes = new RangeObservableCollection<FuelType>();

        }

        public void Init()
        {
            PilotTypes.Clear();
            Airstrips.Clear();
            AircraftList.Clear();
            FuelTypes.Clear();
            DutyTypes.Clear();

          
            PilotTypes.AddRange(PilotType.GetPilotTypes()); 
            Airstrips.AddRange(AirstripInfo.GetAirstrips());
            AircraftList.AddRange(AircraftInfo.GetAircraftList(true));
            AircraftTypes.AddRange(AircraftType.GetACTypes());
            FuelTypes.AddRange(FuelType.GetFuelTypes());
            ScheduleACPilot.AircraftList = AircraftList;


            NotifyPropertyChanged("FuelTypes");
            NotifyPropertyChanged("AircraftTypes");
            NotifyPropertyChanged("AircraftList");
            NotifyPropertyChanged("Airstrips");
            NotifyPropertyChanged("PilotTypes");
        }

        public void UpdateTotalsGrid(ScheduleACPilot selectedACPilot)
        {
            TotalsViewModel.Recalculate(selectedACPilot);
        }


        public void RefreshLegsGrid(ScheduleACPilot selectedACPilot)
        {
            LegsViewModel.Refresh(ScheduleDate,selectedACPilot);
        }

        public void AddPilot()
        {
            var scheduledACPilot = new ScheduleACPilot();
            scheduledACPilot.RowNumber = ScheduledPilots.Count + 1;
            scheduledACPilot.FlightDate = ScheduleDate;
            ScheduledPilots.Add(scheduledACPilot);
            SelectedPilot = scheduledACPilot;

            LegsViewModel.AddLeg();
            NotifyPropertyChanged("SelectedPilot");
            NotifyPropertyChanged("ScheduledPilots");
        }

        public void ClearPilot()
        {
            if (SelectedPilot != null)
            {
                SelectedPilot.IDX_Pilot_1 = null;
                SelectedPilot.Pilot1Name = null;
                SelectedPilot.Pilot1_AP = null; ;
                SelectedPilot.IDX_PilotAP_1 = null;
                SelectedPilot.Pilot1EstFT = null;
                SelectedPilot.PilotWeight = null;
            }
        }
        public void RemovePilot2()
        {
            if (SelectedPilot != null)
            {
                SelectedPilot.IDX_PilotType_2 = null;
                SelectedPilot.IDX_Pilot_2 = null;
                SelectedPilot.Pilot2Name = null;
                SelectedPilot.Pilot2_AP = null;
                SelectedPilot.IDX_PilotAP_2 = null;
                SelectedPilot.Pilot2EstFT = null;
            }
        }


        public void RemoveSelectedPilot()
        {
            if (SelectedPilot != null)
            {
                SelectedPilot.IsDeleted = true;
              

                if (!SelectedPilot.IsNew)
                    DeletedPilots.Add(SelectedPilot);

                LegsViewModel.DeleteAllLegs();

                ScheduledPilots.Remove(SelectedPilot);

                SelectedPilot = null;

                NotifyPropertyChanged("SelectedPilot");
                NotifyPropertyChanged("ScheduledPilots");
            }
        }

        public void Clear()
        {
            DutyTypes.Clear();
            LegsViewModel.Clear();
            ScheduledPilots.Clear();
            NotifyPropertyChanged("ScheduledPilots");
        }


        public async void UpdatePilot1Details(PilotInfo pilotInfo)
        {
            if (SelectedPilot!=null && pilotInfo!=null)
            {
                if (ScheduledPilots.FirstOrDefault(x => x.IDX_Pilot_1 == pilotInfo.IDX_Personnel) != null || ScheduledPilots.FirstOrDefault(x => x.IDX_Pilot_2 == pilotInfo.IDX_Personnel) != null)
                    WarnWindow.Display("This pilot has already been scheduled on this date.");
         
                SelectedPilot.PilotWeight = pilotInfo.Weight;
                SelectedPilot.Pilot1Name = pilotInfo.Name;
                SelectedPilot.IDX_Pilot_1 = pilotInfo.IDX_Personnel;
                SelectedPilot.Pilot1EstFT = await ScheduleDutyRoster.GetPilotEFT(ScheduleDate, pilotInfo.IDX_Personnel, Server, Database);
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    SelectedPilot.IDX_PilotAP_1 = await Schedule.GetPilotLastAP(ScheduleDate, pilotInfo.IDX_Personnel, Server, Database);
                    if (SelectedPilot.IDX_PilotAP_1.HasValue)
                    {
                        SelectedPilot.Pilot1_AP = AirstripInfo.GetAirstripCode(SelectedPilot.IDX_PilotAP_1.Value);
                        //LegsViewModel.UpdateScheduleStartAP(SelectedPilot.IDX_PilotAP_1.Value);
                    }

                }
             
                LegsViewModel.Refresh();
                                         
            }

        }


        public async void UpdatePilot2Details(PilotInfo pilotInfo)
        {
            if (SelectedPilot != null)
            {
                if (ScheduledPilots.FirstOrDefault(x => x.IDX_Pilot_1 == pilotInfo.IDX_Personnel) != null || ScheduledPilots.FirstOrDefault(x => x.IDX_Pilot_2 == pilotInfo.IDX_Personnel) != null)
                    WarnWindow.Display("This pilot has already been scheduled on this date.");

                SelectedPilot.PilotWeight = pilotInfo.Weight;
                SelectedPilot.Pilot2Name = pilotInfo.Name;
                SelectedPilot.IDX_Pilot_2 = pilotInfo.IDX_Personnel;
                SelectedPilot.Pilot2EstFT = await ScheduleDutyRoster.GetPilotEFT(ScheduleDate, pilotInfo.IDX_Personnel, Server, Database);
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {

                    SelectedPilot.IDX_PilotAP_2 = await Schedule.GetPilotLastAP(ScheduleDate, pilotInfo.IDX_Personnel, Server, Database);
                    if (SelectedPilot.IDX_PilotAP_2.HasValue)
                        SelectedPilot.Pilot2_AP = AirstripInfo.GetAirstripCode(SelectedPilot.IDX_PilotAP_2.Value);
                }
              

            }

        }


        public async void UpdateACDetails(AircraftInfo selectedACInfo)
        {
            if (selectedACInfo != null && SelectedPilot != null)
            {
                if (ScheduledPilots.FirstOrDefault(x => x.IDX_Aircraft == selectedACInfo.IDX) != null)
                    WarnWindow.Display("This aircraft has already been scheduled on this date.");

                SelectedPilot.BuyRate = Math.Round(selectedACInfo.BuyRate, 2);
                SelectedPilot.SellRate = Math.Round(selectedACInfo.SellRate, 2);
                SelectedPilot.ReserveFuel = selectedACInfo.ReserveFuel;
                SelectedPilot.EmptyWeight = selectedACInfo.EmptyMass;
                SelectedPilot.AircraftRegistration = selectedACInfo.Registration;
                SelectedPilot.IDX_Aircraft = selectedACInfo.IDX;
                var aircraftType = AircraftTypes.FirstOrDefault(x => x.IDX == selectedACInfo.IDX_AC_Type);
                if (aircraftType != null)
                {

                    SelectedPilot.AircraftType = aircraftType.TypeName;
                    SelectedPilot.RangeKM = aircraftType.RangeKM;
                    SelectedPilot.Pax = aircraftType.Pax;
                    SelectedPilot.RangeHours = aircraftType.RangeHours;
                    SelectedPilot.AircraftWeight = aircraftType.MaxGrossWeight;
                    SelectedPilot.AircraftSpeed = aircraftType.Speed;
                    SelectedPilot.IDX_FuelType = aircraftType.IDX_Fueltype;
                    SelectedPilot.IDX_AircraftType = aircraftType.IDX;
                    SelectedPilot.FuelFlow = aircraftType.FuelFlow;

                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                        int lastACAP = await Schedule.GetAircraftLastAP(ScheduleDate, selectedACInfo.IDX, Server, Database);
                        if (lastACAP > 0)
                        {
                            var lastAP = Airstrips.FirstOrDefault(x => x.IDX == lastACAP);
                            SelectedPilot.IDX_AircraftAP = lastAP != null ? lastAP.IDX : 0;
                            SelectedPilot.AircraftAP = lastAP != null ? lastAP.Code : "";

                        }

                        else
                        {
                            var owner = Company.GetCompany(selectedACInfo.IDX_Owner);
                            if (owner!=null)
                            {
                                var baseAP = owner.IDX_BaseAP;
                                var lastAP = Airstrips.FirstOrDefault(x => x.IDX == baseAP);
                                SelectedPilot.IDX_AircraftAP = lastAP != null ? lastAP.IDX : 0;
                                SelectedPilot.AircraftAP = lastAP != null ? lastAP.Code : "";
                            }

                        }

                        LegsViewModel.UpdateScheduleStartAP(SelectedPilot.IDX_AircraftAP);
                    }



                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                        SelectedPilot.AircraftService = await Schedule.GetACScheduleService(ScheduleDate, selectedACInfo.IDX, Server, Database);

                    }
 
                    LegsViewModel.Refresh();
                }


            }
        }


        public void Refresh(List<ScheduleACPilot> listScheduledPilots)
        {
            ReadOnly = false;
            ScheduleACPilot.PilotList = Pilots;
            ScheduledPilots.Clear();
            ScheduledPilots.AddRange(listScheduledPilots);
            int PilotLeg = 1;

            Debug.WriteLine("Begin Pilot Refresh");
            foreach (var schedPilot in ScheduledPilots)
            {
 
                schedPilot.RowNumber = PilotLeg++;
                var pilotInfo = Pilots.FirstOrDefault(x => x.IDX_Personnel == schedPilot.IDX_Pilot_1);
                if (pilotInfo!=null)
                {
                    schedPilot.PilotWeight = pilotInfo.Weight;
                    schedPilot.Pilot1Name = pilotInfo.Name;
                    Debug.WriteLine("Pilot :" + schedPilot.Pilot1Name);
                }


                pilotInfo = Pilots.FirstOrDefault(x => x.IDX_Personnel == schedPilot.IDX_Pilot_2);
                if (pilotInfo != null)
                {
                    schedPilot.Pilot2Name = pilotInfo.Name;
                }

                var pilot1AP = Airstrips.FirstOrDefault(x => x.IDX == schedPilot.IDX_PilotAP_1);
                if (pilot1AP!=null)
                {
                    schedPilot.Pilot1_AP = pilot1AP.Code;
                }

                var pilot2AP = Airstrips.FirstOrDefault(x => x.IDX == schedPilot.IDX_PilotAP_2);
                if (pilot2AP!=null)
                {
                    schedPilot.Pilot2_AP = pilot2AP.Code;
                }


                var aircraftInfo = AircraftList.FirstOrDefault(x => x.IDX == schedPilot.IDX_Aircraft);
                if (aircraftInfo!=null)
                {
                    schedPilot.BuyRate = Math.Round(aircraftInfo.BuyRate, 2);
                    schedPilot.SellRate = Math.Round(aircraftInfo.SellRate, 2);
                    schedPilot.ReserveFuel = aircraftInfo.ReserveFuel;
                    schedPilot.EmptyWeight = aircraftInfo.EmptyMass;
                    schedPilot.AircraftRegistration = aircraftInfo.Registration;

                    var aircraftType = AircraftTypes.FirstOrDefault(x => x.IDX == aircraftInfo.IDX_AC_Type);
                    if (aircraftType != null)
                    {
                        schedPilot.AircraftType = aircraftType.TypeName;
                        schedPilot.RangeKM = aircraftType.RangeKM;
                        schedPilot.Pax = aircraftType.Pax;
                        schedPilot.RangeHours = aircraftType.RangeHours;
                        schedPilot.AircraftWeight = aircraftType.MaxGrossWeight;
                        schedPilot.AircraftSpeed = aircraftType.Speed;
                        schedPilot.IDX_FuelType = aircraftType.IDX_Fueltype;
                        schedPilot.FuelFlow = aircraftType.FuelFlow;

                    }

                }


                schedPilot.AircraftAP = Airstrips.FirstOrDefault(x => x.IDX == schedPilot.IDX_AircraftAP).Code;
               

                Debug.WriteLine("Before UpdateLegData");
                schedPilot.UpdateLegData();
                Debug.WriteLine("Before CalculateFuelWTs");
                schedPilot.CalculateFuelWTs(schedPilot.ReserveFuel);

                Debug.WriteLine("End Foreach");
            }

           
            NotifyPropertyChanged("ScheduledPilots");

            Debug.WriteLine("End Pilot Refresh");

        }
    }
}
