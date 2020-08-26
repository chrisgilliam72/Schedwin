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

namespace Schedwin.Prep
{
    /// <summary>
    /// Interaction logic for WeightsBalanceView.xaml
    /// </summary>
    public partial class WeightsBalanceView : SchedwinBaseWindow
    {
        public WeightsBalanceView()
        {
            InitializeComponent();
        }

        private async void SchedwinBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WeightsBalanceViewModel;
            viewModel.View = this;
            viewModel.AddLegControl(LegCntrl0);
            viewModel.AddLegControl( LegCntrl1);
            viewModel.AddLegControl( LegCntrl2);
            viewModel.AddLegControl(LegCntrl3);
            viewModel.AddLegControl(LegCntrl4);
            viewModel.AddLegControl(LegCntrl5);
            viewModel.AddLegControl(LegCntrl6);
            viewModel.AddLegControl(LegCntrl7);
            viewModel.AddLegControl(LegCntrl8);
            viewModel.AddLegControl(LegCntrl9);
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
               await viewModel.Init();
            }
        }

        private async void RadDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as WeightsBalanceViewModel;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                await viewModel.Refresh();
            }
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as WeightsBalanceViewModel;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
               await  viewModel.PilotSelected();
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WeightsBalanceViewModel;
            viewModel.Print();
      
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as WeightsBalanceViewModel;
            await viewModel.Save();
        }
    }
}
