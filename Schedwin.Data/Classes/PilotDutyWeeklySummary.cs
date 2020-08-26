using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class PilotDutyWeeklySummary
    {
        public int PilotID { get; set; }

        public double Week1Hours { get; set; }

        public double Week2Hours { get; set; }

        public double Week3Hours { get; set; }

        public double Week4Hours { get; set; }

        public double Week5Hours { get; set; }


        //public static PilotDutyWeeklySummary GetWeeklySummary(int idxPilot, DateTime startDate,String Server, string regionalDBName)
        //{
        //    List<PilotDutyLegHours> listDutyHours = null;
        //    var summary = new PilotDutyWeeklySummary();
        //    var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
        //    var ctx = new SchedwinRegionalEntities(constring);

        //    int daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
        //    var endDate = startDate.AddMonths(1);
        //    using (ctx)
        //    {


        //        var pilotLegData = ctx.tsch_AC_Pilot.Include("tsch_legs").Include("tset_Personnel").
        //                                               Where(x => x.FlightDate >= startDate && x.FlightDate <= endDate && x.tset_Personnel.IDX == idxPilot)
        //                                               .OrderBy(x => x.FlightDate).ToList();

        //        listDutyHours = pilotLegData.Select(x => (PilotDutyLegHours)x).OrderBy(x => x.FlightDate).ToList();

        //        for (int week = 0; week < 5; week++)
        //        {
        //            DateTime weekStart = startDate.AddDays(week * 7);
        //            DateTime weekEnd = startDate.AddDays((week + 1) * 7);
        //            var weekDuties = listDutyHours.Where(x => x.FlightDate >= weekStart && x.FlightDate < weekEnd);
        //            switch (week)
        //            {
        //                case 0: summary.Week1Hours = weekDuties.Sum(x => x.DutyHours); break;
        //                case 1: summary.Week2Hours = weekDuties.Sum(x => x.DutyHours); break;
        //                case 2: summary.Week3Hours = weekDuties.Sum(x => x.DutyHours); break;
        //                case 3: summary.Week4Hours = weekDuties.Sum(x => x.DutyHours); break;
        //                case 4: summary.Week5Hours = weekDuties.Sum(x => x.DutyHours); break;
        //            }
        //        }
        //    }
        //    return summary;
        //}
    }
}
