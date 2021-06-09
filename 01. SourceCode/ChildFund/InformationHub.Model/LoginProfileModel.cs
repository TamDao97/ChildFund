using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
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
        public string Type { get; set; }
        /// <summary>
        /// Tài khoản đăng nhập
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Cấp sử dụng tài khoản
        /// </summary>
        public string UserLever { get; set; }
        /// <summary>
        /// Id vùng hoạt động
        /// </summary>
        public string AreaUserId { get; set; }
        public string WardId { get; set; }

        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        /// <summary>
        /// Trạng thái hoạt động tài khoản
        /// </summary>
        public bool IsDisable { get; set; }
        /// <summary>
        /// Danh sách quyền
        /// </summary>
     //   public List<string> ListRoles { get; set; }
        /// <summary>
        /// Image path
        /// </summary>
        public string ImagePath { get; set; }
        public string SecurityKey { get; set; }
        public string HomeUrl { get; set; }
        public string IdentifyNumber { get; set; }

        public List<string> ListRoles { get; set; }

        public DateTime LoginTime { get; set; }
    }
}
