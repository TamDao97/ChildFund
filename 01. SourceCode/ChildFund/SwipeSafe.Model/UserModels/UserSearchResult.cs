using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.UserModels
{
    public class UserSearchResult
    {
        public string Id { get; set; }
        

        /// <summary> 
        /// Tên đăng nhập 
        /// </summary> 
        public string Name { get; set; }
        /// <summary> 
        /// Họ tên 
        /// </summary> 
        public string FullName { get; set; }
        /// <summary> 
        /// Ngày sinh 
        /// </summary> 
        public Nullable<DateTime> BirthDate { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Chức vụ
        /// </summary>
        public string Role { get; set; }
        /// <summary> 
        /// Điện thoại 
        /// </summary> 
        public string PhoneNumber { get; set; }

        public string Address { get; set; }
        public bool IsDisable { get; set; }

        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
    }
}
