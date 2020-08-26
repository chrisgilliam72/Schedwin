using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Schedwin.Techlogs
{
    public class TechlogCaptureViewModel : ViewModelBase
    {
        ScheduleACPilot acSchedule;
        public bool IsNew { get; set; }

        public int IDX { get; set; }

        private bool _canSave;
        public bool CanSave
        {
            get
            {
                return _canSave;
            }
            set
            {
                _canSave = value;
                NotifyPropertyChanged("CanSave");
            }
        }
        public bool NonRevenueTypeEnabled { get; set; }

        public bool NonRevenueDetailEnabled { get; set; }
        public String TechlogDate { get; set; }

        public String Pilot { get; set; }

        public String CoPilot { get; set; }

        public String Aircraft { get; set; }
        public int IDX_Aircraft { get; set; }
        public int? TechLogID { get; set; }

        public int  IDX_Pilot {get;set;}
        public int? IDX_CoPilot { get; set; }

        public double PilotFlightTime { get; set; }

        public double TachStart { get; set; }

        private double _TachEnd;
        public double TachEnd
        {
            get
            {
                return _TachEnd;
            }
            set
            {
                _TachEnd = value;
                UpdatePilotFT();
            }
        }

        public DateTime? DutyStart { get; set; }

        public DateTime? DutyEnd { get;set; }


        public String ScheduleRoute { get; set; }

        private bool _nonRevenue;
        public bool NonRevenue
        {
            get
            {
                return _nonRevenue;
            }
            set
            {
                _nonRevenue = value;
                NonRevenueTypeEnabled = value; ;
                NotifyPropertyChanged("NonRevenueTypeEnabled");
            }
        }


        public int NonRevenueType { get; set; }

        public String MaintenanceDetail{ get; set; }

        public double MaintenanceCost { get; set; }

        public double GameFlightTime { get; set; }

        public int Distance { get; set; }

        public double Speed { get; set; }

        private double _FlightTime;
        public double FlightTime
        {
            get
            {
                return _FlightTime;
            }
            set
            {
                _FlightTime = value;
                UpdateSpeed();
            }
        }
        
        public short Landings { get; set; }

        public String TechLogNotes { get; set; }

        public int Starts { get; set; }

        public String SelectedNonRevenueType { get; set; }

        public RangeObservableCollection<String> NonRevenueTypeList { get; set; }

        public RangeObservableCollection<TechLogFuel> FuelList { get; set; }

        public RangeObservableCollection<AirstripInfo> AirportList { get; set; }

        public TechLogFuel SelectedFuelEntry { get; set; }

        public int IDX_FuelAirportStart { get; set; }
        public int IDX_FuelAirportEnd { get; set; }


        public TechlogCaptureViewModel()
        {
            FuelList = new RangeObservableCollection<TechLogFuel>();
            NonRevenueTypeList = new RangeObservableCollection<string> { "Maintenance", "Ferry", "Business", "Training", "Revenue" };
            AirportList = new RangeObservableCollection<AirstripInfo>();
        }
        

        public void AddFuelEntry()
        {
            if (TechLogID.HasValue)
            {
                var fuelEntry = new TechLogFuel();
                fuelEntry.TechLogID = TechLogID.Value;
                FuelList.Add(fuelEntry);
                NotifyPropertyChanged("FuelList");
            }
            else
                FailWindow.Display("Please enter a techlog ID before capturing fuel lines.");
        }

        public void RemoveFuelEntry()
        {
            if (SelectedFuelEntry != null)
            {
                var fuelEntry = FuelList.FirstOrDefault(x => x.IDX == SelectedFuelEntry.IDX);
                FuelList.Remove(fuelEntry);
                NotifyPropertyChanged("FuelList");
            }
        }

       

        public bool Validate()
        {
            if (!TechLogID.HasValue)
            {
                FailWindow.Display("Techlog number must be captured");
                return false;
            }


            if (TachEnd < TachStart)
            {
                FailWindow.Display("Tach End must be larger than Tech Start");
                return false;
            }

    

            return true;
        }

     

        public async void Init(DateTime? lastTechLogDate,int lastTechLogID, double lastTachEnd, int aircraftID)
        {
          
            IsNew = true;
            AirportList.Clear();
            AirportList.AddRange(AirstripInfo.GetAirstrips());
  
            using (new StackedCursorOverride(Cursors.Wait))
            {
                if (lastTechLogDate.HasValue)
                    acSchedule = await Schedule.LoadNextTechlogSchedule(aircraftID, lastTechLogDate.Value, Server, Database);
                else
                    acSchedule = await Schedule.LoadFirstFlightSchedule(aircraftID,  Server, Database);
            }

            if (acSchedule!=null)
            {
                if (acSchedule.IDX_Pilot_1.HasValue)
                {
                    Pilot = PilotInfo.GetPilotNameFromPersonnelID(acSchedule.IDX_Pilot_1.Value);
                    IDX_Pilot = acSchedule.IDX_Pilot_1.Value;
                }


                if (acSchedule.IDX_Pilot_2.HasValue)
                {
                    CoPilot = PilotInfo.GetPilotNameFromPersonnelID(acSchedule.IDX_Pilot_2.Value);
                    IDX_CoPilot = acSchedule.IDX_Pilot_2.Value;
                }

                Aircraft = AircraftInfo.GetAircraftInfo(aircraftID).Registration;
                IDX_Aircraft = aircraftID;
                TechlogDate = acSchedule.FlightDate.ToShortDateString();

                if (acSchedule.Legs.Count == 1)
                {
                    var leg = acSchedule.Legs.First();
                    var code = AirstripInfo.GetAirstripCode(leg.IDX_FromAP);
                    ScheduleRoute = code + " - ";
                    code = AirstripInfo.GetAirstripCode(leg.IDX_ToAP);
                    Distance = leg.Distance;

                    ScheduleRoute += code;

                }

                else
                {
                    foreach (var acScheduleLeg in acSchedule.Legs)
                    {
                        var code = AirstripInfo.GetAirstripCode(acScheduleLeg.IDX_FromAP);
                        ScheduleRoute = ScheduleRoute+ code + " - ";
                        Distance = Distance + acScheduleLeg.Distance;
                    }
                    var lastLeg = acSchedule.Legs.Last();
                    ScheduleRoute += AirstripInfo.GetAirstripCode(lastLeg.IDX_ToAP);
                }

                DutyStart = acSchedule.Legs.First().ETD;
                DutyEnd = acSchedule.Legs.Last().ETA;
                TachStart = lastTachEnd;
                Landings = Convert.ToInt16(acSchedule.Legs.Count());
                //TechLogID = lastTechLogID + 1;
                NonRevenueTypeEnabled = false;

                NotifyPropertyChanged("NonRevenueTypeEnabled");
                NotifyPropertyChanged("AirportList");
                NotifyPropertyChanged("Distance");
                NotifyPropertyChanged("TachStart");
                NotifyPropertyChanged("DutyStart");
                NotifyPropertyChanged("DutyEnd");
                NotifyPropertyChanged("TechLogID");
                NotifyPropertyChanged("TechlogDate");
                NotifyPropertyChanged("Aircraft");
                NotifyPropertyChanged("Pilot");
                NotifyPropertyChanged("CoPilot");
                NotifyPropertyChanged("ScheduleRoute");
                NotifyPropertyChanged("Landings");
                CanSave = true;
            }
            else
            {
                FailWindow.Display("Unable to load last schedule information");
            }
        }

        public void Refresh (Techlog techLog)
        {
            IsNew = false;
            IDX = techLog.IDX;
            IDX_Pilot = techLog.IDX_Pilot;
            IDX_CoPilot = techLog.IDX_CoPiliot;
            IDX_Aircraft = techLog.IDX_Aicraft;
            TechLogID = techLog.TechLogID;
            Pilot = techLog.Pilot;
            CoPilot = techLog.CoPilot;
            TechlogDate = techLog.TechLogDate.ToShortDateString();
            TachStart = techLog.TachStart;
            TachEnd = techLog.TachEnd;
            Aircraft = techLog.Aircraft;
            DutyStart = techLog.DutyStart;
            DutyEnd = techLog.DutyEnd;
            PilotFlightTime = techLog.PilotFlightTime;
            Starts = techLog.Starts;
            ScheduleRoute = techLog.ScheduledRoute;
            GameFlightTime = techLog.GameFlightTime;
            NonRevenue = techLog.NonRevenue;
  
            MaintenanceDetail = techLog.MaintenanceDetail;
            MaintenanceCost = Math.Round(techLog.MaintenanceCost,2);
            Distance = techLog.Distance;
            Speed = Math.Round(techLog.Speed,2);
            FlightTime = Math.Round(techLog.FlightTime,2);
            Landings = techLog.Landings;
            TechLogNotes = techLog.Notes;
            
            if (techLog.FuelList!=null)
            {
                FuelList.AddRange(techLog.FuelList);
            }

            if (NonRevenue)
            {
                SelectedNonRevenueType = NonRevenueTypeList[techLog.NonRevenueType];
            }

            AirportList.Clear();
            AirportList.AddRange(AirstripInfo.GetAirstrips());

            NotifyPropertyChanged("SelectedNonRevenueType");
            NotifyPropertyChanged("GameFlightTime");
            NotifyPropertyChanged("AirportList");
            NotifyPropertyChanged("FuelList");
            NotifyPropertyChanged("ActualRoute");
            NotifyPropertyChanged("ScheduleRoute");
            NotifyPropertyChanged("NonRevenue");
            NotifyPropertyChanged("NonRevenueType");
            NotifyPropertyChanged("MaintenanceDetail");
            NotifyPropertyChanged("MaintenanceCost");
            NotifyPropertyChanged("Distance");
            NotifyPropertyChanged("Speed");
            NotifyPropertyChanged("FlightTime");
            NotifyPropertyChanged("Landings");
            NotifyPropertyChanged("TechLogNotes");
            NotifyPropertyChanged("CoPilot");
            NotifyPropertyChanged("PilotFlightTime");
            NotifyPropertyChanged("DutyStart");
            NotifyPropertyChanged("DutyEnd");
            NotifyPropertyChanged("Aircraft");
            NotifyPropertyChanged("TachStart");
            NotifyPropertyChanged("TachEnd");
            NotifyPropertyChanged("TechlogDate");
            NotifyPropertyChanged("TechLogID");
            NotifyPropertyChanged("Pilot");
            NotifyPropertyChanged("CoPilot");
            NotifyPropertyChanged("Starts");
            CanSave = true;
        }

        public void UpdateSpeed()
        {
            if (FlightTime>0)
            {
                Speed = Math.Round(Distance / FlightTime, 2);
                NotifyPropertyChanged("Speed");
            }

        }

        public void UpdatePilotFT()
        {
            FlightTime = Math.Round(TachEnd - TachStart,1);
            PilotFlightTime = FlightTime + FlightTime * 0.1;

            NotifyPropertyChanged("PilotFlightTime");
            NotifyPropertyChanged("FlightTime");
        }

        public async Task<bool> Save()
        {

            try
            {
                CanSave = false;
                var techLog = new Techlog();
                techLog.IsNew = IsNew;
                techLog.IDX = IDX;
                techLog.TechLogID = TechLogID.Value;
                techLog.Pilot = Pilot;
                techLog.IDX_Pilot = IDX_Pilot;
                techLog.IDX_CoPiliot = IDX_CoPilot;
                techLog.IDX_Aicraft = IDX_Aircraft;
                techLog.TechLogDate = Convert.ToDateTime(TechlogDate);
                techLog.TachStart = TachStart;
                techLog.TachEnd = TachEnd;
                techLog.Aircraft = Aircraft;
                techLog.DutyStart = DutyStart.Value;
                techLog.DutyEnd = DutyEnd.Value;
                techLog.PilotFlightTime = PilotFlightTime;
                techLog.Starts = Starts;
                techLog.ActualRoute = ScheduleRoute;
                techLog.GameFlightTime = GameFlightTime;
                techLog.ScheduledRoute = ScheduleRoute;
                techLog.NonRevenue = NonRevenue;

            
                if (!String.IsNullOrEmpty(SelectedNonRevenueType))
                {
                    techLog.NonRevenueType = NonRevenueTypeList.IndexOf(SelectedNonRevenueType);
                }
             
                techLog.MaintenanceDetail = MaintenanceDetail;
                techLog.MaintenanceCost = MaintenanceCost;

                techLog.Distance = Distance;
                techLog.Speed = Speed;
                techLog.FlightTime = FlightTime;
                techLog.Landings = Landings;
                techLog.Notes = TechLogNotes; ;

                techLog.FuelList.AddRange(FuelList);


       
                var tempTechLog = await Techlog.LoadTechLog(TechLogID.Value, Server, Database);
                if (tempTechLog != null)
                {
                    if (IsNew)
                    {
                        FailWindow.Display("A techlog with this ID already exists");
                        return false;
                    }
                    else if (tempTechLog.IDX!=IDX)
                    {
                        FailWindow.Display("A techlog with this ID already exists");
                        return false;
                    }
                       
                }
                 


                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await techLog.Save(Server, Database);
                    if (acSchedule!=null)
                    {
                        acSchedule.TechLogID = TechLogID.Value;
                        await acSchedule.UpdateTechLogID(Server, Database);
                    }

                }
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("\"{0}\"  has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                FailWindow.Display(sb.ToString());

                return false;
              
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
            finally
            {
                CanSave = true;
            }
            return true;
        }
    }
}
