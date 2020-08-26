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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Schedwin.Prep
{
    /// <summary>
    /// Interaction logic for TicketsReservationView.xaml
    /// </summary>
    public partial class TicketsReservationView : UserControl
    {
        public TicketsReservationView()
        {
            InitializeComponent();
        }



        private async void PaxViewTickets_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var selitems = PaxGrd.SelectedItems.Select(x => (PassengerTicket)x).ToList();
            var viewModel = DataContext as TicketsReservationViewModel;
            await viewModel.PrintPaxTicket(selitems,false);
        }

        private async void PaxPrintTickets_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var selitems = PaxGrd.SelectedItems.Select(x => (PassengerTicket)x).ToList();
            var viewModel = DataContext as TicketsReservationViewModel;
            await viewModel.PrintPaxTicket(selitems,true);
        }
    }
}
