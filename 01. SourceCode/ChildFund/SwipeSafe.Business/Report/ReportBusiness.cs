
using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using SwipeSafe.Model.Model.CacheModel;
using SwipeSafe.Model.ProfileReport;
using SwipeSafe.Model.Repositories;
using SwipeSafe.Model.SearchCondition;
using SwipeSafe.Model.SearchResults;
using SwipeSafe.Model.SwipeSafeModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SwipeSafe.Business
{
    public class ReportBusiness
    {
        private ReportAppEntities db = new ReportAppEntities();
        public SearchResultObject<ReportSearchResult> SearchReport(ReportSearchCondition searchCondition)
        {
            SearchResultObject<ReportSearchResult> searchResult = new SearchResultObject<ReportSearchResult>();
            try
            {
                var listmodel = (from a in db.Reports.AsNoTracking()
                                 join b in db.Relationships.AsNoTracking() on a.Relationship equals b.Id into ab
                                 from ab1 in ab.DefaultIfEmpty()
                                 select new ReportSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Status = a.Status,
                                     Type = a.Type,
                                     CreateDate = a.CreateDate,
                                     Address = a.Address,
                                     FullAddress = a.FullAddress,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     Description = a.Description,
                                     Gender = a.Gender,
                                     Email = a.Email,
                                     Phone = a.Phone,
                                     Relationship = ab1 != null ? a.Relationship : "",
                                     Birthday = a.Birthday,
                                     CountChild = db.Children.Where(u => u.ReportId.Equals(a.Id)).Select(u => u.Id).Count(),
                                     CountPrisoner = db.Prisoners.Where(u => u.ReportId.Equals(a.Id)).Select(u => u.Id).Count(),
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.Phone))
                {
                    listmodel = listmodel.Where(r => r.Phone.ToLower().Contains(searchCondition.Phone.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.Email))
                {
                    listmodel = listmodel.Where(r => r.Email.ToLower().Contains(searchCondition.Email.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.Status))
                {
                    listmodel = listmodel.Where(r => r.Status.Equals(searchCondition.Status));
                }
                if (!string.IsNullOrEmpty(searchCondition.Type))
                {
                    listmodel = listmodel.Where(r => r.Type.Equals(searchCondition.Type));
                }
                if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(searchCondition.ProvinceId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(searchCondition.DistrictId));
                }
                if (!string.IsNullOrEmpty(searchCondition.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(searchCondition.WardId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                    listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                    listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                foreach (var item in searchResult.ListResult)
                {
                    item.StatusView = GenStatus(item.Status);
                    item.Type = item.Type.Equals("1") ? "Báo cáo ẩn danh" : "Báo cáo thường";
                    item.Gender = item.Gender != null ? (item.Gender.Equals("0") ? "Nữ" : "Nam") : "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public string GenStatus(string stt)
        {
            string rs = "";
            switch (stt)
            {
                case "0":
                    rs = "Báo cáo mới";
                    break;
                case "1":
                    rs = "Chuyển thành ca";
                    break;
                case "2":
                    rs = "Đã xử lý";
                    break;
                default:
                    break;
            }
            return rs;
        }
        public void DeleteReport(ReportSearchResult model)
        {
            var checkReport = db.Reports.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkReport == null)
            {
                throw new Exception("Báo cáo đã bị xóa bởi người dùng khác");
            }
            try
            {
                db.Reports.Remove(checkReport);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public ReportSearchResult Detail(string id)
        {
            ReportSearchResult model = new ReportSearchResult();
            try
            {
                model = (from a in db.Reports.AsNoTracking()
                         where a.Id.Equals(id)
                         select new ReportSearchResult()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             Status = a.Status,
                             Type = a.Type,
                             CreateDate = a.CreateDate,
                             Address = a.Address,
                             FullAddress = a.FullAddress,
                             WardId = a.WardId,
                             DistrictId = a.DistrictId,
                             ProvinceId = a.ProvinceId,
                             Description = a.Description,
                             Gender = a.Gender,
                             Email = a.Email,
                             Phone = a.Phone,
                             Relationship = a.Relationship,
                             Birthday = a.Birthday,
                             ListPrisonerSearchResult = (from ps in db.Prisoners
                                                         where ps.ReportId.Equals(id)
                                                         select new PrisonerSearchResult
                                                         {
                                                             Id = ps.Id,
                                                             Name = ps.Name,
                                                             Birthday = ps.Birthday,
                                                             Phone = ps.Phone,
                                                             Relationship = ps.Relationship,
                                                             Gender = ps.Gender.Equals("0") ? "Nữ" : "Nam",
                                                             Address = ps.FullAddress
                                                         }).ToList(),
                             ListChildSearchResult = (from ps in db.Children
                                                      where ps.ReportId.Equals(id)
                                                      select new ChildSearchResult
                                                      {
                                                          Id = ps.Id,
                                                          Name = ps.Name,
                                                          Age = ps.Age,
                                                          Birthday = ps.Birthday,
                                                          Level = ps.Level,
                                                          DateAction = ps.DateAction,
                                                          Gender = ps.Gender.Equals("0") ? "Nữ" : "Nam",
                                                          Address = ps.FullAddress,
                                                          FormAbuse = (from fb in db.ChildAbuses
                                                                       where fb.ChildId.Equals(ps.Id)
                                                                       select new ChildAbuseModel
                                                                       {
                                                                           Id = fb.Id,
                                                                           ChildId = fb.ChildId,
                                                                           AbuseId = fb.AbuseId,
                                                                           AbuseName = fb.AbuseName,
                                                                       }).ToList(),
                                                      }).ToList(),
                             ListFile = (from f in db.FileAttaches
                                         where f.ReportId.Equals(a.Id)
                                         orderby f.Type
                                         select new FileAttachSearchResult
                                         {
                                             Id = f.Id,
                                             Name = f.Name,
                                             Size = f.Size,
                                             Type = f.Type,
                                             Parth = f.Parth,
                                             ParthThumbnail = f.ParthThumbnail,
                                         }).ToList()
                         }).FirstOrDefault();
            }
            catch (Exception)
            { }
            if (model == null)
            {
                throw new Exception("Sự vụ đã bị xóa bởi người dùng khác");
            }
            model.Status = GenStatus(model.Status);
            model.Type = model.Type.Equals("1") ? "Báo cáo ẩn danh" : "Báo cáo thường";
            model.Gender = model.Gender != null ? (model.Gender.Equals("0") ? "Nữ" : "Nam") : "";
            return model;
        }

        public void AddReport(ReportModel model, HttpFileCollection httpFile)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    string IsImg = "0";
                    #region[Thông tin sự vụ và Người báo cáo]
                    var dateNow = DateTime.Now;
                    Report report = new Report();
                    report.Id = Guid.NewGuid().ToString();
                    report.Status = model.Status;
                    report.Type = model.Type;
                    report.Name = model.Name;
                    report.ProvinceId = model.ProvinceId;
                    report.DistrictId = model.DistrictId;
                    report.WardId = model.WardId;
                    report.Address = model.Address;
                    report.FullAddress = model.FullAddress;
                    report.Phone = model.Phone;
                    report.Email = model.Email;
                    report.Relationship = model.Relationship;
                    report.Birthday = model.Birthday;
                    report.Gender = model.Gender;
                    report.Description = model.Description;
                    report.CreateDate = dateNow;
                    db.Reports.Add(report);
                    #endregion

                    #region[Nghi pham]
                    if (model.ListPrisoner != null && model.ListPrisoner.Count > 0)
                    {
                        Prisoner prisoner;
                        foreach (var itemPrisoner in model.ListPrisoner)
                        {
                            prisoner = new Prisoner();
                            prisoner.Id = Guid.NewGuid().ToString();
                            prisoner.ReportId = report.Id;
                            prisoner.Name = itemPrisoner.Name;
                            prisoner.Gender = itemPrisoner.Gender;
                            prisoner.Birthday = itemPrisoner.Birthday;
                            prisoner.Phone = itemPrisoner.Phone;
                            prisoner.Relationship = itemPrisoner.Relationship;
                            prisoner.ProvinceId = itemPrisoner.ProvinceId;
                            prisoner.DistrictId = itemPrisoner.DistrictId;
                            prisoner.WardId = itemPrisoner.WardId;
                            prisoner.Address = itemPrisoner.Address;
                            prisoner.FullAddress = itemPrisoner.FullAddress;
                            db.Prisoners.Add(prisoner);
                        }
                    }
                    #endregion

                    #region[Trẻ bị xâm hại]
                    if (model.ListChild != null && model.ListChild.Count > 0)
                    {
                        Child child;
                        ChildAbuse childAbuse;
                        foreach (var itemChild in model.ListChild)
                        {
                            child = new Child();
                            child.Id = Guid.NewGuid().ToString();
                            child.ReportId = report.Id;
                            child.Age = itemChild.Age;
                            child.Name = itemChild.Name;
                            child.Gender = itemChild.Gender;
                            child.Birthday = itemChild.Birthday;
                            child.ProvinceId = itemChild.ProvinceId;
                            child.DistrictId = itemChild.DistrictId;
                            child.WardId = itemChild.WardId;
                            child.Address = itemChild.Address;
                            child.FullAddress = itemChild.FullAddress;
                            child.Level = itemChild.Level;
                            child.DateAction = itemChild.DateAction;
                            db.Children.Add(child);

                            foreach (var itemAbuse in itemChild.ListAbuse)
                            {
                                childAbuse = new ChildAbuse();
                                childAbuse.Id = Guid.NewGuid().ToString();
                                childAbuse.ChildId = child.Id;
                                childAbuse.AbuseId = itemAbuse.AbuseId;
                                childAbuse.AbuseName = itemAbuse.AbuseName;
                                db.ChildAbuses.Add(childAbuse);
                            }

                        }
                    }
                    #endregion

                    string imgTem = string.Empty;
                    #region[file]
                    if (httpFile.Count > 0)
                    {
                        FileAttach itemFileAttach;
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            IsImg = CheckIsImg(httpFile[i].FileName);
                            NTS.Storage.ImageResult imageResult = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadImageAsync(httpFile[i], Constants.FolderImageChildProfile);
                            }).Result;
                            if (imageResult != null)
                            {
                                if (i == 0)
                                {
                                    imgTem = imageResult.ImageThumbnail;
                                }
                                itemFileAttach = new FileAttach();
                                itemFileAttach.Id = Guid.NewGuid().ToString();
                                itemFileAttach.ReportId = report.Id;
                                itemFileAttach.Name = httpFile[i].FileName;
                                itemFileAttach.Size = httpFile[i].ContentLength;
                                itemFileAttach.Type = IsImg;
                                itemFileAttach.Parth = imageResult.ImageOrigin;
                                itemFileAttach.ParthThumbnail = imageResult.ImageThumbnail;
                                db.FileAttaches.Add(itemFileAttach);
                            }
                        }
                    }
                    #endregion


                    db.SaveChanges();
                    trans.Commit();
                    #region[lưu cache notify]
                    RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();

                    //địa phương duyệt- lấy tk trung ương
                    NotifyModel notifyModel;
                    string address = "";
                    string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                    TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                    notifyModel = new NotifyModel();
                    notifyModel.Image = imgTem;
                    notifyModel.Id = Guid.NewGuid().ToString();
                    notifyModel.Addres = address;
                    notifyModel.CreateDate = dateNow;
                    notifyModel.Status = Constants.NotViewNotification;
                    notifyModel.Title = "Ca báo cáo: trẻ <b>" + string.Join(";", model.ListChild.Select(u => u.Name).ToList()) + " báo cáo bị xâm hại " + "(" + dateNow.ToString("dd/MM/yyyy HH:mm") + ")";
                    notifyModel.Link = "/Report/Detail/"+ report.Id;
                    redisService.Add(cacheNotify + "Admin:" + notifyModel.Id, notifyModel, ts);
                    #endregion
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public string CheckIsImg(string name)
        {
            name = name.ToLower();
            var rs = "0";
            if (name.Contains(".mp4") || name.Contains(".avi") || name.Contains(".mov") || name.Contains(".vob") || name.Contains(".wmv"))
            {
                rs = "1";
            }
            return rs;
        }

        public List<NotifyModel> GetNotify(string userId)
        {
            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
            List<NotifyModel> lst = new List<NotifyModel>();
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                lst = redisService.GetContains(cacheNotify + userId + ":*");
            }
            catch (Exception)
            { }
            return lst;
        }

        public void DeleteNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + "Admin:" + id;
                redisService.Remove(key);
            }
            catch (Exception)
            {
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }
        public void TickNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + "Admin:" + id;
                NotifyModel notify = redisService.Get<NotifyModel>(key);
                notify.Status = Constants.ViewNotification;
                redisService.Replace(key, notify);
            }
            catch (Exception)
            {
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }
    }
}
