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
    
    public partial class tbSefBooking
    {
        public int pkSefBookingId { get; set; }
        public int fkSefFlightId { get; set; }
        public int fkSectorBookingId { get; set; }
        public Nullable<int> ExPrincipalId { get; set; }
        public Nullable<int> ForPrincipalId { get; set; }
    
        public virtual tbSectorBooking tbSectorBooking { get; set; }
        public virtual tbSefBooking tbSefBookings1 { get; set; }
        public virtual tbSefBooking tbSefBooking1 { get; set; }
        public virtual tbPrincipal tbPrincipal { get; set; }
        public virtual tbPrincipal tbPrincipal1 { get; set; }
        public virtual tbSefFlight tbSefFlight { get; set; }
    }
}
