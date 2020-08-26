using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class AllowableDutyHours
    {

        static public List<tset_AllowbleHours> AllowableMatrx { get; set; }
        static public List<tset_AllowableDutyHours> AllowableDuties { get; set; }

        public async static Task<List<tset_AllowbleHours>> LoadAllowableMatrix(String Server, string regionalDBName)
        {
            if (AllowableMatrx != null)
                return AllowableMatrx;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    AllowableMatrx = await ctx.tset_AllowbleHours.OrderByDescending(x => x.Start_Time).ToListAsync();
                    return AllowableMatrx;
                }
            }
        }

        public async static Task<List<tset_AllowableDutyHours>> LoadAllowableDuties(String Server, string regionalDBName)
        {
            if (AllowableDuties != null)
                return AllowableDuties;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    AllowableDuties = await ctx.tset_AllowableDutyHours.ToListAsync();
                    return AllowableDuties;
                }
            }

        }
    
    }
}
