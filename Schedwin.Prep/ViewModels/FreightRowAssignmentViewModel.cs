using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class FreightRowAssignmentViewModel : ViewModelBase
    {
        private String _rowDetails;
        public String RowDetails
        {
            get
            {
                return _rowDetails;
            }

            set
            {
                _rowDetails = value;
                NotifyPropertyChanged("RowDetails");
            }
        }

        private int _ActualWeight;
        public int ActualWeight
        {
            get
            {
                return _ActualWeight;
            }
            set
            {
                _ActualWeight = value;
                NotifyPropertyChanged("ActualWeight");
            }
        }

        public void Clear()
        {
            ActualWeight = 0 ;
        }
    }
}
