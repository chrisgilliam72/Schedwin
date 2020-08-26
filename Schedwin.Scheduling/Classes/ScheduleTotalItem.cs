using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling.Classes
{
   public class ScheduleTotalItem : ViewModelBase
    {
        private String _description;
        public String Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private double _thisAircaft;
        public double ThisAircraft
        {
            get
            {
                return _thisAircaft;
            }
            set
            {
                _thisAircaft = value;
                NotifyPropertyChanged("ThisAircraft");
            }
        }

        private double _allAircraft;
        public double AllAircraft
        {
            get
            {
                return _allAircraft;
            }
            set
            {
                _allAircraft = value;
                NotifyPropertyChanged("AllAircraft");
            }
        }
    }
}
