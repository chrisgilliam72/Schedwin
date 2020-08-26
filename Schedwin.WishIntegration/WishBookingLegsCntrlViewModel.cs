using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.WishIntegration;
using Schedwin.Data;
using Schedwin.Data.Classes;
using Schedwin.WishIntegration.Classes;
using Schedwin.Reservations.Classes;
using Schedwin.Common;
using Telerik.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace Schedwin.WishIntegration
{
    public class WishBookingLegsCntrlViewModel : Schedwin.Common.ViewModelBase
    {
        public int IDX_User { get; set; }

        public String Server { get; set; }

        public String Database { get; set; }


        public RangeObservableCollection<WIReservationLeg> BookingLegs { get; set; }
        public RangeObservableCollection<WIReservationLeg> Cancelledlegs { get; set; }

        public List<AirStripExFor> AllExForTimes { get; set; }


        public List<tset_Airports> AllAirports { get; set; }

        public List<Lodge> LodgeAirports { get; set; }

        public WIReservationLeg SelectedLeg { get; set; }

        private WIReservationLeg _selectedCancelledLeg;

        public WIReservationLeg SelectedCancelledLeg
        {
            get
            {
                return _selectedCancelledLeg;
            }
            set
            {
                _selectedCancelledLeg = value;
                NotifyPropertyChanged("GridMenuEnabled");

            }
        }

        private bool IsRefreshing { get; set; }

        public bool GridMenuEnabled
        {
            get
            {
                return SelectedCancelledLeg != null ? true : false;
            }
        }
        public WishBookingLegsCntrlViewModel()
        {
            BookingLegs = new RangeObservableCollection<WIReservationLeg>();
            Cancelledlegs = new RangeObservableCollection<WIReservationLeg>();
            IsRefreshing = false;
        }



        public void UpdateLegExForLists(WIReservationLeg currLeg)
        {

            if (currLeg != null)
            {

                //var dummyTBC = new vl_CampFlightTimes();
                //dummyTBC.FlightNumberOrCamp = "TBA";

                //var dummyOther = new vl_CampFlightTimes();
                //dummyOther.FlightNumberOrCamp = "Other";
                currLeg.ForList.Clear();
                currLeg.ExList.Clear();

                var items = AllExForTimes.Where(x => x.IDX_Airstrip == currLeg.WishResLeg.IDX_ToAP).OrderBy(x=>x.Description).ToList();
                currLeg.ForList.AddRange(items);
                //currLeg.ForList.Add(dummyTBC);
                //currLeg.ForList.Add(dummyOther);



                items = AllExForTimes.Where(x => x.IDX_Airstrip == currLeg.WishResLeg.IDX_FromAP).OrderBy(x => x.Description).ToList();
                currLeg.ExList.AddRange(items);

            }

        }

        public void Clear()
        {
            BookingLegs.Clear();
            Cancelledlegs.Clear();
            NotifyPropertyChanged("BookingLegs");
            NotifyPropertyChanged("Cancelledlegs");
        }


        public void DeleteLeg()
        {
            if (SelectedCancelledLeg != null)
            {

                DialogParameters parameters = new DialogParameters();
                parameters.Header = "Delete Leg";
                parameters.Content = "Are you sure want to delete this leg";
                parameters.OkButtonContent = "Yes";
                parameters.CancelButtonContent = "No";
                parameters.Closed = OnDeleteLegConfirmed;
                RadWindow.Confirm(parameters);
               
            }

        }


        public void CancelLeg()
        {
            DialogParameters parameters = new DialogParameters();
            parameters.Header = "Delete Leg";
            parameters.Content = "Are you sure want to cancel this leg";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnCancelLegConfirmed;
            RadWindow.Confirm(parameters);
        }

        public async Task RefreshV2(WICharterBooking wishBooking, WIReservationHeader resHeader)
        {
        
            BookingLegs.Clear();
            Cancelledlegs.Clear();

            if (wishBooking != null)
            {
                IsRefreshing = true;

                //ResLegs comes from schedwin

                AllExForTimes = await CampExForAirstrips.GetExForListV2(Server, Database);
                LodgeAirports = await Lodge.LoadLodgeList(Server, Database,false);
                AllAirports = await CampExForAirstrips.GetAirportList(Server, Database);
                var nonCancelledLegs = wishBooking.Legs.Where(x => x.IsCancelled == false).ToList();
                var resLegs = resHeader.Legs.Where(x => x.WishResLeg.Canceled == false).ToList();


                foreach (var charterLeg in nonCancelledLegs)
                {
                    var resLeg = resLegs.FirstOrDefault(x => x.WishResLeg.WishSectorID == charterLeg.SectorBookingID);
                    if (resLeg == null)
                    {
                        var newResLeg = (WIReservationLeg)charterLeg;
                        resHeader.Legs.Add(newResLeg);
                        BookingLegs.Add(newResLeg);
                    }
                }

                resLegs = resHeader.Legs.Where(x => x.WishResLeg.Canceled == false).ToList();

                foreach (var leg in resLegs)
                {
                    if (String.IsNullOrEmpty(leg.WishResLeg.FromAP))
                    {
                        var airstrpInfo = GetAirportIdentityFromCampV2(true, leg);
                        if (airstrpInfo == null)
                            airstrpInfo = GetAirportIdentityFromAirportV2(true, leg);
                        if (airstrpInfo != null)
                        {
                            leg.WishResLeg.FromAP = airstrpInfo.Code;
                            leg.WishResLeg.IDX_FromAP = airstrpInfo.IDX;
                        }
                    }
                    else if (leg.WishResLeg.IDX_FromAP < 1)
                    {
                        var airportInfo = AllAirports.FirstOrDefault(x => x.IATA == leg.WishResLeg.FromAP && x.Active == true);
                        if (airportInfo != null)
                            leg.WishResLeg.IDX_FromAP = airportInfo.IDX;
                    }

                    if (String.IsNullOrEmpty(leg.WishResLeg.ToAP))
                    {
                        var airstrpInfo = GetAirportIdentityFromCampV2(false, leg);
                        if (airstrpInfo == null)
                            airstrpInfo = GetAirportIdentityFromAirportV2(false, leg);
                        if (airstrpInfo != null)
                        {                           
                            leg.WishResLeg.ToAP = airstrpInfo.Code;
                            leg.WishResLeg.IDX_ToAP = airstrpInfo.IDX;
                        }
                    }
                    else if  (leg.WishResLeg.IDX_ToAP<1)
                    {
                        var airportInfo = AllAirports.FirstOrDefault(x => x.IATA == leg.WishResLeg.ToAP && x.Active == true);
                        if (airportInfo!=null)
                            leg.WishResLeg.IDX_ToAP = airportInfo.IDX;
                    }

                    UpdateLegExForLists(leg);
                    var charterleg = nonCancelledLegs.FirstOrDefault(x => x.SectorBookingID == leg.WishResLeg.WishSectorID);
                    if (charterleg != null)
                    {
                                                        

                        if (charterleg.BookingDate != leg.WishResLeg.BookingDate)
                        {
                            leg.WishResLeg.BookingDate = charterleg.BookingDate;
                            leg.HasChangedBookingDate = true;
                        }

                        if (charterleg.ETA != leg.WishResLeg.ETA)
                        {
                            leg.WishResLeg.ETA = charterleg.ETA;
                            leg.HasChangedETA = true;
                        }

                        if (charterleg.ETD != leg.WishResLeg.ETD)
                        {
                            leg.WishResLeg.ETD = charterleg.ETD;
                            leg.HasChangedETD = true; 
                        }

                        if (charterleg.Ex!=leg.WishResLeg.WishEx)
                        {
                            leg.WishResLeg.WishEx = charterleg.Ex;
                            leg.HasChangedWishEx = true;
                        }
                        if (charterleg.For !=leg.WishResLeg.WishFor)
                        {
                            leg.WishResLeg.WishFor = charterleg.For;
                            leg.HasChangedWishFor = true;
                        }

                        if (charterleg.SectorNotes != leg.WishResLeg.Notes)
                        {
                            leg.WishResLeg.Notes = charterleg.SectorNotes;
                            leg.HasChangedNotes = true;
                        }

                        BookingLegs.Add(leg);
                    }                    
                    else
                    {

                        leg.State = WIReservationLeg.DBLegState.IsCancelled;
                        leg.WishResLeg.DBState = Reservations.DBState.IsCancelled;
                        Cancelledlegs.Add(leg);

                    }
     
                }

 
              
                IsRefreshing = false;

              
            }

            NotifyPropertyChanged("BookingLegs");
            NotifyPropertyChanged("Cancelledlegs");

        }



        private AirstripInfo GetAirportIdentityFromAirportV2(bool dirFrom, WIReservationLeg leg)
        {
            String airportName = "";
            if (dirFrom)
                airportName = leg.WishResLeg.WishEx;
            else
                airportName = leg.WishResLeg.WishFor;
            airportName = airportName.ToLower();


            if (airportName.Contains("international airport"))
            {
                int cmpIndex = airportName.IndexOf("international airport");
                airportName = airportName.Substring(0, cmpIndex);
                airportName = airportName.TrimEnd(' ');
            }

            if (airportName.Contains("airport"))
            {
                int cmpIndex = airportName.IndexOf("airport");
                airportName = airportName.Substring(0, cmpIndex);
                airportName = airportName.TrimEnd(' ');
            }

            if (airportName.Contains("airstrip"))
            {
                int cmpIndex = airportName.IndexOf("airstrip");
                airportName = airportName.Substring(0, cmpIndex);
                airportName = airportName.TrimEnd(' ');
            }


            var airportInfo = AllAirports.FirstOrDefault(x => x.Airport == airportName && x.Active == true);
            if (airportInfo != null)
            {
                var airstrpInfo = new AirstripInfo();
                airstrpInfo.IDX = airportInfo.IDX;
                airstrpInfo.Code = airportInfo.IATA;
                return airstrpInfo;
            }
            else
                return null;

        }

        private AirstripInfo GetAirportIdentityFromCampV2(bool dirFrom, WIReservationLeg leg)
        {
            String campName = "";
            DateTime travelDate = leg.WishResLeg.BookingDate;
            if (dirFrom)
                campName = leg.WishResLeg.WishEx;
            else
                campName = leg.WishResLeg.WishFor;

            if (campName.Contains("("))
            {
                int brkIndex = campName.IndexOf("(");
                campName = campName.Substring(0, brkIndex);
            }
            if (campName.Contains("Camp"))
            {
                int cmpIndex = campName.IndexOf("Camp");
                campName = campName.Substring(0, cmpIndex);
            }
            campName = campName.TrimEnd(' ');

            var lodgeInfo = LodgeAirports.FirstOrDefault(x => x.Name == campName);
           
            if (lodgeInfo != null)
            {
                var airstrpInfo = new AirstripInfo();
                airstrpInfo.IDX = lodgeInfo.IDX_Airstrip;
                airstrpInfo.Code = lodgeInfo.AirstripIATA;
                return airstrpInfo;
            }

            return null;
        }


        private async void OnCancelLegConfirmed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult.HasValue && e.DialogResult == true)
            {
                if (SelectedCancelledLeg.WishResLeg.IsScheduled)
                    FailWindow.Display("This has already been scheduled and can not be deleted.");
                else
                {
                    var res = new Schedwin.Reservations.Classes.Reservations();
                   var result= await res.CancelLeg(SelectedCancelledLeg.WishResLeg.IDX, Server, Database);
                    if (result)
                    {
                        SuccessWindow.Display("Leg cancelled");
                        Cancelledlegs.Remove(SelectedCancelledLeg);
                        NotifyPropertyChanged("CancelledLegs");

                    }

                    else
                        FailWindow.Display("Unable to cancel leg:\r\n" + res.LastError);

                }
                    
            }


        }

        private async void OnDeleteLegConfirmed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult.HasValue && e.DialogResult==true)
            {
                if (SelectedCancelledLeg.WishResLeg.IsScheduled)
                    RadWindow.Alert("This has already been scheduled and can not be deleted.");
                else
                {
                    using (new StackedCursorOverride(Cursors.Wait))
                    {
                        var res = new Schedwin.Reservations.Classes.Reservations();
                        var result= await res.DeleteLeg(SelectedCancelledLeg.WishResLeg.IDX, Server, Database);
                        if (result)
                        {
                            SuccessWindow.Display("Leg deleted");
                            Cancelledlegs.Remove(SelectedCancelledLeg);
                            NotifyPropertyChanged("CancelledLegs");
                        }
                            
                        else
                        {
                            FailWindow.Display("Unable to delete leg :\r\n"+res.LastError);
                        }
                    }

                }
            }

            
        }
    }
}
