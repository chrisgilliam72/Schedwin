using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ACStationType
    {
        public int IDX { get; set; }

        public String Description { get; set; }

        private static List<ACStationType> stationTypeLists = null;

        public static explicit operator ACStationType (tlst_StationType tlstType)
        {
            var stationType = new ACStationType();
            stationType.IDX = tlstType.IDX;
            stationType.Description = tlstType.StationType;
            return stationType;
        }

        public static  explicit operator tlst_StationType (ACStationType stationType)
        {
            var tlstType = new tlst_StationType();
            tlstType.IDX = stationType.IDX;
            tlstType.StationType = stationType.Description;
            return tlstType;

        }

        public static List<ACStationType> GetStationTypeList()
        {
            return stationTypeLists;
        }

        public static async Task<List<ACStationType>> LoadStationTyoes(String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
               var dbList= await ctx.tlst_StationType.ToListAsync();
               stationTypeLists = dbList.Select(x => (ACStationType)x).ToList();
               return stationTypeLists;
            }
        }
    }
}

