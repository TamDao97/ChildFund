using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common.Utils;
using NTS.Utils;
using NTS.Common;
using ChildProfiles.Model.ReportProfileModel;
using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using System.Web;
using NTS.Storage;
using ChildProfiles.Model.UserModels;
using Syncfusion.XlsIO;
using ChildProfiles.Business.Business;
using ChildProfiles.Model.Model.FliesLibrary;
using ChildProfiles.Model.Model.CacheModel;
using System.Configuration;
using NTS.Caching;
using ChildProfiles.Model.ChildProfileModels;
using System.IO;

namespace ChildProfiles.Business
{
    public class ReportProfileBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();

        public SearchResultObject<ReportProfileSearchResult> SearchReportProfileProvince(ReportProfileSearchCondition searchCondition)
        {
            SearchResultObject<ReportProfileSearchResult> searchResult = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 where !a.ProcessStatus.Equals(Constants.CreateNew)
                                 && a.IsDelete == Constants.IsUse
                                 join b in db.ChildProfiles.AsNoTracking() on a.ChildProfileId equals b.Id
                                 join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                                 join d in db.Users.AsNoTracking() on a.AreaApproverBy equals d.Id into ad
                                 from ad1 in ad.DefaultIfEmpty()
                                 orderby a.CreateDate descending
                                 select new ReportProfileSearchResult()
                                 {
                                     Id = a.Id,
                                     Content = a.Content,
                                     Status = a.ProcessStatus,
                                     ApproveDate = a.AreaApproveDate,
                                     ApproverId = ad1 != null ? ad1.Name : "",
                                     Name = b.Name,
                                     ChildCode = b.ChildCode,
                                     ProgramCode = b.ProgramCode,
                                     Handicap = b.Handicap.HasValue ? (bool)b.Handicap : false,
                                     CreateDate = a.CreateDate,
                                     CreateBy = c.Name,
                                     Description = a.Description,
                                     AreaApproverNotes = a.AreaApproverNotes,
                                     CountFile = db.AttachFileReports.Where(u => u.ReportProfileId.Equals(a.Id) && u.IsDelete != Constants.IsDelete).Select(u => u.Id).Count(),
                                     HealthHandicap = b.Health.Contains("\"Id\":\"04\",\"Check\":true") ? true : false,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()) || r.ChildCode.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(searchCondition.CreateBy.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.Status))
                {
                    listmodel = listmodel.Where(r => r.Status.Equals(searchCondition.Status));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    try
                    {
                        var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                        listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    try
                    {
                        var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                        listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                    }
                    catch (Exception)
                    {
                    }

                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                searchResult.PathFile = "";
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.SearchReportProfileProvince", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public SearchResultObject<ReportProfileSearchResult> SearchReportProfileWard(ReportProfileSearchCondition searchCondition)
        {
            string provinceId = (from r in db.Users.Where(r => r.Id.Equals(searchCondition.UserId))
                                 join x in db.AreaUsers on r.AreaUserId equals x.Id
                                 select x.ProvinceId).FirstOrDefault();
            SearchResultObject<ReportProfileSearchResult> searchResult = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 where //a.Status.Equals(Constants.ProfilesNew) &&
                                 a.IsDelete == Constants.IsUse
                                 join b in db.ChildProfiles.AsNoTracking().Where(r => r.ProvinceId.Equals(provinceId)) on a.ChildProfileId equals b.Id
                                 join c in db.Users.AsNoTracking() on a.CreateBy equals c.Id
                                 join d in db.Users.AsNoTracking() on a.AreaApproverBy equals d.Id into ad
                                 from ad1 in ad.DefaultIfEmpty()
                                 orderby a.CreateDate descending
                                 select new ReportProfileSearchResult()
                                 {
                                     Id = a.Id,
                                     Content = a.Content,
                                     Status = a.ProcessStatus,
                                     ApproveDate = a.AreaApproveDate,
                                     ApproverId = ad1 != null ? ad1.Name : "",
                                     Name = b.Name,
                                     ChildCode = b.ChildCode,
                                     ProgramCode = b.ProgramCode,
                                     CreateDate = a.CreateDate,
                                     CreateBy = c.Name,
                                     CountFile = db.AttachFileReports.Where(u => u.ReportProfileId.Equals(a.Id) && u.IsDelete != Constants.IsDelete).Select(u => u.Id).Count(),
                                     Description = a.Description,
                                     AreaApproverNotes = a.AreaApproverNotes,
                                     HealthHandicap = b.Health.Contains("\"Id\":\"04\",\"Check\":true") ? true : false,
                                     Handicap = b.Handicap.HasValue ? (bool)b.Handicap : false,

                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()) || r.ChildCode.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(searchCondition.CreateBy.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.Status))
                {
                    listmodel = listmodel.Where(r => r.Status.Equals(searchCondition.Status));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    try
                    {
                        var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                        listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    try
                    {
                        var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                        listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                    }
                    catch (Exception)
                    { }

                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                searchResult.PathFile = "";
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.SearchReportProfileWard", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public void DeleteReportProfile(ReportProfile model)
        {
            var checkChild = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("báo cáo đã bị xóa bởi người dùng khác");
            }
            try
            {
                checkChild.IsDelete = Constants.IsDelete;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.DeleteReportProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public void ConfimReportProfile(ReportProfile model)
        {
            var checkChild = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("Báo cáo đã bị xóa bởi người dùng khác");
            }
            try
            {
                if (checkChild.ProcessStatus.Equals(Constants.CreateNew))
                {
                    checkChild.ProcessStatus = Constants.ApproverArea;
                    checkChild.AreaApproverNotes = model.AreaApproverNotes;
                    #region[lưu cache notify]
                    var child = (from a in db.ChildProfiles.AsNoTracking()
                                 where a.Id.Equals(checkChild.ChildProfileId)
                                 select new { ChildCode = a.ChildCode, Name = a.Name, WardId = a.WardId, DistrictId = a.DistrictId, ProvinceId = a.ProvinceId, Img = a.ImageThumbnailPath }).FirstOrDefault();

                    RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                    var addressModel = (from a in db.Provinces.AsNoTracking()
                                        join b in db.Districts.AsNoTracking() on a.Id equals b.ProvinceId
                                        join c in db.Wards.AsNoTracking() on b.Id equals c.DistrictId
                                        where c.Id.Equals(child.WardId)
                                        select new
                                        {
                                            ProvinceName = a.Name,
                                            DistrictName = b.Name,
                                            WardName = c.Name
                                        }).FirstOrDefault();
                    var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.CreateBy);
                    //địa phương duyệt- lấy tk trung ương
                    var userNotify = db.Users.Where(u => u.UserLever.Equals(Constants.LevelOffice)).ToList();
                    NotifyModel notifyModel;
                    var dateNow = DateTime.Now;
                    string address = "";
                    if (addressModel != null)
                    {
                        address = addressModel.WardName + ", " + addressModel.DistrictName + ", " + addressModel.ProvinceName;
                    }

                    string isSendEmail = ConfigurationManager.AppSettings["IsSendEmail"];
                    if (isSendEmail.ToLower().Equals("true"))
                    {
                        string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                        TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                        foreach (var item in userNotify)
                        {
                            notifyModel = new NotifyModel();
                            notifyModel.Image = child.Img;
                            notifyModel.Id = Guid.NewGuid().ToString();
                            notifyModel.Addres = address;
                            notifyModel.CreateDate = dateNow;
                            notifyModel.Status = Constants.NotViewNotification;
                            notifyModel.Title = "Báo cáo thay đổi thông tin trẻ: <b>" + child.ChildCode + "-" + child.Name + "</b> từ cán bộ <b>" + userInfo.Name + "</b>";
                            notifyModel.Link = "/ReportProfile/ReportProvince";
                            redisService.Add(cacheNotify + item.Id + ":" + notifyModel.Id, notifyModel, ts);
                        }
                    }
                    #endregion
                }
                else if (checkChild.ProcessStatus.Equals(Constants.ApproverArea))
                {
                    checkChild.ProcessStatus = Constants.ApproveOffice;
                    checkChild.AreaApproverNotes = model.AreaApproverNotes;
                }
                else if (checkChild.ProcessStatus.Equals(Constants.ApproveOffice))
                {
                    checkChild.ProcessStatus = Constants.ApproverArea;
                    checkChild.AreaApproverNotes = model.AreaApproverNotes;
                }
                checkChild.AreaApproveDate = DateTime.Now;
                checkChild.AreaApproverBy = model.CreateBy;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.ConfimReportProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public List<string> GetFileDownload(ReportProfile model)
        {
            List<string> path = new List<string>();
            var checkChild = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("Báo cáo đã bị xóa bởi người dùng khác");
            }
            try
            {
                ImageLibraryDA imageLibraryDA = new ImageLibraryDA();
                var urls = db.AttachFileReports.Where(u => u.ReportProfileId.Equals(model.Id) && u.IsDelete != Constants.IsDelete).Select(e => new AttachmentImageModel
                {
                    ImagePath = e.Path,
                    Name = e.Name
                }).ToList();

                foreach (var item in urls)
                {
                    //string folder = "~/fileUpload/FileUser/";
                    //imageLibraryDA.DownLoadSingleFileToServer(item, folder);

                    //path.Add("/fileUpload/FileUser/" + item.Name);
                    path.Add(item.ImagePath);
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.GetFileDownload", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return path;
        }

        public void UploadAttachFile(HttpFileCollection httpFile, UploadAttachFileModel model, string createBy)
        {
            var checkChild = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("Báo cáo đã bị xóa bởi người dùng khác");
            }
            try
            {
                if (model.ListIdRemote == null)
                {
                    model.ListIdRemote = new List<string>();
                }
                var fileOld = db.AttachFileReports.Where(u => model.ListIdRemote.Contains(u.Id)).ToList();
                foreach (var item in fileOld)
                {
                    item.IsDelete = Constants.IsDelete;
                }
                //Upload file lên cloud
                if (httpFile.Count > 0)
                {
                    var dateNow = DateTime.Now;
                    List<string> listFileKey = httpFile.AllKeys.ToList();
                    AttachFileReport profileAttachmentModel;
                    for (int i = 0; i < httpFile.Count; i++)
                    {
                        profileAttachmentModel = new AttachFileReport();
                        profileAttachmentModel.Id = Guid.NewGuid().ToString();
                        profileAttachmentModel.UploadDate = dateNow;
                        profileAttachmentModel.UploadBy = createBy;
                        profileAttachmentModel.ReportProfileId = model.Id;
                        profileAttachmentModel.IsDelete = Constants.IsUse;
                        profileAttachmentModel.Name = httpFile[i].FileName;
                        profileAttachmentModel.Path = Task.Run(async () =>
                        {
                            return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                        }).Result;
                        db.AttachFileReports.Add(profileAttachmentModel);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.UploadAttachFile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public List<AttachmentImageModel> GetAttachFile(string id)
        {
            List<AttachmentImageModel> lst = new List<AttachmentImageModel>();
            try
            {
                lst = db.AttachFileReports.Where(u => u.ReportProfileId.Equals(id) && u.IsDelete != Constants.IsDelete).Select(e => new AttachmentImageModel
                {
                    ImagePath = e.Path,
                    Name = e.Name,
                    Id = e.Id
                }).ToList();
            }
            catch (Exception ex)
            { LogUtils.ExceptionLog("ReportProfileBusiness.GetAttachFile", ex.Message, id); }
            return lst;
        }

        /// <summary>
        /// Them moi bao cao tinh trang tre
        /// </summary>
        /// <param name="model"></param>
        public bool AddReportProfile(ReportProfilesModel model, HttpFileCollection httpFile)
        {
            try
            {
                DateTime range = DateTime.Now.AddMinutes(-1);

                bool isExisted = (from r in db.ReportProfiles
                                  where r.ChildProfileId.Equals(model.ChildProfileId) && r.CreateDate > range
                                  select r).Any();

                if (isExisted)
                {
                    return false;
                }

                ReportProfile reportProfile = new ReportProfile();
                reportProfile.Id = Guid.NewGuid().ToString();
                reportProfile.ChildProfileId = model.ChildProfileId;
                reportProfile.ProcessStatus = Constants.CreateNew;
                reportProfile.Content = model.Content;
                reportProfile.IsDelete = Constants.IsUse;
                reportProfile.Description = model.Description;
                reportProfile.CreateDate = DateTime.Now;
                reportProfile.CreateBy = model.CreateBy;
                reportProfile.UpdateDate = DateTime.Now;
                reportProfile.UpdateBy = model.UpdateBy;
                db.ReportProfiles.Add(reportProfile);
                db.SaveChanges();

                if (httpFile.Count > 0)
                {
                    for (int index = 0; index < httpFile.Count; index++)
                    {
                        string fileName = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + ".jpg";
                        String fileResult = Task.Run(async () =>
                        {
                            return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[index], fileName, Constants.FolderReportProfile);
                        }).Result;

                        if (fileResult != null && !String.IsNullOrEmpty(fileResult))
                        {
                            AttachFileReport attachFileReport = new AttachFileReport
                            {
                                Id = Guid.NewGuid().ToString(),
                                Path = fileResult,
                                Name = fileName,
                                UploadDate = DateTime.Now,
                                UploadBy = model.CreateBy,
                                ReportProfileId = reportProfile.Id
                            };
                            db.AttachFileReports.Add(attachFileReport);
                        }
                    }
                }
                db.SaveChanges();

                #region[lưu cache notify]
                var child = (from a in db.ChildProfiles.AsNoTracking()
                             where a.Id.Equals(model.ChildProfileId)
                             select new { ChildCode = a.ChildCode, Name = a.Name, WardId = a.WardId, DistrictId = a.DistrictId, ProvinceId = a.ProvinceId, Img = a.ImageThumbnailPath }).FirstOrDefault();

                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var addressModel = (from a in db.Provinces.AsNoTracking()
                                    join b in db.Districts.AsNoTracking() on a.Id equals b.ProvinceId
                                    join c in db.Wards.AsNoTracking() on b.Id equals c.DistrictId
                                    where c.Id.Equals(child.WardId)
                                    select new
                                    {
                                        ProvinceName = a.Name,
                                        DistrictName = b.Name,
                                        WardName = c.Name
                                    }).FirstOrDefault();
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.CreateBy);
                //địa phương duyệt- lấy tk trung ương
                List<User> userNotify = new List<User>();
                try
                {
                    userNotify = (from a in db.Users.AsNoTracking()
                                  join b in db.AreaUsers.AsNoTracking() on a.AreaUserId equals b.Id
                                  where b.ProvinceId.Equals(child.ProvinceId)
                                  join c in db.AreaDistricts.AsNoTracking() on a.AreaDistrictId equals c.Id into ac
                                  from ac1 in ac.DefaultIfEmpty()
                                  where (string.IsNullOrEmpty(a.AreaDistrictId) || (ac1 != null && ac1.DistrictId.Equals(child.DistrictId)))
                                  select a).ToList();
                }
                catch (Exception)
                { }

                NotifyModel notifyModel;
                var dateNow = DateTime.Now;
                string address = "";
                if (addressModel != null)
                {
                    address = addressModel.WardName + ", " + addressModel.DistrictName + ", " + addressModel.ProvinceName;
                }

                string isSendEmail = ConfigurationManager.AppSettings["IsSendEmail"];
                if (isSendEmail.ToLower().Equals("true"))
                {
                    TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                    string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                    foreach (var item in userNotify)
                    {
                        notifyModel = new NotifyModel();
                        notifyModel.Image = child.Img;
                        notifyModel.Id = Guid.NewGuid().ToString();
                        notifyModel.Addres = address;
                        notifyModel.CreateDate = DateTime.Now;
                        notifyModel.Status = Constants.NotViewNotification;
                        notifyModel.Title = "Báo cáo thay đổi thông tin trẻ: <b>" + child.ChildCode + "-" + child.Name + "</b> từ cán bộ <b>" + userInfo.Name + "</b> ";
                        notifyModel.Link = "/ReportProfile/ReportWard/";
                        redisService.Add(cacheNotify + item.Id + ":" + notifyModel.Id, notifyModel, ts);
                    }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.AddReportProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public ChildProfileModel DetailProfile(string id)
        {
            var villages = db.Villages.AsNoTracking();
            ChildProfileModel childProfiles = new ChildProfileModel();
            try
            {
                childProfiles = (from a in db.ReportProfiles.AsNoTracking()
                                 where a.Id.Equals(id)
                                 join b in db.ChildProfiles.AsNoTracking() on a.ChildProfileId equals b.Id
                                 join c in db.Ethnics.AsNoTracking() on b.EthnicId equals c.Id
                                 join s in db.Schools.AsNoTracking() on b.SchoolId equals s.Id into asc
                                 from asc1 in asc.DefaultIfEmpty()
                                 join d in db.Religions.AsNoTracking() on b.ReligionId equals d.Id
                                 join e in db.Provinces.AsNoTracking() on b.ProvinceId equals e.Id
                                 join f in db.Districts.AsNoTracking() on b.DistrictId equals f.Id
                                 join g in db.Wards.AsNoTracking() on b.WardId equals g.Id
                                 join h in db.Villages.AsNoTracking() on b.Address equals h.Id into gh
                                 from gh1 in gh.DefaultIfEmpty()
                                 select new ChildProfileModel
                                 {
                                     ReportProfileId = a.Id,
                                     Name = b.Name,
                                     ReligionId = d.Name,
                                     EthnicId = c.Name,
                                     ProcessStatus = b.ProcessStatus,
                                     ProgramCode = b.ProgramCode,
                                     SchoolId = asc1.SchoolName,
                                     ChildCode = b.ChildCode,
                                     ProvinceId = e.Name,
                                     DistrictId = f.Name,
                                     WardId = g.Name + "," +
                                     "" + f.Name,
                                     VillageName = gh1 != null ? gh1.Name : b.Address,
                                     FamilyMember = b.FamilyMember,
                                     Address = gh1.Name,
                                     EmployeeName = b.EmployeeName,
                                     InfoDate = b.InfoDate,
                                     DateOfBirth = b.DateOfBirth,
                                     Gender = b.Gender,
                                     NickName = b.NickName,
                                     LeaningStatus = b.LeaningStatus,
                                     ClassInfo = b.ClassInfo,
                                     Content = a.Content,
                                     ImagePath = b.ImagePath,
                                     ImageThumbnailPath = b.ImageThumbnailPath,
                                     Description = a.Description,
                                     AreaApproverNotes = a.AreaApproverNotes,

                                 }).FirstOrDefault();
                if (childProfiles == null)
                {
                    throw new Exception("Báo cáo đã bị xóa bởi người dùng khách");
                }

            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.DetailProfile", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return childProfiles;
        }
    }
}
