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

namespace Schedwin.WishIntegration
{
    /// <summary>
    /// Interaction logic for RemovedWishBookingHeaderCntrlView.xaml
    /// </summary>
    public partial class RemovedWishBookingHeaderCntrlView : UserControl
    {
        public RemovedWishBookingHeaderCntrlView()
        {
            InitializeComponent();
        }

        private void RadMenuItemDelete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as RemovedWishBookingHeaderCntrlViewModel;
            viewModel.DeleteBooking();
        }

        private void RadMenuItemCancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as RemovedWishBookingHeaderCntrlViewModel;
            viewModel.CancelBooking();
        }
    }
}
