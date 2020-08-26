using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class DistanceGridItem
    {

       public String AP { get; set; }
       
      public Dictionary<String,string> DistanceList { get; set; }

        public DistanceGridItem()
        {
            DistanceList = new Dictionary<string, string>();
        }
    }
}
