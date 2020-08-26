using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class ReservationLegSchedule
    {
        public DateTime BookingDate { get; set; }
        public int IDX_Pilot { get; set; }
        public String PilotName { get; set; }
        public int IDX_AircraftType { get; set; }
        public String ACRegistration { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ETA { get; set; }
        public int IDX_FromAP { get; set; }
        public String FromAP { get; set; }
        public int IDX_ToAP { get; set; }
        public String ToAP { get; set; }

    }
}
