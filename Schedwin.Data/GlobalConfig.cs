using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data
{
    class CheckedRoleItem
    {
        public String RoleName { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SchedwinUserRole
    {
        public String Username { get; set; }
        public String RoleName { get; set;}

        public String Region { get; set; }

        public bool Active { get; set; }

        public String RealName { get; set; }
    }

   
    public class SchedwinScreenPermissions
    {
        public String RoleName { get; set; }

        public String ScreenName { get; set; }

        public bool CanView { get; set; }

        public bool CanAdd { get; set; }

        public bool CanEdit { get; set; }
    }

   

    public class GlobalConfig
    {


        public async Task<bool> ActivateUser(string username,bool makeActive)
        {
            try
            {
                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    var user = await ctx.tbUsers.FirstOrDefaultAsync(x => x.username == username);
                    if (user != null)
                    {
                        user.Active = makeActive;
                        await ctx.SaveChangesAsync();
                    }

                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }    
        }

        public async Task<bool> RemoveRole(string username, string roleName, string region)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var role = await ctx.tbUserRoles.Include("tbuser").Include("tbrole").
                                        FirstOrDefaultAsync(x => x.tbUser.username == username && x.tbRole.RoleName == roleName &&
                                                            x.tbDBRegionInfo.Region == region);
                if (role!=null)
                {
                    ctx.tbUserRoles.Remove(role);
                    await ctx.SaveChangesAsync();
                }

             }

            return true;

        }

        public async Task<bool> ResetRoles(string username, List<string> newRoles, string forRegion)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var roles = await ctx.tbRoles.Where(x => newRoles.Contains(x.RoleName)).ToListAsync();
                var user = await ctx.tbUsers.Include("tbuserRoles").
                                              Include("tbuserRoles.tbRole").
                                             Include("tbuserRoles.tbDBRegionInfo").Where(x => x.username == username).FirstOrDefaultAsync();
                var region = await ctx.tbDBRegionInfoes.FirstOrDefaultAsync(x => x.Region == forRegion);

                var regionRoles = user.tbUserRoles.Where(x => x.tbDBRegionInfo.Region == forRegion).ToList();
                while (regionRoles.Count>0)
                {
                    var role = user.tbUserRoles.FirstOrDefault();
                    if (role!=null)
                    {
                        ctx.tbUserRoles.Remove(role);
                        user.tbUserRoles.Remove(role);
                    }
                    regionRoles = user.tbUserRoles.Where(x => x.tbDBRegionInfo.Region == forRegion).ToList();
                }

                foreach (var role in roles)
                {
                    var userRole = new tbUserRole();
                    userRole.tbUser = user;
                    userRole.tbRole = role;
                    userRole.tbDBRegionInfo = region;

                    ctx.tbUserRoles.Add(userRole);
                }
                
                
                await ctx.SaveChangesAsync();
                return true;
            }
        }

       public  async Task<bool> CheckRoleExists(string username, string roleName, string region)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var role = await  ctx.tbUserRoles.Include("tbuser").Include("tbrole").
                                        FirstOrDefaultAsync(x => x.tbUser.username == username &&  x.tbRole.RoleName == roleName &&
                                                            x.tbDBRegionInfo.Region == region);
                return (role == null) ? false : true;
            }
        }

        public async Task<bool> AddUserRole(string username, string roleName, string userRegion)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var role = await ctx.tbRoles.FirstOrDefaultAsync(x => x.RoleName == roleName);
                var region = await ctx.tbDBRegionInfoes.FirstOrDefaultAsync(x => x.Region == userRegion);
                var user = await ctx.tbUsers.FirstOrDefaultAsync(x => x.username == username);

                if (role != null && region != null && user!=null)
                {
                    var tbuserrole = new tbUserRole();
                    tbuserrole.tbUser=user;
                    tbuserrole.tbDBRegionInfo = region;
                    tbuserrole.tbRole = role;

                    ctx.tbUserRoles.Add(tbuserrole);

                    await ctx.SaveChangesAsync();
                }

            }
            return false;
        }
        public async Task<bool> AddNewUser(string username, string roleName, string userRegion)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var role = await ctx.tbRoles.FirstOrDefaultAsync(x => x.RoleName == roleName);
                var region = await ctx.tbDBRegionInfoes.FirstOrDefaultAsync(x => x.Region == userRegion);

                if (role!=null && region!=null)
                {
                    var tbuser = new tbUser();
                    tbuser.username = username;
                    tbuser.Active = true;

                    var tbuserRole = new tbUserRole();
                    tbuserRole.tbUser = tbuser;
                    tbuserRole.tbDBRegionInfo = region;
                    tbuserRole.tbRole = role;

                    ctx.tbUsers.Add(tbuser);
                    ctx.tbUserRoles.Add(tbuserRole);
                    await ctx.SaveChangesAsync();

                    return true;
                }

            }

            return false;
        }

        public async Task<bool> CheckUserExists(string username)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var user = await ctx.tbUsers.FirstOrDefaultAsync(x => x.username == username);

                return (user == null) ? false : true;
            }
        }

        
   
        public async  Task<List<String>> GetUserRoles(String username,string region)
        {
            try
            {
                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    var roles = await ctx.tbUserRoles.Where(x => x.tbUser.username == username && x.tbDBRegionInfo.Region == region)
                                                      .Select(x => x.tbRole.RoleName).ToListAsync();

                    return roles;
                }

            }

            catch (Exception)
            {

                return null;
            }
        }


        public List<String> GetAllRoles()
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                return ctx.tbRoles.Select(x => x.RoleName).ToList();

            }
        }

        public async Task SaveSettings(String userName, Guid guidAD, String lastRegion)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var userSettings = await ctx.tbUserSettings.FirstOrDefaultAsync(x => x.Username == userName);
                if (userSettings==null)
                {
                    userSettings = new tbUserSetting();
                    userSettings.Username = userName;
                    userSettings.ADGuid = guidAD;
                    ctx.tbUserSettings.Add(userSettings);
                }

                userSettings.LastRegion = lastRegion;

               await ctx.SaveChangesAsync();
            }
        }

        public async Task<String> GetLastRegion(String userName)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var userSettings = await ctx.tbUserSettings.FirstOrDefaultAsync(x => x.Username == userName);
                return userSettings!=null ? userSettings.LastRegion : ""; 
            }
        }

        public List<tbDBRegionInfo> GetRegionalInfo()
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                return ctx.tbDBRegionInfoes.ToList();
            }
        }



        public List<SchedwinUserRole> GetAllUserRoles()
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var users = ctx.tbUserRoles.Include("tbuser").Include("tbRole").Include("tbDBRegionInfo").ToList();
                var userRoles = users.Select(x => new SchedwinUserRole { Username = x.tbUser.username, Region = x.tbDBRegionInfo.Region, RoleName = x.tbRole.RoleName, Active = x.tbUser.Active}).ToList();

                return userRoles;
                
            }
        }

  

        public async Task<List<string>> GetRegions()
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var regions= await  ctx.tbDBRegionInfoes.Where(x => x.Display.HasValue && x.Display.Value).Select(x => x.Region).OrderBy(x=>x).ToListAsync();
                return regions;

            }
        }

        public async Task<tbDBRegionInfo> GetRegionInfo(string regionName)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var regionInfo= await ctx.tbDBRegionInfoes.FirstOrDefaultAsync(x => x.Region == regionName);
                return regionInfo;
            }
        }
        
        public async Task<List<tbRole>> GetRoles(string userName, String regionName)
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
                var result = await ctx.tbUserRoles.Include("tbRole").Include("tbUser").
                             Where(x => x.tbUser.username == userName && x.tbDBRegionInfo.Region == regionName).Select(x=>x.tbRole).ToListAsync();

                return result;
            }
        }
  
    }
}
