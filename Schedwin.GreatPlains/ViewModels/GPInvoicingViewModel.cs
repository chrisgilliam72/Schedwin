using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.GreatPlains
{
    public class GPInvoicingViewModel: ViewModelBase
    {

        public int CountryID { get; set;  }
        private String _status;
        public String Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public DateTime TransactionDate { get; set; }

        public RangeObservableCollection<GPInvoiceLineItem> Items { get; set; }

        public GPInvoicingViewModel()
        {
            TransactionDate = DateTime.Today;
            Items = new RangeObservableCollection<GPInvoiceLineItem>();

        }


        public async Task<bool> Init()
        {
            try
            {
                Status = "Initializing ...";
                await Company.LoadCompanyList(Server, Database,false);
               
                Status = "";
                return true;
            }
            catch (Exception ex) 
            {
                Status = "Initialization error";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Initialization error :" + Environment.NewLine + message);

                return false;
       
            }
          
        }
        public void FilterSelectecExport()
        {
            Status = "Generating spreadsheet data...";
            var tmp = Items.Where(x => x.IsSelected).ToList();
            Items.Clear();
            Items.AddRange(tmp);
            NotifyPropertyChanged("Items");
        }


        public void UpdateBatchID()
        {
            var getTxtWnd = new BatchIDWindow();
            getTxtWnd.ShowDialog();
            if (getTxtWnd.DialogResult.HasValue && getTxtWnd.DialogResult.Value)
            {
                var selItems = Items.Where(x => x.IsSelected).ToList();
                if (selItems.Count >1)
                {
                    foreach (var item in selItems)
                    {
                        item.BatchID = getTxtWnd.DebtorCode;
                        item.ItemStatus = "Pending";
                        item.IsInvoiced = false;
                    }
                }
                else
                {
                    if (getTxtWnd.UseSameIDForAllDebtors)
                    {
                        var debtorToUse = selItems.First().Debtor;
                        var similarItems = Items.Where(x => x.Debtor == debtorToUse && x.IsInvoiced==false).ToList();
                        foreach (var item in similarItems)
                        {
                            item.BatchID = getTxtWnd.DebtorCode;
                            item.ItemStatus = "Pending";
                            item.IsInvoiced = false;
                        }
                    }

                }



                NotifyPropertyChanged("Items");
            }
  
        }

        public bool Validate()
        {
            String message = "";
            var batchIDSMissing = Items.Where(x =>x.IsSelected && String.IsNullOrEmpty(x.BatchID)).ToList();
            var DebtorMissing = Items.Where(x => x.IsSelected && String.IsNullOrEmpty(x.Debtor)).ToList();
            if (batchIDSMissing.Count >0)
            {
                message += "Please enter Batch IDs for the following lines:" + Environment.NewLine;
                message += String.Join(Environment.NewLine, batchIDSMissing.Select(x => x.Reservation).ToArray());
            }

            if (DebtorMissing.Count >0)
            {
                message += Environment.NewLine;
                message += "Please enter Debtor IDs for the following lines:" + Environment.NewLine;
                message += String.Join(Environment.NewLine, DebtorMissing.Select(x => x.Reservation).ToArray());
            }


            if (batchIDSMissing.Count > 0 || DebtorMissing.Count > 0)
            {
                FailWindow.Display(message);
                return false;
            }

            return true;
        }

        public async Task<bool> Export()
        {
            try
            {
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var itemsToSave = Items.Where(x => x.IsInvoiced==false).ToList();
                    if (itemsToSave.Count >0)
                    {
                        var updatedItems = Items.Where(x => x.RowGuid.HasValue).ToList();
                        await GPIntegration.RemoveItems(updatedItems, Server, Database);
                        await GPIntegration.ExportItems(itemsToSave, Server, Database);
                    }
                       
                }
                return true;

            }
            catch (Exception ex)
            {

                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to export GP Line Items :" + Environment.NewLine + message);
                return false;
            }
        }

        public async Task Refresh()
        {
            try
            {
                Country thisCountry = Country.GetCountry(CountryID);
                Items.Clear();

                //using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                //{
                //   var tmpLst= await Task.Run(() => GPIntegration.GPInvoicing(TransactionDate, Server, Database));
                //    Items.AddRange(tmpLst);

                //}

                Status = "Loading..";

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var tmpLst=await GPIntegration.GetLineItems(TransactionDate, Server, Database);
                    
                    foreach (var lineItem in tmpLst)
                    {

                        lineItem.SiteID = AirstripInfo.GetAirstripCode(lineItem.IDX_From)+"-"+AirstripInfo.GetAirstripCode(lineItem.IDX_TO);
                        var company = Company.GetCompany(lineItem.IDX_CompanyID);
                        if (company != null && String.IsNullOrEmpty(lineItem.Debtor))
                        {
                            lineItem.Debtor = company.GPID;
                            lineItem.Company = company.Description;
                        }


                        if (String.IsNullOrEmpty(lineItem.Description))
                            lineItem.Description = lineItem.VoucherNo+" "+lineItem.Reservation + " " + lineItem.SiteID + " " + lineItem.ItemID;
                    }

                    Items.AddRange(tmpLst);
                }

                NotifyPropertyChanged("Items");

                Status = "";
            }

            catch (Exception ex)
            {

                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to refresh GP Line Items :" + Environment.NewLine + message);
            }
        }
    }
}
