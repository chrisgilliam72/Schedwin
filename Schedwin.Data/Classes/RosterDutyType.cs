using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.Data.Classes
{
   public class RosterDutyType
    {
        public int IDX { get; set; }
        public String Code { get; set; }

        public String Description { get; set; }

        public String ColorName { get; set; }

        public Brush Color { get; set; }


        public static explicit operator RosterDutyType(tset_RosterDutyTypes tsetRosterType)
        {
            var rosterDutyType = new RosterDutyType();
            rosterDutyType.IDX = tsetRosterType.IDX;
            rosterDutyType.Code = tsetRosterType.Code;
            rosterDutyType.Description = tsetRosterType.Description;
            rosterDutyType.ColorName = tsetRosterType.Color;
            return rosterDutyType;
        }


        public static explicit operator RosterDutyType(tbRosterDutyType tbRosterDutyType)
        {
            var rosterDutyType = new RosterDutyType();
            rosterDutyType.IDX = tbRosterDutyType.pkDutyRosterTypeID;
            rosterDutyType.Code = tbRosterDutyType.Code;
            rosterDutyType.Description = tbRosterDutyType.Description;
            rosterDutyType.ColorName = tbRosterDutyType.Color;
            return rosterDutyType;
        }

        public static async Task<List<RosterDutyType>> LoadRosterDutyTypes()
        {
            var ctx = new SchedwinGlobalEntities();
            using (ctx)
            {
               var dbList= await ctx.tbRosterDutyTypes.ToListAsync();
                var rosterDutyList = dbList.Select(x => (RosterDutyType)x).ToList();
                return rosterDutyList;

            }
        }

        public static async Task<List<RosterDutyType>> LoadRosterDutyTypes(String server , String regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);

            using (ctx)
            {
                var dbList = await ctx.tset_RosterDutyTypes.ToListAsync();
                var rosterDutyList = dbList.Select(x => (RosterDutyType)x).ToList();
                return rosterDutyList;
            }

        }
    }
}
