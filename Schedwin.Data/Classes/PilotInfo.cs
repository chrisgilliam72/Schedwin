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
    public class PilotInfo
    {

        public bool IsNew { get; set; }
        public int IDX { get; set; }

        public bool Active { get; set; }

        public int IDX_Personnel { get; set; }

        public String Name { get; set; }

        public String Nationality { get; set; }

        public String PassportNumber { get; set; }

        public DateTime PassportExpiryDate { get; set; }

        public String PilotsAddress { get; set; }

        public String PilotsTelephone { get; set; }

        public String PilotsLicenseNumber { get; set; }

        public DateTime PilotsLicenseExpiryDate { get; set; }

        public bool CanRoster { get; set; }

        public bool InstrumentRating { get; set; }

        public bool DangerousGoods { get; set; }

        public bool CRM { get; set; }

        public bool SMS { get; set; }

        public bool OPC { get; set; }

        public bool SEPT { get; set; }
        public DateTime? SEPTExpiryDate { get; set; }

        public DateTime? SMSExpiryDate { get; set; }

        public DateTime? OPCExpiryDate { get; set; }

        public DateTime? InstrumentRatingExpiryDate { get; set; }

        public bool InstructorRating { get; set; }

        public DateTime? InstructorRatingExpiryDate { get; set; }

        public String ResidencePermitNumber { get; set; }

        public DateTime ResidencePermitExpiryDate { get; set; }

        public String WorkPermitNumber { get; set; }

        public DateTime WorkPermitExpiryDate { get; set; }

        public DateTime? DangerousGoodsExpiryDate { get; set; }

        public DateTime? CRMExpiryDate { get; set; }

        public DateTime? MedicalExpiryDate { get; set; }

        public String ContactName { get; set; }

        public String ContactCompany { get; set; }

        public String ContactAddress { get; set;  }

        public String ContactTelephone { get; set; }

        public DateTime StartingDate { get; set; }

        public double StartingFlyingHours { get; set; }
        public DateTime? COfT206 { get; set; }
        public DateTime? COfT208 { get; set; }
        public DateTime? CofT210 { get; set; }
        public DateTime? COfTPC12 { get; set; }

        public DateTime? COfT402 { get; set; }

        public DateTime? COfT510 { get; set; }
        public DateTime? COfT310 { get; set; }
         public DateTime? CofTGA8 { get; set; }
        public DateTime? CofTC172 { get; set; }
        public DateTime? CofTC210 { get; set; }
 
        public DateTime CofT { get; set; }
        public DateTime RouteCheck { get; set; }

        public short Weight { get; set; }

        public Byte[] LicenceImage { get; set; } 

        private static List<PilotInfo> _list = null;

        public static List<PilotInfo> GetPilotList()
        {
            return _list;
        }

        public static void UpdateChachedObject(PilotInfo newPilotInfoObj)
        {
            if (_list!=null)
            {
                var oldObject = _list.FirstOrDefault(x => x.IDX == newPilotInfoObj.IDX);
                if (oldObject != null)
                    _list.Remove(oldObject);

                _list.Add(newPilotInfoObj);
            }
        }

        public static explicit operator PilotInfo(tset_PilotsDetails pilotdetails)
        {
            var pilotInfo = new PilotInfo();
            pilotInfo.IDX = pilotdetails.IDX;
            pilotInfo.Active = pilotdetails.Active;
            pilotInfo.IDX_Personnel = pilotdetails.IDX_Personnel;
            if (pilotdetails.tset_Personnel!=null)
                pilotInfo.Name = pilotdetails.tset_Personnel.Firstname + " " + pilotdetails.tset_Personnel.Surname;
            pilotInfo.Nationality = pilotdetails.Nationality;
            pilotInfo.PassportNumber = pilotdetails.PassportNumber;
            pilotInfo.PassportExpiryDate = pilotdetails.PassportExpiryDate;
            pilotInfo.PilotsAddress = pilotdetails.PilotsAddress;
            pilotInfo.PilotsTelephone = pilotdetails.PilotsTelephone;
            pilotInfo.PilotsLicenseNumber = pilotdetails.PilotsLicenseNumber;
            pilotInfo.PilotsLicenseExpiryDate = pilotdetails.PilotsLicenseExpiryDate;
            pilotInfo.CanRoster = pilotdetails.CanRoster;
            pilotInfo.InstrumentRating = pilotdetails.InstrumentRating == "Yes" ? true : false;
            pilotInfo.InstrumentRatingExpiryDate = pilotdetails.InstrumentRatingExpiryDate;
            pilotInfo.InstructorRating = pilotdetails.InstructorRating== "Yes" ? true : false;
            pilotdetails.DangerousGoods = pilotdetails.DangerousGoods ?? false;
            pilotInfo.InstructorRatingExpiryDate = pilotdetails.InstructorRatingExpiryDate;
            pilotInfo.ResidencePermitNumber = pilotdetails.ResidencePermitNumber;
            pilotInfo.ResidencePermitExpiryDate = pilotdetails.ResidencePermitExpiryDate;
            pilotInfo.WorkPermitNumber = pilotdetails.WorkPermitNumber;
            pilotInfo.WorkPermitExpiryDate = pilotdetails.WorkPermitExpiryDate;
            pilotInfo.ContactName = pilotdetails.ContactName;
            pilotInfo.ContactCompany = pilotdetails.ContactCompany;
            pilotInfo.ContactAddress = pilotdetails.ContactAddress;
            pilotInfo.ContactTelephone = pilotdetails.ContactTelephone;
            pilotInfo.StartingDate = pilotdetails.StartingDate;
            pilotInfo.SEPT = pilotdetails.SEPT ?? false;
            pilotInfo.SEPTExpiryDate = pilotdetails.SEPTDate;
            pilotInfo.SMS = pilotdetails.SMS ?? false;
            pilotInfo.SMSExpiryDate = pilotdetails.SMSDate;
            pilotInfo.OPC = pilotdetails.OPC ?? false;
            pilotInfo.OPCExpiryDate = pilotdetails.OPCDate;
            pilotInfo.DangerousGoods = pilotdetails.DangerousGoods ?? false;
            pilotInfo.DangerousGoodsExpiryDate = pilotdetails.DangerousGoodsDate;
            pilotInfo.StartingFlyingHours = pilotdetails.StartingFlyingHours;
            pilotInfo.CRMExpiryDate = pilotdetails.CRMDate;
            pilotInfo.MedicalExpiryDate = pilotdetails.Medical;
            pilotInfo.CofT = pilotdetails.CofT;
            pilotInfo.COfT206 = pilotdetails.CofT206;
            pilotInfo.COfT208 = pilotdetails.CofT208;
            pilotInfo.CofTC210 = pilotdetails.CofTC210;
            pilotInfo.COfTPC12 = pilotdetails.CofTPC12;
            pilotInfo.COfT402 = pilotdetails.CofTC402;
            pilotInfo.COfT510 = pilotdetails.CofTC510;
            pilotInfo.COfT310 = pilotdetails.CofTC310;
            pilotInfo.CofTGA8 = pilotdetails.CofTGA8;
            pilotInfo.CofTC210 = pilotdetails.CofTC210;
            pilotInfo.LicenceImage = pilotdetails.PilotsLicenceImage;
            pilotInfo.RouteCheck = pilotdetails.RouteCheck;
            
            pilotInfo.Weight = pilotdetails.PilotWeight;

            return pilotInfo;

        }

        public static explicit operator PilotInfo(tbPilot pilotdetails)
        {
            var pilotInfo = new PilotInfo();
            pilotInfo.IDX = pilotdetails.pkPilotID;
            pilotInfo.Active = pilotdetails.Active;
            pilotInfo.IDX_Personnel = pilotdetails.fkUserID ?? 0;
            if (pilotdetails.tbUser != null)
                pilotInfo.Name = pilotdetails.tbUser.Firstname + " " + pilotdetails.tbUser.Surname;
            pilotInfo.Nationality = pilotdetails.Nationality;
            pilotInfo.PassportNumber = pilotdetails.PassportNumber;
            pilotInfo.PassportExpiryDate = pilotdetails.PassportExpiryDate;
            pilotInfo.PilotsAddress = pilotdetails.PilotsAddress;
            pilotInfo.PilotsTelephone = pilotdetails.PilotsTelephone;
            pilotInfo.PilotsLicenseNumber = pilotdetails.PilotsLicenseNumber;
            pilotInfo.PilotsLicenseExpiryDate = pilotdetails.PilotsLicenseExpiryDate;
            pilotInfo.CanRoster = pilotdetails.CanRoster;
            pilotInfo.InstrumentRating = pilotdetails.InstrumentRating == "Yes" ? true : false;
            pilotInfo.InstrumentRatingExpiryDate = pilotdetails.InstrumentRatingExpiryDate;
            pilotInfo.InstructorRating = pilotdetails.InstructorRating == "Yes" ? true : false;
            pilotdetails.DangerousGoods = pilotdetails.DangerousGoods ?? false;
            pilotInfo.InstructorRatingExpiryDate = pilotdetails.InstructorRatingExpiryDate;
            pilotInfo.ResidencePermitNumber = pilotdetails.ResidencePermitNumber;
            pilotInfo.ResidencePermitExpiryDate = pilotdetails.ResidencePermitExpiryDate;
            pilotInfo.WorkPermitNumber = pilotdetails.WorkPermitNumber;
            pilotInfo.WorkPermitExpiryDate = pilotdetails.WorkPermitExpiryDate;
            pilotInfo.ContactName = pilotdetails.ContactName;
            pilotInfo.ContactCompany = pilotdetails.ContactCompany;
            pilotInfo.ContactAddress = pilotdetails.ContactAddress;
            pilotInfo.ContactTelephone = pilotdetails.ContactTelephone;
            pilotInfo.StartingDate = pilotdetails.StartingDate;
            pilotInfo.SEPT = pilotdetails.SEPT ?? false;
            pilotInfo.SEPTExpiryDate = pilotdetails.SEPTDate;
            pilotInfo.SMS = pilotdetails.SMS ?? false;
            pilotInfo.SMSExpiryDate = pilotdetails.SMSDate;
            pilotInfo.OPC = pilotdetails.OPC ?? false;
            pilotInfo.OPCExpiryDate = pilotdetails.OPCDate;
            pilotInfo.DangerousGoods = pilotdetails.DangerousGoods ?? false;
            pilotInfo.DangerousGoodsExpiryDate = pilotdetails.DangerousGoodsDate;
            pilotInfo.StartingFlyingHours = pilotdetails.StartingFlyingHours;
            pilotInfo.CRMExpiryDate = pilotdetails.CRMDate;
            pilotInfo.MedicalExpiryDate = pilotdetails.Medical;
            pilotInfo.CofT = pilotdetails.CofT;
            pilotInfo.COfT206 = pilotdetails.CofT206;
            pilotInfo.COfT208 = pilotdetails.CofT208;
            pilotInfo.CofTC210 = pilotdetails.CofTC210;
            pilotInfo.COfTPC12 = pilotdetails.CofTPC12;
            pilotInfo.COfT402 = pilotdetails.CofTC402;
            pilotInfo.COfT510 = pilotdetails.CofTC510;
            pilotInfo.COfT310 = pilotdetails.CofTC310;
            pilotInfo.CofTGA8 = pilotdetails.CofTGA8;
            pilotInfo.CofTC210 = pilotdetails.CofTC210;
            pilotInfo.LicenceImage = pilotdetails.PilotsLicenceImage;
            pilotInfo.RouteCheck = pilotdetails.RouteCheck;

            pilotInfo.Weight = pilotdetails.PilotWeight;

            return pilotInfo;

        }


        public static explicit operator tset_PilotsDetails (PilotInfo pilotInfo)
        {
            var pilotdetails = new tset_PilotsDetails();
            pilotdetails.IDX = pilotInfo.IDX; 
            pilotdetails.Active = pilotInfo.Active;
            pilotdetails.IDX_Personnel = pilotInfo.IDX_Personnel;
            pilotdetails.Nationality = pilotInfo.Nationality;
            pilotdetails.PassportNumber = pilotInfo.PassportNumber;
            pilotdetails.PassportExpiryDate = pilotInfo.PassportExpiryDate;
            pilotdetails.PilotsAddress = pilotInfo.PilotsAddress;
            pilotdetails.PilotsTelephone = pilotInfo.PilotsTelephone;
            pilotdetails.PilotsLicenseNumber = pilotInfo.PilotsLicenseNumber;
            pilotdetails.PilotsLicenseExpiryDate = pilotInfo.PilotsLicenseExpiryDate;
            pilotdetails.PilotsLicenceImage = pilotInfo.LicenceImage;
            pilotdetails.CanRoster = pilotInfo.CanRoster;
            pilotdetails.SMS = pilotInfo.SMS;
            pilotdetails.SMSDate = pilotInfo.SMSExpiryDate;
            pilotdetails.SEPT = pilotInfo.SEPT;
            pilotdetails.SEPTDate = pilotInfo.SEPTExpiryDate;
            pilotdetails.OPC = pilotInfo.OPC;
            pilotdetails.OPCDate = pilotInfo.OPCExpiryDate;
            pilotdetails.InstrumentRating = pilotInfo.InstrumentRating == true ? "Yes" : "No";
            if (pilotInfo.InstrumentRatingExpiryDate.HasValue)
                pilotdetails.InstrumentRatingExpiryDate = pilotInfo.InstrumentRatingExpiryDate.Value;
            pilotdetails.InstructorRating= pilotInfo.InstructorRating==true ? "Yes" : "No";
            if (pilotInfo.InstructorRatingExpiryDate.HasValue)
                pilotdetails.InstructorRatingExpiryDate = pilotInfo.InstructorRatingExpiryDate.Value;

            pilotdetails.ResidencePermitNumber = pilotInfo.ResidencePermitNumber;
            pilotdetails.ResidencePermitExpiryDate = pilotInfo.ResidencePermitExpiryDate;
            pilotdetails.DangerousGoods = pilotInfo.DangerousGoods;
            pilotdetails.DangerousGoodsDate = pilotInfo.DangerousGoodsExpiryDate;
            pilotdetails.WorkPermitNumber = pilotInfo.WorkPermitNumber;
            pilotdetails.WorkPermitExpiryDate = pilotInfo.WorkPermitExpiryDate;
            pilotdetails.ContactName = pilotInfo.ContactName;
            pilotdetails.ContactCompany = pilotInfo.ContactCompany;
            pilotdetails.ContactAddress = pilotInfo.ContactAddress;
            pilotdetails.ContactTelephone = pilotInfo.ContactTelephone;
            pilotdetails.StartingDate = pilotInfo.StartingDate;
            pilotdetails.StartingFlyingHours = pilotInfo.StartingFlyingHours;
            pilotdetails.CofT = pilotInfo.CofT;
            pilotdetails.CofT206 = pilotInfo.COfT206;
            pilotdetails.CofT208 = pilotInfo.COfT208;
            pilotdetails.CofTC210 = pilotInfo.CofTC210;
            pilotdetails.CofTPC12 = pilotInfo.COfTPC12;
            pilotdetails.CofTC402 = pilotInfo.COfT402;
            pilotdetails.CofTC510 = pilotInfo.COfT510;
            pilotdetails.CofTC310 = pilotInfo.COfT310;
            pilotdetails.CofTGA8 = pilotInfo.CofTGA8;
            pilotdetails.RouteCheck = pilotInfo.RouteCheck;
            pilotdetails.PilotWeight = pilotInfo.Weight;
            pilotdetails.DangerousGoodsDate = pilotInfo.DangerousGoodsExpiryDate;
            pilotdetails.DangerousGoods = pilotInfo.DangerousGoods;
            pilotdetails.CRMDate = pilotInfo.CRMExpiryDate;
            pilotdetails.Medical = pilotInfo.MedicalExpiryDate.Value;
            pilotdetails.PilotsLicenceImage = pilotInfo.LicenceImage;
            pilotdetails.ContactFax = "";
            pilotdetails.PilotsFax = "";
            pilotdetails.CanRoster = pilotdetails.Active;
            return pilotdetails;
        }

        public async Task <bool> Save(String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    tset_PilotsDetails tsetPilot = null ;
                    if (IsNew)
                    {
                        tsetPilot = (tset_PilotsDetails)this;
                        ctx.tset_PilotsDetails.Add(tsetPilot);
                    }
                    else
                    {
                        tsetPilot = ctx.tset_PilotsDetails.FirstOrDefault(x => x.IDX == IDX);
                        tsetPilot.Active = Active;
                        tsetPilot.IDX_Personnel = IDX_Personnel;
                        tsetPilot.Nationality = Nationality;
                        tsetPilot.PassportNumber = PassportNumber;
                        tsetPilot.PassportExpiryDate = PassportExpiryDate;
                        tsetPilot.PilotsAddress = PilotsAddress;
                        tsetPilot.PilotsTelephone = PilotsTelephone;
                        tsetPilot.PilotsLicenseNumber = PilotsLicenseNumber;
                        tsetPilot.PilotsLicenseExpiryDate = PilotsLicenseExpiryDate;
                        tsetPilot.CanRoster = CanRoster;
                        tsetPilot.SMS = SMS;
                        tsetPilot.SMSDate = SMSExpiryDate;
                        tsetPilot.SEPT = SEPT;
                        tsetPilot.SEPTDate= SEPTExpiryDate;
                        tsetPilot.OPC = OPC;
                        tsetPilot.OPCDate = OPCExpiryDate;
                        tsetPilot.DangerousGoods = DangerousGoods;
                        tsetPilot.DangerousGoodsDate = DangerousGoodsExpiryDate;
                        tsetPilot.InstrumentRating = InstrumentRating == true ? "Yes" : "No";

                        tsetPilot.InstrumentRatingExpiryDate = InstrumentRatingExpiryDate;
                        tsetPilot.InstructorRating = InstructorRating == true ? "Yes" : "No";
                        tsetPilot.InstructorRatingExpiryDate = InstructorRatingExpiryDate;
                        tsetPilot.ResidencePermitNumber = ResidencePermitNumber;
                        tsetPilot.ResidencePermitExpiryDate = ResidencePermitExpiryDate;
                        tsetPilot.WorkPermitNumber = WorkPermitNumber;
                        tsetPilot.WorkPermitExpiryDate = WorkPermitExpiryDate;
                        tsetPilot.ContactName = ContactName;
                        tsetPilot.ContactCompany = ContactCompany;
                        tsetPilot.ContactAddress = ContactAddress;
                        tsetPilot.ContactTelephone = ContactTelephone;
                        tsetPilot.StartingDate = StartingDate;
                        tsetPilot.StartingFlyingHours = StartingFlyingHours;
                        tsetPilot.CofT = CofT;
                        tsetPilot.CofT206 = COfT206;
                        tsetPilot.CofT208 = COfT208;
                        tsetPilot.CofTC210 = CofTC210;
                        tsetPilot.CofTPC12 = COfTPC12;
                        tsetPilot.CofTC402 = COfT402;
                        tsetPilot.CofTC510 = COfT510;
                        tsetPilot.CofTC310 = COfT310;
                        tsetPilot.CofTGA8 = CofTGA8;
                        tsetPilot.RouteCheck = RouteCheck;
                        tsetPilot.PilotWeight = Weight;
                        tsetPilot.DangerousGoodsDate = DangerousGoodsExpiryDate;
                        tsetPilot.DangerousGoods = DangerousGoods;
                        tsetPilot.CRMDate = CRMExpiryDate;
                        tsetPilot.Medical = MedicalExpiryDate.Value;
                        tsetPilot.PilotsLicenceImage = LicenceImage;
                        tsetPilot.CanRoster = tsetPilot.Active;

                    }
                    await ctx.SaveChangesAsync();
                    IDX = tsetPilot.IDX;
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

        public static PilotInfo GetPilot(int idxPilot)
        {
            if (_list != null && _list.FirstOrDefault(x => x.IDX == idxPilot) != null)
                return _list.FirstOrDefault(x => x.IDX == idxPilot);
            return null;
        }

        public static PilotInfo GetPilotFromPersonnelID(int idxPersonnel)
        {
            if (_list != null && _list.FirstOrDefault(x => x.IDX_Personnel == idxPersonnel) != null)
                return _list.FirstOrDefault(x => x.IDX_Personnel == idxPersonnel);
            return null;
        }

        public static String GetPilotName(int idxPilot)
        {
            if (_list != null && _list.FirstOrDefault(x => x.IDX == idxPilot) != null)
            {
                return _list.FirstOrDefault(x => x.IDX == idxPilot).Name;
            }

            return "";
        }

        public static String GetPilotNameFromPersonnelID(int idxPersonnel)
        {
            if (_list != null && _list.FirstOrDefault(x => x.IDX_Personnel == idxPersonnel) != null)
            {
                return _list.FirstOrDefault(x => x.IDX_Personnel == idxPersonnel).Name;
            }

            return "";
        }

        public static int GetPilotWeight(int idxPilot)
        {
            if (_list != null && _list.FirstOrDefault(x => x.IDX == idxPilot) != null)
            {
                return _list.FirstOrDefault(x => x.IDX == idxPilot).Weight;
            }

            return 0;
        }

        public static int GetPilotweightFromPersonnelID(int idxPersonnel)
        {
            if (_list != null && _list.FirstOrDefault(x => x.IDX_Personnel == idxPersonnel) != null)
            {
                return _list.FirstOrDefault(x => x.IDX_Personnel == idxPersonnel).Weight;
            }

            return 0;
        }

        public static async Task<bool> DeactivatePilot(int IDXPilot ,String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var pilot = await ctx.tset_PilotsDetails.FirstOrDefaultAsync(x => x.IDX == IDXPilot);
                if (pilot != null)
                {
                    pilot.Active = false;
                    pilot.CanRoster = false;
                    await ctx.SaveChangesAsync();
                }

            }
            return true;
        }

        public static async Task<bool> LoadPilotInfo()
        {

            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {

                var tmpLst = await ctx.tbPilots.Include("tbUser").Where(x => x.Active).ToListAsync();
                _list = tmpLst.Select(x => (PilotInfo)x).OrderBy(x => x.Name).ToList();
            }
            return true;
        }
        public static async Task<bool> LoadPilotInfo(String Server, string regionalDBName)
        {
            try
            {

                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                                    
                    var tmpLst= await ctx.tset_PilotsDetails.Include("tset_Personnel").Where(x => x.Active).ToListAsync();
                    _list = tmpLst.Select(x => (PilotInfo)x).OrderBy(x=>x.Name).ToList();
                }
                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);

                throw new Exception("Error loading pilot info list:" + Environment.NewLine + exMessage);
            }
        }

    }
}
