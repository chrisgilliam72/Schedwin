
using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Reporting.Crystal;
using Schedwin.Reports.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{

    public class TicketsViewModel : ViewModelBase
    {
        public TicketsView View { get; set; }
        public bool ShowLegs { get; set; }

        public bool ShowReservations { get; set; }

        public TicketsReservationViewModel TicketReservationViewModel { get; set; }
        public TicketsLegsViewModel TicketLegsViewModel { get; set; }

        public List<ReservationTicketInfo> TicketList { get; set; }

        public int CompanyID { get; set; }

        public String CompanyName { get; set; }

        public String CurrentUser { get; set; }

        public DateTime TicketDate { get; set; }

        public String Region { get; set; }

        public bool Initialized { get; set; }

        public TicketsViewModel()
        {
            TicketList = new List<ReservationTicketInfo>();
        }

        public async Task<bool> Init()
        {
            try
            {
              
                TicketReservationViewModel.CurrentUser = CurrentUser;
                TicketReservationViewModel.Server = Server;
                TicketReservationViewModel.Database = Database;
                TicketReservationViewModel.Region = Region;
                TicketReservationViewModel.View = View;
                TicketReservationViewModel.TicketViewModel = this;

                TicketLegsViewModel.View = View;
                TicketLegsViewModel.CurrentUser = CurrentUser;
                TicketLegsViewModel.Server = Server;
                TicketLegsViewModel.Database = Database;
                TicketLegsViewModel.Region = Region;
                TicketLegsViewModel.TicketViewModel = this;

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var company = await Company.GetCurrentCompany(Server, Database);
                    CompanyName = company.Description;
                }

                TicketLegsViewModel.CompanyName = CompanyName;
                TicketReservationViewModel.CompanyName = CompanyName;
                Initialized = true;
                TicketDate = DateTime.Today;
                TicketReservationViewModel.Show = true;
                ShowReservations = true;
                NotifyPropertyChanged("ShowReservations");
                NotifyPropertyChanged("TicketDate");
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

        public async Task<bool> Refresh()
        {
            if (Initialized)
            {
                TicketList.Clear();
                List<ReservationTicketInfo> ticketList = null;

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {

                    ticketList = await ReservationTicketInfo.GetTicketList(TicketDate, Server, Database);
                }


                TicketList.AddRange(ticketList);

                var paxTickets = ticketList.SelectMany(x => x.Pax).ToList();
                TicketReservationViewModel.Refresh(paxTickets);
                TicketLegsViewModel.Refresh(paxTickets, TicketDate);
            }

            return true;

        }

        public void ShowLegList()
        {
            TicketLegsViewModel.Show = true;
            TicketReservationViewModel.Show = false;
        }
        public void ShowReservationList()
        {
            TicketLegsViewModel.Show = false;
            TicketReservationViewModel.Show = true;
        }
    }
}
