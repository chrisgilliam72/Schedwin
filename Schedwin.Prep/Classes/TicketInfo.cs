using Schedwin.Common;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{


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

        public int IDX_LegRes { get; set; }

        public bool IsSelected { get; set; }

        public TicketInfo TicketReservation { get; set; }



        LegTicketInfo()
        {
    
        }

        public static explicit operator LegTicketInfo (tsch_LegsRes tschLegRes)
        {
            var legTicketInfo = new LegTicketInfo();
            legTicketInfo.Date = tschLegRes.tsch_ReservationLegs.BookingDate;
            legTicketInfo.IDX_ToAP = tschLegRes.tsch_Legs.ToAP;
            legTicketInfo.ETA = new DateTime(legTicketInfo.Date.Year, legTicketInfo.Date.Month, legTicketInfo.Date.Day, tschLegRes.tsch_Legs.ETA.Hour, tschLegRes.tsch_Legs.ETA.Minute,00);
            legTicketInfo.ETD = new DateTime(legTicketInfo.Date.Year, legTicketInfo.Date.Month, legTicketInfo.Date.Day, tschLegRes.tsch_Legs.ETD.Hour, tschLegRes.tsch_Legs.ETD.Minute, 00);

            legTicketInfo.IDX_LegRes = tschLegRes.IDX;
            legTicketInfo.IDX_FromAP = tschLegRes.tsch_Legs.FromAP;
            if (tschLegRes.tsch_AC_Pilot.IDX_Pilots.HasValue)
                    legTicketInfo.IDX_Pilot = tschLegRes.tsch_AC_Pilot.IDX_Pilots.Value;
            legTicketInfo.IDX_Aircraft = tschLegRes.tsch_AC_Pilot.IDX_ACDetails;
            return legTicketInfo;

        }

        public static explicit operator LegTicketInfo (tsch_ReservationLegs legs)
        {
            var legTicketInfo = new LegTicketInfo();
            legTicketInfo.Date = legs.BookingDate;
            if (legs.tset_Airports!=null)
            {
                legTicketInfo.IDX_ToAP = legs.tset_Airports.IDX;
                legTicketInfo.ToAP = legs.tset_Airports.Airport;
            }
            if (legs.tset_Airports1!=null)
            {
                legTicketInfo.IDX_FromAP = legs.tset_Airports1.IDX;
                legTicketInfo.FromAP = legs.tset_Airports.Airport;
            }

            return legTicketInfo;
        }
    }


    public class TicketInfo : ViewModelBase
    {
        public int IDX_Group { get; set; }

        public String GroupName { get; set; }


        //public String ReferenceNumber { get; set; }

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

        private PassengerTicketInfo selectedPassenger;
        public PassengerTicketInfo SelectedPassenger
        {
            get
            {
                return selectedPassenger;
            }
            set
            {
                selectedPassenger = value;
            }
        }

        public RangeObservableCollection<LegTicketInfo> Legs { get; set; }

        public RangeObservableCollection<PassengerTicketInfo> Pax { get; set; }



        TicketInfo()
        {
            Legs = new RangeObservableCollection<LegTicketInfo>();
            Pax = new RangeObservableCollection<PassengerTicketInfo>();

        } 

        public void FilterLegs(DateTime date)
        {
            var tmpList = Legs.Where(x => x.Date >= date).ToList();
            Legs.Clear();
            Legs.AddRange(tmpList);
        }

        public static explicit operator TicketInfo(tsch_ReservationHeader resHeader)
        {
            DateTime now = DateTime.Now;
            var ticketinfo = new TicketInfo();
            ticketinfo.IDX_Group = resHeader.IDX;
            ticketinfo.GroupName = resHeader.Reservationname;
            //ticketinfo.IssueDate = DateTime.Today;
            //ticketinfo.ReferenceNumber = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString() + now.Millisecond.ToString();
            ticketinfo.TicketPrinted = resHeader.TicketPrinted;

            foreach (var tschPax in resHeader.tsch_Passengers)
            {
                var paxInfo = new PassengerTicketInfo();
                paxInfo.IDX_Pax = tschPax.IDX;
                paxInfo.IDX_ResHDR = resHeader.IDX;
                paxInfo.FirstName = tschPax.FirstName;
                paxInfo.LastName = tschPax.Surname;
                if (tschPax.tset_PassengerTicketHistory != null && tschPax.tset_PassengerTicketHistory.Count > 0)
                    paxInfo.IDX_Ticket = tschPax.tset_PassengerTicketHistory.FirstOrDefault().IDX;

                ticketinfo.Pax.Add(paxInfo);
            }


            //foreach (var tschLeg in resHeader.tsch_ReservationLegs)
            //{
            //    var tmpLst = new List<LegTicketInfo>();
            //    foreach (var leg in tschLeg.tsch_LegsRes)
            //    {
            //        var ticketLeg = (LegTicketInfo)leg;
          
            //        tmpLst.Add(ticketLeg);
            //    }

            //    ticketinfo.Legs.AddRange(tmpLst.OrderBy(x => x.Date).ThenBy(x => x.ETD));
            //}


     

            //{
            //    var ticketLegInfo = new LegTicketInfo();
            //    ticketLegInfo.Date = tschLeg.BookingDate;
            //    ticketLegInfo.IDX_ToAP = tschLeg.ToAp;
            //    ticketLegInfo.IDX_FromAP = tschLeg.FromAp;
            //    ticketinfo.Legs.Add(ticketLegInfo);

            //}

   
            return ticketinfo;
        }


        public static async Task<int> UpdatePrintedTicket(PassengerTicketInfo ticket, String userName, String companyName, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {

                var ticketHist = (tset_PassengerTicketHistory)ticket;

                ticketHist.IssuedBy = userName;
                ticketHist.IssueDate = DateTime.Today;
                ticketHist.IssuePlace = companyName;
                ticketHist.LOGUSERID = userName;
                ticketHist.LOGDATETIMESTAMP = DateTime.Now;
                ctx.tset_PassengerTicketHistory.Add(ticketHist);
              
                await ctx.SaveChangesAsync();

                return ticketHist.IDX;
            }
        }



        public static List<LegTicketInfo> GetTicketLegs(int ResIDX, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
                var tschLegs= ctx.tsch_ReservationLegs.Include("tset_airports").Include("tset_airports1").Where(x => x.IDX_ResHeader == ResIDX).OrderBy(x=>x.BookingDate).ToList();
                if (tschLegs!=null)
                {
                    var lstLegs = tschLegs.Select(x => (LegTicketInfo)x).ToList();
                    return lstLegs;
                }

                return null;
            }
        }





    }
}
