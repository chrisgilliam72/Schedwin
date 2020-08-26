using Schedwin.Common;
using Schedwin.Reservations.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace Schedwin.Reservations
{
    /// <summary>
    /// Interaction logic for ReservationInfoView.xaml
    /// </summary>
    public partial class ReservationInfoView : RadWindow
    {
        public ReservationInfoView()
        {
            InitializeComponent();
        }

        private async void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.View = this;
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
               await dataContext.Init();
            }
                

        }

        private void AddNewLeg_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.AddNewLeg();
        }

        private void RemoveLeg_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadWindow.Confirm("Are you sure you want to remove this leg", ConfirmRemoveLeg);
        }

        private void CancelLeg_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadWindow.Confirm("Are you sure you want to cancel this leg?", ConfirmCancelLeg);
        }

        private void BudgetInfo_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.ViewBudgetInfo();
        }


        private void AddNewPax_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.AddNewPax();
        }

      

        private void RemovePax_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadWindow.Confirm("Are you sure you want to remove these passengers?", ConfirmRemovePax);
        }

        private async void RefreshPax_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;       
            await dataContext.RefreshPax();    
        }

        private void SplitPax_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            RadWindow.Confirm("Are you sure you want to split off these pax?", ConfirmSplitPax);


        }

        
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            btnSave.IsEnabled = false;
            var dataContext = DataContext as ReservationInfoViewModel;
            var saveResult= await dataContext.Save();
            if (saveResult)
            {
                DialogResult = true;
                Close();
            }
            btnSave.IsEnabled = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            RadWindow.Confirm("Are you sure you want to cancel and discard any changes?", CancelCloseWindow);
        }



        private void ConfirmRemoveLeg(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                var selectedPax = this.gridPax.SelectedItems.OfType<ReservationPax>().ToList();
                var dataContext = DataContext as ReservationInfoViewModel;
                dataContext.RemoveLeg();

            }
        }

        private void ConfirmCancelLeg(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                var selectedPax = this.gridPax.SelectedItems.OfType<ReservationPax>().ToList();
                var dataContext = DataContext as ReservationInfoViewModel;
                dataContext.CancelLeg();

            }
        }

        private async void ConfirmSplitPax(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {

                var selectedPax = this.gridPax.SelectedItems.OfType<ReservationPax>().ToList();
                var dataContext = DataContext as ReservationInfoViewModel;
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var result = await dataContext.SplitPax(selectedPax);

                    if (result != null)
                    {
                        SuccessWindow.Display("Split successful, this window will now close");
                        Close();
                    }

                    else
                        FailWindow.Display("An error hass occurred and this reservation was not split.");

                }

            }
        }

        private void ConfirmRemovePax(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                var selectedPax = this.gridPax.SelectedItems.OfType<ReservationPax>().ToList();
                var dataContext = DataContext as ReservationInfoViewModel;
                dataContext.RemoveSelectedPax(selectedPax);

            }
        }
        private void CancelCloseWindow(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult==true)
            {
                DialogResult = false;
                Close();
            }
        }



        private void RadGridView_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.LegRowChangeComitted();
        }

        private void AirportTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.LegTOSelectionChanged();
        }

        private void AirportFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.UpdateLegDistance();
        }

        private void RadGridView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
       
        }

        private void Button_SelectConsultant_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.SelectConsultant();
        }

        private void Button_SelectWishConsultant_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.SelectWishConsultant();
        }

        private void Operator_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = DataContext as ReservationInfoViewModel;
            dataContext.OperatorSelectionChanged();

        }
    }
}
