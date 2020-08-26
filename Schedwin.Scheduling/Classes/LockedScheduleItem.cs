using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling.Classes
{
    public class LockedScheduleItem
    {
        public String LockedByUser { get; set; }
        public String ScheduleDate { get; set; }
        public bool Unlock { get; set; }
    }
}
