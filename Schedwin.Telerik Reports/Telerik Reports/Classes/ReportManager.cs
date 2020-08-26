using Schedwin.Reports.Tickets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data.Classes;
using Schedwin.Common;
using Schedwin.Scheduling.Classes;
using Telerik.Reporting;
using System.Windows;

namespace Schedwin.Reports.Classes
{
    public class ReportManager
    {
       public String SchedRegion { get; set; }

        
        public async void AirportArrivalLogBookReport(Window parent , String ServerName, String DatabaseName, bool Arrival )
        {
            Schedule schedule = null;
            var reportViewerWnd = new ReportBookViewerWnd();
            var reportBook = new Telerik.Reporting.ReportBook();
            var reportParamsWnd = new ReportParametersWindow();
            reportParamsWnd.Date1Label = "Select Date:";
            reportParamsWnd.List1Label = "Select Aircraft";
            reportParamsWnd.Date1 = DateTime.Today;
            reportParamsWnd.WindowTitle = "Log book report parameters"; 
            reportParamsWnd.ShowList2 = false;
            reportParamsWnd.ShowDate2 = false;

            var acList = AircraftInfo.GetAircraftList(true);
            var apList = AirstripInfo.GetAirstrips();

            if (apList == null)
            {
                return;
            }

            reportParamsWnd.List1.AddRange(acList.Select(x=> new ListboxItem {  ID= x.IDX, Description= x.Registration}).ToList());
            reportParamsWnd.ShowDialog();

           if (reportParamsWnd.DialogResult.HasValue && reportParamsWnd.DialogResult.Value)
            {

                var selectedAircraft = acList.FirstOrDefault(x => x.IDX == reportParamsWnd.SelectedItem1.ID);
                var acType = AircraftType.GetACType(selectedAircraft.IDX_AC_Type);
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    schedule = await Schedule.LoadAircraftSchedule(reportParamsWnd.Date1, selectedAircraft.IDX, ServerName, DatabaseName);
                }

                //var acPilot = schedule.list_ACPilots.FirstOrDefault();

                if (schedule.list_ACPilots.Count() > 0)
                {
                    foreach (var item in schedule.list_ACPilots)
                    {

                        var acPilot = item;

                        if (acPilot != null)
                        {
                            var pilotInfo = acPilot.IDX_Pilot_1.HasValue ? PilotInfo.GetPilotFromPersonnelID(acPilot.IDX_Pilot_1.Value) : null;

                            var legs = acPilot.Legs;

                            foreach (var leg in legs)
                            {
                                var report = new AirstripLogbook();

                                report.ReportParameters["Registration"].Value = reportParamsWnd.SelectedItem1.Description;
                                if (Arrival)
                                {
                                    report.ReportParameters["TitleText"].Value = "Arrival Information";
                                    report.ReportParameters["AP"].Value = apList.FirstOrDefault(x => x.IDX == leg.IDX_ToAP).Description;
                                    report.ReportParameters["Time"].Value = leg.ETA.ToShortTimeString();
                                }
                                else
                                {
                                    report.ReportParameters["TitleText"].Value = "Departure Information";
                                    report.ReportParameters["AP"].Value = apList.FirstOrDefault(x => x.IDX == leg.IDX_FromAP).Description;
                                    report.ReportParameters["Time"].Value = leg.ETD.ToShortTimeString();
                                }

                                report.ReportParameters["AircraftType"].Value = acType.TypeName;
                                report.ReportParameters["Date"].Value = reportParamsWnd.Date1.ToShortDateString();
                                report.ReportParameters["PaxCount"].Value = leg.ResList.SelectMany(x => x.PaxList).ToList().Count();

                                report.ReportParameters["PilotName"].Value = pilotInfo != null ? pilotInfo.Name : "";
                                report.ReportParameters["PilotLicence"].Value = pilotInfo != null ? pilotInfo.PilotsLicenseNumber : "";
                                report.ReportParameters["ResLegIDX"].Value = leg.IDX;
                                switch (SchedRegion)
                                {
                                    case "Botswana": report.ReportParameters["ConnectionString"].Value = "ReportingDBBOTS"; break;
                                    case "Zimbabwe": report.ReportParameters["ConnectionString"].Value = "ReportingDBZIM"; break;
                                    case "Namibia": report.ReportParameters["ConnectionString"].Value = "ReportingDBNam"; break;
                                    case "ZZTest_Botswana": report.ReportParameters["ConnectionString"].Value = "ReportingDBBOTSTest"; break;
                                }

                                reportBook.Reports.Add(report);
                            }

                           
                        }
                    }

                    Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
                    instanceReportSource.ReportDocument = reportBook;
                    reportViewerWnd.ReportViewer.ReportSource = instanceReportSource;
                    reportViewerWnd.Title = "Airstrip Log Book";
                    reportViewerWnd.Owner = parent;
                    reportViewerWnd.Show();
                }

              
            //    if (acPilot!=null)
            //    {
            //        var pilotInfo = acPilot.IDX_Pilot_1.HasValue ? PilotInfo.GetPilotFromPersonnelID(acPilot.IDX_Pilot_1.Value) : null;
                   
            //        var legs = acPilot.Legs;

            //        foreach (var leg in legs)
            //        {
            //            var report = new AirstripLogbook();

            //            report.ReportParameters["Registration"].Value = reportParamsWnd.SelectedItem1.Description;
            //            if (Arrival)
            //            {
            //                report.ReportParameters["TitleText"].Value = "Arrival Information";
            //                report.ReportParameters["AP"].Value = apList.FirstOrDefault(x => x.IDX == leg.IDX_ToAP).Description;
            //                report.ReportParameters["Time"].Value = leg.ETA.ToShortTimeString();
            //            }
            //            else
            //            {
            //                report.ReportParameters["TitleText"].Value = "Departure Information";
            //                report.ReportParameters["AP"].Value = apList.FirstOrDefault(x => x.IDX == leg.IDX_FromAP).Description;
            //                report.ReportParameters["Time"].Value = leg.ETD.ToShortTimeString();
            //            }

            //            report.ReportParameters["AircraftType"].Value = acType.TypeName;
            //            report.ReportParameters["Date"].Value = reportParamsWnd.Date1.ToShortDateString();
            //            report.ReportParameters["PaxCount"].Value = leg.ResList.SelectMany(x => x.PaxList).ToList().Count();

            //            report.ReportParameters["PilotName"].Value = pilotInfo!=null ? pilotInfo.Name:"";
            //            report.ReportParameters["PilotLicence"].Value = pilotInfo != null ? pilotInfo.PilotsLicenseNumber:"";
            //            report.ReportParameters["ResLegIDX"].Value = leg.IDX;
            //            switch (SchedRegion)
            //            {
            //                case "Botswana": report.ReportParameters["ConnectionString"].Value = "ReportingDBBOTS"; break;
            //                case "Zimbabwe": report.ReportParameters["ConnectionString"].Value = "ReportingDBZIM"; break;
            //                case "Namibia": report.ReportParameters["ConnectionString"].Value = "ReportingDBNam"; break;
            //                case "ZZTest_Botswana": report.ReportParameters["ConnectionString"].Value = "ReportingDBBOTSTest"; break;
            //            }

            //            reportBook.Reports.Add(report);
            //        }

            //        Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            //        instanceReportSource.ReportDocument = reportBook;
            //        reportViewerWnd.ReportViewer.ReportSource = instanceReportSource;
            //        reportViewerWnd.Title = "Airstrip Log Book";
            //        reportViewerWnd.Owner = parent;
            //        reportViewerWnd.Show();
            //    }
            }

        }        

    }
}


