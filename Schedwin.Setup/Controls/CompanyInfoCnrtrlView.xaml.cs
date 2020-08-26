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
    /// Interaction logic for CompanyInfoCnrtrlView.xaml
    /// </summary>
    public partial class CompanyInfoCnrtrlView : ItemControlBase
    {
        public CompanyInfoCnrtrlView()
        {
            InitializeComponent();
        }

        private void btnTPLookup_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            viewModel.TPLookUp();
        }



        private void AddNewPriceList_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            viewModel.AddNewPriceList();
        }

        private void DeletePriceList_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            viewModel.DeletePriceListEntry();
        }

        private async void RadGridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            var entryItem = e.Row.Item as PriceList;
            await viewModel.SavePriceListEntry(entryItem);

        }
        private async void AgentsGridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            var entryItem = e.Row.Item as User;

            await viewModel.SaveAgentEntry(entryItem);
        }

        private void AddNewAgent_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            viewModel.AddNewAgent();
        }

        private void DeleteAgent_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as CompanyInfoCnrtrlViewModel;
            viewModel.RemoveAgent();
        }


    }
}
