using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class SeatRate
    {
        public int StartAP { get; set; }
        public int EndEP { get; set; }
        public double Rate { get; set; }
    }
    public class OperatorSeatRates
    {
        private static List<SeatRate> SeatRateMatrix { get; set; }

        public async static Task<bool> LoadSeatRateMatrix(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var rates = await ctx.tset_OperatorSeatRate.ToListAsync();

                SeatRateMatrix = rates.Select(x => new SeatRate { StartAP = x.StartAP, EndEP = x.DestAP, Rate=x.SellSeatRate }).ToList();

                return true;
            }

            catch (Exception )
            {
                return false;
            }
        }

        public static SeatRate GetSeatRate(int AP1, int AP2)
        {
            if (SeatRateMatrix != null)
            {
                var seatRate = SeatRateMatrix.FirstOrDefault(x => (x.StartAP == AP1 && x.EndEP == AP2) || (x.StartAP == AP2 && x.EndEP == AP1));
                return seatRate != null ? seatRate : null;
            }
            else
                return null;
        }
    }
}
