using InformationHub.Model;
using InformationHub.Model.Repositories;
using InformationHub.Model.SearchResults;
using InformationHub.Model.UserModels;
using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace InformationHub.Business
{
    public class UserBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        private RedisService<ComboboxResult> redisService = RedisService<ComboboxResult>.GetInstance();

        public SearchResultObject<UserSearchResult> SearchUser(UserSearchCondition searchCondition)
        {
            SearchResultObject<UserSearchResult> searchResult = new SearchResultObject<UserSearchResult>();
            try
            {
                var listmodel = (from a in db.Users.AsNoTracking()
                                 join b in db.Provinces.AsNoTracking() on a.ProvinceId equals b.Id into ab
                                 from ab1 in ab.DefaultIfEmpty()
                                 join c in db.Districts.AsNoTracking() on a.DistrictId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 join d in db.Wards.AsNoTracking() on a.WardId equals d.Id into ad
                                 from ad1 in ad.DefaultIfEmpty()
                                 select new UserSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.UserName,
                                     FullName = a.FullName,
                                     Address = a.Address,
                                     PhoneNumber = a.Phone,
                                     BirthDay = a.Birthdate,
                                     Type = a.Type,
                                     Gender = a.Gender,
                                     IsDisable = a.IsDisable,
                                     ProvinceName = ab1.Name,
                                     DistrictName = ac1.Name,
                                     WardName = ad1.Name,
                                     ProvinceId = ab1.Id,
                                     DistrictId = ac1.Id,
                                     WardId = ad1.Id
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.FullName))
                {
                    listmodel = listmodel.Where(r => r.FullName.ToLower().Contains(searchCondition.FullName.ToLower()));
                }
                if (searchCondition.Type != null)
                {
                    listmodel = listmodel.Where(r => r.Type == searchCondition.Type);
                }
                if (!string.IsNullOrEmpty(searchCondition.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.ToLower().Contains(searchCondition.WardId.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.ToLower().Contains(searchCondition.DistrictId.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.ToLower().Contains(searchCondition.ProvinceId.ToLower()));
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                int indexExcel = 1;

                //export
                if (searchCondition.Export != 0)
                {
                    searchResult.ListResult = listmodel.OrderBy(u => u.Type).ToList();
                    searchResult.PathFile = ExportListUser(searchResult.ListResult);
                    searchResult.ListResult = searchResult.ListResult.Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                }
                else
                {
                    searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                    searchResult.PathFile = "";
                }
                foreach (var item in searchResult.ListResult)
                {
                    item.Index = indexExcel++;
                    item.GenderName = Common.GenGender(item.Gender);
                    item.UserLevel = Common.GenUserType(item.Type);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public void CreateUser(UserModel model, HttpFileCollection httpFile)
        {
            var checkExist = db.Users.FirstOrDefault(u => u.UserName.ToLower().Equals(model.UserName.ToLower()));
            var checkEmail = db.Users.FirstOrDefault(u => u.Email.Equals(model.Email));
            if (checkExist != null)
            {
                throw new Exception(Resource.Resource.User_Used);
            }
            if (checkEmail != null)
            {
                throw new Exception(Resource.Resource.Email_Used);
            }
            if (model.GroupUserId == null)
            {
                throw new Exception(Resource.Resource.User_Uncheck_GroupPermission);
            }

            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (httpFile.Count > 0)
                    {
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            model.AvatarPath = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;
                        }
                    }
                    var uId = Guid.NewGuid().ToString();
                    var password = Guid.NewGuid().ToString();
                    User newUser = new User()
                    {
                        Id = uId,
                        GroupUserId = model.GroupUserId,
                        UserName = model.UserName,
                        Type = model.Type,
                        WardId = model.WardId,
                        DistrictId = model.DistrictId,
                        ProvinceId = model.ProvinceId,
                        FullName = model.FullName,
                        Gender = model.Gender,
                        Birthdate = model.Birthdate,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.Address,
                        AvatarPath = model.AvatarPath,
                        IsDisable = false,
                        PasswordHash = PasswordUtil.ComputeHash(model.Password + password),
                        Password = password,
                        CreateBy = model.CreateBy,
                        CreateDate = DateTime.Now,
                        UpdateBy = model.CreateBy,
                        UpdateDate = DateTime.Now
                    };
                    db.Users.Add(newUser);
                    // db.SaveChanges();

                    foreach (var item in model.ListPermission)
                    {
                        var permission = new UserPermission()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = uId,
                            PermissionId = item
                        };
                        if (permission.PermissionId != null)
                        {
                            db.UserPermissions.Add(permission);
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
                }
            }
        }

        public UserModel GetUserInfo(string id)
        {
            var data = db.Users.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            try
            {
                UserModel model = new UserModel();
                model.Id = data.Id;
                model.FullName = data.FullName;
                model.UserName = data.UserName;
                model.Email = data.Email;
                model.Phone = data.Phone;
                model.Address = data.Address;
                model.Birthdate = data.Birthdate;
                model.GroupUserId = data.GroupUserId;
                model.Gender = data.Gender;
                model.ProvinceId = data.ProvinceId;
                model.DistrictId = data.DistrictId;
                model.WardId = data.WardId;
                model.Password = data.Password;
                model.PasswordHash = data.PasswordHash;
                model.UpdateBy = data.UpdateBy;
                model.UpdateDate = data.UpdateDate;
                model.CreateBy = data.CreateBy;
                model.CreateDate = data.CreateDate;
                model.Type = data.Type;
                model.AvatarPath = data.AvatarPath;
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void UpdateUser(UserModel model, HttpFileCollection httpFile)
        {
            var checkExist = db.Users.FirstOrDefault(u => !u.Id.Equals(model.Id) && u.UserName.ToLower().Equals(model.UserName.ToLower()));
            var checkEmail = db.Users.FirstOrDefault(u => u.Email.Equals(model.Email) && !u.Id.Equals(model.Id));
            if (checkExist != null)
            {
                throw new Exception(Resource.Resource.User_Used);
            }
            if (checkEmail != null)
            {
                throw new Exception(Resource.Resource.Email_Used);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (httpFile.Count > 0)
                    {
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            model.AvatarPath = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;
                        }
                    }
                    var oldPermission = db.UserPermissions.Where(u => u.UserId.Equals(model.Id)).ToList();

                    //edit
                    var modelUpdate = db.Users.FirstOrDefault(u => u.Id.Equals(model.Id));
                    modelUpdate.UserName = model.UserName;
                    modelUpdate.FullName = model.FullName;
                    modelUpdate.Gender = model.Gender;
                    modelUpdate.Birthdate = model.Birthdate;
                    modelUpdate.Email = model.Email;
                    modelUpdate.Phone = model.Phone;
                    modelUpdate.Type = model.Type;
                    modelUpdate.GroupUserId = model.GroupUserId;
                    modelUpdate.WardId = model.WardId;
                    modelUpdate.DistrictId = model.DistrictId;
                    modelUpdate.ProvinceId = model.ProvinceId;
                    modelUpdate.AvatarPath = model.AvatarPath;
                    modelUpdate.UpdateDate = DateTime.Now;
                    modelUpdate.UpdateBy = model.UpdateBy;
                    //remove old permission
                    if (oldPermission.Count > 0)
                    {
                        db.UserPermissions.RemoveRange(oldPermission);
                    }
                    //add new permission
                    foreach (var item in model.ListPermission)
                    {
                        var permission = new UserPermission()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = model.Id,
                            PermissionId = item
                        };
                        if (permission.PermissionId != null)
                        {
                            db.UserPermissions.Add(permission);
                        }
                    }
                    db.SaveChanges();
                    trans.Commit();

                    //save
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
                }
            }
        }

        public List<GroupPermissonViewModel> GetListPermission(string id, int type)
        {
            List<GroupPermissonViewModel> result = new List<GroupPermissonViewModel>();
            List<Permission> listPermission = new List<Permission>();
            GroupPermissonViewModel itemGroupPermissonViewModel;
            try
            {
                var listresult = (from a in db.Permissions.AsNoTracking().Where(r => (type == 0 && r.TypeLevel1) || (type == 1 && r.TypeLevel2)
                                         || (type == 2 && r.TypeLevel3) || (type == 3 && r.TypeLevel4))
                                  join b in db.GroupPermissions.AsNoTracking().Where(c => c.GroupUserId.Equals(id)) on a.Id equals b.PermissionId
                                  select a).ToList();
                var listresultId = listresult.Select(u => u.GroupFunctionId).ToList();
                var groupFunctions = db.GroupFunctions.Where(u => listresultId.Contains(u.Id)).ToList();
                foreach (var item in groupFunctions)
                {
                    listPermission = listresult.Where(u => u.GroupFunctionId.Equals(item.Id)).ToList();
                    itemGroupPermissonViewModel = new GroupPermissonViewModel();
                    itemGroupPermissonViewModel.GroupFunctionId = item.Id;
                    itemGroupPermissonViewModel.Name = item.Name;
                    itemGroupPermissonViewModel.GroupUserId = "1";
                    result.Add(itemGroupPermissonViewModel);
                    foreach (var itemSub in listPermission)
                    {
                        itemGroupPermissonViewModel = new GroupPermissonViewModel();
                        itemGroupPermissonViewModel.GroupFunctionId = item.Id;
                        itemGroupPermissonViewModel.PermissionId = itemSub.Id;
                        itemGroupPermissonViewModel.Name = itemSub.Name;
                        itemGroupPermissonViewModel.Code = itemSub.Code;
                        itemGroupPermissonViewModel.GroupUserId = "0";
                        result.Add(itemGroupPermissonViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return result;
        }

        public List<GroupPermissonViewModel> GetListPermissionUpdate(string groupUserId, string userId, int type)
        {
            List<GroupPermissonViewModel> result = new List<GroupPermissonViewModel>();
            try
            {
                List<Permission> listPermission = new List<Permission>();
                GroupPermissonViewModel itemGroupPermissonViewModel;
                var listPermisson = db.UserPermissions.Where(u => u.UserId.Equals(userId)).Select(u => u.PermissionId).ToList();
                var listresult = (from a in db.Permissions.AsNoTracking().Where(r => (type == 0 && r.TypeLevel1) || (type == 1 && r.TypeLevel2)
                                         || (type == 2 && r.TypeLevel3) || (type == 3 && r.TypeLevel4))
                                  join b in db.GroupPermissions.AsNoTracking().Where(c => c.GroupUserId.Equals(groupUserId)) on a.Id equals b.PermissionId
                                  select a).ToList();
                var listresultId = listresult.Select(u => u.GroupFunctionId).ToList();
                var groupFunctions = db.GroupFunctions.Where(u => listresultId.Contains(u.Id)).ToList();
                foreach (var item in groupFunctions)
                {
                    listPermission = listresult.Where(u => u.GroupFunctionId.Equals(item.Id)).ToList();
                    itemGroupPermissonViewModel = new GroupPermissonViewModel();
                    itemGroupPermissonViewModel.GroupFunctionId = item.Id;
                    itemGroupPermissonViewModel.Name = item.Name;
                    itemGroupPermissonViewModel.GroupUserId = "1";
                    result.Add(itemGroupPermissonViewModel);
                    foreach (var itemSub in listPermission)
                    {
                        itemGroupPermissonViewModel = new GroupPermissonViewModel();
                        itemGroupPermissonViewModel.GroupFunctionId = item.Id;
                        itemGroupPermissonViewModel.PermissionId = itemSub.Id;
                        itemGroupPermissonViewModel.Name = itemSub.Name;
                        itemGroupPermissonViewModel.Code = itemSub.Code;
                        itemGroupPermissonViewModel.GroupUserId = "0";
                        itemGroupPermissonViewModel.ItemChecked = listPermisson.Contains(itemSub.Id) ? "checked" : "";
                        result.Add(itemGroupPermissonViewModel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return result;
        }

        public void ResetPassword(string id, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception(Resource.Resource.User_Not_Exist);
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
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public void LockUser(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id.Equals(id));
            if (user == null || user.IsDisable == true)
            {
                throw new Exception(Resource.Resource.User_Locked);
            }

            try
            {
                user.IsDisable = true;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public void UnLockUser(string id)
        {
            var user = db.Users.FirstOrDefault(u => u.Id.Equals(id));
            if (user == null || user.IsDisable == false)
            {
                throw new Exception(Resource.Resource.User_Locked);
            }

            try
            {
                user.IsDisable = false;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public string ExportListUser(List<UserSearchResult> list)
        {
            string pathExport = "/Template/Export/Danh-Sach-Nguoi-Dung";
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            //string FullPathPDF = HttpContext.Current.Server.MapPath(pathExport + ".pdf");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/ListUser.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];

                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.User_List.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<Index>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<Index>", Resource.Resource.Index_Title);

                IRange rangeValue3 = worksheet.FindFirst("<Account>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<Account>", Resource.Resource.User_Acc);

                IRange rangeValue4 = worksheet.FindFirst("<UserName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<UserName>", Resource.Resource.User_Name);

                IRange rangeValue5 = worksheet.FindFirst("<BirthDay>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<BirthDay>", Resource.Resource.Label_Birthday);

                IRange rangeValue6 = worksheet.FindFirst("<Gender>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Gender>", Resource.Resource.ReportProfile_Gender);

                IRange rangeValue7 = worksheet.FindFirst("<Type>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<Type>", Resource.Resource.User_Type);

                IRange rangeValue8 = worksheet.FindFirst("<Province>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Province>", Resource.Resource.Area_Province);

                IRange rangeValue9 = worksheet.FindFirst("<District>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue9.Text = rangeValue9.Text.Replace("<District>", Resource.Resource.Label_District);

                IRange rangeValue10 = worksheet.FindFirst("<Ward>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue10.Text = rangeValue10.Text.Replace("<Ward>", Resource.Resource.Label_Ward);

                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                int Index = 0;
                var listExport = (from a in list
                                  select new
                                  {
                                      row = Index++,
                                      a.Name,
                                      a.FullName,
                                      a.BirthDay,
                                      GenderName = Common.GenGender(a.Gender),
                                      UserLevel = Common.GenUserType(a.Type),
                                      a.ProvinceName,
                                      a.DistrictName,
                                      a.WardName,
                                  }).ToList();
                var count = listExport.Count();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 9].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 9].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 9].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 9].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 9].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 9].CellStyle.WrapText = true;

                workbook.SaveAs(FullPath);
                workbook.Close();

                return pathExport += ".xlsx";
            }
        }

        public void ImportExcel()
        {
            //Instantiate the spreadsheet creation engine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                string pathTemplate = HttpContext.Current.Server.MapPath("/Template/Export/Mau-Excel-Them-Nguoi-Dung.xlsx");

                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                int rowCount = worksheet.Rows.Count();
                var list = new List<User>();
                User itemUser;
                for (int i = 0; i < rowCount; i++)
                {
                    itemUser = new User();
                    itemUser.FullName = worksheet["A1"].Text;
                }
                // db.Users.AddRange(list);
            }
        }

        public string DownloadTemplate()
        {
            string pathExport = "/Template/Export/Mau-Excel-Them-Nguoi-Dung";
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/CreateUserTemplate.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];

                worksheet.Range["H2:H10000"].DataValidation.ListOfValues = db.GroupUsers.Select(u => u.Name).ToArray();

                workbook.SaveAs(FullPath);
                workbook.Close();

                return pathExport += ".xls";
            }
        }

        public void ImportProfile(string createBy, HttpPostedFile file)
        {
            using (var trans = db.Database.BeginTransaction())
            {

                var fileArray = file.FileName.ToString().Split('.');
                string fileName = string.Empty;
                fileName = Guid.NewGuid().ToString() + "." + fileArray[fileArray.Length - 1];
                string pathFolder = "Template/Upload/";
                string pathFolderServer = HostingEnvironment.MapPath("~/" + pathFolder);
                string fileResult = string.Empty;
                try
                {
                    #region[tải file lên để đọc]
                    // Kiểm tra folder là tên của ProjectId đã tồn tại chưa.
                    if (!Directory.Exists(pathFolderServer))
                    {
                        Directory.CreateDirectory(pathFolderServer);
                    }
                    // kiểm tra size file > 0
                    if (file.ContentLength > 0)
                    {
                        file.SaveAs(pathFolderServer + fileName);
                    }
                    #endregion
                }
                catch (Exception)
                { throw new Exception("Xử lý file excel lỗi, vui lòng thử lại"); }
                #region[add quyên mac dinh]
                int typeLevelTeacher = int.Parse(Constants.LevelTeacher);
                var groupUser = db.GroupUsers.FirstOrDefault(u => u.Type == typeLevelTeacher);
                List<string> listPermission = new List<string>();
                if (groupUser != null)
                {
                    listPermission = (from a in db.Permissions.AsNoTracking().Where(r => (typeLevelTeacher == 3 && r.TypeLevel4))
                                      join b in db.GroupPermissions.AsNoTracking().Where(c => c.GroupUserId.Equals(groupUser.Id)) on a.Id equals b.PermissionId
                                      select a.Id).ToList();
                }
                #endregion
                string keyAllProvince = ConfigurationManager.AppSettings["cacheNotify"] + "AllProvince:";
                List<ComboboxResult> lProvince = redisService.Get<List<ComboboxResult>>(keyAllProvince + "lProvince");
                if (lProvince == null)
                {
                    lProvince = (from a in db.Provinces.AsNoTracking()
                                 select new ComboboxResult
                                 { Id = a.Id, Name = a.Name }).ToList();
                    try
                    {
                        redisService.Add(keyAllProvince + "lProvince", lProvince);
                    }
                    catch (Exception)
                    { }
                }
                List<ComboboxResult> lDistrict = redisService.Get<List<ComboboxResult>>(keyAllProvince + "lDistrict");
                if (lDistrict == null)
                {
                    lDistrict = (from a in db.Districts.AsNoTracking()
                                 select new ComboboxResult
                                 { Id = a.Id, Name = a.Name, PId = a.ProvinceId }).ToList();
                    try
                    {
                        redisService.Add(keyAllProvince + "lDistrict", lDistrict);
                    }
                    catch (Exception)
                    { }
                }
                List<ComboboxResult> lWard = redisService.Get<List<ComboboxResult>>(keyAllProvince + "lWard");
                if (lWard == null)
                {
                    lWard = (from a in db.Wards.AsNoTracking()
                             select new ComboboxResult
                             { Id = a.Id, Name = a.Name, PId = a.DistrictId }).ToList();
                    try
                    {
                        redisService.Add(keyAllProvince + "lWard", lWard);
                    }
                    catch (Exception)
                    { }
                }
                ComboboxResult comboboxResult;
                var lUsers = db.Users.ToList();
                User userItem;
                User userValidate;
                #region[đọc file excel]
                ExcelEngine excelEngine = new ExcelEngine();
                IWorkbook workbook = excelEngine.Excel.Workbooks.Open(pathFolderServer + fileName);
                var provinceId = string.Empty;
                var districtId = string.Empty;
                var wardId = string.Empty;
                string name = string.Empty;
                var username = string.Empty;
                var email = string.Empty;
                IWorksheet sheet = workbook.Worksheets[0];
                int countRow = sheet.Rows.Count(); int startRow = 3;
                bool isInsert = false;
                UserPermission userPermission;
                List<User> listInsert = new List<User>();
                List<UserPermission> listPermissionInsert = new List<UserPermission>();

                for (int indexRow = startRow; indexRow <= countRow; indexRow++)
                {
                    provinceId = sheet.Range[indexRow, 1].Value;
                    districtId = sheet.Range[indexRow, 2].Value;
                    wardId = sheet.Range[indexRow, 3].Value;
                    name = sheet.Range[indexRow, 4].Value;
                    username = sheet.Range[indexRow, 5].Value;
                    email = sheet.Range[indexRow, 6].Value;

                    userItem = lUsers.FirstOrDefault(u => u.UserName.ToLower().Equals(username.ToLower()));
                    if (userItem != null)
                    {
                        isInsert = false;
                    }
                    else
                    {
                        isInsert = true;
                        userItem = new User();
                        userItem.Id = Guid.NewGuid().ToString();
                        userItem.CreateDate = DateTime.Now;
                        userItem.UpdateDate = DateTime.Now;
                        userItem.Password = Guid.NewGuid().ToString();
                        userItem.PasswordHash = PasswordUtil.ComputeHash("123456" + userItem.Password);
                        userItem.GroupUserId = groupUser != null ? groupUser.Id : "";
                        userItem.CreateBy = createBy;
                        userItem.UpdateBy = createBy;
                        userItem.IsDisable = false;
                        userItem.AvatarPath = "";
                    }
                    //tỉnh
                    comboboxResult = lProvince.FirstOrDefault(u => u.Name.ToLower().Equals(provinceId.ToLower()));
                    if (comboboxResult == null)
                    {
                        throw new Exception("Tỉnh có tên sau không tồn tại: " + provinceId);
                    }
                    userItem.ProvinceId = comboboxResult.Id;
                    //huyện
                    if (string.IsNullOrEmpty(userItem.DistrictId))
                    {// chi xu ly khi chua có
                        comboboxResult = lDistrict.FirstOrDefault(u => u.PId.Equals(userItem.ProvinceId) && u.Name.ToLower().Equals(districtId.ToLower()));
                        if (comboboxResult == null)
                        {
                            throw new Exception("Huyện có tên sau không tồn tại: " + districtId);
                        }
                        userItem.DistrictId = comboboxResult.Id;
                    }
                    //xã
                    if (string.IsNullOrEmpty(userItem.WardId))
                    {// chi xu ly khi chua có
                        comboboxResult = lWard.FirstOrDefault(u => u.PId.Equals(userItem.DistrictId) && u.Name.ToLower().Equals(wardId.ToLower()));
                        if (comboboxResult == null)
                        {
                            throw new Exception("Xã có tên sau không tồn tại: " + wardId);
                        }
                        userItem.WardId = comboboxResult.Id;
                    }
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new Exception("Họ tên tài khoản không được để trống");
                    }
                    if (string.IsNullOrEmpty(username))
                    {
                        throw new Exception("Tài khoản không được để trống");
                    }
                    if (string.IsNullOrEmpty(email))
                    {
                        throw new Exception("Email không được để trống");
                    }
                    userItem.FullName = name;
                    userItem.Gender = (sheet.Range[indexRow, 7].Value + "").ToLower().Equals("nam") ? 1 : 0;
                    userItem.UserName = username;
                    userItem.Email = email;
                    userItem.Type = Constants.LevelTeacher;
                    //check trùng tài khoản trong list exel
                    userValidate = listInsert.FirstOrDefault(u => u.UserName.ToLower().Equals(username.ToLower()));
                    if (userValidate != null)
                    {
                        throw new Exception("Tài khoản '" + username + "' bị trùng");
                    }
                    if (isInsert)
                    {//thêm mới hs
                     // db.Users.Add(userItem);
                        listInsert.Add(userItem);
                        foreach (var item in listPermission)
                        {
                            userPermission = new UserPermission()
                            {
                                Id = Guid.NewGuid().ToString(),
                                UserId = userItem.Id,
                                PermissionId = item
                            };
                            listPermissionInsert.Add(userPermission);
                        }
                    }
                }
                #endregion
                db.Users.AddRange(listInsert);
                db.UserPermissions.AddRange(listPermissionInsert);
                db.SaveChanges();
                trans.Commit();
            }

        }
    }
}
