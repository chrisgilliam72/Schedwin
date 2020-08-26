using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class SetupCountryItem
    {
        public int IDX { get; set; }
        public String Description { get; set; }

        public RangeObservableCollection<SetupBaseTreeItem> SubItems { get; set; }

       public SetupCountryItem()
        {
            SubItems = new RangeObservableCollection<SetupBaseTreeItem>();
        }
    }
}
