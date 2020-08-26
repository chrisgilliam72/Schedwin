using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public enum PriceListType { }
    public class PriceList
    {
        public bool IsNew { get; set; }
        public int IDX { get; set; }
        public int CompanyIDX { get; set; }

        public String Name { get; set; }

        public double BuyRate { get; set; }

        public double SellRate { get; set; }


        public int StartIDX { get; set; }

        public int DestIDX { get; set; }

        public String Start { get; set; }

        public String End { get; set; }

        public String Type { get; set; }

        private static List<PriceList> _priceListCache;

        public static PriceList GetPriceList(int idxCompany, int AP1, int AP2)
        {
            if (_priceListCache != null)
            {
                return _priceListCache.FirstOrDefault(x => x.CompanyIDX == idxCompany &&
                                                    ((x.StartIDX == AP1 && x.DestIDX == AP2) || (x.DestIDX == AP1 && x.StartIDX == AP2)));
            }
            return null;
        }

        public  static async Task<List<PriceList>> GetAllPriceLists(String Server, string regionalDBName)
        {
            if (_priceListCache != null)
                return _priceListCache;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var priceLists = await ctx.tset_OperatorSeatRate.Include("tset_OperatorPriceLists").ToListAsync();

                    _priceListCache = priceLists.Select(x => (PriceList)x).ToList();
                    return _priceListCache;
                }
            }

        }

        public static explicit operator PriceList (tset_OperatorSeatRate tset_priceList)
        {
            var priceList = new PriceList();
            priceList.IDX = tset_priceList.tset_OperatorPriceLists.IDX;
            priceList.CompanyIDX = tset_priceList.tset_OperatorPriceLists.IDXCompanies.Value;
            priceList.Name = tset_priceList.tset_OperatorPriceLists.PriceList;
            priceList.StartIDX = tset_priceList.StartAP;
            priceList.DestIDX = tset_priceList.DestAP;
            priceList.SellRate = tset_priceList.SellSeatRate;
            priceList.BuyRate = tset_priceList.BuySeatRate;
            priceList.Type = "Seat";
            priceList.IsNew = false;
            return priceList;

        }

        public static explicit operator tset_OperatorPriceLists(PriceList priceList)
        {
            var tsetPriceList = new tset_OperatorPriceLists();
            tsetPriceList.IDX = priceList.IDX;
            tsetPriceList.IDXCompanies = priceList.CompanyIDX;
            tsetPriceList.PriceList = priceList.Name;
            switch (priceList.Type)
            {
                case "Seat": var seatRate= new tset_OperatorSeatRate();
                                seatRate.StartAP = priceList.StartIDX;
                                seatRate.DestAP = priceList.DestIDX;
                                seatRate.BuySeatRate = priceList.BuyRate;
                                seatRate.SellSeatRate = priceList.SellRate;
                                tsetPriceList.tset_OperatorSeatRate.Add(seatRate);
                     break;

            }

            return tsetPriceList;
        }

        public async static Task Delete(int IDX, String listType,String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            switch (listType)
            {
                case "Seat":
                    var tset_priceList = await ctx.tset_OperatorPriceLists.Include("tset_OperatorSeatRate").FirstOrDefaultAsync(x => x.IDX == IDX);
                    if (tset_priceList != null)
                    {
                        while (tset_priceList.tset_OperatorSeatRate.Count > 0)
                        {
                            ctx.tset_OperatorSeatRate.Remove(tset_priceList.tset_OperatorSeatRate.FirstOrDefault());
                        }
                        tset_priceList.tset_OperatorSeatRate.Clear();
                        ctx.tset_OperatorPriceLists.Remove(tset_priceList);

                    }
                    else
                        throw new Exception("The pricelist could not be found.");
                    break;
            }

            await ctx.SaveChangesAsync();
        }

        public async Task Save(String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                if (IsNew)
                {
                    var tset_priceList = (tset_OperatorPriceLists)this;
                    foreach (var seatRate in tset_priceList.tset_OperatorSeatRate)
                    {
                        ctx.tset_OperatorSeatRate.Add(seatRate);
                    }
                    ctx.tset_OperatorPriceLists.Add(tset_priceList);

                }
                else
                {
                    switch (Type)
                    {
                        case "Seat": var tset_SeatRate = await ctx.tset_OperatorSeatRate.FirstOrDefaultAsync(x => x.IDXPriceLists==IDX && x.StartAP==StartIDX && x.DestAP== DestIDX );
                                    if (tset_SeatRate!=null)
                                    {
                                        tset_SeatRate.SellSeatRate = SellRate;
                                        tset_SeatRate.BuySeatRate = BuyRate;
                              
                                    }
                                    break;

                    }
                }

                await ctx.SaveChangesAsync();
            }
        }

        public static async Task<List<PriceList>> GetSeatRatePriceList(int companyIDX,String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var priceLists = await ctx.tset_OperatorSeatRate.Include("tset_OperatorPriceLists").Where(x=>x.tset_OperatorPriceLists.IDXCompanies==companyIDX).ToListAsync();

                var seatRatePricelist = priceLists.Select(x => (PriceList)x).ToList();
                return seatRatePricelist;
            }

          

        }
    }
}
