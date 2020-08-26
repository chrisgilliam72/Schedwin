using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class AircraftInfo
    {
        public bool IsNew { get; set; }
        public int IDX { get; set; }
        public String Registration { get; set; }

        public String ProfitCenter { get; set; }

        public int IDX_AC_Type { get; set; }

        public int IDX_Owner { get; set; }

        public float SellRate { get; set; }

        public float  BuyRate { get; set; }

        public int ReserveFuel { get; set; }

        public String SerialNumber { get; set; }

        public DateTime? YearOfManufacture { get; set; }
        public DateTime? DateOfTotalTime { get; set; }

        public double TotalTimeAirframe { get; set; }
        public bool Active { get; set; }

        public bool OwnAircraft { get; set; }

        public Double EmptyMass { get; set; }
        
        public Double EmptyArm { get; set; }

        public String Colours { get; set; }

        public String Equipment { get; set; }

        public int AverageSpeed { get; set; }

        public int Demurrage { get; set; }

        public float ShortCycleFee { get; set; }

        public int TotalFuel { get; set; }

        public int DOC { get; set; }

        public String InvoiceCode { get; set; }

        public int?  IDX_UnderWriter { get; set; }

        public DateTime InsuranceExpiryDate { get; set; }

        public String Liability { get; set; }


        public static explicit operator AircraftInfo(tbAircraft aircraftDetails)
        {
            var aircraftInfo = new AircraftInfo();
            aircraftInfo.IsNew = false;
            aircraftInfo.IDX = aircraftDetails.pkAircraftID;
            aircraftInfo.IDX_AC_Type = aircraftDetails.fkAircraftTypeID ?? 0;
            aircraftInfo.IDX_Owner = aircraftDetails.fkOwnerID ?? 0;
            aircraftInfo.Registration = aircraftDetails.Registration;
            aircraftInfo.EmptyMass = aircraftDetails.EmptyMass;
            aircraftInfo.BuyRate = aircraftDetails.BuyingRate;
            aircraftInfo.SellRate = aircraftDetails.SellingRate;
            aircraftInfo.ReserveFuel = aircraftDetails.Reserve_Fuel ?? 0;
            aircraftInfo.SerialNumber = aircraftDetails.Serial_Number;
            aircraftInfo.YearOfManufacture = aircraftDetails.YOM ?? null;
            aircraftInfo.DateOfTotalTime = aircraftDetails.DTT ?? null;
            aircraftInfo.Active = Convert.ToBoolean(aircraftDetails.Active);
            aircraftInfo.OwnAircraft = Convert.ToBoolean(aircraftDetails.OwnAircraft);
            aircraftInfo.TotalTimeAirframe = aircraftDetails.TTA ?? 0;
            aircraftInfo.EmptyArm = aircraftDetails.EmptyArm;
            aircraftInfo.Colours = aircraftDetails.Colours;
            aircraftInfo.Equipment = aircraftDetails.Equipment;
            aircraftInfo.AverageSpeed = aircraftDetails.AvgSpeed;
            aircraftInfo.Demurrage = aircraftDetails.Demurrage ?? 0;
            aircraftInfo.ShortCycleFee = aircraftDetails.ShortCycleFee ?? 0;
            aircraftInfo.TotalFuel = aircraftDetails.TotalFuelWT;
            aircraftInfo.Liability = aircraftDetails.Liability;
            aircraftInfo.InvoiceCode = aircraftDetails.InvoiceCode;
            aircraftInfo.IDX_UnderWriter = aircraftDetails.fkUnderwriterID ?? 0;
            aircraftInfo.InsuranceExpiryDate = aircraftDetails.InsuranceExp;
            aircraftInfo.ProfitCenter = aircraftDetails.ProfitCenter;
            return aircraftInfo;
        }

        public static explicit operator AircraftInfo(tset_ACDetails tsetACDetails)
        {
            var aircraftInfo = new AircraftInfo();
            aircraftInfo.IsNew = false;
            aircraftInfo.IDX = tsetACDetails.IDX;
            aircraftInfo.IDX_AC_Type = tsetACDetails.IDX_ACTypes;
            aircraftInfo.IDX_Owner = tsetACDetails.IDX_Owner ?? 0;
            aircraftInfo.Registration = tsetACDetails.Registration;
            aircraftInfo.EmptyMass = tsetACDetails.EmptyMass;
            aircraftInfo.BuyRate = tsetACDetails.BuyingRate;
            aircraftInfo.SellRate = tsetACDetails.SellingRate;
            aircraftInfo.ReserveFuel = tsetACDetails.Reserve_Fuel ?? 0;
            aircraftInfo.SerialNumber = tsetACDetails.Serial_Number;
            aircraftInfo.YearOfManufacture = tsetACDetails.YOM ?? null;
            aircraftInfo.DateOfTotalTime = tsetACDetails.DTT ?? null;
            aircraftInfo.Active = Convert.ToBoolean(tsetACDetails.Active);
            aircraftInfo.OwnAircraft = Convert.ToBoolean(tsetACDetails.OwnAircraft);
            aircraftInfo.TotalTimeAirframe = tsetACDetails.TTA ?? 0;
            aircraftInfo.EmptyArm = tsetACDetails.EmptyArm;
            aircraftInfo.Colours = tsetACDetails.Colours;
            aircraftInfo.Equipment = tsetACDetails.Equipment;
            aircraftInfo.AverageSpeed = tsetACDetails.AvgSpeed;
            aircraftInfo.Demurrage = tsetACDetails.Demurrage ?? 0;
            aircraftInfo.ShortCycleFee = tsetACDetails.ShortCycleFee ?? 0 ;
            aircraftInfo.TotalFuel = tsetACDetails.TotalFuelWT;
            aircraftInfo.Liability = tsetACDetails.Liability;
            aircraftInfo.InvoiceCode = tsetACDetails.InvoiceCode;
            aircraftInfo.IDX_UnderWriter = tsetACDetails.IDX_UnderWriter  ?? 0;
            aircraftInfo.InsuranceExpiryDate = tsetACDetails.InsuranceExp;
            aircraftInfo.ProfitCenter = tsetACDetails.ProfitCenter;
            return aircraftInfo;
        }

        public static explicit operator tset_ACDetails(AircraftInfo aircraftInfo)
        {
            var tsetACDetails = new tset_ACDetails();
            tsetACDetails.IDX = aircraftInfo.IDX;
            tsetACDetails.IDX_ACTypes= aircraftInfo.IDX_AC_Type;
            tsetACDetails.IDX_Owner = aircraftInfo.IDX_Owner;
            tsetACDetails.Registration = aircraftInfo.Registration;
            tsetACDetails.EmptyMass = aircraftInfo.EmptyMass;
            tsetACDetails.BuyingRate= aircraftInfo.BuyRate;
            tsetACDetails.SellingRate = aircraftInfo.SellRate;
            tsetACDetails.Reserve_Fuel = aircraftInfo.ReserveFuel;
            tsetACDetails.Serial_Number = aircraftInfo.SerialNumber;
            tsetACDetails.YOM = aircraftInfo.YearOfManufacture;
            tsetACDetails.DTT = aircraftInfo.DateOfTotalTime;
            tsetACDetails.Active = Convert.ToByte(aircraftInfo.Active);
            tsetACDetails.OwnAircraft = Convert.ToByte(aircraftInfo.OwnAircraft);
            tsetACDetails.TTA = aircraftInfo.TotalTimeAirframe;
            tsetACDetails.EmptyArm = aircraftInfo.EmptyArm;
            tsetACDetails.Colours = aircraftInfo.Colours;
            tsetACDetails.Equipment = aircraftInfo.Equipment;
            tsetACDetails.AvgSpeed = aircraftInfo.AverageSpeed;
            tsetACDetails.Demurrage = aircraftInfo.Demurrage;
            tsetACDetails.ShortCycleFee = aircraftInfo.ShortCycleFee;
            tsetACDetails.TotalFuelWT = aircraftInfo.TotalFuel;
            tsetACDetails.Liability = aircraftInfo.Liability;
            tsetACDetails.InvoiceCode = aircraftInfo.InvoiceCode;
            tsetACDetails.IDX_UnderWriter = aircraftInfo.IDX_UnderWriter;
            tsetACDetails.InsuranceExp = aircraftInfo.InsuranceExpiryDate;
            tsetACDetails.ProfitCenter = aircraftInfo.ProfitCenter;
            tsetACDetails.CoCode = "SEFO";
            tsetACDetails.NavComm1 = "n/a";
            tsetACDetails.NavComm2 = "n/a";
            tsetACDetails.AutoPilot = "n/a";
            tsetACDetails.Wx = "n/a";
            tsetACDetails.GPS = "n/a";
            tsetACDetails.TCAS = "n/a";
            tsetACDetails.EGPWS = "n/a";
            tsetACDetails.HF = "n/a";
            tsetACDetails.FM = "n/a";
            tsetACDetails.MFD = "n/a";
            tsetACDetails.Transponder = "n/a";
            tsetACDetails.Other1 = "n/a";
            tsetACDetails.Other2 = "n/a";
            tsetACDetails.Other3 = "n/a";
            tsetACDetails.Other4 = "n/a";
            tsetACDetails.Other5 = "n/a";
            return tsetACDetails;

        }

        private static List<AircraftInfo> _aircraftList = null;

        public static List<AircraftInfo> GetAircraftList(bool activeOnly)
        {

            if (_aircraftList!=null && activeOnly)
            {
                var tmpACList = _aircraftList.Where(x => x.OwnAircraft && x.Active).OrderBy(x=>x.Registration).ToList();
                tmpACList.AddRange(_aircraftList.Where(x => x.OwnAircraft == false && x.Active).OrderBy(x => x.Registration).ToList());
                return tmpACList;
            }
                

            return _aircraftList;
        }


        public static async Task DeactivateAircraft(int IDX, String Server, string regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tsetAircraft = await ctx.tset_ACDetails.FirstOrDefaultAsync(x => x.IDX == IDX);
                if (tsetAircraft != null)
                    tsetAircraft.Active = 0;
                await ctx.SaveChangesAsync();


            }
        }


        public static bool UpdateCachedObject (AircraftInfo aircraftInfo)
        {
            if (_aircraftList != null)
            {
                var oldObject = _aircraftList.FirstOrDefault(x => x.IDX == aircraftInfo.IDX);
                if (oldObject != null)
                    _aircraftList.Remove(oldObject);

                _aircraftList.Add(aircraftInfo);
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
                    tset_ACDetails acInfo;

                    if (!IsNew)
                    {
                        acInfo = await ctx.tset_ACDetails.FirstOrDefaultAsync(x => x.IDX == IDX);
                        acInfo.IDX_ACTypes = IDX_AC_Type;
                        acInfo.IDX_Owner = IDX_Owner;
                        acInfo.Registration = Registration;
                        acInfo.EmptyMass = EmptyMass;
                        acInfo.BuyingRate = BuyRate;
                        acInfo.SellingRate = SellRate;
                        acInfo.Reserve_Fuel = ReserveFuel;
                        acInfo.Serial_Number = SerialNumber;
                        acInfo.YOM = YearOfManufacture;
                        acInfo.DTT = DateOfTotalTime;
                        acInfo.Active = Convert.ToByte(Active);
                        acInfo.OwnAircraft = Convert.ToByte(OwnAircraft);
                        acInfo.TTA = TotalTimeAirframe;
                        acInfo.EmptyArm = EmptyArm;
                        acInfo.Colours = Colours;
                        acInfo.Equipment = Equipment;
                        acInfo.AvgSpeed = AverageSpeed;
                        acInfo.Demurrage = Demurrage;
                        acInfo.ShortCycleFee = ShortCycleFee;
                        acInfo.TotalFuelWT = TotalFuel;
                        acInfo.ProfitCenter = ProfitCenter;
                        acInfo.InvoiceCode = InvoiceCode;
                        acInfo.IDX_UnderWriter = IDX_UnderWriter;
                        acInfo.InsuranceExp = InsuranceExpiryDate;
                    }                    
                    else
                    {
                        acInfo = (tset_ACDetails)this;
                        ctx.tset_ACDetails.Add(acInfo);
                    }


          
                    await ctx.SaveChangesAsync();

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

            return true;
        }

        public static AircraftInfo GetAircraftInfo(String registration)
        {
            if (_aircraftList != null && _aircraftList.FirstOrDefault(x => x.Registration == registration) != null)
                return _aircraftList.FirstOrDefault(x => x.Registration == registration);

            return null;
        }

        public static AircraftInfo GetAircraftInfo(int IDX_AC)
        {
            if (_aircraftList != null && _aircraftList.FirstOrDefault(x => x.IDX == IDX_AC) != null)
                return _aircraftList.FirstOrDefault(x => x.IDX == IDX_AC);

            return null;
        }

        public static String GetAircraftRegistration(int IDX_AC)
        {
            var acInfo = GetAircraftInfo(IDX_AC);

            return acInfo != null ? acInfo.Registration : "";
        }

        public static async Task<AircraftInfo> FindAircraft(String registration, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var acDetails = await ctx.tset_ACDetails.FirstOrDefaultAsync(x => x.Registration == registration);
                if (acDetails!=null)
                {
                    var aircraftInfo = (AircraftInfo)acDetails;
                    return aircraftInfo;
                }
                return null;
            }

        }

        public static async Task<List<AircraftInfo>> LoadAircraftList( bool bForceReload)
        {

                if (_aircraftList == null || bForceReload)
                {

                var ctx = new SchedwinGlobalEntities();
                    using (ctx)
                    {
                        var tmpLst = await ctx.tbAircrafts.Where(x => x.Active == 1).ToListAsync();
                        _aircraftList = tmpLst.Select(x => (AircraftInfo)x).OrderBy(x => x.Registration).ToList();
                    }

                }
                return _aircraftList;

        }
   

    public static async Task<List<AircraftInfo>> LoadAircraftList(String Server, string regionalDBName, bool bForceReload)
        {
            try
            {
                if (_aircraftList == null || bForceReload)
                {
                    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                    var ctx = new SchedwinRegionalEntities(constring);
                    using (ctx)
                    {
                        var tmpLst = await ctx.tset_ACDetails.Where(x=>x.Active==1).ToListAsync();
                        _aircraftList = tmpLst.Select(x =>  (AircraftInfo)x).OrderBy(x => x.Registration).ToList();
                    }

                }
                return _aircraftList;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                throw new Exception("Error loading aircraft list:" + Environment.NewLine + exMessage);
            }
        }
    }
}
