using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SchedwinAPI
{
    public class SchedwinLeg
    {
        public int ExternalReferenceID { get; set; }
        public DateTime Date { get; set; }

        public String For { get; set; }
        public String From { get; set; }

        public String To { get; set; }

        public String Ex { get; set; }
        public bool SoleUse { get; set; }

        public bool Scheduled { get; set; }

        public String Notes { get; set; }

        public DateTime? LatestEx { get; set; }
        public DateTime? EarliestEx { get; set; }

        public DateTime? LatestFor { get; set; }
        public DateTime? EarliestFor { get; set; }

        public bool Cancelled { get; set; }

        public static explicit operator SchedwinLeg (tsch_ReservationLegs tschLeg)
        {
            var schedwinLeg = new SchedwinLeg();
            schedwinLeg.ExternalReferenceID = Convert.ToInt32(tschLeg.WISHIDLegs ?? -1);
            schedwinLeg.Ex = tschLeg.ExField;
            schedwinLeg.For = tschLeg.ForField;
            schedwinLeg.To = tschLeg.tset_Airports.Airport;
            schedwinLeg.From = tschLeg.tset_Airports1.Airport;
            schedwinLeg.Date = tschLeg.BookingDate;
            schedwinLeg.Notes = tschLeg.Notes;
            schedwinLeg.SoleUse = tschLeg.SoleUse ?? false;
            schedwinLeg.LatestEx = tschLeg.LatestEx;
            schedwinLeg.EarliestEx = tschLeg.EarliestEx;
            schedwinLeg.LatestFor = tschLeg.LatestFor;
            schedwinLeg.EarliestFor = tschLeg.EarliestFor;
            schedwinLeg.Cancelled = tschLeg.Cancelled ?? false; ;
            return schedwinLeg;
        }


        public static explicit operator tsch_ReservationLegs(SchedwinLeg leg)
        {
            var tschResLeg = new tsch_ReservationLegs();
            tschResLeg.WISHIDLegs = leg.ExternalReferenceID;
            tschResLeg.BookingDate = leg.Date;
            tschResLeg.ForField = leg.For;
            tschResLeg.ExField = leg.Ex;
            tschResLeg.RateType = "";
            tschResLeg.EarliestEx = DateTime.Today;
            tschResLeg.LatestEx = DateTime.Today;
            tschResLeg.EarliestFor = DateTime.Today;
            tschResLeg.LatestFor = DateTime.Today;
            tschResLeg.Notes = leg.Notes;
            tschResLeg.GameflightTime = 0;
            tschResLeg.DirectDistance = 0;
            tschResLeg.Budget = 0;
            tschResLeg.Cancelled = leg.Cancelled;
            tschResLeg.rowguid = new Guid();


            var tschWishLeg = new tsch_WishIntegrationLeg();
            tschWishLeg.WishSectorID = leg.ExternalReferenceID;
            tschWishLeg.ETA = leg.Date;
            tschWishLeg.ETD = leg.Date;
            tschWishLeg.WishFor = leg.For;
            tschWishLeg.WishEx = leg.Ex;
            tschResLeg.tsch_WishIntegrationLeg.Add(tschWishLeg);

            return tschResLeg;
        }
    }
}
