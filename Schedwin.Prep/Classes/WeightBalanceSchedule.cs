using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep.Classes
{
    public class WeightBalanceSchedule
    {
        public static async Task<List<WeightBalancePositionItem>> Load(List<int> LegIDs,String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {

               var vbItems= await ctx.tsch_LegWeightBalances.Where(x => LegIDs.Contains(x.IDX_Leg)).ToListAsync();
               var rowItems= vbItems.Select(x => (WeightBalancePositionItem)x).ToList();
                return rowItems;
            }
        }
        public static async Task Clear (List<int> LegIDs,String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {

                ctx.tsch_LegWeightBalances.RemoveRange(ctx.tsch_LegWeightBalances.Where(x => LegIDs.Contains(x.IDX_Leg)));
                await ctx.SaveChangesAsync();
            }
        }

        public static async Task Save(List<WeightBalancePositionItem> RowList, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dbItems = RowList.Select(x => (tsch_LegWeightBalances)x).ToList();
                ctx.tsch_LegWeightBalances.AddRange(dbItems);
                await ctx.SaveChangesAsync();
            }

        }
    }
}
