using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class PrintedTicketControlViewModel : ViewModelBase
    {

        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }
        public String GroupName
        {
            get
            {
               
                return Ticket!=null ? Ticket.ReservationName : "";
            }
        }


        public String PaxName
        {
            get
            {
                return Ticket != null ?  Ticket.FullName : "";
            }
        }


        public String TicketRef
        {
            get
            {
                return Ticket != null ? Ticket.ReferenceNumber : "";
            }
        }


        public String Issuer
        {
            get
            {
                return Ticket != null ? Ticket.Issuer : "";
            }
        }

        public String DateOfIssue
        {
            get
            {
                return Ticket != null ? Ticket.IssueDate : "";
            }
        }

        public String PlaceOfIssue
        {
            get
            {
                return Ticket != null ? Ticket.IssuePlace : "";
            }
        }

        public RangeObservableCollection<PassengerTicketLeg> Legs
        {
           get
            {
                var legsList = new RangeObservableCollection<PassengerTicketLeg>();
                if (Ticket!=null)
                    legsList.AddRange(Ticket.Legs.OrderBy(x=>x.Date).Take(7));
                return legsList;
            }
        }



        private PassengerTicket _ticket;
        public  PassengerTicket  Ticket
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
                NotifyPropertyChanged("Legs");
                NotifyPropertyChanged("IsVisible");
                NotifyPropertyChanged("GroupName");
                NotifyPropertyChanged("PaxName");
                NotifyPropertyChanged("PlaceOfIssue");
                NotifyPropertyChanged("DateOfIssue");
                NotifyPropertyChanged("Issuer");
                NotifyPropertyChanged("TicketRef");
            }
        }

       public  PrintedTicketControlViewModel()
        {
            IsVisible = false;
        }
    }
}
