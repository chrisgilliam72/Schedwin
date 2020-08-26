using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.DragDrop;

namespace Schedwin.Scheduling
{
    /// <summary>
    /// Interaction logic for SchedulingLegsListView.xaml
    /// </summary>
    public partial class SchedulingLegsListView : UserControl
    {
        public SchedulingLegsListView()
        {
            InitializeComponent();
        }

        private void RadTimePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            double totalTimeDiff = 0;
            try
            {
                if (e.AddedItems.Count > 0 && e.RemovedItems.Count > 0)
                {
                    var oldTime = e.RemovedItems[0] as DateTime?;
                    var newTime = e.AddedItems[0] as DateTime?;

                    var viewModel = DataContext as SchedulingLegsListViewModel;
                    var timeDiff = (newTime.Value - oldTime.Value).TotalMinutes;
                    totalTimeDiff = timeDiff;
                    viewModel.ShiftLegTimes(timeDiff);
                }
            }

            catch (Exception)
            {
                FailWindow.Display("Unable to shift schedule by " + totalTimeDiff + " mins");
            }
        }

        private void AddNewLeg_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            viewModel.AddLeg();
        }

        private void RemoveLeg_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            viewModel.RemoveLeg();
        }
        private void AirstripFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            if (e.AddedItems.Count > 0)
            {
                var selectedAirstrip = e.AddedItems[0] as AirstripInfo;
                viewModel.UpdateSelectedLegSource(selectedAirstrip);
            }
        }
        private void AirstripTo_DropDownClosed(object sender, EventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
 
        }

        private void AirstripTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            if (e.AddedItems.Count > 0)
            {
                var selectedAirstrip = e.AddedItems[0] as AirstripInfo;
                viewModel.UpdateSelectedLegDestination(selectedAirstrip);
            }

        }

        private void legsGridView_Drop(object sender, System.Windows.DragEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            var draggedGroup = DragDropPayloadManager.GetDataFromObject(e.Data, "DraggedData") as ScheduleGroup;
            var textBlock = e.OriginalSource as TextBlock;
            var borderCntrl = e.OriginalSource as Border;
            if (textBlock!=null)
            {
                var droppedLeg = textBlock.DataContext as ScheduleLeg;
                viewModel.AddGroupToLeg(droppedLeg, draggedGroup);
            }
            else if (borderCntrl!=null)
            {
                var cell = borderCntrl.TemplatedParent as GridViewCell;
                if (cell!=null)
                {
                    var droppedLeg = cell.DataContext as ScheduleLeg;
                    viewModel.AddGroupToLeg(droppedLeg, draggedGroup);
                }
            }
        }

        private void radContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            var menuItem = e.Source as RadContextMenu;
            viewModel.ShowGroupMenuIems(menuItem);
        }

        private void legsGridView_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            DateTime tmpOld=DateTime.Today;
            DateTime tmpNew=DateTime.Today;
            try
            {

                var colName = e.Cell.Column.UniqueName;
                var viewModel = DataContext as SchedulingLegsListViewModel;
                if (colName == "GameFT")
                {
                    var newGameFT = Convert.ToInt32(e.NewData);
                    viewModel.UpdateLegdistance(newGameFT);
                }
                if (colName == "ETD" && e.OldData != null)
                {
                    e.Handled = true;

                    var oldTime = e.OldData as DateTime?;
                    var newTime = e.NewData as DateTime?;
    


                    if (oldTime.HasValue && newTime.HasValue)
                    {
                        tmpOld = oldTime.Value;
                        tmpNew = newTime.Value;
                        var timeDiff = (newTime.Value - oldTime.Value).TotalMinutes;
                        viewModel.ShiftLegTimes(timeDiff);
                      
                    }


                }

                if (colName == "TurnAroundTime")
                {
                    e.Handled = true;
                    var oldmins = e.OldData as int?;
                    var newmins = e.NewData as int?;
                    if (oldmins.HasValue && newmins.HasValue)
                    {
                        var minDiff = newmins - oldmins;
                        viewModel.ShiftLegTimes(minDiff.Value);
                    }
                }

            }
            catch (Exception)
            {
                FailWindow.Display("Fail to shift times. Old=" + tmpOld.ToString() + " new=" + tmpNew.ToString());
            }
        }

        private void AddGroups_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            viewModel.AddSelectedGroups();
        }

        private void UpdateLegTimes_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingLegsListViewModel;
            viewModel.RecalculateLegExForTimes();
        }
    }
}
