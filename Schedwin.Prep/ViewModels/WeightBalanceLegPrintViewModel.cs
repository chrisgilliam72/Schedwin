using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class WeightBalanceLegPrintViewModel :ViewModelBase 
    {
        private bool _isvisible;
        public bool IsVisible
        {
            get
            {
                return _isvisible;
            }
            set
            {
                _isvisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }

        private String _title;
        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private RangeObservableCollection<WeightBalancePositionItem> _LastChangesRows;
        public RangeObservableCollection<WeightBalancePositionItem> LastChangesRows
        {
            get
            {
                return _LastChangesRows;
            }
            set
            {
                _LastChangesRows = value;
                NotifyPropertyChanged("LastChangesRows");
            }
        }


        private RangeObservableCollection<WeightBalancePositionItem> _rows;
        public RangeObservableCollection<WeightBalancePositionItem> Rows
        {
            get
            {
                return _rows;
            }
            set
            {
                _rows = value;
                NotifyPropertyChanged("Rows");
            }
        }


        public C208ArmGraphControlViewModel GraphControlViewModel { get; set; }

        public WeightBalanceLegPrintViewModel()
        {
            IsVisible = false;
            Rows = new RangeObservableCollection<WeightBalancePositionItem>();
            LastChangesRows = new RangeObservableCollection<WeightBalancePositionItem>()
            { new WeightBalancePositionItem(0,"Change +/-"),  new WeightBalancePositionItem(0,"New Ramp Weight"),  new WeightBalancePositionItem(0,"New Land Weight"),
             new WeightBalancePositionItem(0,"Signature"), new WeightBalancePositionItem(0,"Land fuel"), new WeightBalancePositionItem(0,"Uplift Fuel")
            };
        }

        public void Refresh(WeightBalanceLeg Leg)
        {
            Rows.Clear();
            Rows .AddRange( Leg.RowItems);
            if (GraphControlViewModel != null && Rows != null && Rows.Count>0)
            {
                var takeOffRow = Rows.First(x => x.Name == "T/Off Weight");
                var landingRow = Rows.First(x => x.Name == "Land Weight");
                GraphControlViewModel.Refresh(takeOffRow.Weight, takeOffRow.Arm, landingRow.Weight, landingRow.Arm);
                NotifyPropertyChanged("Rows"); ;
            }
            Title = Leg.DisplayValue;

        }
    }
}
