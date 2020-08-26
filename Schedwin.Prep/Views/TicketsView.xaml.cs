using Schedwin.Common;
using Schedwin.Data.Classes;
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
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Schedwin.Prep
{
    /// <summary>
    /// Interaction logic for TicketsView.xaml
    /// </summary>
    public partial class TicketsView : SchedwinBaseWindow
    {


        public TicketsView()
        {

            InitializeComponent();

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ticketsViewModel = ticketsResView.DataContext as TicketsReservationViewModel;
            var legsViewModel = ticketsLegsView.DataContext as TicketsLegsViewModel;

            var viewModel = DataContext as TicketsViewModel;
            viewModel.View = this;
            viewModel.TicketReservationViewModel = ticketsViewModel;
            viewModel.TicketLegsViewModel = legsViewModel;
            await viewModel.Init();
        }

        private async void RadDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TicketsViewModel;
            await viewModel.Refresh();
        }


        private void rdBtnLegsView_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TicketsViewModel;
            viewModel.ShowLegList();
        }

        private void rdBtnResView_Click_1(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TicketsViewModel;
            viewModel.ShowReservationList();
        }
    }
}
