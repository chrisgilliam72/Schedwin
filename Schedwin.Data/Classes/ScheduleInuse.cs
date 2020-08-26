using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ScheduleInUse
    {
        public String Username { get; set; }

        public DateTime FlightDate { get; set; }

            static public async Task<String> FlightDateInUse(DateTime flightDate, String Server, string regionalDBName)
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var tschInUse = await ctx.tsch_InUse.FirstOrDefaultAsync(x => x.FlightDate == flightDate);
                    return tschInUse != null ? tschInUse.Username : "";

                }
            }
        }

}
