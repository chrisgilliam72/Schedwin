namespace Schedwin.Reports.Pilot
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for PilotDetailedSummary.
    /// </summary>
    public partial class PilotDetailedSummary : Telerik.Reporting.Report
    {
        public PilotDetailedSummary()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        private void PilotDetailedSummary_NeedDataSource(object sender, EventArgs e)
        {
            Telerik.Reporting.Processing.Report report = (Telerik.Reporting.Processing.Report)sender;

            var datasource = sqlDataSource1;
            datasource.ConnectionString = report.Parameters["ConnectionString"].Value.ToString();
            report.DataSource = datasource;
        }
    }
}