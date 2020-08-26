using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Schedwin.Common
{
    public  abstract class PrintedControl : UserControl 
    {
        public abstract int TotalPages();
        public abstract void Print();
        public abstract int CurrentPage();
        public abstract void NextPage();
        public abstract void PrevPage();
        public abstract void LastPage();
        public abstract void FirstPage();

    }
}
