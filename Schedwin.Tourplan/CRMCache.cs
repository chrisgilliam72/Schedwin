using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Tourplan
{
    public class CRMCode
    {
        public String Code { get; set; }

        public String Name { get; set; }

        public static async Task< List<CRMCode>> GetCRMList(String nameMatch,String Server, String databaseName )
        {

            var conString = RegionalConnectionGenerator.GetConnectionString(Server, databaseName);
            var ctx = new TPRegionalEntities(conString);
            using (ctx)
            {
               var tmpCRMList = await ctx.CRMs.Where(x=>x.NAME.StartsWith(nameMatch)).ToListAsync();
               return tmpCRMList.Select(x => new CRMCode { Code = x.CODE, Name = x.NAME }).ToList();
            }

        }
    }
}
