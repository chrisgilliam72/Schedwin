using Schedwin.Reservations.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration.Classes
{
    public class WIReservationPax
    {
        public ReservationPax ResPax { get; set; }

        public static explicit operator WIReservationPax(ReservationPax resPax)
        {
            var wiResPax = new WIReservationPax();
            wiResPax.ResPax = resPax;
            return wiResPax;
        }

        public static explicit operator WIReservationPax(WICharterBookingPax resPax)
        {
            var wiResPax = new WIReservationPax();
            wiResPax.ResPax = new ReservationPax();
            wiResPax.ResPax.WishGuestID = resPax.WishGuestID;
            wiResPax.ResPax.FirstName = resPax.FirstName;
            wiResPax.ResPax.Surname = resPax.Surname;
            wiResPax.ResPax.Sex = resPax.Gender;
            wiResPax.ResPax.Age = resPax.Age;
            wiResPax.ResPax.PassportNo = resPax.PassportNo;
            wiResPax.ResPax.LuggageWeight = 44;
            return wiResPax;
        }
    }
}
