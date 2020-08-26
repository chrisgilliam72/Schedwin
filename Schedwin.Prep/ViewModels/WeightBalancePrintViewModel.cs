using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
   public class WeightBalancePrintViewModel : ViewModelBase
    {
        private String _pageText;
        public String PageText
        {
            get
            {
                return _pageText;
            }
            set
            {
                _pageText = value;
                NotifyPropertyChanged("PageText");
            }
        }

        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get
            {
                double totalLegs = Legs.Count();
                return  Convert.ToInt32(Math.Ceiling(totalLegs / 4.0));

            }
        }

        private string _pilot;
        public String Pilot
        {
            get
            {
                return _pilot;
            }
            set
            {
                _pilot = value;
                NotifyPropertyChanged("Pilot");
            }
        }

        private String _aircraft;
        public String Aircraft
        {
            get
            {
                return _aircraft;
            }
            set
            {
                _aircraft = value;
                NotifyPropertyChanged("Aircraft");
            }
        }

        private String _aircraftType;
        public String AircraftType
        {
            get
            {
                return _aircraftType;
            }
            set
            {
                _aircraftType = value;
                NotifyPropertyChanged("AircraftType");
            }
        }

        private String _DOF;
        public String DOF
        {
            get
            {
                return _DOF;
            }
            set
            {
                _DOF = value;
                NotifyPropertyChanged("DOF");
            }
        }
        
        public int LegCount { get; set; }

        public List<WeightBalanceLeg> Legs { get; set; }

        public WeightBalanceLegPrintViewModel[] legViewModels { get; set; }

        private bool _showButton;
        public bool ShowButton
        {
            get
            {
                return _showButton;
            }
            set
            {
                _showButton = value;
                NotifyPropertyChanged("ShowButton");
            }
        }

        public WeightBalancePrintViewModel()
        {
            Legs = new List<WeightBalanceLeg>();
            legViewModels = new WeightBalanceLegPrintViewModel[4];
            LegCount = 0;
            ShowButton = true;
        }


        public void Init(WeightBalanceLegPrintViewModel viewModel1, WeightBalanceLegPrintViewModel viewModel2, 
                         WeightBalanceLegPrintViewModel viewModel3, WeightBalanceLegPrintViewModel viewModel4)
        {
            legViewModels[0] = viewModel1;
            legViewModels[1] = viewModel2;
            legViewModels[2] = viewModel3;
            legViewModels[3] = viewModel4;
        }

        public void Reset()
        {
            legViewModels[0].IsVisible = false;
            legViewModels[1].IsVisible = false;
            legViewModels[2].IsVisible = false;
            legViewModels[3].IsVisible = false;
        }

        public void Refresh()
        {
            FirstPage();
        }
        public void AddLeg(List<WeightBalancePositionItem> rows,String From, String To, String Tet)
        {
            var wbLegItem = new WeightBalanceLeg();
            wbLegItem.RowItems.AddRange(rows);
            wbLegItem.FromAP = From;
            wbLegItem.ToAP = To;
            wbLegItem.TET = Tet;
            wbLegItem.LegNo = ++LegCount;
            Legs.Add(wbLegItem);
        }

        public void RefreshPage(int pageNo)
        {
            int startIndex = 4 * (pageNo - 1);
            int endIndex = (4 * pageNo);

            if (endIndex > Legs.Count)
                endIndex =Legs.Count;

            var sublist = Legs.GetRange(startIndex, endIndex - startIndex);

            DisplayPage(sublist);
        }

        public void DisplayPage(List<WeightBalanceLeg> legs)
        {
            int index = 0;
            Reset();

            foreach (var leg in legs)
            {
                legViewModels[index].IsVisible = true;
                legViewModels[index++].Refresh(leg);
            }
        }

        public void NextPage()
        {
            if (CurrentPage < TotalPages)
                RefreshPage(++CurrentPage);
        }

        public void PreviousPage()
        {
            if (CurrentPage > 1)
                RefreshPage(--CurrentPage);
        }

        public void FirstPage()
        {
            RefreshPage(1);
            CurrentPage = 1;
        }

        public void LastPage()
        {
            RefreshPage(TotalPages);
            CurrentPage = TotalPages;
        }
    }
}
