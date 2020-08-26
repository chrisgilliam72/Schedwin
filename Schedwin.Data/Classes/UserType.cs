using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class UserType
    {
        public int IDX { get; set; }

        public String Description { get; set; }

        private static List<UserType> _UserTypeCache { get; set; }


        public static List<UserType> GetUserTypes()
        {
            return _UserTypeCache;
        }

        public static async Task<List<UserType>> LoadUserTypes(String Server, string regionalDBName)
        {
            if (_UserTypeCache != null)
                return _UserTypeCache;

            else
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                _UserTypeCache = new List<UserType>();
                using (ctx)
                {
                    var tmpList= await ctx.tlst_PersonnelType.ToListAsync();

                    var typeList = tmpList.Select(x => new UserType { IDX = x.IDX, Description = x.PersonnelType }).ToList();
                    _UserTypeCache.AddRange(typeList);
                    return typeList;
                }
            }
        }
    }
}
