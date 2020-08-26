using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class GridReservationLegSubItem
    {
        public ReservationLeg ReservationLeg { get; set; }

        public DateTime BookingDate
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.BookingDate;
                else
                    return DateTime.Today;
            }
        }
        public String From
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.FromAP;
                else
                    return "";
            }
        }
        public String Ex
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.ExField;
                else
                    return "";
            }
        }

        public String To
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.ToAP;
                else
                    return "";
            }
        }

        public String For
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.ForField;
                else
                    return "";
            }
        }

        public int Distance
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.Distance;
                else
                    return 0;
            }
        }

        public bool TicketPrinted
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.TicketPrinted;
                else
                    return false;
            }
        }

        public bool SoleUse
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.SoleUse;
                else
                    return false;
            }
        }

        public String Notes
        {
            get
            {
                if (ReservationLeg != null)
                    return ReservationLeg.Notes;
                else
                    return "";
            }
            
        }


        public static explicit operator GridReservationLegSubItem(ReservationLeg resLeg)
        {
            var gridLegitem = new GridReservationLegSubItem();
            gridLegitem.ReservationLeg = resLeg;
            return gridLegitem;
        }
    }
}
