namespace Schedwin.Reports.Misc
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for BusinessKMSReport.
    /// </summary>
    public partial class BusinessKMSReport : Telerik.Reporting.Report
    {
        public BusinessKMSReport()
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
            Telerik.Reporting.Processing.Table table = (Telerik.Reporting.Processing.Table)sender;
            Telerik.Reporting.Processing.Report report = table.Report;

            var datasource = sqlDataSource1;
            datasource.ConnectionString = report.Parameters["ConnectionString"].Value.ToString();
            table.DataSource = datasource;
        }
    }
}