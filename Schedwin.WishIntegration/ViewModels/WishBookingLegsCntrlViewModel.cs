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
using Schedwin.Tourplan;

namespace Schedwin.WishIntegration
{
    public class WishBookingLegsCntrlViewModel : Schedwin.Common.ViewModelBase
    {
        public int CountryID { get; set; }
        public int IDX_User { get; set; }

        public RangeObservableCollection<WIReservationLeg> BookingLegs { get; set; }
        public RangeObservableCollection<WIReservationLeg> Cancelledlegs { get; set; }

        public List<AirStripExFor> AllExForTimes { get; set; }


        public List<AirstripInfo> AllAirports { get; set; }

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

                 currLeg.ForList.Clear();
                currLeg.ExList.Clear();

                var items = AllExForTimes.Where(x => x.IDX_Airstrip == currLeg.WishResLeg.IDX_ToAP).OrderBy(x=>x.Description).ToList();
                currLeg.ForList.AddRange(items);

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


        public async Task Refresh(WICharterBooking wishBooking, WIReservationHeader resHeader)
        {
            if (UseGlobalDB)
                await RefreshGlobal(wishBooking,resHeader);
            else
                await RefreshRegional(wishBooking, resHeader);
        }
        public async Task RefreshRegional(WICharterBooking wishBooking, WIReservationHeader resHeader)
        {
            List<LegVoucherDetails> lstVouchers = null;
            Country thisCountry = Country.GetCountry(CountryID);
            int VATPercentage = 0;

            BookingLegs.Clear();
            Cancelledlegs.Clear();

            if (wishBooking != null)
            {
                IsRefreshing = true;

                if (!String.IsNullOrEmpty(resHeader.TPRef))
                {
                    var tpDBName = TPDBLookup.GetDBName(resHeader.TPRef);
                    if (!String.IsNullOrEmpty(tpDBName))
                    {
                        var refList = new List<String>();
                        refList.Add(resHeader.TPRef);

                        VATPercentage = thisCountry.VATPercentage ?? 0;
                        lstVouchers = await LegVoucherDetails.GetReservationVouchers(refList, VATPercentage, Server, tpDBName);
                        var nonA2AVouchers = lstVouchers.Where(x => x.AreaToArea == false).ToList();
                        foreach (var voucher in nonA2AVouchers)
                        {
                            var tpCode = TPRoutingCode.GetTPRoutingCode(voucher.VoucherCode);
                            if (tpCode != null)
                            {
                                voucher.IsSoleUse = tpCode.IsSoleUse;
                                voucher.AP1 = tpCode.AP1;
                                voucher.AP2 = tpCode.AP2;
                            }

                        }
                    }
                }

                //ResLegs comes from schedwin

                AllExForTimes = await CampExForAirstrips.GetExForListV2(Server, Database);
                LodgeAirports = await Lodge.LoadLodgeList(Server, Database, false);
                AllAirports = await AirstripInfo.LoadAirstrips(Server, Database);
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

                        var airstrpInfo = GetAirportIdentityFromCode(true, leg);
                        if (airstrpInfo != null)
                        {
                            leg.WishResLeg.FromAP = airstrpInfo.Code;
                            leg.WishResLeg.IDX_FromAP = airstrpInfo.IDX;
                        }
                    }
                    else if (leg.WishResLeg.IDX_FromAP < 1)
                    {
                        var airportInfo = AllAirports.FirstOrDefault(x => x.Code == leg.WishResLeg.FromAP && x.IsActive == true);
                        if (airportInfo != null)
                            leg.WishResLeg.IDX_FromAP = airportInfo.IDX;
                    }

                    if (String.IsNullOrEmpty(leg.WishResLeg.ToAP))
                    {

                        var airstrpInfo = GetAirportIdentityFromCode(false, leg);
                        if (airstrpInfo != null)
                        {                           
                            leg.WishResLeg.ToAP = airstrpInfo.Code;
                            leg.WishResLeg.IDX_ToAP = airstrpInfo.IDX;
                        }
                    }
                    else if  (leg.WishResLeg.IDX_ToAP<1)
                    {
                        var airportInfo = AllAirports.FirstOrDefault(x => x.Code == leg.WishResLeg.ToAP && x.IsActive == true);
                        if (airportInfo!=null)
                            leg.WishResLeg.IDX_ToAP = airportInfo.IDX;
                    }

                    UpdateLegExForLists(leg);
                    var charterleg = nonCancelledLegs.FirstOrDefault(x => x.SectorBookingID == leg.WishResLeg.WishSectorID);
                    if (charterleg != null)
                    {
                        if (!String.IsNullOrEmpty(leg.WishResLeg.FromAP) && !String.IsNullOrEmpty(leg.WishResLeg.ToAP) &&
                            lstVouchers!=null)
                        {
                            if (!charterleg.IsMultiLegFlight)
                            {
                                if (leg.WishResLeg.FromAP != null && leg.WishResLeg.ToAP != null)
                                {
                                    var exArea = AirstripInfo.GetAirstripInfo(leg.WishResLeg.FromAP).AreaCode;
                                    var forArea = AirstripInfo.GetAirstripInfo(leg.WishResLeg.ToAP).AreaCode;

                                    if (charterleg.SoleUse)
                                    {
                                        var voucher = lstVouchers.FirstOrDefault(x => x.IsSoleUse && x.Date==charterleg.BookingDate);
                                        if (voucher != null)
                                        {
                                            lstVouchers.Remove(voucher);
                                            leg.WishResLeg.Voucher = voucher.VoucherNo;
                                            leg.WishResLeg.VoucherAmount = voucher.VoucherAmount;
                                            leg.WishResLeg.VoucherCurrency = voucher.Currency;
                                        }
                                    }
                                    else
                                    {
                                        //first try area to area 
                                        var voucher = lstVouchers.FirstOrDefault(x => (x.Date == charterleg.BookingDate) &&
                                                                     ((x.AreaFrom == exArea && x.AreaTo == forArea) ||
                                                                     (x.AreaFrom == forArea && x.AreaTo == exArea)));

                                        if (voucher == null)
                                        {
                                            // no voucher found so try point to point

                                            voucher = lstVouchers.FirstOrDefault(x => (x.Date == charterleg.BookingDate) && (x.AreaToArea == false) &&
                                                                             ((x.AP1 == leg.WishResLeg.FromAP && x.AP2 == leg.WishResLeg.ToAP) ||
                                                                             (x.AP2 == leg.WishResLeg.FromAP && x.AP1 == leg.WishResLeg.ToAP)));
                                        }

                                        if (voucher != null)
                                        {
                                            lstVouchers.Remove(voucher);
                                            leg.WishResLeg.Voucher = voucher.VoucherNo;
                                            leg.WishResLeg.VoucherAmount = voucher.VoucherAmount;
                                            leg.WishResLeg.VoucherCurrency = voucher.Currency;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                var voucher = lstVouchers.FirstOrDefault(x => (x.Date == charterleg.BookingDate) && (x.AreaToArea == false) &&
                                                                       ((x.AP1 == charterleg.MultiLegFrom && x.AP2 == charterleg.MultiLegTo) ||
                                                                       (x.AP2 == charterleg.MultiLegTo && x.AP1 == charterleg.MultiLegFrom)));
                                if (voucher != null)
                                {
                                  
                                    leg.WishResLeg.Voucher = voucher.VoucherNo;
                                    leg.WishResLeg.VoucherAmount = voucher.VoucherAmount;
                                    leg.WishResLeg.VoucherCurrency = voucher.Currency;
                                }
                            }

                        }

                       
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

        public async Task RefreshGlobal(WICharterBooking wishBooking, WIReservationHeader resHeader)
        {
            List<LegVoucherDetails> lstVouchers = null;
            Country thisCountry = Country.GetCountry(CountryID);
            int VATPercentage = 0;

            BookingLegs.Clear();
            Cancelledlegs.Clear();

            if (wishBooking != null)
            {
                IsRefreshing = true;

                if (!String.IsNullOrEmpty(resHeader.TPRef))
                {
                    var tpDBName = TPDBLookup.GetDBName(resHeader.TPRef);
                    if (!String.IsNullOrEmpty(tpDBName))
                    {
                        var refList = new List<String>();
                        refList.Add(resHeader.TPRef);

                        VATPercentage = thisCountry.VATPercentage ?? 0;
                        lstVouchers = await LegVoucherDetails.GetReservationVouchers(refList, VATPercentage, Server, tpDBName);
                        var nonA2AVouchers = lstVouchers.Where(x => x.AreaToArea == false).ToList();
                        foreach (var voucher in nonA2AVouchers)
                        {
                            var tpCode = TPRoutingCode.GetTPRoutingCode(voucher.VoucherCode);
                            if (tpCode != null)
                            {
                                voucher.IsSoleUse = tpCode.IsSoleUse;
                                voucher.AP1 = tpCode.AP1;
                                voucher.AP2 = tpCode.AP2;
                            }

                        }
                    }
                }

                //ResLegs comes from schedwin

                AllExForTimes = await AirStripExFor.LoadExForList();
                LodgeAirports = await Lodge.LoadLodgeList(false);
                AllAirports = await AirstripInfo.LoadAirstrips();
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

                        var airstrpInfo = GetAirportIdentityFromCode(true, leg);
                        if (airstrpInfo != null)
                        {
                            leg.WishResLeg.FromAP = airstrpInfo.Code;
                            leg.WishResLeg.IDX_FromAP = airstrpInfo.IDX;
                        }
                    }
                    else if (leg.WishResLeg.IDX_FromAP < 1)
                    {
                        var airportInfo = AllAirports.FirstOrDefault(x => x.Code == leg.WishResLeg.FromAP && x.IsActive == true);
                        if (airportInfo != null)
                            leg.WishResLeg.IDX_FromAP = airportInfo.IDX;
                    }

                    if (String.IsNullOrEmpty(leg.WishResLeg.ToAP))
                    {

                        var airstrpInfo = GetAirportIdentityFromCode(false, leg);
                        if (airstrpInfo != null)
                        {
                            leg.WishResLeg.ToAP = airstrpInfo.Code;
                            leg.WishResLeg.IDX_ToAP = airstrpInfo.IDX;
                        }
                    }
                    else if (leg.WishResLeg.IDX_ToAP < 1)
                    {
                        var airportInfo = AllAirports.FirstOrDefault(x => x.Code == leg.WishResLeg.ToAP && x.IsActive == true);
                        if (airportInfo != null)
                            leg.WishResLeg.IDX_ToAP = airportInfo.IDX;
                    }

                    UpdateLegExForLists(leg);
                    var charterleg = nonCancelledLegs.FirstOrDefault(x => x.SectorBookingID == leg.WishResLeg.WishSectorID);
                    if (charterleg != null)
                    {
                        if (!String.IsNullOrEmpty(leg.WishResLeg.FromAP) && !String.IsNullOrEmpty(leg.WishResLeg.ToAP) &&
                            lstVouchers != null)
                        {
                            if (!charterleg.IsMultiLegFlight)
                            {
                                if (leg.WishResLeg.FromAP != null && leg.WishResLeg.ToAP != null)
                                {
                                    var exArea = AirstripInfo.GetAirstripInfo(leg.WishResLeg.FromAP).AreaCode;
                                    var forArea = AirstripInfo.GetAirstripInfo(leg.WishResLeg.ToAP).AreaCode;

                                    if (charterleg.SoleUse)
                                    {
                                        var voucher = lstVouchers.FirstOrDefault(x => x.IsSoleUse && x.Date == charterleg.BookingDate);
                                        if (voucher != null)
                                        {
                                            lstVouchers.Remove(voucher);
                                            leg.WishResLeg.Voucher = voucher.VoucherNo;
                                            leg.WishResLeg.VoucherAmount = voucher.VoucherAmount;
                                            leg.WishResLeg.VoucherCurrency = voucher.Currency;
                                        }
                                    }
                                    else
                                    {
                                        //first try area to area 
                                        var voucher = lstVouchers.FirstOrDefault(x => (x.Date == charterleg.BookingDate) &&
                                                                     ((x.AreaFrom == exArea && x.AreaTo == forArea) ||
                                                                     (x.AreaFrom == forArea && x.AreaTo == exArea)));

                                        if (voucher == null)
                                        {
                                            // no voucher found so try point to point

                                            voucher = lstVouchers.FirstOrDefault(x => (x.Date == charterleg.BookingDate) && (x.AreaToArea == false) &&
                                                                             ((x.AP1 == leg.WishResLeg.FromAP && x.AP2 == leg.WishResLeg.ToAP) ||
                                                                             (x.AP2 == leg.WishResLeg.FromAP && x.AP1 == leg.WishResLeg.ToAP)));
                                        }

                                        if (voucher != null)
                                        {
                                            lstVouchers.Remove(voucher);
                                            leg.WishResLeg.Voucher = voucher.VoucherNo;
                                            leg.WishResLeg.VoucherAmount = voucher.VoucherAmount;
                                            leg.WishResLeg.VoucherCurrency = voucher.Currency;
                                        }
                                    }

                                }
                            }
                            else
                            {
                                var voucher = lstVouchers.FirstOrDefault(x => (x.Date == charterleg.BookingDate) && (x.AreaToArea == false) &&
                                                                       ((x.AP1 == charterleg.MultiLegFrom && x.AP2 == charterleg.MultiLegTo) ||
                                                                       (x.AP2 == charterleg.MultiLegTo && x.AP1 == charterleg.MultiLegFrom)));
                                if (voucher != null)
                                {

                                    leg.WishResLeg.Voucher = voucher.VoucherNo;
                                    leg.WishResLeg.VoucherAmount = voucher.VoucherAmount;
                                    leg.WishResLeg.VoucherCurrency = voucher.Currency;
                                }
                            }

                        }


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

                        if (charterleg.Ex != leg.WishResLeg.WishEx)
                        {
                            leg.WishResLeg.WishEx = charterleg.Ex;
                            leg.HasChangedWishEx = true;
                        }
                        if (charterleg.For != leg.WishResLeg.WishFor)
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

        private AirstripInfo GetAirportIdentityFromCode (bool dirFrom, WIReservationLeg leg)
        {
            Lodge lodgeInfo = null;
            if (dirFrom)
            {
                lodgeInfo = LodgeAirports.FirstOrDefault(x => x.TPCode == leg.WishResLeg.ExCode);
                if (lodgeInfo == null)
                    return AllAirports.FirstOrDefault(x => x.TPCode == leg.WishResLeg.ExCode);
            }
             
            else
            {
                lodgeInfo = LodgeAirports.FirstOrDefault(x => x.TPCode == leg.WishResLeg.ForCode);
                if (lodgeInfo == null)
                    return AllAirports.FirstOrDefault(x => x.TPCode == leg.WishResLeg.ForCode);
            }
               

            if (lodgeInfo != null)
                return AllAirports.FirstOrDefault(x => x.IDX == lodgeInfo.IDX_Airstrip);
           

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
