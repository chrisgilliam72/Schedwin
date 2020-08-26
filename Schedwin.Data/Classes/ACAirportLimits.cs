using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ACAirportLimits
    {
        public int IDX_Airport { get; set; }
        public int IDX_ACType { get; set; }

        public int TurnAroundTime { get; set; }
        public short MaxLandingWeight { get; set; }
        public short MaxTakeOffWeight { get; set; }

        public DateTime StartPeriod { get; set; }

        public DateTime EndPeriod { get; set; }

        private static List<ACAirportLimits> _listLimits = null;

        public static explicit operator tset_Limitations(ACAirportLimits acLimits)
        {
            var tset_limits = new tset_Limitations();

            tset_limits.IDX_ACType = acLimits.IDX_ACType;
            tset_limits.IDX_Airports = acLimits.IDX_Airport;
            tset_limits.TurnaroundTime = acLimits.TurnAroundTime;
            tset_limits.MaxLandingWeight = acLimits.MaxLandingWeight;
            tset_limits.MaxTakeoffWeight = acLimits.MaxTakeOffWeight;
            tset_limits.StartPeriod = acLimits.StartPeriod;
            tset_limits.EndPeriod = acLimits.EndPeriod;

            return tset_limits;

        }

        public static ACAirportLimits GetAPLimits(int idxAP, int idxACType, DateTime start, DateTime end)
        {
            if (_listLimits!=null)
            {
                return _listLimits.FirstOrDefault(x => x.IDX_Airport == idxAP && x.IDX_ACType == idxACType && x.StartPeriod < start && x.EndPeriod > end);
            }

            return null;
        }

        public static List<ACAirportLimits> GetAPLimits(int idxAP)
        {
            if (_listLimits != null)
            {
                return _listLimits.Where(x => x.IDX_Airport == idxAP).ToList();
            }

            return null;
        }

        public static void  UpdateCachedObjects(int idxAiport, List<ACAirportLimits> listLimits)
        {
            if (_listLimits != null)
            {
                _listLimits.RemoveAll(x => x.IDX_Airport == idxAiport);
                _listLimits.AddRange(listLimits);
            }
        }

        public static async Task UpdateACLimits(int idxAiport, List<ACAirportLimits> listLimits, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
               var acLimits= await ctx.tset_Limitations.Where(x => x.IDX_Airports == idxAiport).ToListAsync();
                ctx.tset_Limitations.RemoveRange(acLimits);
               
                //await ctx.SaveChangesAsync();

                var newtset_Limits = listLimits.Select(x => (tset_Limitations)x).ToList();
                ctx.tset_Limitations.AddRange(newtset_Limits);

                await ctx.SaveChangesAsync();
            }
        }


        public static async Task<bool> LoadACAirportLimits(String Server, string regionalDBName)
        {
            try
            {
                if (_listLimits == null)
                {
                    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                    var ctx = new SchedwinRegionalEntities(constring);

                    var limitsLst = await ctx.tset_Limitations.ToListAsync();

                    _listLimits = limitsLst.Select(x => new ACAirportLimits { IDX_ACType = x.IDX_ACType, IDX_Airport = x.IDX_Airports,
                                                                                MaxLandingWeight = x.MaxLandingWeight, MaxTakeOffWeight = x.MaxTakeoffWeight,
                                                                                StartPeriod=x.StartPeriod, EndPeriod=x.EndPeriod}).ToList();
                  
                }
              
                
                return true;

            }

            catch (Exception ex )
            {
                throw new Exception("Error loading ACAirportLimits list:" + ex.Message);
            }
        }
    }
}
