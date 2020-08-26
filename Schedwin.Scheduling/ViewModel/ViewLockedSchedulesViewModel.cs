using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Scheduling.Classes;

namespace Schedwin.Scheduling
{

   public class ViewLockedSchedulesViewModel : ViewModelBase
    {
        public String CurrentUser { get; set; }
        public RangeObservableCollection<LockedScheduleItem> LockedSchedules { get; set; }

        public bool UnlockEnabled
        {
            get
            {
                var unlockCount =LockedSchedules.Where(x => x.Unlock).ToList().Count();
                return unlockCount > 0 ? true : false;

            }
        }
        public ViewLockedSchedulesViewModel()
        {
            LockedSchedules = new RangeObservableCollection<LockedScheduleItem>();
        }

        public void UpdateUnlockButtonStatus()
        {
            NotifyPropertyChanged("UnlockEnabled");
        }

        public async void Unlock()
        {
            try
            {
                var varTmpList = LockedSchedules.Where(x => x.Unlock == true).Select(x=>x.ScheduleDate).ToList();
                var dateLst = varTmpList.Select(x => Convert.ToDateTime(x)).ToList();
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await Schedule.Unlock(CurrentUser, dateLst, Server, Database);
                }

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to unlock schedules" + Environment.NewLine + exMessage);
            }
        }
        public async void Refresh()
        {
            try
            {
                LockedSchedules.Clear();
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var tmpLst = await Schedule.GetLockedSchedules(Server, Database);
                    var currUserLst = tmpLst.Where(x => x.LockedByUser == CurrentUser).ToList();
                    LockedSchedules.AddRange(currUserLst);
                }

                NotifyPropertyChanged("LockedSchedules");
                   
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to load lock list:" + Environment.NewLine + exMessage);
            }
        }
    }
}
