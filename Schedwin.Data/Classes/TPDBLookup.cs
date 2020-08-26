using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class TPDBLookup
    {
        public int IDX { get; set; }

        public String BranchCode { get; set; }

        public String DBName { get; set; }
        
        private static List<TPDBLookup> _lstTPDBLookupsCache = null;

        public static explicit operator TPDBLookup(tset_TP_BranchCode_DB tsetTPBranchCode)
        {

            var tpDBLookup = new TPDBLookup();
            tpDBLookup.IDX = tsetTPBranchCode.IDX;
            tpDBLookup.BranchCode = tsetTPBranchCode.TPBranchCODE;
            tpDBLookup.DBName = tsetTPBranchCode.TPDBName;
            return tpDBLookup;
        }

        public static List<TPDBLookup> GetTPDBLookups()
        {
            return _lstTPDBLookupsCache;
        }

        public static String GetDBName(String TPRef)
        {
            if (_lstTPDBLookupsCache !=null)
            {
                var refBranchCode = TPRef.Substring(0, 2);
                var tpDBLookup = _lstTPDBLookupsCache.FirstOrDefault(x => x.BranchCode == refBranchCode);
                return tpDBLookup != null ? tpDBLookup.DBName : "";

            }
            else
                return "";
        }
        public static async Task<List<TPDBLookup>> LoadTPDBLookups(String Server, string regionalDBName)
        {
            if (_lstTPDBLookupsCache == null)
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var lstDBNames = await ctx.tset_TP_BranchCode_DB.ToListAsync();
                    _lstTPDBLookupsCache = lstDBNames.Select(x => (TPDBLookup)x).ToList();
                    return _lstTPDBLookupsCache;
                }
            }
            else
                return _lstTPDBLookupsCache;

        }
    }

}

