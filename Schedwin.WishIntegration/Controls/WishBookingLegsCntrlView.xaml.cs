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

namespace Schedwin.WishIntegration
{
    /// <summary>
    /// Interaction logic for WishBookingLegsCntrlView.xaml
    /// </summary>
    public partial class WishBookingLegsCntrlView : UserControl
    {
        public WishBookingLegsCntrlView()
        {
            InitializeComponent();
        }



        private void RadMenuItemCancel_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as WishBookingLegsCntrlViewModel;
            viewModel.CancelLeg();
        }

        private void RadMenuItemDelete_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as WishBookingLegsCntrlViewModel;
            viewModel.DeleteLeg();
        }

        private void RadGridView_FieldFilterEditorCreated(object sender, Telerik.Windows.Controls.GridView.EditorCreatedEventArgs e)
        {

            if (e.Column.UniqueName == "Earliest EX" || e.Column.UniqueName == "Latest EX" ||
                e.Column.UniqueName== "Earliest For" || e.Column.UniqueName=="Latest For" )
            {
                RadDateTimePicker editor = e.Editor as RadDateTimePicker;
                editor.InputMode = Telerik.Windows.Controls.InputMode.DateTimePicker;
            }
    
        }
    }
}
