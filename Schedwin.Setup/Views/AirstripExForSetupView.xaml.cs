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

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for AirstripExForSetupView.xaml
    /// </summary>
    public partial class AirstripExForSetupView : SchedwinBaseWindow
    {
        public AirstripExForSetupView()
        {
            InitializeComponent();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripExForSetupViewModel;
            viewModel.Init();
        }

        private void AddNewItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripExForSetupViewModel;
            viewModel.AddNewExForGridItem();
        }


        private void RemoveItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as AirstripExForSetupViewModel;
            viewModel.RemoveExForGridItem();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }
    }
}
