using Schedwin.Common;
using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class ReservationTicketInfo : ViewModelBase
    {
        public int IDX_Group { get; set; }

        public String GroupName { get; set; }


        private bool _ticketPrinted;
        public bool TicketPrinted
        {
            get
            {
                return _ticketPrinted;
            }
            set
            {
                _ticketPrinted = value;
                NotifyPropertyChanged("TicketPrinted");
            }
        }


        public RangeObservableCollection<PassengerTicket> Pax { get; set; }

        public RangeObservableCollection<ReservationTicketLeg> Legs { get; set; }


        public ReservationTicketInfo()
        {
            Pax = new RangeObservableCollection<PassengerTicket>();
            Legs = new RangeObservableCollection<ReservationTicketLeg>();
        }


        public static explicit operator ReservationTicketInfo(tsch_ReservationHeader resHeader)
        {
            DateTime now = DateTime.Now;
            var ticketinfo = new ReservationTicketInfo();
            ticketinfo.IDX_Group = resHeader.IDX;
            ticketinfo.GroupName = resHeader.Reservationname;

            ticketinfo.TicketPrinted = resHeader.TicketPrinted;

            foreach (var tschPax in resHeader.tsch_Passengers)
            {
                var paxInfo = new PassengerTicket();
                paxInfo.IDX_Pax = tschPax.IDX;
                paxInfo.IDX_ResHDR = resHeader.IDX;
                paxInfo.FullName = tschPax.FirstName+" "+ tschPax.Surname;
                paxInfo.ReservationName = ticketinfo.GroupName;

                if (tschPax.tset_PassengerTicketHistory != null && tschPax.tset_PassengerTicketHistory.Count > 0)
                {
                    var ticketHist = tschPax.tset_PassengerTicketHistory.FirstOrDefault();
                    paxInfo.ReferenceNumber = ticketHist.TicketRef;
                    paxInfo.IssueDate = ticketHist.IssueDate.ToShortDateString() ;
                    paxInfo.Issuer = ticketHist.IssuedBy;
                    paxInfo.IssuePlace = ticketHist.IssuePlace;
                }

                var orderedLegs = resHeader.tsch_ReservationLegs.OrderByDescending(x => x.BookingDate).ToList();
                foreach (var tschLeg in orderedLegs)
                {
                    if (!tschLeg.Cancelled.HasValue || (tschLeg.Cancelled.HasValue && tschLeg.Cancelled.Value==false))
                    {
                        var ticketLeg = (PassengerTicketLeg)tschLeg;
                        paxInfo.Legs.Add(ticketLeg);
                    }

                }

                ticketinfo.Pax.Add(paxInfo);
            }

            return ticketinfo;
        }


        public static async Task UpdateReservationTicketPrinted(List<int> listResIDX, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var rsHdrs = await ctx.tsch_ReservationHeader.Where(x => listResIDX.Contains(x.IDX)).ToListAsync();
                rsHdrs.ForEach(x => x.TicketPrinted = true);
                await ctx.SaveChangesAsync();
            }
        }

        public static async Task<List<ReservationTicketInfo>> GetTicketList(DateTime flightDate, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var resIDS = await ctx.tsch_ReservationLegs.Where(x => x.BookingDate == flightDate &&
                                                                    x.Cancelled == false).Select(x => x.IDX_ResHeader).ToListAsync();


                var reservations = await ctx.tsch_ReservationHeader.Include("tsch_ReservationLegs")
                                                                    .Include("tsch_ReservationLegs.tset_Airports")
                                                                     .Include("tsch_ReservationLegs.tset_Airports1")
                                                                     .Include("tsch_Passengers")
                                                                       .Include("tsch_Passengers.tset_PassengerTicketHistory")
                                                                     .Where(x => resIDS.Contains(x.IDX)).ToListAsync();

                var lstTickets = reservations.Select(x => (ReservationTicketInfo)x).ToList();
                //lstTickets.ForEach(x => x.FilterLegs(flightDate));

                return lstTickets;

            }
        }
    }
}
