using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Tourplan
{
    public static class LocationCache
    {
        private static List<LOC> _locationList=null;

        public static String GetLOCDescription(String locCode)
        {
            if (_locationList==null)
            {
                var etx = new TourplanIS_PAFEntities();
                using (etx)
                {
                    var locs = etx.LOCs.ToList();
                    _locationList.AddRange(locs);
                }
            }

            var loc = _locationList.FirstOrDefault(x => x.CODE == locCode);
            return loc != null ? loc.NAME : "";
        }
    }
}
