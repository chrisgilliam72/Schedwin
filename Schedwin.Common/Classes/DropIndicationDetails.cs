using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Schedwin.Common.Classes
{
    public class DropIndicationDetails : ViewModelBase
    {
        private object currentDraggedItem;
        private DropPosition currentDropPosition;
        private object currentDraggedOverItem;

        public object CurrentDraggedOverItem
        {
            get
            {
                return currentDraggedOverItem;
            }
            set
            {
                if (this.currentDraggedOverItem != value)
                {
                    currentDraggedOverItem = value;
                    NotifyPropertyChanged("CurrentDraggedOverItem");
                }
            }
        }

        public int DropIndex { get; set; }

        public DropPosition CurrentDropPosition
        {
            get
            {
                return this.currentDropPosition;
            }
            set
            {
                if (this.currentDropPosition != value)
                {
                    this.currentDropPosition = value;
                    NotifyPropertyChanged("CurrentDropPosition");
                }
            }
        }

        public object CurrentDraggedItem
        {
            get
            {
                return this.currentDraggedItem;
            }
            set
            {
                if (this.currentDraggedItem != value)
                {
                    this.currentDraggedItem = value;
                    NotifyPropertyChanged("CurrentDraggedItem");
                }
            }
        }
    }
}
