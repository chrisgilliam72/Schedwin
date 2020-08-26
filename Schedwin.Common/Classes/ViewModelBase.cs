using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Common
{
   public class ViewModelBase : INotifyPropertyChanged
    {
        public bool UseGlobalDB
        {
            get
            {

                return (Database == "Schedwin_Global");
            }

        }
        public String Database { get; set; }

        public String Server { get; set; }

        public String RegionName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
