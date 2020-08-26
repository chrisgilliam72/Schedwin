using Schedwin.Data;
using System;
using Schedwin.Common;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data.Classes;

namespace Schedwin.Scheduling.Classes
{
    public class ScheduleDutyRoster
    {

        public List<_30DayPilotDutyRoster> ScheduleRosters { get; set; }

        ScheduleDutyRoster()
        {
            ScheduleRosters = new List<_30DayPilotDutyRoster>();
        }

        public _30DayPilotDutyRoster GetPilotRoster(int pilotIDX)
        {
            return ScheduleRosters.FirstOrDefault(x => x.IDX_Pilot == pilotIDX);
        }


        public String CanSchedule(int pilotIDX,DateTime scheduleDate, DateTime startTime, DateTime endTime)
        {
            var pilotRoster = GetPilotRoster(pilotIDX);
            if (pilotRoster != null)
            {
                var actualDate = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day);
                var dutyType = pilotRoster.DutyTypes.FirstOrDefault(x => x.Date == actualDate);
                if (dutyType!=null)
                {
                    switch (dutyType.Duty)
                    {
                        case "0": return "Pilot can not be scheduled because he is rostered for office duty on this day.";
                        case "T": return "Pilot can not be scheduled as he is rostered for training on this day.";
                        case "L": return "Pilot can not be scheduled as he is on leave on this day";
                        default: break;
                    }
                }
               

                double allowable7Dayhrs = AllowableDutyHours.AllowableDuties.FirstOrDefault(x => x.Is_Duty_Hours == 1 && x.History_Period == 7).Allowable_Hours;
                double allowable30Dayhrs = AllowableDutyHours.AllowableDuties.FirstOrDefault(x => x.Is_Duty_Hours == 1 && x.History_Period == 30).Allowable_Hours;
                double currentLeghours = (endTime - startTime).TotalHours;
                if (pilotRoster.FlightHours7Days+ currentLeghours > allowable7Dayhrs)
                    return "Pilot can not be scheduled as flight duty time exceeds 7 day allowable flight duty hours";

                if (pilotRoster.FlightHours30Days+ currentLeghours > allowable30Dayhrs)
                    return "Pilot can not be scheduled as flight duty time exceeds 30 day allowable flight duty hours";
            }

           
            return "";
        }

        public void RemovePilot(int pilotIDX)
        {
            var roster = GetPilotRoster(pilotIDX);
            if (roster != null)
                ScheduleRosters.Remove(roster);
        }

        public void RemoveLeg(int pilotIDX, DateTime date, DateTime startTime, DateTime endTime)
        {
            var pilotRoster = GetPilotRoster(pilotIDX);
            if (pilotRoster!=null)
            {
                var legInfo=pilotRoster.LegDetails.FirstOrDefault(x => x.LegDate == date && x.StartTime == startTime && x.EndTime == endTime);
                if (legInfo != null)
                    pilotRoster.LegDetails.Remove(legInfo);
            }
        }

        public void AddLeg(int pilotIDX, DateTime date, DateTime startTime, DateTime endTime)
        {
            var pilotRoster = GetPilotRoster(pilotIDX);
            if (pilotRoster!=null)
            {
                pilotRoster.AddLeg(date, startTime, endTime);
            }
        }
         
        public async static Task<List<Tuple<int,double>>> GetPilotEFTs(DateTime forDate, List<int> pilotIDXList, bool pilot1, string Server, string regionalDBName)
        {
            var pilotHrsLst = new List<Tuple<int, double>>();
            var dataStartDate = forDate.AddDays(-28);
            var dataEndDate = forDate.AddDays(1);
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                List<tsch_AC_Pilot> pilotLegData = null;

                if (pilot1)
                    pilotLegData = await ctx.tsch_AC_Pilot.Include("tsch_legs").
                                                           Where(x => x.FlightDate >= dataStartDate && x.FlightDate < dataEndDate && x.IDX_Pilots.HasValue && pilotIDXList.Contains(x.IDX_Pilots.Value)).ToListAsync();
                else
                    pilotLegData = await ctx.tsch_AC_Pilot.Include("tsch_legs").
                                            Where(x => x.FlightDate >= dataStartDate && x.FlightDate < dataEndDate && x.IDX_Pilots2.HasValue && pilotIDXList.Contains(x.IDX_Pilots2.Value)).ToListAsync();

                foreach (var pilotIDX in pilotIDXList)
                {
                    List<tsch_AC_Pilot> thisPilotLegData = null;
                    if (pilot1)
                         thisPilotLegData= pilotLegData.Where(x => x.IDX_Pilots == pilotIDX).ToList();
                    else
                        thisPilotLegData = pilotLegData.Where(x => x.IDX_Pilots2 == pilotIDX).ToList();
                    var legs = thisPilotLegData.SelectMany(x => x.tsch_Legs).ToList();
                    int totalTime = legs.Sum(x => (x.ETA - x.ETD).Minutes);
                    var pilotHrs = new Tuple<int, double>(pilotIDX, (double)totalTime / (double)60);
                    pilotHrsLst.Add(pilotHrs);

                }
            }

            return pilotHrsLst;

        }
        public async static Task<double> GetPilotEFT(DateTime forDate, int pilotIDX, string Server, string regionalDBName)
        {
            var dataStartDate = forDate.AddDays(-28);
            var dataEndDate = forDate.AddDays(1);
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var pilotDetails = await ctx.tset_PilotsDetails.FirstOrDefaultAsync(x => x.IDX_Personnel == pilotIDX);

                if (pilotDetails!=null)
                {
                    var pilotLegData = await ctx.tsch_AC_Pilot.Include("tsch_legs").
                                                              Where(x => x.FlightDate >= dataStartDate && x.FlightDate < dataEndDate && x.IDX_Pilots == pilotIDX).ToListAsync();
                    if (pilotLegData!=null && pilotLegData.Count>0)
                    {
                        var legs = pilotLegData.SelectMany(x => x.tsch_Legs).ToList();
                        int totalTime = legs.Sum(x => (x.ETA - x.ETD).Minutes);
                        return (double)totalTime/(double)60;
                    }

                    return 0;
                }

                return 0;
            }
        }

        public async Task<bool> AddPilot(DateTime forDate,int pilotIDX, string Server, string regionalDBName)
        {
            try
            {
                var dataStartDate = forDate.AddDays(-31);
                var dataEndDate = forDate.AddDays(1);
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    Debug.WriteLine("start roster load");

                    
                    var pilotDetails = await ctx.tset_PilotsDetails.FirstOrDefaultAsync(x => x.IDX_Personnel == pilotIDX);

                    var pilotLegData = await ctx.tsch_AC_Pilot.Include("tsch_legs").
                                                             Where(x => x.FlightDate >= dataStartDate && x.FlightDate < dataEndDate && x.IDX_Pilots == pilotIDX).ToListAsync();


                    var rosterEntry = new _30DayPilotDutyRoster();
                    rosterEntry.ScheduleDate = forDate;
                    rosterEntry.IDX_Pilot = pilotIDX;


                    foreach (var leg in pilotLegData)
                    {
                        foreach (var tschLeg in leg.tsch_Legs)
                        {
                            var rosterLegEntry = new RosterLegInfo();
                            rosterLegEntry.LegDate = leg.FlightDate;
                            rosterLegEntry.StartTime = tschLeg.ETD;
                            rosterLegEntry.EndTime = tschLeg.ETA;

                            rosterEntry.LegDetails.Add(rosterLegEntry);
                        }
                    }

                    ScheduleRosters.Add(rosterEntry);

                    var dutyinfos = await ctx.tsch_PilotRoster.Where(x => x.IDX_Pilot== pilotDetails.IDX && x.Date >= dataStartDate && x.Date < dataEndDate).ToListAsync();


                    foreach (var dailyduty in dutyinfos)
                    {
                        var rsterDutyType = new RosterDutyType();
                        rsterDutyType.Date = dailyduty.Date;
                        rsterDutyType.Duty = dailyduty.Type;
                        rosterEntry.DutyTypes.Add(rsterDutyType);
                    }
                    
                }

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                message = "Error loading single pilot roster:" + Environment.NewLine + message;
                throw new Exception(message);
            }

            return true;
        }
      
        public static async Task<ScheduleDutyRoster> BuildRoster(DateTime forDate, List<int> PilotIDXs, string Server, string regionalDBName)
        {
            try
            {
                var roster = new ScheduleDutyRoster();
                var dataStartDate = forDate.AddDays(-31);
                var dataEndDate = forDate.AddDays(1);
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    Debug.WriteLine("start roster load");

                    var pilotLegData = await ctx.tsch_AC_Pilot.Include("tsch_legs").
                                                             Where(x => x.FlightDate >= dataStartDate && x.FlightDate < dataEndDate && x.IDX_Pilots.HasValue 
                                                                    && PilotIDXs.Contains(x.IDX_Pilots.Value)).ToListAsync();


                    Debug.WriteLine("end roster load");

                    var grpedPilotData = pilotLegData.GroupBy(x => x.IDX_Pilots);

                    foreach (var pilotData in grpedPilotData)
                    {
                        var rosterEntry = new _30DayPilotDutyRoster();
                        rosterEntry.ScheduleDate = forDate;
                        rosterEntry.IDX_Pilot = pilotData.Key.Value;

                        var legs = pilotData.OrderBy(x => x.FlightDate).ToList();
                        foreach (var leg in legs)
                        {
                            foreach (var tschLeg in leg.tsch_Legs)
                            {
                                var rosterLegEntry = new RosterLegInfo();
                                rosterLegEntry.IDX_TschLeg = tschLeg.IDX;
                                rosterLegEntry.LegDate = leg.FlightDate;
                                rosterLegEntry.StartTime = tschLeg.ETD;
                                rosterLegEntry.EndTime = tschLeg.ETA;
                                rosterEntry.LegDetails.Add(rosterLegEntry);
                            }
                        }

                        roster.ScheduleRosters.Add(rosterEntry);
                    }

                    var dutyinfos = await ctx.tsch_PilotRoster.Where(x => PilotIDXs.Contains(x.IDX_Pilot) && x.Date>=dataStartDate && x.Date<dataEndDate).ToListAsync();
                    var grpedInfos = dutyinfos.GroupBy(x => x.IDX_Pilot).ToList();
                    foreach (var pilotDutyTypeList in grpedInfos)
                    {
                        var pilotRoster = roster.GetPilotRoster(pilotDutyTypeList.Key);
                        if (pilotRoster != null)
                        {
                            foreach (var dailyduty in pilotDutyTypeList)
                            {
                                var rsterDutyType = new RosterDutyType();
                                rsterDutyType.Date = dailyduty.Date;
                                rsterDutyType.Duty = dailyduty.Type;
                                pilotRoster.DutyTypes.Add(rsterDutyType);
                            }

                        }


                    }

                }

                return roster;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                message = "Error building pilot roster:" + Environment.NewLine + message;
                throw new Exception(message);

            }
        }
    }
}
