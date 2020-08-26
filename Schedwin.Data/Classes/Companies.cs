using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class Company
    {
        public bool IsActive { get; set; }
        public bool IsNew { get; set; }
        public int IDX { get; set; }
        public int? IDX_Country { get; set; }

        public int? IDX_CompanyType { get; set; }

        public String Description { get; set; }
        
        public String Registration { get; set; }

        public String PostalAddress { get; set; }

        public String PhysicalAddress { get; set; }

        public String Email { get; set; }

        public String TelNo { get; set; }

        public byte? VatPercentage { get; set; }

        public int? IDX_BaseAP { get; set; }

        public int? IDX_CurrencyType { get; set; }

        public String GPID { get; set; }

        static private List<Company> _CompaniescacheList = null;

        private static Company _currentCompany = null;


        public async static Task<Company> GetCurrentCompany(String Server, String regionalDBName)
        {
            if (_currentCompany == null)
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var tmpTsetCompany= await ctx.tset_Companies.FirstOrDefaultAsync(x => x.IDX == 99998);
                    _currentCompany = (Company)tmpTsetCompany;
                }
            }

            return _currentCompany;
        }

        public static bool UpdateCachedObject(Company newCompanyObj)
        {
            if (_CompaniescacheList != null)
            {
                var oldObject = _CompaniescacheList.FirstOrDefault(x => x.IDX == newCompanyObj.IDX);
                if (oldObject != null)
                {
                    var index = _CompaniescacheList.IndexOf(oldObject);
                    _CompaniescacheList.Remove(oldObject);
                    _CompaniescacheList.Insert(index, newCompanyObj);
                }
                  else 
                    _CompaniescacheList.Add(newCompanyObj);

                return true;

            }
            return false;

        }

        public static List<Company> GetCompanyList()
        {
            return _CompaniescacheList;
        }

        public static Company GetCompany (int Company_IDX)
        {
            if (_CompaniescacheList!=null)
            {
                return _CompaniescacheList.FirstOrDefault(x => x.IDX == Company_IDX);
            }

            return null;
        }

        public static explicit operator Company (tbOperator tbOperator)
        {
            var company = new Company();
            company.IDX = tbOperator.pkOperatorID;
            company.IDX_BaseAP = tbOperator.fkBaseAPID;
            company.IDX_Country = tbOperator.fkCountryID;
            company.Description = tbOperator.Name;
            company.PhysicalAddress = tbOperator.Physical_Address;
            company.TelNo = tbOperator.Phone_No;
            company.Email = tbOperator.Email;
            company.GPID = tbOperator.GP_ID;
            return company;
        }

        public static explicit operator Company(tset_Companies tset_Companies)
        {
            var company = new Company();
            company.IDX = tset_Companies.IDX;
            company.IDX_CompanyType = tset_Companies.IDX_CompanyType;
            company.IDX_BaseAP = tset_Companies.IDX_BaseAP;
            company.IDX_Country = tset_Companies.IDX_Countries;
            company.Registration = tset_Companies.CompanyRegistration;
            company.Description = tset_Companies.CompanyName;
            company.PhysicalAddress = tset_Companies.PhysicalAddress;
            company.PostalAddress = tset_Companies.PostalAddress;
            company.TelNo = tset_Companies.TelNo;
            company.Email = tset_Companies.EMail;
            company.VatPercentage = tset_Companies.VatPercentage;
            company.IDX_CurrencyType = tset_Companies.IDX_CurrencyType;
            company.GPID = tset_Companies.GP_ID;
            company.IsActive = tset_Companies.Active  ;
            return company;

        }

        public static explicit operator tset_Companies(Company company)
        {
            var tsetCompany = new tset_Companies();
            tsetCompany.IDX = company.IDX;
            tsetCompany.CoCode = "";
            tsetCompany.IDX_CompanyType = company.IDX_CompanyType.Value;
            tsetCompany.IDX_BaseAP = company.IDX_BaseAP;
            tsetCompany.IDX_Countries = company.IDX_Country;
            tsetCompany.CompanyRegistration = company.Registration;
            tsetCompany.CompanyName = company.Description;
            tsetCompany.PhysicalAddress = company.PhysicalAddress;
            tsetCompany.PostalAddress = company.PostalAddress;
            tsetCompany.VatPercentage = company.VatPercentage;
            tsetCompany.IDX_CurrencyType = company.IDX_CurrencyType;
            tsetCompany.GP_ID = company.GPID;
            tsetCompany.TelNo = company.TelNo;
            tsetCompany.EMail = company.Email;
            tsetCompany.Active = company.IsActive;

            return tsetCompany;

        }

        public async  Task<bool> Save(String Server, String regionalDBName)
        {

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
               if (IsNew)
                {
                    var tsetCompany = (tset_Companies)this;
                    ctx.tset_Companies.Add(tsetCompany);
                }
               else
                {
                    var oCompany = await ctx.tset_Companies.FirstOrDefaultAsync(x => x.IDX == IDX);
                    if (oCompany!=null)
                    {


                        oCompany.IDX_CompanyType = IDX_CompanyType.Value;
                        oCompany.IDX_BaseAP = IDX_BaseAP;
                        oCompany.IDX_Countries = IDX_Country;
                        oCompany.CompanyRegistration = Registration;
                        oCompany.CompanyName = Description;
                        oCompany.PhysicalAddress = PhysicalAddress;
                        oCompany.PostalAddress = PostalAddress;
                        oCompany.VatPercentage = VatPercentage;
                        oCompany.IDX_CurrencyType = IDX_CurrencyType;
                        oCompany.GP_ID = GPID;
                        oCompany.TelNo = TelNo;
                        oCompany.EMail = Email;
                        oCompany.Active = IsActive;
                    }
                }


               await  ctx.SaveChangesAsync();
            }

            return true;
        }

        public static async Task<bool> DeactivateCompany(int IDXCompany, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var company = await ctx.tset_Companies.FirstOrDefaultAsync(x => x.IDX == IDXCompany);
                if (company != null)
                {
                    company.Active = false;
                    await ctx.SaveChangesAsync();
                }

            }
            return true;
        }

        public static async Task<List<Company>> LoadCompanyList( bool forceReload)
        {


            if (_CompaniescacheList == null || forceReload)
            {

                var ctx = new SchedwinGlobalEntities();

                using (ctx)
                {
                    var tsetComps = await ctx.tbOperators.ToListAsync();
                    _CompaniescacheList = tsetComps.Select(x => (Company)x).OrderBy(x => x.Description).ToList();
                    return _CompaniescacheList;

                }
            }


            return _CompaniescacheList;
        }

        public static async Task<List<Company>> LoadCompanyList(String Server, String  regionalDBName, bool forceReload)
        {
            
         
            if (_CompaniescacheList==null  || forceReload)
             {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var tsetComps = await ctx.tset_Companies.Where(x=>x.Active).ToListAsync();
                    _CompaniescacheList = tsetComps.Select(x => (Company)x).OrderBy(x => x.Description).ToList();
                    return _CompaniescacheList;

                }
            }


            return _CompaniescacheList;
        }
    }
}
