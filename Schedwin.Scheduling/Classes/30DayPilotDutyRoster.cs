using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling.Classes
{
    public class _30DayPilotDutyRoster
    {

        public int IDX_Pilot { get; set; }

        public DateTime ScheduleDate { get; set; }

        public double FlightHours30Days
        {
            get
            {
                var totalFlighttime = LegDetails.Sum(x => x.LegDuration);
                return totalFlighttime;
            }
        }

        public double FlightHours7Days
        {
            get
            {
                var startDate = ScheduleDate.AddDays(-8);
                var last7DayLegs = LegDetails.Where(x => x.LegDate > startDate).ToList();
                var flighttime = last7DayLegs.Sum(x => x.LegDuration);

                return flighttime;
            }
        }


     
        public List<RosterLegInfo> LegDetails { get; set; }

        public List<RosterDutyType> DutyTypes { get; set; }

        public _30DayPilotDutyRoster()
        {
            LegDetails = new List<RosterLegInfo>();
            DutyTypes = new List<RosterDutyType>();

        }

        public void AddLeg(DateTime legDate, DateTime etd, DateTime eta)
        {
            var legInfo = new RosterLegInfo();
            legInfo.IDX_TschLeg = -1;
            legInfo.LegDate = legDate;
            legInfo.StartTime = etd;
            legInfo.EndTime = eta;
            LegDetails.Add(legInfo);
        }
    }

  

    
}
