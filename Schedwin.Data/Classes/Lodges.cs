using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Schedwin.Common;

namespace Schedwin.Data.Classes
{
    public class Lodge
    {
        public bool IsNew { get; set; }
        public int IDX { get; set; }
        public int IDX_Country { get; set; }

        public int? IDX_Company { get; set; }
        public int IDX_Airstrip { get; set; }

        public String AirstripIATA { get; set; }
        public  short? NoBeds { get; set; }
        public String Name { get; set; }
        public String Manager { get; set; }

        public String Email { get; set; }

        public String Phone { get; set; }
        
        public DateTime EarliestCheckIn { get; set; }

        public DateTime LatestCheckOut { get; set; }

        static private List<Lodge> _LodgeAirstripcacheList = null;

        public String TPCode { get; set; }

        public bool IsActive { get; set; }

        public static List<Lodge> GetLodgeList()
        {
            return _LodgeAirstripcacheList;

        }


        public static explicit operator Lodge(tbLodge tbLodge)
        {
            var lodge = new Lodge();
            lodge.IDX = tbLodge.pkLodgeID;
            lodge.IDX_Company = tbLodge.fkOperatorID;
            lodge.Name = tbLodge.Name;
            lodge.IsActive = tbLodge.Active;
            lodge.IDX_Country = tbLodge.fkCountryID;

            if (tbLodge.tbAirstrip!=null)
            {
                lodge.IDX_Airstrip = tbLodge.tbAirstrip.pkAirstripID;
                lodge.AirstripIATA = tbLodge.tbAirstrip.IATA;
            }

            lodge.NoBeds = (short)tbLodge.Number_of_Beds ;
            lodge.TPCode = tbLodge.TPCode;
            lodge.Manager = tbLodge.Manager;
            lodge.Email = tbLodge.Email;
            lodge.Phone = tbLodge.Tel_Number;


            lodge.EarliestCheckIn = tbLodge.CheckInTime ?? new DateTime(2020, 01, 01);
            lodge.LatestCheckOut = tbLodge.CheckOutTime ?? new DateTime(2020, 01, 01);



            return lodge;
        }


        public static explicit operator Lodge(tset_Lodges tsetLodge)
        {
            var lodge = new Lodge();
            lodge.IDX = tsetLodge.IDX;
            lodge.IDX_Company = tsetLodge.IDX_Operator;
            lodge.Name = tsetLodge.Lodge;
            lodge.IsActive = tsetLodge.IsActive;
            if (tsetLodge.tset_Airports != null)
            {
                lodge.IDX_Country = tsetLodge.tset_Airports.IDX_Countries;
                lodge.IDX_Airstrip = tsetLodge.tset_Airports.IDX;
                lodge.AirstripIATA = tsetLodge.tset_Airports.IATA;
            }

            lodge.NoBeds = tsetLodge.NumberOfBeds;
            lodge.TPCode = tsetLodge.TPCode;
            lodge.Manager = tsetLodge.Manager;
            lodge.Email = tsetLodge.EMail;
            lodge.Phone = tsetLodge.TelephoneOrCallsign;

    
            lodge.EarliestCheckIn = tsetLodge.CheckInTime ?? new DateTime(2020, 01, 01);
            lodge.LatestCheckOut = tsetLodge.CheckOutTime ?? new DateTime(2020, 01, 01);
           
        

            return lodge;

        }



        public static bool UpdateCachedObject(Lodge newLodgeObj)
        {
            if (_LodgeAirstripcacheList!=null)
            {
                var oldObject = _LodgeAirstripcacheList.FirstOrDefault(x => x.IDX == newLodgeObj.IDX);
                if (oldObject != null)
                    _LodgeAirstripcacheList.Remove(oldObject);

                _LodgeAirstripcacheList.Add(newLodgeObj);
            }
            return false;

        }


        public static async Task DeactivateLodge(int IDX, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tset_lodgeItem = await ctx.tset_Lodges.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tset_lodgeItem != null)
                    tset_lodgeItem.IsActive = false;
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
                    tset_Lodges Lodge;

                    if (!IsNew)
                        Lodge = await ctx.tset_Lodges.FirstOrDefaultAsync(x => x.IDX == IDX);
                    else
                    {
                        Lodge = new tset_Lodges();
                        ctx.tset_Lodges.Add(Lodge);
                    }


                    Lodge.IDX_Airports = IDX_Airstrip;
                    Lodge.Lodge = Name;
                    Lodge.IDX_Operator = IDX_Company;
                    Lodge.EMail = Email;
                    Lodge.CheckInTime = new DateTime(2020,01,01,EarliestCheckIn.Hour, EarliestCheckIn.Minute,00); 
                    Lodge.CheckOutTime = new DateTime(2020, 01, 01, LatestCheckOut.Hour, LatestCheckOut.Minute, 00);
                    Lodge.Manager = Manager;
                    Lodge.NumberOfBeds = NoBeds;
                    Lodge.TPCode = TPCode;
                    Lodge.Indemnity = "";
                    Lodge.CoCode = "CoCode";
                    Lodge.TelephoneOrCallsign = Phone;
                    Lodge.Faxno = "";
                    Lodge.IsActive = IsActive;
                    await ctx.SaveChangesAsync();
                    IDX = Lodge.IDX;

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

        static public async Task<Lodge> FindLodge(String lodgeName, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var tsetLodge = await ctx.tset_Lodges.Include("tset_Airports").
                                                    Include("tset_Flights_Camps").Where(x=>x.Lodge== lodgeName).FirstOrDefaultAsync();
                if (tsetLodge != null)
                {
                    var lodge = (Lodge)tsetLodge;
                    return lodge;
                }

                return null;
            }
        }

        static public async Task<List<Lodge>> LoadLodgeList(bool forceReload)
        {
            if (_LodgeAirstripcacheList != null && !forceReload)
                return _LodgeAirstripcacheList;

            var ctx = new SchedwinGlobalEntities();
            var tbLodgeList = await ctx.tbLodges.Include("tbAirstrip").Where(x => x.Active == true).ToListAsync();

            _LodgeAirstripcacheList = tbLodgeList.Select(x => (Lodge)x).ToList();
            return _LodgeAirstripcacheList;
        }


        static public async Task<List<Lodge>> LoadLodgeList(String Server, string regionalDBName, bool forceReload)
        {
            try
            {
                if (_LodgeAirstripcacheList != null && !forceReload)
                    return _LodgeAirstripcacheList;
                else
                {
                    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                    var ctx = new SchedwinRegionalEntities(constring);

                    using (ctx)
                    {
                        var tsetLodges = await ctx.tset_Lodges.Include("tset_Airports").
                                            Include("tset_Flights_Camps").Where(x => x.IsActive).ToListAsync();

                        _LodgeAirstripcacheList = tsetLodges.Select(x => (Lodge)x).ToList();

                        return _LodgeAirstripcacheList;


                    }
                }
            }

            catch (Exception ex )
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);

                throw new Exception("Error loading lodge list:" + exMessage);
            }

        }
    }
}
