using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.UserModels
{
    public class UserModel
    {
        public string Id { get; set; }
        public string AreaDistrictId { get; set; }
        public string AreaWardId { get; set; }
        public string DepartmentId { get; set; }
        public string JobPositionId { get; set; }
        public string GroupUserId { get; set; }
        public string TaxCode { get; set; }
        public string IdentifyNum { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public string AreaUserId { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
        public string PasswordHash { get; set; }
        public string HomeURL { get; set; }
        public string FullName { get; set; }
        public string UserLevel { get; set; }
        public int Gender { get; set; }
        public Nullable<System.DateTime> Birthdate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string AvatarPath { get; set; }
        public bool IsDisable { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
    }
}
