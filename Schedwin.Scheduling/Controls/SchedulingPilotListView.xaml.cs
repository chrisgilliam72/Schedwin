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

namespace Schedwin.Scheduling
{
    /// <summary>
    /// Interaction logic for SchedulingPilotListView.xaml
    /// </summary>
    public partial class SchedulingPilotListView : UserControl
    {
        public SchedulingPilotListView()
        {
            InitializeComponent();
        }

        private void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            if (e.AddedItems.Count >0)
            {
                var acPilot = e.AddedItems[0] as ScheduleACPilot;
                viewModel.RefreshLegsGrid(acPilot);
                viewModel.UpdateTotalsGrid(acPilot);
            }

        }

        private void AddPilot_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            viewModel.AddPilot();
        }

        private void RemovePilot_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            viewModel.RemoveSelectedPilot();
        }

        private void ACRegistration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            if (e.AddedItems.Count >0)
            {
                var acInfo = e.AddedItems[0] as AircraftInfo;
                viewModel.UpdateACDetails(acInfo);
            }
        
        }

        private void Pilot1Info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            if (e.AddedItems.Count > 0)
            {
                var pilotInfo = e.AddedItems[0] as PilotInfo;
                viewModel.UpdatePilot1Details(pilotInfo);
            }
        }

        private void Pilot2Info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            if (e.AddedItems.Count > 0)
            {
                var pilotInfo = e.AddedItems[0] as PilotInfo;
                viewModel.UpdatePilot2Details(pilotInfo);
            }
        }

        private void RemovePilot2_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            viewModel.RemovePilot2();
        }

        private void ClearPilot_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as SchedulingPilotListViewModel;
            viewModel.ClearPilot();
        }
    }
}
