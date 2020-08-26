using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class PassengerTicket
    {
        public static Random TicketNo = new Random();

        public int IDX_Ticket { get; set; }
        public int IDX_ResHDR { get; set; }
        public int IDX_Pax { get; set; }
        public String ReservationName {  get; set; }
        public String FullName { get; set; }
        public String ReferenceNumber { get; set; }
        public String IssueDate { get; set; }
        public String IssuePlace { get; set; }
       public String Issuer { get; set; }

        public bool TicketPrinted
        {
            get
            {
                return !String.IsNullOrEmpty(ReferenceNumber);
            }
        }

        public  static String GenerateReferenceNumber()
        {
            String strRef = (DateTime.Now.Year).ToString() + (DateTime.Now.Month).ToString() + (DateTime.Now.Day).ToString()+
                             (DateTime.Now.Hour).ToString()+(DateTime.Now.Minute).ToString()+ (DateTime.Now.Second).ToString();
            
            strRef += TicketNo.Next(1, 999); 
            return strRef;
        }

        public List<PassengerTicketLeg> Legs { get; set; }

        public PassengerTicket()
        {
            Legs = new List<PassengerTicketLeg>();
        }


        public static explicit operator PassengerTicket (TicketLegGridItem legGridItem)
        {
            var paxTicket = new PassengerTicket();
            paxTicket.ReservationName = legGridItem.GroupName;
            paxTicket.FullName = legGridItem.PaxName;
            paxTicket.Issuer = legGridItem.IssuedBy;
            paxTicket.IssueDate = legGridItem.IssueDate;
            paxTicket.IDX_Pax = legGridItem.PaxIDX;
            paxTicket.IDX_ResHDR = legGridItem.ResIDX;
            var leg = new PassengerTicketLeg();
            leg.Date = legGridItem.Date;
            leg.From = legGridItem.From;
            leg.To = legGridItem.To;

            paxTicket.Legs.Add(leg);

            return paxTicket;
        }

        public static explicit operator tset_PassengerTicketHistory(PassengerTicket passengerTicket)
        {


            var tsetPaxHistory = new tset_PassengerTicketHistory();
            tsetPaxHistory.IDX_Pax = passengerTicket.IDX_Pax;
            tsetPaxHistory.IDX_ResHDR = passengerTicket.IDX_ResHDR;
            tsetPaxHistory.TicketRef = passengerTicket.ReferenceNumber;
            tsetPaxHistory.IssuedBy = passengerTicket.Issuer;
            tsetPaxHistory.IssuePlace = passengerTicket.IssuePlace;
            tsetPaxHistory.IssueDate = DateTime.Today;
            return tsetPaxHistory;
        }



        public static explicit operator PassengerTicket(tset_PassengerTicketHistory dbPassengerTicketHist)
        {


            var passengerTicket = new PassengerTicket();
            passengerTicket.IDX_Ticket = dbPassengerTicketHist.IDX;
            passengerTicket.IDX_Pax = dbPassengerTicketHist.IDX_Pax;
            passengerTicket.IDX_ResHDR = dbPassengerTicketHist.IDX_ResHDR;

            return passengerTicket;
        }



        public static async Task UpdatePrintedTickets(List<PassengerTicket> tickets, String userName, String serverName, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(serverName, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {

                var resIDX = tickets.FirstOrDefault().IDX_ResHDR;
                var lstTicketHists = tickets.Select(x => (tset_PassengerTicketHistory)x).ToList();
                foreach (var ticketHist in lstTicketHists)
                {
                
                    ticketHist.LOGUSERID = userName;
                    ticketHist.LOGDATETIMESTAMP = DateTime.Now;

                    ctx.tset_PassengerTicketHistory.Add(ticketHist);
                }

                await ctx.SaveChangesAsync();
           
            }

         
        }
    }
}
