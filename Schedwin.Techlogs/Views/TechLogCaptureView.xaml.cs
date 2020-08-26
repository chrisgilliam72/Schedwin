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
    /// Interaction logic for TechLogCaptureView.xaml
    /// </summary>
    public partial class TechLogCaptureView : Schedwin.Common.SchedwinBaseWindow
    {
        public TechLogCaptureView()
        {
            InitializeComponent();
        }


        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TechlogCaptureViewModel;
            if (viewModel.Validate())
            {
                var result = await viewModel.Save();
                if (result)
                {
                    DialogResult = true;
                    Close();
                }
            }

        }

        private void AddNewFuel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechlogCaptureViewModel;
            viewModel.AddFuelEntry();
        }

        private void AddNewBlock_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechlogCaptureViewModel;
        }

        private void RemoveFuel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as TechlogCaptureViewModel;
            viewModel.RemoveFuelEntry();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
        }

  

        private void TachEnd_LostFocus(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as TechlogCaptureViewModel;
        }
    }
}
