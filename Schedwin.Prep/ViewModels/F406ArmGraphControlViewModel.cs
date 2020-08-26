using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Schedwin.Prep
{
    class F406ArmGraphControlViewModel : ArmGraphControlViewModelBase
    {
        public F406ArmGraphControlViewModel()
        {
            var tmpPoints = new PointCollection();
            tmpPoints.Add(ValueToPoint(167,5500));
            tmpPoints.Add(ValueToPoint(167, 6000));
            tmpPoints.Add(ValueToPoint(167, 6500));
            tmpPoints.Add(ValueToPoint(168, 7100));
            tmpPoints.Add(ValueToPoint(169, 7700));
            tmpPoints.Add(ValueToPoint(170, 8400));
            tmpPoints.Add(ValueToPoint(171, 8900));
            tmpPoints.Add(ValueToPoint(172, 9500));
            tmpPoints.Add(ValueToPoint(173, 9925));
            tmpPoints.Add(ValueToPoint(180, 9925));
            tmpPoints.Add(ValueToPoint(180, 5500));
            EnevelopePoints = tmpPoints;
        }

        private Point ValueToPoint (int armValue,int weightValue)
        {
            var pnt = new Point();
            pnt.X = ArmToPointX(armValue);
            pnt.Y = LbsToPointY(weightValue);
            return pnt;

        }
        private int LbsToPointY(int weight)
        {
            double retVal = ((10500.0 -(double) weight) / 5000.0) * 175;
            return Convert.ToInt32(retVal);
        }

        private int ArmToPointX(int Arm)
        {
            double retVal = (((double)Arm - 165.0) / 20.0) *200;
            return Convert.ToInt32(retVal);
        }

        public override void Refresh(double TakeOffWeight, double TakeOffArm, double LandingWeight, double LandingArm)
        {
            if ((LandingArm >164 && LandingArm < 186) && (LandingWeight > 5499 && LandingWeight < 10501))
            {
                LandingCircleX = ArmToPointX (Convert.ToInt32(LandingArm))- 2;
                LandingCircleY= LbsToPointY(Convert.ToInt32(LandingWeight)) + 2;
            }

            if ((TakeOffArm > 164 && TakeOffArm < 186) && (TakeOffWeight > 5499 && TakeOffWeight < 10501))
            {
                int tmpTakeOffX = ArmToPointX(Convert.ToInt32(TakeOffArm));
                int tmpTakeOffY = LbsToPointY(Convert.ToInt32(TakeOffWeight));
                var takeoffTriangle = new PointCollection();

                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX - 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX + 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX, tmpTakeOffY));


                TakeOffTriangle = takeoffTriangle;
            }

        }
    }
}
