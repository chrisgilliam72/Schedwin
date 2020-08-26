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
    /// Interaction logic for TechLogListView.xaml
    /// </summary>
    public partial class TechLogListView : Schedwin.Common.SchedwinBaseWindow
    {
        public TechLogListView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            viewModel.View = this;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await viewModel.Init();
            }
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await viewModel.Refresh();

            }

        }

        private void NewTechlog_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            viewModel.AddTechLog();
        }

        private void DeleteTechlog_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            viewModel.DeleteTechLog();
        }

        private void RadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            viewModel.EditTechLog();

        }

        private async void Aircraft_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await viewModel.Refresh();

            }
        }

        private void btnServiceHistoryClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TechLogListViewModel;
            viewModel.EditServiceHistory();
        }

        private void SchedwinBaseWindow_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }
    }
}
