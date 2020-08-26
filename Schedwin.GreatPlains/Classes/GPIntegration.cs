using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.GreatPlains
{
    public class GPIntegration
    {
        public static async  Task<List<GPInvoiceLineItem>> GetLineItems(DateTime flightDate, String serverName, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var legs = await ctx.tsch_ReservationLegs.Include("tsch_ReservationLegBudget").
                                        Include("tsch_ReservationLegBudget.tset_GPExportInfo").
                                         Include("tsch_ReservationHeader").
                                         Include("tsch_LegsRes").
                                           Include("tsch_LegsRes.tsch_AC_Pilot").
                                             Include("tsch_LegsRes.tsch_AC_Pilot.tset_ACDetails").
                                         Where(x => x.BookingDate == flightDate).ToListAsync();

                var noBudgetLegs = legs.Where(x => x.tsch_ReservationLegBudget.Count == 0).ToList();
                var budgets = legs.SelectMany(x => x.tsch_ReservationLegBudget).ToList();
                var lineItems = budgets.Select(x => (GPInvoiceLineItem)x).ToList();
                var noBudgetItems = noBudgetLegs.Select(x => (GPInvoiceLineItem)x).ToList();
                lineItems.AddRange(noBudgetItems);
                return lineItems;
            }

        }

        public static async Task RemoveItems(List<GPInvoiceLineItem> itemList, String serverName, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var guidList = itemList.Select(x => x.RowGuid).ToList();
                ctx.tset_GPExportInfo.RemoveRange(ctx.tset_GPExportInfo.Where(x => guidList.Contains(x.rowguid)));
                await ctx.SaveChangesAsync();
            }
        }

        public static async Task ExportItems(List<GPInvoiceLineItem> itemList, String serverName, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {

               var exportList= itemList.Select(x => (tset_GPExportInfo)x).ToList();
                ctx.tset_GPExportInfo.AddRange(exportList);
                await ctx.SaveChangesAsync();
            }
        }


    }
}
