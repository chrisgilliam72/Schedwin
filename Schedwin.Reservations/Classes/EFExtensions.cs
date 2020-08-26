using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
   static class EFExtensions
    {
        public static tsch_ReservationLegs ChildLegFromMaster(this tsch_ReservationLegs masterLeg)
        {
            var newResLeg = new tsch_ReservationLegs();
            newResLeg.BookingDate = masterLeg.BookingDate;
            newResLeg.FromAp = masterLeg.FromAp;
            newResLeg.ExField = masterLeg.ExField;
            newResLeg.ToAp = masterLeg.ToAp;
            newResLeg.ForField = masterLeg.ForField;
            newResLeg.EarliestEx = masterLeg.EarliestEx;
            newResLeg.LatestEx = masterLeg.LatestEx;
            newResLeg.EarliestFor = masterLeg.EarliestFor;
            newResLeg.LatestFor = masterLeg.LatestFor;
            newResLeg.Budget = masterLeg.Budget;
            newResLeg.DirectDistance = masterLeg.DirectDistance;
            newResLeg.GameflightTime = masterLeg.GameflightTime;
            newResLeg.Notes = masterLeg.Notes;
            newResLeg.IDX_Personnel = masterLeg.IDX_Personnel;
            newResLeg.IDX_Company = masterLeg.IDX_Company;
            newResLeg.TicketPrinted = masterLeg.TicketPrinted;
            newResLeg.IDX_ScheduledFlight = masterLeg.IDX_ScheduledFlight;
            newResLeg.IDX_SpecificACType = masterLeg.IDX_SpecificACType;
            newResLeg.SoleUse = masterLeg.SoleUse;
            newResLeg.WISHIDLegs = masterLeg.WISHIDLegs;
            newResLeg.IDX_AreaPriceList = masterLeg.IDX_AreaPriceList;
            newResLeg.Cancelled = masterLeg.Cancelled;
            newResLeg.Voucher = masterLeg.Voucher;
            newResLeg.INV_Number = masterLeg.INV_Number;
            newResLeg.RateType = masterLeg.RateType;
            newResLeg.FOC = masterLeg.FOC;
            newResLeg.Currency = masterLeg.Currency;
            newResLeg.IDX_LegParent = masterLeg.IDX;
            return newResLeg;
        }

        public static void UpdateFrom(this tsch_ReservationLegs originalleg, tsch_ReservationLegs newleg)
        {
            originalleg.BookingDate = newleg.BookingDate;
            originalleg.FromAp = newleg.FromAp;
            originalleg.ExField = newleg.ExField;
            originalleg.ToAp = newleg.ToAp;
            originalleg.ForField = newleg.ForField;
            originalleg.EarliestEx = newleg.EarliestEx;
            originalleg.LatestEx = newleg.LatestEx;
            originalleg.EarliestFor = newleg.EarliestFor;
            originalleg.LatestFor = newleg.LatestFor;
            originalleg.Budget = newleg.Budget;
            originalleg.DirectDistance = newleg.DirectDistance;
            originalleg.GameflightTime = newleg.GameflightTime;
            originalleg.Notes = newleg.Notes;
            originalleg.IDX_Personnel = newleg.IDX_Personnel;
            originalleg.IDX_Company = newleg.IDX_Company;
            originalleg.TicketPrinted = newleg.TicketPrinted;
            originalleg.IDX_ScheduledFlight = newleg.IDX_ScheduledFlight;
            originalleg.IDX_SpecificACType = newleg.IDX_SpecificACType;
            originalleg.SoleUse = newleg.SoleUse;
            originalleg.WISHIDLegs = newleg.WISHIDLegs;
            originalleg.IDX_AreaPriceList = newleg.IDX_AreaPriceList;
            originalleg.Cancelled = newleg.Cancelled;
            originalleg.Voucher = newleg.Voucher;
            originalleg.INV_Number = newleg.INV_Number;
            originalleg.RateType = newleg.RateType;
            originalleg.FOC = newleg.FOC;
            originalleg.Currency = newleg.Currency;
        }
    }
}
