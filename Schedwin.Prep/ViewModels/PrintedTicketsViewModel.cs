using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class PrintedTicketsViewModel
    {
        public  PrintedTicketControlViewModel[] TicketModels { get; set; }

        public List<PassengerTicket> TicketList { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling((double)TicketList.Count() / 3.0));
            }
        }

        public PrintedTicketsViewModel()
        {
            TicketModels = new PrintedTicketControlViewModel[3];
            TicketList = new List<PassengerTicket>();
        }

        public void  Init(PrintedTicketControlViewModel ticket1ViewModel, PrintedTicketControlViewModel ticket2ViewModel, PrintedTicketControlViewModel ticket3ZViewModel)
        {
           

            TicketModels[0] = ticket1ViewModel;
            TicketModels[1] = ticket2ViewModel;
            TicketModels[2] = ticket3ZViewModel;
        }

        public void Refresh(List<PassengerTicket> tickets)
        {
            TicketList.Clear();
            TicketList.AddRange(tickets);
            FirstPage();
        }

        public void NextPage()
        {
            if (CurrentPage <TotalPages)
                RefreshPage(++CurrentPage);
        }

        public void PreviousPage()
        {
            if (CurrentPage >1)
                RefreshPage(--CurrentPage);
        }

        public void FirstPage()
        {
            RefreshPage(1);
            CurrentPage = 1;
        }

        public void LastPage()
        {
            RefreshPage(TotalPages);
            CurrentPage = TotalPages;
        }

        public void RefreshPage(int pageNo)
        {
            int startIndex =3* (pageNo - 1) ;
            int endIndex = (3 * pageNo);

            if (endIndex > TicketList.Count)
                endIndex = TicketList.Count ;

            var sublist = TicketList.GetRange(startIndex, endIndex-startIndex);
            DisplayPage(sublist);
        }

        private void DisplayPage(List<PassengerTicket> tickets)
        {
            int index = 0;
            TicketModels[1].IsVisible = false;
            TicketModels[2].IsVisible = false;

            foreach (var ticket in tickets)
            {
                TicketModels[index].IsVisible = true;
                TicketModels[index++].Ticket = ticket;

            }
        }
    }
}
