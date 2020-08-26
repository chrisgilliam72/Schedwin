using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using Schedwin.Common;
using Schedwin.Data;

namespace SchedwinAPI
{
    public class SchedwinAPI
    {



        public static SchedwinReservation GetReservation(int referenceID)
        {
            SchedwinReservation reservation = null;
            try
            {
                var regonalInfo = GetRegionalDBInfo("ZZTest_Botswana");
                var constring = RegionalConnectionGenerator.GetConnectionString(regonalInfo.Server, regonalInfo.Database);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var resHdr =  ctx.tsch_ReservationHeader
                                        .Include("tsch_WISHIntegrationHeader")
                                        .Include("tsch_ReservationLegs")
                                         .Include("tsch_ReservationLegs.tset_Airports")
                                            .Include("tsch_ReservationLegs.tset_Airports1")
                                        .Include("tsch_Passengers")
                                        .Include("tlst_ResStatus").FirstOrDefault(x => x.IDX == referenceID);
                    if (resHdr!=null)
                         reservation = (SchedwinReservation)resHdr;
                  
                    else
                    {
                        reservation = new SchedwinReservation();
                        reservation.ReferenceID = -1;
                    }

                }
  
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                reservation = new SchedwinReservation();
                reservation.ReferenceID = -1000;
                reservation.Name = exMessage;
            }

            return reservation;
        }

        public static SchedwinReservationStatus CreateReservation(SchedwinReservation reservation)
        {
            var status = new SchedwinReservationStatus();
            try
            {
                var regonalInfo = GetRegionalDBInfo("ZZTest_Botswana");
                var constring = RegionalConnectionGenerator.GetConnectionString(regonalInfo.Server, regonalInfo.Database);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {

                    int noACIDX = ctx.tset_ACTypes.FirstOrDefault(x => x.ACType == "None").IDX;
                    var apLst = reservation.Legs.Select(x => x.To).ToList();
                    apLst.AddRange(reservation.Legs.Select(x => x.From).ToList());
                    apLst = apLst.Distinct().ToList();
                    var airports = ctx.tset_Airports.Where(x => apLst.Contains(x.Airport)).ToList();
                    var standardWeights = ctx.tset_StandardPassengerWeights.Where(x => x.IDX_Countries == regonalInfo.DefaultCountryID && x.MaxRange > 13).ToList();

                    DateTime deptDate = DateTime.Today;

                    var tschHeader = (tsch_ReservationHeader)reservation;

                    tschHeader.IDX_Operators = regonalInfo.DefaultOperatorID.Value;
                    tschHeader.IDX_Personnel = regonalInfo.APIServiceID.Value;

                    foreach (var leg in reservation.Legs)
                    {
                        int ToAP = airports.FirstOrDefault(x => x.Airport == leg.To).IDX;
                        int FromAP = airports.FirstOrDefault(x => x.Airport == leg.From).IDX;

                        var tschResLeg = (tsch_ReservationLegs)leg;
                        var wishIntegrationLeg = tschResLeg.tsch_WishIntegrationLeg.FirstOrDefault();
                        wishIntegrationLeg.tsch_WISHIntegrationHeader = tschHeader.tsch_WISHIntegrationHeader.FirstOrDefault();

                        tschResLeg.IDX_SpecificACType = noACIDX;
     
                        tschResLeg.IDX_Personnel = regonalInfo.DefaultOperatorID.Value;
                        tschResLeg.IDX_Company = regonalInfo.APIServiceID.Value;
                        tschResLeg.ToAp = ToAP;
                        tschResLeg.FromAp = FromAP;
                        tschHeader.tsch_ReservationLegs.Add(tschResLeg);

                        ctx.tsch_ReservationLegs.Add(tschResLeg);
                    }


                    ctx.tsch_ReservationHeader.Add(tschHeader);

                    foreach (var schedwinPax in reservation.Passengers)
                    {
                        var tschPax = (tsch_Passengers)schedwinPax;

                        if (tschPax.Sex == "F")
                            tschPax.Weight = standardWeights.FirstOrDefault().Female;
                        else
                            tschPax.Weight = standardWeights.FirstOrDefault().Male;
                        tschPax.Luggageweight = standardWeights.FirstOrDefault().Luggage;

                        tschHeader.tsch_Passengers.Add(tschPax);
                        ctx.tsch_Passengers.Add(tschPax);
                    }
                    ctx.SaveChanges();

                    status.ReservationName = tschHeader.Reservationname;
                    status.ReservationReferenceID = tschHeader.IDX;
                }


                status.Status = "OK";
            }
            catch (DbEntityValidationException entityValEx)
            {
                status.ReservationReferenceID = -200;
                status.Status = "Validation error: " ;
                foreach (var entityValidationErrors in entityValEx.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        status.Status+= validationError.ErrorMessage+" ";
                    }
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                status.ReservationReferenceID = -100;
                status.Status = "Error :" + exMessage;
            }

            return status;
        }


        public static SchedwinReservationStatus UpdateReservation(SchedwinReservation reservation)
        {
            var status = new SchedwinReservationStatus();
            try
            {
                var regonalInfo = GetRegionalDBInfo("ZZTest_Botswana");
                var constring = RegionalConnectionGenerator.GetConnectionString(regonalInfo.Server, regonalInfo.Database);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var resHdr = ctx.tsch_ReservationHeader.
                                    Include("tsch_WishIntegrationHeader")
                                    .Include("tsch_ReservationLegs")
                                    .Include("tsch_ReservationLegs.tset_Airports")
                                    .Include("tsch_ReservationLegs.tset_Airports1")
                                    .Include("tsch_Passengers")
                                    .Include("tlst_ResStatus").FirstOrDefault(x => x.IDX == reservation.ReferenceID);

                    int noACIDX = ctx.tset_ACTypes.FirstOrDefault(x => x.ACType == "None").IDX;
                    var standardWeights = ctx.tset_StandardPassengerWeights.Where(x => x.IDX_Countries == regonalInfo.DefaultCountryID && x.MaxRange > 13).ToList();
                    var apLst = reservation.Legs.Select(x => x.To).ToList();
                    apLst.AddRange(reservation.Legs.Select(x => x.From).ToList());
                    apLst = apLst.Distinct().ToList();
                    var airports = ctx.tset_Airports.Where(x => apLst.Contains(x.Airport)).ToList();

                    resHdr.Numpax = reservation.Passengers.Count;
                    resHdr.Notes = reservation.Notes;
                    resHdr.Reservationname = reservation.ExternalBookingName + "[" + reservation.ExternalGroupName + "]";

                    foreach (var leg in reservation.Legs)
                    {
                        var dbLeg = resHdr.tsch_ReservationLegs.FirstOrDefault(x => x.WISHIDLegs == leg.ExternalReferenceID && x.Cancelled == false);
                        if (leg.Cancelled && dbLeg != null)
                            dbLeg.Cancelled = true;
                        else if (dbLeg == null)
                        {
                            int ToAP = airports.FirstOrDefault(x => x.Airport == leg.To).IDX;
                            int FromAP = airports.FirstOrDefault(x => x.Airport == leg.From).IDX;

                            dbLeg = (tsch_ReservationLegs)leg;
                            var wishIntegrationLeg = dbLeg.tsch_WishIntegrationLeg.FirstOrDefault();
                            wishIntegrationLeg.tsch_WISHIntegrationHeader = resHdr.tsch_WISHIntegrationHeader.FirstOrDefault();
                            dbLeg.IDX_SpecificACType = noACIDX;
                            dbLeg.IDX_Personnel = regonalInfo.DefaultOperatorID.Value;
                            dbLeg.IDX_Company = regonalInfo.APIServiceID.Value;
                            dbLeg.ToAp = ToAP;
                            dbLeg.FromAp = FromAP;
                            resHdr.tsch_ReservationLegs.Add(dbLeg);
                            ctx.tsch_ReservationLegs.Add(dbLeg);
                        }
                        else
                        {
                            dbLeg.Notes = leg.Notes;
                            dbLeg.SoleUse = leg.SoleUse;
                            dbLeg.ExField = leg.Ex;
                            dbLeg.ForField = leg.For;
                        }

                    }


                    ctx.tsch_Passengers.RemoveRange(resHdr.tsch_Passengers);
                    resHdr.tsch_Passengers.Clear();

                    foreach (var passenger in reservation.Passengers)
                    {
                        var tschPax = (tsch_Passengers)passenger;
                        if (tschPax.Sex == "F")
                            tschPax.Weight = standardWeights.FirstOrDefault().Female;
                        else
                            tschPax.Weight = standardWeights.FirstOrDefault().Male;
                        tschPax.Luggageweight = standardWeights.FirstOrDefault().Luggage;

                        resHdr.tsch_Passengers.Add(tschPax);
                        ctx.tsch_Passengers.Add(tschPax);
                    }

                    ctx.SaveChanges();

                    status.ReservationName = resHdr.Reservationname;
                    status.ReservationReferenceID = reservation.ReferenceID;
                    status.Status = "OK";
                }

            }
            catch (DbEntityValidationException entityValEx)
            {
                status.ReservationReferenceID = reservation.ReferenceID;
                status.Status = "Validation error: ";
                foreach (var entityValidationErrors in entityValEx.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        status.Status += validationError.ErrorMessage + " ";
                    }
                }
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                status.Status = "Error :" + exMessage;
            }
            return status;
        }

        private static tbDBRegionInfo GetRegionalDBInfo(String region)
        {
            var ctx = new SchedwinGlobalEntities();
            return ctx.tbDBRegionInfoes.FirstOrDefault(x => x.Region == region);
        }
    }
}
