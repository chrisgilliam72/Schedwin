using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data
{
   public class ACWeightStation
    {
        public int IDX { get; set; }
        public String StationWeight { get; set; }

        public short Rank { get; set; }
        
        public ACWeightStation()
        {
            StationWeight = "0";
        }

        public static explicit operator tset_ACWeightStation(ACWeightStation weightStation)
        {
            var tsetWeightStation = new tset_ACWeightStation();
            tsetWeightStation.IDX_Station = weightStation.IDX;
            tsetWeightStation.StationMaxWeight = weightStation.StationWeight;
            tsetWeightStation.Rank = Convert.ToByte(weightStation.Rank);
            return tsetWeightStation;
        }

        public static explicit operator ACWeightStation (tset_ACWeightStation tset_ACWeight)
        {
            var acStationWeight = new ACWeightStation();
            acStationWeight.IDX = tset_ACWeight.IDX_Station;

            acStationWeight.StationWeight = tset_ACWeight.StationMaxWeight!=null? tset_ACWeight.StationMaxWeight :"0";
            acStationWeight.Rank = tset_ACWeight.Rank;
            return acStationWeight;
        }
    }
}
