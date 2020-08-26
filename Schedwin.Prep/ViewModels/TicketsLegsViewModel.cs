using Schedwin.Common;
using Schedwin.Reports.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class TicketsLegsViewModel : ViewModelBase 
    {
        public TicketsView View { get; set; }
        public String CompanyName { get; set; }
        public String Region { get; set; }

        public String CurrentUser { get; set; }

        public TicketsViewModel TicketViewModel { get; set; }

        private bool _show;
        public bool Show
        {
            get
            {
                return _show;
            }
            set
            {
                _show = value;
                NotifyPropertyChanged("Show");
            }
        }

       public RangeObservableCollection<TicketLegGridItem> LegList { get; set; }

        public TicketsLegsViewModel()
        {
            LegList = new RangeObservableCollection<TicketLegGridItem>();
        }

        public void Refresh(List<PassengerTicket> ticketList, DateTime filterDate)
        {
            var stringDate = filterDate.ToShortDateString();
            LegList.Clear();
            foreach (var ticket in ticketList)
            {
                foreach (var leg in ticket.Legs)
                {
                   
                    if (leg.Date==stringDate)
                    {
                        var gridItem = new TicketLegGridItem();
                        gridItem.Date = leg.Date;
                        gridItem.ResIDX = ticket.IDX_ResHDR;
                        gridItem.PaxIDX = ticket.IDX_Pax;
                        gridItem.GroupName = ticket.ReservationName;
                        gridItem.PaxName = ticket.FullName;
                        gridItem.From = leg.From;
                        gridItem.To = leg.To;
                        gridItem.IssueDate = ticket.IssueDate;
                        gridItem.IssuedBy = ticket.Issuer;
                        gridItem.IssuePlace = ticket.IssuePlace;

                        gridItem.TicketPrinted = ticket.TicketPrinted;
                        LegList.Add(gridItem);
                    }
                }
            }
                
            NotifyPropertyChanged("LegList");
        }

        public async Task<bool> PrintTickets(List<TicketLegGridItem> listTickets,bool printTicket)
        {
            try
            {
                var printedTicketView = new PrintedTicketsView();
                var prntWindow = new PrintWindowContainer();
                prntWindow.PrintedControl = printedTicketView;
                var viewModel = printedTicketView.DataContext as PrintedTicketsViewModel;

                var printTickets = listTickets.Select(x => (PassengerTicket)x).ToList();

        
                var unprintedTickets = printTickets.Where(x => x.TicketPrinted == false).ToList();
                foreach (var ticket in unprintedTickets)
                {
                    ticket.IssueDate = DateTime.Today.ToShortDateString();
                    ticket.Issuer = CurrentUser;
                    ticket.IssuePlace = CompanyName;
                    ticket.ReferenceNumber = PassengerTicket.GenerateReferenceNumber();
                }

                viewModel.Refresh(unprintedTickets);

                prntWindow.WindowTitle = "Print tickets";
                prntWindow.Owner = View;
                prntWindow.Show();


                if (printTicket)
                {
                    var ResHdrIds = printTickets.Select(x => x.IDX_ResHDR).ToList();
                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                        await PassengerTicket.UpdatePrintedTickets(unprintedTickets, CurrentUser, Server,Database);
                        await ReservationTicketInfo.UpdateReservationTicketPrinted(ResHdrIds, Server, Database);
                        await TicketViewModel.Refresh();

                    }
                }





            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Initialization error :" + Environment.NewLine + message);
                return false;
            }
            return true;
        }
    }
}
