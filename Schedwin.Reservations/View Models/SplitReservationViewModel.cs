using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations
{
    public class SplitReservationViewModel : ViewModelBase
    {
        public int SplitGuestCount { get; set; }

        public int CurrentBookingGuestCount { get; set; }

        public void Init()
        {
            NotifyPropertyChanged("CurrentBookingGuestCount");
        }
    }
}
