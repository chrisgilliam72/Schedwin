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

namespace Schedwin.Prep
{
    /// <summary>
    /// Interaction logic for TicketsLegsView.xaml
    /// </summary>
    public partial class TicketsLegsView : UserControl
    {
        public TicketsLegsView()
        {
            InitializeComponent();
        }

        private void LegsGrd_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }

        private async void ViewTickets_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var selitems = LegsGrd.SelectedItems.Select(x => (TicketLegGridItem)x).ToList();
            var viewModel = DataContext as TicketsLegsViewModel;
            await viewModel.PrintTickets(selitems,false);
        }

        private async void PrintTickets_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var selitems = LegsGrd.SelectedItems.Select(x => (TicketLegGridItem)x).ToList();
            var viewModel = DataContext as TicketsLegsViewModel;
            await viewModel.PrintTickets(selitems, true);
        }
    }
}
