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
    
    public partial class tset_ACDetailTypeStations
    {
        public int IDX { get; set; }
        public int IDX_ACTypes { get; set; }
        public int IDX_ACDetails { get; set; }
        public int IDX_ACTypeLoadingArrangement { get; set; }
        public short StationNumber { get; set; }
        public string StationName { get; set; }
        public float StationArm { get; set; }
        public string StationMaxWeight { get; set; }
        public int StationType { get; set; }
        public byte MaxNumSeats { get; set; }
        public string LOGUSERID { get; set; }
        public Nullable<System.DateTime> LOGDATETIMESTAMP { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual tset_ACDetails tset_ACDetails { get; set; }
        public virtual tset_ACTypeLoadingArrangement tset_ACTypeLoadingArrangement { get; set; }
        public virtual tset_ACTypes tset_ACTypes { get; set; }
        public virtual tlst_StationType tlst_StationType { get; set; }
    }
}
