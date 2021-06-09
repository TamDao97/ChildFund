using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using SwipeSafe.Model.Model.CacheModel;
using SwipeSafe.Model.ProfileReport;
using SwipeSafe.Model.Repositories;
using SwipeSafe.Model.SearchResults;
using SwipeSafe.Model.SwipeSafeModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SwipeSafe.Business.ProfileReport
{
    public class ProfileReportBusiness
    {
        private ReportAppEntities db = new ReportAppEntities();

        /// <summary>
        /// Tìm kiếm ca
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        public SearchResultObject<ProfileChildSearchResult> SearchProfileChild(ProfileChildSearchCondition searchCondition)
        {
            SearchResultObject<ProfileChildSearchResult> searchResult = new SearchResultObject<ProfileChildSearchResult>();
            try
            {
                var listmodel = (from a in db.ProfileChilds.AsNoTracking()
                                 join b in db.ProfileChildAbuses.AsNoTracking() on a.Id equals b.ProfileChildId into ab
                                 select new ProfileChildSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.ChildName,
                                     ProcessingStatus = a.ProcessingStatus,
                                     ReceptionDate = a.ReceptionDate,
                                     Address = a.FullAddress,
                                     FullAddress = a.FullAddress,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     Gender = a.Gender.HasValue && a.Gender == 1 ? "Nam" : (a.Gender.HasValue && a.Gender == 0 ? "Nữ" : "Không xác định"),
                                     Age = a.Age,
                                     Birthday = a.ChildBirthdate,

                                     ProviderName = a.ProviderName,
                                     ProviderPhone = a.ProviderPhone,
                                     ProviderNote = a.ProviderNote,
                                     FormAbuse = (from b in db.ProfileChildAbuses
                                                  where b.ProfileChildId.Equals(a.Id)
                                                  select new ProfileChildAbuseModel
                                                  {
                                                      AbuseId = b.AbuseId,
                                                      AbuseName = b.AbuseName,
                                                      Id = b.Id
                                                  }).ToList()
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (searchCondition.ProcessingStatus.HasValue)
                {
                    listmodel = listmodel.Where(r => r.ProcessingStatus.Equals(searchCondition.ProcessingStatus));
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
                    listmodel = listmodel.Where(r => r.ReceptionDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                    listmodel = listmodel.Where(r => r.ReceptionDate <= dateTo);
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                foreach (var item in searchResult.ListResult)
                {
                    item.ProcessingName = GenStatus(item.ProcessingStatus);
                    item.FormAbuseName = string.Join(", ", item.FormAbuse.Select(r => r.AbuseName).ToList());
                    item.FormAbuse = null;
                    item.ReceptionDateView = item.ReceptionDate.HasValue ? item.ReceptionDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        /// <summary>
        /// Trạng thái xử lý
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GenStatus(int status)
        {
            string rs = "";
            switch (status)
            {
                case 0:
                    rs = "Chưa xử lý";
                    break;
                case 1:
                    rs = "Đang điều tra xác minh";
                    break;
                case 2:
                    rs = "Đã xử lý";
                    break;
                case 3:
                    rs = "Cần theo dõi thêm";
                    break;
                case 4:
                    rs = "Đóng sự vụ";
                    break;
                default:
                    break;
            }
            return rs;
        }

        /// <summary>
        /// Xóa ca
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProfileChild(string id)
        {
            var checkProfile = db.ProfileChilds.FirstOrDefault(u => u.Id.Equals(id));
            if (checkProfile == null)
            {
                throw new Exception("Ca đã bị xóa bởi người dùng khác");
            }
            try
            {
                db.ProfileChilds.Remove(checkProfile);

                var profilePrisoners = db.ProfilePrisoners.Where(u => u.ProfileChildId.Equals(id)).ToList();
                if (profilePrisoners != null)
                    db.ProfilePrisoners.RemoveRange(profilePrisoners);

                var profileFileAttaches = db.ProfileFileAttaches.Where(u => u.ProfileChildId.Equals(id)).ToList();
                if (profileFileAttaches != null)
                    db.ProfileFileAttaches.RemoveRange(profileFileAttaches);

                var processingContents = db.ProcessingContents.Where(u => u.ProfileChildId.Equals(id)).ToList();
                if (processingContents != null)
                    db.ProcessingContents.RemoveRange(processingContents);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        /// <summary>
        /// Chi tiết ca
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProfileChildSearchResult DetailProfileChild(string id)
        {
            ProfileChildSearchResult model = new ProfileChildSearchResult();
            var data = db.ProfileChilds.FirstOrDefault(u => u.Id.Equals(id));
            try
            {
                model = new ProfileChildSearchResult()
                {
                    Id = data.Id,
                    Name = data.ChildName,
                    ProcessingStatus = data.ProcessingStatus,
                    ReceptionDate = data.ReceptionDate,
                    Address = data.FullAddress,
                    FullAddress = data.FullAddress,
                    WardId = data.WardId,
                    DistrictId = data.DistrictId,
                    ProvinceId = data.ProvinceId,
                    Gender = data.Gender.HasValue && data.Gender == 1 ? "Nam" : (data.Gender.HasValue && data.Gender == 0 ? "Nữ" : "Không xác định"),
                    Age = data.Age,
                    Birthday = data.ChildBirthdate,
                    ProviderName = data.ProviderName,
                    ProviderPhone = data.ProviderPhone,
                    ProviderNote = data.ProviderNote,
                    FormAbuse = (from b in db.ProfileChildAbuses
                                 where b.ProfileChildId.Equals(id)
                                 select new ProfileChildAbuseModel
                                 {
                                     AbuseId = b.AbuseId,
                                     AbuseName = b.AbuseName,
                                     Id = b.Id
                                 }).ToList()
                };
            }
            catch (Exception ex)
            { }
            if (data == null)
            {
                throw new Exception("Sự vụ đã bị xóa bởi người dùng khác");
            }
            model.ProcessingName = GenStatus(model.ProcessingStatus);
            //model.ListProcessingContent = (from p in db.ProcessingContents.AsNoTracking()
            //                               where p.ProfileChildId.Equals(id)
            //                               join q in db.Users.AsNoTracking() on p.ProcessingBy equals q.Id into pq
            //                               from pq1 in pq.DefaultIfEmpty()
            //                               select new ProcessingContentModel
            //                               {
            //                                   Id = p.Id,
            //                                   Content = p.Content,
            //                                   ProcessingDate = p.ProcessingDate,
            //                                   ProcessingBy = pq1 != null ? pq1.FullName : ""
            //                               }).ToList();
            model.FormAbuseName = string.Join(", ", model.FormAbuse.Select(r => r.AbuseName).ToList());
            return model;
        }
        public ProfileChildSearchResult GetContent(string id)
        {
            ProfileChildSearchResult model = new ProfileChildSearchResult();
            try
            {
                model.ListProcessingContent = (from p in db.ProcessingContents.AsNoTracking()
                                               where p.ProfileChildId.Equals(id)
                                               join q in db.Users.AsNoTracking() on p.ProcessingBy equals q.Id into pq
                                               from pq1 in pq.DefaultIfEmpty()
                                               select new ProcessingContentModel
                                               {
                                                   Id = p.Id,
                                                   Content = p.Content,
                                                   ProcessingDate = p.ProcessingDate,
                                                   ProcessingBy = pq1 != null ? pq1.FullName : ""
                                               }).OrderBy(u => u.ProcessingDate).ToList();
            }
            catch (Exception ex)
            { }
            return model;
        }
        /// <summary>
        /// Thêm mới ca
        /// </summary>
        /// <param name="model"></param>
        /// <param name="httpFile"></param>
        public void AddProfileChild(ProfileChildModel model, HttpFileCollection httpFile)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    string IsImg = "0";
                    #region[Thông tin ca]
                    var dateNow = DateTime.Now;
                    ProfileChild profileChild = new ProfileChild();
                    profileChild.Id = Guid.NewGuid().ToString();
                    profileChild.InformationSources = model.InformationSources;
                    profileChild.ReceptionDate = model.ReceptionDate;
                    profileChild.ChildName = model.ChildName;
                    profileChild.ChildBirthdate = model.ChildBirthdate;
                    profileChild.Gender = model.Gender;
                    profileChild.Age = model.Age;
                    profileChild.CaseLocation = model.CaseLocation;
                    profileChild.WardId = model.WardId;
                    profileChild.DistrictId = model.DistrictId;
                    profileChild.ProvinceId = model.ProvinceId;
                    profileChild.FullAddress = model.FullAddress;
                    profileChild.CurrentHealth = model.CurrentHealth;
                    profileChild.SequelGuess = model.SequelGuess;
                    profileChild.FatherName = model.FatherName;
                    profileChild.FatherAge = model.FatherAge;
                    profileChild.FatherJob = model.FatherJob;
                    profileChild.MotherName = model.MotherName;
                    profileChild.MotherAge = model.MotherAge;
                    profileChild.MotherJob = model.MotherJob;
                    profileChild.FamilySituation = model.FamilySituation;
                    profileChild.PeopleCare = model.PeopleCare;
                    profileChild.Support = model.Support;
                    profileChild.ProviderName = model.ProviderName;
                    profileChild.ProviderPhone = model.ProviderPhone;
                    profileChild.ProviderAddress = model.ProviderAddress;
                    profileChild.ProviderNote = model.ProviderNote;
                    profileChild.ProcessingStatus = 0;
                    profileChild.SeverityLevel = model.SeverityLevel;
                    profileChild.CreateBy = model.CreateBy;
                    profileChild.CreateDate = dateNow;
                    profileChild.UpdateBy = model.UpdateBy;
                    profileChild.UpdateDate = dateNow;
                    db.ProfileChilds.Add(profileChild);

                    if (model.ListAbuse != null && model.ListAbuse.Count > 0)
                    {
                        ProfileChildAbuse childAbuse;
                        foreach (var itemAbuse in model.ListAbuse)
                        {
                            childAbuse = new ProfileChildAbuse();
                            childAbuse.Id = Guid.NewGuid().ToString();
                            childAbuse.ProfileChildId = profileChild.Id;
                            childAbuse.AbuseId = itemAbuse.AbuseId;
                            childAbuse.AbuseName = itemAbuse.AbuseName;
                            db.ProfileChildAbuses.Add(childAbuse);
                        }
                    }
                    #endregion

                    string imgTem = string.Empty;
                    #region[file]
                    if (httpFile.Count > 0)
                    {
                        ProfileFileAttach itemFileAttach;
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
                                itemFileAttach = new ProfileFileAttach();
                                itemFileAttach.Id = Guid.NewGuid().ToString();
                                itemFileAttach.ProfileChildId = profileChild.Id;
                                itemFileAttach.Name = httpFile[i].FileName;
                                itemFileAttach.Size = httpFile[i].ContentLength;
                                itemFileAttach.Type = IsImg;
                                itemFileAttach.Parth = imageResult.ImageOrigin;
                                itemFileAttach.ParthThumbnail = imageResult.ImageThumbnail;
                                db.ProfileFileAttaches.Add(itemFileAttach);
                            }
                        }
                    }
                    #endregion

                    db.SaveChanges();
                    trans.Commit();

                    #region[lưu cache notify]
                    RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();

                    List<string> listUserId = GetListUserIdByNotify(profileChild.ProvinceId, profileChild.DistrictId, profileChild.WardId, profileChild.CreateBy);

                    foreach (string userId in listUserId)
                    {
                        NotifyModel notifyModel;
                        string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                        TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                        notifyModel = new NotifyModel();
                        notifyModel.Image = imgTem;
                        notifyModel.ChildProfileId = profileChild.Id;
                        notifyModel.Id = Guid.NewGuid().ToString();
                        notifyModel.Addres = profileChild.FullAddress;
                        notifyModel.CreateDate = dateNow;
                        notifyModel.Status = Constants.NotViewNotification;
                        notifyModel.Title = "Tạo mới ca trẻ: " + profileChild.ChildName + " báo cáo bị xâm hại " + "(" + dateNow.ToString("dd/MM/yyyy HH:mm") + ")";
                        notifyModel.Link = "/ProfileReport/Detail/" + profileChild.Id;
                        redisService.Add(cacheNotify + userId + ":" + notifyModel.Id, notifyModel, ts);
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Chuyển báo cáo thành ca sự vụ
        /// </summary>
        /// <param name="id"></param>
        public List<Child> MoveReportToProfile(string id)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    #region[Thông tin sự vụ và Người báo cáo]
                    var itemReport = db.Reports.Where(r => r.Id.Equals(id)).FirstOrDefault();
                    if (itemReport == null)
                    {
                        throw new Exception("Báo cáo đã bị xóa bởi người dùng khác");
                    }

                    itemReport.Status = "1";
                    #endregion

                    var listPrisoner = db.Prisoners.Where(r => r.ReportId.Equals(id)).ToList();
                    var listFileAttaches = db.FileAttaches.Where(r => r.ReportId.Equals(id)).ToList();

                    #region[Trẻ bị xâm hại]
                    var listChild = db.Children.Where(r => r.ReportId.Equals(id)).ToList();
                    if (listChild != null && listChild.Count() > 0)
                    {
                        ProfileChild profileChild;
                        ProfileChildAbuse childAbuse;
                        foreach (var itemChild in listChild)
                        {
                            profileChild = new ProfileChild();
                            profileChild.Id = Guid.NewGuid().ToString();
                            profileChild.ReportId = itemReport.Id;
                            profileChild.InformationSources = 2;
                            profileChild.ReceptionDate = itemChild.DateAction;
                            profileChild.ChildName = itemChild.Name;
                            profileChild.ChildBirthdate = itemChild.Birthday;
                            profileChild.Gender = !string.IsNullOrEmpty(itemChild.Gender) ? int.Parse(itemChild.Gender) : default(int);
                            profileChild.Age = itemChild.Age;
                            profileChild.CaseLocation = itemChild.Address;
                            profileChild.WardId = itemChild.WardId;
                            profileChild.DistrictId = itemChild.DistrictId;
                            profileChild.ProvinceId = itemChild.ProvinceId;
                            profileChild.FullAddress = itemChild.FullAddress;
                            profileChild.SeverityLevel = !string.IsNullOrEmpty(itemChild.Level) ? int.Parse(itemChild.Level) : default(int);

                            profileChild.ProviderName = itemReport.Name;
                            profileChild.ProviderPhone = itemReport.Phone;
                            profileChild.ProviderAddress = itemReport.FullAddress;
                            profileChild.ProviderNote = itemReport.Description;
                            profileChild.ProcessingStatus = 0;

                            db.ProfileChilds.Add(profileChild);

                            var listChildAbuses = db.ChildAbuses.Where(r => r.ChildId.Equals(itemChild.Id)).ToList();
                            foreach (var itemAbuse in listChildAbuses)
                            {
                                childAbuse = new ProfileChildAbuse();
                                childAbuse.Id = Guid.NewGuid().ToString();
                                childAbuse.ProfileChildId = profileChild.Id;
                                childAbuse.AbuseId = itemAbuse.AbuseId;
                                childAbuse.AbuseName = itemAbuse.AbuseName;
                                db.ProfileChildAbuses.Add(childAbuse);
                            }

                            if (listPrisoner != null && listPrisoner.Count > 0)
                            {
                                ProfilePrisoner prisoner;
                                foreach (var itemPrisoner in listPrisoner)
                                {
                                    prisoner = new ProfilePrisoner();
                                    prisoner.Id = Guid.NewGuid().ToString();
                                    prisoner.ProfileChildId = profileChild.Id;
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
                                    db.ProfilePrisoners.Add(prisoner);
                                }
                            }

                            if (listFileAttaches != null && listFileAttaches.Count > 0)
                            {
                                ProfileFileAttach itemFileAttach;
                                foreach (var itemFile in listFileAttaches)
                                {
                                    itemFileAttach = new ProfileFileAttach();
                                    itemFileAttach.Id = Guid.NewGuid().ToString();
                                    itemFileAttach.ProfileChildId = profileChild.Id;
                                    itemFileAttach.Name = itemFile.Name;
                                    itemFileAttach.Size = itemFile.Size;
                                    itemFileAttach.Type = itemFile.Type;
                                    itemFileAttach.Parth = itemFile.Parth;
                                    itemFileAttach.ParthThumbnail = itemFile.ParthThumbnail;
                                    db.ProfileFileAttaches.Add(itemFileAttach);
                                }
                            }

                            #region[lưu cache notify]
                            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();

                            List<string> listUserId = GetListUserIdByNotify(profileChild.ProvinceId, profileChild.DistrictId, profileChild.WardId, profileChild.CreateBy);

                            NotifyModel notifyModel;
                            string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                            foreach (string userId in listUserId)
                            {
                                TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                                notifyModel = new NotifyModel();
                                notifyModel.ChildProfileId = profileChild.Id;
                                notifyModel.Id = Guid.NewGuid().ToString();
                                notifyModel.Addres = profileChild.FullAddress;
                                notifyModel.CreateDate = DateTime.Now;
                                notifyModel.Status = Constants.NotViewNotification;
                                notifyModel.Title = "Tạo mới ca trẻ: " + profileChild.ChildName + " báo cáo bị xâm hại " + "(" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ")";
                                notifyModel.Link = "/ProfileReport/Detail" + profileChild.Id;
                                redisService.Add(cacheNotify + userId + ":" + notifyModel.Id, notifyModel, ts);
                            }
                            #endregion
                        }
                    }
                    #endregion

                    db.SaveChanges();
                    trans.Commit();

                    return listChild;
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
            return lst.OrderByDescending(r => r.CreateDate).ToList();
        }

        public int CountNotify(string userId)
        {
            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                List<NotifyModel> lst = redisService.GetContains(cacheNotify + userId + ":*");
                return lst.Where(r => r.Status.Equals(Constants.NotViewNotification)).Count();
            }
            catch (Exception)
            { }
            return 0;
        }

        public void DeleteNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + System.Web.HttpContext.Current.User.Identity.Name + ":" + id;
                redisService.Remove(key);
            }
            catch (Exception)
            {
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }
        public void TickNotify(string userId, string notifyId)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = $"{cacheNotify}{userId}:{notifyId}";
                NotifyModel notify = redisService.Get<NotifyModel>(key);
                notify.Status = Constants.ViewNotification;
                redisService.Replace(key, notify);
            }
            catch (Exception)
            {
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }

        /// <summary>
        /// Chi tiết nôi dung báo cáo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProcessingContentModel DetailProcessingContent(string id)
        {
            ProcessingContentModel model = new ProcessingContentModel();
            try
            {
                model = (from a in db.ProcessingContents.AsNoTracking()
                         where a.Id.Equals(id)
                         select new ProcessingContentModel()
                         {
                             Id = a.Id,
                             Content = a.Content,
                             ProcessingDate = a.ProcessingDate,
                             ProcessingBy = a.ProcessingBy
                         }).FirstOrDefault();
            }
            catch (Exception)
            { }
            return model;
        }

        /// <summary>
        /// Thêm mới nội dung báo cáo
        /// </summary>
        /// <param name="model"></param>
        public void AddProcessingContent(ProcessingContentModel model)
        {
            try
            {
                ProcessingContent processingContent = new ProcessingContent()
                {
                    Id = Guid.NewGuid().ToString(),
                    Content = model.Content,
                    ProfileChildId = model.ProfileChildId,
                    ProcessingDate = DateTime.Now,
                    ProcessingBy = model.ProcessingBy
                };
                db.ProcessingContents.Add(processingContent);
                db.SaveChanges();
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Chi tiết nôi dung báo cáo theo id ca
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChildProcessingContentModel GetProcessingContentByProfile(string id)
        {
            ChildProcessingContentModel childProcessingContentModel = new ChildProcessingContentModel();
            try
            {
                var child = db.ProfileChilds.Where(r => r.Id.Equals(id)).FirstOrDefault();
                if (child != null)
                {
                    childProcessingContentModel.ChildName = child.ChildName;
                    childProcessingContentModel.FullAddress = child.FullAddress;
                }
                childProcessingContentModel.ListProcessingContent = (from a in db.ProcessingContents.AsNoTracking()
                                                                     where a.ProfileChildId.Equals(id)
                                                                     orderby a.ProcessingDate descending
                                                                     select new ProcessingContentModel()
                                                                     {
                                                                         Id = a.Id,
                                                                         Content = a.Content,
                                                                         ProcessingDate = a.ProcessingDate,
                                                                         ProcessingBy = a.ProcessingBy
                                                                     }).ToList();
            }
            catch (Exception)
            { }
            foreach (var item in childProcessingContentModel.ListProcessingContent)
            {
                //item.ReceptionDateView = item.ReceptionDate.HasValue ? item.ReceptionDate.Value.ToString("dd/MM/yyyy HH:mm") : "";
                item.ProcessingDateView = item.ProcessingDate.ToString("dd/MM/yyyy HH:mm");
            }
            return childProcessingContentModel;
        }

        public List<string> GetListUserIdByNotify(string provinceId, string districtId, string wardId, string userOtherId)
        {
            try
            {
                List<string> listUserId = db.Users.Where(r => (r.ProvinceId.Equals(provinceId) || r.DistrictId.Equals(districtId)
                || r.WardId.Equals(wardId)) && !r.Id.Equals(userOtherId)).Select(s => s.Id).ToList();
                return listUserId;
            }
            catch { }
            return new List<string>();
        }
        public void SendContent(ProcessingContentModel model)
        {
            try
            {
                ProcessingContent processingContent = new ProcessingContent();
                processingContent.Id = Guid.NewGuid().ToString();
                processingContent.ProfileChildId = model.Id;
                processingContent.Content = model.Content;
                processingContent.ProcessingBy = model.ProcessingBy;
                processingContent.ProcessingDate = DateTime.Now;
                db.ProcessingContents.Add(processingContent);
                db.SaveChanges();
            }
            catch (Exception)
            { throw new Exception("Xảy ra lỗi"); }
        }
    }
}
