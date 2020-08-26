using Schedwin.Common;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class PaxRowAssignementViewModel : ViewModelBase
    {

        private int _additionalFreight;
        public int AdditionalFreight
        {
            get
            {
                return _additionalFreight;
            }
            set
            {
                _additionalFreight = value;
                NotifyPropertyChanged("AdditionalFreight");
            }
        }

        public WeightBalanceLegsViewModel WeightBalanceLegViewModel { get; set; }
        public List<ScheduleLegRes> ResList { get; set; }

        public WeightBalancePositionItem RowPositionItem { get; set; }

        public int MaxSeats
        {
            get
            {
                if (RowPositionItem!=null)
                    return RowPositionItem.MaxSeats;

                return 0;
            }
        }
        public int TotalPax
        {
            get
            {
                if (ResList!=null)
                {
                    var paxList= ResList.SelectMany(x => x.PaxList).ToList();
                    return paxList.Count();
                }
                return 0;
            }
        }

        public int TotalWomen
        {
            get
            {
                if (ResList!=null)
                {
                    var paxList = ResList.SelectMany(x => x.PaxList).Where(x => x.IsMale == false).ToList();
                    return paxList.Count();
                }
                return 0;
            }
        }

        public int TotalMen
        {
            get
            {
                if (ResList != null)
                {
                    var paxList = ResList.SelectMany(x => x.PaxList).Where(x => x.IsMale == true).ToList();
                    return paxList.Count();
                }
                return 0;
            }
        }


        public int TotalUnassignedPax
        {
            get
            {
               if (RowPositionItem!=null)
                   return TotalPax- WeightBalanceLegViewModel.TotalAssignedPax;

                return 0;
            }
            
        }


        public int TotalUnassignedMen
        {
            get
            {
                if (RowPositionItem != null)
                    return TotalMen - WeightBalanceLegViewModel.TotalAssignedMen;

                return 0;
            }
        }



        public int TotalUnassignedWomen
        {
            get
            {
                if (RowPositionItem != null)
                    return TotalWomen - WeightBalanceLegViewModel.TotalAssignedWomen;

                return 0;
            }
 
        }

        public int RowWeight
        {
            get
            {
                int totalWeight = 0;
                if (RowPositionItem!=null)
                {
                    int weightMen = RowPositionItem.PaxSeatAssignments.Where(x => x.IsMale).Count() * WeightBalanceLegViewModel.MaleWeight;
                    int weightWomen = RowPositionItem.PaxSeatAssignments.Where(x => x.IsFemale).Count() * WeightBalanceLegViewModel.FemaleWeight;
                
                    totalWeight = weightMen + weightWomen+ RowPositionItem.PilotWeight;
                }
                return totalWeight;
            }
        }

        private String _RowDetails;
        public String RowDetails
        {
            get
            {
                return _RowDetails;
            }
            set
            {
                _RowDetails = value;
                NotifyPropertyChanged("RowDetails");
            }
        }

        public void UpdatePaxTotals()
        {
            Refresh();
        }

        public void Clear()
        {
            RowPositionItem.PaxSeatAssignments.ToList().ForEach(x => x.Gender = "");
            AdditionalFreight = 0;
      
        }
     
        public bool Valid()
        {
           
            if (TotalUnassignedMen < 0)
            {
                FailWindow.Display("Too many men have been assigned.");
                return false;
            }
            if (TotalUnassignedWomen < 0)
            {
                FailWindow.Display("Too many women have been assigned.");
                return false;
            }

            return true;
        }

        public void Refresh()
        {
            NotifyPropertyChanged("MaxSeats");
            NotifyPropertyChanged("RowPositionItem");
            NotifyPropertyChanged("TotalMen");
            NotifyPropertyChanged("TotalWomen");
            NotifyPropertyChanged("TotalPax");
            NotifyPropertyChanged("TotalUnassignedMen");
            NotifyPropertyChanged("TotalUnassignedWomen");
            NotifyPropertyChanged("TotalUnassignedPax");
          
        }
    }
}
