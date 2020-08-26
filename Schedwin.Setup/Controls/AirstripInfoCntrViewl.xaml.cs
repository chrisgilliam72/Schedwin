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

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for AirstripInfo.xaml
    /// </summary>
    public partial class AirstripInfoCntrlView : ItemControlBase
    {
        public AirstripInfoCntrlView()
        {
            InitializeComponent();
        }
        private void btnTPLookup_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.TPLookUp();
        }

        private void AddNewExForItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        private void RemoveExForItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

        }

        

        private void btnMap_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.ShowOnMap();
        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
           
        }

        private void RemoveLimitations_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.RemoveLimit();

        }

        private void AddLimitations_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.AddLimits();
        }

        private void AddNewFee_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.AddFee();
        }

        private void RemoveFee_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.RemoveFee();
        }

        private void AddFuel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.AddFuel();
        }

        private void RemoveFuel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripInfoCntrlViewModel;
            viewModel.RemoveFuel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
