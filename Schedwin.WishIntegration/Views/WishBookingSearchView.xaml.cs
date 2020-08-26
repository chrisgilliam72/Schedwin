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

namespace Schedwin.WishIntegration
{
    /// <summary>
    /// Interaction logic for WishBookingSearchView.xaml
    /// </summary>
    public partial class WishBookingSearchView : Window
    {
        public WishBookingHeaderCntrlView bookingHdrView { get; set; }
        public int CurrentSearchIteration { get; set;  }
        public WishBookingSearchView()
        {
            InitializeComponent();
            CurrentSearchIteration = 0;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WishBookingSearchViewModel;
            if (viewModel.LocalModel)
            {
                if (!bookingHdrView.ScrollToReservation(viewModel.BookingName, viewModel.BookingID, CurrentSearchIteration++))
                {
                    CurrentSearchIteration = 0;
                    FailWindow.Display("No more instances found.");
                }
            }
            else
            {
                DialogResult = true;
                Close();
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SearchByID_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WishBookingSearchViewModel;
            viewModel.SetSearchByID();
        }

        private void SearchByName_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WishBookingSearchViewModel;
            viewModel.SetSearchByName();
        }
    }
}
