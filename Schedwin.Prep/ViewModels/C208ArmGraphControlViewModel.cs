using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Schedwin.Prep
{
    public class C208ArmGraphControlViewModel : ArmGraphControlViewModelBase
    {

        public C208ArmGraphControlViewModel()
        {

            LandingCircleX = 2;
            LandingCircleY = 170;
            var takeoffTriangle = new PointCollection();
            takeoffTriangle.Add(new System.Windows.Point(0, 175));
            takeoffTriangle.Add(new System.Windows.Point(5, 175));
            takeoffTriangle.Add(new System.Windows.Point(3, 170));
            TakeOffTriangle = takeoffTriangle;
            var tmpPoints = new PointCollection();
            tmpPoints.Add(new Point(0, 175));
            tmpPoints.Add(new Point(65, 50));
            tmpPoints.Add(new Point(95, 36));
            tmpPoints.Add(new Point(100, 0));
            tmpPoints.Add(new Point(115, 0));
            tmpPoints.Add(new Point(115, 175));

            EnevelopePoints = tmpPoints;

        }

        public override void Refresh(double TakeOffWeight, double TakeOffArm, double LandingWeight, double LandingArm)
        {
            if ((LandingArm > 179 && LandingArm < 206) && (LandingWeight > 5999 && LandingWeight < 9001))
            {
                LandingCircleX = Convert.ToInt32(((LandingArm - 180.0) / 25.0) * 125.0) - 2;
                LandingCircleY = Convert.ToInt32(((9000 - LandingWeight) / 3000.0) * 175.0) + 2;
            }

            if ((TakeOffArm > 179 && TakeOffArm < 206) && (TakeOffWeight > 5999 && TakeOffWeight < 9001))
            {
                int tmpTakeOffX = Convert.ToInt32(((TakeOffArm - 180.0) / 25.0) * 125.0);
                int tmpTakeOffY = Convert.ToInt32(((9000 - TakeOffWeight) / 3000.0) * 175.0);
                var takeoffTriangle = new PointCollection();

                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX - 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX + 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX, tmpTakeOffY));


                TakeOffTriangle = takeoffTriangle;
            }

        }


    }
}
