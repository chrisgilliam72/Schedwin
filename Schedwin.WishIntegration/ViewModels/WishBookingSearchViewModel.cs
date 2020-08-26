using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration
{
    public class WishBookingSearchViewModel :ViewModelBase
    {
        private bool _isByID;
        public bool IsByID
        {
            get
            {
                return _isByID;
            }
            set
            {
                _isByID = value;
                NotifyPropertyChanged("IsByID");
            }
        }

        private bool _isByName;
        public bool IsByName
        {
            get
            {
                return _isByName;
            }
            set
            {
                _isByName = value;
                NotifyPropertyChanged("IsByName");
            }
        }

        public String BookingName { get; set; }

        public int BookingID { get; set; }

        public bool LocalModel { get; set; }

        public WishBookingSearchViewModel()
        {
            IsByID = true;
            IsByName = false;
            LocalModel = true;
        }

        public void SetSearchByName()
        {
           
            IsByID = false;
            IsByName = true;
            BookingID = 0;
            NotifyPropertyChanged("BookingID");
        }

        public void SetSearchByID()
        {
            IsByID = true;
            IsByName = false;
            BookingName = "";
            NotifyPropertyChanged("BookingName");
        }
    }
}
