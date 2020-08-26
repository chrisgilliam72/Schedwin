using Schedwin.Common;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Schedwin.Reservations.Classes;
namespace Schedwin.WishIntegration.Classes
{
   
    public class WIReservationLeg :ViewModelBase
    {
   
        public enum DBLegState { IsNew, IsModified, IsUnmodified,IsCancelled };
        public WIReservationHeader WIHeader { get; set; }
        public ReservationLeg WishResLeg { get; set; }

        private DBLegState _state;
        public DBLegState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        private bool _hasChangedETD;
 
        public bool HasChangedETD
        {
            get
            {
                return _hasChangedETD;
            }
            set
            {
                _hasChangedETD = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                NotifyPropertyChanged("ETDBrush");
            }
        }


        private bool _hasChangedETA;
        public bool HasChangedETA
        {
            get
            {
                return _hasChangedETA;
            }
            set
            {
                _hasChangedETA = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                NotifyPropertyChanged("ETABrush");
            }
        }



        private bool _hasChangedWishEx;
        public bool HasChangedWishEx
        {
            get
            {
                return _hasChangedWishEx;
            }

            set
            {

                _hasChangedWishEx = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                NotifyPropertyChanged("WishExBrush");
            }
        }

        private bool _hasChangedWishFor;
        public bool HasChangedWishFor
        {
            get
            {
                return _hasChangedWishFor;
            }

            set
            {

                _hasChangedWishFor = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                NotifyPropertyChanged("WishForBrush");
            }
        }

        private bool _hasChangedBookingDate;
        public bool HasChangedBookingDate
        {
            get
            {
                return _hasChangedBookingDate;
            }

            set
            {
                _hasChangedBookingDate = value;
                NotifyPropertyChanged("BookingDateBrush");
            }
        }

        private bool _hasChangeNotes;
        public bool HasChangedNotes
        {
            get
            {
                return _hasChangeNotes;
            }

            set
            {
                _hasChangeNotes = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                NotifyPropertyChanged("BookingDateBrush");
            }
        }

        public DateTime EarliestEx
        {
            get
            {
                return WishResLeg.EarliestEx;

            }
            set
            {
               
                WishResLeg.EarliestEx = value;
                if (State!=DBLegState.IsNew)
                    State = DBLegState.IsModified;
            }
        }

        public DateTime LatestEx
        {
            get
            {
                return WishResLeg.LatestEx;
               
            }
            set
            {
                WishResLeg.LatestEx =value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
            }
        }

        public DateTime EarliestFor
        {
            get
            {
                return WishResLeg.EarliestFor;
               
            }
            set
            {
  
                WishResLeg.EarliestFor = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
            }
        }

        public DateTime LatestFor
        {
            get
            {
                return WishResLeg.LatestFor;

            }
            set
            {
                WishResLeg.LatestFor = value;
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
            }
        }


        public String SelectedEx
        {
            get
            {
                return WishResLeg.ExField;
            }

            set
            {
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                WishResLeg.ExField = value;
                UpdateEXTimes(value);
            }
        }


        public String SelectedFor
        {
            get
            {
                return WishResLeg.ForField;
            }
            set
            {
                if (State != DBLegState.IsNew)
                    State = DBLegState.IsModified;
                WishResLeg.ForField = value;
                UpdateForTimes(value);
            }
        }

        public decimal Budget
        {
            get
            {
                return Math.Round(WishResLeg.VoucherAmount, 2);
            }
        }

        public Brush NotesBrush
        {
            get
            {
                if (HasChangedNotes)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush BookingDateBrush
        {
            get
            {
                if (HasChangedBookingDate)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush WishExBrush
        {
            get
            {
                if (HasChangedWishEx)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush WishForBrush
        {
            get
            {
                if (HasChangedWishFor)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush ETABrush
        {
            get
            {
                if (HasChangedETA)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush ETDBrush
        {
            get
            {
                if (HasChangedETD)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        private RangeObservableCollection<AirStripExFor> _forList;
        public RangeObservableCollection<AirStripExFor> ForList
        {
            get
            {
                return _forList;
            }
            set
            {
                _forList = value;
                NotifyPropertyChanged("ForList");

            }
        }

        private RangeObservableCollection<AirStripExFor> _exList;
        public RangeObservableCollection<AirStripExFor> ExList
        {
            get
            {
                return _exList;
            }
            set
            {
                ExList = value;
                NotifyPropertyChanged("ExList");

            }
        }

        private void UpdateEXTimes(String exValue)
        {
            var exObject = ExList.FirstOrDefault(x => x.Description == exValue);
            if (exObject != null)
            {
                WishResLeg.EarliestEx = exObject.TimeOut;
                WishResLeg.LatestEx = exObject.TimeOut;

                RefreshTimes();
            }

        }

        private void UpdateForTimes(String exValue)
        {
            var forObject = ForList.FirstOrDefault(x => x.Description == exValue);
            if (forObject != null)
            {
                WishResLeg.EarliestFor = forObject.TimeIn;
                WishResLeg.LatestFor = forObject.TimeIn;

                RefreshTimes();
            }

        }

        public void RefreshTimes()
        {
            NotifyPropertyChanged("EarliestEx");
            NotifyPropertyChanged("EarliestFor");
            NotifyPropertyChanged("LatestEx");
            NotifyPropertyChanged("LatestFor");

        }


        public WIReservationLeg()
        {
            _forList = new RangeObservableCollection<AirStripExFor>();
            _exList = new RangeObservableCollection<AirStripExFor>();
        }


        public static explicit operator WIReservationLeg(WICharterBookingLeg charterLeg)
        {
            var resLeg = new WIReservationLeg();
            resLeg.WishResLeg = new ReservationLeg();
            if (charterLeg.IsCancelled)
                resLeg.State = WIReservationLeg.DBLegState.IsCancelled;
            else
                resLeg.State = WIReservationLeg.DBLegState.IsNew;
            resLeg.WishResLeg.WishSectorID = charterLeg.SectorBookingID;
            if (charterLeg.ETA != null)
                resLeg.WishResLeg.ETA = Convert.ToDateTime(charterLeg.ETA);
            if (charterLeg.ETD != null)
                resLeg.WishResLeg.ETD = Convert.ToDateTime(charterLeg.ETD);
            if (charterLeg.SectorNotes != null)
                resLeg.WishResLeg.Notes = charterLeg.SectorNotes.Replace("\r\n", "");
            resLeg.WishResLeg.ExCode = charterLeg.ExCode;
            resLeg.WishResLeg.WishEx = charterLeg.ActualEx;
            resLeg.WishResLeg.WishFor = charterLeg.ActualFor;
            resLeg.WishResLeg.ForCode = charterLeg.ForCode;

            resLeg.WishResLeg.BookingDate = charterLeg.BookingDate;
            resLeg.WishResLeg.CharterType = charterLeg.CharterType;
            resLeg.WishResLeg.SoleUse = charterLeg.SoleUse;
            resLeg.WishResLeg.FromAP = charterLeg.From;
            resLeg.WishResLeg.ToAP = charterLeg.To;
            return resLeg;
        }

        public static explicit operator WIReservationLeg(ReservationLeg wishResLeg)
        {
            var wiResLeg = new WIReservationLeg();
            wiResLeg.WishResLeg = wishResLeg;
            if (wishResLeg.Canceled)
                wiResLeg.State = WIReservationLeg.DBLegState.IsCancelled;
            else
                wiResLeg.State = WIReservationLeg.DBLegState.IsUnmodified;
            return wiResLeg;
        }
    }
}
