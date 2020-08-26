using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public enum SchedulingStatus { none, partial, scheduled}
    public class Reservation
    {
        public bool IsNew {get;set;}
        public ReservationHeader Header { get; set; }

        public List<ReservationPax> Passengers { get; set; }

        public List<ReservationLeg> Legs { get; set; }

        public SchedulingStatus ScheduledStatus
        {
            
            get
            {
                SchedulingStatus tmpStatus = SchedulingStatus.none;
                foreach (var leg in Legs)
                {
                    if (leg.IsScheduled)
                        tmpStatus = SchedulingStatus.scheduled;
                    else
                    {
                        if (tmpStatus == SchedulingStatus.scheduled)
                            tmpStatus = SchedulingStatus.partial;
                    }
                }

                return tmpStatus;
            }
        }
        public List<ReservationLegSchedule> LegSchedules
        {
            get
            {
                return Legs.SelectMany(x => x.Schedules).ToList();
            }
        }

        public List<ReservationPax> DeletedPax { get; set; }

        public Reservation()
        {
            Header = new ReservationHeader();
            Passengers = new List<ReservationPax>();
            Legs = new List<ReservationLeg>();
            DeletedPax = new List<ReservationPax>();

            IsNew = true;
        }

        public Reservation(Reservation Reservation)
        {
            Header = new ReservationHeader(Reservation.Header);
            Passengers = new List<ReservationPax>();
            Legs = new List<ReservationLeg>();

            foreach (var pax in Reservation.Passengers)
                Passengers.Add(new ReservationPax(pax));

            foreach (var leg in Reservation.Legs)
                Legs.Add(new ReservationLeg(leg));

            IsNew = Reservation.IsNew;
        }

        public static explicit operator tbReservationHeader(Reservation reservation)
        {
            var tbResHeader = (tbReservationHeader)reservation.Header;
            foreach (var leg in reservation.Legs)
            {
                if (!leg.Deleted)
                {
                    var tbResLeg = (tbReservationLeg)leg;
                    tbResHeader.tbReservationLegs.Add(tbResLeg);

                    foreach (var budget in leg.Budgets)
                    {
                        var tbLegBudget = (tbReservationLegBudget)budget;
                        tbResLeg.tbReservationLegBudgets.Add(tbLegBudget);
                    }
                }

            }

            foreach (var pax in reservation.Passengers)
            {

                var tbPassenger = (tbPassenger)pax;
                tbResHeader.tbPassengers.Add(tbPassenger);
            }

            tbResHeader.Numpax = reservation.Passengers.Count;

            return tbResHeader;
        }

        public tsch_ReservationHeader ToDBTree()
        {
            var tschHdr = (tsch_ReservationHeader)Header;
            foreach (var leg in Legs)
            {
                if (!leg.Deleted)
                {
                    var tschLeg = (tsch_ReservationLegs)leg;
                    tschHdr.tsch_ReservationLegs.Add(tschLeg);

                    foreach (var budget in leg.Budgets)
                    {
                        var tschBudget = (tsch_ReservationLegBudget)budget;
                        tschLeg.tsch_ReservationLegBudget.Add(tschBudget);
                    }
                }


            }

            foreach (var pax in Passengers)
            {
              
                var tschPax = (tsch_Passengers)pax; 
                tschHdr.tsch_Passengers.Add(tschPax);
            }

            tschHdr.Numpax = Passengers.Count;
            return tschHdr;
        }

        public void RemovePassenger(ReservationPax pax)
        {
            Passengers.Remove(pax);
            DeletedPax.Add(pax);
        }
      

        public static implicit operator Reservation(tbReservationHeader tbResHeader)
        {
            var reservation = new Reservation();

            if (tbResHeader != null)
            {
                int masterBookingID = 0;
                reservation.Header.Res_IDX = tbResHeader.pkReservationHeaderID;
                reservation.Header.ReservationName = tbResHeader.Reservationname;
                reservation.Header.PaxCount = tbResHeader.Numpax;
                reservation.Header.DateCaptured = tbResHeader.DateCaptured;
                reservation.Header.IDX_Operator = tbResHeader.fkOperatorID;
                if (tbResHeader.tbOperator != null)
                    reservation.Header.OperatorName = tbResHeader.tbOperator.Name;
                if (tbResHeader.tbUser != null)
                    reservation.Header.SefofaneAgentName = tbResHeader.tbUser.username;

                reservation.Header.IDX_OperatorAgent = tbResHeader.fkOperatorAgentID;

                reservation.Header.IDX_Personnel = tbResHeader.fkUserID; 
                reservation.Header.IDX_ResStatus = tbResHeader.fkResStatusID;
                if (tbResHeader.tbReservationStatu != null)
                    reservation.Header.ReservationStatus = tbResHeader.tbReservationStatu.Description;
                reservation.Header.Active = tbResHeader.Active;
                reservation.Header.TicketPrinted = tbResHeader.TicketPrinted;
                reservation.Header.TicketRequired = tbResHeader.TicketRequired;
                reservation.Header.IDX_ResType = tbResHeader.fkResTypeID;
                if (tbResHeader.tbReservationType != null)
                    reservation.Header.ReservationType = tbResHeader.tbReservationType.Description;
                reservation.Header.ReservationNote = tbResHeader.Notes;
                reservation.Header.CurrencyID = tbResHeader.CURNCYID;

                var resPax = tbResHeader.tbPassengers.Select(x => (ReservationPax)x).ToList();
                var resLegs = tbResHeader.tbReservationLegs.Select(x => (ReservationLeg)x).ToList();

                var orderedLegs = resLegs.OrderBy(x => x.BookingDate).ToList();
                if (orderedLegs != null && orderedLegs.Count > 0)
                {
                    reservation.Header.FirstBookingDate = orderedLegs.First().BookingDate;
                    reservation.Header.LastBookingDate = orderedLegs.Last().BookingDate;
                }

                if (tbResHeader.tbUser!=null)
                    reservation.Header.OperatorAgentName = tbResHeader.tbUser.Firstname + " " + tbResHeader.tbUser.Surname;
                reservation.Legs.AddRange(resLegs);
                reservation.Passengers.AddRange(resPax);

                if (tbResHeader.tbWISHIntegrationHeaders != null && tbResHeader.tbWISHIntegrationHeaders.Count > 0)
                {
                    reservation.Header.BookingID = tbResHeader.tbWISHIntegrationHeaders.First().WISHBookingID;
                    reservation.Header.PartyGroupID = tbResHeader.tbWISHIntegrationHeaders.First().WISHPGID;
                    reservation.Header.WishConsultant = tbResHeader.tbWISHIntegrationHeaders.First().WishConsultant;
                }


                //if (tschHeader.tset_Personnel != null)
                //    reservation.Header.OperatorAgentName = tschHeader.tset_Personnel.Firstname + " " + tschHeader.tset_Personnel.Surname;


                //reservation.Header.IsMaster = masterBookingID == tschHeader.IDX ? true : false; ;

                //if (tschHeader.tsch_SplitReservations1.Count > 0)
                //{
                //    reservation.Header.IsSplit = true;
                //    masterBookingID = tschHeader.tsch_SplitReservations1.First().fkParentReservation.Value;
                //}

                //else if (tschHeader.tsch_SplitReservations.Count > 0)
                //{
                //    reservation.Header.IsSplit = true;
                //    masterBookingID = tschHeader.tsch_SplitReservations.First().fkParentReservation.Value;
                //}




            }

            return reservation;
        }
        public static implicit operator Reservation (tsch_ReservationHeader tschHeader)
        {
            var reservation = new Reservation();

            if (tschHeader != null)
            {
                int masterBookingID = 0;
                reservation.Header.Res_IDX = tschHeader.IDX;
                reservation.Header.ReservationName = tschHeader.Reservationname;
                reservation.Header.PaxCount = tschHeader.Numpax;
                reservation.Header.DateCaptured = tschHeader.DateCaptured;
                reservation.Header.IDX_Operator = tschHeader.IDX_Operators;
                if (tschHeader.tset_Companies!=null)
                    reservation.Header.OperatorName = tschHeader.tset_Companies.CompanyName;
                reservation.Header.IDX_OperatorAgent = tschHeader.IDX_OperatorAgent;
                if (tschHeader.tset_Personnel!=null)
                    reservation.Header.OperatorAgentName = tschHeader.tset_Personnel.Firstname + " " + tschHeader.tset_Personnel.Surname;
                reservation.Header.IDX_Personnel = tschHeader.IDX_Personnel;
                if (tschHeader.tset_Personnel1!=null)
                    reservation.Header.SefofaneAgentName = tschHeader.tset_Personnel1.Firstname + " " + tschHeader.tset_Personnel1.Surname;
                reservation.Header.IDX_ResStatus = tschHeader.IDX_ResStatus;
                if (tschHeader.tlst_ResStatus!=null)
                    reservation.Header.ReservationStatus = tschHeader.tlst_ResStatus.ReservationStatus;
                reservation.Header.Active = tschHeader.Active;
                reservation.Header.TicketPrinted = tschHeader.TicketPrinted;
                reservation.Header.TicketRequired = tschHeader.TicketRequired;
                reservation.Header.IDX_ResType = tschHeader.IDX_ResType.Value;
                if (tschHeader.tlst_ResType!=null)
                    reservation.Header.ReservationType = tschHeader.tlst_ResType.ResType;
                reservation.Header.ReservationNote = tschHeader.Notes;
                reservation.Header.CurrencyID = tschHeader.CURNCYID;
                if (tschHeader.tsch_WISHIntegrationHeader!=null && tschHeader.tsch_WISHIntegrationHeader.Count >0)
                {
                    reservation.Header.BookingID = tschHeader.tsch_WISHIntegrationHeader.First().WISHBookingID;
                    reservation.Header.PartyGroupID = tschHeader.tsch_WISHIntegrationHeader.First().WISHPGID;
                    reservation.Header.WishConsultant = tschHeader.tsch_WISHIntegrationHeader.First().WishConsultant;
                }
              
                if (tschHeader.tsch_SplitReservations1.Count > 0)
                {
                    reservation.Header.IsSplit = true;
                    masterBookingID = tschHeader.tsch_SplitReservations1.First().fkParentReservation.Value;
                }

                else if (tschHeader.tsch_SplitReservations.Count > 0)
                {
                    reservation.Header.IsSplit = true;
                    masterBookingID = tschHeader.tsch_SplitReservations.First().fkParentReservation.Value;
                }


                reservation.Header.IsMaster = masterBookingID == tschHeader.IDX ? true : false; ;
                var resPax = tschHeader.tsch_Passengers.Select(x => (ReservationPax)x).ToList();
                var resLegs = tschHeader.tsch_ReservationLegs.Select(x => (ReservationLeg)x).ToList();

                var orderedLegs=resLegs.OrderBy(x => x.BookingDate).ToList();
                if (orderedLegs!=null && orderedLegs.Count >0)
                {
                    reservation.Header.FirstBookingDate = orderedLegs.First().BookingDate;
                    reservation.Header.LastBookingDate = orderedLegs.Last().BookingDate;
                }

                reservation.Legs.AddRange(resLegs);
                reservation.Passengers.AddRange(resPax);



            }
            return reservation;
        }

        public static implicit operator Reservation(tbWISHIntegrationHeader tbWISHIntegrationHeader)
        {
            var reservation = new Reservation();
            reservation.Header.BookingID = tbWISHIntegrationHeader.WISHBookingID;
            reservation.Header.PartyGroupID = tbWISHIntegrationHeader.WISHPGID;
            reservation.Header.PaxCount = tbWISHIntegrationHeader.WISHPGPax;
            reservation.Header.WishResStatus = tbWISHIntegrationHeader.WISHBookingStatus;
            reservation.Header.DepartureDate = tbWISHIntegrationHeader.DepartureDate;
            reservation.Header.PartyGroupName = tbWISHIntegrationHeader.WISHPGName;
            reservation.Header.WishConsultant = tbWISHIntegrationHeader.WishConsultant;
            if (tbWISHIntegrationHeader.tbReservationHeader != null)
            {
                reservation.Header.ReservationName = tbWISHIntegrationHeader.tbReservationHeader.Reservationname;
                reservation.Header.DateCaptured = tbWISHIntegrationHeader.tbReservationHeader.DateCaptured;
                reservation.Header.Res_IDX = tbWISHIntegrationHeader.tbReservationHeader.pkReservationHeaderID;
                if (tbWISHIntegrationHeader.tbReservationHeader.tbOperator != null)
                {
                    reservation.Header.OperatorName = tbWISHIntegrationHeader.tbReservationHeader.tbOperator.Name;
                    reservation.Header.IDX_Operator = tbWISHIntegrationHeader.tbReservationHeader.fkOperatorID;
                }

                if (tbWISHIntegrationHeader.tbReservationHeader.tbReservationStatu != null)
                {
                    reservation.Header.ReservationStatus = tbWISHIntegrationHeader.tbReservationHeader.tbReservationStatu.Description;
                    reservation.Header.IDX_ResStatus = tbWISHIntegrationHeader.tbReservationHeader.fkResStatusID;
                }

                //if (tbWISHIntegrationHeader.tbReservationHeader.tset_Personnel != null)
                //{
                //    reservation.Header.OperatorAgentName = tschResHeader.tsch_ReservationHeader.tset_Personnel.Firstname + " " + tschResHeader.tsch_ReservationHeader.tset_Personnel.Surname;
                //    reservation.Header.IDX_OperatorAgent = tschResHeader.tsch_ReservationHeader.IDX_OperatorAgent ?? -1;
                //}

                if (tbWISHIntegrationHeader.tbReservationHeader.tbUser != null)
                {
                    reservation.Header.SefofaneAgentName = tbWISHIntegrationHeader.tbReservationHeader.tbUser.Firstname + " " + tbWISHIntegrationHeader.tbReservationHeader.tbUser.Surname;
                    reservation.Header.IDX_Personnel = tbWISHIntegrationHeader.tbReservationHeader.fkUserID;
                }

                reservation.Header.ReservationNote = tbWISHIntegrationHeader.tbReservationHeader.Notes;

            }


            foreach (var leg in tbWISHIntegrationHeader.tbWishIntegrationLegs)
            {
                var wishLeg = (ReservationLeg)leg.tbReservationLeg;
                reservation.Legs.Add(wishLeg);
            }

            foreach (var pax in tbWISHIntegrationHeader.tbReservationHeader.tbPassengers)
            {
                var resPax = (ReservationPax)pax;
                reservation.Passengers.Add(resPax);
            }


            if (reservation.Legs != null && reservation.Legs.Count > 0)
            {
                reservation.Header.FirstBookingDate = reservation.Legs.Min(x => x.BookingDate);
                reservation.Header.LastBookingDate = reservation.Legs.Max(x => x.BookingDate);
            }

            return reservation;
        }

        public static implicit operator Reservation(tsch_WISHIntegrationHeader tschResHeader)
        {
            var reservation = new Reservation();
            reservation.Header.BookingID = tschResHeader.WISHBookingID;
            reservation.Header.PartyGroupID = tschResHeader.WISHPGID;
            reservation.Header.PaxCount = tschResHeader.WISHPGPax;
            reservation.Header.WishResStatus = tschResHeader.WISHBookingStatus;
            reservation.Header.DepartureDate = tschResHeader.DepartureDate;
            reservation.Header.PartyGroupName = tschResHeader.WISHPGName;
            reservation.Header.WishConsultant = tschResHeader.WishConsultant;
            if (tschResHeader.tsch_ReservationHeader != null)
            {
                reservation.Header.ReservationName = tschResHeader.tsch_ReservationHeader.Reservationname;
                reservation.Header.DateCaptured = tschResHeader.tsch_ReservationHeader.DateCaptured;
                reservation.Header.Res_IDX = tschResHeader.tsch_ReservationHeader.IDX;
                if (tschResHeader.tsch_ReservationHeader.tset_Companies != null)
                {
                    reservation.Header.OperatorName = tschResHeader.tsch_ReservationHeader.tset_Companies.CompanyName;
                    reservation.Header.IDX_Operator = tschResHeader.tsch_ReservationHeader.IDX_Operators;
                }
                   
                if (tschResHeader.tsch_ReservationHeader.tlst_ResStatus != null)
                {
                    reservation.Header.ReservationStatus = tschResHeader.tsch_ReservationHeader.tlst_ResStatus.ReservationStatus;
                    reservation.Header.IDX_ResStatus = tschResHeader.tsch_ReservationHeader.IDX_ResStatus;
                }
                   
                if (tschResHeader.tsch_ReservationHeader.tset_Personnel != null)
                {
                    reservation.Header.OperatorAgentName = tschResHeader.tsch_ReservationHeader.tset_Personnel.Firstname + " " + tschResHeader.tsch_ReservationHeader.tset_Personnel.Surname;
                    reservation.Header.IDX_OperatorAgent = tschResHeader.tsch_ReservationHeader.IDX_OperatorAgent ?? -1;
                }
                   
                if (tschResHeader.tsch_ReservationHeader.tset_Personnel1 != null)
                {
                    reservation.Header.SefofaneAgentName = tschResHeader.tsch_ReservationHeader.tset_Personnel1.Firstname + " " + tschResHeader.tsch_ReservationHeader.tset_Personnel1.Surname;
                    reservation.Header.IDX_Personnel = tschResHeader.tsch_ReservationHeader.IDX_Personnel;
                }

                reservation.Header.ReservationNote = tschResHeader.tsch_ReservationHeader.Notes;
                    
            }


            foreach (var leg in tschResHeader.tsch_WishIntegrationLeg)
            {
                var wishLeg = (ReservationLeg)leg.tsch_ReservationLegs;
                reservation.Legs.Add(wishLeg);
            }

            foreach (var pax in tschResHeader.tsch_ReservationHeader.tsch_Passengers)
            {
                var resPax = (ReservationPax)pax;
                reservation.Passengers.Add(resPax);
            }

          
            if (reservation.Legs != null && reservation.Legs.Count > 0)
            {
                reservation.Header.FirstBookingDate = reservation.Legs.Min(x=>x.BookingDate);
                reservation.Header.LastBookingDate = reservation.Legs.Max(x => x.BookingDate);
            }

            return reservation;

        }
    }
}
