using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data
{
    public class RegionalUsers
    {
        public String LastError { get; set; }
        public async Task<bool> CreateUser(String firstName, String surname, String adUsername, int companyID, String email, String Server, String regionalDatabase)
        {
            try
            {

                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
                var ctx = new SchedwinRegionalEntities(constring);
                var schedUser = await ctx.tset_Personnel.FirstOrDefaultAsync(x => x.Username == adUsername);
                if (schedUser==null)
                {
                    var newuser = new tset_Personnel();
                    newuser.IDX_Company = companyID;
                    newuser.Firstname = firstName;
                    newuser.Surname = surname;
                    newuser.Telephone = "1";
                    newuser.Fax = "1";
                    newuser.DateOfBirth = DateTime.Today;
                    newuser.EMail = email;
                    newuser.Username = adUsername;
                    newuser.WebPassword = "";
                    newuser.Email2 = email;
                    newuser.IDX_PersonnelType = 1;
                    ctx.tset_Personnel.Add(newuser);
                    await ctx.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex )
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return false;

            }
        }

       public  async Task<int> GetRegionalIDX(String adUsername,String Server,String regionalDatabase)
       {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
                var ctx = new SchedwinRegionalEntities(constring);

                using (ctx)
                {
                    var schedUser = await ctx.tset_Personnel.FirstOrDefaultAsync(x => x.Username == adUsername && x.Active.HasValue && x.Active.Value);
                    return schedUser != null ? schedUser.IDX : -1;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return -999;
            }

               
        }
    }
}
