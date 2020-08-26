using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{


    public class FilePermissionsAccess
    {
        public String Version { get; set; }

        public int LowestBuildNo { get; set; }

        public static explicit operator FilePermissionsAccess(tset_AllowedVersionBuilds versionBuilds)
        {
            var permissions = new FilePermissionsAccess();
            permissions.Version = versionBuilds.Version;
            permissions.LowestBuildNo = versionBuilds.LowestBuildNo;
            return permissions;
        }

        public List<FilePermissionsAccess> GetAllowedPermissions(String Server, String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var lstItems = ctx.tset_AllowedVersionBuilds.ToList();
                var lstPermissions = lstItems.Select(x => (FilePermissionsAccess)x).ToList();
                return lstPermissions;

            }
        }


    }

}
