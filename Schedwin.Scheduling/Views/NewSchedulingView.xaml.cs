using Schedwin.Common;
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
    /// Interaction logic for NewSchedulingView.xaml
    /// </summary>
    public partial class NewSchedulingView : SchedwinBaseWindow
    {
        public NewSchedulingView()
        {
            InitializeComponent();
        }

        private void RadDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var viewModel = DataContext as NewSchedulingViewModel;
            viewModel.PilotsVM = this.pilotCntrlView.DataContext as SchedulingPilotListViewModel;
            viewModel.PilotsVM.LegsViewModel = this.legsCntrlView.DataContext as SchedulingLegsListViewModel;
            viewModel.PilotsVM.TotalsViewModel = this.totalsCntrlView.DataContext as ScheduleTotalViewModel;
            viewModel.GroupsVM = this.grpsCntrlView.DataContext as SchedulingGrpsViewModel;
            viewModel.LegsVM = this.legsCntrlView.DataContext as SchedulingLegsListViewModel;
            viewModel.TotalsVM = this.totalsCntrlView.DataContext as ScheduleTotalViewModel;

            await viewModel.Init();
        }

        private async void btn_ClickRefresh(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewSchedulingViewModel;
            await viewModel.LoadSchedule();
        }

        private async void Button_ClickSave(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewSchedulingViewModel;
            var saveResult= await viewModel.Save();
            if (saveResult)
                await viewModel.LoadSchedule();
        }

        private async void Schedule_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var viewModel = DataContext as NewSchedulingViewModel;
            await viewModel.Unlock();
        }


        private async void Button_ClickUnlock(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewSchedulingViewModel;
            await viewModel.Unlock();
        }
    }
}
