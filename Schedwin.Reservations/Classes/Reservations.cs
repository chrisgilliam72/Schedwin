using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;
using System.Data.Entity;
using Schedwin.Common;
using System.Data.Entity.Validation;
using Schedwin.Data.Classes;

namespace Schedwin.Reservations.Classes
{
    public class Reservations
    {
        public Dictionary<Guid,tsch_ReservationLegs> DBIDUpdateMap { get; set; }
        public String LastError { get; set; }

        public Reservations()
        {
            LastError = "";
            DBIDUpdateMap = new Dictionary<Guid, tsch_ReservationLegs>();
        }

        public async Task<Reservation> WIRefreshPassengersFromWish(Reservation reservation , String Server, String regionalDatabase)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
                var ctx = new SchedwinRegionalEntities(constring);
                {
                    int resHeaderIDX = reservation.Header.Res_IDX;
                    int countryID = GetCountryIDFromDB(regionalDatabase);
                    int pgID = reservation.Header.PartyGroupID;
                    var wishGuests= await ctx.vw_WishGuestsPartyGroup.Where(x => x.pkPartyGroupId == pgID).AsNoTracking().ToListAsync();
                    var lstPaxToRemove = new List<ReservationPax>();

                    foreach (var wishGuest in wishGuests)
                    {
                        var resPax = reservation.Passengers.FirstOrDefault(x => x.WishGuestID == wishGuest.pkGuestId);
                        if (resPax==null)
                        {
                            var tschPax = new tsch_Passengers();
                            tschPax.IDX = -1;
                            tschPax.IDX_ResHeader = reservation.Header.Res_IDX;
                            tschPax.WISHGuestID = wishGuest.pkGuestId;
                            tschPax.FirstName = wishGuest.Wish_First_Name;
                            tschPax.Surname = wishGuest.Wish_Surname;
                            tschPax.Passport = wishGuest.PassportNumber ?? "-";
                            tschPax.Nationality = wishGuest.PassportNationality ?? "-";
                            if (String.IsNullOrEmpty(wishGuest.Gender))
                                tschPax.Sex = "M";
                            else
                                tschPax.Sex = wishGuest.Gender;
                            if (wishGuest.Age.HasValue)
                                tschPax.Age = Convert.ToByte(wishGuest.Age.Value);
                            tschPax.Weight = StandardPassengerWeights.GetStandardWeight(countryID, tschPax.Sex == "M", tschPax.Age > 12, Server, regionalDatabase);
                            tschPax.Luggageweight = 44;
                             resPax = (ReservationPax)tschPax;
                            reservation.Passengers.Add(resPax);
                        }
                        else
                        {
                            resPax.FirstName = wishGuest.Wish_First_Name;
                            resPax.Surname= wishGuest.Wish_Surname;
                            resPax.PassportNo = wishGuest.PassportNumber ?? "-";
                            resPax.Nationality = wishGuest.PassportNationality ?? "-";
                            if (!String.IsNullOrEmpty(wishGuest.Gender))
                                resPax.Sex = wishGuest.Gender;
                            if (wishGuest.Age.HasValue)
                                resPax.Age = Convert.ToByte(wishGuest.Age.Value);
                        }
                        
                    }

                    foreach (var resGuest in reservation.Passengers)
                    {
                        var wishGuest = wishGuests.FirstOrDefault(x => x.pkGuestId == resGuest.WishGuestID);
                        if (wishGuest == null)
                            lstPaxToRemove.Add(resGuest);

                    }

                    foreach (var resGuest in lstPaxToRemove)
                        reservation.RemovePassenger(resGuest);

                    //await ctx.SaveChangesAsync();
                    return reservation;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                LastError = message;
                return null;
            }

        }


        public async Task<Reservation> LoadReservation(int idxResHdr, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var reservation = new Reservation();

                using (ctx)
                {
                    var tschResHdr = await ctx.tsch_ReservationHeader.Include("tset_Companies").
                                                                      Include("tset_Personnel1").
                                                                       Include("tset_Personnel").
                                                                       Include("tlst_ResType").
                                                                       Include("tlst_ResStatus").
                                                                       Include("tsch_Passengers").
                                                                       Include("tsch_ReservationLegs").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_GPExportInfo").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_Airports").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_Airports1").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_ACTypes").
                                                                       Include("tsch_ReservationLegs.tset_ACTypes").
                                                                       Include("tsch_ReservationLegs.tsch_LegsRes").
                                                                       Include("tsch_ReservationLegs.tset_Airports").
                                                                       Include("tsch_SplitReservations").
                                                                       Include("tsch_SplitReservations1").
                                                                       Include("tsch_ReservationLegs.tset_Airports1").FirstOrDefaultAsync(x => x.IDX == idxResHdr);

                    if (tschResHdr != null)
                    {
                        int masterBookingID = 0;
                        reservation.Header.Res_IDX = tschResHdr.IDX;
                        reservation.Header.ReservationName = tschResHdr.Reservationname;
                        reservation.Header.PaxCount = tschResHdr.Numpax;
                        reservation.Header.DateCaptured = tschResHdr.DateCaptured;
                        reservation.Header.IDX_Operator = tschResHdr.IDX_Operators;
                        reservation.Header.OperatorName = tschResHdr.tset_Companies.CompanyName;
                        reservation.Header.IDX_OperatorAgent = tschResHdr.IDX_OperatorAgent;
                        reservation.Header.OperatorAgentName = tschResHdr.tset_Personnel1.Firstname + " " + tschResHdr.tset_Personnel1.Surname;
                        reservation.Header.IDX_Personnel = tschResHdr.IDX_Personnel;
                        reservation.Header.SefofaneAgentName = tschResHdr.tset_Personnel.Firstname + " " + tschResHdr.tset_Personnel.Surname;
                        reservation.Header.IDX_ResStatus = tschResHdr.IDX_ResStatus;
                        reservation.Header.ReservationStatus = tschResHdr.tlst_ResStatus.ReservationStatus;
                        reservation.Header.Active = tschResHdr.Active;
                        reservation.Header.TicketPrinted = tschResHdr.TicketPrinted;
                        reservation.Header.TicketRequired = tschResHdr.TicketRequired;
                        reservation.Header.IDX_ResType = tschResHdr.IDX_ResType.Value;
                        reservation.Header.ReservationType = tschResHdr.tlst_ResType.ResType;
                        reservation.Header.ReservationNote = tschResHdr.Notes;
                        reservation.Header.CurrencyID = tschResHdr.CURNCYID;
                        if (tschResHdr.tsch_SplitReservations1.Count > 0)
                        {
                            reservation.Header.IsSplit = true;
                            masterBookingID = tschResHdr.tsch_SplitReservations1.First().fkParentReservation.Value;
                        }

                        else if (tschResHdr.tsch_SplitReservations.Count > 0)
                        {
                            reservation.Header.IsSplit = true;
                            masterBookingID = tschResHdr.tsch_SplitReservations.First().fkParentReservation.Value;
                        }


                        reservation.Header.IsMaster = masterBookingID == tschResHdr.IDX ? true : false; ;
                        var resPax = tschResHdr.tsch_Passengers.Select(x => (ReservationPax)x).ToList();
                        var resLegs = tschResHdr.tsch_ReservationLegs.Select(x => (ReservationLeg)x).ToList();


                        reservation.Legs.AddRange(resLegs);
                        reservation.Passengers.AddRange(resPax);



                    }
                    else
                        return null;

                    reservation.IsNew = false;
                    return reservation;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                LastError = message;
                return null;
            }

        }


        public async Task<Reservation> SaveReservation(Reservation updatedRes)
        {
            try
            {

                var ctx = new SchedwinGlobalEntities();
                tbReservationHeader savedHdr = null;

                Reservation savedReservation = null;

                if (updatedRes.IsNew)
                    savedHdr = await CreateReservation(ctx, updatedRes);
                else
                    savedHdr = await UpdateReservation(ctx, updatedRes);

                savedReservation = (Reservation)savedHdr;
                return savedReservation;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);

            }
        }

        public async Task<Reservation> SaveReservation (Reservation updatedRes, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                tsch_ReservationHeader savedHdr = null;

                Reservation savedReservation = null;

                if (updatedRes.IsNew)
                    savedHdr =await CreateReservation(ctx, updatedRes, Server, regionalDBName);
                else
                    savedHdr=  await UpdateReservation(ctx, updatedRes, Server, regionalDBName);

                savedReservation = (Reservation)savedHdr;
                return savedReservation;
            }
            catch   (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);
               
            }
        }

        private async Task<tbReservationHeader> CreateReservation(SchedwinGlobalEntities ctx, Reservation updatedRes)
        {
            var tbResHeader = (tbReservationHeader)updatedRes;

            ctx.tbReservationHeaders.Add(tbResHeader);

            foreach (var tbResLeg in tbResHeader.tbReservationLegs)
            {
                foreach (var tbResLegBudget in tbResLeg.tbReservationLegBudgets)
                    ctx.tbReservationLegBudgets.Add(tbResLegBudget);

                ctx.tbReservationLegs.Add(tbResLeg);

            }

            foreach (var tbPax in tbResHeader.tbPassengers)
                ctx.tbPassengers.Add(tbPax);



            await ctx.SaveChangesAsync();

            return tbResHeader;


        }

        private async Task<tsch_ReservationHeader> CreateReservation(SchedwinRegionalEntities ctx,Reservation updatedRes, String Server, string regionalDBName)
        {
            var tsch_resHdr = updatedRes.ToDBTree();
            ctx.tsch_ReservationHeader.Add(tsch_resHdr);

            foreach (var tschleg in tsch_resHdr.tsch_ReservationLegs)
            {
                foreach (var tschResLegBudget in tschleg.tsch_ReservationLegBudget)
                    ctx.tsch_ReservationLegBudget.Add(tschResLegBudget);

                ctx.tsch_ReservationLegs.Add(tschleg);
                
            }

            foreach (var tschpax in tsch_resHdr.tsch_Passengers)
                ctx.tsch_Passengers.Add(tschpax);

            

            await ctx.SaveChangesAsync();

            return tsch_resHdr;


        }
        private async Task<tsch_ReservationHeader> UpdateReservation(SchedwinRegionalEntities ctx, Reservation updatedRes, String Server, string regionalDBNam)
        {

            var LegDelList = new List<tsch_ReservationLegs>();

            var tschResHdrDB = await ctx.tsch_ReservationHeader.Include("tsch_Passengers").
                                                               Include("tsch_ReservationLegs").
                                                               Include("tsch_ReservationLegs.tsch_ReservationLegBudget").
                                                               FirstAsync(x => x.IDX == updatedRes.Header.Res_IDX);

            var tschResHdrUpdate = updatedRes.ToDBTree();


            tschResHdrDB.Reservationname = tschResHdrUpdate.Reservationname;
            tschResHdrDB.Numpax = tschResHdrUpdate.Numpax;

            tschResHdrDB.IDX_Operators = tschResHdrUpdate.IDX_Operators;
            tschResHdrDB.IDX_OperatorAgent = tschResHdrUpdate.IDX_OperatorAgent;
            tschResHdrDB.IDX_Personnel = tschResHdrUpdate.IDX_Personnel;
            tschResHdrDB.IDX_ResStatus = tschResHdrUpdate.IDX_ResStatus;

            tschResHdrDB.TicketPrinted = tschResHdrUpdate.TicketPrinted;
            tschResHdrDB.TicketRequired = tschResHdrUpdate.TicketRequired;
            tschResHdrDB.Active = tschResHdrUpdate.Active;
            tschResHdrDB.IDX_ResType = tschResHdrUpdate.IDX_ResType;
            tschResHdrDB.Notes = tschResHdrUpdate.Notes;
            tschResHdrDB.CURNCYID = tschResHdrUpdate.CURNCYID;
            tschResHdrDB.WISHID = tschResHdrUpdate.WISHID;


            foreach (var tschleg in tschResHdrUpdate.tsch_ReservationLegs)
            {


                var tschDBLeg = tschResHdrDB.tsch_ReservationLegs.FirstOrDefault(x => x.IDX == tschleg.IDX);
                if (tschDBLeg != null)
                {

                    tschDBLeg.BookingDate = tschleg.BookingDate;
                    tschDBLeg.FromAp = tschleg.FromAp;
                    tschDBLeg.ToAp = tschleg.ToAp;
                    tschDBLeg.IDX_SpecificACType = tschleg.IDX_SpecificACType;

                    tschDBLeg.ExField = tschleg.ExField;
                    tschDBLeg.ForField = tschleg.ForField;
                    tschDBLeg.EarliestEx = tschleg.EarliestEx;
                    tschDBLeg.LatestEx = tschleg.LatestEx;
                    tschDBLeg.EarliestFor = tschleg.EarliestFor;
                    tschDBLeg.LatestFor = tschleg.LatestFor;

                    tschDBLeg.DirectDistance = tschleg.DirectDistance;
                    tschDBLeg.GameflightTime = tschleg.GameflightTime;
                    tschDBLeg.TicketPrinted = tschleg.TicketPrinted;
                    tschDBLeg.SoleUse = tschleg.SoleUse;
                    tschDBLeg.Cancelled = tschleg.Cancelled;
                    tschDBLeg.Voucher = tschleg.Voucher;
                    tschDBLeg.INV_Number = tschleg.INV_Number;
                    tschDBLeg.Notes = tschleg.Notes;
                    tschDBLeg.RateType = tschleg.RateType;

                    var budgets = tschleg.tsch_ReservationLegBudget;



                    foreach (var tschResBudget in budgets)
                    {
                        if (tschResBudget.IDX < 0)
                        {
                            var dbLegBudgetToDelete = tschDBLeg.tsch_ReservationLegBudget.FirstOrDefault(x => x.IDX == tschResBudget.IDX * -1);
                            if (dbLegBudgetToDelete != null)
                            {
                                tschDBLeg.tsch_ReservationLegBudget.Remove(dbLegBudgetToDelete);
                                ctx.tsch_ReservationLegBudget.Remove(dbLegBudgetToDelete);
                            }
                        }
                        else
                        {
                            if (tschResBudget.IDX == 0)
                            {
                                tschDBLeg.tsch_ReservationLegBudget.Add(tschResBudget);
                                ctx.tsch_ReservationLegBudget.Add(tschResBudget);
                            }
                            else
                            {
                                var dbLegBudget = tschDBLeg.tsch_ReservationLegBudget.FirstOrDefault(x => x.IDX == tschResBudget.IDX);
                                if (dbLegBudget != null)
                                {
                                    dbLegBudget.IDXFrom = tschResBudget.IDXFrom;
                                    dbLegBudget.IDXTo = tschResBudget.IDXTo;
                                    dbLegBudget.IDXACtype = tschResBudget.IDXACtype;
                                    dbLegBudget.Currency = tschResBudget.Currency;
                                    dbLegBudget.RateType = tschResBudget.RateType;
                                    dbLegBudget.Budget = tschResBudget.Budget;
                                    dbLegBudget.FOC = tschResBudget.FOC;
                                    dbLegBudget.Qty = tschResBudget.Qty;
                                    dbLegBudget.Rate = tschResBudget.Rate;
                                }
                            }
                        }

                    }

                    var budgetsToDelete = budgets.Where(x => x.IDX < 0).ToList();
                    tschleg.tsch_ReservationLegBudget = budgets.Except(budgetsToDelete).ToList();
                }
                else
                {

                    tschResHdrDB.tsch_ReservationLegs.Add(tschleg);
                    ctx.tsch_ReservationLegs.Add(tschleg);
                    foreach (var tschResBudget in tschleg.tsch_ReservationLegBudget)
                        ctx.tsch_ReservationLegBudget.Add(tschResBudget);
                }

            }

            foreach (var tschDBLeg in tschResHdrDB.tsch_ReservationLegs)
            {
                var updateLeg = tschResHdrUpdate.tsch_ReservationLegs.FirstOrDefault(x => x.IDX == tschDBLeg.IDX);
                if (updateLeg == null)
                {
                    ctx.tsch_ReservationLegBudget.RemoveRange(tschDBLeg.tsch_ReservationLegBudget);
                    tschDBLeg.tsch_ReservationLegBudget.Clear();


                    LegDelList.Add(tschDBLeg);

                }

            }


            ctx.tsch_ReservationLegs.RemoveRange(LegDelList);



            var paxToUpdate = updatedRes.Passengers.Where(x => x.IsModified && !x.IsDeleted && !x.IsNew).ToList();

            foreach (var tschPax in tschResHdrUpdate.tsch_Passengers)
            {
                if (tschPax.IDX < 1)
                {
                    tschResHdrDB.tsch_Passengers.Add(tschPax);
                    ctx.tsch_Passengers.Add(tschPax);
                }
                else
                {
                    var dbTschPax = tschResHdrDB.tsch_Passengers.FirstOrDefault(x => x.IDX == tschPax.IDX);
                    if (dbTschPax != null)
                    {
                        dbTschPax.FirstName = tschPax.FirstName;
                        dbTschPax.Surname = tschPax.Surname;
                        dbTschPax.Weight = tschPax.Weight;
                        dbTschPax.Nationality = tschPax.Nationality;
                        dbTschPax.Passport = tschPax.Passport;
                        dbTschPax.Age = tschPax.Age;
                        dbTschPax.Sex = tschPax.Sex;
                        dbTschPax.Luggageweight = tschPax.Luggageweight;

                    }
                }
            }

            foreach (var pax in updatedRes.DeletedPax)
            {
                var paxToDel = tschResHdrDB.tsch_Passengers.FirstOrDefault(x => x.IDX == pax.IDX);
                if (paxToDel != null)
                {
                    tschResHdrDB.tsch_Passengers.Remove(paxToDel);
                    ctx.tsch_Passengers.Remove(paxToDel);

                }

            }

            updatedRes.DeletedPax.Clear();

            await ctx.SaveChangesAsync();


            return tschResHdrUpdate;
        }
        private async Task<tbReservationHeader> UpdateReservation(SchedwinGlobalEntities ctx, Reservation updatedRes)
        {

            var LegDelList = new List<tbReservationLeg>();

            var tbResHdrDB = await ctx.tbReservationHeaders.Include("tbPassengers").
                                                               Include("tbReservationLegs").
                                                               Include("tbReservationLegs.tbReservationLegBudgets").
                                                               FirstAsync(x => x.pkReservationHeaderID == updatedRes.Header.Res_IDX);

            var tbResHdrUpdate = (tbReservationHeader)updatedRes;


            tbResHdrDB.Reservationname = tbResHdrUpdate.Reservationname;
            tbResHdrDB.Numpax = tbResHdrUpdate.Numpax;

            tbResHdrDB.fkOperatorID = tbResHdrUpdate.fkOperatorID;
            tbResHdrDB.fkOperatorAgentID = tbResHdrUpdate.fkOperatorAgentID;
            tbResHdrDB.fkUserID = tbResHdrUpdate.fkUserID;
            tbResHdrDB.fkResStatusID = tbResHdrUpdate.fkResStatusID;

            tbResHdrDB.TicketPrinted = tbResHdrUpdate.TicketPrinted;
            tbResHdrDB.TicketRequired = tbResHdrUpdate.TicketRequired;
            tbResHdrDB.Active = tbResHdrUpdate.Active;
            tbResHdrDB.fkResTypeID = tbResHdrUpdate.fkResTypeID;
            tbResHdrDB.Notes = tbResHdrUpdate.Notes;
            tbResHdrDB.CURNCYID = tbResHdrUpdate.CURNCYID;
            tbResHdrDB.WISHID = tbResHdrUpdate.WISHID;


            foreach (var tbResLeg in tbResHdrUpdate.tbReservationLegs)
            {
                

                var tbDBLeg = tbResHdrDB.tbReservationLegs.FirstOrDefault(x => x.pkReservationLegID == tbResLeg.pkReservationLegID);
                if (tbDBLeg != null)
                {

                    tbDBLeg.BookingDate = tbResLeg.BookingDate;
                    tbDBLeg.fkFromAirstripID = tbResLeg.fkFromAirstripID;
                    tbDBLeg.fkToAirstripID = tbResLeg.fkToAirstripID;
                    tbDBLeg.fkToAirstripID = tbResLeg.fkToAirstripID;

                    tbDBLeg.ExField = tbResLeg.ExField;
                    tbDBLeg.ForField = tbResLeg.ForField;
                    tbDBLeg.EarliestEx = tbResLeg.EarliestEx;
                    tbDBLeg.LatestEx = tbResLeg.LatestEx;
                    tbDBLeg.EarliestFor = tbResLeg.EarliestFor;
                    tbDBLeg.LatestFor = tbResLeg.LatestFor;

                    tbDBLeg.DirectDistance = tbResLeg.DirectDistance;
                    tbDBLeg.GameflightTime = tbResLeg.GameflightTime;
                    tbDBLeg.TicketPrinted = tbResLeg.TicketPrinted;
                    tbDBLeg.SoleUse = tbResLeg.SoleUse;
                    tbDBLeg.Cancelled = tbResLeg.Cancelled;
                    tbDBLeg.Voucher = tbResLeg.Voucher;
                    tbDBLeg.INV_Number = tbResLeg.INV_Number;
                    tbDBLeg.Notes = tbResLeg.Notes;
                    tbDBLeg.RateType = tbResLeg.RateType;

                    var budgets = tbResLeg.tbReservationLegBudgets;

                    foreach (var tbResBudget in budgets)
                    {
                        if (tbResBudget.pkReservationLegBudgetID < 0)
                        {
                            var dbLegBudgetToDelete = tbDBLeg.tbReservationLegBudgets.FirstOrDefault(x => x.pkReservationLegBudgetID == tbResBudget.pkReservationLegBudgetID * -1);
                            if (dbLegBudgetToDelete != null)
                            {
                                tbDBLeg.tbReservationLegBudgets.Remove(dbLegBudgetToDelete);
                                ctx.tbReservationLegBudgets.Remove(dbLegBudgetToDelete);
                            }
                        }
                        else
                        {
                            if (tbResBudget.pkReservationLegBudgetID==0)
                            {
                                tbDBLeg.tbReservationLegBudgets.Add(tbResBudget);
                                ctx.tbReservationLegBudgets.Add(tbResBudget);
                            }
                            else
                            {
                                var dbLegBudget = tbDBLeg.tbReservationLegBudgets.FirstOrDefault(x => x.pkReservationLegBudgetID == tbResBudget.pkReservationLegBudgetID);
                                if (dbLegBudget != null)
                                {
                                    dbLegBudget.fkFromAPID = tbResBudget.fkFromAPID;
                                    dbLegBudget.fkToAPID = tbResBudget.fkToAPID;
                                    dbLegBudget.fkACTypeID = tbResBudget.fkACTypeID;
                                    dbLegBudget.Currency = tbResBudget.Currency;
                                    dbLegBudget.RateType = tbResBudget.RateType;
                                    dbLegBudget.Budget = tbResBudget.Budget;
                                    dbLegBudget.FOC = tbResBudget.FOC;
                                    dbLegBudget.Qty = tbResBudget.Qty;
                                    dbLegBudget.Rate = tbResBudget.Rate;
                                }
                            }
                        }

                    }

                    var budgetsToDelete = budgets.Where(x => x.pkReservationLegBudgetID < 0).ToList();
                    tbResLeg.tbReservationLegBudgets = budgets.Except(budgetsToDelete).ToList();
                }
                else
                {

                    tbResHdrDB.tbReservationLegs.Add(tbResLeg);
                    ctx.tbReservationLegs.Add(tbResLeg);
                    foreach (var tbResLegBudget in tbResLeg.tbReservationLegBudgets)
                        ctx.tbReservationLegBudgets.Add(tbResLegBudget);
                }

            }

            foreach (var tbDBLeg in tbResHdrDB.tbReservationLegs)
            {
                var updateLeg = tbResHdrUpdate.tbReservationLegs.FirstOrDefault(x => x.pkReservationLegID == tbDBLeg.pkReservationLegID);
                if (updateLeg == null)
                {
                    ctx.tbReservationLegBudgets.RemoveRange(tbDBLeg.tbReservationLegBudgets);
                    tbDBLeg.tbReservationLegBudgets.Clear();
                    LegDelList.Add(tbDBLeg);
                }                                    
            }


            ctx.tbReservationLegs.RemoveRange(LegDelList);

            var paxToUpdate = updatedRes.Passengers.Where(x => x.IsModified && !x.IsDeleted && !x.IsNew).ToList();     
            
            foreach (var tbPax in tbResHdrUpdate.tbPassengers)
            {
                if (tbPax.pkPassengerID<1)
                {
                    tbResHdrDB.tbPassengers.Add(tbPax);
                    ctx.tbPassengers.Add(tbPax);
                }
                else
                {
                    var dbTschPax = tbResHdrDB.tbPassengers.FirstOrDefault(x => x.pkPassengerID == tbPax.pkPassengerID);
                    if (dbTschPax != null)
                    {
                        dbTschPax.FirstName = tbPax.FirstName;
                        dbTschPax.Surname = tbPax.Surname;
                        dbTschPax.Weight = tbPax.Weight;
                        dbTschPax.Nationality = tbPax.Nationality;
                        dbTschPax.Passport = tbPax.Passport;
                        dbTschPax.Age = tbPax.Age;
                        dbTschPax.Sex = tbPax.Sex;
                        dbTschPax.Luggageweight = tbPax.Luggageweight;

                    }
                }
            }

            foreach (var pax in updatedRes.DeletedPax)
            {
                var paxToDel = tbResHdrUpdate.tbPassengers.FirstOrDefault(x => x.pkPassengerID == pax.IDX);
                if (paxToDel!=null)
                {
                    tbResHdrDB.tbPassengers.Remove(paxToDel);
                    ctx.tbPassengers.Remove(paxToDel);

                }

            }
           
            updatedRes.DeletedPax.Clear(); 

            await ctx.SaveChangesAsync();


            return tbResHdrUpdate;
        }

        public async Task<List<int>> GetReservationIDS(String partialReservationName)
        {
            try
            {
                var ctx = new SchedwinGlobalEntities();
                var resHdrIDs = await ctx.tbReservationHeaders.Where(x => x.Reservationname.Contains(partialReservationName)).Select(x => x.pkReservationHeaderID).ToListAsync();
                resHdrIDs = resHdrIDs.Distinct().ToList();

                return resHdrIDs;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);
            }
        }

        public async Task<List<int>> GetReservationIDS(String partialReservationName, String Server, string regionalDBName)
        {
            try
            {
                
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var resHdrIDs = await ctx.tsch_ReservationHeader.Where(x=>x.Reservationname.Contains(partialReservationName)).Select(x=>x.IDX).ToListAsync();
                resHdrIDs = resHdrIDs.Distinct().ToList();
                
                return resHdrIDs;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);
            }
        }

        public async Task<List<int>> GetReservationIDS(DateTime date, int resType)
        {

            DateTime nextDay = date.AddDays(1);

            var ctx = new SchedwinGlobalEntities();
            var resHdrIDs = await ctx.tbReservationLegs.Where(x => x.BookingDate >= date && x.BookingDate < nextDay).Select(x => x.fkReservationHeaderID).ToListAsync();
            resHdrIDs = resHdrIDs.Distinct().ToList();
            return resHdrIDs;
            
         
        }


        public async Task<List<int>> GetReservationIDS (DateTime date, int resType, String Server, string regionalDBName)
        {
            try
            {
                DateTime nextDay = date.AddDays(1);
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var resHdrIDs = await ctx.tsch_ReservationLegs.Where(x => x.BookingDate >= date && x.BookingDate < nextDay).Select(x => x.IDX_ResHeader).ToListAsync();
                resHdrIDs = resHdrIDs.Distinct().ToList();
                return resHdrIDs;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);
            }
        }


        public async Task<List<Reservation>> GetReservations(List<int> resHdrIDs, int resType)
        {
         
            var ctx = new SchedwinGlobalEntities();
            ctx.Database.CommandTimeout = 180;
            using (ctx)
            {
                var tbHeaders = await ctx.tbReservationHeaders. 
                                       Include("tbPassengers").
                                       Include("tbReservationLegs").
                                       Include("tbUser").
                                       Include("tbOperator").
                                       Include("tbReservationLegs.tbAirstrip").
                                       Include("tbReservationLegs.tbAirstrip1").
                                       Include("tbOperator").
                                       Include("tbReservationType").
                                       Include("tbReservationStatu").
                                       Include("tbWISHIntegrationHeaders").
                                       //Include("tset_Personnel1").
                                       //Include("tset_Personnel").
                                       //Include("tsch_ReservationLegs.tset_ACTypes").
                                       //Include("tsch_ReservationLegs.tsch_LegsRes").
                                       //Include("tsch_SplitReservations").
                                       //Include("tsch_SplitReservations1").
                                       //Include("tsch_ReservationLegs.tsch_LegsRes.tsch_Legs").
                                       //Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot").
                                       //Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot.tset_ACDetails").
                                       //Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot.tset_Personnel").
                                       Where(x => resHdrIDs.Contains(x.pkReservationHeaderID)).
                                       ToListAsync();


                var resList = tbHeaders.Select(x => (Reservation)x).OrderBy(x => x.Header.ReservationName).ToList();
                resList.ForEach(x => x.IsNew = false);
                return resList;

            }

      
        }

        public async Task<List<Reservation>> GetReservations(List<int> resHdrIDs, int resType, String Server, string regionalDBName)
        {
            try
            {
                var tmptschLst = new List<tsch_ReservationHeader>();

                int lstCount = resHdrIDs.Count;
                tmptschLst = await GetReservationsPvt(resHdrIDs, Server, regionalDBName);
                //var tmpTask4 = GetReservationsPvt(resHdrIDs.GetRange((lstCount / 2), lstCount- (lstCount / 2)), Server, regionalDBName);
                //var tmpResListArray = await Task.WhenAll(tmpTask1, tmpTask4);
                //tmptschLst.AddRange(tmpResListArray[0]);
                //tmptschLst.AddRange(tmpResListArray[1]);

                var resList = tmptschLst.Select(x => (Reservation)x).OrderBy(x => x.Header.ReservationName).ToList();
                resList.ForEach(x => x.IsNew = false);
                return resList;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);

            }
        }

        public async Task<List<Reservation>> GetReservations(String partialReservationName, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);



                using (ctx)
                {
                    var tschResLegs = await ctx.tsch_ReservationLegs.Include("tsch_ReservationHeader").
                                                    Include("tsch_ReservationHeader.tsch_WISHIntegrationHeader").
                                                    Include("tsch_ReservationHeader.tset_Companies").
                                                    Include("tsch_ReservationHeader.tset_Personnel1").
                                                     Include("tsch_ReservationHeader.tset_Personnel").
                                                     Include("tsch_ReservationHeader.tlst_ResType").
                                                     Include("tsch_ReservationHeader.tlst_ResStatus").
                                                     Include("tsch_ReservationHeader.tsch_Passengers").
                                                     Include("tsch_ReservationLegBudget").
                                                     Include("tsch_ReservationLegBudget.tset_GPExportInfo").
                                                     Include("tsch_ReservationLegBudget.tset_Airports").
                                                     Include("tsch_ReservationLegBudget.tset_Airports1").
                                                     Include("tsch_ReservationLegBudget.tset_ACTypes").
                                                     Include("tset_ACTypes").
                                                     Include("tsch_LegsRes").
                                                     Include("tset_Airports").
                                                     Include("tset_Airports1").
                                                     Include("tsch_ReservationHeader.tsch_SplitReservations").
                                                     Include("tsch_ReservationHeader.tsch_SplitReservations1").
                                                     Include("tsch_LegsRes.tsch_Legs").
                                                     Include("tsch_LegsRes.tsch_AC_Pilot").
                                                     Include("tsch_LegsRes.tsch_AC_Pilot.tset_ACDetails").
                                                     Include("tsch_LegsRes.tsch_AC_Pilot.tset_Personnel").
                                                     Where(x => x.tsch_ReservationHeader.Reservationname.Contains(partialReservationName)).
                                                     ToListAsync();

                    var tschResHeaders = tschResLegs.Select(x => x.tsch_ReservationHeader).ToList();
                    var reservations = tschResHeaders.Select(x => (Reservation)x).OrderByDescending(x => x.Header.DateCaptured).ToList();
                    reservations = reservations.GroupBy(x => x.Header.ReservationName).Select(x => x.First()).ToList();
                    reservations.ForEach(x => x.IsNew = false);
                    return reservations;
                }


            }
            catch (Exception ex)
            {

                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return null;
            }
        }

        public async Task<List<ReservationLegBudget>> GetReservationLegBudgets (int idxLegID, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var tschLegBudgets= await ctx.tsch_ReservationLegBudget. Include("tset_GPExportInfo").
                                                                        Include("tset_Airports").
                                                                        Include("tset_Airports1").
                                                                        Include("tset_ACTypes").
                                                                        Where(x => x.IDXResLeg == idxLegID).ToListAsync();

                    var resLegBudgets = tschLegBudgets.Select(x => (ReservationLegBudget)x).ToList();
                    return resLegBudgets;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return null;
            }
        }

        public async Task<bool> UpdateReservationLegBudgets(int idxLegID, List<ReservationLegBudget> legBudgets)
        {
            try
            {

                var ctx = new SchedwinGlobalEntities();
                using (ctx)
                {
                    var tbReservationLeg = await ctx.tbReservationLegs.Include("tbReservationLegBudgets").
                                                                            Where(x => x.pkReservationLegID == idxLegID).FirstOrDefaultAsync();

                    var tbResLegBudgets = tbReservationLeg.tbReservationLegBudgets.ToList();
                    var newtbLegBudgets = legBudgets.Where(x => x.DBState != DBState.IsDeleted).Select(x => (tbReservationLegBudget)x).ToList();
                    var budgetsToDelete = legBudgets.Where(x => x.DBState == DBState.IsDeleted).ToList();
                    foreach (var tbLegBudget in newtbLegBudgets)
                    {
                        var oldBudget = tbResLegBudgets.FirstOrDefault(x => x.pkReservationLegBudgetID == tbLegBudget.pkReservationLegBudgetID);
                        if (oldBudget != null)
                        {

                            oldBudget.fkFromAPID = tbLegBudget.fkFromAPID;
                            oldBudget.fkToAPID = tbLegBudget.fkToAPID;
                            oldBudget.fkACTypeID = tbLegBudget.fkACTypeID;
                            oldBudget.Currency = tbLegBudget.Currency.TrimEnd(' ');
                            oldBudget.RateType = tbLegBudget.RateType;
                            oldBudget.fkPriceList = tbLegBudget.fkPriceList;
                            oldBudget.FOC = tbLegBudget.FOC;
                            oldBudget.Qty = tbLegBudget.Qty;
                            oldBudget.Rate = tbLegBudget.Rate;
                            oldBudget.Budget = tbLegBudget.Budget;
                        }
                        else
                        {
                            tbReservationLeg.tbReservationLegBudgets.Add(tbLegBudget);
                            ctx.tbReservationLegBudgets.Add(tbLegBudget);
                        }

                    }

                    foreach (var delBudget in budgetsToDelete)
                    {
                        var oldBudget = tbResLegBudgets.FirstOrDefault(x => x.pkReservationLegBudgetID == delBudget.IDX);
                        if (oldBudget != null)
                        {
                            tbReservationLeg.tbReservationLegBudgets.Remove(oldBudget);
                            ctx.tbReservationLegBudgets.Remove(oldBudget);
                        }
                           

                    }

                    await ctx.SaveChangesAsync();
                }

                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);
            }
        }

        public async Task <bool> UpdateReservationLegBudgets(int idxLegID,List<ReservationLegBudget> legBudgets, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var tsch_ReservationLeg = await ctx.tsch_ReservationLegs.Include("tsch_ReservationLegBudget").
                                                                            Where(x => x.IDX == idxLegID).FirstOrDefaultAsync();

                    var tschLegBudgets = tsch_ReservationLeg.tsch_ReservationLegBudget.ToList();             
                    var newtschLegBudgets = legBudgets.Where(x=>x.DBState!=DBState.IsDeleted).Select(x => (tsch_ReservationLegBudget)x).ToList();
                    var budgetsToDelete = legBudgets.Where(x => x.DBState == DBState.IsDeleted).ToList();
                    foreach (var tschResBudget in newtschLegBudgets)
                    {
                        var oldBudget = tschLegBudgets.FirstOrDefault(x => x.IDX == tschResBudget.IDX);
                        if (oldBudget!=null)
                        {

                            oldBudget.IDXFrom = tschResBudget.IDXFrom;
                            oldBudget.IDXTo = tschResBudget.IDXTo;
                            oldBudget.IDXACtype = tschResBudget.IDXACtype;
                            oldBudget.Currency = tschResBudget.Currency.TrimEnd(' ');
                            oldBudget.RateType = tschResBudget.RateType;
                            oldBudget.IDXPricelist = tschResBudget.IDXPricelist;
                            oldBudget.FOC = tschResBudget.FOC;
                            oldBudget.Qty = tschResBudget.Qty;
                            oldBudget.Rate = tschResBudget.Rate;
                            oldBudget.Budget = tschResBudget.Budget;
                        }
                        else
                        {
                            tsch_ReservationLeg.tsch_ReservationLegBudget.Add(tschResBudget);
                            ctx.tsch_ReservationLegBudget.Add(tschResBudget);
                        }

                    }

                    foreach (var delBudget in budgetsToDelete)
                    {
                        var oldBudget = tschLegBudgets.FirstOrDefault(x => x.IDX == delBudget.IDX);
                        if (oldBudget!=null)
                            ctx.tsch_ReservationLegBudget.Remove(oldBudget);
                           
                    }

                    await ctx.SaveChangesAsync();
                }

                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                throw new Exception(LastError);
            }
        }


        private async Task<List<tsch_ReservationHeader>> GetReservationsPvt(List<int> resHdrIDs, String Server, string regionalDBName)
        {

              
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            ctx.Database.CommandTimeout = 180;
            using (ctx)
            {
                var tschResHeaders = await ctx.tsch_ReservationHeader.Include("tsch_WISHIntegrationHeader").
                                       Include("tset_Companies").
                                       Include("tset_Personnel1").
                                       Include("tset_Personnel").
                                       Include("tlst_ResType").
                                       Include("tlst_ResStatus").
                                       Include("tsch_Passengers").
                                       Include("tsch_ReservationLegs").
                                       //Include("tsch_ReservationLegs.tsch_ReservationLegBudget").
                                       //Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_GPExportInfo").
                                       //Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_Airports").
                                       //Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_Airports1").
                                       //Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_ACTypes").
                                       Include("tsch_ReservationLegs.tset_ACTypes").
                                       Include("tsch_ReservationLegs.tsch_LegsRes").
                                       Include("tsch_ReservationLegs.tset_Airports").
                                       Include("tsch_ReservationLegs.tset_Airports1").
                                       Include("tsch_SplitReservations").
                                       Include("tsch_SplitReservations1").
                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_Legs").
                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot").
                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot.tset_ACDetails").
                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot.tset_Personnel").
                                       Where(x => resHdrIDs.Contains(x.IDX)).
                                       ToListAsync();

                return tschResHeaders;

            }
        }

        public async Task<List<Reservation>> GetReservations(DateTime date, int resType, String Server, string regionalDBName)
        {
            try
            {
                DateTime nextDay = date.AddDays(1);
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                ctx.Database.CommandTimeout = 180;
                var reservation = new Reservation();

                using (ctx)
                {
                    var resHdrIDs = await ctx.tsch_ReservationLegs.Where(x => x.BookingDate >= date && x.BookingDate <= nextDay).Select(x => x.IDX_ResHeader).ToListAsync();
                    var tschResHeaders = await ctx.tsch_ReservationHeader.Include("tsch_WISHIntegrationHeader").
                                                                      Include("tset_Companies").
                                                                      Include("tset_Personnel1").
                                                                       Include("tset_Personnel").
                                                                       Include("tlst_ResType").
                                                                       Include("tlst_ResStatus").
                                                                       Include("tsch_Passengers").
                                                                       Include("tsch_ReservationLegs").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_GPExportInfo").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_Airports").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_Airports1").
                                                                       Include("tsch_ReservationLegs.tsch_ReservationLegBudget.tset_ACTypes").
                                                                       Include("tsch_ReservationLegs.tset_ACTypes").
                                                                       Include("tsch_ReservationLegs.tsch_LegsRes").
                                                                       Include("tsch_ReservationLegs.tset_Airports").
                                                                       Include("tsch_ReservationLegs.tset_Airports1").
                                                                       Include("tsch_SplitReservations").
                                                                       Include("tsch_SplitReservations1").
                                                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_Legs").
                                                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot").
                                                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot.tset_ACDetails").
                                                                       Include("tsch_ReservationLegs.tsch_LegsRes.tsch_AC_Pilot.tset_Personnel").
                                                                       Where(x=> resHdrIDs.Contains(x.IDX) &&  x.IDX_ResType == resType).
                                                                       ToListAsync();

                    //var tschResHeaders = tschResLegs.Select(x => x.tsch_ReservationHeader).ToList();
                    var reservations =tschResHeaders.Select(x => (Reservation)x).OrderBy(x=>x.Header.ReservationName).ToList();
                    //reservations = reservations.GroupBy(x => x.Header.ReservationName).Select(x => x.First()).ToList();
                    reservations.ForEach(x => x.IsNew = false);
                    return reservations;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return null;
            }
        }

        public async Task<List<Reservation>> GetReservations(DateTime dateStart, DateTime dateEnd, int resType, String Server, string regionalDBName)
        {
            try
            {
              
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var reservation = new Reservation();

                using (ctx)
                {
                    var tschResLegs = await ctx.tsch_ReservationLegs.Include("tsch_ReservationHeader").
                                                                      Include("tsch_ReservationHeader.tsch_WISHIntegrationHeader").
                                                                      Include("tsch_ReservationHeader.tset_Companies").
                                                                      Include("tsch_ReservationHeader.tset_Personnel1").
                                                                       Include("tsch_ReservationHeader.tset_Personnel").
                                                                       Include("tsch_ReservationHeader.tlst_ResType").
                                                                       Include("tsch_ReservationHeader.tlst_ResStatus").
                                                                       Include("tsch_ReservationHeader.tsch_Passengers").
                                                                       Include("tsch_ReservationLegBudget").
                                                                       Include("tsch_ReservationLegBudget.tset_GPExportInfo").
                                                                       Include("tsch_ReservationLegBudget.tset_Airports").
                                                                       Include("tsch_ReservationLegBudget.tset_Airports1").
                                                                       Include("tsch_ReservationLegBudget.tset_ACTypes").
                                                                       Include("tset_ACTypes").
                                                                       Include("tsch_LegsRes").
                                                                       Include("tset_Airports").
                                                                       Include("tset_Airports1").
                                                                       Include("tsch_ReservationHeader.tsch_SplitReservations").
                                                                       Include("tsch_ReservationHeader.tsch_SplitReservations1").
                                                                       Include("tsch_LegsRes.tsch_Legs").
                                                                       Include("tsch_LegsRes.tsch_AC_Pilot").
                                                                       Include("tsch_LegsRes.tsch_AC_Pilot.tset_ACDetails").
                                                                       Include("tsch_LegsRes.tsch_AC_Pilot.tset_Personnel").
                                                                       Where(x => x.BookingDate >= dateStart && x.BookingDate < dateEnd && x.tsch_ReservationHeader.IDX_ResType == resType).
                                                                       ToListAsync();

                    var tschResHeaders = tschResLegs.Select(x => x.tsch_ReservationHeader).ToList();
                    var reservations = tschResHeaders.Select(x => (Reservation)x).OrderBy(x => x.Header.ReservationName).ToList();
                    reservations = reservations.GroupBy(x => x.Header.ReservationName).Select(x => x.First()).ToList();
                    reservations.ForEach(x => x.IsNew = false);
                    return reservations;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return null;
            }
        }

        public  async Task<bool> DeleteLeg(int idx_Leg, String Server, string regionalDBName)
        {
            LastError = "";
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            try
            {
                using (ctx)
                {
                    ctx.tset_GPExportInfo.RemoveRange(ctx.tset_GPExportInfo.Where(x => x.IDX_ResLeg == idx_Leg));
                    ctx.tsch_ReservationLegBudget.RemoveRange(ctx.tsch_ReservationLegBudget.Where(x => x.IDXResLeg == idx_Leg));
                    ctx.tsch_ReservationLegs.Remove(ctx.tsch_ReservationLegs.FirstOrDefault(x => x.IDX == idx_Leg));

                    await ctx.SaveChangesAsync();
                    return true;
                }
            }

            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.InnerException != null)
                    LastError = ex.InnerException.InnerException.Message;
                else
                    LastError = ex.Message;
                return false;
            }

        }

        public  async Task<bool> CancelLeg(int idx_Leg, String Server, string regionalDBName)
        {
            LastError = "";
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            try
            {
                using (ctx)
                {

                    var legToCancel = await ctx.tsch_ReservationLegs.FirstOrDefaultAsync(x => x.IDX == idx_Leg);
                    if (legToCancel != null)
                        legToCancel.Cancelled = true;
                    await ctx.SaveChangesAsync();

                    return true;

                }
            }
            catch (Exception ex)
            {

                LastError = ex.Message;
                return false;
            }
        }
        public  async Task<bool> CancelBooking(int idxResHeader, String Server, String regionalDatabase)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
                var ctx = new SchedwinRegionalEntities(constring);
                var hdr = await ctx.tsch_ReservationHeader.FirstOrDefaultAsync(x => x.IDX == idxResHeader);
                var legs = await ctx.tsch_ReservationLegs.Where(x => x.IDX_ResHeader == idxResHeader).ToListAsync();
                hdr.IDX_ResStatus = 3;
                foreach (var leg in legs)
                    leg.Cancelled = true;

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
              
                return false;
            }
        }

        public async Task<bool> DeleteBooking(int idxResHeader, String Server, String regionalDatabase)
        {
            try
            {


                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var hdr=await ctx.tsch_WISHIntegrationHeader.Include("tsch_ReservationHeader")
                                                                .Include("tsch_ReservationHeader.tsch_Passengers")
                                                                .Include("tsch_WishIntegrationLeg")
                                                                .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs")
                                                                .Include("tsch_WishIntegrationLeg.tsch_ReservationLegs.tsch_ReservationLegBudget")
                                                                .FirstOrDefaultAsync(x => x.tsch_ReservationHeader.IDX == idxResHeader);

                    if (hdr.tsch_ReservationHeader.tsch_Passengers.Count>0)
                        ctx.tsch_Passengers.RemoveRange(hdr.tsch_ReservationHeader.tsch_Passengers);

                    foreach (var wiLeg in hdr.tsch_WishIntegrationLeg)
                    {
                        ctx.tsch_ReservationLegBudget.RemoveRange(wiLeg.tsch_ReservationLegs.tsch_ReservationLegBudget);
                        ctx.tsch_ReservationLegs.Remove(wiLeg.tsch_ReservationLegs);
                    }
                   

                    if (hdr.tsch_WishIntegrationLeg.Count>0)
                        ctx.tsch_WishIntegrationLeg.RemoveRange(hdr.tsch_WishIntegrationLeg);

                    ctx.tsch_ReservationHeader.Remove(hdr.tsch_ReservationHeader);

                    ctx.tsch_WISHIntegrationHeader.Remove(hdr);

                   await  ctx.SaveChangesAsync();
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                LastError = string.Join(Environment.NewLine, messages);
                return false;
            }
            return true;
        }



        private List<tsch_ReservationLegs> RefreshChildLegsFromMaster(tsch_ReservationHeader child, tsch_ReservationHeader master)
        {
            var newChildlegs = new List<tsch_ReservationLegs>();

            child.IDX_Operators = master.IDX_Operators;
            child.IDX_OperatorAgent = master.IDX_OperatorAgent;
            child.IDX_Personnel = master.IDX_Personnel;
            child.IDX_ResClass = master.IDX_ResClass;
            child.CoCode = master.CoCode;
            child.WISHID = master.WISHID;
            child.Voucher = master.Voucher;
            child.CURNCYID = master.CURNCYID;

            var childResLegs = child.tsch_ReservationLegs.ToList();
            var masterResLegs = master.tsch_ReservationLegs.ToList();
            foreach (var childLeg in childResLegs)
            {
                var masterLeg = masterResLegs.FirstOrDefault(x => x.IDX == childLeg.IDX_LegParent);
                if (master!=null)
                {

                    childLeg.UpdateFrom(masterLeg);
                    masterResLegs.Remove(masterLeg);
                }
                else
                {
                    childLeg.Cancelled = true;
                }

               
                foreach (var newLeg in masterResLegs)
                {
                    var newChildLeg = newLeg.ChildLegFromMaster();
                        newChildlegs.Add(newChildLeg);
                }
            }

            return newChildlegs;
        }

        public async Task<bool> RefreshFromMaster(int idxResHdr, String Server, string regionalDBName)
        {

            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);

                var masterRes = await ctx.tsch_ReservationHeader.Include("tsch_passengers").
                                                                    Include("tsch_ReservationLegs").
                                                                    FirstOrDefaultAsync(x => x.IDX == idxResHdr);

                var varSplitRes = await ctx.tsch_SplitReservations.Include("tsch_ReservationHeader").
                                                                Include("tsch_ReservationHeader.tsch_passengers").
                                                                Include("tsch_ReservationHeader.tsch_ReservationLegs").
                                                                 Where(x => x.fkParentReservation == idxResHdr).ToListAsync();
                var childRess = varSplitRes.Select(x => x.tsch_ReservationHeader).ToList();

                foreach (var childres in childRess)
                {
                   var newLegs= RefreshChildLegsFromMaster(childres, masterRes);
                    foreach (var newLeg in newLegs)
                    {
                        childres.tsch_ReservationLegs.Add(newLeg);
                        ctx.tsch_ReservationLegs.Add(newLeg);
                    }
                   
                }

                var MasterPax = masterRes.tsch_Passengers.ToList();
                var childPax = childRess.SelectMany(x => x.tsch_Passengers).ToList();

               foreach (var childPass in childPax)
                {
                    var masterPass = MasterPax.FirstOrDefault(x => x.WISHGuestID == childPass.WISHGuestID);
                    if (masterPass!=null)
                    {
                        masterRes.tsch_Passengers.Remove(masterPass);
                        ctx.tsch_Passengers.Remove(masterPass);
                    }

                }

                await ctx.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                LastError = message;
                return false;
            }
        }
    



        public static async Task<Reservation> SplitReservation(int idx_ReservationHDR, List<int> splitGuestList, string Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {

                    
                    var oldResHdr = await ctx.tsch_ReservationHeader.Include("tsch_passengers").
                                                            Include("tsch_ReservationLegs")
                                                            .FirstOrDefaultAsync(x => x.IDX == idx_ReservationHDR);
                    var oldResLegs = oldResHdr.tsch_ReservationLegs.ToList();
                    var newResHdr = new tsch_ReservationHeader();
                    newResHdr.Reservationname = oldResHdr.Reservationname+"(split 2/2)";
                    newResHdr.DateCaptured = DateTime.Today;
                    newResHdr.Numpax = splitGuestList.Count;
                    newResHdr.IDX_Operators = oldResHdr.IDX_Operators;
                    newResHdr.IDX_OperatorAgent = oldResHdr.IDX_OperatorAgent;
                    newResHdr.IDX_Personnel = oldResHdr.IDX_Personnel;
                    newResHdr.IDX_ResClass = oldResHdr.IDX_ResClass;
                    newResHdr.IDX_ResStatus = oldResHdr.IDX_ResStatus;
                    newResHdr.TicketPrinted = oldResHdr.TicketPrinted;
                    newResHdr.TicketRequired = oldResHdr.TicketRequired;
                    newResHdr.CoCode = oldResHdr.CoCode;
                    newResHdr.IDX_ResType = oldResHdr.IDX_ResType;
                    newResHdr.IDX_ResStatus = oldResHdr.IDX_ResStatus;
                    newResHdr.Notes = oldResHdr.Notes;
                    newResHdr.WISHID = oldResHdr.WISHID;
                    newResHdr.Voucher = oldResHdr.Voucher;
                    newResHdr.CURNCYID = oldResHdr.CURNCYID;
                    newResHdr.Active = true;

                    ctx.tsch_ReservationHeader.Add(newResHdr);
                    foreach (var oldResLeg in oldResLegs)
                    {
                        var newResLeg = oldResLeg.ChildLegFromMaster();
                        newResHdr.tsch_ReservationLegs.Add(newResLeg);
                        ctx.tsch_ReservationLegs.Add(newResLeg);
                    }

                    var lstPaxRemoves = new List<tsch_Passengers>();

                    foreach (var pax in oldResHdr.tsch_Passengers)
                    {

                        if (splitGuestList.Contains(pax.IDX))
                        {
                            lstPaxRemoves.Add(pax);
                            newResHdr.tsch_Passengers.Add(pax);
                        }
                    }



                    foreach (var pax in lstPaxRemoves)
                        oldResHdr.tsch_Passengers.Remove(pax);

                    oldResHdr.Numpax = oldResHdr.Numpax - splitGuestList.Count();
                    oldResHdr.Reservationname = oldResHdr.Reservationname + "(split 1/2)";
                    var tschSplitEntry = new tsch_SplitReservations();
                    tschSplitEntry.tsch_ReservationHeader1 = oldResHdr;
                    tschSplitEntry.tsch_ReservationHeader = newResHdr;
                    ctx.tsch_SplitReservations.Add(tschSplitEntry);

                    await ctx.SaveChangesAsync();

                    var newReservation = (Reservation)newResHdr;

                    return newReservation;
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var newMessage = string.Join(Environment.NewLine, messages);

                throw new Exception(newMessage);

            }

        }


        public static async Task<bool> SplitReservation(int idx_ReservationHDR, int noSplitGuest, string Server, string regionalDBName)
        {
           try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var oldResHdr = await ctx.tsch_ReservationHeader.Include("tsch_passengers").
                                                            Include("tsch_ReservationLegs")
                                                            .FirstOrDefaultAsync(x => x.IDX == idx_ReservationHDR);
                    var oldResLegs = oldResHdr.tsch_ReservationLegs.ToList();

                    oldResHdr.Numpax = oldResHdr.Numpax - noSplitGuest;
                    var newResHdr = new tsch_ReservationHeader();
                    newResHdr.Reservationname = oldResHdr.Reservationname;
                    newResHdr.DateCaptured = DateTime.Today;
                    newResHdr.Numpax = noSplitGuest;
                    newResHdr.IDX_Operators = oldResHdr.IDX_Operators;
                    newResHdr.IDX_OperatorAgent = oldResHdr.IDX_OperatorAgent;
                    newResHdr.IDX_Personnel = oldResHdr.IDX_Personnel;
                    newResHdr.IDX_ResClass = oldResHdr.IDX_ResClass;
                    newResHdr.IDX_ResStatus = oldResHdr.IDX_ResStatus;
                    newResHdr.TicketPrinted = oldResHdr.TicketPrinted;
                    newResHdr.TicketRequired = oldResHdr.TicketRequired;
                    newResHdr.CoCode = oldResHdr.CoCode;
                    newResHdr.IDX_ResType = oldResHdr.IDX_ResType;
                    newResHdr.IDX_ResStatus = oldResHdr.IDX_ResStatus;
                    newResHdr.Notes = oldResHdr.Notes;
                    newResHdr.WISHID = oldResHdr.WISHID;
                    newResHdr.Voucher = oldResHdr.Voucher;
                    newResHdr.CURNCYID = oldResHdr.CURNCYID;
                    newResHdr.Active = true;

                    ctx.tsch_ReservationHeader.Add(newResHdr);
                    foreach (var oldResLeg in oldResLegs)
                    {
                        var newResLeg = oldResLeg.ChildLegFromMaster();
                        newResHdr.tsch_ReservationLegs.Add(newResLeg);
                        ctx.tsch_ReservationLegs.Add(newResLeg);
                    }

                    for (int i = 0; i < noSplitGuest; i++)
                    {
                        var oldPass = oldResHdr.tsch_Passengers.First();
                        oldResHdr.tsch_Passengers.Remove(oldPass);
                        newResHdr.tsch_Passengers.Add(oldPass);
                    }

                    var tschSplitEntry = new tsch_SplitReservations();

                    tschSplitEntry.tsch_ReservationHeader1 = oldResHdr;
                    tschSplitEntry.tsch_ReservationHeader = newResHdr;
                    ctx.tsch_SplitReservations.Add(tschSplitEntry);

                    await ctx.SaveChangesAsync();

                    
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var newMessage=  string.Join(Environment.NewLine, messages);

                throw new Exception(newMessage);
            }

            return true;
        }

        private int GetCountryIDFromDB(String regionalDBName)
        {
            int countryID = 0;

            switch (regionalDBName)
            {
                case "Sefofane_Bots": countryID = 1; break;
                case "Sefofane_Nam": countryID = 7; break;
                case "Sefofane_Zim": countryID = 3; break;
                default: countryID = 3; break;

            }

            return countryID;
        }

        //public  async Task<bool> DeleteBooking(int idxResHeader, String Server, String regionalDatabase)
        //{
        //    try
        //    {
        //        List<int> idWishReslist = null;
        //        List<int> idResList = null;
        //        List<int> idBudgeList = null;
        //        List<int> idResHDrList = null;

        //        var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDatabase);
        //        var ctx = new SchedwinRegionalEntities(constring);
        //        using (ctx)
        //        {
        //            var legs = await ctx.tsch_ReservationLegs.Include("tsch_WISHReservationLegs").
        //                                                        Include("tsch_ReservationLegBudget").Where(x => x.IDX_ResHeader == idxResHeader).ToListAsync();

        //            idWishReslist = legs.SelectMany(x => x.tsch_WISHReservationLegs).Select(x => x.IDX).ToList();
        //            idResList = legs.Select(x => x.IDX).ToList();
        //            idBudgeList = legs.SelectMany(x => x.tsch_ReservationLegBudget).Select(x => x.IDX).ToList();

        //        }
        //        ctx = new SchedwinRegionalEntities(constring);
        //        using (ctx)
        //        {
        //            var hdr = await ctx.tsch_ReservationHeader.Include("tsch_WishReservationHeader").FirstOrDefaultAsync(x => x.IDX == idxResHeader);
        //            idResHDrList = hdr.tsch_WISHReservationHeader.Select(x => x.IDX).ToList();
        //            var rmList1 = ctx.tsch_WISHReservationLegs.Where(x => idWishReslist.Contains(x.IDX)).ToList();
        //            ctx.tsch_WISHReservationLegs.RemoveRange(rmList1);

        //            await ctx.SaveChangesAsync();
        //            var rmlist3 = ctx.tsch_ReservationLegBudget.Where(x => idBudgeList.Contains(x.IDX)).ToList();
        //            ctx.tsch_ReservationLegBudget.RemoveRange(rmlist3);

        //            await ctx.SaveChangesAsync();
        //            var rmList2 = ctx.tsch_ReservationLegs.Where(x => idResList.Contains(x.IDX)).ToList();
        //            ctx.tsch_ReservationLegs.RemoveRange(rmList2);


        //            ctx.tsch_WISHReservationHeader.RemoveRange(ctx.tsch_WISHReservationHeader.Where(x => idResHDrList.Contains(x.IDX)));
        //            ctx.tsch_ReservationHeader.Remove(hdr);

        //            await ctx.SaveChangesAsync();
        //            //legs.ForEach(x => ctx.tsch_ReservationLegs.Remove(x));

        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
        //        LastError = string.Join(Environment.NewLine, messages);
        //        return false;
        //    }
        //    return true;
        //}
    }
}
