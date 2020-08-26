using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class ExpiryRatingGridItem : ViewModelBase
    {
       public bool RatingNotApplicable { get; set; }


        public String Name { get; set; }

        private bool _rating;
        public bool Rating
        {
            get
            {
                return _rating; ;
            }
            set
            {
                _rating = value;
                NotifyPropertyChanged("DateReadOnly");
                NotifyPropertyChanged("Rating");
            }
        }

        public bool DateReadOnly
        {
            get
            {
                return !_rating;
            }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get
            {
                if (Rating)
                    return _date;
                else
                    return null;
            }
            set
            {
                _date = value;
                NotifyPropertyChanged("Date");
            }
        }

        public ExpiryRatingGridItem()
        {
            Rating = false;
            Date = null;
            RatingNotApplicable = false;
        }
    }
}
