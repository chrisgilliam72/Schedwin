using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System.Collections.ObjectModel;

namespace Schedwin.Scheduling
{
    public class PilotDutyHeader
    {
        public List<String> Days { get; set; }
    }

    public class PilotRosterViewModel : ViewModelBase
    {

    
        public bool CanCopy
        {
            get
            {
                return SelectedDutyPeriod != null;
            }

        }

        public bool Show29Th
        {
            get
            {
                return GetDaysInMonth() > 28;
            }
        }

        public bool Show30Th
        {
            get
            {
                return GetDaysInMonth() > 29;
            }
        }

        public bool Show31St
        {
            get
            {
                return GetDaysInMonth() > 30;
            }
        }

        public bool CanPaste
        {
            get
            {
                return SelectedDutyPeriod != null && CopyiedDutyPeriod!=null;
            }
        }

        public String Header { get; set; }
        public DateTime StartDate { get; set; }
        public RangeObservableCollection<PilotDutyPeriod> PilotList { get; set; }

        public ObservableCollection<PilotDutyHeader> DayHeader { get;set;}

        public ObservableCollection<String> DutyTypes { get; set; }

        public String Server { get; set; }

        public String Database { get; set; }


        public PilotDutyPeriod CopyiedDutyPeriod { get; set; }

        public PilotDutyPeriod SelectedDutyPeriod { get; set; }

        public PilotRosterViewModel()
        {
            PilotList = new RangeObservableCollection<PilotDutyPeriod>();
            DayHeader = new ObservableCollection<PilotDutyHeader>();
            DutyTypes = new ObservableCollection<string> { "1", "0", "T", "L" };
        }

        public async void Refresh(DateTime newDate)
        {
            try

            {
                StartDate = newDate;
                DateTime EndDate = StartDate.AddMonths(1);
                DateTime dateCur = StartDate;
                int DaysInMonth = DateTime.DaysInMonth(StartDate.Year, StartDate.Month);

                Header = "Pilot Roster for :" + StartDate.ToString("MMMM") + " " + StartDate.Year;
                NotifyPropertyChanged("Header");
                PilotList.Clear();
                DayHeader.Clear();

                var dutyHeader = new PilotDutyHeader
                {
                    Days = new List<string>()
                };

                DayHeader.Add(dutyHeader);

                do
                {
                    String dayAbrv = dateCur.DayOfWeek.ToString().Substring(0, 3);
                    dutyHeader.Days.Add(dayAbrv);
                    dateCur = dateCur.AddDays(1);
                } while (dateCur < EndDate);

                while (dutyHeader.Days.Count < 31)
                    dutyHeader.Days.Add("");

               

                using (new StackedCursorOverride(Cursors.Wait))
                {
                    var results = await Pilots.GetPilotRosters(StartDate, Server, Database);

                    results.ForEach(x => x.dutyStart = StartDate);
                    PilotList.AddRange(results);
                }


            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

            NotifyPropertyChanged("Show29Th");
            NotifyPropertyChanged("Show30th");
            NotifyPropertyChanged("Show31St");
            NotifyPropertyChanged("DayList");
            NotifyPropertyChanged("PilotList");
        }

        public async void SavePrevious(PilotDutyPeriod dutyPeriod)
        {
            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await Pilots.UpdatePilotRoster(dutyPeriod, Server, Database);
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }
        }


        public void Paste()
        {
            if (SelectedDutyPeriod!=null)
                SelectedDutyPeriod.CopyFrom(CopyiedDutyPeriod);
        }

        public void Copy()
        {
            CopyiedDutyPeriod = SelectedDutyPeriod;
            NotifyPropertyChanged("CanPaste");
        }

        public int GetDaysInMonth()
        {
            var month = StartDate.Month;
            var year = StartDate.Year;

           return DateTime.DaysInMonth(year, month);
        }

        public async Task<bool> Save()
        {
            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await Pilots.ClearRosterForMonth(StartDate, this.PilotList.ToList(), Server, Database);
                    await Pilots.SaveEntireRoster(StartDate,this.PilotList.ToList(), Server, Database);
                }

                SuccessWindow.Display("Roster saved");
                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }
    }
}
