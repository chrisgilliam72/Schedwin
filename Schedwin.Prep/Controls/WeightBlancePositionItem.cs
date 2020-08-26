using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Data;

namespace Schedwin.Prep
{
    public class WeightBalancePositionItem : ViewModelBase
    {
        public int IDX_Loading { get; set; }
        public int IDX_Leg { get; set; }
        public int MaxSeats { get; set; }
        public String Name { get; set; }

        private bool _freight;
        public bool Freight
        {
            get
            {
                return _freight;
            }
            set
            {
                _freight = value;
                NotifyPropertyChanged("Freight");
            }
        }

        public int AdditionalFreightWeight { get; set; }
        private double _weight;
        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                NotifyPropertyChanged("Weight");
                NotifyPropertyChanged("Mom");
            }
        }

        private double _Arm;
        public double Arm
        {
            get
            {
                return _Arm;
            }
            set
            {
                _Arm = value;
                NotifyPropertyChanged("Arm");
                NotifyPropertyChanged("Mom");
            }
        }
        public double Mom
        {
            get
            {
                return Math.Round(Weight * Arm / 1000.0, 1);
            }
        }

        public int PilotWeight
        {
            get
            {
                var pilotSeat = PaxSeatAssignments.FirstOrDefault(x => x.IsPilot);
                return pilotSeat != null ? pilotSeat.Weight : 0;
            }

        }

        public bool UseForZFW { get; set; }

        public PaxRowSeatAssignment[] PaxSeatAssignments { get; set; }



        public String SeatingAssignment
        {
            get
            {
                String seating = "";

                for (int i = 0; i < MaxSeats; i++)
                {
                    if (PaxSeatAssignments[i].Show)
                    {
                        if (!String.IsNullOrEmpty(PaxSeatAssignments[i].Gender))
                            seating += PaxSeatAssignments[i].Gender + " ";
                        else
                            seating += "- ";
                    }
                }

                return seating;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                     var tmpSeatingAssignment = value.TrimEnd(' ');
                    var seats = tmpSeatingAssignment.Split(' ');
                    MaxSeats = seats.Count();
                    for (int i = 0; i < MaxSeats; i++)
                    {
                        PaxSeatAssignments[i].Show = true;
                        if (seats[i] != "-")
                            PaxSeatAssignments[i].Gender = seats[i];
                    }
                }
            }
        }
        private bool _CanEdit { get; set; }
        public bool CanEdit
        {
            get
            {
                return _CanEdit;
            }
            set
            {
                _CanEdit = value;
                NotifyPropertyChanged("CanEdit");
            }
        }

        public void AddPilot(int pilotWeight)
        {
            PaxSeatAssignments[0].Weight = pilotWeight;
            PaxSeatAssignments[0].Gender = "P";
            Weight += pilotWeight;
        }



        public void RefreshSeatingAssignment()
        {
            NotifyPropertyChanged("SeatingAssignment");
        }

        public WeightBalancePositionItem(String seatingAssignment)
        {
            PaxSeatAssignments = new PaxRowSeatAssignment[10] { new PaxRowSeatAssignment(true), new PaxRowSeatAssignment(true), new PaxRowSeatAssignment(false), new PaxRowSeatAssignment(false), new PaxRowSeatAssignment(),
                                                               new PaxRowSeatAssignment(), new PaxRowSeatAssignment(), new PaxRowSeatAssignment(), new PaxRowSeatAssignment(), new PaxRowSeatAssignment()  };

            if (!String.IsNullOrEmpty(seatingAssignment))
            {
                seatingAssignment = seatingAssignment.TrimEnd(' ');
                var seats = seatingAssignment.Split(' ');
                MaxSeats = seats.Count();
                for (int i=0;i<MaxSeats;i++)
                {
                   PaxSeatAssignments[i].ParentPositionItem = this;
                   PaxSeatAssignments[i].Show = true;
                    if (seats[i]!="-")
                        PaxSeatAssignments[i].Gender = seats[i];
                }
            }

        }

        public WeightBalancePositionItem(int maxSeats, String rowName )
        {
            UseForZFW = false;
            CanEdit = false;
            MaxSeats = maxSeats;
            Name = rowName;           
        }

        public WeightBalancePositionItem(int maxSeats)
        {
            UseForZFW = true;
            CanEdit = true;
            MaxSeats = maxSeats;
            PaxSeatAssignments = new PaxRowSeatAssignment[10] { new PaxRowSeatAssignment(true), new PaxRowSeatAssignment(true), new PaxRowSeatAssignment(false), new PaxRowSeatAssignment(false), new PaxRowSeatAssignment(),
                                                               new PaxRowSeatAssignment(), new PaxRowSeatAssignment(), new PaxRowSeatAssignment(), new PaxRowSeatAssignment(), new PaxRowSeatAssignment()  };
            for (int i = 0; i < maxSeats; i++)
            {
                PaxSeatAssignments[i].ParentPositionItem = this;
                PaxSeatAssignments[i].Show = true;
            }

        }


        public static explicit operator WeightBalancePositionItem(tsch_LegWeightBalances tschLegWeightBalance)
        {
            var weightBalancePositionItem = new WeightBalancePositionItem(tschLegWeightBalance.Seating);
            weightBalancePositionItem.IDX_Leg = tschLegWeightBalance.IDX_Leg;
            weightBalancePositionItem.Name= tschLegWeightBalance.Row_Name;
 
            weightBalancePositionItem.Weight = tschLegWeightBalance.TotalWeight;
            weightBalancePositionItem.Freight = tschLegWeightBalance.Freight;
            weightBalancePositionItem.CanEdit = tschLegWeightBalance.CanEdit;
            weightBalancePositionItem.UseForZFW = tschLegWeightBalance.UseForZFW;
            weightBalancePositionItem.IDX_Loading = tschLegWeightBalance.IDX_Loading;
            return weightBalancePositionItem;

        }

        public static explicit operator tsch_LegWeightBalances(WeightBalancePositionItem weightBalancePositionItem)
        {
            var tschLegWeightBalance = new tsch_LegWeightBalances();
            tschLegWeightBalance.IDX_Leg = weightBalancePositionItem.IDX_Leg;
            tschLegWeightBalance.Row_Name = weightBalancePositionItem.Name;
            tschLegWeightBalance.Seating = weightBalancePositionItem.SeatingAssignment;
            tschLegWeightBalance.TotalWeight = weightBalancePositionItem.Weight;
            tschLegWeightBalance.Freight = weightBalancePositionItem.Freight;
            tschLegWeightBalance.CanEdit = weightBalancePositionItem.CanEdit;
            tschLegWeightBalance.UseForZFW = weightBalancePositionItem.UseForZFW;
            tschLegWeightBalance.IDX_Loading = weightBalancePositionItem.IDX_Loading;
            return tschLegWeightBalance;
        }
    }
}
