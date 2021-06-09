//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace InformationHub.Model.Repositories
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.ActivityLogs = new HashSet<ActivityLog>();
            this.UserPermissions = new HashSet<UserPermission>();
        }
    
        public string Id { get; set; }
        public string GroupUserId { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public string AreaUserId { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public string FullAddress { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string HomeURL { get; set; }
        public string FullName { get; set; }
        public int Gender { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdentifyNumber { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
        public bool IsDisable { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual GroupUser GroupUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}
