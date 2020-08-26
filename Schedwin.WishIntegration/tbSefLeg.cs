//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Schedwin.WishIntegration
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbSefLeg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbSefLeg()
        {
            this.tbSefLegSchedules = new HashSet<tbSefLegSchedule>();
            this.tbSefFlightLegs = new HashSet<tbSefFlightLeg>();
        }
    
        public int pkSefLegID { get; set; }
        public int fkLocationID { get; set; }
        public string Description { get; set; }
        public System.DateTime StartDateTime { get; set; }
        public System.DateTime EndDateTime { get; set; }
        public int fkDepartureAirstripID { get; set; }
        public int fkArrivalAirstripID { get; set; }
        public int fkPrincipalId { get; set; }
        public string Notes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSefLegSchedule> tbSefLegSchedules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSefFlightLeg> tbSefFlightLegs { get; set; }
        public virtual tbAirstrip tbAirstrip { get; set; }
        public virtual tbAirstrip tbAirstrip1 { get; set; }
        public virtual tbPrincipal tbPrincipal { get; set; }
    }
}
