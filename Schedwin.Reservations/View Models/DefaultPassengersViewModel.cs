using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations
{
    public class DefaultPassengersViewModel : ViewModelBase
    {
        public int CountryID { get; set; }
        public int NoPax { get; set; }

        public int DefaultAge { get; set; }
        public int DefaultWeight { get; set; }

        public int DefaultLuggageWeight { get; set; }
        public ObservableCollection<String> Genders { get; set; }

        public String _selectedGender;
        public String SelectedGender
        {
            get
            {
                return _selectedGender;
            }
            set
            {
                _selectedGender = value;
                DefaultWeight = StandardPassengerWeights.GetStandardWeight(CountryID, SelectedGender == "Male", DefaultAge > 12);
                NotifyPropertyChanged("DefaultWeight");
            }

        }
        public DefaultPassengersViewModel()
        {
            Genders = new ObservableCollection<string>();
            Genders.Add("Male");
            Genders.Add("Female");
        }

        public void Init()
        {
            NoPax = 1;
            DefaultAge = 30;
            DefaultLuggageWeight = 20;
            DefaultWeight = 175;
            SelectedGender = "Male";

            NotifyPropertyChanged("SelectedGender");
            NotifyPropertyChanged("Genders");
            NotifyPropertyChanged("DefaultAge");
            NotifyPropertyChanged("DefaultWeight");
            NotifyPropertyChanged("DefaultLuggageWeight");
            NotifyPropertyChanged("NoPax");
        }
    }
}
