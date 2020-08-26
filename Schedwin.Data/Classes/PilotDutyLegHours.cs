using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class PilotDutyLegHours
    {
       
        public DateTime FlightDate { get; set; }

        public int PilotIDX { get; set; }

        public String FirstName { get; set; }

        public String Surname{ get; set; }

        public DateTime DutyStart { get; set; }

        public DateTime DutyEnd { get; set; }

        public String DutyType { get; set; }

        public int LegCount {get;set;}

        public DateTime AllowableHours { get; set; }

        public double FlightHours { get; set; }

        public double Accumulated30DayDuty { get; set; }
        public double Accumulated30DayFlightHours { get; set; }

        public double AccumulatedAllowable30DayFlightHours { get; set; }

        public double AccumulatedAllowable30DayDutyHours { get; set; }

        public double Accumulated7DayDuty { get; set; }  
     
        public double Accumulated7DayFlightHours { get; set; }

        public double AccumulatedAllowable7dayduty{ get; set; }

        public double AccumulatedAllowable7DayFlightHours { get; set; }

      

        public double DutyHours
        {
            get
            {
                return (DutyEnd - DutyStart).TotalHours;
            }
        }


        public PilotDutyLegHours()
        {
            DutyStart = new DateTime(2010, 01, 01, 8, 0, 0);
            DutyEnd = new DateTime(2010, 01, 01, 12, 0, 0);
        }

        public PilotDutyLegHours(List<tsch_AC_Pilot> pilotLegs)
        {
            FlightDate = pilotLegs.First().FlightDate;
            FirstName = pilotLegs.First().tset_Personnel.Firstname;
            Surname = pilotLegs.First().tset_Personnel.Surname;
            var legs = pilotLegs.SelectMany(x => x.tsch_Legs).ToList();
            LegCount = legs.Count();
            if (LegCount>0)
            {
                DutyStart = legs.Min(x => x.ETD).AddHours(-1);
                DutyEnd = legs.Max(x => x.ETA).AddMinutes(30);

                foreach (var leg in legs)
                {
                    FlightHours += (leg.ETA - leg.ETD).TotalHours;
                }
            }
            else
            {
                DutyStart = FlightDate.AddHours(8);
                DutyEnd = FlightDate.AddHours(12);
            }

        }


        //public static implicit operator PilotDutyLegHours(tsch_AC_Pilot acPilot)
        //{
        //    var pilotDutyObj = new PilotDutyLegHours();
        //    pilotDutyObj.FlightDate = acPilot.FlightDate;
        //    pilotDutyObj.FirstName = acPilot.tset_Personnel.Firstname;
        //    pilotDutyObj.Surname = acPilot.tset_Personnel.Surname;
        //    pilotDutyObj.DutyStart = acPilot.tsch_Legs.Min(x => x.ETD).AddHours(-1);
        //    pilotDutyObj.DutyEnd = acPilot.tsch_Legs.Max(x => x.ETA).AddMinutes(30);
        //    pilotDutyObj.LegCount = acPilot.tsch_Legs.Count();

        //    foreach (var leg in acPilot.tsch_Legs)
        //    {
        //        pilotDutyObj.FlightHours += (leg.ETA - leg.ETD).TotalHours;
        //    }

        //    return pilotDutyObj;
        //}

            
        public static async Task<String> CanSchedule(int idxPilot,  DateTime Date, String Server, string regionalDBName)
        {
            List<PilotDutyLegHours> listDutyHours = null;
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                ctx.Database.CommandTimeout = 60;
                var dataStartDate = Date.AddDays(-31);
                var dutyRoster = await ctx.tsch_PilotRoster.Include("tset_PilotsDetails").FirstOrDefaultAsync(x => x.tset_PilotsDetails.IDX_Personnel == idxPilot && x.Date == Date);

                if (dutyRoster != null)
                {
                    if (dutyRoster.Type=="1")
                    {
                        var allowableMatrx = ctx.tset_AllowbleHours.OrderByDescending(x => x.Start_Time).ToList();
                        var allowableDuties = ctx.tset_AllowableDutyHours.ToList();
                        var pilotLegData = await ctx.tsch_AC_Pilot.Include("tsch_legs").Include("tset_Personnel").
                                                               Where(x => x.FlightDate >= dataStartDate && x.FlightDate <= Date && x.tset_Personnel.IDX == idxPilot)
                                                               .ToListAsync();
                        var pilotLegDataGrped = pilotLegData.GroupBy(x => x.FlightDate).ToList();
                        listDutyHours = pilotLegDataGrped.Select(x => new PilotDutyLegHours(x.ToList())).ToList();
                        var flightDateDutyHours = listDutyHours.FirstOrDefault(x => x.FlightDate == Date);
                        int flightDateLegCount = flightDateDutyHours != null ? flightDateDutyHours.LegCount : 0;

                        var day7Start = Date.AddDays(-7);
                        var day30Start = Date.AddDays(-30);

                        var lst7Days = listDutyHours.Where(x => x.FlightDate >= day7Start && x.FlightDate < Date).ToList();
                        var lst30days = listDutyHours.Where(x => x.FlightDate >= day30Start && x.FlightDate < Date).ToList();

                        double accumulated7DayDuty = lst7Days.Sum(x => x.DutyHours);
                        double accumulatedAllowable7dayduty = Convert.ToDouble(allowableDuties.First(x => x.Is_Duty_Hours == 1 && x.History_Period == 7).Allowable_Hours);
                        double accumulated7DayFlightHours = lst7Days.Sum(x => x.FlightHours);
                        double accumulatedAllowable7DayFlightHours = allowableDuties.First(x => x.Is_Duty_Hours == 0 && x.History_Period == 7).Allowable_Hours;


                        double accumulated30DayDuty = lst30days.Sum(x => x.DutyHours);
                        double accumulated30DayFlightHours = lst30days.Sum(x => x.FlightHours);
                        double accumulatedAllowable30DayDutyHours = allowableDuties.First(x => x.Is_Duty_Hours == 1 && x.History_Period == 30).Allowable_Hours;
                        double accumulatedAllowable30DayFlightHours = allowableDuties.First(x => x.Is_Duty_Hours == 0 && x.History_Period == 30).Allowable_Hours;

                        if (accumulated7DayFlightHours > accumulatedAllowable7DayFlightHours)
                            return "Pilot will have exceeded allowable flight hours for the past 7 days";
                        if (accumulated30DayFlightHours > accumulatedAllowable30DayDutyHours)
                            return "Pilot will have exceeded allowable flight hours for the past 30 days";

                        var allowbleHours = allowableMatrx.FirstOrDefault(x => x.Start_Time <= Date);
                        int workableHours = 0;
                        if (allowbleHours != null)
                        {
                            switch (flightDateLegCount)
                            {
                                case 0: workableHours = allowbleHours.C0.Value.Hours; break;
                                case 1: workableHours = allowbleHours.C1.Value.Hours; break;
                                case 2: workableHours = allowbleHours.C2.Value.Hours; break;
                                case 3: workableHours = allowbleHours.C3.Value.Hours; break;
                                case 4: workableHours = allowbleHours.C4.Value.Hours; break;
                                case 5: workableHours = allowbleHours.C5.Value.Hours; break;
                                case 6: workableHours = allowbleHours.C6.Value.Hours; break;
                                case 7: workableHours = allowbleHours.C7.Value.Hours; break;
                                case 8: workableHours = allowbleHours.C8.Value.Hours; break;
                            }

                            if (flightDateDutyHours!=null && flightDateDutyHours.FlightHours > workableHours)
                                return "Pilot flying hours exceeds daily allowance";
                        }
                        return "";
                    }
                    else
                    {
                        switch (dutyRoster.Type)
                        {
                            case "0": return "Pilot rostered for office duty on this day";
                            case "L": return "Pilot rostered for leave on this day";
                            case "T": return "Pilot rostered for training on this day";
                            default: return "";
                        }

                    }
                       

                }
                else
                    return "No duty roster entry found";
                
            }  
        }
        public static List<PilotDutyLegHours> GetDutyHoursReport(int idxPilot, DateTime startDate, DateTime endDate , String Server, string regionalDBName)
        {
            List<PilotDutyLegHours> listDutyHours = null;
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dataStartDate = startDate.AddDays(-31);
                var dutyRoster = ctx.tsch_PilotRoster.Include("tset_PilotsDetails").Where(x => x.tset_PilotsDetails.IDX_Personnel == idxPilot && x.Date >= startDate && x.Date <= endDate).ToList();
                var allowableMatrx = ctx.tset_AllowbleHours.OrderByDescending(x=>x.Start_Time).ToList();
                var allowableDuties = ctx.tset_AllowableDutyHours.ToList();
                var pilotLegData = ctx.tsch_AC_Pilot.Include("tsch_legs").Include("tset_Personnel").
                                                       Where(x => x.FlightDate>= dataStartDate && x.FlightDate<=endDate && x.tset_Personnel.IDX== idxPilot)
                                                       .ToList();
                var pilotLegDataGrped = pilotLegData.GroupBy(x => x.FlightDate).ToList();
                listDutyHours = pilotLegDataGrped.Select(x => new PilotDutyLegHours(x.ToList())).ToList();
                var dateIndex = startDate;
                do
                {
                    var rosterEntry = dutyRoster.FirstOrDefault(x => x.Date == dateIndex);
                    var dutyhour = listDutyHours.FirstOrDefault(x => x.FlightDate == dateIndex);
                    if (dutyhour == null)
                    {
                        dutyhour = new PilotDutyLegHours();
                        dutyhour.FlightDate = dateIndex;
                        listDutyHours.Add(dutyhour);
                    }

                    if (rosterEntry != null)
                    {
                        switch (rosterEntry.Type)
                        {
                            case "0":  dutyhour.DutyType = "Office";
                                                  dutyhour.DutyStart = new DateTime(dateIndex.Year, dateIndex.Month, dateIndex.Day, 9, 0, 0);
                                                  dutyhour.DutyEnd = new DateTime(dateIndex.Year, dateIndex.Month, dateIndex.Day, 18, 0, 0);
                                                  break;
                            case "L":   dutyhour.DutyType = "Leave";
                                        dutyhour.DutyStart = new DateTime(dateIndex.Year, dateIndex.Month, dateIndex.Day, 0, 0, 0);
                                        dutyhour.DutyEnd = new DateTime(dateIndex.Year, dateIndex.Month, dateIndex.Day,0, 0, 0);
                                        break;
                            case "1":  dutyhour.DutyType = "Flight"; break;
                            case "T":  dutyhour.DutyType = "Training"; break;

                        }
                    }
                   
                        var day7Start = dutyhour.FlightDate.AddDays(-7);
                        var day30Start = dutyhour.FlightDate.AddDays(-30);

                        var lst7Days = listDutyHours.Where(x => x.FlightDate >= day7Start && x.FlightDate < dutyhour.FlightDate).ToList();
                        var lst30days = listDutyHours.Where(x => x.FlightDate >= day30Start && x.FlightDate < dutyhour.FlightDate).ToList();


                        dutyhour.Accumulated7DayDuty = lst7Days.Sum(x => x.DutyHours);
                        dutyhour.AccumulatedAllowable7dayduty = Convert.ToDouble(allowableDuties.First(x => x.Is_Duty_Hours == 1 && x.History_Period == 7).Allowable_Hours);
                        dutyhour.Accumulated7DayFlightHours = lst7Days.Sum(x => x.FlightHours);
                        dutyhour.AccumulatedAllowable7DayFlightHours = allowableDuties.First(x => x.Is_Duty_Hours == 0 && x.History_Period == 7).Allowable_Hours;


                        dutyhour.Accumulated30DayDuty = lst30days.Sum(x => x.DutyHours);
                        dutyhour.Accumulated30DayFlightHours = lst30days.Sum(x => x.FlightHours);
                        dutyhour.AccumulatedAllowable30DayDutyHours = allowableDuties.First(x => x.Is_Duty_Hours == 1 && x.History_Period == 30).Allowable_Hours;
                        dutyhour.AccumulatedAllowable30DayFlightHours = allowableDuties.First(x => x.Is_Duty_Hours == 0 && x.History_Period == 30).Allowable_Hours;


                        var allowbleHours = allowableMatrx.FirstOrDefault(x => x.Start_Time <= dutyhour.DutyStart);
                        if (allowbleHours != null)
                        {
                            switch (dutyhour.LegCount)
                            {
                                case 0: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C0.Value; break;
                                case 1: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C1.Value; break;
                                case 2: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C2.Value; break;
                                case 3: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C3.Value; break;
                                case 4: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C4.Value; break;
                                case 5: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C5.Value; break;
                                case 6: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C6.Value; break;
                                case 7: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C7.Value; break;
                                case 8: dutyhour.AllowableHours = new DateTime(2000, 01, 01) + allowbleHours.C8.Value; break;
                            }
                        }
                  

                    dateIndex = dateIndex.AddDays(1);
                } while (dateIndex <= endDate);

            }


            return listDutyHours.OrderBy(x=>x.FlightDate).Where(x => x.FlightDate >= startDate && x.FlightDate <= endDate).ToList();
        }

    }
}
