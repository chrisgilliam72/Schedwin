using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class AirportFuel
    {
        public int IDX_Airport { get; set; }      
        public int IDX_FuelType { get; set; }
        public String  FuelType { get; set; }    
        public float FuelCost { get; set; }


        public static explicit operator tset_Fuel (AirportFuel fuel)
        {
            var tsetFuel = new tset_Fuel();
            tsetFuel.IDX_Airports = fuel.IDX_Airport;
            tsetFuel.IDX_FuelTypes = fuel.IDX_FuelType;
            tsetFuel.FuelCost = fuel.FuelCost;
            tsetFuel.StartDate = DateTime.Today;
            tsetFuel.EndDate = new DateTime(2030, 01, 01);
            return tsetFuel;
        }

        private static List<AirportFuel> lstAirportFuel = null;

        public static String GetFuelTypeDescription(int idxFuelType)
        {
            if (lstAirportFuel!=null && lstAirportFuel.FirstOrDefault(x=>x.IDX_FuelType== idxFuelType)!=null)
            {
                return lstAirportFuel.FirstOrDefault(x => x.IDX_FuelType == idxFuelType).FuelType;
            }

            return "";
        }

        public static List<AirportFuel> GetAirportFuel(int idxAirport)
        {
            if (lstAirportFuel != null)
            {
                return lstAirportFuel.Where(x => x.IDX_Airport == idxAirport).ToList();
            }
            return null;
        }

        public static  AirportFuel GetAirportFuel(int idxFuelType, int idxAirport, DateTime startDate, DateTime endDate)
        {
            if (lstAirportFuel != null)
            {
                return lstAirportFuel.FirstOrDefault(x => x.IDX_FuelType == idxFuelType && x.IDX_Airport == idxAirport);
            }

            return null;
        }

        public static async Task SaveAirportFuel(int IDX_Airstrip,List<AirportFuel> listAirportFuel, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetFuelList = listAirportFuel.Select(x => (tset_Fuel)x).ToList();

                ctx.tset_Fuel.RemoveRange(ctx.tset_Fuel.Where(x => x.IDX_Airports == IDX_Airstrip));
                await ctx.SaveChangesAsync();

                ctx.tset_Fuel.AddRange(tsetFuelList);
                await ctx.SaveChangesAsync();
            }

        }

        public static async Task<bool> LoadFuelList(String Server, string regionalDBName)
        {
            try
            {
                if (lstAirportFuel == null)
                {
                    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                    var ctx = new SchedwinRegionalEntities(constring);
                    var fuelLst = await ctx.tset_Fuel.Include("tset_FuelTypes").ToListAsync();

                    lstAirportFuel = fuelLst.Select(x => new AirportFuel
                                                    {
                                                        IDX_Airport = x.IDX_Airports,
                                                        IDX_FuelType = x.IDX_FuelTypes,
                                                        FuelCost = x.FuelCost,
                                                        FuelType = x.tset_FuelTypes.FuelType
                                                    }).ToList();

                    return true;
                }

                return false;
            }

            catch (Exception)
            {
                return false;
            }
        }
    }
}
