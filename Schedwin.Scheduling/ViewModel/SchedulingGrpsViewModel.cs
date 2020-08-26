using Schedwin.Common;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling
{
    public class SchedulingGrpsViewModel : ViewModelBase
    {

        public NewSchedulingViewModel SchedulingViewModel { get; set; }
        public RangeObservableCollection<ScheduleGroup> Groups { get; set; }

        public SchedulingGrpsViewModel()
        {
            Groups = new RangeObservableCollection<ScheduleGroup>();
        }

        public void Clear()
        {
            Groups.Clear();
            NotifyPropertyChanged("Groups");
        }

        public void Refresh()
        {
            SchedulingViewModel.RefreshGroups();
        }

        public void UpdateGroupList()
        {
            NotifyPropertyChanged("Groups");
        }

        public void ReEvaluateAllGroupsStatus(Schedule currentSchedule)
        {
            foreach (var grp in Groups)
            {
                var resLegs = currentSchedule.GetReservationLegs(grp.IDX_RL);
                grp.SetScheduledStatus(resLegs);
            }

            NotifyPropertyChanged("Groups");
        }

        public async Task  UpdateExFors(ScheduleGroup group)
        {
            try
            {
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await group.UpdateExFor(SchedulingViewModel.Server, SchedulingViewModel.Database);
                }
            }
            catch (Exception ex)
            {

                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to update group information:" + Environment.NewLine + exMessage);
            }
        }

        public void UnselectAllGroups()
        {
            foreach (var grp in Groups)
                grp.Selected = false;
        }

        public bool MarkGroupAsSelected(ScheduleGroup group, bool selected=true)
        {
            if (group.SoleUse && NoSelectedGroups() >0 && selected)
            {
                FailWindow.Display("You can not select a sole use group with other groups");
                return false;
            }

           
            group.Selected = selected;
            return true;
        }

        public int NoSelectedGroups()
        {
           return Groups.Where(x => x.Selected).Count();
        }
        public List<ScheduleGroup> GetSelectedGroups()
        {
            return Groups.Where(x => x.Selected).ToList();
        }

        public async Task Refresh(DateTime date, List<ScheduleGroup> groups)
        {
            var resIDS = groups.Select(x => x.IDX_RH).ToList();
            Groups.Clear();
            Groups.AddRange(groups);
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                var splitBkgsLst = await ScheduleGroup.GetSplitBookingsList(resIDS, SchedulingViewModel.Server, SchedulingViewModel.Database);
                foreach (var splitbooking in splitBkgsLst)
                {
                    var childGrp = groups.FirstOrDefault(x => x.IDX_RH == splitbooking.fkChildReservation);
                    if (childGrp != null)
                        childGrp.ChildIDX =splitbooking.fkChildReservation;
                    var parentGRP = groups.FirstOrDefault(x => x.IDX_RH == splitbooking.fkChildReservation);
                    if (parentGRP != null)
                        parentGRP.ParentIDX = splitbooking.fkParentReservation;
                }
            }
            NotifyPropertyChanged("Groups");

        }
    }
}
