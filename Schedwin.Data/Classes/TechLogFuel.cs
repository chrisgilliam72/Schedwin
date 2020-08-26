using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class TechLogFuel
    {
        public int IDX { get; set; }

        public int TechLogID { get; set; }

        public double FuelCost { get; set; }

        public int FuelQty { get; set; }

        public double OilCost { get; set; }

        public int QilQty { get; set; }

        public String Reciept { get; set; }

        public int IDX_AC_Details { get; set; }

        public int IDX_Airport_Dep { get; set; }

        public int IDX_Airport_Dest { get; set; }

        public static explicit operator t_Techlog_Fuel (TechLogFuel techLogFuel)
        {
            var t_Techlog_Fuel = new t_Techlog_Fuel();

            t_Techlog_Fuel.IDX = t_Techlog_Fuel.IDX;
            t_Techlog_Fuel.FuelCost = techLogFuel.FuelCost;
            t_Techlog_Fuel.FuelQty = techLogFuel.FuelQty;
            t_Techlog_Fuel.OilCost = techLogFuel.OilCost;
            t_Techlog_Fuel.OilQty = techLogFuel.QilQty;
            t_Techlog_Fuel.Receipt = techLogFuel.Reciept;
            t_Techlog_Fuel.TechlogID = techLogFuel.TechLogID;
            return t_Techlog_Fuel;

        }

        public static explicit operator TechLogFuel(t_Techlog_Fuel t_techLog_Fuel)
        {
            var techLogFuel = new TechLogFuel();
            techLogFuel.TechLogID = t_techLog_Fuel.TechlogID;
            techLogFuel.FuelCost = t_techLog_Fuel.FuelCost;
            techLogFuel.FuelQty = t_techLog_Fuel.FuelQty;
            techLogFuel.OilCost = t_techLog_Fuel.OilCost;
            techLogFuel.QilQty = t_techLog_Fuel.OilQty;
            techLogFuel.Reciept = t_techLog_Fuel.Receipt;
            techLogFuel.IDX_AC_Details = t_techLog_Fuel.IDX_AC_Details;
            techLogFuel.IDX_Airport_Dep = t_techLog_Fuel.IDX_Airports ?? 0;
            techLogFuel.IDX_Airport_Dest = t_techLog_Fuel.IDX_AirportDest;
             return techLogFuel;

        }
    }
}
