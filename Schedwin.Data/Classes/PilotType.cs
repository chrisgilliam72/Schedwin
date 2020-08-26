using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class PilotType
    {
        public int IDX { get; set; }

        public String Description { get; set; }

        private static List<PilotType> _listPilotTypes = null;


        public static List<PilotType> GetPilotTypes()
        {
            return _listPilotTypes;
        }

        public static async Task<bool> LoadPilotTypes(String Server, string regionalDBName)
        {
            try
            {
                if (_listPilotTypes == null)
                {
                    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                    var ctx = new SchedwinRegionalEntities(constring);
                    var tmpLst=  await ctx.tlst_PilotTypes.ToListAsync();

                    _listPilotTypes = tmpLst.Select(x => new PilotType { IDX = x.IDX, Description = x.PilotType }).OrderBy(x=>x.Description).ToList();
                }

                return false;
            }

            catch (Exception ex)
            {
                throw new Exception("Error looading pilot types list:" + ex.Message);
            }
        }
    }
}
