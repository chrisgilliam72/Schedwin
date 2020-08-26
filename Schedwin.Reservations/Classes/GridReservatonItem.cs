using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.Reservations.Classes
{
    public class GridReservatonItem : ViewModelBase
    {
        public Brush RowColor
        {
            get
            {
                if (Reservation.Header.IDX_ResStatus == 3)
                    return Brushes.Red;
                else
                {
                    switch (Reservation.ScheduledStatus)
                    {
                        case SchedulingStatus.scheduled: return Brushes.LightGreen;
                        case SchedulingStatus.partial: return Brushes.Yellow;
                        default: return null;

                    }
                }
              
            }
        }
        public Reservation Reservation { get; set; }

        public RangeObservableCollection<GridReservationPaxSubItem> PaxList { get; set; }
        public RangeObservableCollection<GridReservationHeaderSubItem> HeaderList { get; set; }

        public RangeObservableCollection<GridReservationLegSubItem> LegList { get; set; }

        public RangeObservableCollection<GridReservationScheduleSubItem> ScheduleList { get; set; }
        public String Operator
        {
            get
            {
                if (Reservation != null && Reservation.Header != null)
                    return Reservation.Header.OperatorName;
                else
                    return "";
            }
        }
        public String Name
        {
            get
            {
                if (Reservation != null && Reservation.Header != null)
                    return Reservation.Header.ReservationName;
                else
                    return "";
            }
        }

        public String Type
        {
            get
            {
                if (Reservation != null && Reservation.Header != null)
                    return Reservation.Header.ReservationType;
                else
                    return "";
            }
        }

        public String WishNo
        {
            get
            {
                if (Reservation != null && Reservation.Header != null)
                {
                    return Reservation.Header.BookingID > 0 ? Reservation.Header.BookingID.ToString() : "-";
                }
                else
                    return "";
            }
        }
        public DateTime FirstLeg
        {
            get
            {
                if (Reservation != null && Reservation.Legs != null && Reservation.Legs.Count > 0)
                    return Reservation.Legs.Min(x => x.BookingDate);
                else
                    return DateTime.Today;
            }


        }

        public DateTime LastLeg
        {
            get
            {
                if (Reservation != null && Reservation.Legs != null && Reservation.Legs.Count > 0)
                    return Reservation.Legs.Max(x => x.BookingDate);
                else
                    return DateTime.Today;
            }
        }

        public bool IsCancelled
        {
            get
            {
                if (Reservation!=null && Reservation.Header.IDX_ResStatus == 3)
                    return true;
                else
                return false;
            }
        }


        public GridReservatonItem()
        {
            PaxList = new RangeObservableCollection<GridReservationPaxSubItem>();
            HeaderList = new RangeObservableCollection<GridReservationHeaderSubItem>();
            LegList = new RangeObservableCollection<GridReservationLegSubItem>();
            ScheduleList = new RangeObservableCollection<GridReservationScheduleSubItem>();

        }

        public  void ConfigureSubLists()
        {
           
            var gridHdr = (GridReservationHeaderSubItem)Reservation.Header;
            var gridPaxList = Reservation.Passengers.Select(x => (GridReservationPaxSubItem)x).ToList();
            var gridLegList = Reservation.Legs.Select(x => (GridReservationLegSubItem)x).OrderBy(x=>x.BookingDate).ToList();
            var gridScheduleList = Reservation.LegSchedules.Select(x => (GridReservationScheduleSubItem)x).OrderBy(x=>x.FlightDate).ToList();


            HeaderList.Clear();
            PaxList.Clear();
            LegList.Clear();
            ScheduleList.Clear();


            HeaderList.Add(gridHdr);
            PaxList.AddRange(gridPaxList);
            LegList.AddRange(gridLegList);
            ScheduleList.AddRange(gridScheduleList);

            NotifyPropertyChanged("ScheduleList");
            NotifyPropertyChanged("LegList");
            NotifyPropertyChanged("HeaderList");
            NotifyPropertyChanged("PaxList");
        }

    }
}
