using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using Schedwin.Common;

namespace SchedwinWPF
{
    public interface IMainWindowController
    {
        SchedwinBaseWindow ShowNewScheduling(Window Parent);
        SchedwinBaseWindow ShowExForSetup(Window Parent);
        SchedwinBaseWindow ShowNewReservations(Window Parent);
        SchedwinBaseWindow ShowWishIntegration(Window Parent);
        SchedwinBaseWindow ShowPilotRoster(Window Parent);
        SchedwinBaseWindow ShowSetup(Window Parent);
        SchedwinBaseWindow ShowNewTechLogs(Window Parent);

        SchedwinBaseWindow ShowNewGPInvoicing(Window Parent);
        SchedwinBaseWindow ShowTicketsNew (Window Parent);
        SchedwinBaseWindow ShowNewIndigoTrack(Window Parent);
        SchedwinBaseWindow ShowWeightBalance(Window Parent);
        void ShowUnlockSchedules(Window Parent);
        Form ShowGPInvoicing(System.Windows.Forms.Control Parent);

        Form ShowUpdateOldSchedule(System.Windows.Forms.Control Parent);
        Form ShowAircraftPrep(System.Windows.Forms.Control Parent);
        Form ShowTickets(System.Windows.Forms.Control Parent);
        Form ShowTicketHistory(System.Windows.Forms.Control Parent);
        Form ShowBaggageTags(System.Windows.Forms.Control Parent);
        Form ShowIndigoTrack(System.Windows.Forms.Control Parent);
        Form ShowTrackingTimes(System.Windows.Forms.Control Parent);

        Form ShowGPSetup(System.Windows.Forms.Control Parent);
        Form ShowReporting(System.Windows.Forms.Control frmParent, Window wndParent, String reportName);


    }
}
