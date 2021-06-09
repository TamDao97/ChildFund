using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
    public class ForwardPasswordModel
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Key xác nhận
        /// </summary>
        public string ConfirmKey { get; set; }
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
