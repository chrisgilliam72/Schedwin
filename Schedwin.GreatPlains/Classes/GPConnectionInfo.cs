using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.GreatPlains.Classes
{
    public class GPConnectionInfo
    {
        public String GPServer { get; set; }

        public String GPDatabase { get; set; }

        public static async Task< GPConnectionInfo> GetGPConnectionInfo(String serverName, String regionalDBName)
        {
            var gpConInfo = new GPConnectionInfo();

            var constring = Data.RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dbConInfo= await ctx.tGP_ConnectInformation.FirstOrDefaultAsync();
                gpConInfo.GPServer = dbConInfo.GP_Server;
                gpConInfo.GPDatabase = dbConInfo.GP_Schedwin_Database;

            }

            return gpConInfo;
        }

    }


}
