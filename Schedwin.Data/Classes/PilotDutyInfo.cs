using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Common;
using System.Windows.Media;

namespace Schedwin.Data.Classes
{
    public class DutyItem : ViewModelBase
    {

        public PilotDutyPeriod DutyPeriodParent { get; set; }
        public DateTime  ActualDate { get; set; }
        private string _DutyType;
        public String DutyType
        {
            get
            {
                return _DutyType;
            }
            set
            {
                _DutyType = value;
                NotifyPropertyChanged("Color");
            }
        }

        public Brush Color
        {
            get
            {
                var dutyType = DutyPeriodParent.DutyTypes.FirstOrDefault(x => x.Code == DutyType);
                if (dutyType != null)
                {
                   
                    BrushConverter conv = new BrushConverter();
                    return conv.ConvertFromString(dutyType.ColorName) as SolidColorBrush;
                }
                else
                    return Brushes.Transparent;
            }
        }


        public bool ReadOnly
        {
            get
            {
                return !CanRoster;
            }
        }
        public bool CanRoster
        {
            get
            {
                return DutyType != "E";
            }
        }

        public DutyItem()
        {
            _DutyType = "1";
        }
    }


    public class PilotDutyPeriod : ViewModelBase
    {

        public int IDX { get; set; }
        public String FirstName { get; set; }
        public String Surname { get; set; }

        public DateTime LicenceExpiry { get; set; }
        public DateTime DutyStart { get; set; }

        public    RangeObservableCollection<RosterDutyType> DutyTypes { get; set; }


        public String PilotFullName
        {
            get
            {
                return FirstName + " " + Surname;
            }
        }



        private RangeObservableCollection<DutyItem> _dailyDutyTypeList;
        public RangeObservableCollection<DutyItem> DailyDutyTypeList
        {
            get
            {
                return _dailyDutyTypeList;
            }
            set
            {
                _dailyDutyTypeList = value;
            }
        }


        public static explicit operator PilotDutyPeriod(tbPilot tbPilot)
        {
            var pilotDutyPeriod = new PilotDutyPeriod();
            pilotDutyPeriod.IDX = tbPilot.pkPilotID;
            pilotDutyPeriod.FirstName = tbPilot.tbUser.Firstname;
            pilotDutyPeriod.Surname = tbPilot.tbUser.Surname;
            pilotDutyPeriod.LicenceExpiry = tbPilot.PilotsLicenseExpiryDate;
            return pilotDutyPeriod;
        }


        public void Init(int DaysInMonth)
        {
           
            
            for (int i = 0; i <= DaysInMonth; i++)
            {
                var actualDate = DutyStart.AddDays(i);
                if (actualDate <= LicenceExpiry)
                    _dailyDutyTypeList.Add(new DutyItem { DutyType = "1" , ActualDate=actualDate, DutyPeriodParent = this });
                else
                    _dailyDutyTypeList.Add(new DutyItem { DutyType = "E", ActualDate = actualDate, DutyPeriodParent = this });
            }
        }
        public void CopyFrom(PilotDutyPeriod src)
        {

            for (int i = 0; i < 31; i++)
            {
                if (src.DailyDutyTypeList[i].CanRoster)
                     _dailyDutyTypeList[i].DutyType = src.DailyDutyTypeList[i].DutyType;
            }

            NotifyPropertyChanged("DailyDutyTypeList");
        }

        public void RefreshMatrix()
        {
            NotifyPropertyChanged("DailyDutyTypeList");
        }

        public PilotDutyPeriod()
        {
            _dailyDutyTypeList = new RangeObservableCollection<DutyItem>();

           

        }
     
    }
}
