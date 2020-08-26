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
using Telerik.Windows.Controls.TreeView;

namespace Schedwin.Scheduling
{
    /// <summary>
    /// Interaction logic for SchedulingGrpsView.xaml
    /// </summary>
    public partial class SchedulingGrpsView : UserControl
    {
        public SchedulingGrpsView()
        {
            InitializeComponent();

            ScheduleGroupGridsDragDropBehavior.SetIsEnabled(gridViewGrps, true);
        }

       

        private void RefreshItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingGrpsViewModel;
            viewModel.Refresh();
        }

        private async void gridViewGrps_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            var viewModel = DataContext as SchedulingGrpsViewModel;

            if ( e.Cell.DataColumn.Header.ToString()=="Earliest EX" || e.Cell.DataColumn.Header.ToString() == "Latest For")
            {

                var selectedGrp = e.Cell.DataContext as ScheduleGroup;
                if (selectedGrp!=null)
                {
                    await viewModel.UpdateExFors(selectedGrp);
                }

              
            }
           
        }

        private void gridViewGrps_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as SchedulingGrpsViewModel;
            foreach (var item in e.AddedItems)
            {
                var grpItem = item as ScheduleGroup;
                if (! viewModel.MarkGroupAsSelected(grpItem))
                {
                    gridViewGrps.SelectedItems.Clear();
                    viewModel.UnselectAllGroups();
                    return;
                }

            }

            foreach (var item in e.RemovedItems)
            {
                var grpItem = item as ScheduleGroup;
                viewModel.MarkGroupAsSelected(grpItem,false);
            }
        }

        private void UnselectAll_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingGrpsViewModel;
            gridViewGrps.SelectedItems.Clear();
            viewModel.UnselectAllGroups();
        }
    }
}
