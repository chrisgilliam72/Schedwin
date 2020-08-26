using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Reservations.Classes;

namespace Schedwin.WishIntegration.Classes
{
    public class WICharterBookingLeg
    {
        public DateTime BookingDate { get; set; }

        public bool SoleUse { get; set; }
        public String CharterType { get; set; }
        public int BookingID { get; set; }

        public int PartyGroupID { get; set; }
        public int SectorBookingID { get; set; }

        public String SectorNotes { get; set; }

        public String From { get; set; }

        public String ActualFrom { get; set; }

        public String Ex { get; set; }

        public String ExCode { get; set; }

        public String ActualEx { get; set; }

        public String To { get; set; }
        

        public String ActualTo { get; set; }

        public String For { get; set; }

        public String ForCode { get; set; }

        public String ActualFor { get; set; }

        public DateTime ETA { get; set; }

        public DateTime ETD { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsMultiLegFlight { get; set; }

        public String MultiLegFrom { get; set; }
        public String MultiLegTo { get; set; }

        public List<WICharterBookingPax> PaxLst { get; set; }
        public WICharterBookingLeg()
        {
            CharterType = "";
            SectorNotes = "";
            From = "";
            ActualFrom = "";
            ExCode = "";
            Ex = "";
            ActualEx = "";
            To = "";
            ActualTo = "";
            For = "";
            ForCode = "";
            ActualFor = "";
            IsCancelled=false;
            IsMultiLegFlight = false;
            SoleUse = false;
            PaxLst = new List<WICharterBookingPax>();
    }


        public static explicit operator WICharterBookingLeg(spSchedwinCharterRetrieval_Result spCharterRetrieval)
        {
            var charterBkgleg = new WICharterBookingLeg();
            charterBkgleg.BookingDate = spCharterRetrieval.StartDate.Value;
            charterBkgleg.CharterType=spCharterRetrieval.CHType;
            charterBkgleg.BookingID = spCharterRetrieval.pkBookingId;
            charterBkgleg.PartyGroupID = spCharterRetrieval.fkPartyGroupId.Value;
            charterBkgleg.SectorBookingID = spCharterRetrieval.pkSectorBookingId;
            charterBkgleg.SectorNotes = spCharterRetrieval.SectorNotes;
            charterBkgleg.From = spCharterRetrieval.From;
            charterBkgleg.ActualFrom = spCharterRetrieval.ActualFrom;
            charterBkgleg.Ex = spCharterRetrieval.Ex;
            charterBkgleg.ActualEx = spCharterRetrieval.ActualEx;
            charterBkgleg.To = spCharterRetrieval.To;
            charterBkgleg.ActualTo = spCharterRetrieval.ActualTo;
            charterBkgleg.For = spCharterRetrieval.For;
            charterBkgleg.ActualFor = spCharterRetrieval.ActualFor;
            //charterBkgleg.ETA = spCharterRetrieval.ETA;
            //charterBkgleg.ETD = spCharterRetrieval.ETD;
            return charterBkgleg;
        }

    }
}
