using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Schedwin.Data.Classes
{
    public class ACLoadingStation
    {
        public int IDX { get; set; }
        public short Number { get; set; }
        public String Name { get; set; }
        public float Arm { get; set; }
        public int  Weight { get; set; }
        public String Type { get; set; }
        public int IDX_Type { get;set; }

        public int IDX_AC_TYPE { get; set; }
        public short MaxSeats { get; set; }

        public int[] PaxStations { get; set; }

        public ACWeightStation WeightStation { get; set; }

        public ACLoadingStation()
        {
            PaxStations = new int[16] { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
            WeightStation = new ACWeightStation();
            MaxSeats = 1;
        }

        public static explicit operator tset_ACTypes_Station(ACLoadingStation acLoadingStation)
        {
            var tsetStation = new tset_ACTypes_Station();
            tsetStation.IDX = acLoadingStation.IDX;
            tsetStation.StationNumber = acLoadingStation.Number;
            tsetStation.StationName = acLoadingStation.Name;
            tsetStation.StationArm = acLoadingStation.Arm;
            tsetStation.StationMaxWeight = acLoadingStation.Weight.ToString();
            tsetStation.StationType = acLoadingStation.IDX_Type;
            tsetStation.IDX_ACTypes = acLoadingStation.IDX_AC_TYPE;
            tsetStation.MaxNumSeats = Convert.ToByte(acLoadingStation.MaxSeats);
            if (acLoadingStation.Type == "Pax")
            {
                var tsetPaxStation = new tset_ACPaxStation();
                tsetPaxStation.C0_Pax = acLoadingStation.PaxStations[0];
                tsetPaxStation.C1_Pax = acLoadingStation.PaxStations[1];
                tsetPaxStation.C2_Pax = acLoadingStation.PaxStations[2];
                tsetPaxStation.C3_Pax = acLoadingStation.PaxStations[3];
                tsetPaxStation.C4_Pax = acLoadingStation.PaxStations[4];
                tsetPaxStation.C5_Pax = acLoadingStation.PaxStations[5];
                tsetPaxStation.C6_Pax = acLoadingStation.PaxStations[6];
                tsetPaxStation.C7_Pax = acLoadingStation.PaxStations[7];
                tsetPaxStation.C8_Pax = acLoadingStation.PaxStations[8];
                tsetPaxStation.C9_Pax = acLoadingStation.PaxStations[9];
                tsetPaxStation.C10_Pax = acLoadingStation.PaxStations[10];
                tsetPaxStation.C11_Pax = acLoadingStation.PaxStations[11];
                tsetPaxStation.C12_Pax = acLoadingStation.PaxStations[12];
                tsetPaxStation.C13_Pax = acLoadingStation.PaxStations[13];
                tsetPaxStation.C14_Pax = acLoadingStation.PaxStations[14];
                tsetPaxStation.C15_Pax = acLoadingStation.PaxStations[15];
                tsetStation.tset_ACPaxStation = tsetPaxStation;            
            }
            else if (acLoadingStation.Type=="Freight")
            {
                var tsetWeightStation = new tset_ACWeightStation();
                tsetWeightStation.StationMaxWeight = acLoadingStation.WeightStation.StationWeight!=null ? acLoadingStation.WeightStation.StationWeight :"";
                tsetWeightStation.Rank = Convert.ToByte(acLoadingStation.WeightStation.Rank);
                tsetStation.tset_ACWeightStation = tsetWeightStation;

            }
            return tsetStation;
        }

        public static explicit operator ACLoadingStation(tset_ACTypes_Station tsetStation)
        {
            var acLoadingStation = new ACLoadingStation();
            acLoadingStation.IDX = tsetStation.IDX;
            acLoadingStation.Number = tsetStation.StationNumber;
            acLoadingStation.Name = tsetStation.StationName;
            acLoadingStation.Arm = tsetStation.StationArm;
            acLoadingStation.Weight = Convert.ToInt32(tsetStation.StationMaxWeight);
            acLoadingStation.Type = tsetStation.tlst_StationType != null ? tsetStation.tlst_StationType.StationType : "";
            acLoadingStation.IDX_AC_TYPE = tsetStation.IDX_ACTypes;
            acLoadingStation.MaxSeats = tsetStation.MaxNumSeats ?? 0;
            if (tsetStation.tset_ACPaxStation!=null)
            {
                acLoadingStation.PaxStations[0] = tsetStation.tset_ACPaxStation.C0_Pax ?? 0;
                acLoadingStation.PaxStations[1] = tsetStation.tset_ACPaxStation.C1_Pax ?? 0;
                acLoadingStation.PaxStations[2] = tsetStation.tset_ACPaxStation.C2_Pax ?? 0;
                acLoadingStation.PaxStations[3] = tsetStation.tset_ACPaxStation.C3_Pax ?? 0;
                acLoadingStation.PaxStations[4] = tsetStation.tset_ACPaxStation.C4_Pax ?? 0;
                acLoadingStation.PaxStations[5] = tsetStation.tset_ACPaxStation.C5_Pax ?? 0;
                acLoadingStation.PaxStations[6] = tsetStation.tset_ACPaxStation.C6_Pax ?? 0;
                acLoadingStation.PaxStations[7] = tsetStation.tset_ACPaxStation.C7_Pax ?? 0;
                acLoadingStation.PaxStations[8] = tsetStation.tset_ACPaxStation.C8_Pax ?? 0;
                acLoadingStation.PaxStations[9] = tsetStation.tset_ACPaxStation.C9_Pax ?? 0;
                acLoadingStation.PaxStations[10] = tsetStation.tset_ACPaxStation.C10_Pax ?? 0;
                acLoadingStation.PaxStations[11] = tsetStation.tset_ACPaxStation.C11_Pax ?? 0;
                acLoadingStation.PaxStations[12] = tsetStation.tset_ACPaxStation.C12_Pax ?? 0;
                acLoadingStation.PaxStations[13] = tsetStation.tset_ACPaxStation.C13_Pax ?? 0;
                acLoadingStation.PaxStations[14] = tsetStation.tset_ACPaxStation.C14_Pax ?? 0;
                acLoadingStation.PaxStations[15] = tsetStation.tset_ACPaxStation.C15_Pax ?? 0;
            }

            if (tsetStation.tset_ACWeightStation != null)
                acLoadingStation.WeightStation = (ACWeightStation)tsetStation.tset_ACWeightStation;
            else if (tsetStation.StationType == 3)
                acLoadingStation.WeightStation = new ACWeightStation();
            return acLoadingStation;
        }
    }
}
