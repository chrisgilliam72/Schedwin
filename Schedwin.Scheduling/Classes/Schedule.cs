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

namespace Schedwin.Scheduling.Classes
{
    public class Schedule
    {
        public List<ScheduleACPilot> list_ACPilots { get; set; }

        private DateTime _flightDate;
        public DateTime FlightDate
        {
            get
            {
                return _flightDate;
            }
            set
            {
                _flightDate = value;
                foreach (var pilot in list_ACPilots)
                    pilot.FlightDate = value;
            }
        }
        public Schedule()
        {
            list_ACPilots = new List<ScheduleACPilot>();
        }

        public static async Task<List<LockedScheduleItem>> GetLockedSchedules(String Server, string regionalDBName)
        {
            var lcskedItemList = new List<LockedScheduleItem>();

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tschInuseLst = await ctx.tsch_InUse.ToListAsync();
                foreach (var tschInUse in tschInuseLst)
                {
                    var lckedItem = new LockedScheduleItem();
                    lckedItem.ScheduleDate = tschInUse.FlightDate.ToShortDateString();
                    lckedItem.LockedByUser = tschInUse.Username;
                    lcskedItemList.Add(lckedItem);
                }
            }

            return lcskedItemList;
        }

        public async Task Lock(String currentUser, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tschInUse = await ctx.tsch_InUse.FirstOrDefaultAsync(x => x.FlightDate == FlightDate && x.Username == currentUser);
                if (tschInUse==null)
                {
                    tschInUse = new tsch_InUse();
                    tschInUse.FlightDate = FlightDate;
                    tschInUse.Username = currentUser;
                    ctx.tsch_InUse.Add(tschInUse);

                    await ctx.SaveChangesAsync();
                }

            }
        }

        public static async Task Unlock(String currentUser, List<DateTime> lstDates, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                ctx.tsch_InUse.RemoveRange(ctx.tsch_InUse.Where(x=>x.Username==currentUser && lstDates.Contains(x.FlightDate)));
                await ctx.SaveChangesAsync();
            }

        }
        public static async Task Unlock(String currentUser, DateTime flightDate, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                ctx.tsch_InUse.RemoveRange(ctx.tsch_InUse.Where(x => x.FlightDate == flightDate && x.Username == currentUser));
                await ctx.SaveChangesAsync();
            }
        }



        public static async Task Unlock(String currentUser,  String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                ctx.tsch_InUse.RemoveRange(ctx.tsch_InUse.Where(x => x.Username == currentUser));
                await ctx.SaveChangesAsync();
            }
        }

        public List<ScheduleLeg> GetReservationLegs(int idxResLeg)
        {
            var foundLegs = new List<ScheduleLeg>();
            foreach (var acPilot in list_ACPilots)
            {
                foreach (var leg in acPilot.Legs)
                {
                    var LegIDs = leg.ResList.Select(x => x.IDX_Boooking).ToList();
                    if (LegIDs.Contains(idxResLeg))
                        foundLegs.Add(leg);
                }
            }
      
            return foundLegs;
        }

        public List<ScheduleLeg> GetPilotLegs(int pilotAC_IDX)
        {
            return list_ACPilots.SelectMany(x => x.Legs).Where(x => x.IDX_AC_Pilot == pilotAC_IDX).OrderBy(x=>x.ETD).ToList();
        }


        public async Task<bool> Save(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var newPilots = list_ACPilots.Where(x => x.IsNew && !x.IsDeleted).ToList();
                    var oldPilots = list_ACPilots.Where(x => x.IsNew == false).ToList();
                    var delPilotIDs = list_ACPilots.Where(x => x.IsDeleted && !x.IsNew).Select(x=>x.IDX).ToList();


                 
                    SaveNewPilotSchedules(newPilots, Server, regionalDBName, ctx);
                    UpdatePilotSchedules(oldPilots, Server, regionalDBName, ctx);
                    ctx.tsch_AC_Pilot.RemoveRange(ctx.tsch_AC_Pilot.Where(x => delPilotIDs.Contains(x.IDX)));
                    await ctx.SaveChangesAsync();
                }


                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                throw new Exception(message);
            }
        }

        

        public int GetScheduleRevision()
        {
            if (list_ACPilots != null && list_ACPilots.Count > 0)
                return list_ACPilots.Max(x => x.tmp_RevisionNo);
            else
                return 0;
        }
       
       


        public async Task<bool> UpdateACScheduleServices(DateTime dateFlightDate, String Server, string regionalDBName)
        {
            var acServiceMap = new Dictionary<int, double>();
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                if (list_ACPilots != null && list_ACPilots.Count > 0)
                {
                    var tmpDatesList = new List<DateTime>();
                    var lstACIDs = list_ACPilots.Select(x => x.IDX_Aircraft).ToList();
                    //var lastCapturedDates = await ctx.vl_LastServiceCaptureDate.Where(x => lstACIDs.Contains(x.IDX_AC_Details)).ToListAsync();

                    var lastServiceDates = await ctx.T_Techlog_Services.Where(x => lstACIDs.Contains(x.IDX_Setup_Aircraft_Details) && x.Date <= dateFlightDate).ToListAsync();
                    var grpACDates = lastServiceDates.GroupBy(x => x.IDX_Setup_Aircraft_Details).ToList();

                    foreach (var grp in grpACDates)
                    {
                        tmpDatesList.Add(grp.Max(x => x.Date));
                    }

                    if (lastServiceDates != null && tmpDatesList !=null && tmpDatesList.Count >0)
                    {
                        var earlstCptDate = tmpDatesList.Min(x => x.Date);

                        var techlogsAll = await ctx.t_Techlog.Where(x => x.TechlogDate > earlstCptDate && x.TechlogDate <= dateFlightDate && lstACIDs.Contains(x.IDX_AC_Details)).ToListAsync();
                        if (techlogsAll != null)
                        {
                            var tschLegsAll = await ctx.tsch_Legs.Include("tsch_AC_Pilot").Where(x => x.tsch_AC_Pilot.FlightDate > earlstCptDate
                                                                               && x.tsch_AC_Pilot.FlightDate <= dateFlightDate && lstACIDs.Contains(x.tsch_AC_Pilot.IDX_ACDetails)).ToListAsync();

                            if (tschLegsAll != null)
                            {
                                foreach (var acPilot in list_ACPilots)
                                {

                                    Debug.WriteLine("AC Pilot IDX= "+ acPilot.IDX_Aircraft);
                                    var grpedDate = grpACDates.FirstOrDefault(x => x.Key == acPilot.IDX_Aircraft);
                                    if (grpedDate != null)
                                    {
                                        var lastServiceDate = grpedDate.Max((x => x.Date));
                                        if (lastServiceDate != null)
                                        {
                                            Debug.WriteLine("Before techlog query");
                                            var techlogs = ctx.t_Techlog.Where(x => x.TechlogDate > lastServiceDate.Date && x.TechlogDate <= dateFlightDate && x.IDX_AC_Details == acPilot.IDX_Aircraft).ToList();
                                            Debug.WriteLine("After techlog query");
                                            if (techlogs != null && techlogs.Count > 0)
                                            {
                                               
                                                var lastTechlogDate = techlogs.Max(x => x.TechlogDate);
                                                Debug.WriteLine("last techlogdate:"+ lastTechlogDate.ToShortDateString()); ;
                                                var tschLegs = ctx.tsch_Legs.Include("tsch_AC_Pilot").Where(x => x.tsch_AC_Pilot.FlightDate > lastTechlogDate
                                                                && x.tsch_AC_Pilot.FlightDate <= dateFlightDate && x.tsch_AC_Pilot.IDX_ACDetails == acPilot.IDX_Aircraft).ToList();
                                                var techLogHoursSinceService= techlogs.Sum(x => x.FlightTime);
                                                var predictedHours = tschLegs.Sum(x => (x.ETA.TimeOfDay - x.ETD.TimeOfDay).Hours);
                                                acPilot.AircraftService = Math.Round(techLogHoursSinceService+ predictedHours, 2);

                                            }
                                            else
                                            {
                                                Debug.WriteLine("No techlogs");
                                                var tschLegs = ctx.tsch_Legs.Include("tsch_AC_Pilot").Where(x => x.tsch_AC_Pilot.FlightDate > lastServiceDate.Date
                                                                          && x.tsch_AC_Pilot.FlightDate <= dateFlightDate && x.tsch_AC_Pilot.IDX_ACDetails == acPilot.IDX_Aircraft).ToList();
                                                double hoursSinceService=  tschLegs.Sum(x => (x.ETA.TimeOfDay - x.ETD.TimeOfDay).Hours);
                                                acPilot.AircraftService = Math.Round(hoursSinceService, 2);
                                            }

                                            Debug.WriteLine("After Legs query");
                                         
                                                                                 
                                                                                
                                        }
                                    }
                                    else
                                    {
                                        acPilot.AircraftService = 0.0;
                                        var acInfo = AircraftInfo.GetAircraftInfo(acPilot.IDX_Aircraft);
                                        if (acInfo != null && acInfo.OwnAircraft)
                                            WarnWindow.Display("No service informaition for Aircraft: " + acInfo.Registration);
                                       
                                    }


                                }
                                return true;
                            }

                            return false;
                        }
                        return false;
                    }
                    return false;
                }

                return true;
            }
          
        }


        public async static Task<double> GetACScheduleService(DateTime dateFlightDate,int IDX_ACDetails, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var lastCptureDate = await ctx.vl_LastServiceCaptureDate.FirstOrDefaultAsync(x => x.IDX_AC_Details == IDX_ACDetails);
                if (lastCptureDate!=null)
                {
                    var techlogs = await ctx.t_Techlog.Where(x => x.TechlogDate > lastCptureDate.Last_Capture_Date && x.TechlogDate <= dateFlightDate && x.IDX_AC_Details == IDX_ACDetails)
                                                .OrderBy(x=>x.TechlogDate).ToListAsync();
                    if (techlogs != null && techlogs.Count > 0)
                    {
                        var lastTechlogCaptureDate = techlogs.Max(x => x.TechlogDate);
                        var tschLegs = await ctx.tsch_Legs.Include("tsch_AC_Pilot").Where(x => x.tsch_AC_Pilot.FlightDate > lastCptureDate.Last_Capture_Date
                                                                   && x.tsch_AC_Pilot.FlightDate > lastTechlogCaptureDate && x.tsch_AC_Pilot.IDX_ACDetails == IDX_ACDetails).ToListAsync();
                        if (tschLegs!=null && tschLegs.Count >0 )
                        {
                            var knownTime = techlogs.Sum(x => x.FlightTime);
                            var predictedTime = tschLegs.Sum(x => (x.ETA.TimeOfDay - x.ETD.TimeOfDay).Hours);

                            return Math.Round(knownTime + predictedTime, 2);

                        }
                        return 0.0;
                    }
                    else
                        return 0.0;

                }
                return 0.0;
            }
        }
        public async static Task<ScheduleACPilot> LoadFirstFlightSchedule(int IDX_ACDetails, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var tschACPIlots = await ctx.tsch_AC_Pilot
                                            .Include("tset_Personnel")
                                            .Include("tsch_Legs")
                                            .Include("tsch_LegsRes")
                                            .Include("tsch_LegsRes.tsch_reservationlegs")
                                            .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader")
                                            .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader.tsch_passengers")
                                            .Where(x => x.IDX_ACDetails == IDX_ACDetails).OrderBy(x => x.FlightDate).ToListAsync();

                if (tschACPIlots != null && tschACPIlots.Count > 0)
                {
                    var firstPilotLegDate = tschACPIlots.GroupBy(x => x.FlightDate.Date).OrderBy(x => x.Key).First();
                    var allDayLegs = firstPilotLegDate.SelectMany(x => x.tsch_Legs).OrderBy(x => x.ETD.TimeOfDay).ToList();
                    var tschACPilot = allDayLegs.FirstOrDefault().tsch_AC_Pilot;
                    var schedulePilot = (ScheduleACPilot)tschACPilot;
                    return schedulePilot;
                }

                return null;


            }
        }

        public async static Task<ScheduleACPilot> LoadNextTechlogSchedule(int IDX_ACDetails, DateTime dateLastTechLog, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var tmpDate = dateLastTechLog.AddDays(-1);

            using (ctx)
            {
                var tschACPIlots = await  ctx.tsch_AC_Pilot
                                            .Include("tset_Personnel")
                                            .Include("tsch_Legs")
                                            .Include("tsch_LegsRes")
                                            .Include("tsch_LegsRes.tsch_reservationlegs")
                                            .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader")
                                            .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader.tsch_passengers")
                                            .Where(x => x.IDX_ACDetails== IDX_ACDetails && x.FlightDate > tmpDate && x.TechLogID==null).OrderBy(x=>x.FlightDate).ToListAsync();

                if (tschACPIlots!=null && tschACPIlots.Count >0 )
                {
                    var firstPilotLegDate = tschACPIlots.GroupBy(x => x.FlightDate.Date).OrderBy(x=>x.Key).First();
                    var allDayLegs= firstPilotLegDate.SelectMany(x => x.tsch_Legs).OrderBy(x=>x.ETD.TimeOfDay).ToList();
                    if (allDayLegs.Count() == 0)
                    {
                        var ErrorMessage = "The is no legs scheduled for this aircraft please schedule the legs on the Schedule Function";

                        FailWindow.Display(ErrorMessage);

                        return null;

                    }
                    var tschACPilot = allDayLegs.FirstOrDefault().tsch_AC_Pilot;
                    var schedulePilot = (ScheduleACPilot)tschACPilot;
                    return schedulePilot;
                }

                return null;


            }
        }

        public async static Task<int?> GetPilotLastAP(DateTime flightDate, int idxPilot, String Server, string regionalDBName)
          {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsch_AC_Pilot = await ctx.tsch_AC_Pilot.Include("tsch_Legs").Where(x => ( x.IDX_Pilots == idxPilot || x.IDX_Pilots2==idxPilot)  && (x.tsch_Legs.Count >0)  && (x.FlightDate < flightDate)).           
                                             OrderByDescending(x => x.FlightDate).FirstOrDefaultAsync();

                if (tsch_AC_Pilot != null)
                {
                    var tschLeg = tsch_AC_Pilot.tsch_Legs.OrderByDescending(x => x.ETA).FirstOrDefault();

                    //var legInfo = await ctx.tsch_Legs.Include("tsch_AC_Pilot").
                    //                             Where(x => x.tsch_AC_Pilot.IDX_Pilots == idxPilot && x.ETA < flightDate).
                    //                             OrderByDescending(x => x.ETA).FirstOrDefaultAsync();

                    return tschLeg != null ? tschLeg.ToAP : 0;
                }

                return null;
            }
        }


        public async static Task<int> GetAircraftLastAP(DateTime flightDate, int idxAircraft, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var pilotInfo = await ctx.tsch_Legs.Include("tsch_AC_Pilot").
                                                Where(x => x.tsch_AC_Pilot.IDX_ACDetails == idxAircraft && x.ETA < flightDate).
                                                OrderByDescending(x => x.ETA).FirstOrDefaultAsync();
                return pilotInfo != null ? pilotInfo.ToAP : 0;
               
            }

        }



        public static async Task<String>  GetScheduleUser (DateTime flightDate, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();
            schedule.FlightDate = flightDate;
            using (ctx)
            {

                var inUseUser = await ctx.tsch_InUse.Where(x => x.FlightDate == flightDate).FirstOrDefaultAsync();
                return inUseUser != null ? inUseUser.Username : "";

            }
           
        }


        public async static Task<Schedule> LoadPilotSchedule(DateTime flightDate, int idxPilot, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();
            schedule.FlightDate = flightDate;
            ctx.Database.CommandTimeout = 60;
            using (ctx)
            {
                var dbFlightDate = new DateTime(flightDate.Year, flightDate.Month, flightDate.Day);
                var pilots = await ctx.tsch_AC_Pilot.Include("tset_Personnel")
                                             .Include("tsch_Legs")
                                             .Include("tsch_LegsRes")
                                             .Include("tsch_LegsRes.tsch_reservationlegs")
                                             //.Include("tsch_LegsRes.tsch_reservationHeader")
                                             .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader.tsch_passengers")
                                             .Where(x => x.FlightDate == dbFlightDate & x.IDX_Pilots== idxPilot).ToListAsync();


                var lstACPilots = pilots.Select(x => (ScheduleACPilot)x).ToList();
                schedule.list_ACPilots.AddRange(lstACPilots); ;


            }
            return schedule;
        }


        public async static Task<Schedule> LoadAircraftSchedule(DateTime flightDate, int idxAircraft, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();
            schedule.FlightDate = flightDate;
            ctx.Database.CommandTimeout = 60;
            using (ctx)
            {
                var dbFlightDate = new DateTime(flightDate.Year, flightDate.Month, flightDate.Day);
                var pilots = await ctx.tsch_AC_Pilot.Include("tset_Personnel")
                                             .Include("tsch_Legs")
                                             .Include("tsch_LegsRes")
                                             .Include("tsch_LegsRes.tsch_reservationlegs")
                                             //.Include("tsch_LegsRes.tsch_reservationHeader")
                                             .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader.tsch_passengers")
                                             .Where(x => x.FlightDate == dbFlightDate & x.IDX_ACDetails == idxAircraft).ToListAsync();


                var lstACPilots = pilots.Select(x => (ScheduleACPilot)x).ToList();
                schedule.list_ACPilots.AddRange(lstACPilots); ;


            }
            return schedule;
        }
        public async static Task<Schedule> LoadScheduleAsync(DateTime flightDate, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();
            schedule.FlightDate = flightDate;
            ctx.Database.CommandTimeout = 60;
            using (ctx)
            {
                var dbFlightDate = new DateTime(flightDate.Year, flightDate.Month, flightDate.Day);
                var pilots = await ctx.tsch_AC_Pilot.Include("tset_Personnel")
                                             .Include("tsch_Legs")
                                             .Include("tsch_LegsRes")
                                             .Include("tsch_LegsRes.tsch_reservationlegs")
                                             //.Include("tsch_LegsRes.tsch_reservationHeader")
                                             .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader.tsch_passengers")
                                             .Where(x => x.FlightDate == dbFlightDate).ToListAsync();

               
                var lstACPilots = pilots.Select(x => (ScheduleACPilot)x).ToList();
                schedule.list_ACPilots.AddRange(lstACPilots); ;


            }
            return schedule;
        }


        public  static Schedule LoadSchedule(DateTime flightDate, String Server, string regionalDBName)
        {
            
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();
            schedule.FlightDate = flightDate;
            using (ctx)
            {
               var dbFlightDate = new DateTime(flightDate.Year, flightDate.Month, flightDate.Day);
               var pilots=  ctx.tsch_AC_Pilot.Include("tset_Personnel")
                                            .Include("tsch_Legs")
                                            .Include("tsch_LegsRes")
                                            .Include("tsch_LegsRes.tsch_reservationlegs")
                                              .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader")
                                              .Include("tsch_LegsRes.tsch_reservationlegs.tsch_reservationHeader.tsch_passengers")
                                            .Where(x => x.FlightDate == dbFlightDate).ToList();

                var lstACPilots= pilots.Select(x => (ScheduleACPilot)x).ToList();
                schedule.list_ACPilots.AddRange(lstACPilots); ;

            }
            return schedule;
        }

        private void UpdatePilotSchedules(List<ScheduleACPilot> listPilots, String Server, string regionalDBName, SchedwinRegionalEntities ctx)
        {
            var pilotsACIDXList = listPilots.Select(x => x.IDX).ToList();

            var dbPilots = ctx.tsch_AC_Pilot.Include("tsch_Legs")
                                           .Include("tsch_LegsRes").Where(x => pilotsACIDXList.Contains(x.IDX)).ToList();
            var dbLegs = dbPilots.SelectMany(x => x.tsch_Legs).ToList();

            foreach (var acPilot in listPilots)
            {
                var dbPilot = dbPilots.FirstOrDefault(x => x.IDX == acPilot.IDX);
                if (dbPilot!=null)
                {
                    dbPilot.IDX_ACDetails = acPilot.IDX_Aircraft;
                    dbPilot.IDX_Pilots = acPilot.IDX_Pilot_1;
                    dbPilot.IDX_Pilots2 = acPilot.IDX_Pilot_2 ?? 0;
                    dbPilot.RevisionNumber = Convert.ToInt16(acPilot.tmp_RevisionNo);
                    dbPilot.Printed_This_Revision = false;
                    dbPilot.IDX_Airport_Aircraft = acPilot.IDX_AircraftAP;
                    dbPilot.IDX_Airport_Pilot = acPilot.IDX_PilotAP_1;
                    dbPilot.IDX_Airport_Pilot2 = acPilot.IDX_PilotAP_2;
                    dbPilot.IDX_PilotType = acPilot.IDX_PilotType_1;
                    dbPilot.IDX_Pilot2Type = acPilot.IDX_PilotType_2;
                    dbPilot.Comment = acPilot.Comment;
                    dbPilot.IDX_ACTypes = acPilot.IDX_AircraftType;
                    if (acPilot.TechLogID == 0)
                        dbPilot.TechLogID = null;
                    else
                        dbPilot.TechLogID = acPilot.TechLogID;
                    dbPilot.Notes = acPilot.Notes;
                    dbPilot.ts_Date = DateTime.Now;

                    foreach (var delLeg in acPilot.DeletedLegs)
                    {
                        var tschLeg = dbLegs.FirstOrDefault(x => x.IDX == delLeg.IDX);

                        foreach (var delLegRes in delLeg.ResDelList)
                        {
                            var tschLegRes = tschLeg.tsch_LegsRes.FirstOrDefault(x => x.IDX == delLegRes.IDX);
                            if (tschLegRes != null)
                                ctx.tsch_LegsRes.Remove(tschLegRes);
                        }

                        if (tschLeg!=null)
                            ctx.tsch_Legs.Remove(tschLeg);
                    }
                    
                    foreach (var leg in acPilot.Legs)
                    {

                        if (leg.IsNew && leg.IsComplete)
                        {
                            var tschLeg = (tsch_Legs)leg;
                            dbPilot.tsch_Legs.Add(tschLeg);
                            ctx.tsch_Legs.Add(tschLeg);

                            foreach (var legRes in leg.ResList)
                            {

                                var tschLegRes = (tsch_LegsRes)legRes;
                                tschLegRes.IDX_AC_Pilot = dbPilot.IDX;
                                tschLeg.tsch_LegsRes.Add(tschLegRes);
                                ctx.tsch_LegsRes.Add(tschLegRes);
                            }
                        }
                        else
                        {
                            var dbLeg = dbLegs.FirstOrDefault(x => x.IDX == leg.IDX);
                            if (dbLeg!=null)
                            {

                                foreach (var delRes in leg.ResDelList)
                                {
                                    var tschLegRes = dbLeg.tsch_LegsRes.FirstOrDefault(x => x.IDX == delRes.IDX);
                                    if (tschLegRes != null)
                                        ctx.tsch_LegsRes.Remove(tschLegRes);
                                }

                                dbLeg.FromAP = leg.IDX_FromAP;
                                dbLeg.ToAP = leg.IDX_ToAP;
                                dbLeg.ETA = leg.ETA;
                                dbLeg.ETD = leg.ETD;
                                dbLeg.NumPax = Convert.ToInt16(leg.NumPax);
                                dbLeg.DirectDistance = Convert.ToInt16(leg.Distance);
                                dbLeg.Budget = leg.Budget;
                                dbLeg.GameflightTime = leg.GameFT;
                                dbLeg.MaxLandingWeight = leg.MaxLDW;
                                dbLeg.MaxTakeOffWeight = leg.MaxTOW;
                                dbLeg.Turnaround = leg.TurnAroundTime;
                                foreach (var legRes in leg.ResList)
                                {
                                    if (legRes.IsNew)
                                    {
                                        var tschLegRes = (tsch_LegsRes)legRes;
                                        tschLegRes.IDX_AC_Pilot = dbPilot.IDX;
                                        dbLeg.tsch_LegsRes.Add(tschLegRes);
                                        ctx.tsch_LegsRes.Add(tschLegRes);

                                    }
                                }
                            }
                        }
                    }

                }

              
            }
        }

        private void SaveNewPilotSchedules(List<ScheduleACPilot> listPilots, String Server, string regionalDBName, SchedwinRegionalEntities ctx )
        {

            foreach (var newPilot in listPilots)
            {
                var tschACPilot=newPilot.ToDBTree();
                tschACPilot.TechLogID = null;
                tschACPilot.ts_Date = DateTime.Now;
                foreach (var tschLeg in tschACPilot.tsch_Legs)
                {
                    ctx.tsch_Legs.Add(tschLeg);
                 
                    foreach (var tschLegRes in tschLeg.tsch_LegsRes)
                    {
                        ctx.tsch_LegsRes.Add(tschLegRes);
                    }
                }

                ctx.tsch_AC_Pilot.Add(tschACPilot);

            }
        }

    }
}
