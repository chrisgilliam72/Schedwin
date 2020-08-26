using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Setup
{
    public class PricelistCntrlViewModel :  ItemCntrlViewModelBase
    {
        public int CompanyIDX { get; set; }
        public RangeObservableCollection <PriceList> PriceLists { get; set; }
        public RangeObservableCollection<GenericListItem> AirstripCodes { get; set; }

        private PriceList _selectedItem;
        public PriceList SelectedItem
        {
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged("CanDelete");
            }
            get
            {
                return _selectedItem;
            }
        }

        public bool CanDelete
        {
            get
            {
                return SelectedItem != null;
            }
        }

        public PricelistCntrlViewModel()
        {
            PriceLists = new RangeObservableCollection<PriceList>();

            AirstripCodes = new RangeObservableCollection<GenericListItem>();
        }

        //public async Task<bool> Refresh(int companyIDX)
        //{
        //    try
        //    {
        //        AirstripCodes.Clear();
        //        var airStripLst= AirstripInfo.GetAirstrips().Select(x=> new GenericListItem { IDX = x.IDX, Code = x.Code, Description = x.Description }).ToList();
        //        AirstripCodes.AddRange(airStripLst);
        //        CompanyIDX = companyIDX;
        //        PriceLists.Clear();
        //        using (new StackedCursorOverride(Cursors.Wait))
        //        {
        //            var tmpLst = await PriceList.GetSeatRatePriceList(companyIDX, ServerName, Database);
        //            foreach (var item in tmpLst)
        //            {
        //                item.Start = AirstripInfo.GetAirstripCode(item.StartIDX);
        //                item.End = AirstripInfo.GetAirstripCode(item.DestIDX);
        //            }
        //            PriceLists.AddRange(tmpLst);
        //        }

        //        NotifyPropertyChanged("PriceLists");
        //        NotifyPropertyChanged("AirstripCodes");

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public void DeleteEntry()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Price List entry";
            parameters.Content = "Are you sure want to delete this entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnDeleteConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }

        private async void OnDeleteConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedItem != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {
                            await PriceList.Delete(SelectedItem.IDX, SelectedItem.Type, Server, Database);
                            PriceLists.Remove(SelectedItem);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

        }

        public async Task<bool> SaveEntry(PriceList priceList)
        {
            try
            {
                if (priceList != null)
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        await priceList.Save(Server , Database);
                    }
                    priceList.IsNew = false;
                }

                return true;
            }
 
           catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }

        public void AddNewPriceList()
        {
            var newPriceList = new PriceList();
            newPriceList.IsNew = true;
            newPriceList.CompanyIDX = CompanyIDX;
            newPriceList.Type = "Seat";
            PriceLists.Insert(0, newPriceList);
            NotifyPropertyChanged("PriceLists");
        }
    }
}
