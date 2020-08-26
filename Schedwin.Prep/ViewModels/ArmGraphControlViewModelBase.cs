using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.Prep
{
    public class ArmGraphControlViewModelBase : ViewModelBase
    {
        private PointCollection _envelopePoints;
        public PointCollection EnevelopePoints
        {
            get
            {
                return _envelopePoints;
            }
            set
            {
                _envelopePoints = value;
                NotifyPropertyChanged("EnevelopePoints");
            }
        }

        private PointCollection _takeoffTriangle;
        public PointCollection TakeOffTriangle
        {
            get
            {
                return _takeoffTriangle;
            }
            set
            {
                _takeoffTriangle = value;
                NotifyPropertyChanged("TakeOffTriangle");
            }

        }

        private int _landingCircleX;
        public int LandingCircleX
        {
            get
            {
                return _landingCircleX;
            }
            set
            {
                _landingCircleX = value;
                NotifyPropertyChanged("LandingCircleX");
            }
        }
        private int _landingCircleY;
        public int LandingCircleY
        {
            get
            {
                return _landingCircleY;
            }
            set
            {
                _landingCircleY = value;
                NotifyPropertyChanged("LandingCircleY");
            }
        }

        public virtual void Refresh(double TakeOffWeight, double TakeOffArm, double LandingWeight, double LandingArm)
        {

        }
    }
}
