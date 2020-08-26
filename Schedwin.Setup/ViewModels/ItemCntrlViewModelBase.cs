using Schedwin.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Setup
{
    public class ItemCntrlViewModelBase : ViewModelBase
    {
        public NewSetupViewModel MainViewModel { get; set; }
        public int IDX { get; set; }
        public int ParentIDX { get; set; }

        public bool IsNew { get; set; }

        private bool _isvisible;
        public bool IsVisible
        {
            get
            {
                return _isvisible;
            }
            set
            {
                _isvisible = value;
                NotifyPropertyChanged("IsVisible");
            }
        }

        public ItemCntrlViewModelBase()
        {
            IsNew = false;
        }

        public virtual void Init()
        {
           
        }

        public virtual bool Validate()
        {
            return true;
        }

        public virtual Task<bool> Save()
        {
            return null;
        }
  
        public void UpdateStatusText(String newStatus)
        {
            if (MainViewModel != null)
                MainViewModel.StatusText = newStatus;
        }
    }
}
