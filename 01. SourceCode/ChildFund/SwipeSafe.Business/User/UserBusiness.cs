using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using SwipeSafe.Model.Repositories;
using SwipeSafe.Model.SearchResults;
using SwipeSafe.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SwipeSafe.Business.User
{
    public class UserBusiness
    {
        private ReportAppEntities db = new ReportAppEntities();
        public SearchResultObject<UserSearchResult> SearchUser(UserSearchCondition searchCondition)
        {
            SearchResultObject<UserSearchResult> searchResult = new SearchResultObject<UserSearchResult>();
            try
            {
                var listmodel = (from a in db.Users.AsNoTracking()
                                 join b in db.Provinces.AsNoTracking() on a.ProvinceId equals b.Id into ab
                                 from abv in ab.DefaultIfEmpty()
                                 join c in db.Districts.AsNoTracking() on a.DistrictId equals c.Id into ac
                                 from acv in ac.DefaultIfEmpty()
                                 join d in db.Wards.AsNoTracking() on a.WardId equals d.Id into ad
                                 from adv in ad.DefaultIfEmpty()
                                 select new UserSearchResult()
                                 {
                                     Id = a.Id,
                                     FullName = a.FullName,
                                     Name = a.UserName,
                                     Gender = a.Gender,
                                     BirthDate = a.Birthdate,
                                     Type = a.Type,
                                     IsDisable = a.IsDisable,
                                     ProvinceId = a.ProvinceId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     ProvinceName = abv != null ? abv.Name : "",
                                     DistrictName= acv != null ? acv.Name : "",
                                     WardName = adv != null ? adv.Name : "",
                                 }).AsQueryable();

                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.FullName))
                {
                    listmodel = listmodel.Where(r => r.FullName.ToLower().Contains(searchCondition.FullName.ToLower()));
                }

                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
            }
            catch (Exception ex)
            {
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
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderFileUser);
                            }).Result;
                        }
                    }

                    string pass = Guid.NewGuid().ToString().Substring(0, 31);
                    string passwordHash = PasswordUtil.ComputeHash(model.Password + pass);
                    SwipeSafe.Model.Repositories.User newUser = new SwipeSafe.Model.Repositories.User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName.ToLower(),
                        FullName = model.FullName,
                        PasswordHash = passwordHash,
                        IsDisable = model.IsDisable,
                        Birthdate = model.Birthdate,
                        Phone = model.Phone,
                        Email = model.Email,
                        AvatarPath = model.AvatarPath,
                        Gender = model.Gender,
                        Type = model.Type,
                        Address = model.Address,
                        ProvinceId = model.ProvinceId,
                        DistrictId = model.DistrictId,
                        WardId = model.WardId,
                        SecurityStamp = pass,
                        HomeURL = "NTS",
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
                            FullName = a.FullName,
                            Phone = a.Phone,
                            Address = a.Address,
                            Birthdate = a.Birthdate,
                            ProvinceId = a.ProvinceId,
                            DistrictId = a.DistrictId,
                            WardId = a.WardId,
                            Type = a.Type,
                            Gender = a.Gender,
                            AvatarPath = a.AvatarPath,
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
                            return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderFileUser);
                        }).Result;
                    }
                }
                //edit
                var modelUpdate = db.Users.FirstOrDefault(u => u.Id.Equals(model.Id));
                modelUpdate.UserName = model.UserName.ToLower();
                modelUpdate.FullName = model.FullName;
                modelUpdate.Birthdate = model.Birthdate;
                modelUpdate.Phone = model.Phone;
                modelUpdate.Email = model.Email;
                modelUpdate.AvatarPath = model.AvatarPath;
                modelUpdate.Gender = model.Gender;
                modelUpdate.Address = model.Address;
                modelUpdate.ProvinceId = model.ProvinceId;
                modelUpdate.DistrictId = model.DistrictId;
                modelUpdate.WardId = model.WardId;
                modelUpdate.Type = model.Type;
                modelUpdate.IsDisable = model.IsDisable;
                modelUpdate.UpdateDate = DateTime.Now;
                modelUpdate.UpdateBy = model.CreateBy;
                //save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
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
                string pass = Guid.NewGuid().ToString().Substring(0, 31);
                string passwordHash = PasswordUtil.ComputeHash(password + pass);
                user.SecurityStamp = pass;
                user.PasswordHash = passwordHash;
                //save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
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
                if (user.IsDisable) user.IsDisable = false;
                else
                    user.IsDisable = true;
                //save
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public UserModel GetById(string id)
        {
            UserModel user = new UserModel();
            try
            {
                var rs = db.Users.FirstOrDefault(u => u.Id == id);
                if (rs != null)
                {
                    user.Id = rs.Id;
                    user.UserName = rs.UserName;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return user;
        }
    }
}
