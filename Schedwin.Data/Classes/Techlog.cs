using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class Techlog
    {

        public int TechLogID { get; set; }

        public DateTime TechLogDate { get; set; }

    

        public int IDX_Pilot { get; set; }

        public String ActualRoute { get; set; }

        public String ScheduledRoute { get; set; }

        public String Pilot { get; set; }

        public String CoPilot { get; set; }


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

        public int Landings { get; set; }

        public String Notes { get; set; }

        public List<TechLogFuel> FuelList { get; set; }

        Techlog()
        {
            FuelList = new List<TechLogFuel>();
        }

        public static explicit operator Techlog(t_Techlog dbTechLog)
        {
            var techLog = new Techlog();

            techLog.TechLogID = dbTechLog.TechlogID;
            techLog.TechLogDate = dbTechLog.TechlogDate;
            techLog.TachStart = dbTechLog.TachStart;
            techLog.TachEnd = dbTechLog.TachEnd;
            techLog.IDX_Pilot = dbTechLog.IDX_Pilots;
            techLog.DutyStart = dbTechLog.DutyTimeStart;
            techLog.DutyEnd = dbTechLog.DutyTimeEnd;
            techLog.PilotFlightTime = dbTechLog.PilotFlightTime;

            if (dbTechLog.tset_ACDetails!=null)
            {
                techLog.Aircraft = dbTechLog.tset_ACDetails.Registration;
            }
            if (dbTechLog.tset_Personnel!=null)
            {
                techLog.Pilot = dbTechLog.tset_Personnel.Firstname + " " + dbTechLog.tset_Personnel.Surname;
            }

      

            techLog.ActualRoute = dbTechLog.ActualRoute;
            techLog.ScheduledRoute = dbTechLog.ScheduledRoute;
            techLog.GameFlightTime = dbTechLog.ActualGameflightTime;
            techLog.Starts = dbTechLog.Starts;
            techLog.NonRevenue = Convert.ToBoolean(dbTechLog.Nonrevenue ?? 0);
            techLog.NonRevenueType = dbTechLog.NonrevType ?? -1;
            techLog.MaintenanceCost = dbTechLog.MaintenanceCost ?? 0.0;
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

        //public async static Techlog GetTechLogDetails(int techLogID, String serverName, string regionalDBName)
        //{
        //    var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
        //    var ctx = new SchedwinRegionalEntities(constring);
        //    using (ctx)
        //    {
        //        var techLog = await ctx.t_Techlog.FirstOrDefaultAsync(x => x.TechlogID == techLogID);
        //        return ()

        //    }
        //}

        public async static Task<List<Techlog>> GetTechLogs(DateTime startDate, DateTime endDate,int aircraftIDX, String serverName, string regionalDBName)
        {
     
    
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var tmpTechLogList = await ctx.t_Techlog.Include("tset_Personnel")
                                                        .Include("tset_ACDetails")
                                                        .Include("t_Techlog_Fuel")
                                                        .Where(x => x.TechlogDate >= startDate && x.TechlogDate <= endDate && x.IDX_AC_Details == aircraftIDX).ToListAsync();
                var techLogList = tmpTechLogList.Select(x => (Techlog)x).ToList();

                return techLogList;
            }
            
        }
    }
}
