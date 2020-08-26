using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration.Classes
{
    public class WIRemovedBooking
    {
        public int ResHdrID { get; set; }
        public int WishID { get; set; }

        public int WishPGID { get; set; }

        public String ResName { get; set; }

        public String Status { get; set; }

        public RangeObservableCollection<WIRemovedBookingLeg> Legs { get; set; }

        public WIRemovedBooking()
        {
            Legs = new RangeObservableCollection<WIRemovedBookingLeg>();
        }

        public static explicit operator WIRemovedBooking(WIReservationHeader wiResHeader)
        {
            var removedBooking = new WIRemovedBooking();
            removedBooking.ResHdrID = wiResHeader.WishResHeader.Res_IDX;
            removedBooking.WishID = wiResHeader.WishResHeader.BookingID;
            removedBooking.WishPGID = wiResHeader.WishResHeader.PartyGroupID;
            removedBooking.ResName = wiResHeader.WishResHeader.ReservationName;
            return removedBooking;
        }
    }
 }
