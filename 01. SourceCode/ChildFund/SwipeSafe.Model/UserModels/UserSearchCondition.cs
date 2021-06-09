using SwipeSafe.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.UserModels
{
    public class UserSearchCondition: SearchConditionBase
    {
        /// <summary> 
        /// Tên đăng nhập 
        /// </summary> 
        public string Name { get; set; }
        /// <summary> 
        /// Họ tên 
        /// </summary> 
        public string FullName { get; set; }

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary> 
        /// Trạng thái 
        /// </summary> 
        public Nullable<int> Status { get; set; }
    }
}
