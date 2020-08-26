using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Reservations.Classes;
namespace Schedwin.WishIntegration.Classes
{
    public class WIRemovedBookingLeg
    {
        public DateTime FlightDate { get; set; }
        public String From { get; set; }
        public String To { get; set; } 
        public String Status { get; set; }
        public bool Cancelled { get; set; }

        public static explicit operator WIRemovedBookingLeg(ReservationLeg wishLeg)
        {
            var resLeg = new WIRemovedBookingLeg();
            resLeg.FlightDate = wishLeg.BookingDate;
            resLeg.From = wishLeg.FromAP;
            resLeg.To = wishLeg.ToAP;
            resLeg.Status = wishLeg.IsScheduled ? "Scheduled leg" :"";
            return resLeg;
        }
    }
}
