using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Reports.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
   public class TicketsReservationViewModel :ViewModelBase
    {

        public TicketsView View { get; set; }
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
        public String CompanyName { get; set; }

        public String CurrentUser { get; set; }


        public String Region { get; set; }

        public RangeObservableCollection<PassengerTicket> TicketList { get; set; }

        public TicketsViewModel TicketViewModel { get; set; }


        public TicketsReservationViewModel()
        {
            TicketList = new RangeObservableCollection<PassengerTicket>();
        }


        public async Task Refresh()
        {
            await TicketViewModel.Refresh();
        }

        public void Refresh(List<PassengerTicket> newList)
        {
            TicketList.Clear();
            TicketList.AddRange(newList);
            NotifyPropertyChanged("TicketList");
        }

        public async Task<bool> PrintPaxTicket(List<PassengerTicket> SelTickets, bool printTicket)
        {
            try
            {
                var printedTicketView = new PrintedTicketsView();
                var prntWindow = new PrintWindowContainer();
                prntWindow.PrintedControl = printedTicketView;
                
                var viewModel = printedTicketView.DataContext as PrintedTicketsViewModel;
                if (printTicket)
                {
                    var unprintedTickets = SelTickets.Where(x => x.TicketPrinted == false).ToList();
                    foreach (var ticket in unprintedTickets)
                    {
                        ticket.IssueDate = DateTime.Today.ToShortDateString();
                        ticket.Issuer = CurrentUser;
                        ticket.IssuePlace = CompanyName;
                        ticket.ReferenceNumber = PassengerTicket.GenerateReferenceNumber();
                    }
                 
                }
                

                viewModel.Refresh(SelTickets);

                prntWindow.WindowTitle = "Print tickets";
                prntWindow.Owner = View;
                prntWindow.Show();

                if (printTicket)
                {
                    var ResHdrIds = SelTickets.Select(x => x.IDX_ResHDR).ToList();
                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                        await PassengerTicket.UpdatePrintedTickets(SelTickets, CompanyName, Server, Database);
                        await ReservationTicketInfo.UpdateReservationTicketPrinted(ResHdrIds, Server, Database);


                    }

                  
                }


                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Initialization error :" + Environment.NewLine + message);
                return false;
            }
          
        }



    }
}
