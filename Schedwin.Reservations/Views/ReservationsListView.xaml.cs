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
using Telerik.Windows.Controls;

namespace Schedwin.Reservations
{
    /// <summary>
    /// Interaction logic for ReservationsListView.xaml
    /// </summary>
    public partial class ReservationsListView : SchedwinBaseWindow
    {
        public ReservationsListView()
        {
            InitializeComponent();
        }

        private async void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.View = this;
            await viewModel.Init();
        }

        private async void Button_RefreshClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            await viewModel.Refresh();
        }

        private void ReservationLst_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.EditReservation();
        }

        private void Button_ClickEditReservation(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.EditReservation();
        }

        private void Button_ClickNewReservation(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.NewReservation();
        }

        private void Button_ClickBookingItinerary(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.ViewBookingItinerary();
        }

        private void Button_ClickBookingItineraryAllDate(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.ViewItineraryForDate();
        }

        private async void SplitBooking_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            await viewModel.SplitReservation();
        }

        private void Button_ClickNextDay(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.AdvanceDate();
        }

        private void Button_ClickPrevDay(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.AdvanceDate(false);
        }

        private void SchedwinBaseWindow_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationsListViewModel;
            viewModel.ToggleCancelled();
        }
    }
}
