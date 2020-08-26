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
using Schedwin.Data.Classes;
using Telerik.Windows.Controls;

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for AircraftTypeCntrlViiew.xaml
    /// </summary>
    public partial class AircraftTypeCntrlView : ItemControlBase
    {
        public AircraftTypeCntrlView()
        {
            InitializeComponent();
        }

        private void AddNewLoadingArrangement_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AircraftTypeCntrlViewModel;
            viewModel.AddLoadingArrangements();

        }

        private void NewPaxRow_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AircraftTypeCntrlViewModel;
            viewModel.AddPaxRow();
        }

        private void NewFreightRow_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AircraftTypeCntrlViewModel;
            viewModel.AddFreightRow();
        }

        private void radContextMenu2_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AircraftTypeCntrlViewModel;
            var menu = sender as RadContextMenu;

            if (menu!=null)
               viewModel.SelectedArrangement = menu.DataContext as ACLoadingArrangement;
        
        }

        private void radContextMenu3_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AircraftTypeCntrlViewModel;
            var menu = sender as RadContextMenu;

            if (menu != null)
                viewModel.SelectedArrangement = menu.DataContext as ACLoadingArrangement;

        }

        private void DeletePaxRow_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AircraftTypeCntrlViewModel;
        }

 
    }
}
