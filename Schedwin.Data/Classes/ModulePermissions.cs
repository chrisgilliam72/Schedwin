using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedwin.Data.Classes
{
    public class ModulePermissions
    {
        public int IDX { get; set; }

        public int IDX_User { get; set; }

        public String ModuleName { get; set; }

        public bool CanModify { get; set; }

        public bool CanView { get; set; }
    }
}
