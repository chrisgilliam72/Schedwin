using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schedwin.Tourplan;
using System.Collections.ObjectModel;
using Schedwin.Common;
using Schedwin.Data.Classes;

namespace Schedwin.Setup
{
    public class ChooseTPCodeViewModel :ViewModelBase
    {
        public String TPDBName { get; set; }
        public String StatusText { get; set; }
        public String Region { get; set; }
        public CRMCode SelectedCRMCode { get; set; }
        public RangeObservableCollection<CRMCode> CodeList { get; set; }

        private String _PartialName { get; set; }
        public String PartialName
        {
            get
            {
                return _PartialName;
            }
            set
            {
                _PartialName = value;
                NotifyPropertyChanged("PartialName");
            }
        }

        public ChooseTPCodeViewModel()
        {
            CodeList = new RangeObservableCollection<CRMCode>();
            TPDBName = "TourplanIS_PAF";
        }

        public async Task Init()
        {
            var dbList = await TPDBRegion.LoadTPDBRegions(Server, Database);
            var tpDB = dbList.FirstOrDefault(x => x.Region == Region);
             if (tpDB!=null)
            {
                TPDBName = tpDB.DBName.TrimEnd(' ') ;
                StatusText = "Tourplan DB: " + tpDB.DBName;
                NotifyPropertyChanged("StatusText");
            }
           
        }

        public async Task<bool>  RefreshTPLIst()
        {
            if (PartialName!=null && PartialName.Length>2)
            {
                CodeList.Clear();
                var crmList = await CRMCode.GetCRMList(PartialName, @"stratus", TPDBName);
                CodeList.AddRange(crmList);
                NotifyPropertyChanged("CodeList");

            }
           
            return true;
        }
    }
}
