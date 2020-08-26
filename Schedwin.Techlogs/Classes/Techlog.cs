using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;
using System.Data.Entity;

namespace Schedwin.Techlogs
{
    public class Techlog
    {
        public int IDX { get; set; }
        public bool IsNew { get; set; }

        public int TechLogID { get; set; }

        public DateTime TechLogDate { get; set; }
 

        public int IDX_Pilot { get; set; }
        public int? IDX_CoPiliot { get; set; }
        public String ActualRoute { get; set; }

        public String ScheduledRoute { get; set; }

        public String Pilot { get; set; }

        public String CoPilot { get; set; }

        public int IDX_Aicraft { get; set; }
        public String Aircraft { get; set; }

        public double TachStart { get; set; }
        public double TachEnd { get; set; }

        public DateTime DutyStart { get; set; }

        public DateTime DutyEnd { get; set; }

        public double FlightTime { get; set; }

        public bool  NonRevenue { get; set; }

        public int NonRevenueType { get; set; }

        public int NonRevenueDetails { get; set; }

        public double GameFlightTime { get; set; }

        public double PilotFlightTime { get; set; }

        public int Starts { get; set; }

        public String MaintenanceDetail { get; set; }

        public double MaintenanceCost { get; set; }

        public int Distance { get; set; }

        public double Speed { get; set; }

        public short Landings { get; set; }

        public String Notes { get; set; }

        public List<TechLogFuel> FuelList { get; set; }

        public Techlog()
        {
            IsNew = true;
            FuelList = new List<TechLogFuel>();
        }


        public static explicit operator t_Techlog(Techlog techLog)
        {
            var dbTechLog = new t_Techlog();
            dbTechLog.TechlogID = techLog.TechLogID;
            dbTechLog.TechlogDate = techLog.TechLogDate;
            dbTechLog.IDX_AC_Details = techLog.IDX_Aicraft;
            dbTechLog.TachStart = techLog.TachStart;
            dbTechLog.TachEnd = techLog.TachEnd;
            dbTechLog.IDX_Pilots = techLog.IDX_Pilot;
            dbTechLog.IDX_Pilots2 = techLog.IDX_CoPiliot;
            dbTechLog.ActualGameflightTime = techLog.GameFlightTime;
            dbTechLog.DutyTimeStart = techLog.DutyStart;
            dbTechLog.DutyTimeEnd = techLog.DutyEnd;
            dbTechLog.PilotFlightTime = techLog.PilotFlightTime;
            dbTechLog.CoCode = "SEFO";
            dbTechLog.ActualRoute = techLog.ActualRoute;
            dbTechLog.ScheduledRoute = techLog.ScheduledRoute;
            dbTechLog.ActualGameflightTime= techLog.GameFlightTime;
            dbTechLog.Starts = techLog.Starts;
            dbTechLog.Nonrevenue = Convert.ToInt32(techLog.NonRevenue);
            dbTechLog.MaintenanceDetails = techLog.MaintenanceDetail;
            dbTechLog.NonrevType = techLog.NonRevenueType;
            dbTechLog.MaintenanceCost = techLog.MaintenanceCost;
            dbTechLog.ActualDistance = techLog.Distance;
            dbTechLog.Speed = techLog.Speed;
            dbTechLog.FlightTime= techLog.FlightTime;
            dbTechLog.Landings = techLog.Landings;
            dbTechLog.Notes = techLog.Notes;
            return dbTechLog;
        }

        public static explicit operator Techlog(t_Techlog dbTechLog)
        {
            var techLog = new Techlog();
            techLog.IsNew = false;
            techLog.IDX = dbTechLog.IDX;
            techLog.TechLogID = dbTechLog.TechlogID;
            techLog.TechLogDate = dbTechLog.TechlogDate;
            techLog.TachStart = dbTechLog.TachStart;
            techLog.TachEnd = dbTechLog.TachEnd;
            techLog.IDX_Pilot = dbTechLog.IDX_Pilots;
            techLog.DutyStart = dbTechLog.DutyTimeStart;
            techLog.DutyEnd = dbTechLog.DutyTimeEnd;
            techLog.PilotFlightTime = dbTechLog.PilotFlightTime;
            techLog.IDX_Aicraft = dbTechLog.IDX_AC_Details;
            techLog.IDX_Pilot = dbTechLog.IDX_Pilots;
            techLog.IDX_CoPiliot = dbTechLog.IDX_Pilots2;
            techLog.GameFlightTime = dbTechLog.ActualGameflightTime;

            if (dbTechLog.tset_ACDetails!=null)
            {
               
                techLog.Aircraft = dbTechLog.tset_ACDetails.Registration;
            }
            if (dbTechLog.tset_Personnel!=null)
            {
                techLog.Pilot = dbTechLog.tset_Personnel.Firstname + " " + dbTechLog.tset_Personnel.Surname;
            }
            if (dbTechLog.tset_Personnel1 != null)
            {
                techLog.CoPilot= dbTechLog.tset_Personnel1.Firstname + " " + dbTechLog.tset_Personnel1.Surname;
            }
      

            techLog.ActualRoute = dbTechLog.ActualRoute;
            techLog.ScheduledRoute = dbTechLog.ScheduledRoute;
            techLog.GameFlightTime = dbTechLog.ActualGameflightTime;
            techLog.Starts = dbTechLog.Starts;
            techLog.NonRevenue = Convert.ToBoolean(dbTechLog.Nonrevenue ?? 0);
            techLog.NonRevenueType = dbTechLog.NonrevType ?? -1;
            techLog.MaintenanceCost = dbTechLog.MaintenanceCost ?? 0.0;
            techLog.MaintenanceDetail = dbTechLog.MaintenanceDetails;
            techLog.Distance = dbTechLog.ActualDistance;
            techLog.Speed = dbTechLog.Speed;
            techLog.FlightTime = dbTechLog.FlightTime;
            techLog.Landings = dbTechLog.Landings;
            techLog.Notes = dbTechLog.Notes;

            if (dbTechLog.t_Techlog_Fuel!=null)
            {
               var techFuelList= dbTechLog.t_Techlog_Fuel.Select(x => (TechLogFuel)x).ToList();
                techLog.FuelList.AddRange(techFuelList);

            }


            return techLog; 
        }

        public async Task Save(String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                if (IsNew)
                {
                    var dbTechLog = (t_Techlog)this;
                    ctx.t_Techlog.Add(dbTechLog);
                    var newFuelList = FuelList.Select(x => (t_Techlog_Fuel)x).ToList();
                    foreach (var techfuelLog in newFuelList)
                    {
                        dbTechLog.t_Techlog_Fuel.Add(techfuelLog);
                        ctx.t_Techlog_Fuel.Add(techfuelLog);
                    }

                }
                else
                {
                    var dbTechLog = await ctx.t_Techlog.
                                                Include("t_Techlog_Fuel").
                                                FirstOrDefaultAsync(x => x.IDX == IDX);
                    if (dbTechLog!=null)
                    {
                       

                        dbTechLog.TachStart = TachStart;
                        dbTechLog.TachEnd = TachEnd;
                        dbTechLog.IDX_Pilots = IDX_Pilot;
                        dbTechLog.IDX_Pilots2 = IDX_CoPiliot;
                        dbTechLog.ActualGameflightTime = GameFlightTime;
                        dbTechLog.DutyTimeStart = DutyStart;
                        dbTechLog.DutyTimeEnd = DutyEnd;
                        dbTechLog.PilotFlightTime = PilotFlightTime;
                        dbTechLog.ActualRoute = ActualRoute;
                        dbTechLog.ActualGameflightTime = GameFlightTime;
                        dbTechLog.Starts = Starts;
                        dbTechLog.Nonrevenue = Convert.ToInt32(NonRevenue);
                        dbTechLog.MaintenanceDetails = MaintenanceDetail;
                        dbTechLog.NonrevType = NonRevenueType;
                        dbTechLog.MaintenanceCost = MaintenanceCost;
                        dbTechLog.ActualDistance = Distance;
                        dbTechLog.Speed = Speed;
                        dbTechLog.FlightTime = FlightTime;
                        dbTechLog.Landings = Landings;
                        dbTechLog.Notes = Notes;
                        dbTechLog.TechlogID = TechLogID;

                        do
                        {
                            var techlogfuel = dbTechLog.t_Techlog_Fuel.FirstOrDefault();
                            if (techlogfuel!=null)
                                ctx.t_Techlog_Fuel.Remove(techlogfuel);
                        } while (dbTechLog.t_Techlog_Fuel.Count > 0);

                        var newFuelList = FuelList.Select(x => (t_Techlog_Fuel)x).ToList();
                        foreach (var techfuelLog in newFuelList)
                        {
                            techfuelLog.IDX_AC_Details = IDX_Aicraft;
                            dbTechLog.t_Techlog_Fuel.Add(techfuelLog);
                            ctx.t_Techlog_Fuel.Add(techfuelLog);
                        }
                    }
                }

                await ctx.SaveChangesAsync();

              
            }

        }

        public async static Task DeleteTechLog(int techLogID, String serverName, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var techLog = await ctx.t_Techlog.FirstOrDefaultAsync(x => x.TechlogID == techLogID);
                var schedule = await ctx.tsch_AC_Pilot.FirstOrDefaultAsync(x => x.TechLogID == techLogID);
                if (techLog != null)
                    ctx.t_Techlog.Remove(techLog);

                if (schedule != null)
                    schedule.TechLogID = null;

                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<Techlog> LoadTechLog(int techLogID, String serverName, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var dbTechlog = await ctx.t_Techlog.FirstOrDefaultAsync(x => x.TechlogID == techLogID);
                if (dbTechlog !=null)
                {
                    var techlog = (Techlog)dbTechlog;
                    return techlog;
                }
                return null;
            }
        }

        public async static Task<List<Techlog>> GetTechLogs(List<int> techlogIDS,  int aircraftIDX, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var tmpTechLogList = await ctx.t_Techlog.Include("tset_Personnel")
                                                         .Include("tset_Personnel1")
                                                        .Include("tset_ACDetails")
                                                        .Include("t_Techlog_Fuel")
                                                        .Where(x=>x.IDX_AC_Details == aircraftIDX && techlogIDS.Contains(x.TechlogID)).ToListAsync();
                var techLogList = tmpTechLogList.Select(x => (Techlog)x).ToList();

                return techLogList;
            }
        }


        public async static Task<List<Techlog>> GetTechLogs(DateTime startDate, DateTime endDate,int aircraftIDX, String serverName, string regionalDBName)
        {
     
    
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var tmpTechLogList = await ctx.t_Techlog.Include("tset_Personnel")
                                                         .Include("tset_Personnel1")
                                                        .Include("tset_ACDetails")
                                                        .Include("t_Techlog_Fuel")
                                                        .Where(x => x.TechlogDate >= startDate && x.TechlogDate <= endDate && x.IDX_AC_Details == aircraftIDX).ToListAsync();
                var techLogList = tmpTechLogList.Select(x => (Techlog)x).ToList();

                return techLogList;
            }
            
        }
    }
}
