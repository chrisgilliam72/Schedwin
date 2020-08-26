using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{

    public class APDistance
    {
        public int IDX { get; set; }
        public int StartAP_IDX { get; set; }
        public int EndAP_IDX { get; set; }
        public int Distance { get; set; }
        public String StartAP { get; set; }
        public String EndAP { get; set; }
    }

    public class APDistances
    {
        private static List<APDistance> distanceMatrix { get; set; }

        public static List<APDistance> GetDistanceMatrix()
        {
            return distanceMatrix;
        }
   
        public async static Task<bool> LoadDistanceMatrix(String Server, string regionalDBName)
        {
           
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var distances = await ctx.tset_Distance.Include("tset_Airports").
                                                        Include("tset_Airports1").ToListAsync();

                distanceMatrix = distances.Select(x => new APDistance {IDX=x.IDX,StartAP=x.tset_Airports.IATA,StartAP_IDX = x.StartAP, EndAP=x.tset_Airports1.IATA, EndAP_IDX = x.DestAP, Distance = x.Distance.Value }).ToList();

                return true;
            
        }

        public async static Task<bool> Save(APDistance distance, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                using (ctx)
                {
                    var distances = await ctx.tset_Distance.Where(x => (x.StartAP == distance.StartAP_IDX && x.DestAP == distance.EndAP_IDX) ||
                                                  (x.StartAP == distance.EndAP_IDX && x.DestAP == distance.StartAP_IDX)).ToListAsync();

                    if (distances != null)
                        ctx.tset_Distance.RemoveRange(distances);

                    var tsetDistance = new tset_Distance();
                    tsetDistance.StartAP = distance.StartAP_IDX;
                    tsetDistance.DestAP = distance.EndAP_IDX;
                    tsetDistance.Distance = distance.Distance;
                    ctx.tset_Distance.Add(tsetDistance);

                    await ctx.SaveChangesAsync();

                }

                return true;
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                throw new Exception(message);
            }

        }

   

        public static List<APDistance> GetDistancesFrom(String StartAP)
        {
            if (distanceMatrix != null)
            {
               return distanceMatrix.Where(x => x.StartAP == StartAP || x.EndAP == StartAP).ToList();

            }

            return null;
        }

        public static APDistance GetAPDistance(int StartAP_IDX, int EndAP_IDX)
        {
            if (distanceMatrix != null)
            {
                var distance = distanceMatrix.FirstOrDefault(x => (x.StartAP_IDX == StartAP_IDX && x.EndAP_IDX == EndAP_IDX) || (x.StartAP_IDX == StartAP_IDX && x.EndAP_IDX == EndAP_IDX));
                return distance;
            }

            return null;
        }

        public static APDistance GetAPDistance(String StartAP, String EndAP)
        {
            if (distanceMatrix != null)
            {
                var distance = distanceMatrix.FirstOrDefault(x => (x.StartAP == StartAP && x.EndAP == EndAP) || (x.StartAP == EndAP && x.EndAP == StartAP));
                return distance;
            }

            return null;
        }


        public static int GetDistance(int StartAP, int EndAP)
        {
            if (distanceMatrix!=null)
            {
                var distance = distanceMatrix.FirstOrDefault(x => (x.StartAP_IDX == StartAP && x.EndAP_IDX == EndAP) || (x.StartAP_IDX == EndAP && x.EndAP_IDX == StartAP));
                return distance != null ? distance.Distance : -1;
            }

            return -1;
        }

        public static bool GetAPDistances(ref List<ReservationLegDistance> Distances, String Server, string regionalDBName)
        {
            try
            {
                var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
                var ctx = new SchedwinRegionalEntities(constring);
                var ap1S = Distances.Select(x => x.AP1).ToList();
                var ap2S = Distances.Select(x => x.AP2).ToList();

                using (ctx)
                {
                    var set1 = ctx.tset_Distance.Where(x => ap1S.Contains(x.tset_Airports.IATA) && ap2S.Contains(x.tset_Airports1.IATA)).ToList();
                    var set2 = ctx.tset_Distance.Where(x => ap2S.Contains(x.tset_Airports.IATA) && ap1S.Contains(x.tset_Airports1.IATA)).ToList();
                    var all = set1.Union(set2).ToList();

                    foreach (var legDistance in Distances)
                    {
                        var tsetdis = all.FirstOrDefault(x => (x.tset_Airports.IATA == legDistance.AP1 && x.tset_Airports1.IATA == legDistance.AP2) ||
                                                 (x.tset_Airports.IATA == legDistance.AP2 && x.tset_Airports1.IATA == legDistance.AP1));
                        if (tsetdis != null)
                            legDistance.Distance = tsetdis.Distance.Value;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var message = string.Join(Environment.NewLine, messages);
                throw new Exception(message);
            }
        }
    }
}
