using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
   public class TPDBRegion
    {

        public int IDX { get; set; }

        public String Region { get; set; }

        public String DBName { get; set; }



        private static List<TPDBRegion> __TPDBRegionLst = null;

        public static explicit operator TPDBRegion(tset_TP_Region_DB dbRegionInfo)
        {
            var objTPDBRegion = new TPDBRegion();
            objTPDBRegion.Region = dbRegionInfo.Region;
            objTPDBRegion.DBName = dbRegionInfo.DBName;
            return objTPDBRegion;
        }
        public static TPDBRegion GetTPRegion(String region)
        {
            if (__TPDBRegionLst != null)
                return __TPDBRegionLst.FirstOrDefault(x => x.Region == region);

            return null;
        }

        public static List<TPDBRegion> GetTPRegionalDBs()
        {
            return __TPDBRegionLst;
        }

        public async static Task<List<TPDBRegion>> LoadTPDBRegions(String Server, string regionalDBName)
        {
            var conString = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(conString);

            if (__TPDBRegionLst == null)
            {
                using (ctx)
                {
                    var dbDBObjects = await ctx.tset_TP_Region_DB.ToListAsync();
                    __TPDBRegionLst = dbDBObjects.Select(x => (TPDBRegion)x).ToList();

                }
            }

            return __TPDBRegionLst;
        }
    }
}
