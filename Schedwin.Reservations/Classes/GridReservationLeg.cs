using Schedwin.Common;
using Schedwin.Data;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.Reservations.Classes
{
    public class GridReservationLeg : ViewModelBase
    {

        public bool IsSelected { get; set; }

        public Brush RowColor
        {
            get
            {
                if (Leg!=null)
                {
                    if (Leg.Canceled)
                        return Brushes.Red;
                    return Leg.IsScheduled ? Brushes.LightGreen : null;
                }

                return null;
            }
        }
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                NotifyPropertyChanged("IsReadOnly");
            }
        }
        public static List<AirStripExFor> AllExForList { get; set; }

        public  RangeObservableCollection<AirStripExFor> ExList
        {
            get
            {
                var tmpCollection = new RangeObservableCollection<AirStripExFor>();
                var tmplst = AllExForList.Where(x => x.IDX_Airstrip == IDX_FromAP).ToList();
                tmpCollection.AddRange(tmplst);
                return tmpCollection;

            }
        }

        public RangeObservableCollection<AirStripExFor> ForList
        {
            get
            {
                var tmpCollection = new RangeObservableCollection<AirStripExFor>();
                var tmplst = AllExForList.Where(x => x.IDX_Airstrip == IDX_ToAP).ToList();
                tmpCollection.AddRange(tmplst);
                return tmpCollection;

            }
        }
        public static List<AirstripInfo> AirportList { get; set; }
        public ReservationLeg  Leg { get;set;}

        public GridReservationLeg()
        {
            IsReadOnly = true;
        }

        public String ToAP
        {
            get
            {
                return Leg.ToAP;
            }
            set
            {
                Leg.ToAP = value;
            }
        }

        public String FromAP
        {
            get
            {
                return Leg.FromAP;
            }

            set
            {
                Leg.FromAP = value;
            }
        }

        public int IDX_FromAP
        {
            get
            {
                return Leg.IDX_FromAP;
            }

            set
            {
                Leg.IDX_FromAP = value;
                Leg.FromAP = GetAPCode(value);
                NotifyPropertyChanged("ExList");
                NotifyPropertyChanged("FromAP");
            }
        }

        public int IDX_ToAP
        {
            get
            {
                return Leg.IDX_ToAP;
            }
            set
            {
                Leg.IDX_ToAP = value;
                Leg.ToAP= GetAPCode(value);
                NotifyPropertyChanged("ForList");
                NotifyPropertyChanged("ToAP");
            }
        }

        public int Distance
        {
            get
            {
                return Leg.Distance;
            }
            set
            {
                Leg.Distance = value;
                NotifyPropertyChanged("Distance");
            }
        }

        private String GetAPCode(int idx)
        {
            if (AirportList != null)
            {
                var airport = AirportList.FirstOrDefault(x => x.IDX == idx);
                if (airport != null)
                  return airport.Code;        
            }

            return "";
        }

        public void Remove()
        {
            Leg.Deleted = true;
        }

        public void Cancel()
        {
            Leg.Canceled = true;
            NotifyPropertyChanged("RowColor");
        }

        public void Restore()
        {
            Leg.Canceled = false;
            NotifyPropertyChanged("RowColor");
        }

        public async void UpdateReadOnlyStatus(DateTime flightDate,String Server,String Database)
        {
            String userName = "";
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {
                userName = await ScheduleInUse.FlightDateInUse(flightDate, Server, Database);
            }

            if (!String.IsNullOrEmpty(userName))
            {
                FailWindow.Display("Leg currently being modified in schedule by user:" + userName);
                IsReadOnly = true;
            }
            else if (Leg.IsScheduled)            
            {
                IsReadOnly = true;
            }
            else
                IsReadOnly = false;
           
        }


        public static implicit operator GridReservationLeg(ReservationLeg resLeg)
        {
            var newGridLeg = new GridReservationLeg();
            newGridLeg.Leg = resLeg;
            return newGridLeg;
        }

    }

}
