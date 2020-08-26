using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Wish
{
    public class WishAccess
    {
        public async Task<List<String>> GetWishConsultants()
        {
            var ctx = new WISHEntities();
            using (ctx)
            {
               var users= await ctx.tbUsers.ToListAsync();
                return users.Select(x => x.PersonCode).ToList();

            }
        }
    }
}
