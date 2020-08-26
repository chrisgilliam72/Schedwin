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
using System.Windows.Shapes;

namespace Schedwin.Prep
{
    /// <summary>
    /// Interaction logic for PrintedTicketsView.xaml
    /// </summary>
    public partial class PrintedTicketsView : PrintedControl 
    {
        public PrintedTicketsView()
        {
            InitializeComponent();
            var viewModel = DataContext as PrintedTicketsViewModel;
            viewModel.Init(ticketCntr1.DataContext as PrintedTicketControlViewModel, ticketCntr12.DataContext as PrintedTicketControlViewModel, ticketCntr13.DataContext as PrintedTicketControlViewModel);
        }

        public override int TotalPages()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
            return viewModel.TotalPages;
        }

        public override int CurrentPage()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
            return viewModel.CurrentPage;
        }

        public override void Print()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
        }
  
        public override void NextPage()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
            viewModel.NextPage();
        }

        public override void PrevPage()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
            viewModel.PreviousPage();
        }

        public override void LastPage()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
            viewModel.LastPage();
        }
        public override void FirstPage()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
            viewModel.FirstPage();
        }

   
    }
}
