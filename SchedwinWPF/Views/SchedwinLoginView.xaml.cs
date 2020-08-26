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
using Telerik.Windows.Controls;

namespace SchedwinWPF
{
    /// <summary>
    /// Interaction logic for SchedwinLogin.xaml
    /// </summary>
    public partial class SchedwinLoginView : Window
    {
        public SchedwinLoginView()
        {
            InitializeComponent();
        }

        private async void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

    
            var viewModel = DataContext as SchedwinLoginViewModel;
            viewModel.Init();

            var isVersionAllowed = await viewModel.IsVersionAllowed();
            var canLogonNow = await viewModel.CanContinueUsing(true);
            if (isVersionAllowed && canLogonNow)
            {
                viewModel.RefreshRegions();
                viewModel.LoadLastRegion();
            }
            else
                Close();

        }
        private async void Login_Click(object sender, RoutedEventArgs e)
        {

            var viewModel = DataContext as SchedwinLoginViewModel;
            viewModel.Password = this.txtPassword.Password;
            var logonSuccess = await viewModel.Logon();
            if (logonSuccess)
            {
                viewModel.LogLogon();
                viewModel.SaveLastSelectedRegion();
                Visibility = Visibility.Hidden;
                viewModel.ShowMainWindow();
                Close();

            }

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void RadRegions_DropDownOpened(object sender, EventArgs e)
        {
            //var viewModel = DataContext as SchedwinLoginViewModel;
            //viewModel.RefreshRegions();
        }

        private void CurrentADUserCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as SchedwinLoginViewModel;
            viewModel.ShowIncorrectPassword = false;
        }
    }
}
