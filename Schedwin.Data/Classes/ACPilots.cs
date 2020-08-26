using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ACPilot
    {
        public int IDX { get; set; }

        public DateTime FlightDate { get; set; }

        public String Registration { get; set; }
    
        public int IDX_AC { get; set; }

        public String Pilot { get; set; }

        public int IDX_Pilot { get; set; }

        public String Pilot2 { get; set; }

        public int IDX_Pilot2 { get; set; }

        public static explicit operator ACPilot(vl_ScheduleACPilot vl_ScheduleACPilot)
        {
            var scheduleACPilot = new ACPilot();
            scheduleACPilot.IDX = vl_ScheduleACPilot.IDX;
            scheduleACPilot.IDX_Pilot = vl_ScheduleACPilot.IDX_Pilots;
            scheduleACPilot.IDX_Pilot2 = vl_ScheduleACPilot.IDX_Pilots2 ?? -1;
            scheduleACPilot.IDX_AC = vl_ScheduleACPilot.IDX_ACDetails;
            scheduleACPilot.Pilot = vl_ScheduleACPilot.Pilot;
            scheduleACPilot.Pilot2 = vl_ScheduleACPilot.Pilot2 ?? "";
            scheduleACPilot.Registration = vl_ScheduleACPilot.Registration;
            return scheduleACPilot;
        }


       public static async  Task<List<ACPilot>> GetACPilots(DateTime flightDate, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var vlSchedPilots = await ctx.vl_ScheduleACPilot.Where(x => x.FlightDate == flightDate).ToListAsync();
                var scheduleACPiltos = vlSchedPilots.Select(x => (ACPilot)x).ToList();
                return scheduleACPiltos;
            }
        }
    }
}
