using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{

    public class ACLoadingArrangement :ViewModelBase
    {

        public bool IsNew { get; set; }
        public int IDX { get; set; }
        public String Name { get; set; }

        public int IDX_ACType { get; set; }
        public RangeObservableCollection<ACLoadingStation> Stations { get; set; }
        public RangeObservableCollection<ACLoadingStation> PaxStations
        {
           get
            {
                var paxStationsLst = new RangeObservableCollection<ACLoadingStation>();
                var tmpLst = Stations.Where(x => x.Type == "Pax").ToList();
                paxStationsLst.AddRange(tmpLst);
                return paxStationsLst;
            }
        }
        public RangeObservableCollection<ACLoadingStation> WeightStations
        {
            get
            {
                var freightStationsLst = new RangeObservableCollection<ACLoadingStation>();
                var tmpLst = Stations.Where(x => x.Type == "Freight").ToList();
                freightStationsLst.AddRange(tmpLst);
                return freightStationsLst;
            }
        }


        public static explicit operator tset_ACTypeLoadingArrangement(ACLoadingArrangement  loadingArrangement)
        {
            var tsetArrangement = new tset_ACTypeLoadingArrangement();
            tsetArrangement.IDX = loadingArrangement.IDX;
            tsetArrangement.LoadingArrangements = loadingArrangement.Name;
            tsetArrangement.IDX_ACTypes = loadingArrangement.IDX_ACType;

            return tsetArrangement;
        }


        public static explicit operator ACLoadingArrangement (tset_ACTypeLoadingArrangement tsetArrangement)
        {
            var acLoadingArrangement = new ACLoadingArrangement();
            acLoadingArrangement.IDX = tsetArrangement.IDX;
            acLoadingArrangement.Name = tsetArrangement.LoadingArrangements;
            acLoadingArrangement.IDX_ACType = tsetArrangement.IDX_ACTypes;
            acLoadingArrangement.IsNew = false;
            if (tsetArrangement.tset_ACTypes_Station!=null)
            {
                var tmpList = tsetArrangement.tset_ACTypes_Station.Select(x=> (ACLoadingStation)x).ToList();
                acLoadingArrangement.Stations.AddRange(tmpList);

            }
            return acLoadingArrangement;
        }

        public void RefreshStations()
        {
            NotifyPropertyChanged("PaxStations");
            NotifyPropertyChanged("WeightStations");
        }
        public ACLoadingArrangement()
        {
            Stations = new RangeObservableCollection<ACLoadingStation>();
            IsNew = true;
        }

        public async Task Save(String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
           
            using (ctx)
            {
                var tsetArrangement = (tset_ACTypeLoadingArrangement)this;
                if (IsNew)
                {
                    ctx.tset_ACTypeLoadingArrangement.Add(tsetArrangement);

                    foreach (var station in Stations)
                    {
                        var tsetStation = (tset_ACTypes_Station)(station);
                        tsetArrangement.tset_ACTypes_Station.Add(tsetStation);
                    }

                }
                else
                {
                    var dbArrangement =  await ctx.tset_ACTypeLoadingArrangement
                             .Include("tset_ACTypes_Station").Include("tset_ACTypes_Station.tlst_StationType").
                              Include("tset_ACTypes_Station.tset_ACPaxStation").
                               Include("tset_ACTypes_Station.tset_ACWeightStation")
                              .FirstOrDefaultAsync(x => x.IDX == IDX);

                    foreach (var station in Stations)
                    {
                        var dbStation = dbArrangement.tset_ACTypes_Station.FirstOrDefault(x => x.IDX == station.IDX);
                        if (dbStation!=null  && dbStation.IDX >0)
                        {
                            dbStation.StationNumber = station.Number;
                            dbStation.StationName = station.Name;
                            dbStation.StationArm = station.Arm;
                            dbStation.StationMaxWeight = station.Weight.ToString();
                            dbStation.MaxNumSeats = Convert.ToByte(station.MaxSeats);
                            if (station.Type=="Pax" )
                            {
                                if (dbStation.tset_ACPaxStation == null)
                                {
                                    dbStation.tset_ACPaxStation = new tset_ACPaxStation();
                                }
                                  
                              
                                dbStation.tset_ACPaxStation.C0_Pax = station.PaxStations[0];
                                dbStation.tset_ACPaxStation.C1_Pax = station.PaxStations[1];
                                dbStation.tset_ACPaxStation.C2_Pax = station.PaxStations[2];
                                dbStation.tset_ACPaxStation.C3_Pax = station.PaxStations[3];
                                dbStation.tset_ACPaxStation.C4_Pax = station.PaxStations[4];
                                dbStation.tset_ACPaxStation.C5_Pax = station.PaxStations[5];
                                dbStation.tset_ACPaxStation.C6_Pax = station.PaxStations[6];
                                dbStation.tset_ACPaxStation.C7_Pax = station.PaxStations[7];
                                dbStation.tset_ACPaxStation.C8_Pax = station.PaxStations[8];
                                dbStation.tset_ACPaxStation.C9_Pax = station.PaxStations[9];
                                dbStation.tset_ACPaxStation.C10_Pax = station.PaxStations[10];
                                dbStation.tset_ACPaxStation.C11_Pax = station.PaxStations[11];
                                dbStation.tset_ACPaxStation.C12_Pax = station.PaxStations[12];
                                dbStation.tset_ACPaxStation.C13_Pax = station.PaxStations[13];
                                dbStation.tset_ACPaxStation.C14_Pax = station.PaxStations[14];
                                dbStation.tset_ACPaxStation.C15_Pax = station.PaxStations[15];
                           
                              
                            }
                            else if (station.Type=="Freight" )
                            {
                                if( dbStation.tset_ACWeightStation == null)
                                {
                                    dbStation.tset_ACWeightStation = new tset_ACWeightStation();
                                }
                                dbStation.tset_ACWeightStation.Rank = Convert.ToByte(station.WeightStation.Rank);
                                dbStation.tset_ACWeightStation.StationMaxWeight = station.WeightStation.StationWeight;

                            }
                                                    
                        }
                        else
                        {
                            var newdbStation = (tset_ACTypes_Station)station;
                            dbArrangement.tset_ACTypes_Station.Add(newdbStation);
                        }
                    }

                }

                 await ctx.SaveChangesAsync();

                IsNew = false;
            }
        }

        public static async Task<List<ACLoadingArrangement>> LoadLoadArrangements(int IDX_ACType, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            using (ctx)
            {
               var dbArrangements =  await ctx.tset_ACTypeLoadingArrangement.Where(x=>x.IDX_ACTypes==IDX_ACType)
                                           .Include("tset_ACTypes_Station").Include("tset_ACTypes_Station.tlst_StationType").
                                            Include("tset_ACTypes_Station.tset_ACPaxStation").
                                             Include("tset_ACTypes_Station.tset_ACWeightStation")
                                            .ToListAsync();
                var acLoadingArrangements = dbArrangements.Select(x => (ACLoadingArrangement)x).ToList();
                return acLoadingArrangements;
            }
        }
    }
}
