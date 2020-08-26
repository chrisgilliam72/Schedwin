using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class ReservationHeader
    {
     
        public int IDX_Country { get; set; }
        public int Res_IDX { get; set; }
        public String ReservationName { get; set; }

        public DateTime DepartureDate { get; set; }
        public String PartyGroupName { get; set; }

        public String WishConsultant { get; set; }
        public int BookingID { get; set; }
        public int PartyGroupID { get; set; }
        public DateTime DateCaptured { get; set; }
        public int PaxCount { get; set; }
        public int IDX_Operator { get; set; }

        public String OperatorName { get; set; }

        public int? IDX_OperatorAgent { get; set; }

        public String OperatorAgentName { get; set; }

        public int IDX_Personnel { get; set; }

        public String SefofaneAgentName { get; set; }

        public int IDX_ResStatus { get; set; }

        public String ReservationStatus { get; set; }

        public String WishResStatus { get; set; }
        public bool Active { get; set; }

        public bool TicketPrinted { get; set; }

        public bool TicketRequired { get; set; }

        public int IDX_ResType { get; set; }

        public String ReservationType { get; set; }

        public String ReservationNote { get; set; }

        public String CurrencyID { get; set; }

        public bool IsSplit { get; set; }

        public bool IsMaster { get; set; }
        public String TourPlanRef { get; set; }

        public DateTime FirstBookingDate { get; set; }

        public DateTime LastBookingDate { get; set; }


        public ReservationHeader()
        {

        }

        public ReservationHeader(ReservationHeader Header)
        {
            Res_IDX = Header.Res_IDX;
            ReservationName = Header.ReservationName;
            DepartureDate = Header.DepartureDate;
            PartyGroupName = Header.PartyGroupName;
            WishConsultant = Header.WishConsultant;
            BookingID = Header.BookingID;
            PartyGroupID = Header.PartyGroupID;
            DateCaptured = Header.DateCaptured;
            PaxCount = Header.PaxCount;
            IDX_Operator = Header.IDX_Operator;
            OperatorName = Header.OperatorName;
            IDX_OperatorAgent = Header.IDX_OperatorAgent;
            OperatorAgentName = Header.OperatorAgentName;
            IDX_Personnel = Header.IDX_Personnel;
            SefofaneAgentName = Header.SefofaneAgentName;
            IDX_ResStatus = Header.IDX_ResStatus;
            ReservationStatus = Header.ReservationStatus;
            WishResStatus = Header.WishResStatus;
            Active = Header.Active;
            TicketPrinted = Header.TicketPrinted;
            TicketRequired = Header.TicketRequired;
            IDX_ResType = Header.IDX_ResType;
            ReservationType = Header.ReservationType;
            ReservationNote = Header.ReservationNote;
            CurrencyID = Header.CurrencyID;
            IsSplit = Header.IsSplit;
            IsMaster = Header.IsMaster;
            FirstBookingDate = Header.FirstBookingDate;
            LastBookingDate = Header.LastBookingDate;
        }

        public static explicit operator tbReservationHeader (ReservationHeader resHdr)
        {
            var tbResHeader = new tbReservationHeader();

            tbResHeader.pkReservationHeaderID = resHdr.Res_IDX;
            tbResHeader.Reservationname = resHdr.ReservationName;
            tbResHeader.Numpax = resHdr.PaxCount;
            tbResHeader.DateCaptured = resHdr.DateCaptured;
            tbResHeader.fkOperatorID = resHdr.IDX_Operator;
            tbResHeader.fkOperatorAgentID = resHdr.IDX_OperatorAgent;
            tbResHeader.fkUserID = resHdr.IDX_Personnel;
            tbResHeader.fkResStatusID = resHdr.IDX_ResStatus;
            tbResHeader.fkResTypeID = resHdr.IDX_ResType;
            tbResHeader.fkResClassID = 0;
            tbResHeader.fkCountryID = resHdr.IDX_Country;
            tbResHeader.Active = resHdr.Active;
            tbResHeader.TicketPrinted = resHdr.TicketPrinted;
            tbResHeader.TicketRequired = resHdr.TicketRequired;
            tbResHeader.Notes = resHdr.ReservationNote;
            tbResHeader.CURNCYID = resHdr.CurrencyID;
            tbResHeader.WISHID = resHdr.BookingID;
            return tbResHeader;
        }

        public static explicit operator tsch_ReservationHeader(ReservationHeader resHdr)
        {
            var tschHeader = new tsch_ReservationHeader();

            tschHeader.IDX = resHdr.Res_IDX;
            tschHeader.Reservationname = resHdr.ReservationName;
            tschHeader.Numpax = resHdr.PaxCount;
            tschHeader.DateCaptured = resHdr.DateCaptured;
            tschHeader.IDX_Operators = resHdr.IDX_Operator;
            tschHeader.IDX_OperatorAgent = resHdr.IDX_OperatorAgent;
            tschHeader.IDX_Personnel = resHdr.IDX_Personnel;
            tschHeader.IDX_ResStatus = resHdr.IDX_ResStatus;
            tschHeader.IDX_ResClass = 0;
            tschHeader.Active = resHdr.Active;
            tschHeader.TicketPrinted = resHdr.TicketPrinted;
            tschHeader.TicketRequired = resHdr.TicketRequired;
            tschHeader.IDX_ResType = resHdr.IDX_ResType;
            tschHeader.Notes = resHdr.ReservationNote;
            tschHeader.CURNCYID = resHdr.CurrencyID;
            tschHeader.WISHID = resHdr.BookingID;
            tschHeader.CoCode = "SEFO";
            return tschHeader;
        }
    }

}
