using InformationHub.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.UserModels
{
    public class UserSearchCondition: SearchConditionBase
    {

        /// <summary> 
        /// Nhóm người dùng
        /// </summary> 
        public string GroupId { get; set; }

        /// <summary> 
        /// Tên đăng nhập 
        /// </summary> 
        public string Name { get; set; }
        /// <summary> 
        /// Họ tên 
        /// </summary> 
        public string FullName { get; set; }

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
        public string PoliceLevel { get; set; }
        /// <summary>
        /// Chức vụ
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary> 
        /// Trạng thái 
        /// </summary> 
        public Nullable<int> Status { get; set; }
    
        //Loại tài khoản
        public string Type { get; set; }

        public string ToString()
        {
            return "User: " + this.FullName + "jshd: ";
        }

        public string AreaUserId { get; set; }
        public string AreaDistrictId { get; set; }

        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }

        public int Export { get; set; }
    }
}
