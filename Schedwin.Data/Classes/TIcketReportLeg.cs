using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class TicketReportLeg
    {
        public DateTime Date { get; set; }
        public int IDX_FromAP { get; set; }

        public String FromAP { get; set; }

        public int IDX_ToAP { get; set; }

        public String ToAP { get; set; }


        public static explicit operator TicketReportLeg(tsch_ReservationLegs legs)
        {
            var legTicketInfo = new TicketReportLeg();
            legTicketInfo.Date = legs.BookingDate;
            if (legs.tset_Airports != null)
            {
                legTicketInfo.IDX_ToAP = legs.tset_Airports1.IDX;
                legTicketInfo.ToAP = legs.tset_Airports1.Airport;
            }
            if (legs.tset_Airports1 != null)
            {
                legTicketInfo.IDX_FromAP = legs.tset_Airports.IDX;
                legTicketInfo.FromAP = legs.tset_Airports.Airport;
            }

            return legTicketInfo;
        }


        public static List<TicketReportLeg> GetTicketLegs(int ResIDX, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tschLegs = ctx.tsch_ReservationLegs.Include("tset_airports").Include("tset_airports1").Where(x => x.IDX_ResHeader == ResIDX).OrderBy(x=>x.BookingDate).ToList();
                if (tschLegs != null)
                {
                    var lstLegs = tschLegs.Select(x => (TicketReportLeg)x).ToList();
                    return lstLegs;
                }

                return null;
            }
        }
    }
}
