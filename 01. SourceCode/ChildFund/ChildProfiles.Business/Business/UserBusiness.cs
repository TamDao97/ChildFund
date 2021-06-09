using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.UserModels;
using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChildProfiles.Business
{
    public class UserBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();

        public SearchResultObject<UserSearchResult> SearchUser(UserSearchCondition searchCondition)
        {
            SearchResultObject<UserSearchResult> searchResult = new SearchResultObject<UserSearchResult>();
            try
            {
                var listmodel = (from a in db.Users.AsNoTracking()
                                 join b in db.AreaUsers.AsNoTracking() on a.AreaUserId equals b.Id into ab
                                 from ab1 in ab.DefaultIfEmpty()
                                 join c in db.AreaDistricts.AsNoTracking() on a.AreaDistrictId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 join d in db.AreaWards.AsNoTracking() on a.AreaWardId equals d.Id into ad
                                 from adv in ad.DefaultIfEmpty()
                                 select new UserSearchResult()
                                 {
                                     Id = a.Id,
                                     FullName = a.Name,
                                     Name = a.UserName,
                                     Gender = a.Gender,
                                     BirthDate = a.DateOfBirth,
                                     UserLevel = a.UserLever,
                                     IsDisable = a.IsDisable,
                                     ProvinceName = ab1 != null ? ab1.Name : "",
                                     DistrictName = ac1 != null ? ac1.Name : "",
                                     WardName = adv != null ? adv.Name : "",
                                     AreaUserId = a.AreaUserId,
                                     AreaDistrictId = a.AreaDistrictId
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.FullName))
                {
                    listmodel = listmodel.Where(r => r.FullName.ToLower().Contains(searchCondition.FullName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.UserLevel))
                {
                    listmodel = listmodel.Where(r => r.UserLevel.Equals(searchCondition.UserLevel));
                }
                else
                {
                    listmodel = listmodel.Where(r => r.UserLevel.Equals("0") || r.UserLevel.Equals("1"));
                }
                if (!string.IsNullOrEmpty(searchCondition.AreaUserId))
                {
                    listmodel = listmodel.Where(r => r.AreaUserId.Equals(searchCondition.AreaUserId));
                }
                if (!string.IsNullOrEmpty(searchCondition.AreaDistrictId))
                {
                    listmodel = listmodel.Where(r => r.AreaDistrictId.Equals(searchCondition.AreaDistrictId));
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.searchResult", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }

            return searchResult;
        }

        public void CreateUser(UserModel model, HttpFileCollection httpFile)
        {
            model.AvatarPath = "";
            var checkExist = db.Users.FirstOrDefault(u => u.UserName.ToLower().Equals(model.UserName.ToLower()));
            if (checkExist != null)
            {
                throw new Exception("Tên người dùng đã tồn tại");
            }
            //check email
            if (!string.IsNullOrEmpty(model.Email) && model.Email != "")
            {
                var checkEmail = db.Users.FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower()));
                if (checkEmail != null)
                {
                    throw new Exception("Email này đã tồn tại");
                }
            }

            if (!model.Password.Equals(model.RetypePassword))
            {
                throw new Exception("Mật khẩu không khớp");
            }


            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    //Upload file lên cloud
                    if (httpFile.Count > 0)
                    {
                        List<string> listFileKey = httpFile.AllKeys.ToList();

                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            model.AvatarPath = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;
                        }
                    }

                    string pass = Guid.NewGuid().ToString().Substring(0, 31);
                    string passwordHash = PasswordUtil.ComputeHash(model.Password + pass);
                    User newUser = new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName.ToLower(),
                        Name = model.FullName,
                        PasswordHash = passwordHash,
                        IsDisable = model.IsDisable,
                        DateOfBirth = model.Birthdate,
                        PhoneNumber = model.Phone,
                        Email = model.Email,
                        ImagePath = model.AvatarPath,
                        Gender = model.Gender,
                        UserLever = model.UserLevel,
                        IdentifyNumber = model.IdentifyNum,
                        Address = model.Address,
                        AreaUserId = model.AreaUserId,
                        AreaDistrictId = model.AreaDistrictId,
                        AreaWardId = model.AreaWardId,
                        Password = pass,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("UserBusiness.CreateUser", ex.Message, model);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }

        public object GetUserInfo(string id)
        {
            var data = (from a in db.Users
                        where a.Id.Equals(id)
                        select new UserModel
                        {
                            Id = a.Id,
                            UserName = a.UserName,
                            FullName = a.Name,
                            Password = a.Password,
                            Phone = a.PhoneNumber,
                            Address = a.Address,
                            Birthdate = a.DateOfBirth,
                            AreaUserId = a.AreaUserId,
                            AreaDistrictId = a.AreaDistrictId,
                            AreaWardId = a.AreaWardId,
                            UserLevel = a.UserLever,
                            Gender = a.Gender,
                            AvatarPath = a.ImagePath,
                            Email = a.Email,
                            IsDisable = a.IsDisable
                        }).FirstOrDefault();
            if (data == null)
            {
                throw new Exception(ErrorMessage.ERR002);
            }
            try
            {
                return data;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.GetUserInfo", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public void UpdateUser(UserModel model, HttpFileCollection httpFile)
        {
            var checkExist = db.Users.FirstOrDefault(u => !u.Id.Equals(model.Id) && u.UserName.ToLower().Equals(model.UserName.ToLower()));
            if (checkExist != null)
            {
                throw new Exception("Tên người dùng đã tồn tại");
            }

            //check email
            if (!string.IsNullOrEmpty(model.Email) && model.Email != "")
            {
                var checkEmail = db.Users.FirstOrDefault(u => !u.Id.Equals(model.Id) && u.Email.ToLower().Equals(model.Email.ToLower()));
                if (checkEmail != null)
                {
                    throw new Exception("Email này đã tồn tại");
                }
            }

            try
            {
                //Upload file lên cloud
                if (httpFile.Count > 0)
                {
                    List<string> listFileKey = httpFile.AllKeys.ToList();

                    for (int i = 0; i < httpFile.Count; i++)
                    {
                        model.AvatarPath = Task.Run(async () =>
                        {
                            return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                        }).Result;
                    }
                }
                //edit
                var modelUpdate = db.Users.FirstOrDefault(u => u.Id.Equals(model.Id));
                modelUpdate.UserName = model.UserName.ToLower();
                modelUpdate.Name = model.FullName;
                // modelUpdate.Password = model.Password;
                modelUpdate.DateOfBirth = model.Birthdate;
                modelUpdate.PhoneNumber = model.Phone;
                modelUpdate.Email = model.Email;
                modelUpdate.ImagePath = model.AvatarPath;
                modelUpdate.Gender = model.Gender;
                modelUpdate.IdentifyNumber = model.IdentifyNum;
                modelUpdate.Address = model.Address;
                modelUpdate.AreaUserId = model.AreaUserId;
                modelUpdate.AreaDistrictId = model.AreaDistrictId;
                modelUpdate.AreaWardId = model.AreaWardId;
                modelUpdate.UserLever = model.UserLevel;
                modelUpdate.IsDisable = model.IsDisable;
                modelUpdate.UpdateDate = DateTime.Now;
                modelUpdate.UpdateBy = model.CreateBy;
                //save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.UpdateUser", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public void DeleteUser(UserModel model)
        {
            //check conditions

            try
            {
                var data = db.Users.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (data == null)
                {
                    throw new Exception("Người dùng đã được xóa khỏi hệ thống");
                }
                //delete
                db.Users.Remove(data);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.DeleteUser", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public void ResetPassword(string id, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại!");
            }
            try
            {
                RedisService<LoginProfileModel> redisService = RedisService<LoginProfileModel>.GetInstance();
                string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
                LoginProfileModel loginProfileModel = redisService.Get<LoginProfileModel>(KeyLoginUserInfo + id);
                redisService.Remove(KeyLoginUserInfo + id);
                string pass = Guid.NewGuid().ToString().Substring(0, 31);
                string passwordHash = PasswordUtil.ComputeHash(password + pass);
                user.Password = pass;
                user.PasswordHash = passwordHash;
                //save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.ResetPassword", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public void ChangeStatus(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("Tài khoản không tồn tại!");
            }
            try
            {
                RedisService<LoginProfileModel> redisService = RedisService<LoginProfileModel>.GetInstance();
                string KeyLoginUserInfo = ConfigurationManager.AppSettings["cacheNotify"] + "LoginUserInfo:";
                LoginProfileModel loginProfileModel = redisService.Get<LoginProfileModel>(KeyLoginUserInfo + id);
                redisService.Remove(KeyLoginUserInfo + id);
                if (user.IsDisable) user.IsDisable = false;
                else
                    user.IsDisable = true;
                //save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.ChangeStatus", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public UserModel GetById (string id)
        {
            UserModel user = new UserModel();  
            try
            {
                var rs = db.Users.FirstOrDefault(u => u.Id == id);
                if(rs!=null)
                {
                    user.Id = rs.Id;
                    user.UserName = rs.UserName;
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("UserBusiness.GetById", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return user;
        }
    }
}
