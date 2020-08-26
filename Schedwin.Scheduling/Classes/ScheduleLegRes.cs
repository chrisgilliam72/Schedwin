
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;

namespace Schedwin.Scheduling.Classes
{
    public class ScheduleLegRes
    {
     
        public bool IsDeleted { get; set; }
        public int IDX { get; set; }
        public int IDX_AC_Pilot { get; set; }

        public int IDX_Leg { get; set; }

        public int IDX_Boooking { get; set; }

        public int NumPax { get; set; }

        public int Budget { get; set; }

        public int IDX_Reservation { get; set; }
        public String Reservation { get; set; }

        public int PaxWeight { get; set; }

        public int LugWeight { get; set; }

        public bool SoleUse { get; set; }

        public List<ScheduleLegPax> PaxList { get; set; }

        public bool IsNew
        {
            get
            {
                return IDX < 1;
            }
        }


        public ScheduleLegRes()
        {
            PaxList = new List<ScheduleLegPax>();
        }

        public static explicit operator ScheduleLegRes(tsch_LegsRes tschLegsRes)
        {
            var scheduleLegRes = new ScheduleLegRes();
            scheduleLegRes.IDX = tschLegsRes.IDX;
            scheduleLegRes.IDX_Boooking = tschLegsRes.IDX_Bookings;
            scheduleLegRes.IDX_AC_Pilot = tschLegsRes.IDX_AC_Pilot;
            scheduleLegRes.NumPax = tschLegsRes.NumPax;
            scheduleLegRes.IDX_Leg = tschLegsRes.IDX_Legs;
   
            if (tschLegsRes.tsch_ReservationLegs.tsch_ReservationHeader!=null)
            {
                scheduleLegRes.IDX_Reservation = tschLegsRes.tsch_ReservationLegs.tsch_ReservationHeader.IDX;
                scheduleLegRes.Reservation = tschLegsRes.tsch_ReservationLegs.tsch_ReservationHeader.Reservationname;
                scheduleLegRes.SoleUse = tschLegsRes.tsch_ReservationLegs.SoleUse ?? false;

                if (tschLegsRes.tsch_ReservationLegs.tsch_ReservationHeader.tsch_Passengers!=null)
                {
                    var paxLst = tschLegsRes.tsch_ReservationLegs.tsch_ReservationHeader.tsch_Passengers.ToList();
                    foreach (var pax in paxLst)
                    {
                        var schedulePax = (ScheduleLegPax)pax;
                        scheduleLegRes.PaxWeight += pax.Weight ?? 0;
                        scheduleLegRes.LugWeight += pax.Luggageweight ?? 0;
                        scheduleLegRes.PaxList.Add(schedulePax);
                    }
                }

            }

   
            return scheduleLegRes;
        }

        public static explicit operator tsch_LegsRes(ScheduleLegRes scheduleLegRes)
        {
            var tschLegsRes = new tsch_LegsRes();
            tschLegsRes.IDX = scheduleLegRes.IDX;
            tschLegsRes.IDX_Bookings = scheduleLegRes.IDX_Boooking;
            tschLegsRes.IDX_AC_Pilot = scheduleLegRes.IDX_AC_Pilot;
            tschLegsRes.NumPax = Convert.ToInt16(scheduleLegRes.NumPax);
            tschLegsRes.Budget = scheduleLegRes.Budget;
            tschLegsRes.CoCode = "SEFO";
            return tschLegsRes;

        }
    }
}
