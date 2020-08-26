using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class AirportFeeType
    {
        public int IDX { get; set; }

        public String Description { get; set; }

        public int? IDX_ACType { get; set; }

        private static List<AirportFeeType> lstAirportFeesCache=null;

        public static explicit operator AirportFeeType (tbAirportFeeType tbAirportFee)
        {
            var feeType = new AirportFeeType();
            feeType.IDX = tbAirportFee.pkAirportFeeTypeID;
            feeType.Description = tbAirportFee.Description;
            feeType.IDX_ACType = tbAirportFee.fkAircraftTypeID;
            return feeType;
        }

        public static explicit operator AirportFeeType (tset_airportFeeType tsetFees)
        {
            var feeType = new AirportFeeType();
            feeType.IDX = tsetFees.IDX;
            feeType.Description = tsetFees.Description;
            return feeType;

        }

        public static  List<AirportFeeType> GetAllAirportFeeTypes()
        {
            return lstAirportFeesCache;
        }
       
        public static async Task<List<AirportFeeType>> LoadAllAirportFeeTypes()
        {
            if (lstAirportFeesCache == null)
            {

                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    var dbFeeTypes = await ctx.tbAirportFeeTypes.ToListAsync();
                    lstAirportFeesCache = dbFeeTypes.Select(x => (AirportFeeType)x).OrderBy(x => x.Description).ToList();
                    return lstAirportFeesCache;
                }
            }
            else
                return lstAirportFeesCache;
        }

        public static async Task<List<AirportFeeType>> LoadAllAirportFeeTypes(String Server, string regionalDBName)
        {
            if (lstAirportFeesCache == null)
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var dbFeeTypes = await ctx.tset_airportFeeType.ToListAsync();
                    lstAirportFeesCache = dbFeeTypes.Select(x => (AirportFeeType)x).OrderBy(x=>x.Description).ToList();
                    return lstAirportFeesCache;
                }
            }
            else
                return lstAirportFeesCache;

        }
    }


    public class AirportFee
    {
        public bool IsNew { get; set; }
        public int IDX { get; set; }

        public int IDX_Airport { get; set; }

        public int IDX_FeeType { get; set; }

        public String FeeName { get; set; }

        public String Currency { get; set; }

        public double Amount { get; set; }
   
        public int? IDX_Aircraft_Type { get; set; }

        private static List<AirportFee> airportFeeCacheList = null;
        public AirportFee()
        {
            IsNew = true;
        }
        public static explicit operator AirportFee(tbAirportFee tbAirportFee)
        {
            var airportFee = new AirportFee();
            airportFee.IDX = tbAirportFee.pkAirportFeeID;
            airportFee.IDX_FeeType = tbAirportFee.fkAirportFeeType;
            airportFee.IDX_Airport = tbAirportFee.fkAirstripID;
            airportFee.Amount = tbAirportFee.Value;
            airportFee.FeeName = tbAirportFee.tbAirportFeeType.Description;
            airportFee.Currency = tbAirportFee.tbCurrency.Code;
            airportFee.IDX_Aircraft_Type = tbAirportFee.tbAirportFeeType.fkAircraftTypeID;
            airportFee.IsNew = false;

            return airportFee;
        }

        public static explicit operator AirportFee(tset_AirportFeesNew tsetAirportFee)
        {
            var airportFee = new AirportFee();
            airportFee.IDX = tsetAirportFee.IDX;
            airportFee.IDX_FeeType = tsetAirportFee.IDX_Fees;
            airportFee.IDX_Airport = tsetAirportFee.IDX_Airport;
            airportFee.Amount = tsetAirportFee.Value;
            airportFee.FeeName = tsetAirportFee.tset_airportFeeType.Description;
            airportFee.Currency = tsetAirportFee.Currency;
            airportFee.IDX_Aircraft_Type = tsetAirportFee.tset_airportFeeType.IDX_AircraftType ;
            airportFee.IsNew = false;

            return airportFee;
        }

        public static explicit operator tset_AirportFeesNew(AirportFee airportFee)
        {
            var tsetAirportFeesNew = new tset_AirportFeesNew();
            tsetAirportFeesNew.IDX = airportFee.IDX;
            tsetAirportFeesNew.IDX_Airport = airportFee.IDX_Airport;
            tsetAirportFeesNew.IDX_Fees = airportFee.IDX_FeeType;
            tsetAirportFeesNew.Value = airportFee.Amount;
            tsetAirportFeesNew.Currency = airportFee.Currency;
            return tsetAirportFeesNew;
        }

        public static List<AirportFee> GetAirportFees()
        {
            return airportFeeCacheList;
        }
        public static List<AirportFee> GetAirportFees(int idxAirport)
        {
            if (airportFeeCacheList!=null)
                return airportFeeCacheList.Where(x => x.IDX_Airport == idxAirport).ToList();
            else
                return new List<AirportFee>();
        }

        public async static Task DeleteAirportFee(int idxAirportFee, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var fee = await ctx.tset_AirportFeesNew.FirstOrDefaultAsync(x => x.IDX == idxAirportFee);
                if (fee != null)
                    ctx.tset_AirportFeesNew.Remove(fee);
                await ctx.SaveChangesAsync();
                if (airportFeeCacheList!=null)
                {
                    var airportFee = airportFeeCacheList.FirstOrDefault(x => x.IDX == idxAirportFee);
                    if (airportFee != null)
                        airportFeeCacheList.Remove(airportFee);
                }

            }
        }

        public static async Task<List<AirportFee>> LoadAirportFees(String Server, string regionalDBName)
        {
            if (airportFeeCacheList == null)
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var dbFeeTypes = await ctx.tset_AirportFeesNew.Include("tset_airportFeeType").ToListAsync();
                    if (dbFeeTypes != null && dbFeeTypes.Count > 0)
                    {
                        airportFeeCacheList = dbFeeTypes.Select(x => (AirportFee)x).ToList();
                        return airportFeeCacheList;
                    }
                    else
                        return null;
                }
            }
            else
                return airportFeeCacheList;

        }


        public static async Task<List<AirportFee>> LoadAirportFees(List<int> aiportIDXs)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var dbFeeTypes = await ctx.tbAirportFees.Include("tbAirportFeeType").Include("tbCurrency")
                                            .Where(x=>aiportIDXs.Contains(x.pkAirportFeeID)).ToListAsync();
                if (dbFeeTypes != null && dbFeeTypes.Count > 0)
                {
                    var lstAirportFees = dbFeeTypes.Select(x => (AirportFee)x).ToList();
                    return lstAirportFees;
                }
                else
                    return null;
            }
        }

        public static async Task<List<AirportFee>> LoadAirportFee(int idxAiport)
        {

            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var dbFeeTypes = await ctx.tbAirportFees.Include("tbAirportFeeType").Include("tbCurrency")
                                            .Where(x => x.fkAirstripID == idxAiport).ToListAsync();
                if (dbFeeTypes != null && dbFeeTypes.Count > 0)
                {
                    var lstAirportFees = dbFeeTypes.Select(x => (AirportFee)x).ToList();
                    return lstAirportFees;
                }
                else
                    return null;
            }

        }


        public static async Task<List<AirportFee>> LoadAirportFee(int idxAiport, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dbFeeTypes = await ctx.tset_AirportFeesNew.Include("tset_airportFeeType")
                                            .Where(x => x.IDX_Airport == idxAiport).ToListAsync();
                if (dbFeeTypes != null && dbFeeTypes.Count > 0)
                {
                    var lstAirportFees = dbFeeTypes.Select(x => (AirportFee)x).ToList();
                    return lstAirportFees;
                }
                else
                    return null;
            }

        }



        public static async Task SaveAirportFees(List<AirportFee> listAirportFees, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var newItems = listAirportFees.Where(x => x.IsNew).ToList();
                var oldItems = listAirportFees.Except(newItems).ToList();
                if (newItems.Count>0)
                {
                    var lstDBObjects = newItems.Select(x => (tset_AirportFeesNew)x).ToList();
                    ctx.tset_AirportFeesNew.AddRange(lstDBObjects);
                }
               foreach(var item in oldItems)
                {
                    var newDBItem = (tset_AirportFeesNew)item;               
                    ctx.tset_AirportFeesNew.Attach(newDBItem);
                    ctx.Entry(newDBItem).State = EntityState.Modified;
                }
                await ctx.SaveChangesAsync();

            }

        }

    }
}
