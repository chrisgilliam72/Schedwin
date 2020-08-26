using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ModulePermission
    {

        public int IDX { get; set; }

        public int IDX_User { get; set; }
        public String ModuleName { get; set; }

        public bool CanModify { get; set; }

        public bool CanView { get; set; }

        public bool IsNew { get; set; }


        public static explicit operator ModulePermission(tbModulePermission tbModulePermission)
        {
            var permission = new ModulePermission();
            permission.IsNew = false;
            permission.IDX_User = tbModulePermission.tbUser.pkUserID;
            permission.ModuleName = tbModulePermission.Module_Name;
            permission.CanModify = tbModulePermission.Can_Modify;
            permission.CanView = tbModulePermission.Can_View;
            return permission;
        }

        public static explicit operator ModulePermission(tset_ModulePermissions tsetPermission)
        {
            var permission = new ModulePermission();
            permission.IsNew = false;
            permission.ModuleName = tsetPermission.Module_Name;
            permission.CanModify = tsetPermission.Can_Modify;
            permission.CanView = tsetPermission.Can_View;
            return permission;
        }

        public static explicit operator tset_ModulePermissions(ModulePermission modulePermission)
        {
            var tsetPermission = new tset_ModulePermissions();
            tsetPermission.Module_Name = modulePermission.ModuleName;
            tsetPermission.Can_Modify = modulePermission.CanModify;
            tsetPermission.Can_View = modulePermission.CanView;
            return tsetPermission;

        }




        public ModulePermission()
        {
            IsNew = true;
            CanModify = false;
            CanView = false;
        }

        public async static Task<List<ModulePermission>> GetModulePermissions(String username)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var dbPermissions = await ctx.tbModulePermissions.Include("tbUser").Where(x => x.tbUser.username == username).ToListAsync();
                var modPermissions = dbPermissions.Select(x => (ModulePermission)x).ToList();
                return modPermissions;

            }

        }


        public async  static Task<List<ModulePermission>> GetModulePermissions(int userIDX, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var dbPermissions= await  ctx.tset_ModulePermissions.Where(x => x.IDX_User == userIDX).ToListAsync();
                var modPermissions= dbPermissions.Select(x => (ModulePermission)x).ToList();                
                return modPermissions;

            }
          
        }
    }
}
