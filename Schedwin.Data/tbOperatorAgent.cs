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
    
    public partial class tbOperatorAgent
    {
        public int pkOperatorAgentID { get; set; }
        public string Description { get; set; }
        public int fkCompanyID { get; set; }
        public bool Active { get; set; }
    
        public virtual tbOperator tbOperator { get; set; }
    }
}
