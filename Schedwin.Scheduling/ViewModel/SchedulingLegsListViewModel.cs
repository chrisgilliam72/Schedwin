using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Schedwin.Scheduling
{
  
    public class SchedulingLegsListViewModel :Schedwin.Common.ViewModelBase
    {
        private bool _isReadOnly;
        public bool ReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                NotifyPropertyChanged("ReadOnly");
                NotifyPropertyChanged("CanModify");
            }
        }

        public bool CanModify
        {
            get
            {
                return !_isReadOnly;
            }
        }
        public bool CanDelete
        {
            get
            {
                return (SelectedLeg != null );
                
            }
        }

        public bool CanAddLeg
        {
            get
            {
                return (CanModify && ACPilot != null  && ( SelectedLeg != null || Legs.Count==0));
            }
 
        }

        public NewSchedulingViewModel SchedulingViewModel { get; set; }
        public RangeObservableCollection<AirstripInfo> Airstrips { get; set; }

        public RangeObservableCollection<ScheduleLeg> Legs
        {
            get
            {
                var tmpLst = new RangeObservableCollection<ScheduleLeg>();
                if (ACPilot!=null)
                    tmpLst.AddRange(ACPilot.Legs.ToList());
                return tmpLst;

            }
        }

        private ScheduleLeg _selectedLeg;
        public ScheduleLeg SelectedLeg
        {
            get
            {
                return _selectedLeg;
            }
            set
            {
                _selectedLeg = value;
                NotifyPropertyChanged("CanDelete");
                NotifyPropertyChanged("LegSelected");
                NotifyPropertyChanged("CanAddLeg");
            }
        }

        public bool LegSelected
        {
            
            get
            {
                return SelectedLeg != null;
            }
        }
        private ScheduleACPilot _acPilot;
        public ScheduleACPilot ACPilot
        {
            get
            {
                return _acPilot;
            }
            set
            {
                _acPilot = value;
                NotifyPropertyChanged("CanAddLeg");
            }
        }

        public DateTime FlightDate { get; set; }

        public SchedulingLegsListViewModel()
        {
            Airstrips = new RangeObservableCollection<AirstripInfo>();
        }


        public void Init()
        {
            ACPilot = null;
            ReadOnly = true;
            Airstrips.Clear();
            Airstrips.AddRange(AirstripInfo.GetAirstrips());
            NotifyPropertyChanged("Airstrips");
            ScheduleLeg.Airstrips = Airstrips;
        }

        public void AddLeg()
        {
            var newLeg = new ScheduleLeg();
            newLeg.ResFuelWT = ACPilot.ReserveFuel;
            var lastLeg = SelectedLeg!=null ? SelectedLeg : Legs.LastOrDefault();
            newLeg.MaxPax = ACPilot.Pax;
            newLeg.ACRegistration = ACPilot.AircraftRegistration;
            newLeg.GameFT = 0;
            if (lastLeg!=null)
            {
                int index = Legs.IndexOf(lastLeg);
                newLeg.FromAP = lastLeg.ToAP;
                newLeg.IDX_FromAP = lastLeg.IDX_ToAP;
                newLeg.ETD = lastLeg.ETA.AddMinutes(lastLeg.TurnAroundTime);
                newLeg.ETA = newLeg.ETD;           
                var acLimits = ACAirportLimits.GetAPLimits(newLeg.IDX_FromAP, ACPilot.IDX_AircraftType, ACPilot.FlightDate, ACPilot.FlightDate);
                newLeg.FromMTOW = acLimits != null ? acLimits.MaxTakeOffWeight : (short) 0;    
                
                ACPilot.Legs.Insert(index+1,newLeg);
              

            }
            else
            {
                if (ACPilot.IDX_PilotAP_1.HasValue)
                    newLeg.IDX_FromAP = ACPilot.IDX_PilotAP_1.Value;
                newLeg.ETD = new DateTime(FlightDate.Year, FlightDate.Month, FlightDate.Day, 07, 00,00);
                newLeg.ETA = new DateTime(FlightDate.Year, FlightDate.Month, FlightDate.Day, 07, 00, 00);             
                ACPilot.Legs.Add(newLeg);
            }

            SelectedLeg = newLeg;
            ACPilot.UpdateLegData();
            ACPilot.RecalculateTripWTS();
         
            NotifyPropertyChanged("Legs");
        }

        public void RemoveLegFromSchedule()
        {
            var legIndex = ACPilot.Legs.IndexOf(SelectedLeg);
            if (SelectedLeg != ACPilot.Legs.Last() && SelectedLeg != ACPilot.Legs.First())
            {
                var prevLeg = ACPilot.Legs[legIndex - 1];
                var nextLeg = ACPilot.Legs[legIndex + 1];

                nextLeg.IDX_FromAP = prevLeg.IDX_ToAP;
                nextLeg.FromAP = prevLeg.ToAP;             
            }

            ACPilot.RemoveLeg(SelectedLeg);
            ACPilot.UpdateLegData();
            ACPilot.RecalculateTimes();
            ACPilot.RecalculateTripWTS();

            Legs.Remove(SelectedLeg);
            NotifyPropertyChanged("Legs");

            SchedulingViewModel.UpdateAllGroupsStatus();
        }

        public void RemoveLeg()
        {
            if (Legs != null && SelectedLeg!=null)
            {
               
                if (SelectedLeg.ResList.Count>0)
                    RadWindow.Confirm("This leg has groups schedule on it. Are you sure you want to remove it ?", DeleteLegWithGroups);
                else
                    RadWindow.Confirm("Are you sure you want to remove this leg?", DeleteLegWithNoGroups);
            }

        }

        public void AddSelectedGroups()
        {
            var groups = SchedulingViewModel.GroupsVM.GetSelectedGroups();
            foreach (var group in groups)
            {
                SelectedLeg.AddGroup(group);
                SchedulingViewModel.UpdateGroupStatus(group);
            }

            ACPilot.UpdateLegData();
            ACPilot.RecalculateTripWTS();
        }

        public void AddGroupToLeg(ScheduleLeg droppedLeg, ScheduleGroup newGroup)
        {
            droppedLeg.AddGroup(newGroup);
            ACPilot.UpdateLegData();
            ACPilot.RecalculateTripWTS();
            SchedulingViewModel.UpdateGroupStatus(newGroup);
        }

        public void ShowGroupMenuIems(RadContextMenu menuItem)
        {

            
            if (SelectedLeg!=null && menuItem!=null)
            {
                var grpItem = menuItem.Items[2] as RadMenuItem;
                grpItem.Items.Clear();

                var allItem= new RadMenuItem();
                allItem.Header = "All groups";
                allItem.Click += GrpMenuItem_Click;
                grpItem.Items.Add(allItem);

                foreach (var legRes in SelectedLeg.ResList)
                {
                    var menu = new RadMenuItem();
                    menu.DataContext = legRes;
                    menu.Header = legRes.Reservation;
                    menu.Click += GrpMenuItem_Click;
                    grpItem.Items.Add(menu);
                }
            }
        }

        private void GrpMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (SelectedLeg!=null)
            {
                var mnuItem = e.OriginalSource as RadMenuItem;
                var mnuHeader = mnuItem.Header as String;
                if (mnuHeader != "All groups")
                {
                    var grpItem = mnuItem.DataContext as ScheduleLegRes;
                    SelectedLeg.RemoveGroup(grpItem.IDX_Reservation);
                    SchedulingViewModel.UpdateGroupStatus(grpItem.IDX_Boooking);
                }
                else
                {
                    SelectedLeg.RemoveAllGroups();
                    SchedulingViewModel.UpdateAllGroupsStatus();
                }

                ACPilot.UpdateLegData();
                ACPilot.RecalculateTripWTS();
            }
        }


    


        public void UpdateLegdistance(int newGameFT)
        {
            if (SelectedLeg != null)
            {
                var tmpDist = APDistances.GetDistance(SelectedLeg.IDX_FromAP, SelectedLeg.IDX_ToAP);
                var tmpGameDist = ACPilot.AircraftSpeed * (SelectedLeg.GameFT / 60.0);
                SelectedLeg.Distance = tmpDist + (int)tmpGameDist;
            }

        }

        public void UpdateSelectedLegSource(AirstripInfo selAirStrip)
        {
            if (SelectedLeg != null && selAirStrip != null)
            {
                SelectedLeg.FromAP = selAirStrip.Code;
                SelectedLeg.IDX_FromAP = selAirStrip.IDX;
                if (SelectedLeg.IDX_ToAP > 0)
                {
                    var tmpDist = APDistances.GetDistance(SelectedLeg.IDX_ToAP, selAirStrip.IDX);
                    var tmpGameDist = ACPilot.AircraftSpeed * (SelectedLeg.GameFT / 60.0);
                    SelectedLeg.Distance = tmpDist + (int)tmpGameDist;

                    int flightTimeMins = GetFlightTime(SelectedLeg.Distance);
                    SelectedLeg.ETA = SelectedLeg.ETD.AddMinutes(flightTimeMins);

                    var destAirstrip= Airstrips.FirstOrDefault(x => x.IDX == SelectedLeg.IDX_ToAP);
                    var altAirStrip = Airstrips.FirstOrDefault(x => x.IDX == destAirstrip.IDX_Alt);
                    if (altAirStrip != null)
                    {

                        SelectedLeg.AltAP = (altAirStrip != null) ? altAirStrip.Code : "";
                        SelectedLeg.DistanceToAlt = selAirStrip.AltDistance;
                    }

               
                    SelectedLeg.CalculateLegFuelWT(ACPilot.FuelFlow, ACPilot.AircraftSpeed);
                    ACPilot.RecalculateTripWTS();
                    ACPilot.RecalculateTimes();
                }
      
            }
        }

        public void UpdateSelectedLegDestination(AirstripInfo selAirStrip)
        {
            var lastLeg = Legs.LastOrDefault();
            if (SelectedLeg != null && selAirStrip != null &&  selAirStrip.IDX != SelectedLeg.IDX_ToAP )
            {
                SelectedLeg.GameFT = 0;
                SelectedLeg.TurnAroundTime = selAirStrip.TurnAroundTime;
                var tmpDist= APDistances.GetDistance(SelectedLeg.IDX_FromAP, selAirStrip.IDX);
                var tmpGameDist = ACPilot.AircraftSpeed * (SelectedLeg.GameFT / 60.0);
                SelectedLeg.Distance = tmpDist + (int)tmpGameDist;

                SelectedLeg.IDX_ToAP = selAirStrip.IDX;
                SelectedLeg.ToRefuel = selAirStrip.FuelPoint;
                int flightTimeMins = GetFlightTime(SelectedLeg.Distance);
                SelectedLeg.ETA = SelectedLeg.ETD.AddMinutes(flightTimeMins);
                var altAirStrip = Airstrips.FirstOrDefault(x => x.IDX == selAirStrip.IDX_Alt);
                if (altAirStrip!=null)
                { 
                    SelectedLeg.AltAP = (altAirStrip != null) ? altAirStrip.Code : "";
                    SelectedLeg.DistanceToAlt = selAirStrip.AltDistance;
                }
                var acLimits = ACAirportLimits.GetAPLimits(selAirStrip.IDX, ACPilot.IDX_AircraftType, ACPilot.FlightDate, ACPilot.FlightDate);
                SelectedLeg.ToMLW = acLimits != null ? acLimits.MaxLandingWeight : (short)0;
                SelectedLeg.RefreshDestinationFields();
                SelectedLeg.CalculateLegFuelWT(ACPilot.FuelFlow, ACPilot.AircraftSpeed);

                ACPilot.RecalculateTripWTS();
                ACPilot.RecalculateTimes();
                if (SelectedLeg== lastLeg)
                    AddLeg();
                else
                {
                    var currIndex = Legs.IndexOf(SelectedLeg);
                    SelectedLeg = Legs[currIndex + 1];
                    if (SelectedLeg != null)
                        UpdateSelectedLegSource(selAirStrip);
                }

                
                NotifyPropertyChanged("Legs");
            }
        }
        public void Clear()
        {
            ACPilot = null;
            NotifyPropertyChanged("Legs");
        }

        public void UpdateScheduleStartAP(int newStartAPIDX)
        {
            if (Legs!=null && Legs.Count>0)
            {
                var firstLeg = Legs.First();
                firstLeg.IDX_FromAP = newStartAPIDX;
            }

        }
        
        public void Refresh()
        {
            ReadOnly = false;
            if (ACPilot!=null)
            {
                
                ACPilot.RefreshAllLegETAs();
                ACPilot.RecalculateTimes();
                ACPilot.RefreshAllLegFuelWeights();
                ACPilot.UpdateLegData();
                ACPilot.RecalculateTripWTS();         
                NotifyPropertyChanged("Legs");
            }

        }
        
        public void DeleteAllLegs()
        {
            foreach (var leg in Legs)
                leg.RemoveAllGroups();

            while (Legs.Count>0)
                ACPilot.RemoveLeg(ACPilot.Legs.FirstOrDefault());

            SchedulingViewModel.UpdateAllGroupsStatus();
            NotifyPropertyChanged("Legs");
        }

        public void ShiftLegTimes(double minShift)
        {
            if (ACPilot != null && SelectedLeg != null && minShift != 0)
            {
                ACPilot.ShiftTimes(minShift, SelectedLeg);
                ACPilot.RecalculateTimes();
            }
               

        }

        public void RecalculateLegExForTimes()
        {
            if (ACPilot != null)
            {
                ACPilot.RecalculateTimes();
            }
        }

           public void Refresh(DateTime flightDate, ScheduleACPilot acPilot)
        {
            if (acPilot!=null)
            {
                FlightDate = flightDate;
                ACPilot = acPilot;
                foreach (var leg in Legs)
                {
                    leg.ACRegistration = acPilot.AircraftRegistration;
                }
                NotifyPropertyChanged("Legs");
            }

        }
        private void DeleteLegWithNoGroups(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                RemoveLegFromSchedule();
            }
        }

        private  void DeleteLegWithGroups(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                SelectedLeg.RemoveAllGroups();
                RemoveLegFromSchedule();
            }
        }

        private int GetFlightTime(int distance)
        {
            if (ACPilot.AircraftSpeed > 0)
            {
                int minutes = Convert.ToInt32((double)distance / (double)ACPilot.AircraftSpeed * 60.0);
                int outResult;
                var modVal = Math.DivRem(minutes, 5, out outResult);
                modVal = (modVal + 1) * 5;
                minutes += (modVal - minutes);

                return minutes;
            }
            else
                return 0;
        }
    }
}
