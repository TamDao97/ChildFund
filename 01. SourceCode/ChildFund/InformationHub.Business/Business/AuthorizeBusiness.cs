using InformationHub.Model;
using InformationHub.Model.CacheModel;
using InformationHub.Model.Repositories;
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

namespace InformationHub.Business.Business
{
    public class AuthorizeBusiness
    {
        private InformationHubEntities _dbEntities = new InformationHubEntities();
        private RedisService<LoginProfileModel> redisService = RedisService<LoginProfileModel>.GetInstance();
        public void RemoveCache()
        {
            var uId = "";
            try
            {
                uId = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (Exception) { }
            try
            {
                if (!string.IsNullOrEmpty(uId))
                {
                    string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
                    redisService.Remove(KeyLoginUserInfo + uId);
                }
            }
            catch (Exception) { }
        }
        public LoginProfileModel Login(LoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR007, null);
            }

            var userModel = _dbEntities.Users.AsNoTracking().Where(r => r.UserName.ToLower().Equals(loginModel.UserName.ToLower().Trim())).FirstOrDefault();
            if (userModel == null)
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR008, null);
            }

            if (userModel.IsDisable)
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR009, null);
            }

            string passwordHash = PasswordUtil.ComputeHash(loginModel.Password + userModel.Password);
            if (!userModel.PasswordHash.Equals(passwordHash))
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR005, null);
            }

            //Lấy thông tin đăng nhập lưu lên cache
            LoginProfileModel loginProfileModel = new LoginProfileModel();
            loginProfileModel.Id = userModel.Id;
            loginProfileModel.Name = userModel.FullName;
            loginProfileModel.Type = userModel.Type;
            loginProfileModel.AreaUserId = userModel.AreaUserId;
            loginProfileModel.WardId = userModel.WardId;
            loginProfileModel.DistrictId = userModel.DistrictId;// areaUsers != null ? areaUsers.DistrictId : string.Empty;
            loginProfileModel.ProvinceId = userModel.ProvinceId;// areaUsers != null ? areaUsers.ProvinceId : string.Empty;
            loginProfileModel.UserName = userModel.UserName;
            loginProfileModel.UserLever = userModel.Type;
            loginProfileModel.IsDisable = userModel.IsDisable;
            loginProfileModel.ImagePath = userModel.AvatarPath;
            loginProfileModel.SecurityKey = loginModel.SecurityKey;
            loginProfileModel.HomeUrl = userModel.HomeURL;
            loginProfileModel.IdentifyNumber = userModel.IdentifyNumber;
            loginProfileModel.ListRoles = (from up in _dbEntities.UserPermissions
                                           where up.UserId.Equals(userModel.Id)
                                           join p in _dbEntities.Permissions on up.PermissionId equals p.Id
                                           select p.Code).ToList();
            loginProfileModel.LoginTime = DateTime.Now;

            //Xóa cache
            string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";

            LogUtils.ExceptionLog("AuthorizeBusiness.LoginProfileModel", "Login thành công",loginModel);

            redisService.Remove(KeyLoginUserInfo + loginProfileModel.Id);
            //Lưu cache
            redisService.Add(KeyLoginUserInfo + loginProfileModel.Id, loginProfileModel);

            #region[cache notify]
            try
            {
                var userId = loginProfileModel.Id;
                RedisService<NotifyModel> redisPlan = RedisService<NotifyModel>.GetInstance();
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                //   var lstPlan = redisPlan.GetContains(cacheNotify + userId + ":*");
                NotifyModel notifyModel;
                var date75 = DateTime.Now.AddDays(-75);
                var date90 = DateTime.Now.AddDays(-90);
                var sPlan = _dbEntities.SupportPlants.Where(u => u.CreateBy.Equals(userId) && u.PlantDate <= date75).ToList();
                var results = sPlan.GroupBy(p => p.ReportProfileId, (key, g) => new { ReportProfileId = key, list = g.OrderByDescending(u => u.PlantDate).FirstOrDefault() }).ToList();
                var supportPlanAll = (from a in results
                                      join b in _dbEntities.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                      where b.StatusStep6 == false
                                      select new { plan = a.list, fullAddress = b.FullAddress }).ToList();
                var supportPlan75 = supportPlanAll.Where(u => u.plan.PlantDate <= date75 && u.plan.PlantDate > date90 && u.plan.IsRemind75 == false).ToList();
                var supportPlan90 = supportPlanAll.Where(u => u.plan.PlantDate <= date90 && u.plan.IsRemind90 == false).ToList();
                TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                foreach (var item in supportPlan90)
                {
                    var itemUpdate = sPlan.FirstOrDefault(u => u.Id.Equals(item.plan.Id));
                    itemUpdate.IsRemind90 = true;
                    notifyModel = new NotifyModel();
                    notifyModel.Id = Guid.NewGuid().ToString();
                    notifyModel.Addres = (item.plan.PlantDate != null ? item.plan.PlantDate.Value.ToString("dd/MM/yyyy - ") : " - ") + item.fullAddress;
                    notifyModel.IdPlan = item.plan.Id;
                    notifyModel.CreateDate = DateTime.Now;
                    notifyModel.Status = Constants.NotViewNotification;
                    notifyModel.Title = "Nhắc nhở cập nhật sau 3 tháng lập kế hoạch: " + item.plan.TitlePlant + " (lập ngày " + (item.plan.PlantDate != null ? item.plan.PlantDate.Value.ToString("dd/MM/yyyy") : "") + ")";
                    notifyModel.Link = "/ReportProfile/ReportDetail/" + item.plan.ReportProfileId;
                    redisPlan.Add(cacheNotify + notifyModel.Id, notifyModel, ts);
                }
                foreach (var item in supportPlan75)
                {
                    var itemUpdate = sPlan.FirstOrDefault(u => u.Id.Equals(item.plan.Id));
                    itemUpdate.IsRemind75 = true;
                    notifyModel = new NotifyModel();
                    notifyModel.Id = Guid.NewGuid().ToString();
                    notifyModel.Addres = (item.plan.PlantDate != null ? item.plan.PlantDate.Value.ToString("dd/MM/yyyy - ") : " - ") + item.fullAddress;
                    notifyModel.IdPlan = item.plan.Id;
                    notifyModel.CreateDate = DateTime.Now;
                    notifyModel.Status = Constants.NotViewNotification;
                    notifyModel.Title = "Nhắc nhở cập nhật sắp đến hạn 3 tháng kế hoạch: " + item.plan.TitlePlant + " (lập ngày " + (item.plan.PlantDate != null ? item.plan.PlantDate.Value.ToString("dd/MM/yyyy") : "") + ")";
                    notifyModel.Link = "/ReportProfile/ReportDetail/" + item.plan.ReportProfileId;
                    redisPlan.Add(cacheNotify + notifyModel.Id, notifyModel, ts);
                }
                _dbEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("AuthorizeBusiness.LoginProfileModel", ex.Message, loginModel);
            }
            #endregion
            return loginProfileModel;
        }

        /// <summary>
        /// Lấy thôn gtin login trên cache
        /// </summary>
        /// <returns></returns>
        public LoginProfileModel GetCacheLoginProfile(string id)
        {
            try
            {
                string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
                LoginProfileModel loginProfileModel = redisService.Get<LoginProfileModel>(KeyLoginUserInfo + id);
                if (loginProfileModel != null && !string.IsNullOrEmpty(loginProfileModel.Id))
                {
                    return loginProfileModel;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
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
                throw new BusinessException(Resource.Resource.Authorize_ERR003, null);
            }

            profileUserModel.Id = usersFirst.Id;
            profileUserModel.Name = usersFirst.FullName;
            profileUserModel.DateOfBirth = usersFirst.Birthdate;
            profileUserModel.PhoneNumber = usersFirst.Phone;
            profileUserModel.Email = usersFirst.Email;
            profileUserModel.ImagePath = usersFirst.AvatarPath;
            profileUserModel.Gender = usersFirst.Gender;
            profileUserModel.IdentifyNumber = "";
            profileUserModel.Address = usersFirst.Address;
            profileUserModel.IdentifyNumber = usersFirst.IdentifyNumber;

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
                throw new BusinessException(Resource.Resource.Authorize_ERR003, null);
            }

            usersUpdate.FullName = model.Name;
            usersUpdate.Birthdate = model.DateOfBirth;
            usersUpdate.Phone = model.PhoneNumber;
            usersUpdate.Email = model.Email;
            usersUpdate.Gender = model.Gender;
            usersUpdate.Address = model.Address;
            usersUpdate.IdentifyNumber = model.IdentifyNumber;
            usersUpdate.UpdateBy = model.Id;
            usersUpdate.UpdateDate = DateTime.Now;

            if (httpFile.Count > 0)
            {
                model.ImagePath = Task.Run(async () =>
                {
                    return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[0], httpFile[0].FileName, Constants.FolderReportProfile);
                }).Result;
            }
            else
            {
                model.ImagePath = "/img/avatar-34.png";
            }
            usersUpdate.AvatarPath = model.ImagePath;
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
                throw new BusinessException(Resource.Resource.Authorize_ERR006, null);
            }

            if (!model.PasswordNew.Equals(model.ConfirmPasswordNew))
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR004, null);
            }

            var userModel = _dbEntities.Users.Where(r => r.Id.Equals(model.Id) && !r.IsDisable).FirstOrDefault();
            if (userModel == null)
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR003, null);
            }

            string passwordHash = PasswordUtil.ComputeHash(model.PasswordOld + userModel.Password);
            if (!userModel.PasswordHash.Equals(passwordHash))
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR005, null);
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
                throw new BusinessException(Resource.Resource.Authorize_ERR010, null);
            }

            if (userModel.IsDisable)
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR009, null);
            }

            if (!userModel.Email.Equals(forwardPasswordModel.Email))
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR011, null);
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
            var resultSendMail = SendMail(mailSend, mailPass, forwardPasswordModel.Email, title, content);
            if (resultSendMail)
            {
                RedisService<ForwardPasswordModel> redisForwardPassword = RedisService<ForwardPasswordModel>.GetInstance();
                string KeyForwardPassword = ConfigurationManager.AppSettings["cacheNotify"] + "ForwardPassword:";
                redisForwardPassword.Add(KeyForwardPassword + forwardPasswordModel.Id, forwardPasswordModel, new TimeSpan(0, timeOut, 0));
                return forwardPasswordModel.Id;
            }
            else
            {
                throw new BusinessException(Resource.Resource.Authorize_ERR012, null);
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
                throw new BusinessException(Resource.Resource.Authorize_ERR014, null);
            }

            if (modelCache.Id.Equals(modelConfirm.Id) && modelCache.ConfirmKey.Equals(modelConfirm.ConfirmKey))
            {
                var userModel = _dbEntities.Users.FirstOrDefault(r => r.Id.Equals(modelCache.Id) && !r.IsDisable);

                if (userModel == null)
                {
                    throw new BusinessException(Resource.Resource.Authorize_ERR003, null);
                }

                userModel.PasswordHash = PasswordUtil.ComputeHash(modelConfirm.PasswordNew + userModel.Password);
                _dbEntities.SaveChanges();
                redisForwardPassword.Remove(KeyForwardPassword + userModel.Id);
                return true;
            }
            else
            {
                throw new Exception(Resource.Resource.Authorize_ERR013, null);
            }
        }

        public static bool SendMail(string emailSend, string passSend, string emailInbox, string title, string content)
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
                return false;
            }
        }
    }
}
