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
    
    public partial class tbPartyGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbPartyGroup()
        {
            this.tbSectorBookings = new HashSet<tbSectorBooking>();
            this.tbGuests = new HashSet<tbGuest>();
        }
    
        public int pkPartyGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> fkBookingId { get; set; }
        public Nullable<bool> IsDefault { get; set; }
    
        public virtual tbBookingFile tbBookingFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSectorBooking> tbSectorBookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbGuest> tbGuests { get; set; }
    }
}
