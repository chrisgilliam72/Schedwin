namespace ClassLibrary1
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for MainReport.
    /// </summary>
    public partial class TicketMasterReport : Telerik.Reporting.Report
    {
        public TicketMasterReport()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();



            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public static ReportSource SetReportSource(object data)
        {
            return new InstanceReportSource() { ReportDocument = new Report1() { DataSource = data } };
        }

    }

    public class ReportInfo
    {
        public string Name { get; set; }
        public List<string> Data { get; set; }

    }

    public class ListOfReports
    {
        public List<ReportInfo> MyReports { get; set; }

        public ListOfReports()
        {
            MyReports = new List<ReportInfo>();
            MyReports.Add(new ReportInfo { Name = "Test1", Data = new List<string> { "test1", "test2" } });
            MyReports.Add(new ReportInfo { Name = "Test2", Data = new List<string> { "test1", "test2", "test3", "test4" } });
            MyReports.Add(new ReportInfo { Name = "Test3", Data = new List<string> { "test1", "test2" } });
        }
    }
}