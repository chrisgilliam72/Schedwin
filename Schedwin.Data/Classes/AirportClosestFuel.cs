using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class AirportClosestFuel
    {
        public int IDX_FromAP { get; set; }

        public int IDX_ToAP { get; set; }

        public int IDX_Country { get; set; }

        public int Distance { get; set; }

        public int IDX_FuelType { get; set; }

        private static List<AirportClosestFuel> _fuelList { get; set; }

        public static double GetDistance(int IDX_FuelType, int IDX_FromAP, int IDX_ToAP)
        {
            if (_fuelList!=null && _fuelList.FirstOrDefault(x => x.IDX_FuelType == IDX_FuelType && x.IDX_FromAP == IDX_FromAP && x.IDX_ToAP == IDX_ToAP)!=null)
            {
               return _fuelList.FirstOrDefault(x => x.IDX_FuelType == IDX_FuelType && x.IDX_FromAP == IDX_FromAP && x.IDX_ToAP == IDX_ToAP).Distance ;
            }
            return 0;
        }

        public static bool LoadClosestFuelList(DateTime flightDate, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var results = ctx.sl_AirportClosestFuel(flightDate).ToList();

                _fuelList = results.Select(x => new AirportClosestFuel
                                            {
                                                IDX_Country = x.IDX_Country ?? 0,
                                                Distance = x.Distance ?? 0,
                                                IDX_FromAP = x.IDX_FromAP ?? 0,
                                                IDX_ToAP = x.IDX_ToAP ?? 0,
                                                IDX_FuelType = x.IDX_FuelType ?? 0
                                            }).ToList();


                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading ClosestFuel list:" + ex.Message);
            }
        }
    }
}
