using Schedwin.Data;
using Schedwin.Data.Classes;
using Schedwin.Reservations.Classes;
using Schedwin.WishIntegration.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.WishIntegration
{
    public class WishIntegrationGlobal
    {

        public async Task<List<Reservation>> WIGetReservatons(int bookingID)
        {

            var ctx = new SchedwinGlobalEntities();
            {

                var headers = await ctx.tbWISHIntegrationHeaders.Include("tbReservationHeader")
                                                                   .Include("tbReservationHeader.tbPassengers")
                                                                  .Include("tbWishIntegrationLegs")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg")
                                                                   //.Include("tbWishIntegrationLegs.tbReservationLeg.tsch_LegsRes")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg.tbAirstrip")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg.tbAirstrip1")
                                                                   .Include("tbReservationHeader.tbOperator")
                                                                   .Include("tbReservationHeader.tbReservationStatu")
                                                                   .Include("tbReservationHeader.tbUser")
                                                                   //.Include("tbReservationHeader.tset_Personnel1")
                                                                   .Where(x => x.WISHBookingID == bookingID).ToListAsync();

                if (headers != null)
                    return headers.Select(x => (Reservation)x).ToList();
                else
                    return null;
            }
        }

        public async Task<List<Reservation>> WIGetReservatons(List<WishBookingReference> wishBookingReferences)
        {

            var ctx = new SchedwinGlobalEntities();
            {
                var wishBookingIDS = wishBookingReferences.Select(x => x.WishBookingID).ToList();
                var wishPGroupIDs = wishBookingReferences.Select(x => x.WishPartyGroupID).ToList();
                var headers = await ctx.tbWISHIntegrationHeaders.Include("tbReservationHeader")
                                                                    .Include("tbReservationHeader.tbPassengers")
                                                                  .Include("tbWishIntegrationLegs")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg")
                                                                   //.Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_LegsRes")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg.tbAirstrip")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg.tbAirstrip1")
                                                                   .Include("tbReservationHeader.tbOperator")
                                                                   .Include("tbReservationHeader.tbReservationStatu")
                                                                   .Include("tbReservationHeader.tbUser")
                                                                   //.Include("tsch_reservationHeader.tset_Personnel1")
                                                                   .Where(x => wishBookingIDS.Contains(x.WISHBookingID)).ToListAsync();

                headers = headers.Where(x => wishPGroupIDs.Contains(x.WISHPGID)).ToList();

                if (headers != null)
                    return headers.Select(x => (Reservation)x).ToList();
                else
                    return null;
            }
        }

        public async Task<List<Reservation>> WIGetReservatons(List<int> lstlResHdrIDs)
        {
            var ctx = new SchedwinGlobalEntities();
            {

                var headers = await ctx.tbWISHIntegrationHeaders.Include("tbReservationHeader")
                                                                   .Include("tbReservationHeader.tbPassengers")
                                                                  .Include("tbWishIntegrationLegs")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg")
                                                                   //.Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_LegsRes")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg.tbAirstrip")
                                                                   .Include("tbWishIntegrationLegs.tbReservationLeg.tbAirstrip1")
                                                                   .Include("tbReservationHeader.tbOperator")
                                                                   .Include("tbReservationHeader.tbReservationStatu")
                                                                   .Include("tbReservationHeader.tbUser")
                                                                   //.Include("tsch_reservationHeader.tset_Personnel1")
                                                                   .Where(x => lstlResHdrIDs.Contains(x.tbReservationHeader.pkReservationHeaderID)).ToListAsync();


                if (headers != null)
                    return headers.Select(x => (Reservation)x).ToList();
                else
                    return null;
            }
        }

        public async Task<List<int>> WISaveNewReservations(List<WIReservationHeader> newHdrs, int idxUser, int countryID)
        {

            List<int> updateHdrs = null;
            await StandardPassengerWeights.LoadStandardWeights();

            var ctx = new SchedwinGlobalEntities();
            {
                var hrsToCreate = newHdrs.Where(x => x.IsNew).ToList();
                var newDbHdrs = await CreateReservations(hrsToCreate, idxUser, countryID, ctx);
                updateHdrs = newDbHdrs.Values.Select(x => x.tbReservationHeader.pkReservationHeaderID).ToList();

                return updateHdrs;
            }


        }

        public async Task<List<int>> WIUpdateReservations(List<WIReservationHeader> newHdrs, int idxUser, int countryID)
        {

            List<int> updateHdrs = new List<int>();
            await StandardPassengerWeights.LoadStandardWeights();

            var ctx = new SchedwinGlobalEntities();
            {
                var hdrsToUpdate = newHdrs.Where(x => x.IsNew == false).ToList();
                if (hdrsToUpdate!=null && hdrsToUpdate.Count >0)
                {
                    await UpdateReservations(hdrsToUpdate, idxUser, countryID, ctx);
                    updateHdrs.AddRange( hdrsToUpdate.Select(x => x.WishResHeader.Res_IDX).ToList());
                }

                return updateHdrs;
            }

        }

        #region create section

        private tbWISHIntegrationHeader CreateWIHeader(WIReservationHeader wiResHeader, int idxUser, int countryID, SchedwinGlobalEntities ctx)
        {
            var tbReservationHeader = new tbReservationHeader();
            tbReservationHeader.Reservationname = wiResHeader.WishResHeader.ReservationName;
            tbReservationHeader.DateCaptured = wiResHeader.WishResHeader.DateCaptured;
            tbReservationHeader.Numpax = wiResHeader.WishResHeader.PaxCount;
            tbReservationHeader.fkOperatorID = wiResHeader.OperatorID;
            tbReservationHeader.fkOperatorAgentID = null;
            tbReservationHeader.fkUserID = idxUser;
            tbReservationHeader.Active = true;
            tbReservationHeader.TicketPrinted = false;
            tbReservationHeader.TicketRequired = true;
            tbReservationHeader.fkResStatusID = 2;
            tbReservationHeader.fkResTypeID = 1;
            tbReservationHeader.CURNCYID = wiResHeader.CurrencyCode;
            tbReservationHeader.fkCountryID = countryID;
            tbReservationHeader.Notes = wiResHeader.Notes;
            tbReservationHeader.WISHID = wiResHeader.WishResHeader.BookingID;
            switch (wiResHeader.ResStatus)
            {
                case "CONF": tbReservationHeader.fkResStatusID = 2; break;
                case "CANC": tbReservationHeader.fkResStatusID = 3; break;
                default: tbReservationHeader.fkResStatusID = 1; break;
            }


            var wishIntHeader = new tbWISHIntegrationHeader();
            wishIntHeader.WISHBookingID = wiResHeader.BookingID;
            wishIntHeader.WISHBookingStatus = wiResHeader.WishResHeader.WishResStatus;
            wishIntHeader.WISHPGID = wiResHeader.PartyGroupID;
            wishIntHeader.WISHPGPax = wiResHeader.Pax;
            wishIntHeader.DepartureDate = wiResHeader.DepartureDate;
            wishIntHeader.WISHPGName = wiResHeader.WishResHeader.PartyGroupName;
            wishIntHeader.tbReservationHeader = tbReservationHeader;
            wishIntHeader.WISHBookingStatus = wiResHeader.ResStatus;
            wishIntHeader.WishConsultant = wiResHeader.Consultant;
            ctx.tbWISHIntegrationHeaders.Add(wishIntHeader);
            ctx.tbReservationHeaders.Add(tbReservationHeader);

            return wishIntHeader;
        }

        private tbPassenger CreateWIPassenger(WIReservationPax wiResPax, tbReservationHeader hdr, int CountryID, SchedwinGlobalEntities ctx)
        {
            var tbPax = new tbPassenger();
            tbPax.FirstName = wiResPax.ResPax.FirstName;
            tbPax.Surname = wiResPax.ResPax.Surname;
            tbPax.Age = Convert.ToByte(wiResPax.ResPax.Age);
            if (String.IsNullOrEmpty(wiResPax.ResPax.Sex))
                tbPax.Sex = "M";
            else
                tbPax.Sex = wiResPax.ResPax.Sex;
            tbPax.fkPassengerTypeID = 1;
            tbPax.WISHGuestID = wiResPax.ResPax.WishGuestID;
            tbPax.Weight = StandardPassengerWeights.GetStandardWeight(CountryID, tbPax.Sex == "M", tbPax.Age > 12);
            tbPax.Luggageweight = 44;
            tbPax.TicketPrinted = false;
            hdr.tbPassengers.Add(tbPax);
            ctx.tbPassengers.Add(tbPax);
            return tbPax;
        }

        private tbWishIntegrationLeg CreateWILeg(WIReservationLeg wiResLeg, tbWISHIntegrationHeader wiHdr, int idxUser, SchedwinGlobalEntities ctx)
        {
            if (wiResLeg.State == WIReservationLeg.DBLegState.IsNew)
            {

                var tbReservationLeg = new tbReservationLeg();
                tbReservationLeg.BookingDate = wiResLeg.WishResLeg.BookingDate;
                tbReservationLeg.fkFromAirstripID = wiResLeg.WishResLeg.IDX_FromAP;
                tbReservationLeg.fkToAirstripID = wiResLeg.WishResLeg.IDX_ToAP;
                tbReservationLeg.ExField = wiResLeg.WishResLeg.ExField;
                tbReservationLeg.ForField = wiResLeg.WishResLeg.ForField;
                tbReservationLeg.EarliestEx = new DateTime(tbReservationLeg.BookingDate.Year, tbReservationLeg.BookingDate.Month, tbReservationLeg.BookingDate.Day,
                                                      wiResLeg.WishResLeg.EarliestEx.Hour, wiResLeg.WishResLeg.EarliestEx.Minute,
                                                      wiResLeg.WishResLeg.EarliestEx.Second);

                tbReservationLeg.EarliestFor = new DateTime(tbReservationLeg.BookingDate.Year, tbReservationLeg.BookingDate.Month, tbReservationLeg.BookingDate.Day,
                                                       wiResLeg.WishResLeg.EarliestFor.Hour, wiResLeg.WishResLeg.EarliestFor.Minute,
                                                       wiResLeg.WishResLeg.EarliestFor.Second);


                tbReservationLeg.LatestEx = new DateTime(tbReservationLeg.BookingDate.Year, tbReservationLeg.BookingDate.Month, tbReservationLeg.BookingDate.Day,
                                                       wiResLeg.WishResLeg.LatestEx.Hour, wiResLeg.WishResLeg.LatestEx.Minute,
                                                       wiResLeg.WishResLeg.LatestEx.Second);

                tbReservationLeg.LatestFor = new DateTime(tbReservationLeg.BookingDate.Year, tbReservationLeg.BookingDate.Month, tbReservationLeg.BookingDate.Day,
                                       wiResLeg.WishResLeg.LatestFor.Hour, wiResLeg.WishResLeg.LatestFor.Minute,
                                       wiResLeg.WishResLeg.LatestFor.Second);



                tbReservationLeg.DirectDistance = Convert.ToInt16(wiResLeg.WishResLeg.Distance);
                if (wiResLeg.WishResLeg.Notes != null)
                    tbReservationLeg.Notes = wiResLeg.WishResLeg.Notes;
                else
                    tbReservationLeg.Notes = "";
                tbReservationLeg.SoleUse = wiResLeg.WishResLeg.SoleUse;
                tbReservationLeg.FOC = wiResLeg.WishResLeg.FOC;
                tbReservationLeg.Voucher = wiResLeg.WishResLeg.Voucher;
                tbReservationLeg.WISHIDLegs = wiResLeg.WishResLeg.WishSectorID;
                tbReservationLeg.Cancelled = false;
                tbReservationLeg.GameflightTime = 0;
                tbReservationLeg.fkACTypeID = AircraftType.IDX_AC_NONE;

                if (wiResLeg.WishResLeg.CharterType != null)
                    tbReservationLeg.RateType = wiResLeg.WishResLeg.CharterType;
                else
                    tbReservationLeg.RateType = "";
                tbReservationLeg.TicketPrinted = false;
                tbReservationLeg.tbReservationHeader = wiHdr.tbReservationHeader;

                var wishIntLeg = new tbWishIntegrationLeg();
                wishIntLeg.WishSectorID = wiResLeg.WishResLeg.WishSectorID;
                wishIntLeg.ETA = wiResLeg.WishResLeg.ETA;
                wishIntLeg.ETD = wiResLeg.WishResLeg.ETD;
                wishIntLeg.WishFor = wiResLeg.WishResLeg.WishFor;
                wishIntLeg.WishEx = wiResLeg.WishResLeg.WishEx;
                wishIntLeg.tbWISHIntegrationHeader = wiHdr;
                wishIntLeg.tbReservationLeg = tbReservationLeg;

                ctx.tbWishIntegrationLegs.Add(wishIntLeg);
                ctx.tbReservationLegs.Add(tbReservationLeg);

                return wishIntLeg;
            }

            return null;
        }

        public tbReservationLegBudget CreateDepartureTaxBudget(int idxFrom, int idxTo, int idxCompany, int paxCount, String currencyCode, double taxAmount)
        {
            var legBudget = new tbReservationLegBudget();
            legBudget.fkFromAPID = idxFrom;
            legBudget.fkToAPID = idxTo;
            legBudget.RateType = "Departure Tax";
            legBudget.Currency = currencyCode;
            legBudget.Qty = paxCount;
            legBudget.FOC = false;
            legBudget.fkPriceList = 999;
            legBudget.fkACTypeID = AircraftType.IDX_AC_NONE;
            legBudget.Budget = taxAmount * paxCount;
            legBudget.Rate = taxAmount;

            return legBudget;
        }

        public tbReservationLegBudget CreateWILegBudget(int idxFrom, int idxTo, int idxCompany, int Pax, String currencyCode, decimal budget)
        {


            var legBudget = new tbReservationLegBudget();
            legBudget.fkFromAPID = idxFrom;
            legBudget.fkToAPID = idxTo;
            legBudget.RateType = "Seat";
            legBudget.Currency = currencyCode;
            legBudget.Qty = Pax;
            legBudget.FOC = false;
            legBudget.fkPriceList = 999;
            legBudget.fkACTypeID = AircraftType.IDX_AC_NONE;
            legBudget.Budget = Convert.ToDouble(budget);
            legBudget.Rate = legBudget.Budget / Pax;

            return legBudget;

        }

        private async Task<Dictionary<WIReservationHeader, tbWISHIntegrationHeader>> CreateReservations(List<WIReservationHeader> newHdrs, int idxUser, int CountryID,
                                                                                                           SchedwinGlobalEntities ctx)
        {
            try
            {
                var airportFeesLst = AirportFee.GetAirportFees();
                var saveList = new Dictionary<WIReservationHeader, tbWISHIntegrationHeader>();
                var country = Country.GetCountry(CountryID);
                foreach (var newHdr in newHdrs)
                {
                    var newWIHeader = CreateWIHeader(newHdr, idxUser, CountryID, ctx);
                    saveList.Add(newHdr, newWIHeader);
                    foreach (var newLeg in newHdr.Legs)
                    {
                        if (newLeg.State == WIReservationLeg.DBLegState.IsNew)
                        {
                            var newWishIntLeg = CreateWILeg(newLeg, newWIHeader, idxUser, ctx);
                            if (newWishIntLeg != null)
                            {
                                if (newLeg.WishResLeg.VoucherCurrency != null)
                                {


                                    var legbudget = CreateWILegBudget(newLeg.WishResLeg.IDX_FromAP, newLeg.WishResLeg.IDX_ToAP, newHdr.OperatorID,
                                                     newHdr.Pax, newLeg.WishResLeg.VoucherCurrency, newLeg.WishResLeg.VoucherAmount);
                                    newWishIntLeg.tbReservationLeg.tbReservationLegBudgets.Add(legbudget);
                                    ctx.tbReservationLegBudgets.Add(legbudget);

                                    var departureTax = airportFeesLst.FirstOrDefault(x => x.IDX_Airport == newLeg.WishResLeg.IDX_FromAP && x.FeeName == "Departure Tax"
                                                                                        && x.Currency == newLeg.WishResLeg.VoucherCurrency);
                                    if (departureTax != null)
                                    {
                                        legbudget.Budget -= departureTax.Amount;
                                        var taxbudget = CreateDepartureTaxBudget(newLeg.WishResLeg.IDX_FromAP, newLeg.WishResLeg.IDX_ToAP, newHdr.OperatorID,
                                                                            newHdr.Pax, departureTax.Currency, departureTax.Amount);
                                        newWishIntLeg.tbReservationLeg.tbReservationLegBudgets.Add(taxbudget);
                                        ctx.tbReservationLegBudgets.Add(taxbudget);
                                    }


                                    double vatAmount = (country.VATPercentage.Value / 100.0) + 1.0;
                                    legbudget.Budget = Math.Round(legbudget.Budget / vatAmount, 2);
                                    legbudget.Tax = Math.Round(legbudget.Budget * country.VATPercentage.Value / 100.0, 2);
                                }

                            }

                        }

                    }

                    foreach (var newPax in newHdr.PaxList)
                    {
                        CreateWIPassenger(newPax, newWIHeader.tbReservationHeader, CountryID, ctx);
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

        private void UpdateWIHeader(List<tbWISHIntegrationHeader> dbHdrs, WIReservationHeader updatedHdr)
        {
            int IDX = updatedHdr.WishResHeader.Res_IDX;
            var oldDBHdr = dbHdrs.First(x => x.tbReservationHeader.pkReservationHeaderID == IDX);
            oldDBHdr.WISHPGPax = updatedHdr.Pax;
            oldDBHdr.WISHBookingStatus = updatedHdr.WishResHeader.WishResStatus;
            oldDBHdr.DepartureDate = updatedHdr.DepartureDate;
            oldDBHdr.WISHPGName = updatedHdr.WishResHeader.PartyGroupName;
            oldDBHdr.tbReservationHeader.fkUserID = updatedHdr.WishResHeader.IDX_Personnel;
            oldDBHdr.tbReservationHeader.fkOperatorID = updatedHdr.WishResHeader.IDX_Operator;
            oldDBHdr.tbReservationHeader.fkOperatorAgentID = null;
            oldDBHdr.tbReservationHeader.Numpax = updatedHdr.Pax;
            oldDBHdr.tbReservationHeader.Reservationname = updatedHdr.ReservationName;
            oldDBHdr.tbReservationHeader.Notes = updatedHdr.Notes;
        }
        private void UpdateWILeg(List<tbWishIntegrationLeg> dbLegs, WIReservationLeg updatedLeg, int PaxCount)
        {

            var dbResLeg = dbLegs.First(x => x.tbReservationLeg.pkReservationLegID == updatedLeg.WishResLeg.IDX);

            dbResLeg.tbReservationLeg.BookingDate = updatedLeg.WishResLeg.BookingDate;
            dbResLeg.ETA = updatedLeg.WishResLeg.ETA;
            dbResLeg.ETD = updatedLeg.WishResLeg.ETD;
            dbResLeg.WishEx = updatedLeg.WishResLeg.WishEx;
            dbResLeg.WishFor = updatedLeg.WishResLeg.WishFor;
            dbResLeg.tbReservationLeg.ExField = updatedLeg.WishResLeg.ExField;
            dbResLeg.tbReservationLeg.ForField = updatedLeg.WishResLeg.ForField;
            dbResLeg.tbReservationLeg.Notes = updatedLeg.WishResLeg.Notes;
            dbResLeg.tbReservationLeg.FOC = updatedLeg.WishResLeg.FOC;
            dbResLeg.tbReservationLeg.SoleUse = updatedLeg.WishResLeg.SoleUse;
            if (updatedLeg.WishResLeg.CharterType != null)
                dbResLeg.tbReservationLeg.RateType = updatedLeg.WishResLeg.CharterType;
            dbResLeg.tbReservationLeg.Voucher = updatedLeg.WishResLeg.Voucher;

            dbResLeg.tbReservationLeg.EarliestEx = new DateTime(dbResLeg.tbReservationLeg.BookingDate.Year, dbResLeg.tbReservationLeg.BookingDate.Month, dbResLeg.tbReservationLeg.BookingDate.Day,
                                                                      updatedLeg.WishResLeg.EarliestEx.Hour, updatedLeg.WishResLeg.EarliestEx.Minute,
                                                                      updatedLeg.WishResLeg.EarliestEx.Second);

            dbResLeg.tbReservationLeg.EarliestFor = new DateTime(dbResLeg.tbReservationLeg.BookingDate.Year, dbResLeg.tbReservationLeg.BookingDate.Month, dbResLeg.tbReservationLeg.BookingDate.Day,
                                                                        updatedLeg.WishResLeg.EarliestFor.Hour, updatedLeg.WishResLeg.EarliestFor.Minute,
                                                                        updatedLeg.WishResLeg.EarliestFor.Second);


            dbResLeg.tbReservationLeg.LatestEx = new DateTime(dbResLeg.tbReservationLeg.BookingDate.Year, dbResLeg.tbReservationLeg.BookingDate.Month, dbResLeg.tbReservationLeg.BookingDate.Day,
                                                                   updatedLeg.WishResLeg.LatestEx.Hour, updatedLeg.WishResLeg.LatestEx.Minute,
                                                                   updatedLeg.WishResLeg.LatestEx.Second);

            dbResLeg.tbReservationLeg.LatestFor = new DateTime(dbResLeg.tbReservationLeg.BookingDate.Year, dbResLeg.tbReservationLeg.BookingDate.Month, dbResLeg.tbReservationLeg.BookingDate.Day,
                                                                   updatedLeg.WishResLeg.LatestFor.Hour, updatedLeg.WishResLeg.LatestFor.Minute,
                                                                   updatedLeg.WishResLeg.LatestFor.Second);


            var budgetLeg = dbResLeg.tbReservationLeg.tbReservationLegBudgets.FirstOrDefault();
            if (budgetLeg != null)
            {
                budgetLeg.Qty = PaxCount;
                budgetLeg.Budget = budgetLeg.Qty * budgetLeg.Rate;
                dbResLeg.tbReservationLeg.Budget = budgetLeg.Budget;
            }


        }
        private async Task UpdateReservations(List<WIReservationHeader> Hdrs, int idxUser, int CountryID, SchedwinGlobalEntities ctx)
        {
            var country = Country.GetCountry(CountryID);
            var airportFeesLst = AirportFee.GetAirportFees();
            var hdrIDs = Hdrs.Select(x => x.WishResHeader.Res_IDX).ToList();
            var oldDBHdrs = await ctx.tbWISHIntegrationHeaders.Include("tbReservationHeader")
                                                               .Include("tbWishIntegrationLegs")
                                                               .Include("tbWishIntegrationLegs.tbReservationLeg")
                                                               .Include("tbWishIntegrationLegs.tbReservationLeg.tbReservationLegBudgets")
                                                               .Where(x => hdrIDs.Contains(x.tbReservationHeader.pkReservationHeaderID)).ToListAsync();
            var oldLegs = oldDBHdrs.SelectMany(x => x.tbWishIntegrationLegs).ToList();

            await DeleteReservationPax(Hdrs, ctx);

            foreach (var hdr in Hdrs)
            {
                var tbWISHIntegrationHeader = oldDBHdrs.First(x => x.tbReservationHeader.pkReservationHeaderID == hdr.WishResHeader.Res_IDX);
                UpdateWIHeader(oldDBHdrs, hdr);

                foreach (var leg in hdr.Legs)
                {
                    switch (leg.State)
                    {

                        case WIReservationLeg.DBLegState.IsNew:

                            var tschNewLeg = CreateWILeg(leg, tbWISHIntegrationHeader, idxUser, ctx);
                            if (leg.WishResLeg.VoucherCurrency != null)
                            {
                                var tbLegBudget = CreateWILegBudget(leg.WishResLeg.IDX_FromAP, leg.WishResLeg.IDX_ToAP, hdr.OperatorID, hdr.Pax,
                                                        leg.WishResLeg.VoucherCurrency, leg.WishResLeg.VoucherAmount);
                                tschNewLeg.tbReservationLeg.tbReservationLegBudgets.Add(tbLegBudget);
                                ctx.tbReservationLegBudgets.Add(tbLegBudget);

                                var departureTax = airportFeesLst.FirstOrDefault(x => x.IDX_Airport == leg.WishResLeg.IDX_FromAP && x.FeeName == "Departure Tax"
                                                                        && x.Currency == leg.WishResLeg.VoucherCurrency);
                                if (departureTax != null)
                                {
                                    tbLegBudget.Budget -= departureTax.Amount;
                                    var legbudget = CreateDepartureTaxBudget(leg.WishResLeg.IDX_FromAP, leg.WishResLeg.IDX_ToAP, hdr.OperatorID,
                                                                    hdr.Pax, departureTax.Currency, departureTax.Amount);
                                    tschNewLeg.tbReservationLeg.tbReservationLegBudgets.Add(legbudget);
                                    ctx.tbReservationLegBudgets.Add(legbudget);
                                }

                                double vatAmount = (country.VATPercentage.Value / 100.0) + 1.0;
                                tbLegBudget.Budget = Math.Round(tbLegBudget.Budget / vatAmount, 2);
                                tbLegBudget.Tax = Math.Round(tbLegBudget.Budget * country.VATPercentage.Value / 100.0, 2);
                            }


                            break;
                        case WIReservationLeg.DBLegState.IsModified:
                            UpdateWILeg(oldLegs, leg, hdr.Pax);
                            break;
                    }
                }

                foreach (var newPax in hdr.PaxList)
                    CreateWIPassenger(newPax, tbWISHIntegrationHeader.tbReservationHeader, CountryID, ctx);

            }

            await ctx.SaveChangesAsync();

        }
        private async Task DeleteReservationPax(List<WIReservationHeader> hdrs, SchedwinGlobalEntities ctx)
        {
            var hdrIDs = hdrs.Select(x => x.WishResHeader.Res_IDX).ToList();
            var pax = await ctx.tbPassengers.Where(x => hdrIDs.Contains(x.fkReservationHeaderID)).ToListAsync();
            ctx.tbPassengers.RemoveRange(pax);
        }

        #endregion
    }
}
