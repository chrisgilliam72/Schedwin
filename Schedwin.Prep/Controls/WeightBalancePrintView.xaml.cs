using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for WeightBalancePrintView.xaml
    /// </summary>
    public partial class WeightBalancePrintView : PrintedControl
    {
        public WeightBalancePrintView()
        {
            InitializeComponent();
            var viewModel = DataContext as WeightBalancePrintViewModel;
            viewModel.Init(LegCntrl0.DataContext as WeightBalanceLegPrintViewModel, LegCntrl1.DataContext as WeightBalanceLegPrintViewModel,
                           LegCntrl2.DataContext as WeightBalanceLegPrintViewModel, LegCntrl3.DataContext as WeightBalanceLegPrintViewModel);
        }



        public override int TotalPages()
        {
            var viewModel = DataContext as WeightBalancePrintViewModel;
            return viewModel.TotalPages;
        }

        public override int CurrentPage()
        {
            var viewModel = DataContext as WeightBalancePrintViewModel;
            return viewModel.CurrentPage;
        }

        public override void Print()
        {
            var viewModel = DataContext as PrintedTicketsViewModel;
        }

        public override void NextPage()
        {
            var viewModel = DataContext as WeightBalancePrintViewModel;
            viewModel.NextPage();
        }

        public override void PrevPage()
        {
            var viewModel = DataContext as WeightBalancePrintViewModel;
            viewModel.PreviousPage();
        }

        public override void LastPage()
        {
            var viewModel = DataContext as WeightBalancePrintViewModel;
            viewModel.LastPage();
        }
        public override void FirstPage()
        {
            var viewModel = DataContext as WeightBalancePrintViewModel;
            viewModel.FirstPage();
        }

    }
}
