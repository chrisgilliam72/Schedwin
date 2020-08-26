using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class GridReservationScheduleSubItem
    {
        public ReservationLegSchedule ReservationLegSchedule { get; set; }
        public DateTime FlightDate
        {
            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.BookingDate;
                else
                    return DateTime.Today;
            }
        }

   
        public String Aircraft
        {
            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.ACRegistration;
                else
                    return "";
            }
        }

        public String Pilot
        {
            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.PilotName;
                else
                    return "";
            }
        }

        public DateTime ETD
        {
            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.ETD;
                else
                    return DateTime.Today;
            }
        }

        public String From
        {

            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.FromAP;
                else
                    return "";
            }              

        }

        public DateTime ETA
        {
            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.ETA;
                else
                    return DateTime.Today;
            }
        }

        public String To
        {

            get
            {
                if (ReservationLegSchedule != null)
                    return ReservationLegSchedule.ToAP;
                else
                    return "";
            }

        }

        public static explicit operator GridReservationScheduleSubItem(ReservationLegSchedule legSchedule)
        {
            var gridReservationSchedule = new GridReservationScheduleSubItem();
            gridReservationSchedule.ReservationLegSchedule = legSchedule;
            return gridReservationSchedule;

        }
    }
}
