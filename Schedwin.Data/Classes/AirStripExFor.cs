using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public enum AirStripExForType { Lodge, Flight}
   public  class AirStripExFor
    {
       
        public int IDX { get; set; }

        public String Description { get; set; }

        public int IDX_Airstrip { get; set; }

        public DateTime TimeIn { get; set; }

        public DateTime TimeOut { get; set; }

        public AirStripExForType Type { get; set; }

        static private List<AirStripExFor> _globalExForCacheList = null;
        public static List<AirStripExFor> GetExForList()
        {
            return _globalExForCacheList;
        }


        public static explicit operator AirStripExFor (tbLodge lodge)
        {
            var airstripExFor = new AirStripExFor();
            airstripExFor.IDX = lodge.pkLodgeID;
            airstripExFor.Description = lodge.Name;
            airstripExFor.IDX_Airstrip = lodge.fkAirstripID ?? -1;
            airstripExFor.TimeIn = lodge.CheckInTime ?? new DateTime(2000, 1, 1);
            airstripExFor.TimeOut = lodge.CheckOutTime ?? new DateTime(2000, 1, 1);
            airstripExFor.Type = AirStripExForType.Lodge;
            return airstripExFor;
        }

        public static explicit operator AirStripExFor(tbFlight flight)
        {
            var airstripExFor = new AirStripExFor();
            airstripExFor.IDX = flight.pkFlightID;
            airstripExFor.Description = flight.Description;
            airstripExFor.IDX_Airstrip = flight.fkAirstripID ?? -1;
            airstripExFor.TimeIn = flight.TimeIn;
            airstripExFor.TimeOut = flight.TimeOut;
            airstripExFor.Type = AirStripExForType.Flight;
            return airstripExFor;
        }


        static async public Task<List<AirStripExFor>> LoadExForList()
        {
            if (_globalExForCacheList != null)
                return _globalExForCacheList;
            else
            {
                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    _globalExForCacheList = new List<AirStripExFor>();
                    var lodgeExFors = await ctx.tbLodges.Where(x=>x.Active).ToListAsync();
                    var flightExFors = await ctx.tbFlights.Where(x => x.Active).ToListAsync();


                    var templst = lodgeExFors.Select(x => (AirStripExFor)x).ToList();
                    _globalExForCacheList.AddRange(templst);


                    templst = flightExFors.Select(x => (AirStripExFor)x).ToList();
                    _globalExForCacheList.AddRange(templst);

                    return _globalExForCacheList;
                }
            }
        }

    }
}
