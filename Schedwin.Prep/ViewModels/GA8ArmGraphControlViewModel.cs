using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Schedwin.Prep
{
    public class GA8ArmGraphControlViewModel : ArmGraphControlViewModelBase
    {
        public GA8ArmGraphControlViewModel()
        {
           // "30,200 120,20 190,20 190,200" 

            LandingCircleX = 2;
            LandingCircleY = 170;
            var takeoffTriangle = new PointCollection();
            takeoffTriangle.Add(new System.Windows.Point(0, 175));
            takeoffTriangle.Add(new System.Windows.Point(5, 175));
            takeoffTriangle.Add(new System.Windows.Point(3, 170));
            TakeOffTriangle = takeoffTriangle;
            var tmpPoints = new PointCollection();
            tmpPoints.Add(new Point(30,200));
            tmpPoints.Add(new Point(120, 20));
            tmpPoints.Add(new Point(190, 20));
            tmpPoints.Add(new Point(190, 200));

            EnevelopePoints = tmpPoints;
        }

        public override void Refresh(double TakeOffWeight, double TakeOffArm, double LandingWeight, double LandingArm)
        {
            if ((LandingArm > 44 && LandingArm < 66) && (LandingWeight > 2399 && LandingWeight <4201))
            {
                LandingCircleX = Convert.ToInt32((LandingArm - 45) / 10);
                LandingCircleY = Convert.ToInt32(200 - (LandingWeight - 2400) / 10);
            }

            if ((TakeOffArm > 44 && TakeOffArm < 66) && (TakeOffWeight > 2399 && TakeOffWeight < 4201))
            {
                int tmpTakeOffX = Convert.ToInt32((TakeOffArm - 45) / 10);
                int tmpTakeOffY = Convert.ToInt32(200 - (TakeOffWeight - 2400) / 10);
                var takeoffTriangle = new PointCollection();

                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX - 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX + 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX, tmpTakeOffY));


                TakeOffTriangle = takeoffTriangle;
            }

        }
    }
}
