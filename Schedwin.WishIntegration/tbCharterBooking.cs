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
    
    public partial class tbCharterBooking
    {
        public int CharterBookingID { get; set; }
        public int FK_SectorBookingID { get; set; }
        public int FK_ExPrincipalID { get; set; }
        public int FK_ForPrincipalID { get; set; }
    
        public virtual tbPrincipal tbPrincipal { get; set; }
        public virtual tbSectorBooking tbSectorBooking { get; set; }
        public virtual tbPrincipal tbPrincipal1 { get; set; }
    }
}
