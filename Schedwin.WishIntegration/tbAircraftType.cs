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
    
    public partial class tbAircraftType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbAircraftType()
        {
            this.tbSectorBookings = new HashSet<tbSectorBooking>();
        }
    
        public int pkAircraftTypeId { get; set; }
        public string Type { get; set; }
        public Nullable<bool> Active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSectorBooking> tbSectorBookings { get; set; }
    }
}
