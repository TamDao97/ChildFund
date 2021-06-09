using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ChangePasswordUserModel
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Mật khẩu cũ
        /// </summary>
        public string PasswordOld { get; set; }

        /// <summary>
        /// Mật khẩu mới
        /// </summary>
        public string PasswordNew { get; set; }

        /// <summary>
        /// Xác nhận mật khẩu mới
        /// </summary>
        public string ConfirmPasswordNew { get; set; }
    }
}
