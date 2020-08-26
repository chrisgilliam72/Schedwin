using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class TPRoutingCode 
    {
        public int IDX { get; set; }
        public String AP1 { get; set; }
        public String AP2 { get; set; }
        public String TPCode { get; set; }

        public bool IsSoleUse { get; set; }

        private static List<TPRoutingCode> _RoutingCodesLst = null;

        public TPRoutingCode()
        {
            IsSoleUse = false;
        }

        public static explicit operator TPRoutingCode(tset_TP_Routing_Codes tsetTPCode)
        {
            var tpRoutingCode = new TPRoutingCode();
            tpRoutingCode.IDX = tsetTPCode.IDX;
            tpRoutingCode.AP1 = tsetTPCode.StartAP;
            tpRoutingCode.AP2 = tsetTPCode.EndAP;
            tpRoutingCode.TPCode = tsetTPCode.Option_Code;
            tpRoutingCode.IsSoleUse = tsetTPCode.SoleUse;
            return tpRoutingCode;
        }

        public static TPRoutingCode GetTPRoutingCode (String TPCode)
        {
            if (_RoutingCodesLst != null)
                return _RoutingCodesLst.FirstOrDefault(x => x.TPCode == TPCode);

            return null;
        }

        public static List<TPRoutingCode> GetTPRoutingCodes()
        {
            return _RoutingCodesLst;
        }

        public async static Task<List<TPRoutingCode>> LoadTPRoutingCodes(String Server, string regionalDBName)
        {
            var conString = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(conString);

            if (_RoutingCodesLst==null)
            {
                using (ctx)
                {
                    var tpTsetCodes = await ctx.tset_TP_Routing_Codes.ToListAsync();
                    _RoutingCodesLst = tpTsetCodes.Select(x => (TPRoutingCode)x).ToList();
                    
                }
            }

            return _RoutingCodesLst;
        }
    }
}
