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
        private String _status;
        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }
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

        public RangeObservableCollection<RosterDutyType> DutyTypes { get; set; }

        public PilotDutyPeriod CopyiedDutyPeriod { get; set; }

        public PilotDutyPeriod SelectedDutyPeriod { get; set; }



        public PilotRosterViewModel()
        {
            PilotList = new RangeObservableCollection<PilotDutyPeriod>();
            DayHeader = new ObservableCollection<PilotDutyHeader>();
            DutyTypes = new RangeObservableCollection<RosterDutyType>();
        }

        public async Task Init()
        {   try
            {
                List<RosterDutyType> dutyTypeList = null;
                DutyTypes.Clear();
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    if (UseGlobalDB)
                        dutyTypeList = await RosterDutyType.LoadRosterDutyTypes();
                    else
                        dutyTypeList = await RosterDutyType.LoadRosterDutyTypes(Server, Database);
                     DutyTypes.AddRange(dutyTypeList);
                }


            }
            catch   (Exception ex)
            {
                Status = "An initilization error has occurred.";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }
        }

        public async Task Refresh(DateTime newDate)
        {
            if (UseGlobalDB)
                await RefreshGlobal(newDate);
            else
                await RefreshRegional(newDate);
        }

        public async Task RefreshGlobal(DateTime newDate)
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
                    Status = "Loading ...";
                    var rosterCreated = await Pilots.RosterCreated(StartDate);
                    var results = await Pilots.GetPilotRosters(StartDate);
                    Status = "Updating ...";
                    results.ForEach(x => x.DutyStart = StartDate);

                    PilotList.AddRange(results);
                    results.ForEach(x => x.DutyTypes = DutyTypes);
                    if (!rosterCreated)
                    {
                        Status = "Creating default roster...";
                        await Pilots.SaveEntireRoster(StartDate, PilotList.ToList());

                    }
                    Status = "";
                }

            }
            catch (Exception ex)
            {
                Status = "An error has occurred.";
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

        public async Task RefreshRegional(DateTime newDate)
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
                    Status = "Loading ...";
                    var rosterCreated = await Pilots.RosterCreated(StartDate, Server, Database);
                    var results = await Pilots.GetPilotRosters(StartDate, Server, Database);
                    Status = "Updating ...";
                    results.ForEach(x => x.DutyStart = StartDate);
                 
                    PilotList.AddRange(results);
                    results.ForEach(x => x.DutyTypes = DutyTypes);
                    if (!rosterCreated)
                    {
                        Status = "Creating default roster...";
                        await Pilots.SaveEntireRoster(StartDate, PilotList.ToList(), Server, Database);

                    }
                    Status = "";
                }

            }
            catch (Exception ex)
            {
                Status = "An error has occurred.";
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



        public async void Paste()
        {
            try
            {
                if (SelectedDutyPeriod != null)
                {
                    SelectedDutyPeriod.CopyFrom(CopyiedDutyPeriod);
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        await Pilots.UpdatePilotRoster(StartDate, SelectedDutyPeriod, Server, Database);
                    }
                }
            }
            catch (Exception ex)
            {

                Status = "An error has occurred.";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }


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

        public async Task<bool> UpdateDuty(PilotDutyPeriod pilotDutyPeriod, int Day)
        {
            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    Status = "Saving ...";
                    var dutyDate = this.StartDate.AddDays(Day-1);
                    var dutyItem = pilotDutyPeriod.DailyDutyTypeList[Day-1];
                    var dutyTypeIDX = DutyTypes.First(x => x.Code == dutyItem.DutyType).IDX;
                    await Pilots.UpdateRosterDay(dutyDate, pilotDutyPeriod.IDX, dutyItem.DutyType, dutyTypeIDX, Server,Database);
                    Status = "";
                }

                Status = "";
                return true;
            }

            catch (Exception ex)
            {
                Status = "An error has occurred.";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }

        public async Task<bool> Save()
        {
            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    Status = "Erasing old schedule ...";
                    await Pilots.ClearRosterForMonth(StartDate, this.PilotList.ToList(), Server, Database);
                    Status = "Saving ...";
                    await Pilots.SaveEntireRoster(StartDate,this.PilotList.ToList(), Server, Database);
                }

                Status = "";
                return true;
            }

            catch (Exception ex)
            {
                Status = "An error has occurred.";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }
    }
}
