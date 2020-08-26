using Schedwin.Data;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Reservations.Classes;
namespace Schedwin.WishIntegration.Classes
{
    public class WishIntegration
    {
        #region public interface section

        public async Task<List<Reservation>> WIGetReservatons(int bookingID, String Server, String regionalDatabase)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
            var ctx = new SchedwinRegionalEntities(constring);
            {
            
                var headers = await ctx.tsch_WISHIntegrationHeader.Include("tsch_reservationHeader")
                                                                   .Include("tsch_reservationHeader.tsch_Passengers")
                                                                  .Include("tsch_WishIntegrationLeg")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_LegsRes")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tset_airports1")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tset_airports")
                                                                   .Include("tsch_reservationHeader.tset_Companies")
                                                                   .Include("tsch_reservationHeader.tlst_ResStatus")
                                                                   .Include("tsch_reservationHeader.tset_Personnel")
                                                                   .Include("tsch_reservationHeader.tset_Personnel1")
                                                                   .Where(x => x.WISHBookingID==bookingID).ToListAsync();

                if (headers != null)
                    return headers.Select(x => (Reservation)x).ToList();
                else
                    return null;
            }
        }

        public async Task<List<Reservation>> WIGetReservatons(List<WishBookingReference> wishBookingReferences, String Server, String regionalDatabase)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
            var ctx = new SchedwinRegionalEntities(constring);
            {
                var wishBookingIDS = wishBookingReferences.Select(x => x.WishBookingID).ToList();
                var wishPGroupIDs = wishBookingReferences.Select(x => x.WishPartyGroupID).ToList();
                var headers = await ctx.tsch_WISHIntegrationHeader.Include("tsch_reservationHeader")
                                                                    .Include("tsch_reservationHeader.tsch_Passengers")
                                                                  .Include("tsch_WishIntegrationLeg")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_LegsRes")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tset_airports1")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tset_airports")
                                                                   .Include("tsch_reservationHeader.tset_Companies")
                                                                   .Include("tsch_reservationHeader.tlst_ResStatus")
                                                                   .Include("tsch_reservationHeader.tset_Personnel")
                                                                   .Include("tsch_reservationHeader.tset_Personnel1")
                                                                   .Where(x => wishBookingIDS.Contains(x.WISHBookingID)).ToListAsync();

                headers = headers.Where(x => wishPGroupIDs.Contains(x.WISHPGID)).ToList();

                if (headers != null)
                    return headers.Select(x => (Reservation)x).ToList();
                else
                    return null;
            }
        }

        public async Task<List<Reservation>> WIGetReservatons(List<int> lstlResHdrIDs, String Server, String regionalDatabase)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
            var ctx = new SchedwinRegionalEntities(constring);
            {

                var headers = await ctx.tsch_WISHIntegrationHeader.Include("tsch_reservationHeader")
                                                                   .Include("tsch_reservationHeader.tsch_Passengers")
                                                                  .Include("tsch_WishIntegrationLeg")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_LegsRes")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tset_airports1")
                                                                   .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tset_airports")
                                                                   .Include("tsch_reservationHeader.tset_Companies")
                                                                   .Include("tsch_reservationHeader.tlst_ResStatus")
                                                                   .Include("tsch_reservationHeader.tset_Personnel")
                                                                   .Include("tsch_reservationHeader.tset_Personnel1")
                                                                   .Where(x => lstlResHdrIDs.Contains(x.tsch_ReservationHeader.IDX)).ToListAsync();


                if (headers != null)
                    return headers.Select(x => (Reservation)x).ToList();
                else
                    return null;
            }
        }


        public async Task<List<int>> WISaveNewReservations(List<WIReservationHeader> newHdrs, int idxUser, int countryID, String Server, String regionalDatabase)
        {
            
                List<int> updateHdrs = null;
                await StandardPassengerWeights.LoadStandardWeights(Server, regionalDatabase);


                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
                var ctx = new SchedwinRegionalEntities(constring);
                {
                    var hrsToCreate = newHdrs.Where(x => x.IsNew).ToList(); 
                    var newDbHdrs = await CreateReservations(hrsToCreate, idxUser, countryID,ctx);
                    updateHdrs = newDbHdrs.Values.Select(x => x.tsch_ReservationHeader.IDX).ToList();

                    return updateHdrs;
                }
          
          
        }

        public async Task<List<int>> WIUpdateReservations(List<WIReservationHeader> newHdrs, int idxUser, int countryID, String Server, String regionalDatabase)
        {
           
            List<int> updateHdrs = null;
            await StandardPassengerWeights.LoadStandardWeights(Server, regionalDatabase);

            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
            var ctx = new SchedwinRegionalEntities(constring);
            {
                var hdrsToUpdate = newHdrs.Where(x => x.IsNew == false).ToList();
                await UpdateReservations(hdrsToUpdate, idxUser, countryID,  ctx);

                updateHdrs = hdrsToUpdate.Select(x => x.WishResHeader.Res_IDX).ToList();

                return updateHdrs;
            }
        
        }

        #endregion

        #region create section

        private tsch_WISHIntegrationHeader CreateWIHeader(WIReservationHeader wiResHeader, int idxUser, SchedwinRegionalEntities ctx)
        {
            var tschHeader = new tsch_ReservationHeader();
            tschHeader.Reservationname = wiResHeader.WishResHeader.ReservationName;
            tschHeader.DateCaptured = wiResHeader.WishResHeader.DateCaptured;
            tschHeader.Numpax = wiResHeader.WishResHeader.PaxCount;
            tschHeader.IDX_Operators = wiResHeader.OperatorID;
            tschHeader.IDX_OperatorAgent = null;
            tschHeader.IDX_Personnel = idxUser;
            tschHeader.Active = true;
            tschHeader.TicketPrinted = false;
            tschHeader.TicketRequired = true;
            tschHeader.IDX_ResClass = 2;
            tschHeader.IDX_ResType = 1;
            tschHeader.CURNCYID = wiResHeader.CurrencyCode;
            tschHeader.Notes = wiResHeader.Notes;
            tschHeader.WISHID = wiResHeader.WishResHeader.BookingID;
            switch (wiResHeader.ResStatus)
            {
                case "CONF": tschHeader.IDX_ResStatus = 2; break;
                case "CANC": tschHeader.IDX_ResStatus = 3; break;
                default: tschHeader.IDX_ResStatus = 1; break;
            }
            tschHeader.CoCode = "";

            var wishIntHeader = new tsch_WISHIntegrationHeader();
            wishIntHeader.WISHBookingID = wiResHeader.BookingID;
            wishIntHeader.WISHBookingStatus = wiResHeader.WishResHeader.WishResStatus;
            wishIntHeader.WISHPGID = wiResHeader.PartyGroupID;
            wishIntHeader.WISHPGPax = wiResHeader.Pax;
            wishIntHeader.DepartureDate = wiResHeader.DepartureDate;
            wishIntHeader.WISHPGName = wiResHeader.WishResHeader.PartyGroupName;
            wishIntHeader.tsch_ReservationHeader = tschHeader;
            wishIntHeader.WISHBookingStatus = wiResHeader.ResStatus;
            wishIntHeader.WishConsultant = wiResHeader.Consultant;
            ctx.tsch_WISHIntegrationHeader.Add(wishIntHeader);
            ctx.tsch_ReservationHeader.Add(tschHeader);

            return wishIntHeader;
        }

        private tsch_Passengers CreateWIPassenger(WIReservationPax wiResPax, tsch_ReservationHeader hdr, int CountryID,SchedwinRegionalEntities ctx)
        {
            var tschPax = new tsch_Passengers();
            tschPax.FirstName = wiResPax.ResPax.FirstName;
            tschPax.Surname = wiResPax.ResPax.Surname;
            tschPax.Age = Convert.ToByte(wiResPax.ResPax.Age);
            if (String.IsNullOrEmpty(wiResPax.ResPax.Sex))
                tschPax.Sex = "M";
            else
                tschPax.Sex = wiResPax.ResPax.Sex;
            tschPax.IDX_PaxType = 1;
            tschPax.WISHGuestID = wiResPax.ResPax.WishGuestID;
            tschPax.Weight = StandardPassengerWeights.GetStandardWeight(CountryID, tschPax.Sex == "M", tschPax.Age > 12);
            tschPax.Luggageweight = 44;
            tschPax.TicketPrinted = false;
            hdr.tsch_Passengers.Add(tschPax);
            ctx.tsch_Passengers.Add(tschPax);
            return tschPax;
        }

        private tsch_WishIntegrationLeg CreateWILeg(WIReservationLeg wiResLeg, tsch_WISHIntegrationHeader wiHdr, int idxUser, SchedwinRegionalEntities ctx)
        {
            if (wiResLeg.State==WIReservationLeg.DBLegState.IsNew)
            {

                var tschResLeg = new tsch_ReservationLegs();
                tschResLeg.BookingDate = wiResLeg.WishResLeg.BookingDate;
                tschResLeg.FromAp = wiResLeg.WishResLeg.IDX_FromAP;
                tschResLeg.ToAp = wiResLeg.WishResLeg.IDX_ToAP;
                tschResLeg.ExField = wiResLeg.WishResLeg.ExField;
                tschResLeg.ForField = wiResLeg.WishResLeg.ForField;
                tschResLeg.EarliestEx = new DateTime(tschResLeg.BookingDate.Year, tschResLeg.BookingDate.Month, tschResLeg.BookingDate.Day,
                                                      wiResLeg.WishResLeg.EarliestEx.Hour, wiResLeg.WishResLeg.EarliestEx.Minute,
                                                      wiResLeg.WishResLeg.EarliestEx.Second);

                tschResLeg.EarliestFor = new DateTime(tschResLeg.BookingDate.Year, tschResLeg.BookingDate.Month, tschResLeg.BookingDate.Day,
                                                       wiResLeg.WishResLeg.EarliestFor.Hour, wiResLeg.WishResLeg.EarliestFor.Minute,
                                                       wiResLeg.WishResLeg.EarliestFor.Second);


                tschResLeg.LatestEx = new DateTime(tschResLeg.BookingDate.Year, tschResLeg.BookingDate.Month, tschResLeg.BookingDate.Day,
                                                       wiResLeg.WishResLeg.LatestEx.Hour, wiResLeg.WishResLeg.LatestEx.Minute,
                                                       wiResLeg.WishResLeg.LatestEx.Second);

                tschResLeg.LatestFor = new DateTime(tschResLeg.BookingDate.Year, tschResLeg.BookingDate.Month, tschResLeg.BookingDate.Day,
                                       wiResLeg.WishResLeg.LatestFor.Hour, wiResLeg.WishResLeg.LatestFor.Minute,
                                       wiResLeg.WishResLeg.LatestFor.Second);



                tschResLeg.DirectDistance = Convert.ToInt16(wiResLeg.WishResLeg.Distance);
                if (wiResLeg.WishResLeg.Notes != null)
                    tschResLeg.Notes = wiResLeg.WishResLeg.Notes;
                else
                    tschResLeg.Notes = "";
                tschResLeg.SoleUse = wiResLeg.WishResLeg.SoleUse;
                tschResLeg.FOC = wiResLeg.WishResLeg.FOC;
                tschResLeg.Voucher = wiResLeg.WishResLeg.Voucher;
                tschResLeg.WISHIDLegs = wiResLeg.WishResLeg.WishSectorID;
                tschResLeg.Cancelled = false;

                tschResLeg.GameflightTime = 0;

                tschResLeg.IDX_SpecificACType = AircraftType.IDX_AC_NONE;

                if (wiResLeg.WishResLeg.CharterType != null)
                    tschResLeg.RateType = wiResLeg.WishResLeg.CharterType;
                else
                    tschResLeg.RateType = "";
                tschResLeg.TicketPrinted = false;
                tschResLeg.tsch_ReservationHeader = wiHdr.tsch_ReservationHeader;

                var wishIntLeg = new tsch_WishIntegrationLeg();
                wishIntLeg.WishSectorID = wiResLeg.WishResLeg.WishSectorID;
                wishIntLeg.ETA = wiResLeg.WishResLeg.ETA;
                wishIntLeg.ETD = wiResLeg.WishResLeg.ETD;
                wishIntLeg.WishFor = wiResLeg.WishResLeg.WishFor;
                wishIntLeg.WishEx = wiResLeg.WishResLeg.WishEx;
                wishIntLeg.tsch_WISHIntegrationHeader = wiHdr;
                wishIntLeg.tsch_ReservationLegs = tschResLeg;

                ctx.tsch_WishIntegrationLeg.Add(wishIntLeg);
                ctx.tsch_ReservationLegs.Add(tschResLeg);

                return wishIntLeg;
            }

            return null;
        }

        public tsch_ReservationLegBudget CreateDepartureTaxBudget(int idxFrom, int idxTo, int idxCompany, int paxCount, String currencyCode, double taxAmount)
        {
            var legBudget = new tsch_ReservationLegBudget();
            legBudget.IDXFrom = idxFrom;
            legBudget.IDXTo = idxTo;
            legBudget.RateType = "Departure Tax";
            legBudget.Currency = currencyCode;
            legBudget.Qty = paxCount;
            legBudget.FOC = false;
            legBudget.IDXPricelist = 999;
            legBudget.IDXACtype = AircraftType.IDX_AC_NONE;
            legBudget.Budget = taxAmount* paxCount;
            legBudget.Rate = taxAmount;

            return legBudget;
        }
        public tsch_ReservationLegBudget CreateWILegBudget(int idxFrom, int idxTo, int idxCompany, int Pax,String currencyCode,decimal budget)
        {

     
            var legBudget = new tsch_ReservationLegBudget();
            legBudget.IDXFrom = idxFrom;
            legBudget.IDXTo = idxTo;
            legBudget.RateType = "Seat";
            legBudget.Currency = currencyCode;
            legBudget.Qty = Pax;            
            legBudget.FOC = false;
            legBudget.IDXPricelist = 999;
            legBudget.IDXACtype = AircraftType.IDX_AC_NONE;
            legBudget.Budget = Convert.ToDouble( budget);
            legBudget.Rate = legBudget.Budget / Pax;

            return legBudget;
  
        }

        private async Task<Dictionary<WIReservationHeader, tsch_WISHIntegrationHeader>> CreateReservations(List<WIReservationHeader> newHdrs, int idxUser, int CountryID,
                                                                                                           SchedwinRegionalEntities ctx)
        {
            try
            {
                var airportFeesLst = AirportFee.GetAirportFees();
                var saveList = new Dictionary<WIReservationHeader, tsch_WISHIntegrationHeader>();
                var country = Country.GetCountry(CountryID);
                foreach (var newHdr in newHdrs)
                {
                    var newWIHeader = CreateWIHeader(newHdr, idxUser,  ctx);
                    saveList.Add(newHdr, newWIHeader);
                    foreach (var newLeg in newHdr.Legs)
                    {
                        if (newLeg.State==WIReservationLeg.DBLegState.IsNew)
                        {
                            var newWishIntLeg = CreateWILeg(newLeg, newWIHeader, idxUser, ctx);
                            if (newWishIntLeg!=null)
                            {
                                if  (newLeg.WishResLeg.VoucherCurrency !=null)
                                {

                       
                                    var legbudget = CreateWILegBudget(newLeg.WishResLeg.IDX_FromAP, newLeg.WishResLeg.IDX_ToAP, newHdr.OperatorID,
                                                     newHdr.Pax, newLeg.WishResLeg.VoucherCurrency, newLeg.WishResLeg.VoucherAmount);
                                    newWishIntLeg.tsch_ReservationLegs.tsch_ReservationLegBudget.Add(legbudget);
                                    ctx.tsch_ReservationLegBudget.Add(legbudget);

                                    var departureTax = airportFeesLst.FirstOrDefault(x => x.IDX_Airport == newLeg.WishResLeg.IDX_FromAP && x.FeeName == "Departure Tax"
                                                                                        && x.Currency == newLeg.WishResLeg.VoucherCurrency);
                                    if (departureTax != null)
                                    {
                                        legbudget.Budget -= departureTax.Amount;
                                        var taxbudget = CreateDepartureTaxBudget(newLeg.WishResLeg.IDX_FromAP, newLeg.WishResLeg.IDX_ToAP, newHdr.OperatorID,
                                                                            newHdr.Pax, departureTax.Currency, departureTax.Amount);
                                        newWishIntLeg.tsch_ReservationLegs.tsch_ReservationLegBudget.Add(taxbudget);
                                        ctx.tsch_ReservationLegBudget.Add(taxbudget);
                                    }


                                    double vatAmount =  (country.VATPercentage.Value / 100.0) + 1.0;                               
                                    legbudget.Budget = Math.Round( legbudget.Budget/ vatAmount,2);
                                    legbudget.Tax = Math.Round(legbudget.Budget * country.VATPercentage.Value / 100.0,2);
                                }

                            }
                                
                        }

                    }

                    foreach (var newPax in newHdr.PaxList)
                    {
                        CreateWIPassenger(newPax, newWIHeader.tsch_ReservationHeader, CountryID, ctx);
                    }
                }

                await ctx.SaveChangesAsync();
                return saveList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Update section

        private void UpdateWIHeader(List<tsch_WISHIntegrationHeader> dbHdrs, WIReservationHeader updatedHdr)
        {
            int IDX = updatedHdr.WishResHeader.Res_IDX;
            var oldDBHdr = dbHdrs.First(x => x.tsch_ReservationHeader.IDX == IDX);
            oldDBHdr.WISHPGPax = updatedHdr.Pax;
            oldDBHdr.WISHBookingStatus = updatedHdr.WishResHeader.WishResStatus;
            oldDBHdr.DepartureDate = updatedHdr.DepartureDate;
            oldDBHdr.WISHPGName = updatedHdr.WishResHeader.PartyGroupName;
            oldDBHdr.tsch_ReservationHeader.IDX_Personnel = updatedHdr.WishResHeader.IDX_Personnel;
            oldDBHdr.tsch_ReservationHeader.IDX_Operators = updatedHdr.WishResHeader.IDX_Operator;
            oldDBHdr.tsch_ReservationHeader.IDX_OperatorAgent = null;
            oldDBHdr.tsch_ReservationHeader.Numpax = updatedHdr.Pax;
            oldDBHdr.tsch_ReservationHeader.Reservationname = updatedHdr.ReservationName;
            oldDBHdr.tsch_ReservationHeader.Notes = updatedHdr.Notes;        
        }

        private void UpdateWILeg(List<tsch_WishIntegrationLeg> dbLegs, WIReservationLeg updatedLeg, int PaxCount)
        {

            var dbResLeg = dbLegs.First(x => x.tsch_ReservationLegs.IDX == updatedLeg.WishResLeg.IDX);

            dbResLeg.tsch_ReservationLegs.BookingDate = updatedLeg.WishResLeg.BookingDate;
            dbResLeg.ETA = updatedLeg.WishResLeg.ETA;
            dbResLeg.ETD = updatedLeg.WishResLeg.ETD;
            dbResLeg.WishEx = updatedLeg.WishResLeg.WishEx;
            dbResLeg.WishFor = updatedLeg.WishResLeg.WishFor;
            dbResLeg.tsch_ReservationLegs.ExField = updatedLeg.WishResLeg.ExField;
            dbResLeg.tsch_ReservationLegs.ForField = updatedLeg.WishResLeg.ForField;
            dbResLeg.tsch_ReservationLegs.Notes = updatedLeg.WishResLeg.Notes;
            dbResLeg.tsch_ReservationLegs.FOC = updatedLeg.WishResLeg.FOC;
            dbResLeg.tsch_ReservationLegs.SoleUse = updatedLeg.WishResLeg.SoleUse;
            if (updatedLeg.WishResLeg.CharterType != null)
                dbResLeg.tsch_ReservationLegs.RateType = updatedLeg.WishResLeg.CharterType;
            dbResLeg.tsch_ReservationLegs.Voucher = updatedLeg.WishResLeg.Voucher;

            dbResLeg.tsch_ReservationLegs.EarliestEx = new DateTime(dbResLeg.tsch_ReservationLegs.BookingDate.Year, dbResLeg.tsch_ReservationLegs.BookingDate.Month, dbResLeg.tsch_ReservationLegs.BookingDate.Day,
                                                                      updatedLeg.WishResLeg.EarliestEx.Hour, updatedLeg.WishResLeg.EarliestEx.Minute,
                                                                      updatedLeg.WishResLeg.EarliestEx.Second);

            dbResLeg.tsch_ReservationLegs.EarliestFor = new DateTime(dbResLeg.tsch_ReservationLegs.BookingDate.Year, dbResLeg.tsch_ReservationLegs.BookingDate.Month, dbResLeg.tsch_ReservationLegs.BookingDate.Day,
                                                                        updatedLeg.WishResLeg.EarliestFor.Hour, updatedLeg.WishResLeg.EarliestFor.Minute,
                                                                        updatedLeg.WishResLeg.EarliestFor.Second);


            dbResLeg.tsch_ReservationLegs.LatestEx = new DateTime(dbResLeg.tsch_ReservationLegs.BookingDate.Year, dbResLeg.tsch_ReservationLegs.BookingDate.Month, dbResLeg.tsch_ReservationLegs.BookingDate.Day,
                                                                   updatedLeg.WishResLeg.LatestEx.Hour, updatedLeg.WishResLeg.LatestEx.Minute,
                                                                   updatedLeg.WishResLeg.LatestEx.Second);

            dbResLeg.tsch_ReservationLegs.LatestFor = new DateTime(dbResLeg.tsch_ReservationLegs.BookingDate.Year, dbResLeg.tsch_ReservationLegs.BookingDate.Month, dbResLeg.tsch_ReservationLegs.BookingDate.Day,
                                                                   updatedLeg.WishResLeg.LatestFor.Hour, updatedLeg.WishResLeg.LatestFor.Minute,
                                                                   updatedLeg.WishResLeg.LatestFor.Second);

           
            var budgetLeg = dbResLeg.tsch_ReservationLegs.tsch_ReservationLegBudget.FirstOrDefault();
            if (budgetLeg!=null)
            {
                budgetLeg.Qty = PaxCount;
                budgetLeg.Budget = budgetLeg.Qty * budgetLeg.Rate;
                dbResLeg.tsch_ReservationLegs.Budget = budgetLeg.Budget;
            }


        }

        private async Task UpdateReservations(List<WIReservationHeader> Hdrs, int idxUser, int CountryID, SchedwinRegionalEntities ctx)
        {
            var country = Country.GetCountry(CountryID);
            var airportFeesLst = AirportFee.GetAirportFees();
            var hdrIDs = Hdrs.Select(x => x.WishResHeader.Res_IDX).ToList();
            var oldDBHdrs = await ctx.tsch_WISHIntegrationHeader.Include("tsch_ReservationHeader")
                                                               .Include("tsch_WishIntegrationLeg")
                                                               .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs")
                                                               .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_ReservationLegBudget")
                                                               .Where(x => hdrIDs.Contains(x.tsch_ReservationHeader.IDX)).ToListAsync();
            var oldLegs = oldDBHdrs.SelectMany(x => x.tsch_WishIntegrationLeg).ToList();

            await DeleteReservationPax(Hdrs, ctx);

            foreach (var hdr in Hdrs)
            {
                var tschHeader = oldDBHdrs.First(x => x.tsch_ReservationHeader.IDX == hdr.WishResHeader.Res_IDX);
                UpdateWIHeader(oldDBHdrs, hdr);

                foreach (var leg in hdr.Legs)
                {
                    switch (leg.State)
                    {

                        case WIReservationLeg.DBLegState.IsNew:

                        var tschNewLeg = CreateWILeg(leg, tschHeader, idxUser, ctx);
                            if (leg.WishResLeg.VoucherCurrency != null)
                            {
                                var tschLegBudget = CreateWILegBudget(leg.WishResLeg.IDX_FromAP, leg.WishResLeg.IDX_ToAP, hdr.OperatorID, hdr.Pax,
                                                        leg.WishResLeg.VoucherCurrency, leg.WishResLeg.VoucherAmount);
                                tschNewLeg.tsch_ReservationLegs.tsch_ReservationLegBudget.Add(tschLegBudget);
                                ctx.tsch_ReservationLegBudget.Add(tschLegBudget);

                                var departureTax = airportFeesLst.FirstOrDefault(x => x.IDX_Airport == leg.WishResLeg.IDX_FromAP && x.FeeName == "Departure Tax"
                                                                        && x.Currency == leg.WishResLeg.VoucherCurrency);
                                if (departureTax != null)
                                {
                                    tschLegBudget.Budget -= departureTax.Amount;
                                    var legbudget = CreateDepartureTaxBudget(leg.WishResLeg.IDX_FromAP, leg.WishResLeg.IDX_ToAP, hdr.OperatorID,
                                                                    hdr.Pax, departureTax.Currency, departureTax.Amount);
                                    tschNewLeg.tsch_ReservationLegs.tsch_ReservationLegBudget.Add(legbudget);
                                    ctx.tsch_ReservationLegBudget.Add(legbudget);
                                }

                                double vatAmount = (country.VATPercentage.Value / 100.0) + 1.0;
                                tschLegBudget.Budget = Math.Round(tschLegBudget.Budget / vatAmount,2);
                                tschLegBudget.Tax = Math.Round(tschLegBudget.Budget * country.VATPercentage.Value / 100.0,2);
                            }

                       
                        break;
                    case WIReservationLeg.DBLegState.IsModified:
                        UpdateWILeg(oldLegs, leg, hdr.Pax);
                        break;
                    }
                }

                foreach (var newPax in hdr.PaxList)
                    CreateWIPassenger(newPax, tschHeader.tsch_ReservationHeader, CountryID, ctx);

            }

            await ctx.SaveChangesAsync();

        }
        private async Task DeleteReservationPax(List<WIReservationHeader> hdrs, SchedwinRegionalEntities ctx)
        {
            var hdrIDs = hdrs.Select(x => x.WishResHeader.Res_IDX).ToList();
            var pax =await  ctx.tsch_Passengers.Where(x => hdrIDs.Contains(x.IDX_ResHeader)).ToListAsync();
            ctx.tsch_Passengers.RemoveRange(pax);
        }

        #endregion

    }
}
