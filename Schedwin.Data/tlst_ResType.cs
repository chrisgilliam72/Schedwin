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
    
    public partial class tlst_ResType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tlst_ResType()
        {
            this.tsch_ReservationHeader = new HashSet<tsch_ReservationHeader>();
        }
    
        public int IDX { get; set; }
        public string ResType { get; set; }
        public string LOGUSERID { get; set; }
        public Nullable<System.DateTime> LOGDATETIMESTAMP { get; set; }
        public System.Guid rowguid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tsch_ReservationHeader> tsch_ReservationHeader { get; set; }
    }
}
