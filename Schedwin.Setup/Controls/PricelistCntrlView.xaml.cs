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

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for PricelistCntrlView.xaml
    /// </summary>
    public partial class PricelistCntrlView : ItemControlBase
    {
        public PricelistCntrlView()
        {
            InitializeComponent();
        }

        private void AddNewPriceList_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as PricelistCntrlViewModel;
            viewModel.AddNewPriceList();
        }

        private void DeletePriceList_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as PricelistCntrlViewModel;
            viewModel.DeleteEntry();
        }

        private async void RadGridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            var viewModel = DataContext as PricelistCntrlViewModel;
            var entryItem = e.Row.Item as PriceList;
            await viewModel.SaveEntry(entryItem);

        }
    }
}
