using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ProfileUserModel
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Họ tên người dùng
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public Nullable<DateTime> DateOfBirth { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Avatar
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// Số CMTND
        /// </summary>
        public string IdentifyNumber { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Người cập nhật
        /// </summary>
        public string UpdateBy { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public string UpdateDate { get; set; }
    }
}
