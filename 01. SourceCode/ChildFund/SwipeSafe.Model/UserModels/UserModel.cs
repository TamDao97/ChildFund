using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.UserModels
{
    public class UserModel 
    {
        public string Id { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string UserName { get; set; }
        public int Type { get; set; }
        public string ProvinceId { get; set; }
        public string Password { get; set; }
        public string RetypePassword { get; set; }
        public string PasswordHash { get; set; }
        public string HomeURL { get; set; }
        public string FullName { get; set; }
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
