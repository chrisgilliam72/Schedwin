//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Schedwin.Data.Wish
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbUser
    {
        public int pkUserId { get; set; }
        public string PersonCode { get; set; }
        public Nullable<System.Guid> ADirectoryGUID { get; set; }
        public Nullable<System.DateTime> LastAccess { get; set; }
        public string UserLoggedIn { get; set; }
        public Nullable<int> UsedResSysCount { get; set; }
        public bool Active { get; set; }
    }
}
