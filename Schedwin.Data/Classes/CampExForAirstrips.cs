using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class CampExForAirstrips
    {
        static private List<tset_Airports> _airportcacheList=null;
        static private List<AirStripExFor> _globalExForCacheList = null;
        public static List<AirStripExFor> GetExForList()
        {
            return _globalExForCacheList;
        }

        static async public Task<List<tset_Airports>> GetAirportList(String Server, string regionalDBName)
        {
            if (_airportcacheList != null)
                return _airportcacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var today = DateTime.Today;
                    var items = await ctx.tset_Airports.ToListAsync();
                    items.ForEach(x => x.Airport=x.Airport.ToLower());
                    _airportcacheList = items;
                    return items;
                }
            }

        }
        static async public Task<bool> RemoveFlight(String flightDescription, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {

                    var items = ctx.tset_Flights_Camps.Where(x => x.FlightNumberOrCamp == flightDescription);
                    if (items!=null)
                    {
                        ctx.tset_Flights_Camps.RemoveRange(items);
                        await ctx.SaveChangesAsync();
                        if (_globalExForCacheList != null)
                        {
                            var cacheItems = _globalExForCacheList.Where(x => x.Description == flightDescription);
                            foreach (var cacheItem in cacheItems)
                                _globalExForCacheList.Remove(cacheItem);

                        }
                    }


                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        static async public Task<List<AirStripExFor>> GetExForListV2(String Server, string regionalDBName)
        {
            if (_globalExForCacheList != null)
                return _globalExForCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    _globalExForCacheList = new List<AirStripExFor>();
                    var lodgeExFors = await ctx.tset_Lodges.ToListAsync();
                    var FlightExFors = await ctx.tset_Flights_Camps.Where(x=>x.Flight==1).ToListAsync();

                    var templst = lodgeExFors.Select(x => new AirStripExFor { IDX = x.IDX,TimeIn=x.CheckInTime ?? new DateTime(2000,1,1),
                                                                                TimeOut =x.CheckOutTime ?? new DateTime(2000,1,1) ,
                                                                                IDX_Airstrip = x.IDX_Airports, Description = x.Lodge, Type = AirStripExForType.Lodge });
                    _globalExForCacheList.AddRange(templst);

                    var tmpDistinctLst=FlightExFors.GroupBy(x => x.FlightNumberOrCamp).Select(x => x.First()).ToList();
                    templst = tmpDistinctLst.Select(x => new AirStripExFor { IDX = x.IDX,IDX_Airstrip=x.IDX_Airports.Value,TimeIn=x.TimeIn, TimeOut=x.TimeOut,Description = x.FlightNumberOrCamp, Type = AirStripExForType.Flight }); ;
                    _globalExForCacheList.AddRange(templst);

                    return _globalExForCacheList;
                }
            }
        }

    }
}
