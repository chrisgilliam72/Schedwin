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
    
    public partial class tbLodge
    {
        public int pkLodgeID { get; set; }
        public int fkCountryID { get; set; }
        public int fkOperatorID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> CheckInTime { get; set; }
        public Nullable<System.DateTime> CheckOutTime { get; set; }
        public int Number_of_Beds { get; set; }
        public string Tel_Number { get; set; }
        public string Manager { get; set; }
        public string Email { get; set; }
        public string TPCode { get; set; }
        public bool Active { get; set; }
        public string tmpAirstripIATA { get; set; }
        public Nullable<int> fkAirstripID { get; set; }
    
        public virtual tbAirstrip tbAirstrip { get; set; }
    }
}
