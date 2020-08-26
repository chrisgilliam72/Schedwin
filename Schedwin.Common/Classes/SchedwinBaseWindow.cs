using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Schedwin.Common
{
    public class SchedwinBaseWindow : Window
    {
        public Guid WindowID { get; set; }
        public bool IsClosed { get; set; }

        public SchedwinBaseWindow()
        {
            WindowID = Guid.NewGuid();
            IsClosed = false;
        }
    }
}
