using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    
    public class Pilots
    {

        public static async Task ClearRosterForMonth(DateTime startDate, List<PilotDutyPeriod> roster, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            var endDate = startDate.AddMonths(1);

            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            using (ctx)
            {
                ctx.tsch_PilotRoster.RemoveRange(ctx.tsch_PilotRoster.Where(x => x.Date >= startDate && x.Date < endDate));

                await ctx.SaveChangesAsync();

            }
        }


        public static async Task<bool> UpdatePilotRoster(DateTime startDate, PilotDutyPeriod roster, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            var endDate = startDate.AddMonths(1);

            using (ctx)
            {
                var pilotRoster = await ctx.tsch_PilotRoster.Where(x => x.Date >= startDate && x.Date < endDate && x.IDX_Pilot == roster.IDX).OrderBy(x=>x.Date).ToListAsync();
                int dayIndex = 0;
                foreach (var pilotDuty in pilotRoster)
                    pilotDuty.Type = roster.DailyDutyTypeList[dayIndex++].DutyType;
    

                await ctx.SaveChangesAsync();

                return true;
            }
        }


        public static async Task<bool> UpdateRosterDay(DateTime date,int idxPilot, String dutyType, int rosterDutyTypeIDX, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var pilotRoster =await  ctx.tsch_PilotRoster.FirstOrDefaultAsync(x => x.Date==date && x.IDX_Pilot==idxPilot);
                if (pilotRoster!=null)
                {
                    pilotRoster.Type = dutyType;
                    pilotRoster.IDX_RosterDutyType = rosterDutyTypeIDX;
                }
                else
                {
                    var tschPR = new tsch_PilotRoster();
                    tschPR.IDX_Pilot = idxPilot;
                    tschPR.Date = date;
                    tschPR.Type = dutyType;
                    tschPR.IDX_RosterDutyType = rosterDutyTypeIDX;
                    ctx.tsch_PilotRoster.Add(tschPR);
                }

                await ctx.SaveChangesAsync();

                return true;
            }
        }

        public static async Task<bool> SaveEntireRoster(DateTime startDate, List<PilotDutyPeriod> roster)
        {

            var ctx = new SchedwinGlobalEntities();
            var endDate = startDate.AddMonths(1);

            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            using (ctx)
            {


                foreach (var rosterItem in roster)
                {
                    var currentDate = startDate;
                    foreach (var dutyItem in rosterItem.DailyDutyTypeList)
                    {
                        if (currentDate >= endDate)
                            break;

                        var tbPilotRoster = new tbPilotRoster();
                        tbPilotRoster.fkPilotID = rosterItem.IDX;
                        tbPilotRoster.Date = currentDate;
                        tbPilotRoster.fkRosterDutyTypeID = rosterItem.DutyTypes.FirstOrDefault(x => x.Code == dutyItem.DutyType).IDX;
                        ctx.tbPilotRosters.Add(tbPilotRoster);
                        currentDate = currentDate.AddDays(1);

                    }
                }

                await ctx.SaveChangesAsync();
            }

            return true;
        }
        public static async Task<bool> SaveEntireRoster(DateTime startDate,List<PilotDutyPeriod> roster, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
                     
            var endDate = startDate.AddMonths(1);

            var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            using (ctx)
            {
              

                foreach (var rosterItem in roster)
                {
                   var currentDate = startDate;
                   foreach (var dutyItem in rosterItem.DailyDutyTypeList)
                    {
                        if (currentDate >= endDate)
                            break;

                        var tschPR = new tsch_PilotRoster();
                        tschPR.IDX_Pilot = rosterItem.IDX;
                        tschPR.Date = currentDate;
                        tschPR.Type = dutyItem.DutyType;
                        tschPR.IDX_RosterDutyType = rosterItem.DutyTypes.FirstOrDefault(x => x.Code == dutyItem.DutyType).IDX;
                        ctx.tsch_PilotRoster.Add(tschPR);
                        currentDate = currentDate.AddDays(1);
                 
                    }
                }

                await ctx.SaveChangesAsync();
            }

            return true;
        }

        public static async Task<bool> RosterCreated(DateTime date)
        {

            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var pilotRoster = await ctx.tbPilotRosters.FirstOrDefaultAsync(x => x.Date == date);
                return pilotRoster != null;

            }
        }

        public static async Task<bool> RosterCreated(DateTime date, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var pilotRoster = await  ctx.tsch_PilotRoster.FirstOrDefaultAsync(x => x.Date == date);
                return pilotRoster != null;

            }
        }

        public static async Task<List<PilotDutyPeriod>> GetPilotRosters(DateTime date)
        {
           
           
            var lstPilots = new List<PilotDutyPeriod>();
            DateTime startDate = date;
            DateTime endDate = date.AddMonths(1);
            int DaysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {

                var rosterInfos = await ctx.tbPilotRosters.Where(x => x.Date >= startDate && x.Date < endDate && x.tbPilot.CanRoster == true)
                                                              .Include("tbPilot")
                                                              .Include("tbPilot.tbUser").ToListAsync();

                var dbPilots = await ctx.tbPilots.Include("tbUser")
                                                            //.Include("tsch_PilotRoster")
                                                            .Where(x => x.Active == true).ToListAsync();

                var pilotInfos = dbPilots.Select(x => (PilotDutyPeriod)x).OrderBy(x => x.FirstName).ToList();


                foreach (var pilotInfo in pilotInfos)
                {
                    pilotInfo.DutyStart=date;
                    Debug.WriteLine(pilotInfo.PilotFullName);
                    pilotInfo.Init(DaysInMonth);
                    var rosterActivities = rosterInfos.Where(x => x.fkPilotID == pilotInfo.IDX).OrderBy(x => x.Date).Select(x => x.tbRosterDutyType).ToList();
                    int i = 0;
                    foreach (var activity in rosterActivities)
                    {
                        if (pilotInfo.DailyDutyTypeList[i].CanRoster)
                            pilotInfo.DailyDutyTypeList[i].DutyType = activity.Code;
                        i++;
                    }
                }

                lstPilots.AddRange(pilotInfos);
            }

            return lstPilots;
        }

        public static async Task<List<PilotDutyPeriod>> GetPilotRosters(DateTime date, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var lstPilots = new List<PilotDutyPeriod>();
            DateTime startDate = date;
            DateTime endDate = date.AddMonths(1);
            int DaysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
            using (ctx)
            {

                var rosterInfos = await ctx.tsch_PilotRoster.Where(x => x.Date >= startDate && x.Date < endDate && x.tset_PilotsDetails.CanRoster == true)
                                                              .Include("tset_PilotsDetails")
                                                              .Include("tset_PilotsDetails.tset_Personnel").ToListAsync();

                var dbPilots = await ctx.tset_PilotsDetails.Include("tset_Personnel")
                                                            //.Include("tsch_PilotRoster")
                                                            .Where(x => x.Active==true).ToListAsync();

                var pilotInfos = dbPilots.Select(x => new PilotDutyPeriod { IDX = x.IDX, FirstName = x.tset_Personnel.Firstname, Surname = x.tset_Personnel.Surname, DutyStart = date, LicenceExpiry=x.PilotsLicenseExpiryDate }).OrderBy(x=>x.FirstName).ToList();
              
               
                foreach (var pilotInfo in pilotInfos)
                {
                
                    Debug.WriteLine(pilotInfo.PilotFullName);
                    pilotInfo.Init(DaysInMonth);
                    var rosterActivities = rosterInfos.Where(x => x.IDX_Pilot == pilotInfo.IDX).OrderBy(x => x.Date).Select(x => x.Type).ToList();
                    int i = 0;
                    foreach (var activity in rosterActivities)
                    {
                        if (pilotInfo.DailyDutyTypeList[i].CanRoster)
                            pilotInfo.DailyDutyTypeList[i].DutyType = activity;
                        i++;
                    }
                }

                lstPilots.AddRange(pilotInfos);
            }

            return lstPilots;
        }
    }
}
