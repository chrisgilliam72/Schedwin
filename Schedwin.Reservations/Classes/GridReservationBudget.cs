using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class GridReservationBudget : ViewModelBase 
    {
        public DateTime Date { get; set; }

        public ReservationLegBudget LegBudget { get; set; }

        public static List<String> RateTypes { get; set; }
        public static List<AirstripInfo> AirportList { get; set; }

        public static List<AircraftType> ACTypeList { get; set; }

        public static List<Currency> CurrencyList { get; set; }

        public static int PaxCount { get; set; }

        public int IDX_AC_Type
        {
            get
            {
                return LegBudget.IDX_AC_Type;
            }
            set
            {
                LegBudget.IDX_AC_Type = value;
                NotifyPropertyChanged("IDX_AC_Type");
            }
        }

        public double Budget
        {
            get
            {
                return LegBudget.Budget;
            }
            set
            {
                LegBudget.Budget = value;
                NotifyPropertyChanged("Budget");
            }

        }

        public double Rate
        {
            get
            {
                return LegBudget.Rate;
            }
            set
            {
                LegBudget.Rate = value;
                Budget = Qty * Rate;
                NotifyPropertyChanged("Budget");
            }
        }

        public double Qty
        {
            get
            {
                return LegBudget.Qty;
            }
            set
            {
                LegBudget.Qty = value;
                NotifyPropertyChanged("Qty");
            }
        }

        public String RateType
        {
            get
            {
                return LegBudget.RateType;
            }
            set
            {

                LegBudget.RateType = value;
                if (value == "Seat" || value == "DEP Tax")
                    Qty = PaxCount;
                else
                    Qty = 1;

                NotifyPropertyChanged("Qty");
            }
        }

        public String ToAP
        {
            get
            {
                return LegBudget.ToAP;
            }
            set
            {
                LegBudget.ToAP = value;
            }
        }

        public int IDX_To
        {
            get
            {
                return LegBudget.IDX_To;
            }
            set
            {
                LegBudget.IDX_To = value;
                LegBudget.ToAP = GetAPCode(value);
                NotifyPropertyChanged("ToAP");
            }
        }

        public String FromAP
        {
            get
            {
                return LegBudget.FromAP;
            }
            set
            {
                LegBudget.FromAP = value;
            }
        }

        public int IDX_From
        {
            get
            {
                return LegBudget.IDX_From;
            }

            set
            {
                LegBudget.IDX_From = value;
                LegBudget.FromAP = GetAPCode(value);
                NotifyPropertyChanged("FromAP");
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

        public static implicit operator GridReservationBudget(ReservationLegBudget legBudget)
        {
            var newGridBudget = new GridReservationBudget();
            newGridBudget.LegBudget = legBudget;
            return newGridBudget;
        }
    }
}
