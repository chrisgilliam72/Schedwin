using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;
using System.Data.Entity;

namespace Schedwin.Reservations.Classes
{
    public class GPCurrency
    {
        public String CurrencyCode { get; set; }
        public int CurrencyID { get; set; }

        public String Description { get; set; }
    }

    public class ReservationType
    {
        public int IDX { get; set; }
        public String Description { get; set; }
    }

    public class ReservationStatus
    {
        public int IDX { get; set; }
        public String Description { get; set; }
    }

    public class ReservationAirport
    {
        public int IDX { get; set; }
        public String IATA { get; set; }

        public String Description { get; set; }

        public String DisplayString
        {
            get
            {
                return IATA + @" " + Description + @" ";
            }
        }

    }

    public class ReservationACType
    {
        public int IDX { get; set; }
        public String ACType { get; set; }
    }

    public class ReservationCompanyOperators
    {
        public int IDX { get; set; }
        public String Agent { get; set; }
        public int IDX_Operator { get; set; }
        public String CompanyName { get; set; }
    }

    public class ReservationStdWeight
    {
        public int MinRange { get; set; }
        public int MaxRange { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Luggage { get; set; }

    }

    public class ReservationFormDropdownLists
    {
        public static List<GPCurrency> CurrencyList { get; set; }
        public static List<ReservationType> ReservationTypeList { get; set; }
        public static List<ReservationStatus> ReservatonStatusList { get; set; }
        public static List<ReservationAirport> ReservationAirportList { get; set;}
        public static List<ReservationACType> ReservationACTypeList { get; set; }
        public static List<ReservationCompanyOperators> ReservationCompanyOperatorList { get; set; }
        public static List<ReservationStdWeight> ReservationStdWeightList { get; set; }



        public static List<ReservationCompanyOperators> GetAgents(int companyID)
        {
            return ReservationCompanyOperatorList.Where(x => x.IDX_Operator == companyID).OrderBy(x => x.Agent).ToList();
        }

        public static async Task<bool> LoadLists(int countryID , String Server, string regionalDBName)
        {
            try
            {
                CurrencyList = new List<GPCurrency>();
                ReservationTypeList = new List<ReservationType>();
                ReservatonStatusList = new List<ReservationStatus>();
                ReservationAirportList = new List<ReservationAirport>();
                ReservationACTypeList = new List<ReservationACType>();
                ReservationCompanyOperatorList = new List<ReservationCompanyOperators>();
                ReservationStdWeightList = new List<ReservationStdWeight>();

                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var tgCurrList = await ctx.tGP_CurrencyInformation.ToListAsync();
                    var gpCurrencyList= tgCurrList.Select(x => new GPCurrency { CurrencyCode = x.CURNCYID, CurrencyID = x.CURRNIDX, Description = x.CRNCYDSC }).ToList();
                    CurrencyList.AddRange(gpCurrencyList.OrderBy(x=>x.CurrencyCode));

                    var dbResList= await ctx.tlst_ResType.ToListAsync();
                    var resTypeLst = dbResList.Select(x => new ReservationType { IDX = x.IDX, Description = x.ResType });
                    ReservationTypeList.AddRange(resTypeLst.OrderBy(x => x.Description));

                    var dbStatLst = await ctx.tlst_ResStatus.ToListAsync();
                    var resStatusList = dbStatLst.Select(x => new ReservationStatus { IDX = x.IDX, Description = x.ReservationStatus });
                    ReservatonStatusList.AddRange(resStatusList.OrderBy(x=>x.Description));

                    var dbAirportsLst = await ctx.tset_Airports.ToListAsync();
                    var resAirportLst = dbAirportsLst.Select(x => new ReservationAirport { IDX = x.IDX, IATA = x.IATA, Description = x.Airport }).ToList();
                    ReservationAirportList.AddRange(resAirportLst.OrderBy(x => x.IATA));

                    var dbACTypes = await ctx.tset_ACTypes.ToListAsync();
                    var resACTypes = dbACTypes.Select(x => new ReservationACType { IDX = x.IDX, ACType = x.ACType }).ToList();
                    ReservationACTypeList.AddRange(resACTypes.OrderBy(x => x.ACType));

                    var vlCompanyOperators = await ctx.vl_CompanyOperatorsAgents.Where(x => x.Active).ToListAsync();
                    var resCOOperators = vlCompanyOperators.Select(x => new ReservationCompanyOperators { IDX = x.IDX, Agent = x.Agent, IDX_Operator = x.IDX_Operator, CompanyName = x.CompanyName }).ToList();
                    ReservationCompanyOperatorList.AddRange(resCOOperators.OrderBy(x => x.Agent));

                    var dbStdWeights = await ctx.tset_StandardPassengerWeights.Where(x => x.IDX_Countries == countryID).ToListAsync();
                    var resStdWeights = dbStdWeights.Select(x => new ReservationStdWeight { MinRange = x.MinRange, MaxRange = x.MaxRange, Female = x.Female, Male = x.Male, Luggage = x.Luggage });
                    ReservationStdWeightList.AddRange(resStdWeights);
                }
                return true; 
            }

            catch (Exception)
            {
                return false;
            }

        }
    } 
}
