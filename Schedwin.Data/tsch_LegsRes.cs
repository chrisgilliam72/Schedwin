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
    
    public partial class tsch_LegsRes
    {
        public int IDX { get; set; }
        public int IDX_Legs { get; set; }
        public int IDX_Bookings { get; set; }
        public int IDX_AC_Pilot { get; set; }
        public short NumPax { get; set; }
        public string CoCode { get; set; }
        public Nullable<double> Budget { get; set; }
        public string LOGUSERID { get; set; }
        public Nullable<System.DateTime> LOGDATETIMESTAMP { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual tsch_Legs tsch_Legs { get; set; }
        public virtual tsch_ReservationLegs tsch_ReservationLegs { get; set; }
        public virtual tsch_AC_Pilot tsch_AC_Pilot { get; set; }
    }
}