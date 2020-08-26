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

namespace Schedwin.Setup
{
    /// <summary>
    /// Interaction logic for DistanceCntrlView.xaml
    /// </summary>
    public partial class DistanceCntrlView : ItemControlBase
    {

        public DistanceCntrlView()
        {
            InitializeComponent();
            var viewModel = DataContext as DistanceCntrlViewModel;
            viewModel.View = this;

        }

        private async void distanceGrid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            var viewModel = DataContext as DistanceCntrlViewModel;
            var gridview = sender as Telerik.Windows.Controls.RadGridView;
            var colHeader = gridview.CurrentCellInfo.Column.Header as String;
            var rowData = gridview.CurrentCellInfo.Item as DistanceGridItem;

            if (!String.IsNullOrEmpty(rowData.DistanceList[colHeader]))
            {
                int distance = 0;
                if (int.TryParse(rowData.DistanceList[colHeader], out distance))
                   await viewModel.UpdateDistance(rowData.AP, colHeader, distance);
            }

        }

        private void distanceGrid_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {
            int newVal = 0;

            if (!Int32.TryParse(e.NewValue.ToString(), out newVal) && !String.IsNullOrEmpty(e.NewValue.ToString()))
            {
                e.IsValid = false;
                e.ErrorMessage = "Please enter a number";
            }

       
        }

        private void distanceGrid_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            var viewModel = DataContext as DistanceCntrlViewModel;
            var selItem = e.AddedCells[0].Item as DistanceGridItem;
            var selColumn = e.AddedCells[0].Column.Header.ToString();

            viewModel.selelectedSourceAP = selItem.AP;
            viewModel.selectedDestAP = selColumn;
        }

        private void CalculateItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as DistanceCntrlViewModel;

            viewModel.GetAutoDistance();
        }
    }
}
