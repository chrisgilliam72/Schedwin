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

namespace Schedwin.Techlogs
{
    /// <summary>
    /// Interaction logic for TechLogServiceHistory.xaml
    /// </summary>
    public partial class TechLogServiceHistoryView : SchedwinBaseWindow
    {
        public TechLogServiceHistoryView()
        {
            InitializeComponent();
        }

        private void RadGridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (e.RemovedItems.Count >0)
            {
                var viewModel = DataContext as TechLogServiceHistoryViewModel;
                var oldEntry = e.RemovedItems[0] as TechLogService;

                viewModel.SaveEntry(oldEntry);
            }


        }

        private void AddEntry_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogServiceHistoryViewModel;
            viewModel.NewEntry();
        }

        private void RemoveEntry_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogServiceHistoryViewModel;
            viewModel.DeleteEntry();
        }

        private void RadGridView_RowEditEnded(object sender, Telerik.Windows.Controls.GridViewRowEditEndedEventArgs e)
        {
            var viewModel = DataContext as TechLogServiceHistoryViewModel;
            var entryItem = e.Row.Item as TechLogService;
            viewModel.SaveEntry(entryItem);
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogServiceHistoryViewModel;
            var result = await viewModel.Save();
            if (result)
                Close();
        }

        private async void ServiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TechLogServiceHistoryViewModel;
            var sourceElement = e.Source as FrameworkElement;
            var techlogServiceItem = sourceElement.DataContext as TechLogService;
            if (e.AddedItems.Count >0)
            {
                var newDate = e.AddedItems[0] as DateTime?;
                await viewModel.UpdateTechlogID(techlogServiceItem, newDate.Value);
            }
        }
    }
}
