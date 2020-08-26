using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class PassengerTicketInfo : ViewModelBase
    {
        private int idxTicket;
        public int IDX_Ticket
        {
            get
            {
                return idxTicket;
            }
            set
            {
                idxTicket = value;
                NotifyPropertyChanged("ReferenceNumber");
                NotifyPropertyChanged("TicketPrinted");
            }
        }
        public int IDX_ResHDR { get; set; }
        public int IDX_Pax { get; set; }
        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public String ReferenceNumber
        {
            get
            {
                if (IDX_Ticket < 1)
                    return "";
                else
                {
                    String tmpStr = IDX_Ticket.ToString();
                    tmpStr = tmpStr.PadLeft(10, '0');
                    return tmpStr;
                }

            }
        }


        public bool TicketPrinted
        {
            get
            {
                return IDX_Ticket > 1;
            }
        }


        public static explicit operator PassengerTicketInfo(tset_PassengerTicketHistory dbPassengerTicketHist)
        {


            var passengerTicket = new PassengerTicketInfo();
            passengerTicket.idxTicket = dbPassengerTicketHist.IDX;
            passengerTicket.IDX_Pax = dbPassengerTicketHist.IDX_Pax;
            passengerTicket.IDX_ResHDR = dbPassengerTicketHist.IDX_ResHDR;

            return passengerTicket;
        }


        public static explicit operator tset_PassengerTicketHistory(PassengerTicketInfo passengerTicket)
        {


            var tsetPaxHistory = new tset_PassengerTicketHistory();
            tsetPaxHistory.IDX_Pax = passengerTicket.IDX_Pax;
            tsetPaxHistory.IDX_ResHDR = passengerTicket.IDX_ResHDR;
            tsetPaxHistory.TicketRef = passengerTicket.ReferenceNumber;
            return tsetPaxHistory;
        }

    }
}
