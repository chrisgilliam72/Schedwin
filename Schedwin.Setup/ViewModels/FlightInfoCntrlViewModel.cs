using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Setup;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class FlightInfoCntrlViewModel : ItemCntrlViewModelBase
    {
 
        public String FlightDescription { get; set; }
        public DateTime PickUpDropOff { get; set; }
        public DateTime ArrivalDepart { get; set; }
        public bool IsInbound { get; set; }

        public bool IsOutbound { get; set; }

        public bool IsActive { get; set; }
        public AirstripInfo SelectedAirstrip { get; set; }




        public RangeObservableCollection<AirstripInfo> AirstripList { get; set; }
        
        public FlightInfoCntrlViewModel()
        {
            AirstripList = new RangeObservableCollection<AirstripInfo>();
        }

        public override void Init()
        {
            IsActive = true;
            RefreshAirstripList();
            NotifyPropertyChanged("IsActive");
        }

        public override bool Validate()
        {
            var invalidFieldLst = new List<String>();
            if (SelectedAirstrip==null)
            {
                invalidFieldLst.Add("Airstrip");
            }

            if (String.IsNullOrEmpty(FlightDescription))
            {
                invalidFieldLst.Add("Flight description");
            }

            if (IsInbound==false && IsOutbound==false)
            {
                invalidFieldLst.Add("Either inbound or outbound must be selected");
            }

            if (invalidFieldLst.Count > 0)
            {
                String validFailMess = "The following fields need to be completed:" + Environment.NewLine;
                String strfieldList = String.Join(Environment.NewLine, invalidFieldLst);
                validFailMess += strfieldList;

                FailWindow.Display(validFailMess);
                return false;

            }

            return true;
        }

        public void Refresh(FlightInfo flight)
        {
           
            RefreshAirstripList();
            IsNew = false;
            IDX = flight.IDX;
            FlightDescription = flight.Description;
            SelectedAirstrip = AirstripList.FirstOrDefault(x => x.IDX == flight.IDX_Airstrip);
            ArrivalDepart = flight.ArrivalDepartTime;
            PickUpDropOff = flight.PickupDropOff;
            IsInbound = flight.IsInbound ? true : false;
            IsOutbound = !flight.IsInbound ? true : false;
            IsActive = flight.IsActive;

            NotifyPropertyChanged("ArrivalDepart");
            NotifyPropertyChanged("PickUpDropOff");
            NotifyPropertyChanged("FlightDescription");
            NotifyPropertyChanged("SelectedAirstrip");
            NotifyPropertyChanged("IsInBound");
            NotifyPropertyChanged("IsOutBound");

            NotifyPropertyChanged("IsActive");
        }

        public override async Task<bool> Save()
        {
            try
            {
                var flightInfo = new FlightInfo();
                flightInfo.IsNew = IsNew;
                flightInfo.IDX = IDX;
                flightInfo.IDX_Airstrip = SelectedAirstrip.IDX;
                flightInfo.Description = FlightDescription;
                flightInfo.PickupDropOff = PickUpDropOff;
                flightInfo.ArrivalDepartTime = ArrivalDepart;
                flightInfo.IsInbound = IsInbound;
                flightInfo.IsActive = IsActive;

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await flightInfo.Save(Server, Database);
                }

                FlightInfo.UpdateCachedObject(flightInfo);

            }

            catch (Exception ex)
            {
                FailWindow.Display("Unable to save fligt data :" + Environment.NewLine + ex);
                return false;
            }
            return true;
        }

        private void RefreshAirstripList()
        {
            AirstripList.Clear();

            var airstrips = AirstripInfo.GetAirstrips();


            if (airstrips != null)
            {
                AirstripList.AddRange(airstrips);
                NotifyPropertyChanged("AirstripList");
            }
        }
    }
}
