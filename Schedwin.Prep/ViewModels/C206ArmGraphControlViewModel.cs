using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Schedwin.Prep
{
    public class C206ArmGraphControlViewModel : ArmGraphControlViewModelBase
    {
        //       <Polyline Points="12,180 32,120 103,9 128,9 44,180" StrokeThickness="1" Stroke="Blue"/>


        public C206ArmGraphControlViewModel()
        {

            LandingCircleX = 2;
            LandingCircleY = 170;
            var takeoffTriangle = new PointCollection();
            takeoffTriangle.Add(new System.Windows.Point(0, 175));
            takeoffTriangle.Add(new System.Windows.Point(5, 175));
            takeoffTriangle.Add(new System.Windows.Point(3, 170));
            TakeOffTriangle = takeoffTriangle;
            var tmpPoints = new PointCollection();
            tmpPoints.Add(new Point(62, 180));
            tmpPoints.Add(new Point(92, 120));
            tmpPoints.Add(new Point(153, 9));
            tmpPoints.Add(new Point(178, 9));
            tmpPoints.Add(new Point(94, 180));

            EnevelopePoints = tmpPoints;

        }


        public override void Refresh(double TakeOffWeight, double TakeOffArm, double LandingWeight, double LandingArm)
        {
            if ((LandingArm >-1  && LandingArm < 200) && (LandingWeight > 1899 && LandingWeight <3701)  )
            {
                LandingCircleX = Convert.ToInt32(LandingArm) - 2;
                LandingCircleY = Convert.ToInt32(((3700 - LandingWeight) / 1800) * 180) + 2;
            }

            if ((TakeOffArm > -1 && TakeOffArm < 200) && (TakeOffWeight > 1899 && TakeOffWeight < 3701))
            {
                int tmpTakeOffX = Convert.ToInt32(TakeOffArm);
                int tmpTakeOffY = Convert.ToInt32(((3700 - TakeOffWeight) / 1800) * 180);
                var takeoffTriangle = new PointCollection();

                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX - 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX + 3, tmpTakeOffY + 5));
                takeoffTriangle.Add(new System.Windows.Point(tmpTakeOffX, tmpTakeOffY));


                TakeOffTriangle = takeoffTriangle;
            }


        }
    }
}
