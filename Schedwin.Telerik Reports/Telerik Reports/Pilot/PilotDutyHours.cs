namespace Schedwin.Reports
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
    public partial class PilotDutyHours : Telerik.Reporting.Report
	{
		public PilotDutyHours()
		{
			//
			// Required for telerik Reporting designer support
			//
			InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //          
             
        }
        
        public void UpdateDataSource(String connectionString)
        {

        }

        private void PilotDutyHours_NeedDataSource(object sender, EventArgs e)
        {
            Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;

            var datasource = sqlDataSource1;
            datasource.ConnectionString = report.Parameters["ConnectionString"].Value.ToString();
            report.DataSource = datasource;
            //datasource.ConnectionString = "Reporting.SefofaneBots1";
        }
    }
}