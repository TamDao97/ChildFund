using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.UserModels
{
    public class UserSearchResult
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string UnitName { get; set; }

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
        /// Cơ quan
        /// </summary>
        public string Agency { get; set; }
        /// <summary> 
        /// Email 
        /// </summary> 
        public string Email { get; set; }
        /// <summary>
        /// ĐƠn vị quản lý
        /// </summary>
        public string UnitId { get; set; }
        /// <summary>
        /// Mức quyền
        /// </summary>
        public string UserLevel { get; set; }
        /// <summary>
        /// Cấp bậc
        /// </summary>
        public string Type { get; set; }
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
        public string AreaUserId { get; set; }
        public string AreaDistrictId { get; set; }
    }
}
