using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data
{
   public class RegionalConnectionGenerator
    {
        public static String GetConnectionString(String Server, String datbaseName)
        {
            SqlConnectionStringBuilder sqlString = new SqlConnectionStringBuilder()
            {
                IntegratedSecurity = true,
                DataSource = Server,
                InitialCatalog = datbaseName,
                ConnectTimeout=60,
                MultipleActiveResultSets = true
            };


            EntityConnectionStringBuilder entityString = new EntityConnectionStringBuilder()
            {
                Provider = "System.Data.SqlClient",
                Metadata = "res://*/SchedwinRegional.csdl|res://*/SchedwinRegional.ssdl|res://*/SchedwinRegional.msl",
                ProviderConnectionString = sqlString.ToString()
            };

            return entityString.ConnectionString;
        }


    }
   partial class  SchedwinRegionalEntities
    {
        public SchedwinRegionalEntities(String connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            var constring = new EntityConnectionStringBuilder();
        }


    }
}
