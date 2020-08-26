using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class PilotDutyType
    {
        public DateTime Date { get; set; }
        public int IDX_Pilot { get; set; }

        public String DutyType { get; set; }

        public static explicit operator PilotDutyType(tsch_PilotRoster roster)
        {
            var pilotDutyType = new PilotDutyType();
            pilotDutyType.Date = roster.Date;
            pilotDutyType.IDX_Pilot = roster.IDX_Pilot;
            pilotDutyType.DutyType = roster.Type;
            return pilotDutyType;
        }
    }
    public class PilotRoster
    {

        public static async Task UpdateLicencyExpiry(int idxPilot, DateTime newExpiryDate, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dateToday = DateTime.Today;
                var rosterItems = await ctx.tsch_PilotRoster.Where(x => x.Date < newExpiryDate && x.Date > dateToday && x.Type == "E" && x.IDX_Pilot == idxPilot).ToListAsync();
                rosterItems.ForEach(x => x.Type = "1");

               await ctx.SaveChangesAsync();
            }
        }

        public static async Task<List<PilotDutyType>> GetDailyRoster(DateTime date, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
               var rosterLst= await ctx.tsch_PilotRoster.Where(x => x.Date == date).ToListAsync();
                var dutyLst = rosterLst.Select(x => (PilotDutyType)x).ToList();
                return dutyLst;
            }
        }
    }
}
