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
    /// Interaction logic for NewSetupView.xaml
    /// </summary>
    public partial class NewSetupView : SchedwinBaseWindow
    {
        public NewSetupView()
        {
            InitializeComponent();
          
        }

        private void SchedwinBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewSetupViewModel;
            viewModel.View = this;
            viewModel.ldgeCntrlVM = this.lodgeInfoCntrl.DataContext as LodgeInfoCntrlViewModel;
            viewModel.airstrpCntrlVM = this.AirStripInfoCntrl.DataContext as AirstripInfoCntrlViewModel;
            viewModel.aircraftTypeCntrlVM = this.AircraftTypeCntrl.DataContext as AircraftTypeCntrlViewModel;
            viewModel.aircraftInfoCntrlVM =this.AircraftInfoCntrl.DataContext as AircraftInfoCntrlViewModel;
            viewModel.userInfoCntrlVM = this.UserInfoCntrl.DataContext as UserInfoCntrlViewModel;
            viewModel.pilotInfoCntrlVM = this.PilotInfoCntrl.DataContext as PilotInfoCntrlViewModel;
            viewModel.flightInfoCntrlVM = this.FlightInfoCntrl.DataContext as FlightInfoCntrlViewModel;
            viewModel.distanceCntrlVM = this.DistanceCntrl.DataContext as DistanceCntrlViewModel;
            viewModel.companyInfoVM = this.CompanyInfoCntrlView.DataContext as CompanyInfoCnrtrlViewModel;
            viewModel.Init();
        }

        private void RadTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as NewSetupViewModel;
            if (e.AddedItems.Count>0)
            {
                var item = e.AddedItems[0] as SetupBaseTreeItem;
                viewModel.SelectedTreeItem = item;
                viewModel.ShowControl(item);

            }
           
        }


        private void radContextMenu_Opening(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var radmenu = e.Source as RadContextMenu;

            var viewModel = DataContext as NewSetupViewModel;
            viewModel.ConfigureMenu(radmenu);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as NewSetupViewModel;
            if (viewModel.Validate())
                viewModel.Save();
        }
    }
}
