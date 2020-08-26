using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedwinWPF
{
    interface IMainWindowManager
    {
        void AddWindow(Form window);
        void RemoveWindow(Form Window);
        void TileWindows();
        void CascadeWindows();
        void CloseAll();
    }
}
