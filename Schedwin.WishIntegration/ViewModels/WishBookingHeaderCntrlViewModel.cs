
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.WishIntegration.Classes;
using Schedwin.Data;
using Schedwin.Data.Classes;
using Schedwin.Common;
using Telerik.Windows.Controls;
using System.Windows.Input;
using System.Data.Entity.Validation;

namespace Schedwin.WishIntegration
{
    
    public class WishBookingHeaderCntrlViewModel : Schedwin.Common.ViewModelBase
    {
        public int CountryID { get; set; }
        public int IDX_Company { get; set; }
        public int IDX_User { get; set; }

        public int WishPrincipalID { get; set; }
        public int WishPrincipalSoleUseID { get; set; }
        public RemovedWishBookingHeaderCntrlViewModel RemovedLegsViewModel { get; set; }

        public WishIntegrationUIViewModel UIViewModel { get; set; }
        public WishBookingLegsCntrlViewModel LegViewModel { get; set; }

        private List<WIReservationHeader> _currentSearchLst;

        private RangeObservableCollection<WIReservationHeader> _bookingHeaders;

        public RangeObservableCollection<WIReservationHeader> BookingHeaders
        {
            get
            {
                return _bookingHeaders;
            }
            set
            {
                _bookingHeaders = value;

            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool CanRemove
        {
            get
            {
                if (SelectedBooking != null && SelectedBooking.WishResHeader.ReservationStatus!=null)
                    return true;
                else
                    return false;
            }
        }

        public bool CanUpdate
        {
            get
            {
                if (SelectedBooking != null)
                    return true;
                else
                    return false;
            }
        }

        public bool CanRefresh
        {
            get
            {
                if (SelectedBooking != null)
                    return true;
                else
                    return false;
            }
        }
        



        private RangeObservableCollection<WIBookingCompany> _operators;
        public RangeObservableCollection<WIBookingCompany> Operators
        {
            get
            {
                return _operators;
            }
            set
            {
                _operators = value;
            }
        }

        private WIReservationHeader _selectedBooking;
        public WIReservationHeader SelectedBooking
        {
            get
            {
                return _selectedBooking;
            }
            set
            {
                _selectedBooking = value;
            }
        }

        private List<WICharterBooking> WishBookings { get; set; }


        private List<WIReservationHeader> RemovedHeaders { get; set; }

        private List<WIReservationLeg> AllWishLegs { get; set; }

        public WishBookingHeaderCntrlViewModel()
        {

            Operators = new RangeObservableCollection<WIBookingCompany>();
            BookingHeaders = new RangeObservableCollection<WIReservationHeader>();
            RemovedHeaders = new List<WIReservationHeader>();
            WishBookings = new List<WICharterBooking>();
            AllWishLegs = new List<WIReservationLeg>();
            _currentSearchLst = new List<WIReservationHeader>();
            StartDate = DateTime.Today;
            EndDate = DateTime.Today.AddDays(7);

            NotifyPropertyChanged("StartDate");
            NotifyPropertyChanged("EndDate");
        }

        public async Task<bool> Init()
        {
            if (UseGlobalDB)
                return await InitGlobal();
            else
                return await InitRegional();
        }

        public async Task<bool> InitGlobal()
        {
            try
            {
                UIViewModel.StatusText = "Initialising...";
                var exforLstTask = AirStripExFor.LoadExForList();
                var airportLstTask = AirstripInfo.LoadAirstrips();

                await Task.WhenAll(exforLstTask, airportLstTask);

                var lodgelstTask = await Lodge.LoadLodgeList(false);
                //var pricelstTask = PriceList.GetAllPriceLists();
                //var airportfeeTask = AirportFee.LoadAirportFees();

                //await Task.WhenAll(lodgelstTask, pricelstTask, airportfeeTask);

                //var tpTask1 = TPDBLookup.LoadTPDBLookups(Server, Database);
                //var tpTask2 = TPRoutingCode.LoadTPRoutingCodes(Server, Database);

                //await Task.WhenAll(tpTask1, tpTask2);

                var AgentsTsk = WICharterBooking.LoadOperatorAgents();
                var AirCraftNoneTask = AircraftType.GetNoneAircraftIDX();

                await Task.WhenAll(AgentsTsk, AirCraftNoneTask);

                var tmpLst = await AgentsTsk;
                if (tmpLst != null)
                {
                    AircraftType.IDX_AC_NONE = await AirCraftNoneTask;

                    var companies = tmpLst.Select(x => new WIBookingCompany { CompanyIDX = x.IDX_Operator, CompanyName = x.CompanyName, CompanyCurrencyCode = x.CompanyCurrencyCode }).ToList();
                    companies = companies.GroupBy(x => x.CompanyIDX).Select(x => x.First()).OrderBy(x => x.CompanyName).ToList();
                    _operators.AddRange(companies);

                    NotifyPropertyChanged("Operators");
                }

                LegViewModel.CountryID = CountryID;
                UIViewModel.StatusText = "";

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
            }
            return true;
        }
        public async Task<bool> InitRegional()
        {
            try
            {
                UIViewModel.StatusText = "Initialising...";
                var exforLstTask=  CampExForAirstrips.GetExForListV2(Server, Database);
                var airportLstTask= CampExForAirstrips.GetAirportList(Server, Database);

                await Task.WhenAll(exforLstTask, airportLstTask);

                var lodgelstTask=Lodge.LoadLodgeList(Server, Database,false);
                var pricelstTask= PriceList.GetAllPriceLists(Server, Database);
                var airportfeeTask= AirportFee.LoadAirportFees(Server, Database);

                await Task.WhenAll(lodgelstTask, pricelstTask, airportfeeTask);

                var tpTask1= TPDBLookup.LoadTPDBLookups(Server, Database);
                var tpTask2= TPRoutingCode.LoadTPRoutingCodes(Server, Database);

                await Task.WhenAll(tpTask1, tpTask2);

                var AgentsTsk= WICharterBooking.GetOperatorAgents(Server, Database);
                var AirCraftNoneTask= AircraftType.GetNoneAircraftIDX(Server, Database);

                await Task.WhenAll(AgentsTsk, AirCraftNoneTask);

                var tmpLst = await AgentsTsk;
                if (tmpLst!=null)
                {
                    AircraftType.IDX_AC_NONE = await AirCraftNoneTask;

                    var companies = tmpLst.Select(x => new WIBookingCompany { CompanyIDX = x.IDX_Operator, CompanyName = x.CompanyName, CompanyCurrencyCode = x.CompanyCurrencyCode }).ToList();
                    companies = companies.GroupBy(x => x.CompanyIDX).Select(x => x.First()).OrderBy(x => x.CompanyName).ToList();
                    _operators.AddRange(companies);

                    NotifyPropertyChanged("Operators");
                }

                LegViewModel.CountryID = CountryID;
                UIViewModel.StatusText = "";

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(ex.Message);
            }
            return true;
        }


        public bool ValidateSelection()
        {
            var hdrSaveLst = BookingHeaders.Where(x => x.Save).ToList();
        

            foreach (var hdr in hdrSaveLst)
            {
               

                if (hdr.OperatorID < 1)
                {
                    RadWindow.Alert("Please choose an operator for booking:\r\n " + hdr.WishResHeader.ReservationName);
                    return false;
                }

                var legSaveLst = hdr.Legs.Where(x=>x.State!=WIReservationLeg.DBLegState.IsCancelled);
                foreach (var leg in legSaveLst)
                {
       
                    string bookingName = hdr.WishResHeader.ReservationName;
                    if (leg.WishResLeg.IDX_FromAP < 1)
                    {
                        RadWindow.Alert("Please select a from airfield for leg :" + leg.WishResLeg.WishSectorID);
                        return false;
                    }

                    if (leg.WishResLeg.IDX_ToAP < 1)
                    {
                        RadWindow.Alert("Please select a to airfield for leg :" + leg.WishResLeg.WishSectorID);
                        return false;
                    }

                    if (String.IsNullOrEmpty(leg.WishResLeg.ExField))
                    {
                        RadWindow.Alert("Please select an ex value for for leg :" + leg.WishResLeg.WishSectorID);
                        return false;
                    }

                    if (String.IsNullOrEmpty(leg.WishResLeg.ForField))
                    {
                        RadWindow.Alert("Please select a for value for for leg :" + leg.WishResLeg.WishSectorID);
                        return false;
                    } 

                }

            }

   

            return true;
        }

        public async Task<List<int>> SaveSelection()
        {
            if (UseGlobalDB)
                return await SaveSelectionGlobal();
            else
                return await SaveSelectionRegional();
        }

        public async Task<List<int>> SaveSelectionGlobal()
        {
            try
            {
                UIViewModel.StatusText = "Saving...";
                List<int> hdrIDLst = null;
                var WI = new WishIntegrationGlobal();
                var hdrSaveLst = BookingHeaders.Where(x => x.Save).ToList();
                //hdrSaveLst.ForEach(x => x.InvalidateAllLegs());
                var newResTask = WI.WISaveNewReservations(hdrSaveLst, IDX_User, CountryID);
                var updateResTask = WI.WIUpdateReservations(hdrSaveLst, IDX_User, CountryID);

                var refreshList = await Task.WhenAll(newResTask, updateResTask);


                
                hdrIDLst = refreshList[0];
                hdrIDLst.AddRange(refreshList[1]);


                foreach (var hdr in hdrSaveLst)
                    BookingHeaders.Remove(hdr);

                UIViewModel.StatusText = "Saved.";
                return hdrIDLst;
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("\"{0}\"  has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }

                UIViewModel.StatusText = "Validation error";
                FailWindow.Display(sb.ToString());
                return null;
            }
            catch (Exception ex)
            {
                UIViewModel.StatusText = "Database error";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);

                return null;
            }

        }
        public async Task<List<int>> SaveSelectionRegional()
        {
            try
            {
                UIViewModel.StatusText = "Saving...";
                List<int> hdrIDLst = null;
                var WI = new Classes.WishIntegration();
                var hdrSaveLst = BookingHeaders.Where(x => x.Save).ToList();
                //hdrSaveLst.ForEach(x => x.InvalidateAllLegs());
                var newResTask=WI.WISaveNewReservations(hdrSaveLst, IDX_User, CountryID, Server, Database);       
                var updateResTask=WI.WIUpdateReservations(hdrSaveLst, IDX_User, CountryID, Server, Database);

                 var refreshList=await Task.WhenAll(newResTask, updateResTask);

                hdrIDLst = refreshList[0];
                hdrIDLst.AddRange(refreshList[1]);


                foreach (var hdr in hdrSaveLst)
                    BookingHeaders.Remove(hdr);

                UIViewModel.StatusText = "Saved.";
                return hdrIDLst;
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in e.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("\"{0}\"  has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }

                UIViewModel.StatusText = "Validation error";
                FailWindow.Display(sb.ToString());
                return null;
            }
            catch (Exception ex)
            {
                UIViewModel.StatusText = "Database error";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);

                return null;
            }
 
        }



        public async Task RefreshSeletedBooking()
        {
            if (SelectedBooking!=null)
            {

                var tmpBookingList = new List<WIReservationHeader>();

                SelectedBooking.HasChangedLeg = false;
                SelectedBooking.HasRemovedLeg = false;
                SelectedBooking.HasNewLeg = false;

                SelectedBooking.Legs.RemoveAll(x => x.State != WIReservationLeg.DBLegState.IsUnmodified);

                var oldBooking = WishBookings.FirstOrDefault(x => x.BookingID == SelectedBooking.BookingID && x.PartyGroupID == SelectedBooking.PartyGroupID);
                WishBookings.Remove(oldBooking);
                var newbooking= await WICharterBooking.RefreshBooking(oldBooking.BookingID, oldBooking.PartyGroupID, WishPrincipalID, WishPrincipalSoleUseID);
                newbooking.FilterLegs(StartDate, EndDate);
                UpdateHeaderInfo(SelectedBooking, newbooking);
                UpdatePax(SelectedBooking, newbooking);
                WishBookings.Add(newbooking);

                tmpBookingList.Add(SelectedBooking);
                AnalyseBookingLegs(tmpBookingList);
                await SelectBookingV2();
                NotifyPropertyChanged("WishBookings");
            }

        }

       public async Task<List<WIReservationHeader>> RefreshHeader(int BookingID)
        {
            try
            {
                var wishInt = new Classes.WishIntegration();
                WIReservationHeader.Operators = Operators;

                var wiReservations = await wishInt.WIGetReservatons(BookingID, Server, Database);
                if (wiReservations != null)
                {
                    var wiResHdrs = wiReservations.Select(x => (WIReservationHeader)x).ToList();
                    foreach (var newWIRes in wiResHdrs)
                    {

                        var agentOperator = Operators.FirstOrDefault(x => x.CompanyName == newWIRes.WishResHeader.OperatorName);

                        if (agentOperator != null)
                            newWIRes.OperatorID = agentOperator.CompanyIDX;
                    }

                    BookingHeaders.AddRange(wiResHdrs);
                    NotifyPropertyChanged("BookingHeaders");
                    return wiResHdrs;
                }

                return null;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return null;
            }
        }


        public async Task Refresh()
        {
            if (UseGlobalDB)
               await RefreshGlobal();
            else
               await RefreshRegional();
        }

        public async Task RefreshGlobal()
        {
            try
            {

                var wishInt = new WishIntegrationGlobal();
                BookingHeaders.Clear();
                WishBookings.Clear();
                RemovedHeaders.Clear();

                if (StartDate > EndDate)
                {
                    RadWindow.Alert("Start date must precede end date.");
                    return;
                }

                LegViewModel.Clear();

                WIReservationHeader.Operators = Operators;

                UIViewModel.StatusText = "Refreshing ...";
                var chBookingsTask = WICharterBooking.RetrieveCHBookingsEFF(WishPrincipalID, WishPrincipalSoleUseID, StartDate, EndDate);

                var sfBookingsTask = WICharterBooking.RetrieveSFBookingsEFF(WishPrincipalID, StartDate, EndDate);

                var bookingsLst = await Task.WhenAll(chBookingsTask, sfBookingsTask);

                UIViewModel.StatusText = "Analysing ...";
                var wishBookings = bookingsLst[0];
                foreach (var sfBooking in bookingsLst[1])
                {
                    var chBooking = wishBookings.FirstOrDefault(x => x.BookingID == sfBooking.BookingID && x.PartyGroupID == sfBooking.PartyGroupID);
                    if (chBooking == null)
                        wishBookings.Add(sfBooking);
                    else
                        chBooking.Legs.AddRange(sfBooking.Legs);
                }


                var bookingRefs = wishBookings.Select(x => new WishBookingReference { WishBookingID = x.BookingID, WishPartyGroupID = x.PartyGroupID }).ToList();

                var wishReservations = await wishInt.WIGetReservatons(bookingRefs);
                var wiReservations = wishReservations.Select(x => (WIReservationHeader)x).ToList();

                foreach (var wiRes in wiReservations)
                {
                    var booking = wishBookings.FirstOrDefault(x => x.BookingID == wiRes.WishResHeader.BookingID && x.PartyGroupID == wiRes.WishResHeader.PartyGroupID);
                    if (booking != null)
                    {
                        UpdateHeaderInfo(wiRes, booking);
                    }
                    else
                    {
                        RemovedHeaders.Add(wiRes);
                    }



                    var agentOperator = Operators.FirstOrDefault(x => x.CompanyName == wiRes.WishResHeader.OperatorName);

                    if (agentOperator != null)
                        wiRes.OperatorID = agentOperator.CompanyIDX;
                }

                var nonCancelledBookings = wishBookings.Where(x => x.ResStatus != "CANC").ToList();
                var cancelledBookings = wishBookings.Where(x => x.ResStatus == "CANC").ToList();
                foreach (var wishBkng in nonCancelledBookings)
                {
                    if (wiReservations.FirstOrDefault(x => x.WishResHeader.BookingID == wishBkng.BookingID && x.WishResHeader.PartyGroupID == wishBkng.PartyGroupID) == null)
                    {

                        var wishResHdr = wishBkng.ToWIReservation();
                        wishResHdr.WishResHeader.ReservationName += " [" + wishBkng.PartyGroupName + "]";
                        BookingHeaders.Add(wishResHdr);
                    }
                }


                foreach (var wishBkng in cancelledBookings)
                {
                    var wiRes = wiReservations.FirstOrDefault(x => x.WishResHeader.BookingID == wishBkng.BookingID && x.WishResHeader.PartyGroupID == wishBkng.PartyGroupID);
                    if (wiRes != null)
                        RemovedHeaders.Add(wiRes);

                }

                wiReservations = wiReservations.Except(RemovedHeaders).ToList();
                WishBookings.AddRange(wishBookings);
                BookingHeaders.AddRange(wiReservations);
                AnalyseBookingLegs(BookingHeaders.Where(x => !x.IsNew).ToList());

            }
            catch (Exception ex)
            {
                UIViewModel.StatusText = "Error.";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

            UIViewModel.StatusText = "";
            NotifyPropertyChanged("RemovedHeaders");
            NotifyPropertyChanged("BookingHeaders");

            if (RemovedLegsViewModel != null)
            {
                RemovedLegsViewModel.Refresh(RemovedHeaders);
            }
        }

        public async Task RefreshRegional()
        {
            try
            {
               
                var wishInt = new Classes.WishIntegration();
                BookingHeaders.Clear();
                WishBookings.Clear();
                RemovedHeaders.Clear();

                if (StartDate > EndDate)
                {
                    RadWindow.Alert("Start date must precede end date.");
                    return;
                }

                LegViewModel.Clear();

                WIReservationHeader.Operators = Operators;

                UIViewModel.StatusText = "Refreshing ...";
                var  chBookingsTask= WICharterBooking.RetrieveCHBookingsEFF(WishPrincipalID, WishPrincipalSoleUseID, StartDate, EndDate);

                var  sfBookingsTask = WICharterBooking.RetrieveSFBookingsEFF(WishPrincipalID,StartDate, EndDate);

                var bookingsLst = await Task.WhenAll(chBookingsTask, sfBookingsTask);

                UIViewModel.StatusText = "Analysing ...";
                var wishBookings = bookingsLst[0];
                foreach (var sfBooking in bookingsLst[1])
                {
                    var chBooking = wishBookings.FirstOrDefault(x => x.BookingID == sfBooking.BookingID && x.PartyGroupID == sfBooking.PartyGroupID);
                    if (chBooking==null)
                        wishBookings.Add(sfBooking);
                    else
                        chBooking.Legs.AddRange(sfBooking.Legs);
                }


                var bookingRefs = wishBookings.Select(x => new WishBookingReference { WishBookingID = x.BookingID, WishPartyGroupID = x.PartyGroupID }).ToList();
               
                var wishReservations = await wishInt.WIGetReservatons(bookingRefs, Server, Database);
                var wiReservations = wishReservations.Select(x => (WIReservationHeader)x).ToList();

                foreach (var wiRes in wiReservations)
                {
                    var booking = wishBookings.FirstOrDefault(x => x.BookingID == wiRes.WishResHeader.BookingID && x.PartyGroupID == wiRes.WishResHeader.PartyGroupID);
                    if (booking != null )
                    {
                        UpdateHeaderInfo(wiRes, booking);
                    }
                    else
                    {
                        RemovedHeaders.Add(wiRes);
                    }

                 

                    var agentOperator = Operators.FirstOrDefault(x => x.CompanyName == wiRes.WishResHeader.OperatorName);

                    if (agentOperator != null)
                        wiRes.OperatorID = agentOperator.CompanyIDX;
                }

                var nonCancelledBookings = wishBookings.Where(x => x.ResStatus != "CANC").ToList();
                var cancelledBookings= wishBookings.Where(x => x.ResStatus == "CANC").ToList();
                foreach (var wishBkng in nonCancelledBookings)
                {
                    if (wiReservations.FirstOrDefault(x => x.WishResHeader.BookingID == wishBkng.BookingID && x.WishResHeader.PartyGroupID == wishBkng.PartyGroupID) == null)
                    {
                      
                        var wishResHdr = wishBkng.ToWIReservation();
                        wishResHdr.WishResHeader.ReservationName += " [" + wishBkng.PartyGroupName + "]";
                        BookingHeaders.Add(wishResHdr);
                    }
                }

               
                foreach (var wishBkng in cancelledBookings)
                {
                    var wiRes = wiReservations.FirstOrDefault(x => x.WishResHeader.BookingID == wishBkng.BookingID && x.WishResHeader.PartyGroupID == wishBkng.PartyGroupID);
                    if (wiRes != null)
                        RemovedHeaders.Add(wiRes);

                }

                wiReservations = wiReservations.Except(RemovedHeaders).ToList();
                WishBookings.AddRange(wishBookings);
                BookingHeaders.AddRange(wiReservations);
                AnalyseBookingLegs(BookingHeaders.Where(x => !x.IsNew).ToList());

            }
            catch (Exception ex)
            {
                UIViewModel.StatusText = "Error.";
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

            UIViewModel.StatusText = "";
            NotifyPropertyChanged("RemovedHeaders");
            NotifyPropertyChanged("BookingHeaders");

            if (RemovedLegsViewModel != null)
            {
                RemovedLegsViewModel.Refresh(RemovedHeaders);
            }
        }

        public async Task SelectBookingV2()
        {
            try
            {
                if (LegViewModel != null && SelectedBooking != null)
                {
                    var wishBooking = WishBookings.FirstOrDefault(x => x.BookingID == SelectedBooking.WishResHeader.BookingID && x.PartyGroupID == SelectedBooking.WishResHeader.PartyGroupID);

                    if (wishBooking != null)
                    {
                        await LegViewModel.Refresh(wishBooking, SelectedBooking);
                    }

                }

                NotifyPropertyChanged("CanRefresh");
                NotifyPropertyChanged("CanUpdate");
                NotifyPropertyChanged("CanRemove");
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

        }



        public void AnalyseBookingLegs(List<WIReservationHeader> integratedBookings)
        {
          

            foreach (var resBking in integratedBookings)
            {
                var wishBooking = WishBookings.FirstOrDefault(x => x.BookingID == resBking.WishResHeader.BookingID && x.PartyGroupID == resBking.WishResHeader.PartyGroupID);
                if (wishBooking != null)
                {
                    var schedwinResLegs = resBking.Legs.Where(x=>x.WishResLeg.Canceled==false).ToList();
                    var wishCharterLegs = wishBooking.Legs.Where(x=>x.IsCancelled==false).Select(x => (WIReservationLeg)x).ToList();

                    foreach (var resLeg in schedwinResLegs)
                    {
                    
                            var charterLeg = wishCharterLegs.FirstOrDefault(x => x.WishResLeg.WishSectorID == resLeg.WishResLeg.WishSectorID);
                            if (charterLeg == null)
                                resBking.HasRemovedLeg = true;
                            else
                            {
                                if ((charterLeg.WishResLeg.BookingDate != resLeg.WishResLeg.BookingDate) ||
                                     ((!String.IsNullOrEmpty(resLeg.WishResLeg.WishEx) && (charterLeg.WishResLeg.WishEx!= resLeg.WishResLeg.WishEx)) ||
                                    ((!String.IsNullOrEmpty(resLeg.WishResLeg.WishFor) && (charterLeg.WishResLeg.WishFor != resLeg.WishResLeg.WishFor)) ||
                                    ( charterLeg.WishResLeg.ETA != resLeg.WishResLeg.ETA) || (charterLeg.WishResLeg.ETD != resLeg.WishResLeg.ETD) ||
                                    (charterLeg.WishResLeg.Notes != resLeg.WishResLeg.Notes))))
                                    resBking.HasChangedLeg = true;
                            }

 
                    }

                    foreach (var wishLeg in wishCharterLegs)
                    {
                        var resLeg = schedwinResLegs.FirstOrDefault(x => x.WishResLeg.WishSectorID == wishLeg.WishResLeg.WishSectorID);
                        if (resLeg == null)
                            resBking.HasNewLeg = true;
                    }

                }
          
            }

        }

        public async Task<bool> CancelHeader()
        {
            try
            {
                if (SelectedBooking != null)
                {
                    var res = new Schedwin.Reservations.Classes.Reservations();
                    var result = await res.CancelBooking(SelectedBooking.WishResHeader.Res_IDX, Server, Database);
                    if (result)
                    {
                        this.BookingHeaders.Remove(SelectedBooking);
                        NotifyPropertyChanged("BookingHeaders");

                        return true;
                    }
                    else
                    {

                        FailWindow.Display(res.LastError);
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveHeader()
        {
            try
            {
                if (SelectedBooking != null)
                {
                    var res = new Schedwin.Reservations.Classes.Reservations();
                    var result = await res.DeleteBooking(SelectedBooking.WishResHeader.Res_IDX, Server, Database);
                    if (result)
                    {
                        this.BookingHeaders.Remove(SelectedBooking);
                        NotifyPropertyChanged("BookingHeaders");

                        return true;
                    }
                    else
                    {

                        FailWindow.Display(res.LastError);
                        return false;
                    }

                }
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }

            return true;
        }

        private void UpdateHeaderInfo(WIReservationHeader wiResHeader, WICharterBooking newbooking)
        {
            wiResHeader.TPRef = newbooking.TPRef;
            wiResHeader.WishResHeader.WishResStatus = newbooking.ResStatus;
            wiResHeader.WishResHeader.ReservationNote = newbooking.Notes;
            wiResHeader.WishResHeader.WishConsultant = newbooking.Consultant;
            wiResHeader.OldPax = wiResHeader.WishResHeader.PaxCount.ToString();
            if (wiResHeader.WishResHeader.PaxCount != newbooking.PaxCount)
            {
                wiResHeader.HasPaxDifference = true;
                wiResHeader.Pax = newbooking.PaxCount;
            }

            wiResHeader.OldReservationName = wiResHeader.ReservationName;
            if (!wiResHeader.ReservationName.Contains(newbooking.ReservationName))
                wiResHeader.HasDifferentResName = true;

            wiResHeader.OldDepartureDate = wiResHeader.DepartureDate.ToShortDateString();
            if (wiResHeader.WishResHeader.DepartureDate != newbooking.StartDate)
                wiResHeader.HasDateChange = true;

            wiResHeader.WishResHeader.DepartureDate = newbooking.StartDate;
            wiResHeader.WishResHeader.ReservationName = newbooking.ReservationName + "[ " + newbooking.PartyGroupName + " ]";
            wiResHeader.WishResHeader.PartyGroupName = newbooking.PartyGroupName;
        }

        private void UpdatePax(WIReservationHeader wiResHeader, WICharterBooking newbooking)
        {
            wiResHeader.PaxList.Clear();
            wiResHeader.PaxList.AddRange(newbooking.Pax.Select(x => (WIReservationPax)x).ToList());
        }

        private void BuildNameSearchList(String namePattern)
        {
            namePattern = namePattern.ToLower();
            _currentSearchLst.Clear();
            _currentSearchLst.AddRange(BookingHeaders.Where(x => x.ReservationName.ToLower().Contains(namePattern)).ToList());
        }

        private void BuildIDSearchList(int bookingID)
        {
            _currentSearchLst.Clear();
            _currentSearchLst.AddRange(BookingHeaders.Where(x => x.BookingID == bookingID).ToList());
        }

        public WIReservationHeader ScrollReservationIntoView(String reservationName, int bookingID, int iteration)
        {
            WIReservationHeader selectedItem=null;
            if (bookingID>0)
            {
                if (iteration == 0)
                    BuildIDSearchList(bookingID);
            }
            else if (!String.IsNullOrEmpty(reservationName))
            {
                if (iteration == 0)
                    BuildNameSearchList(reservationName);              
            }

            if (iteration < _currentSearchLst.Count)
            {
                selectedItem = BookingHeaders.FirstOrDefault(x => x.BookingID == _currentSearchLst[iteration].BookingID && x.PartyGroupID == _currentSearchLst[iteration].PartyGroupID);
            }
            if (selectedItem != null)
            {
                SelectedBooking = selectedItem;
                NotifyPropertyChanged("SelectedBooking");
            }

            return selectedItem;
        }

        public async Task<bool> LoadSingleBooking(int bookingID)
        {
            try
            {
               var hdrTsks = new List<Task<WICharterBooking>>();
               var hdrs = await RefreshHeader(bookingID);
               foreach (var hdr in hdrs)
               {
                    hdrTsks.Add(WICharterBooking.RefreshBooking(hdr.BookingID, hdr.PartyGroupID, WishPrincipalID, WishPrincipalSoleUseID));
               };


                await Task.WhenAll(hdrTsks);

                foreach (var hdrtsk in hdrTsks)
                {
                    WishBookings.Add(await hdrtsk);
                }

                AnalyseBookingLegs(hdrs);
                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }


        #region V1 Methods

        //public async Task<bool> SaveSelection()
        //{
        //    try
        //    {
        //        UpdateCurrentBookingLegs();

        //        var hdrSaveLst = BookingHeaders.Where(x => x.Save).ToList();
        //        var legSaveLst = AllWishLegs.Where(x => hdrSaveLst.Contains(x.WIHeader)).ToList();

        //        foreach (var hdr in hdrSaveLst)
        //        {
        //            hdr.Legs = legSaveLst.Where(x => x.WIHeader == hdr).ToList();

        //            if (hdr.OperatorID < 1)
        //            {
        //                RadWindow.Alert("Please choose an operator for booking:\r\n " + hdr.WishResHeader.ReservationName);
        //                return false;
        //            }
        //            if (hdr.AgentID < 1)
        //            {
        //                RadWindow.Alert("Please choose an agent for booking:\r\n " + hdr.WishResHeader.ReservationName);
        //                return false;
        //            }

        //        }

        //        foreach (var leg in legSaveLst)
        //        {
        //            if (leg.WIHeader != null)
        //            {
        //                string bookingName = leg.WIHeader.WishResHeader.ReservationName;
        //                if (leg.WishResLeg.IDX_FromAP < 1)
        //                {
        //                    RadWindow.Alert("Please select a from airfield for leg :" + leg.WishResLeg.WishSectorID);
        //                    return false;
        //                }

        //                if (leg.WishResLeg.IDX_ToAP < 1)
        //                {
        //                    RadWindow.Alert("Please select a to airfield for leg :" + leg.WishResLeg.WishSectorID);
        //                    return false;
        //                }

        //                if (String.IsNullOrEmpty(leg.WishResLeg.ExField))
        //                {
        //                    RadWindow.Alert("Please select an ex value for for leg :" + leg.WishResLeg.WishSectorID);
        //                    return false;
        //                }

        //                if (String.IsNullOrEmpty(leg.WishResLeg.ForField))
        //                {
        //                    RadWindow.Alert("Please select a for value for for leg :" + leg.WishResLeg.WishSectorID);
        //                    return false;
        //                }

        //            }

        //        }
        //        using (new StackedCursorOverride(Cursors.Wait))
        //        {


        //            var result = await WIBookings.SaveBookings(hdrSaveLst, IDX_AC_NONE, IDX_User, IDX_Company, Server, Database);
        //            if (!result)
        //                FailWindow.Display(WIBookings.LastError);


        //            var uprreshHdrS = await RefreshHeaders(hdrSaveLst);
        //            if (uprreshHdrS != null)
        //            {
        //                result = await RefreshLegs(hdrSaveLst);
        //                if (!result)
        //                    FailWindow.Display("Failed to refresh booking legs, please try manual refresh.");
        //            }
        //            else
        //            {
        //                FailWindow.Display("Failed to refresh booking headers, pleae try manual refresh");

        //            }
        //            var pgList = uprreshHdrS.Select(x => new WIPartyGroup { PartyGroupID = x.WishResHeader.PartyGroupID, Res_IDX = x.WishResHeader.ResHeaderID }).ToList();
        //            result = await WIBookings.RefreshWishGuests(pgList, Server, Database);
        //            if (!result)
        //            {
        //                FailWindow.Display("Unable to refresh Wish guests:\r\n" + WIBookings.LastError);
        //                return false;
        //            }

        //            SuccessWindow.Display("Bookings(s) saved.");

        //            // WIBookings.SaveLegs(legSaveLst, this.IDX_User, this.IDX_Company, Server, Database);
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        FailWindow.Display("Error saving guests:\r\n" + ex.Message);
        //        return false;
        //    }
        //}

        //public async Task Refresh()
        //{
        //    try
        //    {

        //        BookingHeaders.Clear();
        //        WishBookings.Clear();
        //        RemovedHeaders.Clear();

        //        if (StartDate > EndDate)
        //        {
        //            RadWindow.Alert("Start date must precede end date.");
        //            return;
        //        }

        //        LegViewModel.Clear();

        //        WIReservationHeader.Operators = Operators;

        //        //var wishBookings = await WICharterBooking.RetrieveBookings(1256, StartDate, EndDate);
        //        var wishBookings = await WICharterBooking.RetrieveBookings(WishPrincipalID, StartDate, EndDate);
        //        //var wishBookings = await WICharterBooking.RetrieveCHBookingsEFF(WishPrincipalID,StartDate, EndDate);
        //        //var wishBookings = await WICharterBooking.RetrieveSFBookingsEFF(WishPrincipalID,StartDate, EndDate);
        //        var bookingRefs = wishBookings.Select(x => new WishBookingReference { WishBookingID = x.BookingID, WishPartyGroupID = x.PartyGroupID }).ToList();

        //        var wishReservations = await Reservations.GetWishReservationHeaders(bookingRefs, Server, Database);

        //        var wiReservations = wishReservations.Select(x => (WIReservationHeader)x).ToList();

        //        foreach (var wiRes in wiReservations)
        //        {
        //            var booking = wishBookings.FirstOrDefault(x => x.BookingID == wiRes.WishResHeader.BookingID && x.PartyGroupID == wiRes.WishResHeader.PartyGroupID);
        //            if (booking != null)
        //            {
        //                wiRes.DepartureDate = booking.DepartureDate;
        //                wiRes.Notes = booking.Notes;
        //                wiRes.Consultant = booking.Consultant;
        //                wiRes.ResStatus = booking.ResStatus;

        //                wiRes.OldPax = wiRes.WishResHeader.Pax.ToString();
        //                if (wiRes.WishResHeader.Pax != booking.PaxCount)
        //                {
        //                    wiRes.HasPaxDifference = true;
        //                    wiRes.WishResHeader.Pax = booking.PaxCount;
        //                }

        //                wiRes.OldReservationName = wiRes.ReservationName;
        //                if (!wiRes.ReservationName.Contains(booking.ReservationName))
        //                {
        //                    wiRes.HasDifferentResName = true;
        //                    String subscript = " ";
        //                    int startBracketPos = wiRes.WishResHeader.ReservationName.LastIndexOf('[');
        //                    int endBracketPos = wiRes.WishResHeader.ReservationName.LastIndexOf(']');
        //                    if (startBracketPos > -1 && endBracketPos > -1)
        //                    {
        //                        subscript = wiRes.WishResHeader.ReservationName.Substring(startBracketPos, (endBracketPos - startBracketPos) + 1);
        //                    }

        //                    wiRes.WishResHeader.ReservationName = booking.ReservationName + subscript;
        //                }

        //            }
        //            else
        //            {
        //                RemovedHeaders.Add(wiRes);
        //            }

        //            wiRes.LoadAgentList(_operatorAgents);

        //            var agentOperator = Operators.FirstOrDefault(x => x.CompanyName == wiRes.WishResHeader.OperatorName);

        //            if (agentOperator != null)
        //                wiRes.OperatorID = agentOperator.CompanyIDX;
        //        }

        //        foreach (var wishBkng in wishBookings)
        //        {
        //            if (wiReservations.FirstOrDefault(x => x.WishResHeader.BookingID == wishBkng.BookingID && x.WishResHeader.PartyGroupID == wishBkng.PartyGroupID) == null)
        //            {
        //                int pgCount = BookingHeaders.Where(x => x.WishResHeader.BookingID == wishBkng.BookingID).Count();
        //                var wishResHdr = wishBkng.ToWIReservation();
        //                wishResHdr.LoadAgentList(_operatorAgents);
        //                wishResHdr.WishResHeader.ReservationName += " [" + ++pgCount + "]";
        //                BookingHeaders.Add(wishResHdr);
        //            }
        //        }

        //        if (RemovedLegsViewModel != null)
        //        {
        //            RemovedLegsViewModel.Refresh(RemovedHeaders);
        //        }
        //        wiReservations = wiReservations.Except(RemovedHeaders).ToList();
        //        WishBookings.AddRange(wishBookings);
        //        BookingHeaders.AddRange(wiReservations);

        //        await GetAllWishLegs();

        //        foreach (var bookingHdr in BookingHeaders)
        //        {
        //            AnalyseBookingLegs(bookingHdr);
        //        }


        //    }

        //    catch (Exception ex)
        //    {
        //        var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
        //        var exMessage = string.Join(Environment.NewLine, messages);
        //        FailWindow.Display(exMessage);
        //    }

        //    NotifyPropertyChanged("BookingHeaders");

        //}


        //public void UpdateCurrentBookingLegs()
        //{
        //    if (LegViewModel != null)
        //    {
        //        var nonNewLegs = LegViewModel.BookingLegs.ToList();
        //        foreach (var leg in nonNewLegs)
        //        {
        //            var existingLeg = AllWishLegs.FirstOrDefault(x => x.WishResLeg.WishSectorID == leg.WishResLeg.WishSectorID);
        //            if (existingLeg != null)
        //            {
        //                existingLeg.WishResLeg.WishFrom = leg.WishResLeg.WishFrom;
        //                existingLeg.WishResLeg.WishTo = leg.WishResLeg.WishTo;
        //                existingLeg.WishResLeg.IDX_FromAP = leg.WishResLeg.IDX_FromAP;
        //                existingLeg.WishResLeg.IDX_ToAP = leg.WishResLeg.IDX_ToAP;
        //                existingLeg.WishResLeg.ExField = leg.WishResLeg.ExField;
        //                existingLeg.WishResLeg.ForField = leg.WishResLeg.ForField;
        //                existingLeg.WishResLeg.Notes = leg.WishResLeg.Notes;
        //                existingLeg.WishResLeg.FOC = leg.WishResLeg.FOC;
        //                existingLeg.WishResLeg.SoleUse = leg.WishResLeg.SoleUse;
        //                existingLeg.WishResLeg.Voucher = leg.WishResLeg.Voucher;
        //                existingLeg.WishResLeg.EarliestEx = leg.WishResLeg.EarliestEx;
        //                existingLeg.WishResLeg.EarliestFor = leg.WishResLeg.EarliestFor;
        //                existingLeg.WishResLeg.LatestEx = leg.WishResLeg.LatestEx;
        //                existingLeg.WishResLeg.LatestFor = leg.WishResLeg.LatestFor;
        //                existingLeg.WishResLeg.CharterType = leg.WishResLeg.CharterType;
        //                existingLeg.WishResLeg.BookingDate = leg.WishResLeg.BookingDate;
        //            }
        //        }
        //    }
        //}

        //public async Task SelectBooking()
        //{
        //    if (LegViewModel != null && SelectedBooking != null)
        //    {
        //        var wishBooking = WishBookings.FirstOrDefault(x => x.BookingID == SelectedBooking.WishResHeader.BookingID && x.PartyGroupID == SelectedBooking.WishResHeader.PartyGroupID);

        //        if (wishBooking != null)
        //        {
        //            UpdateCurrentBookingLegs();
        //            //await LegViewModel.Refresh(wishBooking, SelectedBooking.WishResHeader.ResHeaderID);
        //            var bookingWishLegs = AllWishLegs.Where(x => x.WIHeader.WishResHeader == SelectedBooking.WishResHeader).ToList();
        //            var newLegs = await LegViewModel.Refresh(wishBooking, SelectedBooking, bookingWishLegs);
        //            foreach (var newleg in newLegs)
        //            {
        //                if (AllWishLegs.FirstOrDefault(x => x.WishResLeg.WishSectorID == newleg.WishResLeg.WishSectorID) == null)
        //                    AllWishLegs.Add(newleg);
        //            }

        //        }

        //    }
        //    NotifyPropertyChanged("CanUpdate");
        //    NotifyPropertyChanged("CanRemove");
        //}


        //public async Task GetAllWishLegs()
        //{
        //    this.AllWishLegs.Clear();
        //    var resHdrIDs = BookingHeaders.Select(x => x.WishResHeader.ResHeaderID).ToList();
        //    var legs = await Reservations.GetWishReservationLegs(resHdrIDs, Server, Database);
        //    var wiLegs = legs.Select(x => (WIReservationLeg)x).ToList();
        //    foreach (var wiLeg in wiLegs)
        //    {
        //        var hdr = BookingHeaders.FirstOrDefault(x => x.WishResHeader.ResHeaderID == wiLeg.WishResLeg.HeaderID);
        //        wiLeg.WIHeader = hdr;
        //    }
        //    AllWishLegs.AddRange(wiLegs);
        //}


        //public async Task<bool> RefreshLegs(List<WIReservationHeader> lstHdrs)
        //{

        //    foreach (var hdr in lstHdrs)
        //    {
        //        foreach (var leg in hdr.Legs)
        //            AllWishLegs.Remove(leg);
        //    }

        //    var resHdrIDs = BookingHeaders.Select(x => x.WishResHeader.ResHeaderID).ToList();
        //    var legs = await Reservations.GetWishReservationLegs(resHdrIDs, Server, Database);
        //    if (legs != null)
        //    {
        //        var wiLegs = legs.Select(x => (WIReservationLeg)x).ToList();
        //        foreach (var wiLeg in wiLegs)
        //        {
        //            var hdr = BookingHeaders.FirstOrDefault(x => x.WishResHeader.ResHeaderID == wiLeg.WishResLeg.HeaderID);
        //            wiLeg.WIHeader = hdr;
        //        }

        //        AllWishLegs.AddRange(wiLegs);
        //        foreach (var hdr in lstHdrs)
        //        {
        //            AnalyseBookingLegs(hdr);
        //        }

        //        return true;
        //    }

        //    return false;
        //}

        #endregion
    }
}
