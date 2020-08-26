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

namespace Schedwin.Scheduling
{
    /// <summary>
    /// Interaction logic for ViewLockedSchedulesView.xaml
    /// </summary>
    public partial class ViewLockedSchedulesView : Window
    {
        public ViewLockedSchedulesView()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewLockedSchedulesViewModel;
            viewModel.Refresh();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewLockedSchedulesViewModel;
            viewModel.UpdateUnlockButtonStatus();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewLockedSchedulesViewModel;
            viewModel.UpdateUnlockButtonStatus();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewLockedSchedulesViewModel;
            viewModel.Unlock();
            Close();
        }

   
    }
}
