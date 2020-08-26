using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Reservations.Classes
{
   public class ReservationLegBudget
    {
       
        public DBState DBState { get; set; }
        public int IDX { get; set; }

        public int IDX_From { get; set; }

        public String FromAP { get; set; }

        public int IDX_To { get; set; }

        public String ToAP { get; set; }

        public int IDX_AC_Type { get; set; }

        public String ACType { get; set; }
        public String Currency { get; set; }

        public String RateType { get; set; }

        public int IDX_PriceList { get; set; }

        public double Budget { get; set; }

        public bool FOC { get; set; }

        public double Qty { get; set; }

        public double Rate { get; set; }
        public bool Invoiced { get; set; }

        public ReservationLegBudget()
        {
            DBState = DBState.IsNew;
        }

        public static implicit operator tsch_ReservationLegBudget(ReservationLegBudget legbudget)
        {
            var tschLegBudget = new tsch_ReservationLegBudget(); 
            tschLegBudget.IDX =  legbudget.DBState != DBState.IsDeleted ? legbudget.IDX : legbudget.IDX *-1;
            tschLegBudget.IDXFrom = legbudget.IDX_From;
            tschLegBudget.IDXTo = legbudget.IDX_To;
            tschLegBudget.IDXACtype = legbudget.IDX_AC_Type;
            tschLegBudget.Currency = legbudget.Currency.TrimEnd(' '); 
            tschLegBudget.RateType = legbudget.RateType;
            tschLegBudget.IDXPricelist = legbudget.IDX_PriceList;
            tschLegBudget.FOC = legbudget.FOC;
            tschLegBudget.Qty = legbudget.Qty;
            tschLegBudget.Rate = legbudget.Rate;
            tschLegBudget.Budget = legbudget.Budget;
            return tschLegBudget;

        }


        public static implicit operator tbReservationLegBudget(ReservationLegBudget legbudget)
        {
            var tbresLegBudget = new tbReservationLegBudget();
            tbresLegBudget.pkReservationLegBudgetID = legbudget.DBState != DBState.IsDeleted ? legbudget.IDX : legbudget.IDX * -1;
            tbresLegBudget.fkFromAPID = legbudget.IDX_From;
            tbresLegBudget.fkToAPID = legbudget.IDX_To;
            tbresLegBudget.fkACTypeID = legbudget.IDX_AC_Type;
            tbresLegBudget.Currency = legbudget.Currency.TrimEnd(' ');
            tbresLegBudget.RateType = legbudget.RateType;
            tbresLegBudget.fkPriceList = legbudget.IDX_PriceList;
            tbresLegBudget.FOC = legbudget.FOC;
            tbresLegBudget.Qty = legbudget.Qty;
            tbresLegBudget.Rate = legbudget.Rate;
            tbresLegBudget.Budget = legbudget.Budget;
            return tbresLegBudget;
            ;

        }


        public static explicit operator ReservationLegBudget(tbReservationLegBudget dbLegBudget)
        {
            var resLegBudget = new ReservationLegBudget();
            resLegBudget.DBState = DBState.IsLoaded;
            resLegBudget.IDX_From = dbLegBudget.fkFromAPID;
            if (dbLegBudget.tbAirstrip != null)
                resLegBudget.FromAP = dbLegBudget.tbAirstrip.IATA;
            resLegBudget.IDX_To = dbLegBudget.fkToAPID;
            if (dbLegBudget.tbAirstrip1 != null)
                resLegBudget.ToAP = dbLegBudget.tbAirstrip1.IATA;
            resLegBudget.IDX_AC_Type = dbLegBudget.fkACTypeID;
            if (dbLegBudget.tbAircraftType != null)
                resLegBudget.ACType = dbLegBudget.tbAircraftType.ACType;
            resLegBudget.Currency = dbLegBudget.Currency;
            resLegBudget.RateType = dbLegBudget.RateType;
            resLegBudget.IDX_PriceList = dbLegBudget.fkPriceList;
            resLegBudget.Budget = dbLegBudget.Budget;
            resLegBudget.FOC = dbLegBudget.FOC;
            resLegBudget.Qty = dbLegBudget.Qty;
            resLegBudget.Rate = dbLegBudget.Rate;
            resLegBudget.IDX = dbLegBudget.pkReservationLegBudgetID;
            //resLegBudget.Invoiced = (dbLegBudget.tset_GPExportInfo != null && dbLegBudget.tset_GPExportInfo.Count > 0);
            return resLegBudget;

        }

        public static explicit operator ReservationLegBudget(tsch_ReservationLegBudget dbLegBudget)
        {
            var resLegBudget = new ReservationLegBudget();
            resLegBudget.DBState = DBState.IsLoaded;
            resLegBudget.IDX_From = dbLegBudget.IDXFrom;
            if (dbLegBudget.tset_Airports != null)
                resLegBudget.FromAP = dbLegBudget.tset_Airports.IATA;
            resLegBudget.IDX_To = dbLegBudget.IDXTo;
            if (dbLegBudget.tset_Airports1 != null)
                resLegBudget.ToAP = dbLegBudget.tset_Airports1.IATA;
            resLegBudget.IDX_AC_Type = dbLegBudget.IDXACtype;
            if (dbLegBudget.tset_ACTypes != null)
                resLegBudget.ACType = dbLegBudget.tset_ACTypes.ACType;
            resLegBudget.Currency = dbLegBudget.Currency;
            resLegBudget.RateType = dbLegBudget.RateType;
            resLegBudget.IDX_PriceList = dbLegBudget.IDXPricelist;
            resLegBudget.Budget = dbLegBudget.Budget;
            resLegBudget.FOC = dbLegBudget.FOC;
            resLegBudget.Qty = dbLegBudget.Qty;
            resLegBudget.Rate = dbLegBudget.Rate;
            resLegBudget.IDX = dbLegBudget.IDX;
            resLegBudget.Invoiced = (dbLegBudget.tset_GPExportInfo != null && dbLegBudget.tset_GPExportInfo.Count > 0);
            return resLegBudget;

        }

    }
}
