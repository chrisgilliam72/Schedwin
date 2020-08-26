using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling.Classes
{
    public class RosterLegInfo
    {
        public int IDX_TschLeg { get; set; }
        public DateTime LegDate { get; set; }
        public double LegDuration
        {
            get
            {
                return (EndTime - StartTime).TotalHours;
            }
        }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }

}
