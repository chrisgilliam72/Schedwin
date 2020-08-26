using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration
{
    public class WishIntegrationUIViewModel :ViewModelBase
    {
        private String _statusText;
        public String StatusText
        {
            get
            {
                return _statusText;
            }
            set
            {
                _statusText = value;
                NotifyPropertyChanged("StatusText");
            }
        }
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }
    }
}
