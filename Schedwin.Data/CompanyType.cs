using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data
{
   public class CompanyType
    {
        public int IDX { get; set; }

        public String Description { get; set; }

        private static List<CompanyType> _cacheList = null;

        public async static Task<List<CompanyType>> GetCompanyTypes(String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                if (_cacheList==null)
                {
                    var tmpLst = await ctx.tlst_CompanyType.ToListAsync();
                    _cacheList = tmpLst.Select(x => new CompanyType { IDX = x.IDX, Description = x.CompanyType }).OrderBy(x => x.Description).ToList();
                }
                return _cacheList;
            }
        }
    }
}
