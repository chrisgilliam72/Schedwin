
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;
using Schedwin.Common;
using Schedwin.Data.Classes;
using System.Diagnostics;
using System.Windows.Media;

namespace Schedwin.Scheduling.Classes
{
    public class ScheduleLeg : ViewModelBase
    {
        public bool IsComplete
        {
            get
            {
                return (IDX_ToAP > 0 && IDX_FromAP > 0);
            }
        }
        public static RangeObservableCollection<AirstripInfo> Airstrips { get; set; }

        public bool IsDeleted { get; set; }

        public int IDX { get; set; }
        public int IDX_AC_Pilot { get; set; }
        
        public double ACMaxWeight { get; set; }

        public String ACRegistration;

        private int _idxFromAP;
        public int IDX_FromAP
        {
            get
            {
                return _idxFromAP;
            }
            set
            {
                _idxFromAP = value;
                NotifyPropertyChanged("IDX_FromAP");
                NotifyPropertyChanged("FromAP");
                NotifyPropertyChanged("Distance");
            }
        }

        public String FromAP { get; set; }

        private DateTime _ETD;
        public DateTime ETD
        {
            get
            {
                return _ETD;
            }
            set
            {
                _ETD = value;          
                NotifyPropertyChanged("ETD");
            }
        }

        private DateTime _ETA;
        public DateTime ETA
        {
            get
            {
                return _ETA;
            }
            set
            {
                _ETA = value;
                NotifyPropertyChanged("ETA");
            }
        }
        

        private int _idxToAP;
        public int IDX_ToAP
        {
            get
            {
                return _idxToAP;
            }
            set
            {
                _idxToAP = value;
                NotifyPropertyChanged("IDX_ToAP");
                NotifyPropertyChanged("Distance");
            }
        }

        public String ToAP { get; set; }

        public String AltAP { get; set; }

        public int NumPax
        {
            get
            {
                return ResList.Sum(x => x.NumPax);
            }

        }

        private int _distance;
        public int Distance
        {
            get
            {
                return _distance;
            }
            set
            {
                _distance = value;
                NotifyPropertyChanged("Distance");
            }
        }

        public Brush MTOWColor
        {
            get
            {
                if (MTOW <= ACMaxWeight)
                    return Brushes.LightSalmon;

                return Brushes.Red;
            }
        }

        public Brush MLWColor
        {
            get
            {
                if (MLW <= ACMaxWeight)
                    return Brushes.LightSalmon;

                return Brushes.Red;
            }
        }


        public Double GameFT { get; set; }

        public Double Budget { get; set; }

        public short MaxTOW { get; set; }

        public short MaxLDW { get; set; }

        public short FromMTOW { get; set; }

        public short ToMLW { get; set; }

        private int _turnaroundtime;
        public int TurnAroundTime
        {
            get
            {
                return _turnaroundtime;
            }
            set
            {
                _turnaroundtime = value;
                NotifyPropertyChanged("TurnAroundTime");
            }
        }

        private double _totalPaxWeight;
        public double TotalPaxWeight
        {
            get
            {
                return _totalPaxWeight;
            }
            set
            {
                _totalPaxWeight = value;
                NotifyPropertyChanged("TotalPaxWeight");
                NotifyPropertyChanged("MTOW");
                NotifyPropertyChanged("MLW");
            }
        }

        public double ACEmptyWeight { get; set; }

        public double LegFlightTime
        {
            get
            {
                if (IDX_FromAP > 0 && IDX_ToAP > 0)
                    return (ETA - ETD).TotalMinutes;
                else
                    return 0;
            }
        }

        public double LegFuelWTOther { get; set; }

        public double TotalTripFuelWT { get; set; }


        public double TotalFuelWT
        {
            get
            {
                return LegFuelWT + AltAPFuelWT + ConFuelWT;
            }
        }


        public double AvailWT
        {
            get
            {
                return Math.Round(FromMTOW - MTOW,2);
            }
        }


        public double MTOW
        {
            get
            {
                return TotalPaxWeight +TotalTripFuelWT +  ACEmptyWeight;
            }
        }

        public double MLW
        {
            get
            {
                return MTOW - LegFuelWT;
            }
        }

        public double ResFuelWT { get; set; }

        private double _legFuelWT;
        public double LegFuelWT
        {
            get
            {
                return _legFuelWT;
            }
            set
            {
                _legFuelWT = value;
                NotifyPropertyChanged("LegFuelWT");
            }
        }

        private double _conFuelWT;
        public double ConFuelWT
        {
            get
            {
                return _conFuelWT;
            }
            set
            {
                _conFuelWT = value;
                NotifyPropertyChanged("ConFuelWT");
            }
        }

        private double _altAPFuelWT;
        public double AltAPFuelWT
        {
            get
            {
                return _altAPFuelWT;
            }
            set
            {
                _altAPFuelWT = value;
                NotifyPropertyChanged("AltAPFuelWT");
            }
        }

        public double DistanceToAlt { get; set; }

        public bool ToRefuel { get; set; }

        public bool FromRefuel { get; set; }
        public List<ScheduleLegRes> ResList { get; set; }
        public List<ScheduleLegRes> ResDelList { get; set; }

        public int MaxPax { get; set; }

        public String ResNames
        {
            get
            {
                var resNameLst = ResList.Select(x => x.Reservation).ToList();
                return String.Join(",", resNameLst);
            }
            set
            {

            }
        }

        public String Description
        {
            get
            {
              
                String depTime = ETD.ToShortTimeString();
                String arrivTime = ETA.ToShortTimeString();

                return  ACRegistration+" "+ depTime + " " + FromAP + "-" + arrivTime + " " + ToAP;
            }
        }

        public bool IsNew
        {
            get
            {
                return IDX < 1;
            }
        }

        public void AddGroup(ScheduleGroup newGroup)
        {
            if (newGroup!=null)
            {
                var  IsChildBooking = IsSplitBooking(newGroup);
                var grpPaxCount = newGroup.NumPax;

                if (MTOW+newGroup.PaxWeight > FromMTOW)
                {
                    FailWindow.Display("Total weight exceeds FROM MTOW");
                    return;
                }

                if (grpPaxCount+NumPax> MaxPax)
                {
                    FailWindow.Display("Pax count exeeds max pax limit of aircraft");
                    return;
                }

                if (this.ETD < newGroup.EarlyEx)
                {
                    FailWindow.Display("ETD is before earliest ex for group:" + Environment.NewLine + newGroup.Name);
                    return;
                }


                if (this.ETA > newGroup.LatestFor)
                {
                    FailWindow.Display("ETA is after latest for for group :" + Environment.NewLine + newGroup.Name);
                    return;
                }
     

                if (newGroup.SoleUse && ResList.Count > 0 && !IsChildBooking)
                {
                    FailWindow.Display("Can not schedule a sole use booking on a leg which already has groups scheduled.");
                    return;
                }

                var soleUseBooking = ResList.FirstOrDefault(x => x.SoleUse == true);
                if (soleUseBooking == null || IsChildBooking)
                {
                    if (ResList.FirstOrDefault(x => x.IDX_Boooking == newGroup.IDX_RL) == null)
                    {
                        var newSchedRes = new ScheduleLegRes();
                        newSchedRes.Reservation = newGroup.ReservationName;
                        newSchedRes.IDX_Reservation = newGroup.IDX_RH;
                        newSchedRes.IDX_Boooking = newGroup.IDX_RL;
                        newSchedRes.NumPax = newGroup.NumPax;
                        newSchedRes.PaxWeight = newGroup.PaxWeight;
                        newSchedRes.SoleUse = newGroup.SoleUse;
                        ResList.Add(newSchedRes);
                        NotifyPropertyChanged("PaxCount");
                        NotifyPropertyChanged("ResNames");
                    }
                    else
                        FailWindow.Display("Reservation " + newGroup.ReservationName + " has already been scheduled on this leg");
                }
                else if (!IsChildBooking)
                    FailWindow.Display("This leg already has a sole use booking, no more groups can be scheduled on it.");
            }

        }

        public void RemoveGroup(int ReservationIDX)
        {
            var grpItem = ResList.FirstOrDefault(x => x.IDX_Reservation == ReservationIDX);
            if (grpItem!=null)
            {
                grpItem.IsDeleted = true;
                ResList.Remove(grpItem);
                if (!grpItem.IsNew)
                    ResDelList.Add(grpItem);

                NotifyPropertyChanged("PaxCount");
                NotifyPropertyChanged("ResNames");
            }

        }

        public void RemoveAllGroups()
        {
            foreach (var resGrp in ResList)
            {
                resGrp.IsDeleted = true;
                if (!resGrp.IsNew)
                    ResDelList.Add(resGrp);
            }

            ResList.Clear();
            NotifyPropertyChanged("PaxCount");
            NotifyPropertyChanged("ResNames");
        }

        public ScheduleLeg()
        { 
            ResList = new List<ScheduleLegRes>();
            ResDelList = new List<ScheduleLegRes>();
        }

        private bool IsSplitBooking(ScheduleGroup newGroup)
        {
            var allResIDS = ResList.Select(x => x.IDX_Reservation).ToList();

            if (newGroup.ParentIDX.HasValue && allResIDS.Contains(newGroup.ParentIDX.Value))
                return true;

            return false;
        }

        public static explicit operator ScheduleLeg(tsch_Legs tsch_leg)
        {
            var schedLeg = new ScheduleLeg();
            schedLeg.IDX_AC_Pilot = tsch_leg.IDX_AC_Pilot;
            schedLeg.IDX_FromAP = tsch_leg.FromAP;
            schedLeg.IDX_ToAP = tsch_leg.ToAP;
            schedLeg.ETA = tsch_leg.ETA;
            schedLeg.ETD = tsch_leg.ETD;
            schedLeg.TurnAroundTime = tsch_leg.Turnaround ?? 0;
            //schedLeg.NumPax = tsch_leg.NumPax;
            schedLeg.Distance = tsch_leg.DirectDistance;
            schedLeg.Budget = tsch_leg.Budget;
            schedLeg.GameFT = tsch_leg.GameflightTime;
            schedLeg.MaxLDW = tsch_leg.MaxLandingWeight ?? -1;
            schedLeg.MaxTOW = tsch_leg.MaxTakeOffWeight ?? -1;
            schedLeg.IDX = tsch_leg.IDX;

            foreach (var legRes in tsch_leg.tsch_LegsRes)
            {
                var schedLegRes = (ScheduleLegRes)legRes;
                schedLeg.ResList.Add(schedLegRes);
            }

            return schedLeg;

        }


        public static explicit operator tsch_Legs(ScheduleLeg schedLeg)
        {
            var tsch_leg = new tsch_Legs();

            tsch_leg.IDX_AC_Pilot = schedLeg.IDX_AC_Pilot;
            tsch_leg.FromAP = schedLeg.IDX_FromAP;
            tsch_leg.ToAP = schedLeg.IDX_ToAP;
            tsch_leg.ETA = schedLeg.ETA;
            tsch_leg.ETD = schedLeg.ETD;
            tsch_leg.NumPax = Convert.ToInt16(schedLeg.NumPax);
            tsch_leg.DirectDistance = Convert.ToInt16(schedLeg.Distance);
            tsch_leg.Budget = schedLeg.Budget;
            tsch_leg.GameflightTime = schedLeg.GameFT;
            tsch_leg.MaxLandingWeight = schedLeg.MaxLDW;
            tsch_leg.MaxTakeOffWeight = schedLeg.MaxTOW;
            tsch_leg.Turnaround = schedLeg.TurnAroundTime;
            return tsch_leg;
        }


        public void RefreshDestinationFields()
        {
            NotifyPropertyChanged("Distance");
            NotifyPropertyChanged("TurnAroundTime");
            NotifyPropertyChanged("AltAP");
            NotifyPropertyChanged("GameFT");
        }

        public double CalculatePassengerWT(double pilotWeight)
        {
            return ResList.Sum(x => x.PaxWeight) + ResList.Sum(x => x.LugWeight) + pilotWeight;

        }



        public void CalculateLegFuelWT(double fuelFlow, double acSpeed)
        {

            Debug.WriteLine("Start CalculateLegFuelWT");
            if (LegFlightTime > 0)
                LegFuelWT = GetLegFuelWeight(fuelFlow);
    
            ConFuelWT = Math.Round(LegFuelWT * 0.05,2);

            AltAPFuelWT = CalculateAltFuelWeight(fuelFlow, acSpeed);

            Debug.WriteLine("End CalculateLegFuelWT");
        }

        public double GetOtherWeight()
        {
            return LegFuelWT + AltAPFuelWT + ConFuelWT+LegFuelWTOther;
        }


        public double GetFlightTime(double distance, double acSpeed)
        {

            if (acSpeed > 0)
                return (distance / acSpeed) * 60;
            else
                return 0;

        }

        public double GetLegFuelWeight(double fuelFlow)
        {
            return Math.Round(fuelFlow * (LegFlightTime / 60), 2); ;
        }

        private double CalculateAltFuelWeight(double fuelFlow, double acSpeed)
        {
            Debug.WriteLine("Start CalculateLegFuelWT");
            double tmp_time = 0.0;
            double fuelWT = 0.0;

            tmp_time = GetFlightTime(DistanceToAlt, acSpeed);
            if (tmp_time > 0)
                fuelWT = fuelFlow * (tmp_time / 60);

            Debug.WriteLine("End CalculateLegFuelWT");

            return Math.Round(fuelWT, 2);

           
        }


    }
}


