namespace Schedwin.Reports.Tickets
{ 
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

	/// <summary>
	/// Summary description for PilotDutyHours
	/// </summary>
    public partial class PassengerTickets : Telerik.Reporting.Report
    {

        public PassengerTickets()
		{
			//
			// Required for telerik Reporting designer support
			//
			InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //          
             
        }

        private void table1_NeedDataSource(object sender, EventArgs e)
        {
  
        }
    }
}