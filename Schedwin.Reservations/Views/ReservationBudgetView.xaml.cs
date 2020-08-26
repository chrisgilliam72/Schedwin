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
using Telerik.Windows.Controls;

namespace Schedwin.Reservations
{
    /// <summary>
    /// Interaction logic for ReservationBudgetView.xaml
    /// </summary>
    public partial class ReservationBudgetView : RadWindow
    {
        public ReservationBudgetView()
        {
            InitializeComponent();
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewModel = DataContext as ReservationBudgetViewModel;

                var result = await viewModel.Save();
                if (String.IsNullOrEmpty(result))
                {
                    SuccessWindow.Display("Budgets updated.");
                    DialogResult = true;
                    Close();
                }
                else
                {
                    FailWindow.Display("Unavle to update budget:\r\n" + result);
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save budget info :" + Environment.NewLine + message);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationBudgetViewModel;
            DialogResult = true;
            Close();

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationBudgetViewModel;
            viewModel.Cancel();
            DialogResult = false;
            Close();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationBudgetViewModel;
            viewModel.Refresh();
        }

        private async void NewLine_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationBudgetViewModel;
            await viewModel.NewBudgetLine("Seat");
        }

        private void DelLine_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReservationBudgetViewModel;
            if (viewModel.SelectedBudget!=null)
                RadWindow.Confirm("Are you sure you want to remove this budget line ?", DeleteBudgetLine);
        }

        private void DeleteBudgetLine(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                var viewModel = DataContext as ReservationBudgetViewModel;
                viewModel.DeleteBudgetLine();
            }
        }

  
    }
}
