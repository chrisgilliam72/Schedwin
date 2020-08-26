using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration.Classes
{
    public class WIReservation
    {
        public int IDX { get; set; }
        
        public String ReservationName { get; set; }

        public int IDX_ResStatus { get; set; }

        public int WishBookingID { get; set; }

        public int WishPartyGroupID { get; set; }
    }
}
