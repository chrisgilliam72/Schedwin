using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class PassengerTicketLeg
    {
        public String Date { get; set; }
        public String From { get; set; }
        public String To { get; set; }

        public static explicit operator PassengerTicketLeg(tsch_ReservationLegs legs)
        {
            var legTicketInfo = new PassengerTicketLeg();
            legTicketInfo.Date = legs.BookingDate.ToShortDateString();
            if (legs.tset_Airports != null)
                legTicketInfo.To = legs.tset_Airports1.Airport;

            if (legs.tset_Airports1 != null)
                legTicketInfo.From = legs.tset_Airports.Airport;


            return legTicketInfo;
        }
    }



}
