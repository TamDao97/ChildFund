//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChildProfiles.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ward
    {
        public string Id { get; set; }
        public string DistrictId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
    
        public virtual District District { get; set; }
    }
}
