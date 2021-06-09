using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model;
using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace ChildProfiles.Business.Business
{
    public class AuthorizeBusiness
    {
        private ChildProfileEntities _dbEntities = new ChildProfileEntities();
        private RedisService<LoginProfileModel> redisService = RedisService<LoginProfileModel>.GetInstance();

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

            string passwordHash = PasswordUtil.ComputeHash(loginModel.Password + userModel.Password);
            if (!userModel.PasswordHash.Equals(passwordHash))
            {
                throw new BusinessException(ErrorMessage.ERR005, null);
            }

            //Lấy id tỉnh và huyện cho tài khoản giáo viên theo vùng
            var areaUsers = (from a in _dbEntities.AreaUsers
                             join b in _dbEntities.AreaDistricts on a.Id equals b.AreaUserId
                             join c in _dbEntities.AreaWards on b.Id equals c.AreaDistrictId
                             where a.Id.Equals(userModel.AreaUserId) && b.DistrictId.Equals(userModel.AreaDistrictId)
                             && c.WardId.Equals(userModel.AreaWardId)
                             select new { a.ProvinceId, b.DistrictId, c.WardId }).FirstOrDefault();

            //Lấy thông tin đăng nhập lưu lên cache
            LoginProfileModel loginProfileModel = new LoginProfileModel();
            loginProfileModel.Id = userModel.Id;
            loginProfileModel.Name = userModel.Name;
            loginProfileModel.AreaUserId = userModel.AreaUserId;
            loginProfileModel.DistrictId = areaUsers != null ? areaUsers.DistrictId : string.Empty;
            loginProfileModel.ProvinceId = areaUsers != null ? areaUsers.ProvinceId : string.Empty;
            loginProfileModel.WardId = areaUsers != null ? areaUsers.WardId : string.Empty;
            loginProfileModel.UserName = userModel.UserName;
            loginProfileModel.UserLever = userModel.UserLever;
            loginProfileModel.IsDisable = userModel.IsDisable;
            loginProfileModel.ImagePath = userModel.ImagePath;
            loginProfileModel.SecurityKey = loginModel.SecurityKey;
            //loginProfileModel.ListRoles = null;
            //Xóa cache
            string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
            redisService.Remove(KeyLoginUserInfo + loginProfileModel.Id);
            //Lưu cache
            redisService.Add(KeyLoginUserInfo + loginProfileModel.Id, loginProfileModel);
            return loginProfileModel;
        }

        /// <summary>
        /// Lấy thôn gtin login trên cache
        /// </summary>
        /// <returns></returns>
        public LoginProfileModel GetCacheLoginProfile(string id)
        {
            string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
            LoginProfileModel loginProfileModel = redisService.Get<LoginProfileModel>(KeyLoginUserInfo + id);
            if (loginProfileModel != null && !string.IsNullOrEmpty(loginProfileModel.Id))
            {
                return loginProfileModel;
            }
            return null;
        }

        /// <summary>
        /// Get thông tin hồ sơ người dùng
        /// </summary>
        /// <returns></returns>
        public ProfileUserModel GetProfileUser(string id)
        {
            ProfileUserModel profileUserModel = new ProfileUserModel();

            var usersFirst = _dbEntities.Users.AsNoTracking().Where(r => r.Id.Equals(id) && !r.IsDisable).FirstOrDefault();

            if (usersFirst == null)
            {
                throw new BusinessException(ErrorMessage.ERR003, null);
            }

            profileUserModel.Id = usersFirst.Id;
            profileUserModel.Name = usersFirst.Name;
            profileUserModel.DateOfBirth = usersFirst.DateOfBirth;
            profileUserModel.PhoneNumber = usersFirst.PhoneNumber;
            profileUserModel.Email = usersFirst.Email;
            profileUserModel.ImagePath = usersFirst.ImagePath;
            profileUserModel.Gender = usersFirst.Gender;
            profileUserModel.IdentifyNumber = usersFirst.IdentifyNumber;
            profileUserModel.Address = usersFirst.Address;

            return profileUserModel;
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        /// <returns></returns>
        public ProfileUserModel UpdateProfileUser(ProfileUserModel model, HttpFileCollection httpFile)
        {
            var usersUpdate = _dbEntities.Users.FirstOrDefault(r => r.Id.Equals(model.Id) && !r.IsDisable);
            if (usersUpdate == null)
            {
                throw new BusinessException(ErrorMessage.ERR003, null);
            }

            usersUpdate.Name = model.Name;
            usersUpdate.DateOfBirth = model.DateOfBirth;
            usersUpdate.PhoneNumber = model.PhoneNumber;
            usersUpdate.Email = model.Email;
            usersUpdate.ImagePath = model.ImagePath;
            usersUpdate.Gender = model.Gender;
            usersUpdate.IdentifyNumber = model.IdentifyNumber;
            usersUpdate.Address = model.Address;
            usersUpdate.UpdateBy = model.Id;
            usersUpdate.UpdateDate = DateTime.Now;

            if (httpFile.Count > 0)
            {
                usersUpdate.ImagePath = Task.Run(async () =>
                {
                    return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[0], httpFile[0].FileName, Constants.FolderFileUser);
                }).Result;
                model.ImagePath = usersUpdate.ImagePath;
            }
            _dbEntities.SaveChanges();

            return model;
        }

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        public bool ChangePasswordUser(ChangePasswordUserModel model)
        {
            if (string.IsNullOrEmpty(model.PasswordOld) || string.IsNullOrEmpty(model.PasswordNew) || string.IsNullOrEmpty(model.ConfirmPasswordNew))
            {
                throw new BusinessException(ErrorMessage.ERR006, null);
            }

            if (!model.PasswordNew.Equals(model.ConfirmPasswordNew))
            {
                throw new BusinessException(ErrorMessage.ERR004, null);
            }

            var userModel = _dbEntities.Users.Where(r => r.Id.Equals(model.Id) && !r.IsDisable).FirstOrDefault();
            if (userModel == null)
            {
                throw new BusinessException(ErrorMessage.ERR003, null);
            }

            string passwordHash = PasswordUtil.ComputeHash(model.PasswordOld + userModel.Password);
            if (!userModel.PasswordHash.Equals(passwordHash))
            {
                throw new BusinessException(ErrorMessage.ERR005, null);
            }

            userModel.PasswordHash = PasswordUtil.ComputeHash(model.PasswordNew + userModel.Password);
            userModel.UpdateBy = model.Id;
            userModel.UpdateDate = DateTime.Now;
            _dbEntities.SaveChanges();
            //Xóa cache
            string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
            redisService.Remove(KeyLoginUserInfo + model.Id);
            return true;
        }

        /// <summary>
        /// Gửi mã xác thực để thay đổi mật khẩu mới
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        public string ForwardPassword(ForwardPasswordModel forwardPasswordModel)
        {
            var userModel = _dbEntities.Users.AsNoTracking().Where(r => r.UserName.Equals(forwardPasswordModel.UserName)).FirstOrDefault();

            if (userModel == null)
            {
                throw new BusinessException(ErrorMessage.ERR010, null);
            }

            if (userModel.IsDisable)
            {
                throw new BusinessException(ErrorMessage.ERR009, null);
            }

            if (!userModel.Email.Equals(forwardPasswordModel.Email))
            {
                throw new BusinessException(ErrorMessage.ERR011, null);
            }

            forwardPasswordModel.ConfirmKey = Guid.NewGuid().ToString().Substring(0, 8);
            forwardPasswordModel.Id = userModel.Id;

            string mailSend = ConfigurationManager.AppSettings["MailSend"];
            string mailPass = ConfigurationManager.AppSettings["MailPass"];
            string confirmKeyTimeOut = ConfigurationManager.AppSettings["ConfirmKeyTimeOut"];
            int timeOut = (!string.IsNullOrEmpty(confirmKeyTimeOut) ? int.Parse(confirmKeyTimeOut) : 5);
            string content = "";
            string title = "Mã xác thực thay đổi mật khẩu phần mềm Child Profile";
            content += $"<p>Bạn vui lòng nhập mã xác thực là: <b> {forwardPasswordModel.ConfirmKey} </b> để hoàn thành thay đổi mật khẩu tài khoản</p>";
            content += $"<p>Mã xác thực có hiệu lực trong vòng <b> {timeOut} (phút) </b> để thực hiện thay đổi này</p>";
            var resultSendMail = this.SendMail(mailSend, mailPass, forwardPasswordModel.Email, title, content);
            if (resultSendMail)
            {
                RedisService<ForwardPasswordModel> redisForwardPassword = RedisService<ForwardPasswordModel>.GetInstance();
                string KeyForwardPassword = ConfigurationManager.AppSettings["cacheNotify"] + "ForwardPassword:";
                redisForwardPassword.Add(KeyForwardPassword + forwardPasswordModel.Id, forwardPasswordModel, new TimeSpan(0, timeOut, 0));
                return forwardPasswordModel.Id;
            }
            else
            {
                throw new BusinessException(ErrorMessage.ERR012, null);
            }
        }

        /// <summary>
        /// Xác nhận thay đổi mật khẩu
        /// </summary>
        /// <param name="modelConfirm"></param>
        public bool ConfirmForwardPassword(ForwardPasswordModel modelConfirm)
        {
            RedisService<ForwardPasswordModel> redisForwardPassword = RedisService<ForwardPasswordModel>.GetInstance();
            string KeyForwardPassword = ConfigurationManager.AppSettings["cacheNotify"] + "ForwardPassword:";

            var modelCache = redisForwardPassword.Get<ForwardPasswordModel>(KeyForwardPassword + modelConfirm.Id);
            if (modelCache == null)
            {
                throw new BusinessException(ErrorMessage.ERR014, null);
            }

            if (modelCache.Id.Equals(modelConfirm.Id) && modelCache.ConfirmKey.Equals(modelConfirm.ConfirmKey))
            {
                var userModel = _dbEntities.Users.FirstOrDefault(r => r.Id.Equals(modelCache.Id) && !r.IsDisable);

                if (userModel == null)
                {
                    throw new BusinessException(ErrorMessage.ERR003, null);
                }

                userModel.PasswordHash = PasswordUtil.ComputeHash(modelConfirm.PasswordNew + userModel.Password);
                _dbEntities.SaveChanges();
                redisForwardPassword.Remove(KeyForwardPassword + userModel.Id);
                return true;
            }
            else
            {
                throw new Exception(ErrorMessage.ERR013, null);
            }
        }

        public bool SendMail(string emailSend, string passSend, string emailInbox, string title, string content)
        {
            try
            {
                MailMessage mailsend = new MailMessage();
                mailsend.To.Add(emailInbox);
                mailsend.From = new MailAddress(emailSend);
                mailsend.Subject = title;
                mailsend.Body = content;
                mailsend.IsBodyHtml = true;
                int cong = 587;
                SmtpClient client = new SmtpClient("smtp.gmail.com", cong);
                client.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(emailSend, passSend);
                client.Credentials = credentials;
                client.Send(mailsend);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("AuthorizeBusiness.SendMail", ex.Message, emailSend + " | " + title + " | " + content);
                return false;
            }
        }
    }
}
