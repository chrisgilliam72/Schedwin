using Schedwin.Common;
using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedwin.Setup
{
    public class AddNewItemViewModel : ViewModelBase
    {
        public int ParentIDX { get; set; }

        private String _WindowTitle;
        public String WindowTitle
        {
            get
            {
                return _WindowTitle;
            }
            set
            {
                _WindowTitle = value;
                NotifyPropertyChanged("WindowTitle");
            }
        }

        private ItemCntrlViewModelBase CntrlViewModel { get; set; }

        public ItemControlBase Init(String Server, String Database, NewItemType type, Window parentWindow, int ParentIDX) 
        {

            ItemControlBase itemControl = null;
          
            switch (type)
            {
                case NewItemType.Lodge: itemControl = new LodgeInfoCntrlView();  break;
                case NewItemType.Aircraft:itemControl = new AircraftInfoCntrlView();break;
                case NewItemType.Airstrip: itemControl = new AirstripInfoCntrlView();break;
                case NewItemType.User: itemControl = new UserInfoCntrlView();break;
                case NewItemType.Pilot: itemControl = new PilotInfoCntrlView();break;
                case NewItemType.Flight: itemControl = new FlightInfoCntrlView(); break;
                case NewItemType.AircraftType: itemControl = new AircraftTypeCntrlView();break;
                case NewItemType.Company: itemControl = new CompanyInfoCnrtrlView();break;

            }

            itemControl.ParentWindow = parentWindow;
            CntrlViewModel = itemControl.DataContext as ItemCntrlViewModelBase;
            CntrlViewModel.Database = Database;
            CntrlViewModel.Server = Server;
            CntrlViewModel.ParentIDX = ParentIDX;
            CntrlViewModel.IsNew = true;
            CntrlViewModel.Init();
            return itemControl;
        }

        public bool Validate()
        {
            if (CntrlViewModel!=null)
            {
                return CntrlViewModel.Validate();
            }

            return true;
        }
        public async Task<bool> Save()
        {
            if (CntrlViewModel != null)
            {
               return await CntrlViewModel.Save();              
            }

            return false;
        }

    }

 
}
