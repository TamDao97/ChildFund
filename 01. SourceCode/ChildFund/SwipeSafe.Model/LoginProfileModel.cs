using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model
{
    public class LoginProfileModel
    {
        /// <summary>
        /// Id user
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Tài khoản đăng nhập
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        ///Loại tài khoản
        /// </summary>
        public int Type { get; set; }

        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }

        public string WardId { get; set; }
        /// <summary>
        /// Trạng thái hoạt động tài khoản
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// Image path
        /// </summary>
        public string ImagePath { get; set; }
        public string SecurityKey { get; set; }

        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
