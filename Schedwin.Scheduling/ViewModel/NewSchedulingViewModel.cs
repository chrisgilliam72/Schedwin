using Schedwin.Common;
using Schedwin.Data.Classes;
using Schedwin.Scheduling.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Scheduling
{
    public class NewSchedulingViewModel :ViewModelBase
    {
        private bool _canUnlock;
        public bool CanUnlock
        {
            get
            {
                return _canUnlock;
            }
            set
            {
                _canUnlock = value;
                NotifyPropertyChanged("CanUnlock");
            }
        }

        public DateTime ScheduleDate { get; set; }

        public String CurrentUser { get; set; }

        public SchedulingPilotListViewModel PilotsVM { get; set; }
        
        public SchedulingLegsListViewModel LegsVM { get; set; }
        public SchedulingGrpsViewModel GroupsVM { get; set; }

        public ScheduleTotalViewModel TotalsVM { get; set; }

        private String _openedby;
        public String OpenedBy
        {
            get
            {
                return _openedby;
            }
            set
            {
                _openedby = value;
                NotifyPropertyChanged("OpenedBy");
            }
        }



        private String _revisionInfo;
        public String RevisionInfo
        {
            get
            {
                return _revisionInfo;
            }
            set
            {
                _revisionInfo = value;
                NotifyPropertyChanged("RevisionInfo");
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
                NotifyPropertyChanged("Status");
            }
        }

        public  NewSchedulingViewModel()
        {
            ScheduleDate = DateTime.Today;
            //Server = @"vulture\sql02";
            //DatabaseName = "Sefofane_Bots";

        }

        public  async Task<bool> Init()
        {
           try
            {
                Status = "Initializing...";

                PilotsVM.Server = Server;
                PilotsVM.Database = Database;
                GroupsVM.SchedulingViewModel = this;
                LegsVM.SchedulingViewModel = this;

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await PilotType.LoadPilotTypes(Server, Database);
                    await AircraftType.LoadACTypes(Server, Database,false);
                    await FuelType.LoadFuelTypes(Server, Database);
                    await ACAirportLimits.LoadACAirportLimits(Server, Database);
                    await AirportFuel.LoadFuelList(Server, Database);
                    await Company.LoadCompanyList(Server, Database, false);
                    PilotsVM.Init();
                    LegsVM.Init();
                }

                Status = "";
                return true;
            }
            catch (Exception ex )
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to load Schedule:" + Environment.NewLine + exMessage);

                Status = "Initialization error";
                return false;
            }
        }

        public async Task<bool> LoadSchedule()
        {
            try
            {
                Schedule schedule = null;
                List<ScheduleGroup> grps = null;
                RevisionInfo = "";
                OpenedBy = "";

                GroupsVM.Clear();
                PilotsVM.Clear();
                LegsVM.Clear();
                TotalsVM.Clear();

                Status = "loading schedule data...";

                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    schedule = await Schedule.LoadScheduleAsync(ScheduleDate, Server, Database);
                }

                Status = "loading roster....";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var rosterLst=await PilotRoster.GetDailyRoster(ScheduleDate, Server, Database);
                    PilotsVM.DutyTypes.AddRange(rosterLst);
                }

                Status = "loading reservations....";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {

                    await Task.Run(() =>
                    {
                        grps = ScheduleGroup.GetScheduleReservations(ScheduleDate, Server, Database);
                    });
                }

     

                Status = "loading pilot flight hours...";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    var pilotIDLst = schedule.list_ACPilots.Where(x=>x.IDX_Pilot_1.HasValue).Select(x => x.IDX_Pilot_1.Value).ToList();
                    var pilotID2Lst = schedule.list_ACPilots.Where(x=>x.IDX_Pilot_2.HasValue).Select(x => x.IDX_Pilot_2.Value).ToList();


                    var lstPilot1 = await ScheduleDutyRoster.GetPilotEFTs(ScheduleDate, pilotIDLst,true, Server, Database);
                    var lstPilot2 = await ScheduleDutyRoster.GetPilotEFTs(ScheduleDate, pilotID2Lst, false, Server, Database);
                    foreach (var pilot in schedule.list_ACPilots)
                    {
                        var eftItem = lstPilot1.FirstOrDefault(x => x.Item1 == pilot.IDX_Pilot_1);
                        pilot.Pilot1EstFT = eftItem != null ? eftItem.Item2 : 0;
                        if (pilot.IDX_Pilot_2!=null)
                        {
                            eftItem = lstPilot2.FirstOrDefault(x => x.Item1 == pilot.IDX_Pilot_2);
                            pilot.Pilot2EstFT = eftItem != null ? eftItem.Item2 : 0;
                        }
                      
                        foreach (var schedLeg in pilot.Legs)
                        {
                            var acInfo = AircraftInfo.GetAircraftRegistration(pilot.IDX_Aircraft);
                            if (acInfo != null)
                                schedLeg.ACRegistration = acInfo;
                        }

                    }
                }


                foreach (var acPilot in schedule.list_ACPilots)
                {
                    var Registration = AircraftInfo.GetAircraftRegistration(acPilot.IDX_Aircraft);
                    Status = "load last AP for " + Registration;
                    using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                    {
                       
                        acPilot.IDX_AircraftAP = await Schedule.GetAircraftLastAP(ScheduleDate, acPilot.IDX_Aircraft, Server, Database);
                        if (acPilot.IDX_AircraftAP<1)
                        {
                            var aircraftInfo = AircraftInfo.GetAircraftInfo(acPilot.IDX_Aircraft);
                            if (aircraftInfo!=null)
                            {
                                var owner = Company.GetCompany(aircraftInfo.IDX_Owner);
                                if (owner != null)
                                {
                                    var baseAP = owner.IDX_BaseAP;
                                    var lastAP = AirstripInfo.GetAirstripInfo(baseAP.Value);
                                    acPilot.IDX_AircraftAP = lastAP != null ? lastAP.IDX : 0;

                                }
                            }

                        }
  

                    }
                }

                Status = "loading service information...";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await schedule.UpdateACScheduleServices(ScheduleDate, Server, Database);
                }

                Status = "";

                PilotsVM.ScheduleDate = ScheduleDate;
                Status = "loading pilot data...";
                PilotsVM.Refresh(schedule.list_ACPilots);
                Status = "";

                foreach (var grp in grps)
                {
                    var resLegs = schedule.GetReservationLegs(grp.IDX_RL);
                    grp.SetScheduledStatus(resLegs);
                }

                Status = "loading groups...";
                await GroupsVM.Refresh(ScheduleDate,grps);
                Status = "";

                Status = "loading locking info";
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {

                    var userDetails = await Schedule.GetScheduleUser(ScheduleDate, Server, Database);
                    if (String.IsNullOrEmpty(userDetails))
                    {
                        OpenedBy = "";
                        LegsVM.ReadOnly = false;
                        PilotsVM.ReadOnly = false;

                        await schedule.Lock(CurrentUser, Server, Database);
                        CanUnlock = true;
                    }
                        
                    else
                    {
                        if (CurrentUser!=userDetails)
                        {
                            LegsVM.ReadOnly = true;
                            PilotsVM.ReadOnly = true;
                            OpenedBy = "In used by : " + userDetails;
                            CanUnlock = false;
                        }
                        else
                        {
                            LegsVM.ReadOnly = false;
                            PilotsVM.ReadOnly = false;
                            CanUnlock = true;

                        }
                    }
                      
                }


                TotalsVM.Recalculate(PilotsVM.ScheduledPilots.ToList());

                Status = "";

                RevisionInfo = ScheduleDate.DayOfWeek + " Revision " + schedule.GetScheduleRevision();


                return true;
            }
            catch(Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to load Schedule:" + Environment.NewLine + exMessage);
                Status = "Schedule load error.";
                return false;
            }
        }

        public async Task Unlock()
        {
            try
            {
                using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
                {
                    await Schedule.Unlock(CurrentUser, ScheduleDate, Server, Database);
                }
                LegsVM.ReadOnly = true;
                PilotsVM.ReadOnly = true;
                CanUnlock = false;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to unlock Schedule:" + Environment.NewLine + exMessage);
            }
        }


        public Schedule BuildSchedule()
        {
            var schedule = new Schedule();
            schedule.FlightDate = ScheduleDate;
            schedule.list_ACPilots = this.PilotsVM.ScheduledPilots.ToList();
            return schedule;
        }

        public void UpdateGroupStatus(ScheduleGroup bookedGrp)
        {
            if (bookedGrp!=null)
            {
                var schedule = BuildSchedule();
                var resLegs= schedule.GetReservationLegs(bookedGrp.IDX_RL);
                bookedGrp.SetScheduledStatus(resLegs);
                GroupsVM.UpdateGroupList();
            }

        }

        public void UpdateGroupStatus(int idxResLeg)
        {
            var group = GroupsVM.Groups.FirstOrDefault(x => x.IDX_RL == idxResLeg);
            UpdateGroupStatus(group);
        }

        public void UpdateAllGroupsStatus()
        {
            var schedule = BuildSchedule();
            GroupsVM.ReEvaluateAllGroupsStatus(schedule);
        }

        public async void RefreshGroups()
        {
            var schedule = BuildSchedule();
            List<ScheduleGroup> grps = null;

            Status = "loading reservations....";
            using (new StackedCursorOverride(System.Windows.Input.Cursors.Wait))
            {

                await Task.Run(() =>
                {
                    grps = ScheduleGroup.GetScheduleReservations(ScheduleDate, Server, Database);
                });
            }

            foreach (var grp in grps)
            {
                var resLegs = schedule.GetReservationLegs(grp.IDX_RL);
                grp.SetScheduledStatus(resLegs);
            }
            await GroupsVM.Refresh(ScheduleDate, grps);

            Status = "";
        }

        public async Task<bool> Save()
        {

           try
            {
                var schedule = new Schedule();
                schedule.FlightDate = ScheduleDate;
                schedule.list_ACPilots = this.PilotsVM.ScheduledPilots.ToList();
                schedule.list_ACPilots.AddRange(PilotsVM.DeletedPilots);
               Status = "Saving...";
                var result = await schedule.Save(Server, Database);
                Status = " ";
                return result;
            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display("Failed to save Schedule:" + Environment.NewLine + exMessage);
                Status = "Schedule save error.";
                return false;
            }
        }

    }



}
