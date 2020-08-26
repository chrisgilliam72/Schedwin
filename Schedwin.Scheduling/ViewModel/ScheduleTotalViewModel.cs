using Schedwin.Common;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling
 {
    public class ScheduleTotalViewModel : ViewModelBase
    {

        private RangeObservableCollection<ScheduleTotalItem> _items;
        public RangeObservableCollection<ScheduleTotalItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }


        public ScheduleTotalViewModel()
        {
            Items = new RangeObservableCollection<ScheduleTotalItem>();
        }
        

        public void Clear()
        {
            Items.Clear();
            NotifyPropertyChanged("Items");
        }

        public void Recalculate(ScheduleACPilot acPilot)
        {
            var legCount = acPilot.Legs.Count();

            if (legCount > 0)
            {
                var item = Items.FirstOrDefault(x => x.Description == "Total KM");
                if (item!=null)
                    item.ThisAircraft = acPilot.Legs.Sum(x => x.Distance);

                item = Items.FirstOrDefault(x => x.Description == "Pax KM");
                if (item != null)
                    item.ThisAircraft = acPilot.Legs.Sum(x => x.Distance) * acPilot.Legs.Sum(x => x.NumPax);

                item = Items.FirstOrDefault(x => x.Description == "Load Factor");
                if (item != null)
                    item.ThisAircraft = Math.Round((double)acPilot.Legs.Sum(x => x.NumPax) / (double)(acPilot.Pax * legCount), 2);

                item = Items.FirstOrDefault(x => x.Description == "Flight Time");
                if (item != null)
                    item.ThisAircraft = acPilot.Legs.Sum(x => x.LegFlightTime);
            }

            else
            {
                if (Items.Count >0)
                {
                    var item = Items.FirstOrDefault(x => x.Description == "Total KM");
                    item.ThisAircraft = 0;
                    item = Items.FirstOrDefault(x => x.Description == "Pax KM");
                    item.ThisAircraft = 0;
                    item = Items.FirstOrDefault(x => x.Description == "Load Factor");
                    item.ThisAircraft = 0;
                    item = Items.FirstOrDefault(x => x.Description == "Flight Time");
                    item.ThisAircraft = 0;
                }

            }
        }

        public void Recalculate(List<ScheduleACPilot> lstPilots)
        {
            Items.Clear();

            
            var itemTotalKM = new ScheduleTotalItem();
            itemTotalKM.Description = "Total KM";         
            Items.Add(itemTotalKM);


            var itemPaxKM = new ScheduleTotalItem();
            itemPaxKM.Description = "Pax KM";
            Items.Add(itemPaxKM);


            var itemLoadFactor = new ScheduleTotalItem();
            itemLoadFactor.Description = "Load Factor";
            Items.Add(itemLoadFactor);

            var totalseats = 0;
          
            int totalPax = 0;

            foreach (var pilot in lstPilots)
            {
                int totalLegDistance = pilot.Legs.Sum(x => x.Distance);
                itemTotalKM.AllAircraft += totalLegDistance;
                int paxKM = pilot.Legs.Sum(x => x.NumPax) * totalLegDistance;
                itemPaxKM.AllAircraft += paxKM;

                totalseats += pilot.Pax * pilot.Legs.Count();
                totalPax += pilot.Legs.Sum(x => x.NumPax);

            }

            var loadFactor = (double)totalPax / (double)(totalseats);
            itemLoadFactor.AllAircraft = Math.Round(loadFactor, 2);




            var ItemflightTime = new ScheduleTotalItem();
            ItemflightTime.Description = "Flight Time";
            Items.Add(ItemflightTime);



            NotifyPropertyChanged("Items");
        }

    }
}
