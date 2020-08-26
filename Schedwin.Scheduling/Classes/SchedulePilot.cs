using Schedwin.Common;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.Scheduling.Classes
{
    public class ScheduleACPilot : ViewModelBase
    {

        public bool IsDeleted { get; set; }
        public int IDX { get; set; }

        public int RowNumber { get; set; }

        public DateTime FlightDate { get; set; }


        public String Comment { get; set; }

        private double? _pilotWeight;
        public double? PilotWeight
        {
            get
            {
                return _pilotWeight;
            }
            set
            {
                _pilotWeight = value;
                NotifyPropertyChanged("PilotWeight");
            }
        }

        public Brush Pilot1ESTFTColor
        {
            get
            {
                if (Pilot1EstFT > 90)
                    return Brushes.Red;
                else if (Pilot1EstFT >=85 && Pilot1EstFT <90)
                    return Brushes.Orange;

                return null;
            }
        }

        private double? _pilot1EST;
        public double? Pilot1EstFT
        {
            get
            {
                if (_pilot1EST != null)
                    return Math.Round(_pilot1EST.Value, 2);
                else
                    return _pilot1EST;

            }
            set
            {
                _pilot1EST = value;
                NotifyPropertyChanged("Pilot1EstFT");
                NotifyPropertyChanged("Pilot1ESTFTColor");
            }
        }

        public Brush Pilot2ESTFTColor
        {
            get
            {
                if (Pilot1EstFT > 90)
                    return Brushes.Red;
                else if (Pilot1EstFT >= 85 && Pilot1EstFT < 90)
                    return Brushes.Orange;

                return null;
            }
        }

        private double? _pilot2EST;
        public double? Pilot2EstFT
        {
            get
            {
                if (_pilot2EST != null)
                    return Math.Round(_pilot2EST.Value, 2);
                else
                    return _pilot2EST;
            }
            set
            {
                _pilot2EST = value;
                NotifyPropertyChanged("Pilot2EstFT");
                NotifyPropertyChanged("Pilot2ESTFTColor");
            }
        }

        public int IDX_Aircraft { get; set; }


        public String _registration;
        public String AircraftRegistration
        {
            get
            {
                return _registration;
            }
            set
            {
                _registration = value;
                NotifyPropertyChanged("AircraftRegistration");
            }
        }

        public int IDX_AircraftAP { get; set; }

        private String _aircraftAP;
        public String AircraftAP
        {
            get
            {
                return _aircraftAP;
            }
            set
            {
                _aircraftAP = value;
                NotifyPropertyChanged("AircraftAP");
            }
        }

        public int IDX_AircraftType { get; set; }


        private String _aircraftType;
        public String AircraftType
        {
            get
            {
                return _aircraftType;
            }
            set
            {
                _aircraftType = value;
                NotifyPropertyChanged("AircraftType");
            }
        }

        public Brush ServiceColor
        {
            get
            {
                if (Pax > 0 && Pax < 7)
                {
                    if (AircraftService >= 47 && AircraftService <= 53)
                        return Brushes.LightGreen;
                    else if (AircraftService >= 97)
                        return Brushes.Red;
                }
                else
                {
                    if (AircraftService >= 23 && AircraftService <= 177)
                        return Brushes.LightGreen;
                    else if (AircraftService > 195)
                        return Brushes.Red;
                        
                }
                return Brushes.LightGreen;
            }
        }

        private double _aircraftService;
        public double AircraftService
        {
            get
            {
                return _aircraftService;
            }
            set
            {
                _aircraftService = value;
                NotifyPropertyChanged("AircraftService");
                NotifyPropertyChanged("ServiceColor");
            }


        }
        public int? IDX_Pilot_1 { get; set; }

        private String _pilot1Name;
        public String Pilot1Name
        {
            get
            {
                return _pilot1Name;
            }
            set
            {
                _pilot1Name = value ;
                NotifyPropertyChanged("Pilot1Name");
            }
        }


        private String _pilot2Name;

        public String Pilot2Name
        {
            get
            {
                return _pilot2Name;
            }
            set
            {
                _pilot2Name = value;
                NotifyPropertyChanged("Pilot2Name");
            }
        }
        public int? IDX_PilotAP_1 { get; set; }

        private String _pilot1AP;
        public String Pilot1_AP
        {
            get
            {
                return _pilot1AP;
            }
            set
            {
                _pilot1AP = value;
                NotifyPropertyChanged("Pilot1_AP");
            }
        }

        public String PilotType_1 { get; set; }

        public int IDX_PilotType_1 { get; set; }

        private String _pilot2AP;

        public String Pilot2_AP
        {
            get
            {
                return _pilot2AP;
            }
            set
            {
                _pilot2AP = value;
                NotifyPropertyChanged("Pilot2_AP");
            }
        }

        public String PilotType_2 { get; set; }



        public int? IDX_PilotType_2 { get; set; }

        private int? idx_pilot_2;
        public int? IDX_Pilot_2
        {
            get
            {
                return idx_pilot_2;
            }
            set
            {
                idx_pilot_2 = value;
                NotifyPropertyChanged("IDX_Pilot_2");
            }
        }

        private int? idx_pilotAP_2;
        public int? IDX_PilotAP_2
        {
            get
            {
                return idx_pilotAP_2;
            }
            set
            {
                idx_pilotAP_2 = value;
                NotifyPropertyChanged("IDX_PilotAP_2");
            }
        }

        public int tmp_RevisionNo { get; set; }

        public String Notes { get; set; }

        public int TechLogID { get; set; }

        public  RangeObservableCollection<ScheduleLeg> Legs { get; set; }

        public List<ScheduleLeg> DeletedLegs { get; set; }

        private double _aircraftWeight;
        public double AircraftWeight
        {
            get
            {
                return _aircraftWeight;
            }
            set
            {
                _aircraftWeight = value;
                NotifyPropertyChanged("AircraftWeight");
            }
        }

        public double BuyRate { get; set; }

        public double SellRate { get; set; }

        private float _rangeKM;

        public float RangeKM
        {
            get
            {
                return _rangeKM;
            }
            set
            {
                _rangeKM = value;
                NotifyPropertyChanged("RangeKM");
            }
        }

        private float _rangeHours;

        public float RangeHours
        {
            get
            {
                return _rangeHours;
            }
            set
            {
                _rangeHours = value;
                NotifyPropertyChanged("RangeHours");
            }
        }

        private int _aircraftSpeed;

        public int AircraftSpeed
        {
            get
            {
                return _aircraftSpeed;
            }

            set
            {
                _aircraftSpeed = value;
                NotifyPropertyChanged("AircraftSpeed");
            }
        }


        private int _Pax;
        public int Pax
        {
            get
            {
                return _Pax;
            }
            set
            {
                _Pax = value;
                NotifyPropertyChanged("Pax");
                NotifyPropertyChanged("ServiceColor");
            }
        }

        public int TurnAroundTime { get; set; }

        public double EmptyWeight { get; set; }

        private int _fuelFlow;
        public int FuelFlow 
        {
            get
            {
                return _fuelFlow;
            }
            set
            {
                _fuelFlow = value;
                NotifyPropertyChanged("FuelFlow");
            }
        }

        public int ReserveFuel { get; set; }

        private int _idxFuelType;
        public int IDX_FuelType
        {
            get
            {
                return _idxFuelType;
            }
            set
            {
                _idxFuelType = value;
                NotifyPropertyChanged("IDX_FuelType");
            }
        }

        public static RangeObservableCollection<AircraftInfo> AircraftList { get; set; }

        public static RangeObservableCollection<PilotInfo> PilotList { get; set; }
        public bool IsNew
        {
           get
            {
                return IDX < 1;
            }
        }

        public ScheduleACPilot()
        {
            Legs = new RangeObservableCollection<ScheduleLeg>();
            DeletedLegs = new List<ScheduleLeg>();
            IDX_Pilot_2 = null;
            IsDeleted = false;
        }

        public bool IsLastLeg(ScheduleLeg thisLeg)
        {
            var tmpLegs = Legs.OrderBy(x => x.ETA).ToList();
            var lastLeg = tmpLegs.Last();
            if (thisLeg.ETD >= lastLeg.ETD)
                return true;
            return false;

        }

        public void RemoveLeg(ScheduleLeg legToRemove)
        {
            Legs.Remove(legToRemove);
 
            if (!legToRemove.IsNew)
                DeletedLegs.Add(legToRemove);

            legToRemove.IsDeleted = true;
        }

        public double GetFlightTime(double distance, double acSpeed)
        {
            if (acSpeed > 0)
                return ((distance / acSpeed) * 60);
            else
                return 0;

        }


 

        public double GetAlternateReFuelWT(int idxAP )

        {

            double tmp_Distance;
            double tmp_FT;


            tmp_Distance = AirportClosestFuel.GetDistance(IDX_FuelType, idxAP, idxAP);


            tmp_FT = GetFlightTime(tmp_Distance, AircraftSpeed);

        if (tmp_FT > 0)
                return (FuelFlow * (tmp_FT / 60));
            else
                return 0;
        
    
        }
        
        public void CalculateFuelWTs()
        {
            CalculateFuelWTs(ReserveFuel);
        }

        public void CalculateFuelWTs(double acResFuel)
        {
            Debug.WriteLine("Begin CalculateFuelWT  Leg Count: " + Legs.Count());
            foreach (var leg in Legs)
            {
                Debug.WriteLine("CalculateLegFuelWT : " + leg.FromAP + "-" + leg.ToAP);
                leg.CalculateLegFuelWT(FuelFlow, AircraftSpeed);
                leg.ResFuelWT = acResFuel;
            }

            Debug.WriteLine("Before RecalculateTripWTS");
            RecalculateTripWTS();
            Debug.WriteLine("End CalculateFuelWT");
        }

        public void ShiftTimes(double mins, ScheduleLeg fromThisLeg)
        {
         
            if (fromThisLeg!=Legs.First())
            {
                int prevLegIndex = Legs.IndexOf(fromThisLeg) - 1;
                Legs[prevLegIndex].TurnAroundTime += Convert.ToInt32(mins);
            }
            else
            {
               
                foreach (var leg in Legs)
                {
                  if (leg!=Legs.First())
                    leg.ETD = leg.ETD.AddMinutes(mins);
                 leg.ETA = leg.ETA.AddMinutes(mins);
                }

            }
       
        }

        public void RecalculateTimes()
        {
         

            var tmpLegs = Legs.OrderBy(x => x.ETD).ToList();
            int legCount = tmpLegs.Count();
            if (legCount>1)
            {
                var currLeg = tmpLegs.First();
                while (currLeg != null)
                {
                    var nextLeg = tmpLegs.FirstOrDefault(x => x.FromAP == currLeg.ToAP);
                    var flghtTimeMins = currLeg.GetFlightTime(currLeg.Distance, AircraftSpeed);
                    var roundedFlightTime = RoundTo5.Round((int)flghtTimeMins);
                    currLeg.ETA = currLeg.ETD.AddMinutes(roundedFlightTime);

                    if (nextLeg != null)
                        nextLeg.ETD = currLeg.ETA.AddMinutes(currLeg.TurnAroundTime);

                    tmpLegs.Remove(currLeg);
                    currLeg = nextLeg;
        
                }
            }
                      
        }


    
        public void RefreshAllLegETAs()
        {
            foreach (var leg in Legs)
            {
                if (AircraftSpeed > 0)
                {
                    int minutes = Convert.ToInt32((double)leg.Distance / (double)AircraftSpeed * 60.0);
                    int outResult;
                    var modVal = Math.DivRem(minutes, 5, out outResult);
                    modVal = (modVal + 1) * 5;
                    minutes += (modVal - minutes);

                    leg.ETA = leg.ETA.AddMinutes(minutes);
                }
            }
        }


        public void RecalculateTripWTS()
        {
           
            if (Legs != null && Legs.Count()>0)
            {
                var startLeg = Legs.First();
                var lastLeg = Legs.Last();
                var endLeg = startLeg;
                int legIndex = -1;
                Debug.WriteLine("Start Leg: " + startLeg.FromAP + "-" + startLeg.ToAP);
                Debug.WriteLine("Last Leg: " + lastLeg.FromAP + "-" + lastLeg.ToAP);
                do
                {

                    endLeg = Legs.FirstOrDefault(x => x.ToRefuel  && Legs.IndexOf(x) > legIndex) ?? Legs.Last();
                    Debug.WriteLine("End Leg: " + endLeg.FromAP + "-" + endLeg.ToAP);
                    RecalculateTripWTS(startLeg, endLeg);
                    legIndex = Legs.IndexOf(endLeg);
                    if (endLeg!=lastLeg)
                    {
                        startLeg = Legs[legIndex + 1];
                        legIndex = Legs.IndexOf(startLeg);
                    }

                }
                while (endLeg != lastLeg);
            }

        }

        public void RecalculateTripWTS(ScheduleLeg startLeg, ScheduleLeg endLeg)
        {

            int startIndex = Legs.IndexOf(startLeg);
            int endIndex = Legs.IndexOf(endLeg);
            int legCount = 1;

            if (startLeg!=endLeg)
                legCount = (endIndex - startIndex)+1;

            Debug.WriteLine("RecalculateTripWTS: start:" + startLeg.FromAP + "-" + startLeg.ToAP +" end:" + endLeg.FromAP + "-" + endLeg.ToAP);

            var subsetLegs = Legs.ToList().GetRange(startIndex, legCount).OrderBy(x=>x.ETD).ToList();

            foreach (var leg in subsetLegs)
            {
                if (leg != Legs.Last())
                    leg.TotalTripFuelWT = leg.TotalFuelWT + subsetLegs.Where(x => x.ETD > leg.ETD).Sum(x => x.TotalFuelWT) +leg.ResFuelWT;
                else
                    leg.TotalTripFuelWT = leg.TotalFuelWT + leg.ResFuelWT; ;
            }
         
        }

        public void RefreshAllLegFuelWeights()
        {
            foreach (var leg in Legs)
            {
                leg.ResFuelWT = ReserveFuel;

                leg.CalculateLegFuelWT(FuelFlow, AircraftSpeed);
            }

        }




        public void UpdateLegData()
        {

            foreach (var leg in Legs)
            {
                var fromAP = AirstripInfo.GetAirstripInfo(leg.IDX_FromAP);
                var apLimits = ACAirportLimits.GetAPLimits(leg.IDX_FromAP, IDX_AircraftType, FlightDate, FlightDate);
                if (fromAP != null)
                {
                    leg.FromAP = fromAP.Code;
                    leg.IDX_FromAP = fromAP.IDX;
                }
                    
                if (apLimits != null)
                    leg.FromMTOW = apLimits.MaxTakeOffWeight;

                var toAP = AirstripInfo.GetAirstripInfo(leg.IDX_ToAP);
                if (toAP != null && fromAP!=null)
                {
                    leg.ToAP = toAP.Code;
                    leg.IDX_ToAP = toAP.IDX;
                    //leg.TurnAroundTime = toAP.TurnAroundTime;

                    var altAP=AirstripInfo.GetAirstripInfo(toAP.IDX_Alt);
                    if (altAP != null)
                    {
                        leg.AltAP = altAP.Code;
                        leg.DistanceToAlt = toAP.AltDistance;
                    }
                        


                }

                apLimits = ACAirportLimits.GetAPLimits(leg.IDX_ToAP, IDX_AircraftType, FlightDate, FlightDate);
                if (apLimits != null)
                    leg.ToMLW = apLimits.MaxLandingWeight;

                var airportFuel = AirportFuel.GetAirportFuel(IDX_FuelType, leg.IDX_ToAP, FlightDate, FlightDate);
                if (airportFuel != null)
                    leg.ToRefuel = true;
                else
                    leg.ToRefuel = false;

                airportFuel = AirportFuel.GetAirportFuel(IDX_FuelType, leg.IDX_FromAP, FlightDate, FlightDate);
                if (airportFuel != null)
                    leg.FromRefuel = true;
                else
                    leg.FromRefuel = false;


                leg.TotalPaxWeight = leg.CalculatePassengerWT(PilotWeight?? 0) ;
                leg.MaxPax = Pax;
                leg.ACMaxWeight = AircraftWeight;
                leg.ACEmptyWeight = EmptyWeight;

            }

       
        }


        public void UpdateLegPaxPilotWeights()
        {
            foreach (var leg in Legs)
            {
                leg.TotalPaxWeight = leg.CalculatePassengerWT(PilotWeight??0);
            }
        }
  
        public tsch_AC_Pilot ToDBTree()
        {
            var tschACPilot = (tsch_AC_Pilot)this;
            foreach (var leg in Legs)
            {
                if (leg.IsComplete)
                {
                    var tschLeg = (tsch_Legs)leg;
                    tschACPilot.tsch_Legs.Add(tschLeg);

                    foreach (var legRes in leg.ResList)
                    {
                        var tschResLeg = (tsch_LegsRes)legRes;
                        tschLeg.tsch_LegsRes.Add(tschResLeg);
                        tschResLeg.tsch_AC_Pilot = tschACPilot;

                    }
                }

            }
            return tschACPilot;
        }

        public static explicit operator tsch_AC_Pilot(ScheduleACPilot scheduleACPIlot)
        {
            var tschACPilot = new tsch_AC_Pilot();
            tschACPilot.FlightDate = new DateTime( scheduleACPIlot.FlightDate.Year, scheduleACPIlot.FlightDate.Month, scheduleACPIlot.FlightDate.Day);
            tschACPilot.Comment = scheduleACPIlot.Comment;
            tschACPilot.IDX_ACDetails = scheduleACPIlot.IDX_Aircraft;
            tschACPilot.IDX_Pilots = scheduleACPIlot.IDX_Pilot_1;
            tschACPilot.IDX_Pilots2 = scheduleACPIlot.IDX_Pilot_2 ?? 0;
            tschACPilot.RevisionNumber =Convert.ToInt16(scheduleACPIlot.tmp_RevisionNo);
            tschACPilot.Printed_This_Revision = false;
            tschACPilot.IDX_Airport_Aircraft = scheduleACPIlot.IDX_AircraftAP;
            tschACPilot.IDX_Airport_Pilot = scheduleACPIlot.IDX_PilotAP_1;
            tschACPilot.IDX_Airport_Pilot2 = scheduleACPIlot.IDX_PilotAP_2;
            tschACPilot.IDX_PilotType = scheduleACPIlot.IDX_PilotType_1;
            tschACPilot.IDX_Pilot2Type = scheduleACPIlot.IDX_PilotType_2;
            tschACPilot.CoCode = "SEFO";
            tschACPilot.IDX_ACTypes = scheduleACPIlot.IDX_AircraftType;
            tschACPilot.TechLogID = scheduleACPIlot.TechLogID;
            tschACPilot.Notes = scheduleACPIlot.Notes;
            tschACPilot.ts_Date = DateTime.Now;
            tschACPilot.IDX_PT = 0;
            tschACPilot.SignedOff = false;
            tschACPilot.IDXUsername = 0;

           return tschACPilot;
        }


        public static explicit operator ScheduleACPilot(tsch_AC_Pilot tschACPilot)
        {
            var scheduleACPIlot = new ScheduleACPilot();
            scheduleACPIlot.IDX = tschACPilot.IDX;
            scheduleACPIlot.FlightDate = tschACPilot.FlightDate;
            scheduleACPIlot.Comment = tschACPilot.Comment;
            scheduleACPIlot.IDX_Aircraft = tschACPilot.IDX_ACDetails;
            scheduleACPIlot.IDX_Pilot_1 = tschACPilot.IDX_Pilots;
            scheduleACPIlot.IDX_Pilot_2= tschACPilot.IDX_Pilots2 != 0 ? tschACPilot.IDX_Pilots2 : (int?)null;
            scheduleACPIlot.tmp_RevisionNo = tschACPilot.RevisionNumber;
            scheduleACPIlot.IDX_AircraftAP = tschACPilot.IDX_Airport_Aircraft;
            scheduleACPIlot.IDX_PilotAP_1 = tschACPilot.IDX_Airport_Pilot;
            scheduleACPIlot.IDX_PilotAP_2 = tschACPilot.IDX_Airport_Pilot2 ?? -1;
            scheduleACPIlot.IDX_PilotType_1 = tschACPilot.IDX_PilotType;
            scheduleACPIlot.IDX_PilotType_2 = tschACPilot.IDX_Pilot2Type ?? -1;
            scheduleACPIlot.IDX_AircraftType = tschACPilot.IDX_ACTypes ??-1;
            scheduleACPIlot.TechLogID = tschACPilot.TechLogID ?? 0;
            scheduleACPIlot.Notes = tschACPilot.Notes;
            var tmpLegs = tschACPilot.tsch_Legs.OrderBy(x => x.ETD).ToList();
            foreach (var tschLeg in tmpLegs)
            {
                var scheduleLeg = (ScheduleLeg)tschLeg;
                scheduleLeg.ETA = new DateTime(scheduleACPIlot.FlightDate.Year, scheduleACPIlot.FlightDate.Month, scheduleACPIlot.FlightDate.Day, scheduleLeg.ETA.Hour, scheduleLeg.ETA.Minute, scheduleLeg.ETA.Second);
                scheduleLeg.ETD = new DateTime(scheduleACPIlot.FlightDate.Year, scheduleACPIlot.FlightDate.Month, scheduleACPIlot.FlightDate.Day, scheduleLeg.ETD.Hour, scheduleLeg.ETD.Minute, scheduleLeg.ETD.Second);
                scheduleACPIlot.Legs.Add(scheduleLeg);
            }

            return scheduleACPIlot;
        }

        public String GetFinalDestination()
        {
            var tmpLegs= Legs.OrderByDescending(x => x.ETD).ToList();
            if (tmpLegs.Count > 0)
                return Legs.First().ToAP;
            else
                return "";
        }
        public async Task UpdateTechLogID(String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tschACPilot = await ctx.tsch_AC_Pilot.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tschACPilot != null)
                {
                    tschACPilot.TechLogID = TechLogID;
                    await ctx.SaveChangesAsync();
                }

            }
        }

    }
}
