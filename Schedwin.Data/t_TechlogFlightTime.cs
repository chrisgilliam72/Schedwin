//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Schedwin.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class t_TechlogFlightTime
    {
        public int TechlogID { get; set; }
        public int IDXACDetails { get; set; }
        public int IDXLeg { get; set; }
        public int IDXACPilot { get; set; }
        public System.DateTime BlockOff { get; set; }
        public System.DateTime BlockOn { get; set; }
        public string LOGUSERID { get; set; }
        public Nullable<System.DateTime> LOGDATETIMESTAMP { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual tsch_Legs tsch_Legs { get; set; }
        public virtual tsch_AC_Pilot tsch_AC_Pilot { get; set; }
    }
}
