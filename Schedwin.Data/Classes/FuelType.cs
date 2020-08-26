using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class FuelType
    {
        public String TypeName { get; set;  }
        public int IDX { get; set; }

        private static List<FuelType> _fuelTypesCacheList = null;

        public static List<FuelType> GetFuelTypes()
        {
            return _fuelTypesCacheList;
        }

        public static explicit operator FuelType(tset_FuelTypes tsetFuelType )
        {
            var fuelType = new FuelType();
            fuelType.IDX = tsetFuelType.IDX;
            fuelType.TypeName = tsetFuelType.FuelType;
            return fuelType;
        }

        public static explicit operator tset_FuelTypes(FuelType fuelType)
        {
            var tset_FuelTypes = new tset_FuelTypes();
            tset_FuelTypes.IDX = fuelType.IDX;
            tset_FuelTypes.FuelType = fuelType.TypeName;

            return tset_FuelTypes;
        }

        public async static Task<List<FuelType>> LoadFuelTypes(String Server, string regionalDBName)
        {
            if (_fuelTypesCacheList != null)
                return _fuelTypesCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                   var tmpLlst=  await  ctx.tset_FuelTypes.ToListAsync();
                    _fuelTypesCacheList = tmpLlst.Select(x => (FuelType)x).ToList();
                    return _fuelTypesCacheList;
                }
            }
        }
    }
}
