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

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for ChooseTPCodeView.xaml
    /// </summary>
    public partial class ChooseTPCodeView : Window
    {
        public ChooseTPCodeView()
        {
            InitializeComponent();
        }

        private async  void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var viewmodel = DataContext as ChooseTPCodeViewModel;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await viewmodel.RefreshTPLIst();
            }
                

        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewmodel = DataContext as ChooseTPCodeViewModel;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await viewmodel.Init();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox) sender).Text.Length <3)
            {
                MessageBox.Show("Please enter at least 3 characters.");
            }

        }

        private void RadGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Close();
        }


    }
}
