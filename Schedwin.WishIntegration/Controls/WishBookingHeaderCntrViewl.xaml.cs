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
using Telerik.Windows.Controls.GridView;

namespace Schedwin.WishIntegration
{
    /// <summary>
    /// Interaction logic for WishBookingHeaderCntrl.xaml
    /// </summary>
    public partial class WishBookingHeaderCntrlView : UserControl
    {
        private int LastScrollIndex = 0;
        public Window MainWindow { get; set; }
        public WishBookingHeaderCntrlView()
        {
            InitializeComponent();
        }


        public bool ScrollToReservation(String bookingName, int bookingID, int iteration)
        {
            
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            var item = viewModel.ScrollReservationIntoView(bookingName, bookingID,iteration);
            if (item != null)
            {
                LastScrollIndex=gridvwBookings.Items.IndexOf(item);
                gridvwBookings.ScrollIntoView(item);
                gridvwBookings.Focus();
                return true;
            }
            else
            {
              
                return false;
            }
 
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            btnRefresh.IsEnabled = false;
            btnSave.IsEnabled = false;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                await viewModel.Init();
            }
            btnRefresh.IsEnabled = true;
            btnSave.IsEnabled = true;
        }


        private async void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                //await viewModel.SelectBooking();
                await viewModel.SelectBookingV2();
            }
        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            btnSave.IsEnabled = false;
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                await viewModel.Refresh();
                //await viewModel.Refresh();
            }
            btnRefresh.IsEnabled = true;
            btnSave.IsEnabled = true;
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = false;
            btnRefresh.IsEnabled = false;
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            if (viewModel.ValidateSelection())
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    var refreshLst = await viewModel.SaveSelection();
                   
                    btnRefresh.IsEnabled = false;
                    await viewModel.Refresh();
                    btnRefresh.IsEnabled = true;
                   
                    // await viewModel.SaveSelection();
                }

            }
            btnSave.IsEnabled = true;
            btnRefresh.IsEnabled = true;
        }


        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchDlg = new WishBookingSearchView();
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            var searchViewModel = searchDlg.DataContext as WishBookingSearchViewModel;
            searchDlg.Owner = MainWindow;
            searchDlg.bookingHdrView = this;
            searchDlg.Show();

        }


        private  async void ButtonSearchDB_Click(object sender, RoutedEventArgs e)
        {
            var searchDlg = new WishBookingSearchView();
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            var searchViewModel = searchDlg.DataContext as WishBookingSearchViewModel;
            searchDlg.Owner = MainWindow;
            searchDlg.bookingHdrView = this;
            searchViewModel.LocalModel = false;
            var dlgResult = searchDlg.ShowDialog();

            if (dlgResult.HasValue && dlgResult.Value)
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await viewModel.LoadSingleBooking(searchViewModel.BookingID);
                }
            }

        }

        private async void RemoveResItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            var result = false;
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                result= await viewModel.RemoveHeader();
            }

            if (result)
                SuccessWindow.Display("Booking removed.");
        }

        private async void RefreshPaxItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;

            using (new StackedCursorOverride(Cursors.Wait))
            {
                await viewModel.RefreshSeletedBooking();
                await viewModel.SelectBookingV2();
            }            
        }

        private async void CancelItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var result = false;
            var viewModel = DataContext as WishBookingHeaderCntrlViewModel;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                result = await viewModel.CancelHeader();
            }

            if (result)
                SuccessWindow.Display("Booking cancelled.");
        }

 
    }
}
