using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Schedwin.Common;
using System.Data.Entity.Validation;

namespace Schedwin.Data.Classes
{
    public class AirstripInfo
    {
        private static List<AirstripInfo> _listAirstrips = null;

        public bool IsNew { get; set; }

        public bool IsActive { get; set; }
        public int IDX { get; set; }

        public int IDX_Alt { get; set; }

        public int IDX_Country { get; set; }

        public short Altitude { get; set; }
        public String ICAO { get; set; }
        public String Code { get; set; }

        public String TPCode { get; set; }
        public String AlternateCode { get; set; }
        public String AreaCode {get;set;}
        public String Description { get; set; }

        public short TurnAroundTime { get; set; }

        public int AltDistance { get; set;}

        public String Latitude { get; set; }

        public String Longitude { get; set; }

        public float RunwayHeading { get; set; }

       public short RunwayLength { get; set; }

        public float SurfaceFactor { get; set; }
        
        public float OvernightFee { get; set; }

        public float DepTaxInternal { get; set; } 

        public float DepTaxInternational { get; set; }

        public float TasPermitFee { get; set; }

        public bool FuelPoint { get; set; }

        public bool CustomsPoint { get; set; }
        public bool Heliport { get; set; }

        public double PilotNightStop { get; set; }

        public int IDX_CurrencyType { get; set; }

        public DateTime OpeningTime { get; set; }
        public DateTime ClosingTime { get; set; }

        public double DecimalLong { get; set; }
        public double DecimalLat { get; set; }


        public static explicit operator AirstripInfo(tset_Airports tsetAirport)
        {
            var airstripInfo = new AirstripInfo();
            airstripInfo.IsActive = tsetAirport.Active ?? false;
            airstripInfo.IDX = tsetAirport.IDX;
            airstripInfo.IDX_Alt = tsetAirport.IDX_AlternateAirport;
            airstripInfo.IDX_Country = tsetAirport.IDX_Countries;
            airstripInfo.ICAO = tsetAirport.Designator;
            airstripInfo.Code = tsetAirport.IATA;
            airstripInfo.AreaCode = tsetAirport.AreaCode;
            airstripInfo.Description = tsetAirport.Airport;
            airstripInfo.TPCode = tsetAirport.TPCode;
            airstripInfo.TurnAroundTime = tsetAirport.TurnaroundTime;
            if (tsetAirport.tset_Airports2!=null)
                airstripInfo.AlternateCode = tsetAirport.tset_Airports2.AreaCode;
            airstripInfo.AltDistance = tsetAirport.AlternateDist;
            airstripInfo.Latitude = tsetAirport.Latitude;
            airstripInfo.Longitude = tsetAirport.Longitude;
            airstripInfo.RunwayHeading = tsetAirport.RunwayHeading;
            airstripInfo.RunwayLength = tsetAirport.RunwayLength;
            airstripInfo.SurfaceFactor = tsetAirport.SurfaceFactor;
            airstripInfo.TurnAroundTime = tsetAirport.TurnaroundTime;
            airstripInfo.OvernightFee = tsetAirport.OvernightFee;
            airstripInfo.DepTaxInternal = tsetAirport.DepTaxInternal;
            airstripInfo.DepTaxInternational = tsetAirport.DepTaxInternational;
            airstripInfo.TasPermitFee = tsetAirport.TASPermitFee;
            airstripInfo.FuelPoint = tsetAirport.FuelPoint;
            airstripInfo.CustomsPoint = tsetAirport.CustomsPoint;
            airstripInfo.PilotNightStop = tsetAirport.PilotNightStop;
            airstripInfo.IDX_CurrencyType = tsetAirport.IDX_CurrencyType;
            airstripInfo.Heliport = tsetAirport.IsHeliport;
            airstripInfo.Altitude = tsetAirport.Altitude;
            airstripInfo.OpeningTime = tsetAirport.Sunrise;
            airstripInfo.ClosingTime = tsetAirport.Sunset;
            airstripInfo.DecimalLong = tsetAirport.LongitudeDecimal ?? 0;
            airstripInfo.DecimalLat = tsetAirport.LatitudeDecimal ?? 0;
            return airstripInfo;
        }

        public static explicit operator AirstripInfo(tbAirstrip tbAirstrip)
        {
            var airstripInfo = new AirstripInfo();
            airstripInfo.IsActive = tbAirstrip.Active ?? false;
            airstripInfo.IDX = tbAirstrip.pkAirstripID;
            airstripInfo.IDX_Alt = tbAirstrip.fkAlternateAirport ?? -1;
            airstripInfo.IDX_Country = tbAirstrip.fkCountryID;
            airstripInfo.ICAO = tbAirstrip.Designator;
            airstripInfo.Code = tbAirstrip.IATA;
            airstripInfo.AreaCode = tbAirstrip.AreaCode;
            airstripInfo.Description = tbAirstrip.Name;
            airstripInfo.TPCode = tbAirstrip.TPCode;
            airstripInfo.TurnAroundTime = tbAirstrip.TurnaroundTime;
            //if (tsetAirport.tset_Airports2 != null)
            //    airstripInfo.AlternateCode = tsetAirport.tset_Airports2.AreaCode;
            airstripInfo.AltDistance = tbAirstrip.AlternateDist;
            airstripInfo.Latitude = tbAirstrip.Latitude;
            airstripInfo.Longitude = tbAirstrip.Longitude;
            airstripInfo.RunwayHeading = tbAirstrip.RunwayHeading;
            airstripInfo.RunwayLength = tbAirstrip.RunwayLength;
            airstripInfo.SurfaceFactor = tbAirstrip.SurfaceFactor;
            airstripInfo.TurnAroundTime = tbAirstrip.TurnaroundTime;
            airstripInfo.OvernightFee = tbAirstrip.OvernightFee;
            airstripInfo.DepTaxInternal = tbAirstrip.DepTaxInternal;
            airstripInfo.DepTaxInternational = tbAirstrip.DepTaxInternational;
            airstripInfo.TasPermitFee = tbAirstrip.TASPermitFee;
            airstripInfo.FuelPoint = tbAirstrip.FuelPoint;
            airstripInfo.CustomsPoint = tbAirstrip.CustomsPoint;
            airstripInfo.PilotNightStop = tbAirstrip.PilotNightStop;
            airstripInfo.IDX_CurrencyType = tbAirstrip.fkCurrencyID ?? -1;
            airstripInfo.Heliport = tbAirstrip.IsHeliport;
            airstripInfo.Altitude = tbAirstrip.Altitude;
            airstripInfo.OpeningTime = tbAirstrip.Sunrise;
            airstripInfo.ClosingTime = tbAirstrip.Sunset;
            airstripInfo.DecimalLong = tbAirstrip.LongitudeDecimal ?? 0;
            airstripInfo.DecimalLat = tbAirstrip.LatitudeDecimal ?? 0;
            return airstripInfo;
        }


        public static explicit operator tbAirstrip(AirstripInfo airstripInfo)
        {
            var tbAirstrip = new tbAirstrip();

            tbAirstrip.pkAirstripID = airstripInfo.IDX;
            tbAirstrip.Active = airstripInfo.IsActive;
            tbAirstrip.Altitude = airstripInfo.Altitude;
            //tbAirstrip.IDX_AlternateAirport = airstripInfo.IDX_Alt;
            tbAirstrip.fkCountryID = airstripInfo.IDX_Country;
            tbAirstrip.AreaCode = airstripInfo.AreaCode;
            tbAirstrip.IATA = airstripInfo.Code;
            tbAirstrip.TPCode = airstripInfo.TPCode;
            tbAirstrip.Designator = airstripInfo.ICAO;
            tbAirstrip.Name = airstripInfo.Description;
            tbAirstrip.TurnaroundTime = airstripInfo.TurnAroundTime;
            tbAirstrip.AlternateDist = airstripInfo.AltDistance;
            tbAirstrip.Latitude = airstripInfo.Latitude;
            tbAirstrip.Longitude = airstripInfo.Longitude;
            tbAirstrip.RunwayHeading = airstripInfo.RunwayHeading;
            tbAirstrip.RunwayLength = airstripInfo.RunwayLength;
            tbAirstrip.SurfaceFactor = airstripInfo.SurfaceFactor;
            tbAirstrip.OvernightFee = airstripInfo.OvernightFee;
            tbAirstrip.DepTaxInternal = airstripInfo.DepTaxInternal;
            tbAirstrip.DepTaxInternational = airstripInfo.DepTaxInternational;
            tbAirstrip.TASPermitFee = airstripInfo.TasPermitFee;
            tbAirstrip.FuelPoint = airstripInfo.FuelPoint;
            tbAirstrip.CustomsPoint = airstripInfo.CustomsPoint;
            tbAirstrip.Designator = airstripInfo.ICAO;
            tbAirstrip.PilotNightStop = airstripInfo.PilotNightStop;
            tbAirstrip.fkCurrencyID = airstripInfo.IDX_CurrencyType;
            tbAirstrip.IsHeliport = airstripInfo.Heliport;
            tbAirstrip.Sunrise = airstripInfo.OpeningTime;
            tbAirstrip.Sunset = airstripInfo.ClosingTime;
            tbAirstrip.LatitudeDecimal = airstripInfo.DecimalLat;
            tbAirstrip.LongitudeDecimal = airstripInfo.DecimalLong;
            return tbAirstrip;
        }

        public static  explicit operator tset_Airports(AirstripInfo airstripInfo)
        {
            var tsetAirport = new tset_Airports();

            tsetAirport.IDX = airstripInfo.IDX;
            tsetAirport.Active = airstripInfo.IsActive;
            tsetAirport.Altitude = airstripInfo.Altitude;
            tsetAirport.IDX_AlternateAirport = airstripInfo.IDX_Alt;
            tsetAirport.IDX_Countries = airstripInfo.IDX_Country;
            tsetAirport.AreaCode = airstripInfo.AreaCode;
            tsetAirport.IATA = airstripInfo.Code;
            tsetAirport.TPCode = airstripInfo.TPCode;
            tsetAirport.Designator = airstripInfo.ICAO;
            tsetAirport.Airport = airstripInfo.Description;
            tsetAirport.TurnaroundTime = airstripInfo.TurnAroundTime;
            tsetAirport.AlternateDist = airstripInfo.AltDistance;
            tsetAirport.Latitude = airstripInfo.Latitude;
            tsetAirport.Longitude = airstripInfo.Longitude;
            tsetAirport.RunwayHeading = airstripInfo.RunwayHeading;
            tsetAirport.RunwayLength = airstripInfo.RunwayLength;
            tsetAirport.SurfaceFactor = airstripInfo.SurfaceFactor;
            tsetAirport.OvernightFee = airstripInfo.OvernightFee;
            tsetAirport.DepTaxInternal = airstripInfo.DepTaxInternal;
            tsetAirport.DepTaxInternational = airstripInfo.DepTaxInternational;
            tsetAirport.TASPermitFee = airstripInfo.TasPermitFee;
            tsetAirport.FuelPoint = airstripInfo.FuelPoint;
            tsetAirport.CustomsPoint = airstripInfo.CustomsPoint;
            tsetAirport.Designator = airstripInfo.ICAO;
            tsetAirport.PilotNightStop = airstripInfo.PilotNightStop;
            tsetAirport.IDX_CurrencyType = airstripInfo.IDX_CurrencyType;
            tsetAirport.IsHeliport = airstripInfo.Heliport;
            tsetAirport.Sunrise = airstripInfo.OpeningTime;
            tsetAirport.Sunset = airstripInfo.ClosingTime;
            tsetAirport.LatitudeDecimal = airstripInfo.DecimalLat;
            tsetAirport.LongitudeDecimal = airstripInfo.DecimalLong;
            return tsetAirport;
        }

        public String DisplayString
        {
            get
            {
                return Code + " " + Description + " ";
            }
        }

        public static async Task DeactivateAirstrip(int IDX, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetAirport = await ctx.tset_Airports.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tsetAirport != null)
                    tsetAirport.Active = false;
                await ctx.SaveChangesAsync();


            }
        }


        public static bool UpdateCachedObject(AirstripInfo newAirstripObj)
        {
            if (_listAirstrips != null)
            {
                var oldObject = _listAirstrips.FirstOrDefault(x => x.IDX == newAirstripObj.IDX);
                if (oldObject != null)
                    _listAirstrips.Remove(oldObject);

                _listAirstrips.Add(newAirstripObj);
            }
            return false;

        }
        public async Task<bool> Save(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {

                    tset_Airports tsetAirport;

                    if (!IsNew)
                    {
                        tsetAirport = ctx.tset_Airports.FirstOrDefault(x => x.IDX == IDX);
                        tsetAirport.Altitude = Altitude;
                        tsetAirport.IDX_AlternateAirport = IDX_Alt;
                        tsetAirport.IDX_Countries = IDX_Country;
                        tsetAirport.AreaCode = AreaCode;
                        tsetAirport.IATA = Code;
                        tsetAirport.Designator = ICAO;
                        tsetAirport.Airport = Description;
                        tsetAirport.TurnaroundTime = TurnAroundTime;
                        tsetAirport.AlternateDist = AltDistance;
                        tsetAirport.Latitude = Latitude;
                        tsetAirport.Longitude = Longitude;
                        tsetAirport.RunwayHeading = RunwayHeading;
                        tsetAirport.RunwayLength = RunwayLength;
                        tsetAirport.SurfaceFactor = SurfaceFactor;
                        tsetAirport.OvernightFee = OvernightFee;
                        tsetAirport.DepTaxInternal = DepTaxInternal;
                        tsetAirport.DepTaxInternational = DepTaxInternational;
                        tsetAirport.TASPermitFee = TasPermitFee;
                        tsetAirport.FuelPoint = FuelPoint;
                        tsetAirport.CustomsPoint = CustomsPoint;
                        tsetAirport.Designator = ICAO;
                        tsetAirport.PilotNightStop = PilotNightStop;
                        tsetAirport.IDX_CurrencyType = IDX_CurrencyType;
                        tsetAirport.IsHeliport = Heliport;
                        tsetAirport.Sunrise = OpeningTime;
                        tsetAirport.Sunset = ClosingTime;
                        tsetAirport.LatitudeDecimal = DecimalLat;
                        tsetAirport.LongitudeDecimal = DecimalLong;
                        tsetAirport.TPCode = TPCode;
                        tsetAirport.Active = true;
                    }
                    else
                    {
                        tsetAirport = (tset_Airports)this;
                        ctx.tset_Airports.Add(tsetAirport);
                    }

                    await ctx.SaveChangesAsync();
                    IDX = tsetAirport.IDX;
                    return true;
                }
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("\"{0}\"  has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                throw new Exception(sb.ToString());

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                throw new Exception(message);
            }
        }

        public static String GetAirstripCode(int airStripIDX)
        {
            if (_listAirstrips != null && _listAirstrips.FirstOrDefault(x => x.IDX == airStripIDX)!=null)
            {
                return _listAirstrips.FirstOrDefault(x => x.IDX == airStripIDX).Code;
            }

            return "";
        }

        public static int GetAirstripIDX(String airStripCode)
        {
            if (_listAirstrips != null && _listAirstrips.FirstOrDefault(x => x.Code == airStripCode) != null)
            {
                return _listAirstrips.FirstOrDefault(x => x.Code == airStripCode).IDX;
            }

            return -1;
        }

        public static AirstripInfo GetAirstripInfo(int airStripIDX)
        {
            if (_listAirstrips != null)
            {
                return _listAirstrips.FirstOrDefault(x => x.IDX == airStripIDX);
            }

            return null;
        }

        public static  AirstripInfo GetAirstripInfo(String airStripCode)
        {
            if (_listAirstrips!=null)
            {
                return _listAirstrips.FirstOrDefault(x => x.Code == airStripCode);
            }

            return null;
        }


        public static async Task<AirstripInfo> FindAirstrip(String airstripName, String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetAirport= await ctx.tset_Airports.FirstOrDefaultAsync(x => x.Airport.ToLower() == airstripName.ToLower());
                if (tsetAirport != null)
                {
                    var airstripInfo = (AirstripInfo)tsetAirport;
                    return airstripInfo;
                }

                return null;
            }

        }

        public static async Task<List<AirstripInfo>> LoadAirstrips()
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var airportLst = await ctx.tbAirstrips.Where(x => x.Active == true).ToListAsync();
                _listAirstrips = airportLst.Select(x => (AirstripInfo)x).ToList();
                return _listAirstrips;
            }
        }


        public static async Task<List<AirstripInfo>> LoadAirstrips(String Server, string regionalDBName)
        {

            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var airportLst = await ctx.tset_Airports.Include("tset_Airports2").Where(x => x.Active == true).ToListAsync();
                    _listAirstrips = airportLst.Select(x => (AirstripInfo)x).ToList();
                    return _listAirstrips;
                }
              
            }
            catch (Exception ex)
            {

                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);

                throw new Exception("Error loading airstrip list: "+Environment.NewLine + exMessage);
            }

        }

       public static List<AirstripInfo> GetAirstrips()
       {
            if (_listAirstrips!=null)
                return _listAirstrips.OrderBy(x=>x.Code).ToList();
            return null;
        }
    }
}
