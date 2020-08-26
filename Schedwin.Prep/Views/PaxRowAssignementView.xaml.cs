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
    /// Interaction logic for PaxRowAssignementView.xaml
    /// </summary>
    public partial class PaxRowAssignementView : Window
    {
        public PaxRowAssignementView()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PaxRowAssignementViewModel;
            if (viewModel.Valid())
            {
                DialogResult = true;
                Close();
            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

            var viewModel = DataContext as PaxRowAssignementViewModel;
            viewModel.Clear();
            viewModel.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PaxRowAssignementViewModel;
            viewModel.Refresh();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var viewModel = DataContext as PaxRowAssignementViewModel;

            if (textBox.Text == "P")
            {
                e.Handled = true;
                return;
            }

            if (e.Key != Key.Tab)
            {

                if (e.Key != Key.M && e.Key != Key.F && e.Key != Key.Tab)
                    e.Handled = true;

                else if (textBox.Text.Length > 0)
                    e.Handled = true;

            }

            viewModel.UpdatePaxTotals();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PaxRowAssignementViewModel;
            viewModel.UpdatePaxTotals();
        }
    }
}
