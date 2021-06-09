using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Utils
{
    public static class ErrorMessage
    {
        public static string ConvertMessage(Exception ex)
        {
            return ex.InnerException != null ? ErrorMessage.ERR001 : ex.Message;
        }
        public const string ERR001 = "Có lỗi phát sinh trong quá trình xử lý.";
        public const string ERR002 = "Bản ghi này đã bị xóa bởi người dùng khác";
        public const string ERR003 = "Tài khoản không tồn tại hoặc đã bị khóa bởi quản trị viên của phần mềm.Vui lòng kiểm tra lại!";
        public const string ERR004 = "Mật khẩu mới và xác nhận mật khẩu mới không khớp nhau!";
        public const string ERR005 = "Mật khẩu không đúng!";
        public const string ERR006 = "Các trường mật khẩu không được để trống!";
        public const string ERR007 = "Thông tin đăng nhập không được để trống";
        public const string ERR008 = "Tên đăng nhập không đúng!";
        public const string ERR009 = "Tài khoản đang bị khóa. Liên hệ quản trị để kích hoạt lại.";
        public const string ERR010 = "Tài khoản không tồn tại trong hệ thống.";
        public const string ERR011 = "Email xác nhận không đúng của tài khoản này.";
        public const string ERR012 = "Lỗi trong quá trình gửi email xác thực thay đổi mật khẩu. Vui lòng thử lại!";
        public const string ERR013 = "Mã xác thực thay đổi mật khẩu không đúng. Vui lòng kiểm tra lại email để nhập lại mã xác thực cho đúng!";
        public const string ERR014 = "Chưa nhận được yêu cầu thay đổi lại mật khẩu mới. Vui lòng thử lại.";
    }
}
