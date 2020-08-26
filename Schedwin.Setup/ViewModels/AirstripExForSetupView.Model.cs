using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Schedwin.Setup
{
    public class AirstripExForSetupViewModel : Schedwin.Common.ViewModelBase
    {
     
        private AirStripExForGridItem _selectedGriItem;

        public AirStripExForGridItem SelectedGridItem
        {
            get
            {
                return _selectedGriItem;
            }
            set
            {
                _selectedGriItem = value;
            }
        }

        private AirStripExForTreeItem _selectedAirstrip;

        public AirStripExForTreeItem SelectedAirstrip
        {
            get
            {
                return _selectedAirstrip;
            }
            set
            {
                _selectedAirstrip = value;
                if (value!=null)
                {
                    AirstripExForList.Clear();
                    var tmpLst = AllExForList.Where(x => x.IDX_Airstrip== value.IDX).ToList();

                    var tmpGridLst=tmpLst.Select(x => (AirStripExForGridItem)x).ToList();
                    AirstripExForList.AddRange(tmpGridLst);
                    NotifyPropertyChanged("AirstripExForList");
                }

            }
        }
        public RangeObservableCollection<String> ExForType { get; set; }
        public RangeObservableCollection<AirStripExFor> AllExForList { get; set; }

        public RangeObservableCollection<AirStripExForGridItem> AirstripExForList { get; set; }
        private RangeObservableCollection<AirStripCountryTreeItem> _ExForTreeList { get; set; }
        public RangeObservableCollection<AirStripCountryTreeItem> ExForTreeList
        {
            get
            {
                return _ExForTreeList;
            }
            set
            {
                _ExForTreeList = value;
            }
        }

        public AirstripExForSetupViewModel()
        {
            _ExForTreeList = new RangeObservableCollection<AirStripCountryTreeItem>();
            AllExForList = new RangeObservableCollection<AirStripExFor>();
            AirstripExForList = new RangeObservableCollection<AirStripExForGridItem>();
            ExForType = new RangeObservableCollection<string>();
        }


        public  void Init()
        {
          
            Refresh();
        }

       

        public void AddNewExForGridItem()
        {
            var dummyitem = new AirStripExForGridItem();
            dummyitem.Name = "";
            dummyitem.Type = "";
            AirstripExForList.Add(dummyitem);
            NotifyPropertyChanged("AirstripExForList");
        }


        public void  RemoveExForGridItem()
        {
            if (SelectedGridItem!=null)
            {
                if (SelectedGridItem.Type!="Flight")
                {
                    RadWindow.Alert("Only flights may be removed from this screen");
                    return;
                }
                RadWindow.Confirm("Are you sure you want to remove tthis item?", RemmoveItem);
              
            }
        }


        private void AddCountryAirstripNodes(AirStripCountryTreeItem rootTreeItem,List<AirstripInfo> airstripInfoList)
        {
            foreach (var airstrip in airstripInfoList)
            {
                var newAirstripItem = new AirStripExForTreeItem();
                newAirstripItem.IDX = airstrip.IDX;
                newAirstripItem.Name = airstrip.Description + "(" + airstrip.Code + ")";
                rootTreeItem.ExForList.Add(newAirstripItem);
            }

        }


        private  void RefreshCurrentAirstripExForList()
        {
            AirstripExForList.Clear();
            var tmpLst = AllExForList.Where(x => x.IDX_Airstrip == SelectedAirstrip.IDX).ToList();

            var tmpGridLst = tmpLst.Select(x => (AirStripExForGridItem)x).ToList();
            AirstripExForList.AddRange(tmpGridLst);
        }


        private async void Refresh()
        {
            AirstripExForList.Clear();
            AllExForList.Clear();
            _ExForTreeList.Clear();

            List<AirstripInfo> lstAirstrps = null;
            List<AirStripExFor> tmpLstExFors = null;
            using (new StackedCursorOverride(Cursors.Wait))
            {
                lstAirstrps = AirstripInfo.GetAirstrips();
                tmpLstExFors = await CampExForAirstrips.GetExForListV2(Server, Database);

            }
            
            AllExForList.AddRange(tmpLstExFors.OrderBy(x => x.Description).ToList());

            var countryID = Country.GetCountry("Botswana").IDX;
            var botsAirstrips = lstAirstrps.Where(x => x.IDX_Country == countryID).OrderBy(x => x.Description).ToList(); ;

            var newTreeItem = new AirStripCountryTreeItem();
            newTreeItem.Country = "Botswana";
            AddCountryAirstripNodes(newTreeItem, botsAirstrips);
            _ExForTreeList.Add(newTreeItem);
            //newTreeItem.RefreshList();

            countryID = Country.GetCountry("Namibia").IDX;
            var namAirStrips = lstAirstrps.Where(x => x.IDX_Country == countryID).OrderBy(x => x.Description).ToList();

            newTreeItem = new AirStripCountryTreeItem();
            AddCountryAirstripNodes(newTreeItem, namAirStrips);
            newTreeItem.Country = "Namibia";
            _ExForTreeList.Add(newTreeItem);

            countryID = Country.GetCountry("Zambia").IDX;
            var zamAirStrips = lstAirstrps.Where(x => x.IDX_Country == countryID).OrderBy(x => x.Description).ToList();
            newTreeItem = new AirStripCountryTreeItem();
            newTreeItem.Country = "Zambia";
            AddCountryAirstripNodes(newTreeItem, zamAirStrips);
            _ExForTreeList.Add(newTreeItem);

            countryID = Country.GetCountry("Zimbabwe").IDX;
            var zimAirStrips = lstAirstrps.Where(x => x.IDX_Country == countryID).OrderBy(x => x.Description).ToList();
            newTreeItem = new AirStripCountryTreeItem();
            newTreeItem.Country = "Zimbabwe";
            AddCountryAirstripNodes(newTreeItem, zimAirStrips);
            _ExForTreeList.Add(newTreeItem);


            NotifyPropertyChanged("ExForTreeList");
            NotifyPropertyChanged("AllExForList");
            NotifyPropertyChanged("AirstripExForList");
        }

        private async void RemmoveItem(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await CampExForAirstrips.RemoveFlight(SelectedGridItem.Name, Server, Database);
                    var itemtoRemove = AllExForList.FirstOrDefault(x => x.Description == SelectedGridItem.Name);
                    AllExForList.Remove(itemtoRemove);
                    RefreshCurrentAirstripExForList();
                    NotifyPropertyChanged("SelectedAirstrip");
                }
            }
        }
    }
}
