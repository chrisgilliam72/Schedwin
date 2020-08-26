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
    /// Interaction logic for DefaultPassengersView.xaml
    /// </summary>
    public partial class DefaultPassengersView : RadWindow
    {
        public DefaultPassengersView()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as DefaultPassengersViewModel;
            viewModel.Init();
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
