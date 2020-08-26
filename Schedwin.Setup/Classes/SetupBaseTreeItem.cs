using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
   public  enum ItemType {Root,country, lodge,airstrip,aircraftType, aircraftList,user,pilot, flight, distance, company};

    public class SetupBaseTreeItem : ViewModelBase
    {
        public int ParentIDX { get; set; }
        public int IDX { get; set; } 
        public ItemType Type { get; set; }
        public ItemType ChildrenType { get; set; }
        public String Description { get; set; }

        private String _ImagePath;
        public String ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
            }
        }

        public RangeObservableCollection<SetupBaseTreeItem> SubItems { get; set; }

        public SetupBaseTreeItem()
        {
            SubItems = new RangeObservableCollection<SetupBaseTreeItem>();
        }

        public void RefreshChildren()
        {
            NotifyPropertyChanged("SubItems");
        }
    }
}
