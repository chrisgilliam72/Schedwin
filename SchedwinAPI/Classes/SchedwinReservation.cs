using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SchedwinAPI
{
    public class SchedwinReservation
    {

        public String Name { get; set; }
        public int ReferenceID { get; set; }

        public String ExternalBookingName { get; set; }

        public String ExternalGroupName { get; set; }

        public int ExternalGroupID { get; set; }

        public int ExternalID { get; set; }

        public String ExternalAgent { get; set; }

        public int PrincipalID { get; set; }

        public String Notes { get; set; }

        public String Status { get; set; }


        public bool Cancelled { get; set; }

        public List<SchedwinPassenger> Passengers { get; set; }

        public List<SchedwinLeg> Legs { get; set; }

        public SchedwinReservation()
        {
            Passengers = new List<SchedwinPassenger>();
            Legs = new List<SchedwinLeg>();
        }

        public static explicit operator SchedwinReservation(tsch_ReservationHeader resHdr)
        {
            var reservation = new SchedwinReservation();

            reservation.ReferenceID = resHdr.IDX;
            reservation.Name = resHdr.Reservationname;
            reservation.ExternalID = resHdr.tsch_WISHIntegrationHeader.FirstOrDefault().WISHBookingID;
            reservation.ExternalGroupName = resHdr.tsch_WISHIntegrationHeader.FirstOrDefault().WISHPGName;
            reservation.ExternalGroupID = resHdr.tsch_WISHIntegrationHeader.FirstOrDefault().WISHPGID;
            reservation.Notes = resHdr.Notes;
            reservation.Status = resHdr.tlst_ResStatus.ReservationStatus;
            reservation.ExternalAgent = resHdr.tsch_WISHIntegrationHeader.FirstOrDefault().WishConsultant;

            var tmpLegLst = resHdr.tsch_ReservationLegs.Select(x => (SchedwinLeg)x).ToList();
            reservation.Legs.AddRange(tmpLegLst);

            var tmpPaxLst = resHdr.tsch_Passengers.Select(x => (SchedwinPassenger)x).ToList();
            reservation.Passengers.AddRange(tmpPaxLst);

            return reservation;
        }

        public static explicit operator tsch_ReservationHeader(SchedwinReservation reservation)
        {
            var tschHeader = new tsch_ReservationHeader();

            tschHeader.Reservationname = reservation.ExternalBookingName + "[" + reservation.ExternalGroupName + "]";
            tschHeader.DateCaptured = DateTime.Today;
            tschHeader.Numpax = reservation.Passengers.Count;
            //tschHeader.IDX_Operators = wiResHeader.OperatorID;
            tschHeader.IDX_OperatorAgent = null;
            //tschHeader.IDX_Personnel = idxUser;
            tschHeader.Active = true;
            tschHeader.TicketPrinted = false;
            tschHeader.TicketRequired = true;
            tschHeader.IDX_ResClass = 2;
            tschHeader.IDX_ResType = 1;
            tschHeader.CURNCYID = null;
            tschHeader.Notes = reservation.Notes;
            tschHeader.WISHID = reservation.ExternalID;
            tschHeader.IDX_ResStatus = 1;
            tschHeader.CoCode = "";

            var tschWishHeader = new tsch_WISHIntegrationHeader();
            tschWishHeader.WISHBookingID = reservation.ExternalID;
            tschWishHeader.WISHPGName = reservation.ExternalGroupName;
            tschWishHeader.WISHPGID = reservation.ExternalGroupID;
            tschWishHeader.WISHPGPax = tschHeader.Numpax;
            tschWishHeader.DepartureDate = reservation.Legs.Min(x => x.Date);
            tschWishHeader.WISHBookingStatus = "Prov";
            tschWishHeader.WishConsultant = reservation.ExternalAgent;
            tschHeader.tsch_WISHIntegrationHeader.Add(tschWishHeader);

            return tschHeader;
        }
    }
}
