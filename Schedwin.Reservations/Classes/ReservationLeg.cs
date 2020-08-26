using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
    public class ReservationLeg
    {
        public DBState DBState { get; set; }

        public int IDX { get; set; }

        public Guid TmpID { get; set; }

        public int WishSectorID { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ETA { get; set; }
     
        public DateTime BookingDate { get; set; }

        public int IDX_FromAP { get; set; }

        public String FromAP { get; set; }

        public int IDX_ToAP { get; set; }

        public String ToAP { get; set; }

        public String ExField { get; set; }

        public String ForField { get; set; }

        public String ExCode { get; set; }

        public String WishEx { get; set; }

        public String WishFor { get; set; }

        public string ForCode { get; set; }

        public DateTime EarliestEx { get; set; }

        public DateTime LatestEx { get; set; }

        public DateTime EarliestFor { get; set; }

        public DateTime LatestFor { get; set; }

        public int IDX_AC_Type { get; set; }
        public String ACType { get; set; }

        public int Distance { get; set; }

        public bool Deleted { get; set; }
        public bool Canceled { get; set; }

        public double GameFlightTime { get; set; }

        public bool TicketPrinted { get; set; }

        public bool IsScheduled { get; set; }

        public bool SoleUse { get; set; }

        public String VoucherCurrency { get; set; }

        public Decimal VoucherAmount { get; set; }

        public String Voucher { get; set; }

        public String InvNumber { get; set; }

        public String Notes { get; set; }

        public bool FOC { get; set; }

        public int SEQNo { get; set; }

        public String CharterType { get; set; }
        public List<ReservationLegBudget> Budgets { get; set; }

        public List<ReservationLegSchedule> Schedules { get; set; }

        public bool IsNew()
        {
            return DBState == DBState.IsNew ? true : false;
        }

        public ReservationLeg()
        {
            EarliestEx = DateTime.Today;
            LatestEx = DateTime.Today;
            EarliestFor = DateTime.Today;
            LatestFor = DateTime.Today;

            DBState = DBState.IsNew;
            Budgets = new List<ReservationLegBudget>();
            Schedules = new List<ReservationLegSchedule>();
            Notes = "";
        }

        public ReservationLeg(ReservationLeg Leg)
        {
            Budgets = new List<ReservationLegBudget>();
            Schedules = new List<ReservationLegSchedule>();
            DBState = DBState;
            IDX = Leg.IDX;
            WishSectorID = Leg.WishSectorID;
            ETD = Leg.ETD;
            ETA = Leg.ETA;
            BookingDate = Leg.BookingDate;
            IDX_FromAP = Leg.IDX_FromAP;
            FromAP = Leg.FromAP;
            IDX_ToAP = Leg.IDX_ToAP;
            ToAP = Leg.ToAP;
            ExField = Leg.ExField;
            ForField = Leg.ForField;
            WishEx = Leg.WishEx;
            WishFor = Leg.WishFor;
            EarliestEx = Leg.EarliestEx;
            LatestEx = Leg.LatestEx;
            EarliestFor = Leg.EarliestFor;
            LatestFor = Leg.LatestFor;
            IDX_AC_Type = Leg.IDX_AC_Type;
            ACType = Leg.ACType;
            Distance = Leg.Distance;
            Canceled = Leg.Canceled;
            GameFlightTime = Leg.GameFlightTime;
            TicketPrinted = Leg.TicketPrinted;
            IsScheduled = Leg.IsScheduled;
            SoleUse = Leg.SoleUse;
            Voucher = Leg.Voucher;
            InvNumber = Leg.InvNumber;
            Notes = Leg.Notes;
            FOC = Leg.FOC;
            CharterType = Leg.CharterType;
            SEQNo = Leg.SEQNo;

        }
        public static implicit operator tbReservationLeg(ReservationLeg resLeg)
        {
            var tbResLeg = new tbReservationLeg();


            tbResLeg.pkReservationLegID = resLeg.IDX;
            tbResLeg.SEQNo = resLeg.SEQNo;
            tbResLeg.BookingDate = resLeg.BookingDate;
            tbResLeg.fkFromAirstripID = resLeg.IDX_FromAP;
            tbResLeg.fkToAirstripID = resLeg.IDX_ToAP;
            if (tbResLeg.fkACTypeID > 0)
                tbResLeg.fkACTypeID = resLeg.IDX_AC_Type;
            else
                tbResLeg.fkACTypeID = null;

            tbResLeg.ExField = resLeg.ExField;
            tbResLeg.ForField = resLeg.ForField;
            tbResLeg.EarliestEx = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.EarliestEx.Hour, resLeg.EarliestEx.Minute, 00);
            tbResLeg.LatestEx = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.LatestEx.Hour, resLeg.LatestEx.Minute, 00);
            tbResLeg.EarliestFor = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.EarliestFor.Hour, resLeg.EarliestFor.Minute, 00);
            tbResLeg.LatestFor = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.LatestFor.Hour, resLeg.LatestFor.Minute, 00); ;

            tbResLeg.DirectDistance = Convert.ToInt16(resLeg.Distance);
            tbResLeg.GameflightTime = resLeg.GameFlightTime;
            tbResLeg.TicketPrinted = resLeg.TicketPrinted;
            tbResLeg.SoleUse = resLeg.SoleUse;
            tbResLeg.Cancelled = resLeg.Canceled;
            tbResLeg.Voucher = resLeg.Voucher;
            tbResLeg.Budget = Convert.ToDouble(resLeg.VoucherAmount);
            tbResLeg.INV_Number = resLeg.InvNumber;

            if (resLeg.Notes != null)
                tbResLeg.Notes = resLeg.Notes;
            else
                tbResLeg.Notes = "";
            if (!String.IsNullOrEmpty(resLeg.CharterType))
                tbResLeg.RateType = resLeg.CharterType;
            else
                tbResLeg.RateType = "";
            return tbResLeg;

        }

        public static implicit operator tsch_ReservationLegs(ReservationLeg resLeg)
        {
            var tschResLeg = new tsch_ReservationLegs();


            tschResLeg.IDX = resLeg.IDX;
            tschResLeg.SEQNo = resLeg.SEQNo;
            tschResLeg.BookingDate = resLeg.BookingDate;
            tschResLeg.FromAp = resLeg.IDX_FromAP;
            tschResLeg.ToAp = resLeg.IDX_ToAP;
            if (resLeg.IDX_AC_Type >0)
                tschResLeg.IDX_SpecificACType = resLeg.IDX_AC_Type;
            else
                tschResLeg.IDX_SpecificACType = null;

            tschResLeg.ExField =  resLeg.ExField;
            tschResLeg.ForField = resLeg.ForField;
            tschResLeg.EarliestEx = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day,resLeg.EarliestEx.Hour,resLeg.EarliestEx.Minute,00);
            tschResLeg.LatestEx = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.LatestEx.Hour, resLeg.LatestEx.Minute, 00);
            tschResLeg.EarliestFor = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.EarliestFor.Hour, resLeg.EarliestFor.Minute, 00);  
            tschResLeg.LatestFor = new DateTime(resLeg.BookingDate.Year, resLeg.BookingDate.Month, resLeg.BookingDate.Day, resLeg.LatestFor.Hour, resLeg.LatestFor.Minute, 00);;

            tschResLeg.DirectDistance = Convert.ToInt16(resLeg.Distance);
            tschResLeg.GameflightTime = resLeg.GameFlightTime;
            tschResLeg.TicketPrinted = resLeg.TicketPrinted;
            tschResLeg.SoleUse = resLeg.SoleUse;
            tschResLeg.Cancelled = resLeg.Canceled;
            tschResLeg.Voucher = resLeg.Voucher;
            tschResLeg.Budget = Convert.ToDouble(resLeg.VoucherAmount);
            tschResLeg.INV_Number = resLeg.InvNumber;

            if (resLeg.Notes != null)
                tschResLeg.Notes = resLeg.Notes;
            else
                tschResLeg.Notes = "";
            if (!String.IsNullOrEmpty(resLeg.CharterType))
                tschResLeg.RateType = resLeg.CharterType;
            else
                tschResLeg.RateType = "";
            return tschResLeg;

        }


        public static explicit operator ReservationLeg(tbReservationLeg dbLeg)
        {
            var resLeg = new ReservationLeg();

            resLeg.DBState = DBState.IsLoaded;
            resLeg.IDX = dbLeg.pkReservationLegID;
            resLeg.BookingDate = dbLeg.BookingDate;

            resLeg.IDX_FromAP = dbLeg.fkFromAirstripID;
            resLeg.IDX_ToAP = dbLeg.fkToAirstripID;

            if (dbLeg.tbAirstrip != null)
                resLeg.FromAP = dbLeg.tbAirstrip1.IATA;

            if (dbLeg.tbAirstrip1 != null)
                resLeg.ToAP = dbLeg.tbAirstrip.IATA;

            //resLeg.IsScheduled = true ? dbLeg.tsch_LegsRes != null : false;
            //resLeg.IDX_AC_Type = dbLeg.IDX_SpecificACType ?? -1;
            //if (dbLeg.tset_ACTypes != null)
            //    resLeg.ACType = dbLeg.tset_ACTypes.ACType;

            resLeg.ExField = dbLeg.ExField;
            resLeg.ForField = dbLeg.ForField;

            resLeg.EarliestEx = dbLeg.EarliestEx.Value;
            resLeg.LatestEx = dbLeg.LatestEx.Value;
            resLeg.EarliestFor = dbLeg.EarliestFor.Value;
            resLeg.LatestFor = dbLeg.LatestFor.Value;
            resLeg.Distance = dbLeg.DirectDistance;
            resLeg.GameFlightTime = dbLeg.GameflightTime;
            resLeg.TicketPrinted = dbLeg.TicketPrinted ?? false;
            resLeg.SoleUse = dbLeg.SoleUse ?? false;
            resLeg.Canceled = dbLeg.Cancelled ?? false;
            resLeg.Voucher = dbLeg.Voucher ?? "";
            resLeg.VoucherAmount = Convert.ToDecimal(dbLeg.Budget);
            resLeg.InvNumber = dbLeg.INV_Number ?? "";
            resLeg.Notes = dbLeg.Notes;
            resLeg.CharterType = dbLeg.RateType;

            resLeg.ETA = resLeg.BookingDate.AddHours(14);
            resLeg.ETD = resLeg.BookingDate.AddHours(12);
            if (dbLeg.SEQNo.HasValue)
                resLeg.SEQNo = dbLeg.SEQNo.Value;
            else
                resLeg.SEQNo = 1;

            if (dbLeg.tbReservationLegBudgets != null && dbLeg.tbReservationLegBudgets.Count > 0)
            {
                var dbBudgets = dbLeg.tbReservationLegBudgets.ToList();
                var legResBudgets = dbBudgets.Select(x => (ReservationLegBudget)x).ToList();
                resLeg.Budgets.AddRange(legResBudgets);
            }


            var tbWishIntegrationLeg = dbLeg.tbWishIntegrationLegs.FirstOrDefault();
            if (tbWishIntegrationLeg != null)
            {
                if (tbWishIntegrationLeg.WishFor != "n/a")
                    resLeg.WishFor = tbWishIntegrationLeg.WishFor;
                if (tbWishIntegrationLeg.WishEx != "n/a")
                    resLeg.WishEx = tbWishIntegrationLeg.WishEx;

                if (tbWishIntegrationLeg.ETA.HasValue)
                    resLeg.ETA = tbWishIntegrationLeg.ETA.Value;
                if (tbWishIntegrationLeg.ETD.HasValue)
                    resLeg.ETD = tbWishIntegrationLeg.ETD.Value;

                resLeg.WishSectorID = tbWishIntegrationLeg.WishSectorID;
            }

            //resLeg.IsScheduled = (dbLeg.tsch_LegsRes != null && dbLeg.tsch_LegsRes.Count > 0);
            //if (resLeg.IsScheduled)
            //{
            //    foreach (var legRes in dbLeg.tsch_LegsRes)
            //    {
            //        var schedule = new ReservationLegSchedule();
            //        schedule.BookingDate = resLeg.BookingDate;
            //        if (legRes.tsch_Legs != null && legRes.tsch_Legs.tsch_AC_Pilot != null && legRes.tsch_Legs.tsch_AC_Pilot.tset_ACDetails != null &&
            //            legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel != null)
            //        {
            //            schedule.ACRegistration = legRes.tsch_Legs.tsch_AC_Pilot.tset_ACDetails.Registration;
            //            schedule.IDX_AircraftType = legRes.tsch_Legs.tsch_AC_Pilot.IDX_ACTypes.Value;

            //            schedule.PilotName = legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel.Firstname + " " + legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel.Surname;
            //            schedule.IDX_Pilot = legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel.IDX;
            //            schedule.ETA = legRes.tsch_Legs.ETA;
            //            schedule.ETD = legRes.tsch_Legs.ETD;
            //            if (legRes.tsch_Legs.tset_Airports != null)
            //                schedule.FromAP = legRes.tsch_Legs.tset_Airports.IATA;
            //            schedule.IDX_FromAP = legRes.tsch_Legs.FromAP;
            //            if (legRes.tsch_Legs.tset_Airports1 != null)
            //                schedule.ToAP = legRes.tsch_Legs.tset_Airports1.IATA;
            //            schedule.IDX_ToAP = legRes.tsch_Legs.ToAP;

            //        }

            //        resLeg.Schedules.Add(schedule);
            //    }


            //}



            return resLeg;

        }



        public static explicit operator ReservationLeg(tsch_ReservationLegs dbLeg)
        {
            var resLeg = new ReservationLeg();

            resLeg.DBState = DBState.IsLoaded;
            resLeg.IDX = dbLeg.IDX;
            resLeg.BookingDate = dbLeg.BookingDate;

            resLeg.IDX_FromAP = dbLeg.FromAp;
            if (dbLeg.tset_Airports != null)
                resLeg.FromAP = dbLeg.tset_Airports.IATA;
            resLeg.IDX_ToAP = dbLeg.ToAp;
            if (dbLeg.tset_Airports1 != null)
                resLeg.ToAP = dbLeg.tset_Airports1.IATA;

            resLeg.IsScheduled = true ? dbLeg.tsch_LegsRes != null : false;
            resLeg.IDX_AC_Type = dbLeg.IDX_SpecificACType ?? -1;
            if (dbLeg.tset_ACTypes != null)
                resLeg.ACType = dbLeg.tset_ACTypes.ACType;

            resLeg.ExField = dbLeg.ExField;
            resLeg.ForField = dbLeg.ForField;

            resLeg.EarliestEx = dbLeg.EarliestEx.Value;
            resLeg.LatestEx = dbLeg.LatestEx.Value;
            resLeg.EarliestFor = dbLeg.EarliestFor.Value;
            resLeg.LatestFor = dbLeg.LatestFor.Value;
            resLeg.Distance = dbLeg.DirectDistance;
            resLeg.GameFlightTime = dbLeg.GameflightTime;
            resLeg.TicketPrinted = dbLeg.TicketPrinted ?? false;
            resLeg.SoleUse = dbLeg.SoleUse ?? false;
            resLeg.Canceled = dbLeg.Cancelled ?? false;
            resLeg.Voucher = dbLeg.Voucher ?? "";
            resLeg.VoucherAmount =Convert.ToDecimal( dbLeg.Budget);
            resLeg.InvNumber = dbLeg.INV_Number ?? "";
            resLeg.Notes = dbLeg.Notes;
            resLeg.CharterType = dbLeg.RateType;

            resLeg.ETA = resLeg.BookingDate.AddHours(14);
            resLeg.ETD = resLeg.BookingDate.AddHours(12);
            if (dbLeg.SEQNo.HasValue)
                resLeg.SEQNo = dbLeg.SEQNo.Value;
            else
                resLeg.SEQNo = 1;

            if (dbLeg.tsch_ReservationLegBudget!=null && dbLeg.tsch_ReservationLegBudget.Count>0)
            {
                var dbBudgets = dbLeg.tsch_ReservationLegBudget.ToList();
                var legResBudgets = dbBudgets.Select(x => (ReservationLegBudget)x).ToList();
                resLeg.Budgets.AddRange(legResBudgets);
            }


            var tschWishIntLeg = dbLeg.tsch_WishIntegrationLeg.FirstOrDefault();
            if (tschWishIntLeg != null)
            {
                if (tschWishIntLeg.WishFor != "n/a")
                    resLeg.WishFor = tschWishIntLeg.WishFor;
                if (tschWishIntLeg.WishEx != "n/a")
                    resLeg.WishEx = tschWishIntLeg.WishEx;

                if (tschWishIntLeg.ETA.HasValue)
                    resLeg.ETA = tschWishIntLeg.ETA.Value;
                if (tschWishIntLeg.ETD.HasValue)
                    resLeg.ETD = tschWishIntLeg.ETD.Value;

                resLeg.WishSectorID = tschWishIntLeg.WishSectorID;
            }

            resLeg.IsScheduled = (dbLeg.tsch_LegsRes != null && dbLeg.tsch_LegsRes.Count > 0);
            if (resLeg.IsScheduled)
            {
                foreach (var legRes in dbLeg.tsch_LegsRes)
                {
                    var schedule = new ReservationLegSchedule();
                    schedule.BookingDate = resLeg.BookingDate;
                    if (legRes.tsch_Legs!=null && legRes.tsch_Legs.tsch_AC_Pilot!=null && legRes.tsch_Legs.tsch_AC_Pilot.tset_ACDetails!=null &&
                        legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel!=null)
                    {
                        schedule.ACRegistration = legRes.tsch_Legs.tsch_AC_Pilot.tset_ACDetails.Registration;
                        schedule.IDX_AircraftType = legRes.tsch_Legs.tsch_AC_Pilot.IDX_ACTypes.Value;
                     
                        schedule.PilotName = legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel.Firstname + " " + legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel.Surname;
                        schedule.IDX_Pilot = legRes.tsch_Legs.tsch_AC_Pilot.tset_Personnel.IDX;
                        schedule.ETA = legRes.tsch_Legs.ETA;
                        schedule.ETD = legRes.tsch_Legs.ETD;
                        if (legRes.tsch_Legs.tset_Airports!=null)
                            schedule.FromAP = legRes.tsch_Legs.tset_Airports.IATA;
                        schedule.IDX_FromAP = legRes.tsch_Legs.FromAP;
                        if (legRes.tsch_Legs.tset_Airports1!=null)
                            schedule.ToAP = legRes.tsch_Legs.tset_Airports1.IATA;
                        schedule.IDX_ToAP = legRes.tsch_Legs.ToAP;
       
                    }

                    resLeg.Schedules.Add(schedule);
                }
               

            }



            return resLeg;

        }
    }
}
