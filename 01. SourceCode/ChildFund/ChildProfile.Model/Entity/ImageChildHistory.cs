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
    
    public partial class ImageChildHistory
    {
        public string Id { get; set; }
        public string ChildProfileId { get; set; }
        public string ImagePath { get; set; }
        public string ImageThumbnailPath { get; set; }
        public string UploadBy { get; set; }
        public Nullable<System.DateTime> UploadDate { get; set; }
    }
}
