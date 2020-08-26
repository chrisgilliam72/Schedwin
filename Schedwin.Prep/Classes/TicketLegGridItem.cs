using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
   public  class TicketLegGridItem

    {
        public int ResIDX { get; set; }

        public int PaxIDX { get; set; }
        public String PaxName { get; set; }
        public String GroupName { get; set; }
        public String Date { get; set; }

        public String From { get; set; }

        public String To { get; set; }

        public String IssuePlace { get; set; }

        public String IssueDate { get; set; }

        public String IssuedBy { get; set; }

        public bool TicketPrinted { get; set; }

        

        public TicketLegGridItem()
        {
          
        }


    }
}
