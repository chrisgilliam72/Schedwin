using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration.Classes
{
    public class WICharterBookingPax
    {
        public int WishGuestID { get; set; }

        public String FirstName { get; set; }

        public String Surname { get; set; }

        public String PassportNo { get; set; }

        public String Nationality { get; set; }

        public int Age { get; set; } 

        public int Weight { get; set; }

        public String Gender { get; set; }

        public WICharterBookingPax()
        {
            FirstName = "";
            Surname = "";
            Gender = "";
        }

    }
}
