//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SwipeSafe.Model.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProfilePrisoner
    {
        public string Id { get; set; }
        public string ProfileChildId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Phone { get; set; }
        public string Relationship { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
    }
}
