using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class PassengerTicketInfo
    {
        public int IDX { get; set; }
        public String FirstName { get; set; }

        public String LastName { get; set; }
    }
    public class LegTicketInfo
    {
        public DateTime Date { get; set; }

        public DateTime ETD { get; set; }

        public DateTime ETA { get; set; }

        public int IDX_FromAP { get; set; }

        public String FromAP { get; set; }

        public int IDX_ToAP { get; set; }

        public String ToAP { get; set; }

        public String Pilot { get; set; }

        public int IDX_Pilot { get; set; }

        public String Aircraft { get; set; }

        public int IDX_Aircraft { get; set; }

        public static explicit operator LegTicketInfo (tsch_LegsRes tschLegRes)
        {
            var legTicketInfo = new LegTicketInfo();
            legTicketInfo.Date = tschLegRes.tsch_ReservationLegs.BookingDate;
            legTicketInfo.IDX_ToAP = tschLegRes.tsch_Legs.ToAP;
            legTicketInfo.ETA = new DateTime(legTicketInfo.Date.Year, legTicketInfo.Date.Month, legTicketInfo.Date.Day, tschLegRes.tsch_Legs.ETA.Hour, tschLegRes.tsch_Legs.ETA.Minute,00);
            legTicketInfo.ETD = new DateTime(legTicketInfo.Date.Year, legTicketInfo.Date.Month, legTicketInfo.Date.Day, tschLegRes.tsch_Legs.ETD.Hour, tschLegRes.tsch_Legs.ETD.Minute, 00);

            legTicketInfo.IDX_FromAP = tschLegRes.tsch_Legs.FromAP;
            legTicketInfo.IDX_Pilot = tschLegRes.tsch_AC_Pilot.IDX_Pilots;
            legTicketInfo.IDX_Aircraft = tschLegRes.tsch_AC_Pilot.IDX_ACDetails;
            return legTicketInfo;

        }
    }


   public class TicketInfo
    {
        public int IDX_Group { get; set; }

        public String GroupName { get; set; }


        public String IssuePlace { get; set; }

        public DateTime IssueDate { get; set; }

        public String IssuedBy { get; set; }

        public String ReferenceNumber { get; set; }

        public RangeObservableCollection<LegTicketInfo> Legs { get; set; }

        public RangeObservableCollection<PassengerTicketInfo> Pax { get; set; }

        TicketInfo()
        {
            Legs = new RangeObservableCollection<LegTicketInfo>();

            Pax = new RangeObservableCollection<PassengerTicketInfo>();

        }

        public static explicit operator TicketInfo(tsch_ReservationHeader resHeader)
        {
            DateTime now = DateTime.Now;
            var ticketinfo = new TicketInfo();
            ticketinfo.IDX_Group = resHeader.IDX;
            ticketinfo.GroupName = resHeader.Reservationname;
            ticketinfo.IssueDate = DateTime.Today;
            ticketinfo.ReferenceNumber = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString();


            foreach (var tschLeg in resHeader.tsch_ReservationLegs)
            {
                var tmpLst = new List<LegTicketInfo>();
                foreach (var leg in tschLeg.tsch_LegsRes)
                {
                    var ticketLeg = (LegTicketInfo)leg;
                    tmpLst.Add(ticketLeg);
                }

                ticketinfo.Legs.AddRange(tmpLst.OrderBy(x => x.Date).ThenBy(x => x.ETD));
            }


            //{
            //    var ticketLegInfo = new LegTicketInfo();
            //    ticketLegInfo.Date = tschLeg.BookingDate;
            //    ticketLegInfo.IDX_ToAP = tschLeg.ToAp;
            //    ticketLegInfo.IDX_FromAP = tschLeg.FromAp;
            //    ticketinfo.Legs.Add(ticketLegInfo);

            //}

            foreach (var tschPax in resHeader.tsch_Passengers)
            {
                var paxInfo = new PassengerTicketInfo();
                paxInfo.IDX = tschPax.IDX;
                paxInfo.FirstName = tschPax.FirstName;
                paxInfo.LastName = tschPax.Surname;
                ticketinfo.Pax.Add(paxInfo);
            }  
            return ticketinfo;
        }

        public static async Task<List<TicketInfo>> GetTicketList (DateTime flightDate, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var resIDS = await ctx.tsch_ReservationLegs.Where(x => x.BookingDate == flightDate &&
                                                                    x.TicketPrinted.HasValue &&
                                                                    x.TicketPrinted.Value == false &&
                                                                    x.Cancelled == false).Select(x => x.IDX_ResHeader).ToListAsync();

                //var legsRes = await ctx.tsch_LegsRes.Include("tsch_reservationLegs").
                //                         Include("tsch_legs").
                //                          Include("tsch_legs.tsch_AC_Pilot").
                //                        Include("tsch_reservationLegs.tsch_ReservationHeader").
                //                          Include("tsch_reservationLegs.tsch_ReservationHeader.tsch_Passengers").
                //                         Where(x=>x.tsch_ReservationLegs.BookingDate==flightDate).ToListAsync();


                //var reservations = legsRes.Select(x => x.tsch_ReservationLegs.tsch_ReservationHeader).GroupBy(x=>x.IDX).Select(x=>x.First()).ToList();



                var reservations = await ctx.tsch_ReservationHeader.Include("tsch_ReservationLegs")
                                                                     .Include("tsch_Passengers")
                                                                     .Include("tsch_ReservationLegs.tsch_LegsRes")
                                                                     .Include("tsch_ReservationLegs.tsch_LegsRes.tsch_Legs")
                                                                     .Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot")
                                                                     //.Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot")
                                                                     .Where(x => resIDS.Contains(x.IDX)).ToListAsync();

                var lstTickets = reservations.Select(x => (TicketInfo)x).ToList();

     

                return lstTickets;

            }
        }

    }
}
