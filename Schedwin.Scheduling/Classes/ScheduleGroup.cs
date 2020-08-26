using Schedwin.Common;
using Schedwin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Schedwin.Scheduling.Classes
{
    public class ScheduleGroup : ViewModelBase
    {

        public bool Selected { get; set; }
       public Brush RowColor
        {
            get
            {
                if (IsCancelled)
                    return Brushes.Red;
                switch (Status)
                {
                    case "OK": return Brushes.LightGreen;
                    case "Fault": return Brushes.Orange;
                    case "Dep OK": return new SolidColorBrush(Color.FromRgb(219, 183, 255));
                    case "Dest OK": return new SolidColorBrush(Color.FromRgb(152, 182, 242));
                    default: return null;
                }
                
            }
        }


        private String _status;
        public String Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                NotifyPropertyChanged("RowColor");
                NotifyPropertyChanged("Status");
            }
        }

        public bool SLC { get; set; }
        public String FromAP { get; set; }

        public int IDX_FromAP { get; set; }

        public String ToAP { get; set; }

        public int IDX_ToAP { get; set; }

        public int NumPax { get; set; }

        public String ReservationName { get; set; }

        public String Ex { get; set; }

        public String For { get; set; }

        public String Operator { get; set; }

        public int IDX_RH { get; set; }

        public int IDX_RL { get; set; }

        public int PaxWeight { get; set; }

        public int LuggageWeight { get; set; }

        public bool SoleUse { get; set; }

        public String ACType { get; set; }

        public int IDX_ACType { get; set; }

        public DateTime EarlyEx { get; set; }

        public DateTime LatestFor { get; set; }


        public bool IsCancelled { get; set; }

        public String ResType { get; set; }

        public String Notes { get; set; }


        public int? ParentIDX { get; set; }

        public int? ChildIDX { get; set; }

        public String Name
        {
            get
            {
                return ReservationName;
            }
        }
        public String Routing { get; set; }


        public ScheduleGroup()
        {
            Status = "Not Sched";

        }

        public async Task UpdateExFor(String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();

            using (ctx)
            {
                var tschLeg = await ctx.tsch_ReservationLegs.FirstOrDefaultAsync(x => x.IDX == IDX_RL);
                if (tschLeg != null)
                {
                    tschLeg.EarliestEx = EarlyEx;
                    tschLeg.LatestFor = LatestFor;
                    await ctx.SaveChangesAsync();
                }
                    
                
            }
        }

        public void SetScheduledStatus(List<ScheduleLeg> scheduleLegs)
        {
            Routing = "";
            if (scheduleLegs != null && scheduleLegs.Count > 0)
            {
                foreach (var schedLeg in scheduleLegs)
                {
                    
                    if (schedLeg != scheduleLegs.Last())
                        Routing += schedLeg.Description + Environment.NewLine;
                    else
                        Routing += schedLeg.Description;
                }

                if (scheduleLegs.Count == 1)
                {
                    if (scheduleLegs[0].IDX_FromAP == IDX_FromAP && scheduleLegs[0].IDX_ToAP == IDX_ToAP)
                        Status = "OK";
                    else
                    if (scheduleLegs[0].IDX_FromAP == IDX_FromAP && scheduleLegs[0].IDX_ToAP != IDX_ToAP)
                        Status = "Dep OK";
                    else
                    if (scheduleLegs[0].IDX_FromAP != IDX_FromAP && scheduleLegs[0].IDX_ToAP == IDX_ToAP)
                        Status = "Dest OK";
                    else
                        Status = "Fault";

                }
                else
                {
                    var sortedLegs = scheduleLegs.OrderBy(x => x.ETD).ToList();
                    int legCount = sortedLegs.Count;

                    foreach (var schedLeg in scheduleLegs)
                    {
                        var currentIndex = sortedLegs.IndexOf(schedLeg);
                        if (currentIndex< legCount-1)
                        {
                            var nextLeg = sortedLegs[currentIndex + 1];
                            if (schedLeg.IDX_ToAP!=nextLeg.IDX_FromAP)

                            {
                                Status = "Fault";
                                return;
                            }
                        }
                       
                    }

                    

                    if (sortedLegs[0].IDX_FromAP == IDX_FromAP && sortedLegs[legCount - 1].IDX_ToAP == IDX_ToAP)
                        Status = "OK";
                    else
                    if (sortedLegs[0].IDX_FromAP == IDX_FromAP && sortedLegs[legCount - 1].IDX_ToAP != IDX_ToAP)
                        Status = "Dep OK";
                    else
                    if (sortedLegs[0].IDX_FromAP != IDX_FromAP && sortedLegs[legCount - 1].IDX_ToAP == IDX_ToAP)
                        Status = "Dest OK";
                    else
                        Status = "Fault";
                }
            }
            else
                Status = "Not Sched";

        }

      

        public static explicit operator ScheduleGroup(sl_ScheduleReservations_Result1 slScheduleReservation)
        {
            var newGroup = new ScheduleGroup();
            newGroup.FromAP = slScheduleReservation.FromAP;
            newGroup.IDX_FromAP = slScheduleReservation.IDX_FromAP ?? -1;
            newGroup.ToAP = slScheduleReservation.ToAP;
            newGroup.IDX_ToAP = slScheduleReservation.IDX_ToAP ?? -1;
            newGroup.NumPax = slScheduleReservation.NumPax ?? 0;
            newGroup.ReservationName = slScheduleReservation.ReservationName;
            newGroup.Ex = slScheduleReservation.ExField;
            newGroup.For = slScheduleReservation.ForField;
            newGroup.Operator = slScheduleReservation.Operator;
            newGroup.IDX_RH = slScheduleReservation.IDX_RH ?? -1;
            newGroup.IDX_RL = slScheduleReservation.IDX_RL ?? -1;
            newGroup.PaxWeight = slScheduleReservation.PassengerWeight ?? -1;
            newGroup.LuggageWeight = slScheduleReservation.LuggageWeight ?? -1;
            newGroup.SoleUse = slScheduleReservation.SoleUse ?? false;
            newGroup.ACType = slScheduleReservation.AcType;
            newGroup.IDX_ACType = slScheduleReservation.IDX_ACType;
            newGroup.EarlyEx = slScheduleReservation.EarlyEx.Value;
            newGroup.LatestFor = slScheduleReservation.LateFor.Value;
            newGroup.IsCancelled = slScheduleReservation.Cancelled.Value;
            newGroup.Notes = slScheduleReservation.Notes;
            newGroup.ResType = slScheduleReservation.ResType;
            return newGroup;

        }

   
        public  static  async Task<List<tsch_SplitReservations>> GetSplitBookingsList(List<int> ReservationIDs, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();
            using (ctx)
            {
               var splitResList= await  ctx.tsch_SplitReservations.Where(x => ReservationIDs.Contains(x.fkChildReservation.Value) || ReservationIDs.Contains(x.fkParentReservation.Value)).ToListAsync();
                return splitResList;
            }
        }

        public static List<ScheduleGroup> GetScheduleReservations(DateTime flightDate, String Server, string regionalDBName)
        {
            var constring = RegionalConnectionGenerator.GetConnectionString(Server, regionalDBName);
            var ctx = new SchedwinRegionalEntities(constring);
            var schedule = new Schedule();

            using (ctx)
            {
                var results = ctx.sl_ScheduleReservations(flightDate).ToList();
                var lstGroups = results.Select(x => (ScheduleGroup)x).ToList();
                return lstGroups;
            }

          
        }
    }
}
