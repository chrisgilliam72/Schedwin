using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling.Classes
{
    public class ScheduleLegPax
    {
        public String Name { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }


        public static explicit operator ScheduleLegPax(tsch_Passengers tschPassenger)
        {
            var schedulePax = new ScheduleLegPax();
            schedulePax.Name = tschPassenger.FirstName + " " + tschPassenger.Surname;
            schedulePax.Age = tschPassenger.Age ?? 0;
            schedulePax.Weight = tschPassenger.Weight ?? 0;
            schedulePax.IsMale = tschPassenger.Sex == "M" ? true : false;
            return schedulePax;

        }
    }
}
