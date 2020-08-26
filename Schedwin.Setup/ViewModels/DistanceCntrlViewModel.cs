using Schedwin.Common;
using Schedwin.Data.Classes;
using System.Windows.Data;
using Telerik.Windows.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Schedwin.Setup
{
    public class DistanceCntrlViewModel : ItemCntrlViewModelBase
    {
        public String selelectedSourceAP { get; set; }
        public String selectedDestAP { get; set; }

        public DistanceCntrlView View { get; set; }
        public RangeObservableCollection<DistanceGridItem> DistanceMatrix { get; set; }

        public DistanceCntrlViewModel()
        {
            DistanceMatrix = new RangeObservableCollection<DistanceGridItem>();

        }

        public double GetAutoDistance()
        {
            var srcAirStrip = AirstripInfo.GetAirstripInfo(selelectedSourceAP);
            var destAirstrip = AirstripInfo.GetAirstripInfo(selectedDestAP);

            var dist= CalculateDistance((srcAirStrip.DecimalLat*Math.PI)/180, (srcAirStrip.DecimalLong*Math.PI)/180, 
                                         (destAirstrip.DecimalLat*Math.PI)/180, (destAirstrip.DecimalLong*Math.PI)/180);

            SuccessWindow.Display("Calculated distance is :" + Environment.NewLine + " " + dist.ToString("F")+" Kms");

            return dist;
        }


        private double CalculateDistance(Double lat1, Double lon1, Double lat2 , Double lon2)

        {
            double retVal = 0;
            double tmp_dist;

            tmp_dist = (System.Math.Sin(lat1) * System.Math.Sin(lat2)) + (System.Math.Cos(lat1) * System.Math.Cos(lat2) * System.Math.Cos(lon2 - lon1));
            retVal = 6378.7 * Math.Acos(tmp_dist);

            return retVal;
        }


        public async Task<bool> UpdateDistance(String srcAP, string destAP, int distance)
        {
            try
            {
                var newAPDistance = new APDistance();
                newAPDistance.StartAP = srcAP;
                newAPDistance.EndAP = destAP;
                newAPDistance.Distance = distance;
                newAPDistance.StartAP_IDX = AirstripInfo.GetAirstripIDX(srcAP);
                newAPDistance.EndAP_IDX = AirstripInfo.GetAirstripIDX(destAP);

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await APDistances.Save(newAPDistance, Server, Database);
                }

                return true;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Unable to save distance  :" + Environment.NewLine + exMessage);
                return false;

            }

          
        }

        public  override void Init()
        {
            DistanceMatrix.Clear();
            var tempDistMatrix = APDistances.GetDistanceMatrix();
            var airstrips = AirstripInfo.GetAirstrips().ToList();
            var apList = airstrips.Select(x => x.Code).ToList();
            //var tempGrpedMatrix = tempDistMatrix.GroupBy(x => x.StartAP).OrderBy(x=>x.Key).ToList();

            //var StartAPList = tempDistMatrix.Select(x => x.StartAP).ToList();
            //var EndAPList = tempDistMatrix.Select(x => x.EndAP).ToList();
            //var combinedList= StartAPList.Union(EndAPList).ToList();
            //combinedList = combinedList.GroupBy(x => x).Select(x => x.First()).OrderBy(x=>x).ToList();
            List<DistanceGridItem> tmpList = new List<DistanceGridItem>();

            foreach (var startAP in apList)
            {
                var distancesFrom = APDistances.GetDistancesFrom(startAP);
                if (distancesFrom!=null)
                {
                    var gridviewColumn = new Telerik.Windows.Controls.GridViewDataColumn();
                    gridviewColumn.IsReadOnly = false;
                    gridviewColumn.Width = 75;
                    gridviewColumn.Header = startAP;
                    gridviewColumn.ToolTip = startAP;
                    gridviewColumn.DataMemberBinding = new Binding("DistanceList[" + startAP + "]");

                    View.distanceGrid.Columns.Add(gridviewColumn);

                    var gridItem = new DistanceGridItem();
                    gridItem.AP = startAP;

                    foreach (var endAP in apList)
                    {
                        var apDistance = distancesFrom.FirstOrDefault(x => (x.StartAP == startAP && x.EndAP == endAP) || (x.StartAP == endAP && x.EndAP == startAP));
                        if (apDistance != null)
                        {
                            if (apDistance.Distance > -1  && !gridItem.DistanceList.ContainsKey(endAP))
                                gridItem.DistanceList.Add(endAP, apDistance.Distance.ToString());
                            else
                            {
                                if (!gridItem.DistanceList.ContainsKey(endAP))
                                    gridItem.DistanceList.Add(endAP, "");
                                else
                                    MessageBox.Show("Duplicate distance StartAP=" + startAP + " EndAP=" + endAP);
                            }
   

                        }
                        else
                        {
                            if (!gridItem.DistanceList.ContainsKey(endAP))
                                gridItem.DistanceList.Add(endAP, "");
                            //else
                            //    MessageBox.Show("Duplicate distance StartAP=" + startAP + " EndAP=" + endAP);
                        }
                          

                    }

                    tmpList.Add(gridItem);
                }


            }

            DistanceMatrix.AddRange(tmpList.OrderBy(x => x.AP).ToList());
            NotifyPropertyChanged("DistanceMatrix");
        }


    }
}
