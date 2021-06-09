using NTS.Common;
using NTS.Common.Utils;
using NTS.Utils;
using SwipeSafe.Model;
using SwipeSafe.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Business.Authorize
{
    public class AuthorizeBusiness
    {
        private ReportAppEntities _dbEntities = new ReportAppEntities();

        public LoginProfileModel Login(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
            {
                throw new BusinessException(ErrorMessage.ERR007, null);
            }

            var userModel = _dbEntities.Users.AsNoTracking().Where(r => r.UserName.ToLower().Equals(loginModel.UserName.ToLower().Trim())).FirstOrDefault();
            if (userModel == null)
            {
                throw new BusinessException(ErrorMessage.ERR008, null);
            }

            if (userModel.IsDisable)
            {
                throw new BusinessException(ErrorMessage.ERR009, null);
            }

            string passwordHash = PasswordUtil.ComputeHash(loginModel.Password + userModel.SecurityStamp);
            if (!userModel.PasswordHash.Equals(passwordHash))
            {
                throw new BusinessException(ErrorMessage.ERR005, null);
            }

            //Lấy thông tin đăng nhập lưu lên cache
            LoginProfileModel loginProfileModel = new LoginProfileModel();
            loginProfileModel.Id = userModel.Id;
            loginProfileModel.Name = userModel.FullName;
            loginProfileModel.ProvinceId = userModel.ProvinceId;
            loginProfileModel.DistrictId = userModel.DistrictId;
            loginProfileModel.WardId = userModel.WardId;
            loginProfileModel.UserName = userModel.UserName;
            loginProfileModel.Type = userModel.Type;
            loginProfileModel.IsDisable = userModel.IsDisable;
            loginProfileModel.ImagePath = userModel.AvatarPath;
            loginProfileModel.SecurityKey = loginModel.SecurityKey;
            loginProfileModel.Phone = userModel.Phone;
            loginProfileModel.Address = userModel.Address;

            return loginProfileModel;
        }
    }
}
