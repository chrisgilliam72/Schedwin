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
    /// Interaction logic for WeightBalanceLegsVIew.xaml
    /// </summary>
    public partial class WeightBalanceLegsView : UserControl
    {
        public WeightBalanceLegsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WeightBalanceLegsViewModel;
        }

        private void AddPax_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WeightBalanceLegsViewModel;
            var buttonCntrl = sender as RadButton;
            var balanceItem= buttonCntrl.DataContext as WeightBalancePositionItem;

            viewModel.UpdateRowWeights(balanceItem);

        }

        private void BtnRefreshGraph_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WeightBalanceLegsViewModel;
            viewModel.RefreshGraph();
        }

   
    }
}
