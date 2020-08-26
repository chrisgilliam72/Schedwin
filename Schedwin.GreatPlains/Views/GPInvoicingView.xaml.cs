using Microsoft.Win32;
using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace Schedwin.GreatPlains
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GPInvoicingView : SchedwinBaseWindow
    {
        public GPInvoicingView()
        {
            InitializeComponent();
        }


        private async void SchedwinBaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as GPInvoicingViewModel;
            await viewModel.Init();
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as GPInvoicingViewModel;
            await viewModel.Refresh();
        }

   

        private void gridInvList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            var viewModel = DataContext as GPInvoicingViewModel;
            foreach (var item in e.AddedItems)
            {
                var gpItem = item as GPInvoiceLineItem;
                gpItem.IsSelected = true;
            }

            foreach (var item in e.RemovedItems)
            {
                var gpItem = item as GPInvoiceLineItem;
                gpItem.IsSelected = false;
            }

        }


        private async void Invoice_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as GPInvoicingViewModel;
            if (viewModel.Validate())
            {
                viewModel.FilterSelectecExport();
                var exportResult = await viewModel.Export();
                if (exportResult)
                {
                    var tmpPath = System.IO.Path.GetTempPath();
                    var tmpName = tmpPath + DateTime.Now.ToFileTime() + ".xlsx";
                    var fileSteam = File.Create(tmpName);

                    using (fileSteam)
                    {
                        var opts = new GridViewDocumentExportOptions()
                        {
                            ShowColumnHeaders = true,
                            ShowColumnFooters = true,
                            ShowGroupFooters = false,
                        };
                        gridInvList.Columns[2].IsVisible = false;
                        gridInvList.Columns[4].IsVisible = false;
                        gridInvList.Columns[13].IsVisible = false;
                        gridInvList.ExportToXlsx(fileSteam, opts);
                        gridInvList.Columns[2].IsVisible = true;
                        gridInvList.Columns[4].IsVisible = true;
                        gridInvList.Columns[13].IsVisible = true;
                    }

                    await Task.Run(() => System.Diagnostics.Process.Start(fileSteam.Name));
                }

                await viewModel.Refresh();
            }

        }

        private void UpdateBatchID_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var viewModel = DataContext as GPInvoicingViewModel;
            viewModel.UpdateBatchID();

        }


    }
}
