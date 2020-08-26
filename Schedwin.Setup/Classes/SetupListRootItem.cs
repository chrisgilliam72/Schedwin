using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class SetupListRootItem
    {
        public String  Description { get; set; }

        public RangeObservableCollection<SetupCountryItem> CountryList { get; set; }


       public  SetupListRootItem()
        {
            CountryList = new RangeObservableCollection<SetupCountryItem>();
        }
    }
}
