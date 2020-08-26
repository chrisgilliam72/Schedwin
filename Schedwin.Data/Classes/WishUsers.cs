using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class WishUsers
    {
        public static List<User> GetWishUsers()
        {
            List<User> userList = null;

            using (var context = new PrincipalContext(ContextType.Domain, "wilderness.co.za"))
            {
                using (var group = GroupPrincipal.FindByIdentity(context, "#APP.rw-WS-WISH"))
                {
                    if (group != null)
                    {
                        var tmplst = group.GetMembers(true).OrderBy(x => x.DisplayName);
                        var tmplst2 = tmplst.Select(x => (UserPrincipal)x).ToList();
                        userList = tmplst2.Select(x => (User)x).OrderBy(x => x.FullName).ToList();
                    }

                }
            }

            return userList;
        }
    }
}
