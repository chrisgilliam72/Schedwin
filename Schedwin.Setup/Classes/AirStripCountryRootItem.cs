using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class AirStripCountryTreeItem : ViewModelBase
    {
        public String Country { get; set; }

        private RangeObservableCollection<AirStripExForTreeItem> _ExForList;
        public RangeObservableCollection<AirStripExForTreeItem> ExForList
        {
            get
            {
                return _ExForList;
            }
            set
            {
                _ExForList = value;
            }
        }


        public AirStripCountryTreeItem()
        {
            _ExForList = new RangeObservableCollection<AirStripExForTreeItem>();

        }

        public void RefreshList()
        {
            NotifyPropertyChanged("ExForList"); ;
        }
    }
}
