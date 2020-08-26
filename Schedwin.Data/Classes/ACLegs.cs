using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
   public  class ACLeg
    {
        public int IDX { get; set; }
        public DateTime ETD { get; set; }

        public DateTime ETA { get; set; }

        public int IDX_FromAP { get; set; }

        public int IDX_ToAP { get; set; }

        public String FromAP { get; set; }

        public String ToAP { get; set; }




        public static async Task<List<ACLeg>> GetScheduleLegs(int idxACPilot, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var vlSchedLegs= await ctx.vl_ScheduleLegs.Where(x => x.IDX_AC_Pilot == idxACPilot).ToListAsync();
                var scheduleACLegs = vlSchedLegs.Select(x => (ACLeg)x).ToList();
                return scheduleACLegs;
            }
        }
    }
}
