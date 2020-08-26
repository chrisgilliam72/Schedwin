using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class GridReservationHeaderSubItem
    {
        public ReservationHeader ResHeader {get;set;}

        public int PaxCount
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.PaxCount;
                else
                    return 0;
            }
        }

        public DateTime CaptureDate
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.DateCaptured;
                else
                    return DateTime.Today;
            }
        }

        public String OperatorAgent
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.OperatorAgentName;
                else
                    return "";
            }
        }

        public String ReservationStatus
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.ReservationStatus;
                else
                    return "";
            }
        }

       

        public bool TicketRequired
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.TicketRequired;
                else
                    return false;
            }
        }
        
        public bool TicketPrinted
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.TicketPrinted;
                else
                    return false;
            }

        }

        public String Notes
        {
            get
            {
                if (ResHeader != null)
                    return ResHeader.ReservationNote;
                else
                    return "";
            }
        }

        public static explicit operator GridReservationHeaderSubItem (ReservationHeader reservationHeader)
        {
            var gridHeaderItem = new GridReservationHeaderSubItem();
            gridHeaderItem.ResHeader = reservationHeader;
            return gridHeaderItem;
        }
    }
}
