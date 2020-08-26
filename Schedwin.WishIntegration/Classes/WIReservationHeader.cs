using Schedwin.Common;
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
    public class WIReservationHeader : ViewModelBase
    {
        public bool IsNew { get; set; }
        static public RangeObservableCollection<WIBookingCompany> Operators { get; set; }

        public bool Save { get; set; }

        private bool _hasChangedLeg;

        public bool HasChangedLeg
        {
            get
            {
                return _hasChangedLeg;
            }
            set
            {
                _hasChangedLeg = value;
                NotifyPropertyChanged("HasChangedLeg");
            }
        }

        private bool _hasNewLeg;
        public bool HasNewLeg
        {
            get
            {
                return _hasNewLeg;
            }
            set
            {
                _hasNewLeg = value;
                NotifyPropertyChanged("HasNewLeg");
            }
        }

        private bool _hasRemovedLeg;
        public bool HasRemovedLeg
        {
            get
            {
                return _hasRemovedLeg;
            }
            set
            {
                _hasRemovedLeg = value;
                NotifyPropertyChanged("HasRemovedLeg");
            }
        }


        private bool _hasDifferentResName;

        public bool HasDifferentResName
        {
            get
            {
                return _hasDifferentResName;
            }
            set
            {
                _hasDifferentResName = value;
                NotifyPropertyChanged("ResNameColor");
            }
        }

        private bool _hasPaxDifference;
        public bool HasPaxDifference
        {
            get
            {
                return _hasPaxDifference;
            }
            set
            {
                _hasPaxDifference = value;
                NotifyPropertyChanged("PaxColor");
            }
        }

        private bool _hasDateChange;
        public bool HasDateChange
        {
            get
            {
                return _hasDateChange;
            }
            set
            {
                _hasDateChange = value;
                NotifyPropertyChanged("DateColor");
            }
        }

        private bool _hasNotesChange;
        public bool HasNotesChange
        {
            get
            {
                return _hasNotesChange;
            }
            set
            {
                _hasNotesChange = value;
                NotifyPropertyChanged("NotesColor");
            }
        }

        public String DepartureDateString
        {
            get
            {
                return DepartureDate.ToShortDateString();
            }
        }
        public ReservationHeader WishResHeader { get; set; }

        public String StartDate
        {
            get
            {
                return WishResHeader.FirstBookingDate.ToShortDateString();
            }
        }


        public String EndDate
        {
            get
            {
                return WishResHeader.LastBookingDate.ToShortDateString() ;
            }
        }
        public DateTime DepartureDate
        {
            get
            {
                return WishResHeader.DepartureDate;
            }
        }
        public String Notes
        {
            get
            {
                return WishResHeader.ReservationNote;
            }
        }
        public String Consultant
        {
            get
            {
                return WishResHeader.WishConsultant;
            }
        }


        public String TPRef { get; set; }

        public int BookingID
        {
            get
            {
                return WishResHeader.BookingID;
            }
        }

        public int PartyGroupID
        {
            get
            {
                return WishResHeader.PartyGroupID;
            }
        }


        public int Pax
        {
            get
            {
                return WishResHeader.PaxCount;
            }
            set
            {
                WishResHeader.PaxCount = value;
                NotifyPropertyChanged("Pax");
            }
        }

        public String Personnel
        {
            get
            {
                return WishResHeader.SefofaneAgentName;
            }
        }

        public String BookingDate
        {
            get
            {
                return WishResHeader.DateCaptured.ToShortDateString();
            }
        }


        public String OldPax { get; set; }
        public String OldReservationName { get; set; }
        public String OldDepartureDate { get; set; }
        public String ReservationName
        {
            get
            {
                return WishResHeader.ReservationName;
            }
            set
            {
                WishResHeader.ReservationName = value;
                NotifyPropertyChanged("ReservationName");
            }
        }
        private int _agentID;

        public int AgentID
        {
            get
            {
                return _agentID;
            }
            set
            {
                _agentID = value;

                var agent = _agents.FirstOrDefault(x => x.AgentIDX == value);
                if (agent!=null)
                {
                    WishResHeader.IDX_OperatorAgent = _agentID;
                    AgentName = agent.AgentName;
                    NotifyPropertyChanged("AgentName");
                }

            }
        }


        public String CurrencyCode { get; set; }

        public String AgentName { get; set; }


        private int _operatorID;
    
        public int OperatorID
        {
            get
            {
                return _operatorID;
            }
            set
            {

                if (value!=_operatorID)
                {
                  
                    var selOperator= Operators.FirstOrDefault(x => x.CompanyIDX == value);
                    if (selOperator != null)
                    {
                        CompanyName = selOperator.CompanyName;
                        CurrencyCode = selOperator.CompanyCurrencyCode;
                    }


                    _operatorID = value;

                    WishResHeader.IDX_Operator = _operatorID;
                    NotifyPropertyChanged("CompanyName");
                }
               
            }
        }



        public String CompanyName { get; set; }
        public String ResStatus
        {
            get
            {
                return WishResHeader.WishResStatus;
            }
        }
        public Brush StatusColor
        {
            get
            {
                switch (WishResHeader.ReservationStatus)
                {
                    case "Confirmed": return Brushes.LightGreen;
                    case "Provisional": return Brushes.LightGray;
                    case "Cancelled": return Brushes.Red;
                    default: return null;
                }

            }
        }

        public Brush DateColor
        {
            get
            {
                if (HasDateChange)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush PaxColor
        {
            get
            {
                if (HasPaxDifference)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush ResNameColor
        {
            get
            {
                if (HasDifferentResName)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public Brush NotesColor
        {
            get
            {
                if (HasNotesChange)
                    return Brushes.Yellow;
                else
                    return null;
            }
        }

        public WIReservationHeader()
        {
            WishResHeader = new ReservationHeader();
            Legs = new List<WIReservationLeg>();
            PaxList = new List<WIReservationPax>();
            _agents = new RangeObservableCollection<WIBookingAgent>();
            HasNewLeg = false;
            HasRemovedLeg = false;
            HasPaxDifference = false;
            IsNew = true;
        }

        private RangeObservableCollection<WIBookingAgent> _agents;
        public RangeObservableCollection<WIBookingAgent> Agents
        {
            get
            {
                return _agents;
            }
            set
            {
                _agents = value;
            }
        }

        public void InvalidateAllLegs()
        {
            Legs.ForEach(x => x.State = WIReservationLeg.DBLegState.IsModified);
        }

        public List<WIReservationLeg> Legs { get; set; }
        public List<WIReservationPax> PaxList { get; set; }
     
        public static explicit operator WIReservationHeader(Reservation reservation)
        {
            var wiResHeader = new WIReservationHeader();
            wiResHeader.WishResHeader = reservation.Header;
            wiResHeader.IsNew = false;
            wiResHeader.Agents = new RangeObservableCollection<WIBookingAgent>();

            var wiResLegs = reservation.Legs.Select(x => (WIReservationLeg)x).ToList();
            wiResHeader.Legs.AddRange(wiResLegs);

            var wiResPaxList = reservation.Passengers.Select(x => (WIReservationPax)x).ToList();
            wiResHeader.PaxList.AddRange(wiResPaxList);

            return wiResHeader;
        }

    
    }
}
