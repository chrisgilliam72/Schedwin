using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Prep
{
    public class WeightBalanceLeg
    {
        public int LegNo { get; set; }
        public String FromAP { get; set; }
        public String ToAP { get; set; }
        public String TET { get; set; } 

        public List<WeightBalancePositionItem> RowItems { get; set; }

        public WeightBalanceLeg()
        {
            RowItems = new List<WeightBalancePositionItem>();
        }

        public String DisplayValue
        {
            get
            {
                return LegNo.ToString() + " " + FromAP + " - " + ToAP + " TET:" + TET;

            }
        }


    }
}
