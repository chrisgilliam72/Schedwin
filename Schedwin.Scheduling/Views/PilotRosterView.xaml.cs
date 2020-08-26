using Schedwin.Data.Classes;
using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Schedwin.Scheduling
{
    /// <summary>
    /// Interaction logic for PilotRosterView.xaml
    /// </summary>
    public partial class PilotRosterView : SchedwinBaseWindow
    {
        public PilotRosterView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PilotRosterViewModel;
            await viewModel.Init();
        }

        private void DetailGrid_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            //var viewModel = DataContext as PilotRosterViewModel;
            //if (e.RemovedItems.Count >0)
            //{
            //    var prevDutyPeriod = e.RemovedItems[0] as PilotDutyPeriod;
            //    if (prevDutyPeriod != null)
            //    {
            //        viewModel.SavePrevious(prevDutyPeriod);
            //    }
            //}
        }

        private async void RadDatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           if (e.AddedItems.Count>0)
            {
                var viewModel = DataContext as PilotRosterViewModel;
                var newDate = Convert.ToDateTime(e.AddedItems[0]);
                newDate = new DateTime(newDate.Year, newDate.Month, 1);
                await viewModel.Refresh(newDate);
            }

        }

        private void SchedwinBaseWindow_Closed(object sender, EventArgs e)
        {
            IsClosed = true;
        }

        private void MenuItemCopy_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as PilotRosterViewModel;
            viewModel.Copy();
        }

        private void MenuItemPaste_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as PilotRosterViewModel;
            viewModel.Paste();
        }

        private async void DetailGrid_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        {
            var viewModel = DataContext as PilotRosterViewModel;
            if (e.Cell != null)
            {
                var pilotDutyPeriod = e.Cell.DataContext as PilotDutyPeriod;
                int day = e.Cell.Column.DisplayIndex;
                if (pilotDutyPeriod!=null)
                    await viewModel.UpdateDuty(pilotDutyPeriod, day);

            }


        }

        private void DetailGrid_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            //var newDataStr = e.NewValue.ToString();

            //if (newDataStr == "1" || newDataStr == "0" || newDataStr == "L" || newDataStr == "T" || newDataStr == "E")
            //    e.IsValid = true;
            //else
            //{
            //    e.ErrorMessage = "Please enter either 1,0,L or T";
            //    e.IsValid = false;
            //}
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as PilotRosterViewModel;
            await viewModel.Save();
        }

     
    }
}
