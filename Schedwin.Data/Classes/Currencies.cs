using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class Currency
    {
        public int IDX { get; set; } 
        public  String Code { get; set; }

        public String Description { get; set; }

        private static List<Currency> _GPCurrencyCacheList = null;


        public static List<Currency> GetCurrencyList()
        {
            return _GPCurrencyCacheList;
        }


       
        public async static Task<Currency> GetDefaultCurrency(String Server, string regionalDBName)
        {
            if (_GPCurrencyCacheList == null)
              await GetGPCurrencyList(Server, regionalDBName);

            switch (regionalDBName)
            {

                case "Sefofane_Bots": return _GPCurrencyCacheList.FirstOrDefault(x=>x.Code=="BWP");
                case "Sefofane_Nam": return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "NAD");
                case "Sefofane_Zim": return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "USD"); 
                default: return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "USD"); ; 

            }
        }

        //public async static Task<List<Currency>> GetCurrencyList(String Server, string regionalDBName)
        //{
        //    if (_CurrencyCacheList != null)
        //        return _CurrencyCacheList;
        //    else
        //    {
        //        var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
        //        var ctx = new SchedwinRegionalEntities(constring);

        //        using (ctx)
        //        {
        //            var tmpGPCurList = await ctx.tlst_Currency.ToListAsync();
        //            _CurrencyCacheList = tmpGPCurList.Select(x => new Currency { IDX = x.IDX, Code = x.Currency, Description = x.Currency }).OrderBy(x => x.Code).ToList();
        //            return _CurrencyCacheList;
        //        }
        //    }
        //}

        public async static Task<Currency> GetDefaultCurrency(String region)
        {
            if (_GPCurrencyCacheList == null)
                await LoadCurrencies();

            switch (region)
            {

                case "Sefofane_Bots": return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "BWP");
                case "Sefofane_Nam": return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "NAD");
                case "Sefofane_Zim": return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "USD");
                default: return _GPCurrencyCacheList.FirstOrDefault(x => x.Code == "USD"); ;

            }
        }


        public async static Task<List<Currency>> LoadCurrencies()
        {
            if (_GPCurrencyCacheList != null)
                return _GPCurrencyCacheList;
            else
            {
                var ctx = new SchedwinGlobalEntities();

                using (ctx)
                {
                    var tmpGPCurList = await ctx.tbCurrencies.ToListAsync();
                    _GPCurrencyCacheList = tmpGPCurList.Select(x => new Currency { IDX = x.pkCurrencyID, Code = x.Code.TrimEnd(' '), Description = x.Name }).OrderBy(x => x.Code).ToList();
                    return _GPCurrencyCacheList;
                }
            }
        }

        public async static Task<List<Currency>> GetGPCurrencyList(String Server, string regionalDBName)
        {
            if (_GPCurrencyCacheList != null)
                return _GPCurrencyCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var tmpGPCurList = await ctx.tGP_CurrencyInformation.ToListAsync();
                    _GPCurrencyCacheList = tmpGPCurList.Select(x => new Currency { IDX = x.CURRNIDX, Code = x.CURNCYID.TrimEnd(' '), Description = x.CRNCYDSC }).OrderBy(x=>x.Code).ToList();
                    return _GPCurrencyCacheList;
                }
            }
        }
    } 
}
