using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class GridReservationPaxSubItem
    {
        public ReservationPax  ReservationPax {get;set;}

        public String Name
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.FirstName;
                else
                    return "";
            }
        }

        public String Surname
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.Surname;
                else
                    return "";
            }

        }

        public String PaxType
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.PassengerType;
                else
                    return "";
            }
        }

        public int PaxTypeID
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.IDX_PaxType;
                else
                    return 0;
            }
        }

        public int Weight
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.Weight;
                else
                    return 0;
            }
        }

        public int Age
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.Age;
                else
                    return 0;
            }
        }

        public String Sex
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.Sex;
                else
                    return "";
            }
        }

        public int LuggageWeight
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.LuggageWeight;
                else
                    return 0;
            }
        }


         public bool TicketPrinted
        {
            get
            {
                if (ReservationPax != null)
                    return ReservationPax.TicketPrinted;
                else
                    return false;
            }
        }

        public static explicit operator GridReservationPaxSubItem(ReservationPax resPax)
        {
            var gridItem = new GridReservationPaxSubItem();
            gridItem.ReservationPax = resPax;
            return gridItem;
        }
    }

    

}
