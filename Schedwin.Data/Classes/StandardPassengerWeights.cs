using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class StandardPassengerWeight
    {
        public int CountryIDX { get; set; }

        public int WeightMale { get; set; }

        public int WeightFemale { get; set; }

        public bool IsAdult { get; set; }

        public static explicit operator StandardPassengerWeight(tbStandardPaxWeight tbPaxWeight)
        {
            var stndPaxWeight = new StandardPassengerWeight();
            stndPaxWeight.CountryIDX = tbPaxWeight.fkCountryID;
            stndPaxWeight.IsAdult = true ? tbPaxWeight.MaxRange > 13 : false;
            stndPaxWeight.WeightMale = tbPaxWeight.Male;
            stndPaxWeight.WeightFemale = tbPaxWeight.Female;
            return stndPaxWeight;
        }
    }


    public class StandardPassengerWeights
    {
        private static List<StandardPassengerWeight> _listWeights=null;

        public static async Task LoadStandardWeights()
        {
            if (_listWeights != null)
            {

                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    var weights = await ctx.tbStandardPaxWeights.ToListAsync();
                    _listWeights = weights.Select(x => (StandardPassengerWeight)x).ToList();
                }
            }

        }


        public static async Task LoadStandardWeights(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                _listWeights = new List<StandardPassengerWeight>();

                using (ctx)
                {
                    var weights =await  ctx.tset_StandardPassengerWeights.ToListAsync();
                    foreach (var weight in weights)
                    {
                        var stndWeight = new StandardPassengerWeight();
                        stndWeight.CountryIDX = weight.IDX_Countries;
                        stndWeight.IsAdult = true ? weight.MaxRange > 13 : false;
                        stndWeight.WeightMale = weight.Male;
                        stndWeight.WeightFemale = weight.Female;
                        _listWeights.Add(stndWeight);
                    }
                }
            }
            catch (Exception)
            {
                _listWeights = null;
            }

        }

        public static int GetStandardWeight(int countryID, bool maleWeight, bool isAdult)
        {
            if (_listWeights!=null)
            {
                var stndPxWeight = _listWeights.First(x => x.CountryIDX == countryID && x.IsAdult == isAdult);
                int dftWeight = maleWeight ? stndPxWeight.WeightMale : stndPxWeight.WeightFemale;
                return dftWeight;
            }
            return 0;
        }
        public static int GetStandardWeight(int countryID, bool maleWeight, bool isAdult, String Server, string regionalDBName)
        {
            if (_listWeights == null)
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                _listWeights = new List<StandardPassengerWeight>();

                using (ctx)
                {
                    var weights = ctx.tset_StandardPassengerWeights.ToList();
                    foreach (var weight in weights)
                    {
                        var stndWeight = new StandardPassengerWeight();
                        stndWeight.CountryIDX = weight.IDX_Countries;
                        stndWeight.IsAdult = true ? weight.MaxRange > 13 : false;
                        stndWeight.WeightMale = weight.Male;
                        stndWeight.WeightFemale = weight.Female;
                        _listWeights.Add(stndWeight);
                    }
                }
            }

            var stndPxWeight = _listWeights.First(x => x.CountryIDX == countryID && x.IsAdult == isAdult);
            int dftWeight = maleWeight ? stndPxWeight.WeightMale : stndPxWeight.WeightFemale;
            return dftWeight;
        }
    
    }
}
