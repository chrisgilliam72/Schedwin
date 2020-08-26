using Schedwin.Common;
using Schedwin.Data;
using Schedwin.WishIntegration.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using telerik=Telerik.Windows.Controls;
using Schedwin.Reservations.Classes;

namespace Schedwin.WishIntegration
{
    public class RemovedWishBookingHeaderCntrlViewModel : ViewModelBase
    {
        public int IDX_User { get; set; }

        private RangeObservableCollection<WIRemovedBooking> _lstRemovedHdrs;
        public RangeObservableCollection<WIRemovedBooking> ListRemovedHeaders
        {
            get
            {
                return _lstRemovedHdrs;
            }
            set
            {
                _lstRemovedHdrs = value;
            }
        }

        public WIRemovedBooking SelectedBooking { get; set; }


        public RemovedWishBookingHeaderCntrlViewModel()
        {
            ListRemovedHeaders = new RangeObservableCollection<WIRemovedBooking>();
        }


        public void Refresh(List<WIReservationHeader> listHdrs)
        {
            ListRemovedHeaders.Clear();
            var rmvedBookingList = listHdrs.Select(x => (WIRemovedBooking)x).ToList();

            ListRemovedHeaders.AddRange(rmvedBookingList);
            NotifyPropertyChanged("ListRemovedHeaders");

        }
        public  void DeleteBooking()
        {
            if (SelectedBooking != null)
            {
                var parameters = new telerik.DialogParameters();
                parameters.Header = "Delete Reservation";
                parameters.Content = "Are you sure want to delete this reservation?";
                parameters.OkButtonContent = "Yes";
                parameters.CancelButtonContent = "No";
                parameters.Closed = OnDeleteBookingConfirmed;
                telerik.RadWindow.Confirm(parameters);
            }
        }

        public void CancelBooking()
        {
           var parameters = new telerik.DialogParameters();
            parameters.Header = "Cancel Reservation";
            parameters.Content = "Are you sure want to cancel this Reservation ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnCancelLegConfirmed;
            telerik.RadWindow.Confirm(parameters);


        }

        private void RemoveItem(int HdrID)
        {
            var item = ListRemovedHeaders.FirstOrDefault(x => x.ResHdrID == HdrID);
            if (item!=null)
            {
                ListRemovedHeaders.Remove(item);
                NotifyPropertyChanged("ListRemovedHeaders");
            }
        }

        private async void OnDeleteBookingConfirmed(object sender, telerik.WindowClosedEventArgs e)
        {
            if (e.DialogResult.HasValue && e.DialogResult == true)
            {

                if (SelectedBooking != null)
                {
                    var res = new Schedwin.Reservations.Classes.Reservations();
                    var result = await res.DeleteBooking(SelectedBooking.ResHdrID, Server, Database);
                    if (result)
                    {
                       
                        SuccessWindow.Display("Reservation deleted");
                        RemoveItem(SelectedBooking.ResHdrID);

                    }

                    else
                        FailWindow.Display("Unable to delete reservation:\r\n" + res.LastError);
                }

            }

        }

       

        private async void OnCancelLegConfirmed(object sender, telerik.WindowClosedEventArgs e)
        {
            if (e.DialogResult.HasValue && e.DialogResult == true)
            {

                if (SelectedBooking != null)
                {
                    var res = new Schedwin.Reservations.Classes.Reservations();
                    var result=await res.CancelBooking(SelectedBooking.ResHdrID, Server, Database);
                    if (result)
                    {
                        SuccessWindow.Display("Reservation cancelled");
                        RemoveItem(SelectedBooking.ResHdrID);

                    }

                    else
                        FailWindow.Display("Unable to cancel reservation:\r\n" + res.LastError);
                }

            }

        }
    }
}
