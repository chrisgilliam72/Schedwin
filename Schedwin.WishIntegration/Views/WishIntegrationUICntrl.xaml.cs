using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Schedwin.WishIntegration;
using Schedwin.Common;
using Telerik.Windows.Controls;

namespace Schedwin.WishIntegration
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary> 
    public partial class WishIntegrationUICntrl : UserControl
    {
        public int CountryID { get; set; }
        
        public int WishPrincipalID { get; set; }
        public String Server { get; set; }
        public String Database { get; set; }

        public int UserID { get; set; }

        public int CompanyID { get; set; }
        public WishIntegrationUICntrl()
        {
            InitializeComponent();
        }

        public void WaitCursor()
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            var bookingsHeaderVM = this.cntrlBookingHeaders.DataContext as WishBookingHeaderCntrlViewModel;
            var legsVM = this.cntrlBookingLegs.DataContext as WishBookingLegsCntrlViewModel;
            var removedVM = this.cntrlRemovedHeaders.DataContext as RemovedWishBookingHeaderCntrlViewModel;
            //this.cntrlBookingHeaders.MainWindow = System.Windows.Window.GetWindow(sender as RadWindow);
            legsVM.Server = Server;
            legsVM.Database = Database;
            legsVM.IDX_User = UserID;
            removedVM.Server = Server;
            removedVM.Database = Database;
            removedVM.IDX_User = UserID;
            bookingsHeaderVM.Server = Server;
            bookingsHeaderVM.Database = Database;
            bookingsHeaderVM.IDX_User = UserID;
            bookingsHeaderVM.IDX_Company = CompanyID;
            bookingsHeaderVM.LegViewModel = legsVM;
            bookingsHeaderVM.RemovedLegsViewModel = removedVM;
            bookingsHeaderVM.WishPrincipalID = WishPrincipalID;
            bookingsHeaderVM.CountryID = CountryID;

        }


        private async void FileRetrieve_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var bookingsHeaderVM = this.cntrlBookingHeaders.DataContext as WishBookingHeaderCntrlViewModel;
            var legsVM = this.cntrlBookingLegs.DataContext as WishBookingLegsCntrlViewModel;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                await bookingsHeaderVM.Refresh();
            }
        }

        private void RadWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }



       

        //private  void RadSave_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        //{
        //    var bookingsHeaderVM = this.cntrlBookingHeaders.DataContext as WishBookingHeaderCntrlViewModel;
        //     bookingsHeaderVM.SaveSelection();

        //}

    }
}
