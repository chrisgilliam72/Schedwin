using Schedwin.Common;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling

{
    public class PilotLegDutyGridEntry
    {
        public DateTime Date { get; set; }

        public double LegHours { get; set; }

        public double TotalHours { get; set; }
    }

   public class FlightDutyHistoryViewModel : ViewModelBase 
    {
        private DateTime _startDate;
        public String StartDate
        {
            get
            {
                return _startDate.ToShortDateString();
            }
        }

        private DateTime _endDate;
        public String EndDate
        {
            get
            {
                return _endDate.ToShortDateString();
            }
        }

        public String Pilot { get; set; }

        public RangeObservableCollection<PilotLegDutyGridEntry> Legs { get; set; }

        public FlightDutyHistoryViewModel()
        {
            Legs = new RangeObservableCollection<PilotLegDutyGridEntry>();
        }

        public void Refresh(String pilotName,_30DayPilotDutyRoster roster)
        {
            Pilot = pilotName;
            _endDate = roster.ScheduleDate;
            _startDate = _endDate.AddDays(-30);

            var grpedData = roster.LegDetails.GroupBy(x => x.LegDate).ToList();
            var gridEntries= grpedData.Select(x => new PilotLegDutyGridEntry { Date = x.Key, LegHours = Math.Round(x.Sum(y => y.LegDuration),2) }).
                                       OrderBy(z=>z.Date).ToList();

            foreach (var gridentry in gridEntries)
            {
                int gridIndex = gridEntries.IndexOf(gridentry);
                if (gridIndex > 0)
                    gridentry.TotalHours = gridentry.LegHours + gridEntries[gridIndex - 1].TotalHours;
                else
                    gridentry.TotalHours = gridentry.LegHours;
            }

            Legs.AddRange(gridEntries);
            NotifyPropertyChanged("Pilot");
            NotifyPropertyChanged("Legs");
            NotifyPropertyChanged("StartDate");
            NotifyPropertyChanged("Enddate");
        }
    }
}
