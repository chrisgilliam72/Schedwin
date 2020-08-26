using Schedwin.Common;
using Schedwin.Data.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Schedwin.Techlogs
{
    public class TechLogServiceHistoryViewModel :ViewModelBase
    {
        private TechLogService _SelectedHistory;
        public TechLogService SelectedHistory
        {
            get
            {
                return _SelectedHistory;
            }
            set
            {
                _SelectedHistory = value;
            }


        }

        public DateTime Date { get; set; }
        public int IDX_Aircraft { get; set; }
    


        public RangeObservableCollection<Techlog> ListTechlogs { get; set; }

        public RangeObservableCollection<TechLogService> ListTechlogServiceHistory { get; set; }

        public TechLogServiceHistoryViewModel()
        {
            ListTechlogServiceHistory = new RangeObservableCollection<TechLogService>();
        }

        public void Refresh(List<TechLogService> listHistory)
        {
            ListTechlogServiceHistory.Clear();
            ListTechlogServiceHistory.AddRange(listHistory.OrderByDescending(x=>x.Date));

            NotifyPropertyChanged("ListTechlogServiceHistory");

        }

        public void NewEntry()
        {
            var serviceEntry = new TechLogService();
            serviceEntry.IsNew = true;
            serviceEntry.IsModified = true;
            serviceEntry.Date = DateTime.Today;
            serviceEntry.IDX_Setup_Aircraft_Details = IDX_Aircraft;
            ListTechlogServiceHistory.Insert(0, serviceEntry);
            NotifyPropertyChanged("ListTechlogServiceHistory");
        }

        public void DeleteEntry()
        {
            Telerik.Windows.Controls.DialogParameters parameters = new Telerik.Windows.Controls.DialogParameters();
            parameters.Header = "Delete Tech log service entry";
            parameters.Content = "Are you sure want to delete this Entry ?";
            parameters.OkButtonContent = "Yes";
            parameters.CancelButtonContent = "No";
            parameters.Closed = OnDeleteConfirmed;
            Telerik.Windows.Controls.RadWindow.Confirm(parameters);
        }

        public  void SaveEntry(TechLogService techlogEntry)
        {
            if (techlogEntry != null)
                techlogEntry.IsModified = true; ;
        }

        public async Task UpdateTechlogID(TechLogService serviceItem, DateTime newDate)
        {
            Techlog lastTechLog = null;
            Techlog lastTechLogService = null;
            List<Techlog> listTechLogs = null;
            TechLogService lastTechlogSericeHist = null;
            double tachhours=0.0;
            var techlogIDList = new List<int>();
   
            using (new StackedCursorOverride(Cursors.Wait))
            {
                listTechLogs = await Techlog.GetTechLogs(newDate, newDate, IDX_Aircraft, Server, Database);
            }

            if (listTechLogs == null || listTechLogs.Count() == 0)
                return;

            lastTechLog = listTechLogs.OrderByDescending(x => x.TechLogID).FirstOrDefault();

            var lastTechLOGID = ListTechlogServiceHistory.Where(x => x.Date < newDate).OrderByDescending(x => x.Date).First().IDX_TechLogID;
            if (lastTechLOGID!=0)
            {
                lastTechlogSericeHist = ListTechlogServiceHistory.FirstOrDefault(x => x.IDX_TechLogID == lastTechLOGID);
                techlogIDList.Add(lastTechLOGID);

                using (new StackedCursorOverride(Cursors.Wait))
                {
                    var techlogList = await Techlog.GetTechLogs(techlogIDList, IDX_Aircraft, Server, Database);
                    lastTechLogService = techlogList.First();
                }

                tachhours = lastTechLog.TachEnd - lastTechLogService.TachEnd;
                serviceItem.IDX_TechLogID = lastTechLog.TechLogID;

               
            }

            serviceItem.Update(lastTechlogSericeHist, tachhours);
        }

        public async Task<bool> Save()
        {
            try
            {
                using (new StackedCursorOverride(Cursors.Wait))
                {
                    await TechLogService.Save(ListTechlogServiceHistory.ToList(), Server, Database);
                }
                return true;
            }
            catch (DbEntityValidationException entEx)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var eve in entEx.EntityValidationErrors)
                {
                    sb.AppendLine(string.Format("\"{0}\"  has the following validation errors:",
                                                    eve.Entry.Entity.GetType().Name,
                                                    eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                                    ve.PropertyName,
                                                    ve.ErrorMessage));
                    }
                }
                FailWindow.Display(sb.ToString());
                return false;

            }
            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
                return false;
            }
        }


        private async void OnDeleteConfirmed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult.HasValue && e.DialogResult == true)
                {
                    if (SelectedHistory != null)
                    {
                        using (new StackedCursorOverride(Cursors.Wait))
                        {
                            await TechLogService.DeleteHistoryEntry(SelectedHistory.IDX, Server, Database);
                            ListTechlogServiceHistory.Remove(SelectedHistory);
                            NotifyPropertyChanged("ListTechlogServiceHistory");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                var messages = ex.FromHierarchy(x => x.InnerException).Select(x => x.Message);
                var exMessage = string.Join(Environment.NewLine, messages);
                FailWindow.Display(exMessage);
            }

        }


    }
}
