using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class PaxRowSeatAssignment :ViewModelBase
    {
        public WeightBalancePositionItem ParentPositionItem { get; set; }

        public int IDX { get; set; }
        public int IDX_Row { get; set; }

        private int _weight;
        public int Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                NotifyPropertyChanged("Weight");
            }
        }

        private String _gender;
        public String Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
                NotifyPropertyChanged("Gender");
            }
        }

        public bool IsMale
        {
            get
            {
                return Gender == "M" || Gender == "m";
            }
        }

        public bool IsFemale
        {
            get
            {
                return Gender == "F" || Gender == "f";
            }
        }

        public bool IsPilot
        {
            get
            {
                return Gender=="P" || Gender=="p";
            }
        }

        private bool _Show;
        public bool Show
        {
            get
            {
                return _Show;
            }
            set
            {
                _Show = value;
                NotifyPropertyChanged("Show");
            }
        }

        private String _Seating;
        public String Seating
        {
            get
            {
                return _Seating;
            }
            set
            {
                _Seating = value;
                NotifyPropertyChanged("Seating");
            }
        }
        public PaxRowSeatAssignment(bool defaultShow)
        {
            Show = defaultShow;
            Gender = "";
        }

        public PaxRowSeatAssignment()
        {
            Show = false;
            Gender = "";
        }
    }
}
