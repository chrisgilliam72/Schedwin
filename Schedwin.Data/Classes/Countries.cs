using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class Country
    {
        public int IDX { get; set; }

        public String Name { get; set; }

        public String CurrencyCode { get; set; }

        public int? VATPercentage { get; set; }

        private static List<Country> _CountryCacheList = null;


        public static explicit operator Country (tbCountry tbCountry)
        {
            var country = new Country();
            country.IDX = tbCountry.pkCountryID;
            country.Name = tbCountry.Name;
            country.CurrencyCode = tbCountry.tbCurrency.Code;
            country.VATPercentage = tbCountry.VatPercentage;
            return country;

        }
        public static Country GetCountry(String CountryName)
        {
            if (CountryName.StartsWith("ZZTest_"))
                CountryName = CountryName.Replace("ZZTest_", "");

            if (_CountryCacheList != null)
                return _CountryCacheList.FirstOrDefault(x => x.Name == CountryName);

            return null;

        }

        public static Country GetCountry(int countryID)
        {
            if (_CountryCacheList != null)
                return _CountryCacheList.FirstOrDefault(x => x.IDX == countryID);

            return null;
        }

        public static List<Country> GetCountryList()
        {
            return _CountryCacheList;
        }


        public async static Task<List<Country>> LoadCountries(bool forceReload)
        {
            if (_CountryCacheList != null && !forceReload)
                return _CountryCacheList;
            else
            {
                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    _CountryCacheList = new List<Country>();
                    var countries = await ctx.tbCountries.Include("tbCurrency").Where(x => x.Active).ToListAsync();
                    var CountryLst = countries.Select(x => (Country)x).ToList();

                    _CountryCacheList.AddRange(CountryLst);
                    return _CountryCacheList;
                }
            }
        }

        public async static Task<List<Country>> LoadCountries(String Server, string regionalDBName)
        {
            if (_CountryCacheList != null)
                return _CountryCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    _CountryCacheList = new List<Country>();
                    var countries= await ctx.tset_Countries.Where(x=>x.IsActive).ToListAsync();
                   var CountryLst = countries.Select(x => new Country { IDX = x.IDX, Name = x.Country, VATPercentage=x.VATPercentage }).ToList();

                    _CountryCacheList.AddRange(CountryLst);
                    return _CountryCacheList;
                }
            }
        }
    }
}
