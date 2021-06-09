using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NTS.Api;
using NTS.Api.Models;
using System.Text.RegularExpressions;
using System.Data;

namespace SwipeSafe.Api
{
    public class UserController
    {
        //public LoginEntity Login(string userName, string password)
        //{
        //    LoginEntity loginEntity = new LoginEntity();
        //    try
        //    {
        //        AuthenBusiness _authen = new AuthenBusiness();
        //        //thay mới = entity
        //        EMSST1200UserInfoModel userLogin = _authen.GetUserLogin(userName);
        //        if (userLogin != null)
        //        {
        //            if (userLogin.IsDisable == Constants.STATUS_LOCK)
        //            {
        //                //Tài khoản bị khóa. Lên hệ quản trị để kích hoạt lại
        //                loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_LOCK;
        //            }
        //            else
        //            {
        //                var securityStamp = PasswordUtil.ComputeHash(password + userLogin.SecurityStamp);
        //                if (userLogin.PasswordHash.Equals(securityStamp))
        //                {
        //                    UserEntity userEntity = new UserEntity();
        //                    userEntity.FullName = userLogin.EmployeeName;
        //                    userEntity.ImageLink = userLogin.ImagePath;
        //                    userEntity.DepartmentId = userLogin.DepartmentId;
        //                    userEntity.UserId = userLogin.UserId;
        //                    userEntity.ListPermission = new List<string>();
        //                    //thay mới = entity
        //                    List<EMSST1200PermissionModel> listPermission = _authen.GetListPermission(userLogin.UserId);
        //                    userEntity.ListPermission = listPermission.Select(r => r.FunctionCode).ToList<string>();
        //                    loginEntity.UserInfor = userEntity;
        //                }
        //                else
        //                {
        //                    // Mật khẩu không đúng
        //                    loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_WRONG_PASSWORD;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_NOT_EXITS_USER;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loginEntity.ResponseCode = Constants.RESPONSE_LOGIN_STATUS_SERVER_ERROR;
        //    }
        //    return loginEntity;
        //}
    }
}