using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data.Classes;

namespace Schedwin.Setup
{
    public class AirStripExForGridItem : ViewModelBase
    {
        private String _Name;
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                NotifyPropertyChanged("Name");
            }
        }


        private String _type;
        public String Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }

        private AirStripExFor _selectedItem;
        public AirStripExFor SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (value != null)
                {
                    Name = value.Description;
                    switch (value.Type)
                    {
                        case AirStripExForType.Lodge: Type = "Lodge"; break;
                        case AirStripExForType.Flight: Type = "Flight"; break;
                        default: Type = ""; break;

                    }
                }
                _selectedItem = value;
            }
        }

        public AirStripExForGridItem()
        {

        }

        public static explicit operator AirStripExForGridItem(AirStripExFor airstripItem)
        {
            var newItem = new AirStripExForGridItem();
            newItem.Name = airstripItem.Description;
            switch (airstripItem.Type)
            {
                case AirStripExForType.Flight: newItem.Type = "Flight";break;
                case AirStripExForType.Lodge: newItem.Type = "Lodge"; break;
                default: newItem.Type = "Other";break;
            }
            return newItem;
        }

    }
}
