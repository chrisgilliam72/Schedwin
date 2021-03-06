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
    
    public partial class tbPrincipal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbPrincipal()
        {
            this.tbSefLegs = new HashSet<tbSefLeg>();
            this.tbSectorBookings = new HashSet<tbSectorBooking>();
            this.tbSefBookings = new HashSet<tbSefBooking>();
            this.tbSefBookings1 = new HashSet<tbSefBooking>();
            this.tbSefFlights = new HashSet<tbSefFlight>();
            this.tbCharterBookings = new HashSet<tbCharterBooking>();
            this.tbCharterBookings1 = new HashSet<tbCharterBooking>();
        }
    
        public int pkPrincipalId { get; set; }
        public string fkPrincipalCode { get; set; }
        public string fkLocationCode { get; set; }
        public Nullable<bool> Active { get; set; }
        public string fkPrincipalName { get; set; }
        public Nullable<bool> UsedForWishCharters { get; set; }
        public Nullable<int> FK_PrincipalTypeID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSefLeg> tbSefLegs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSectorBooking> tbSectorBookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSefBooking> tbSefBookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSefBooking> tbSefBookings1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSefFlight> tbSefFlights { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbCharterBooking> tbCharterBookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbCharterBooking> tbCharterBookings1 { get; set; }
    }
}
