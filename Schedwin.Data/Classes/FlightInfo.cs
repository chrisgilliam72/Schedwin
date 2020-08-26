using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
   public class FlightInfo
    {
        public bool IsNew { get; set; }
        public int IDX { get; set; }

        public String Code
        {
            get
            {
                if (Description.Contains("@"))
                {
                    var atIndex = Description.IndexOf('@');
                    return Description.Substring(0, atIndex);
                }
                else
                    return Description;
            }
        }
        public String Description { get; set; }
        
        public int IDX_Airstrip { get; set; }

        public DateTime ArrivalDepartTime { get; set; }

        public DateTime PickupDropOff { get; set; }

        public bool IsInbound { get; set; }

        public bool IsActive { get; set; }

        private static List<FlightInfo> _flightCacheList { get; set; }


        public static explicit operator FlightInfo (tbFlight tbFlight)
        {
            var flightInfo = new FlightInfo();

            flightInfo.IDX = tbFlight.pkFlightID;
            flightInfo.IDX_Airstrip = tbFlight.fkAirstripID ?? -1;
            flightInfo.Description = tbFlight.Description;
            flightInfo.PickupDropOff = tbFlight.TimeIn;
            flightInfo.ArrivalDepartTime = tbFlight.TimeOut;
            flightInfo.IsActive = tbFlight.Active;

            return flightInfo;
        }

        public static explicit operator FlightInfo(tset_Flights_Camps flightCamp)
        {
            var flightInfo = new FlightInfo();

            flightInfo.IDX = flightCamp.IDX;
            flightInfo.IDX_Airstrip = flightCamp.IDX_Airports ?? -1;
            flightInfo.Description = flightCamp.FlightNumberOrCamp;
            flightInfo.IsInbound = flightCamp.Direction == 0;
            flightInfo.PickupDropOff = flightCamp.TimeIn;
            flightInfo.ArrivalDepartTime = flightCamp.TimeOut;
            flightInfo.IsActive = flightCamp.EndOfValidityPeriod > DateTime.Today;

            return flightInfo;

        }

        public static explicit operator tset_Flights_Camps(FlightInfo flightInfo)
        {
            var tsetFlightCamps = new tset_Flights_Camps();
            tsetFlightCamps.IDX = flightInfo.IDX;
            tsetFlightCamps.IDX_Airports = flightInfo.IDX_Airstrip;
            tsetFlightCamps.FlightNumberOrCamp = flightInfo.Description;
            tsetFlightCamps.TimeIn =  new DateTime(2001,01,01,flightInfo.PickupDropOff.Hour, flightInfo.PickupDropOff.Minute,0);
            tsetFlightCamps.TimeOut = new DateTime(2001, 01, 01, flightInfo.ArrivalDepartTime.Hour, flightInfo.ArrivalDepartTime.Minute, 0); 
            tsetFlightCamps.Direction = (short) (flightInfo.IsInbound ? 0 : 1);
            tsetFlightCamps.DayOfWeek = 1;
            tsetFlightCamps.Flight = 1;
            tsetFlightCamps.StartOfValidityPeriod = new DateTime(2000, 01, 01);
            if (flightInfo.IsActive)
                tsetFlightCamps.EndOfValidityPeriod = new DateTime(2100, 01, 01);
            else
                tsetFlightCamps.EndOfValidityPeriod = tsetFlightCamps.StartOfValidityPeriod;
            return tsetFlightCamps;
        }


        public static bool UpdateCachedObject(FlightInfo newFlightObj)
        {
            if (_flightCacheList != null)
            {
                var oldObject = _flightCacheList.FirstOrDefault(x => x.IDX == newFlightObj.IDX);
                if (oldObject != null)
                    _flightCacheList.Remove(oldObject);

                _flightCacheList.Add(newFlightObj);
            }
            return false;



        }



        public static async Task DeactivateFlight(int IDX, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetFlight = await ctx.tset_Flights_Camps.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tsetFlight != null)
                    tsetFlight.EndOfValidityPeriod = DateTime.Today;

                await ctx.SaveChangesAsync();


            }
        }

        public async Task<bool> Save(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var tsetFlightCamps = (tset_Flights_Camps)this;

                    if (IsNew)
                    {
                       
                        ctx.tset_Flights_Camps.Add(tsetFlightCamps);
                    }
                    else
                    {
                        var otsetFlightCamp = await ctx.tset_Flights_Camps.FirstOrDefaultAsync(x => x.IDX == IDX);
                        otsetFlightCamp.TimeIn = tsetFlightCamps.TimeIn;
                        otsetFlightCamp.TimeOut = tsetFlightCamps.TimeOut;
                        otsetFlightCamp.IDX_Airports = tsetFlightCamps.IDX_Airports;
                        otsetFlightCamp.FlightNumberOrCamp = tsetFlightCamps.FlightNumberOrCamp;
                        otsetFlightCamp.Direction = tsetFlightCamps.Direction;
                        otsetFlightCamp.StartOfValidityPeriod = tsetFlightCamps.StartOfValidityPeriod;
                        otsetFlightCamp.EndOfValidityPeriod = tsetFlightCamps.EndOfValidityPeriod;
                    }

                    await ctx.SaveChangesAsync();
                    IDX = tsetFlightCamps.IDX;
                }

              
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                throw new Exception(message);
            }

            return true;
        }


        public static List<FlightInfo> GetFlightList()
        {
            return _flightCacheList;
        }

        static async public Task<FlightInfo> FindFlight(String flightDescription, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var tsetFlight = await ctx.tset_Flights_Camps.FirstOrDefaultAsync(x => x.Flight == 1 && x.DayOfWeek == 1 && x.FlightNumberOrCamp==flightDescription);
                if (tsetFlight!=null)
                {
                    var flightInfo = (FlightInfo)tsetFlight;
                    return flightInfo;
                }
                
            }

            return null;
        }

        public async static Task<List<FlightInfo>> LoadFlightList(bool forceReload)
        {
            if (_flightCacheList != null && !forceReload)
                return _flightCacheList;
            else
            {
                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {                
                    var templst = await ctx.tbFlights.Where(x => x.Active).ToListAsync();
                    _flightCacheList=templst.Select(x => (FlightInfo)x).ToList();
                 
                    return _flightCacheList;
                }
            }
        }

        static async public Task<List<FlightInfo>> LoadFlightList(String Server, string regionalDBName, bool forceReload)
        {
            if (_flightCacheList != null && !forceReload)
                return _flightCacheList;
            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    _flightCacheList = new List<FlightInfo>();

                      var tsetFlights  = await ctx.tset_Flights_Camps.Where(x => x.Flight == 1 && x.DayOfWeek==1).ToListAsync();

                    var templst = tsetFlights.Select(x => (FlightInfo)x).ToList();
                    _flightCacheList.AddRange(templst);

                    return _flightCacheList;
                }
            }
        }

    }
}
