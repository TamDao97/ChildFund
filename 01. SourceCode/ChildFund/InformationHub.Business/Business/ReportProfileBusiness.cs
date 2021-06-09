using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.CacheModel;
using InformationHub.Model.Model.ReportProfile;
using InformationHub.Model.Repositories;
using InformationHub.Model.SearchResults;
using InformationHub.Resource;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json;
using NTS.Caching;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Storage;
using NTS.Utils;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using InformationHub.Model.Model.CacheModel;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;
using System.Data;
using NTSFramework.Common.Utils;

namespace InformationHub.Business
{
    public class ReportProfileBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["ConnectionString"]);
        string mailservicequeue = ConfigurationManager.AppSettings["mailservicequeue"];
        /// <summary>
        /// Tạo mới trường hợp
        /// </summary>
        public string CreateProfile(ReportProfileModel model, HttpFileCollection httpFile)
        {
            string Id = "";
            var dataCheck = db.ReportProfiles.Where(u => u.ChildName.ToLower().Equals(model.ChildName.ToLower())
                                                    && u.Age == model.Age
                                                    && u.WardId.Equals(model.WardId)
                                                    && u.ReceptionDate == model.ReceptionDate
                                                    && u.ReceptionTime.ToLower().Equals(model.ReceptionTime.ToLower())
                                                    && u.Gender == model.Gender
                                                    && u.CaseLocation.ToLower().Equals(model.CaseLocation.ToLower())).Count();
            if (dataCheck > 0)
            {
                throw new Exception(Resource.Resource.ReportProfile_IsExist);
            }
            ReportProfile reportProfile = new ReportProfile();
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    string fullAddress = "";
                    try
                    {
                        var provinceName = db.Provinces.FirstOrDefault(u => u.Id.Equals(model.ProvinceId)).Name;
                        var districtName = db.Districts.FirstOrDefault(u => u.Id.Equals(model.DistrictId)).Name;
                        var wardName = db.Wards.FirstOrDefault(u => u.Id.Equals(model.WardId)).Name;
                        fullAddress = wardName + " - " + districtName + " - " + provinceName;
                    }
                    catch (Exception)
                    { }

                    //Thông tin hồ sơ trường hợp
                    reportProfile.Id = Guid.NewGuid().ToString();
                    Id = reportProfile.Id;
                    reportProfile.InformationSources = model.InformationSources;
                    reportProfile.ReceptionTime = model.ReceptionTime;
                    reportProfile.ReceptionDate = model.ReceptionDate;
                    reportProfile.ChildName = model.ChildName;
                    reportProfile.ChildBirthdate = model.ChildBirthdate;
                    reportProfile.Gender = model.Gender;
                    reportProfile.Age = model.Age;
                    reportProfile.CaseLocation = model.CaseLocation;
                    reportProfile.WardId = model.WardId;
                    reportProfile.DistrictId = model.DistrictId;
                    reportProfile.ProvinceId = model.ProvinceId;
                    reportProfile.CurrentHealth = model.CurrentHealth;
                    reportProfile.SequelGuess = model.SequelGuess;
                    reportProfile.FatherName = model.FatherName;
                    reportProfile.FatherAge = model.FatherAge;
                    reportProfile.FatherJob = model.FatherJob;
                    reportProfile.MotherName = model.MotherName;
                    reportProfile.MotherAge = model.MotherAge;
                    reportProfile.MotherJob = model.MotherJob;
                    reportProfile.FamilySituation = model.FamilySituation;
                    reportProfile.PeopleCare = model.PeopleCare;
                    reportProfile.Support = model.Support;
                    reportProfile.ProviderName = model.ProviderName;
                    reportProfile.ProviderPhone = model.ProviderPhone;
                    reportProfile.ProviderAddress = model.ProviderAddress;
                    reportProfile.ProviderNote = model.ProviderNote;
                    reportProfile.StatusStep1 = true;
                    reportProfile.StatusStep2 = false;
                    reportProfile.StatusStep3 = false;
                    reportProfile.StatusStep4 = false;
                    reportProfile.StatusStep5 = false;
                    reportProfile.StatusStep6 = false;
                    reportProfile.IsDelete = !Constants.IsDelete;
                    reportProfile.IsPublish = Constants.NotActive;
                    reportProfile.SeverityLevel = model.SeverityLevel;
                    reportProfile.FinishDate = model.FinishDate;
                    reportProfile.FinishNote = model.FinishNote;
                    reportProfile.CloseDate = model.ClosedDate;
                    reportProfile.FullAddress = fullAddress;
                    reportProfile.CreateBy = model.CreateBy;
                    reportProfile.CreateDate = DateTime.Now;
                    reportProfile.UpdateBy = model.CreateBy;
                    reportProfile.UpdateDate = reportProfile.CreateDate;
                    reportProfile.SourceNote = model.SourceNote;
                    reportProfile.WordTitle = model.WordTitle;
                    reportProfile.SummaryCase = model.SummaryCase;
                    reportProfile.TypeOther = model.TypeOther;
                    #region[them phần sinh mã ca]
                    var code = db.CodeReports.FirstOrDefault();
                    var dateNow = DateTime.Now;
                    if (dateNow.Year != code.Year)
                    {
                        //khác năm thì đổi  năm và số reset về 1
                        code.Year = dateNow.Year;
                        code.Number = 1;
                    }
                    else
                    {
                        //cùng năm thì tăng số lên
                        code.Number = code.Number + 1;
                    }
                    reportProfile.Code = code.Year + code.Number.Value.ToString("D6");

                    #endregion
                    db.ReportProfiles.Add(reportProfile);
                    #region[them loại hình xâm phạm]
                    ReportProfileAbuseType reportProfileAbuseType;
                    foreach (var item in model.ListAbuseType)
                    {
                        reportProfileAbuseType = new ReportProfileAbuseType();
                        reportProfileAbuseType.Id = Guid.NewGuid().ToString();
                        reportProfileAbuseType.ReportProfileId = reportProfile.Id;
                        reportProfileAbuseType.AbuseTypeId = item.Id;
                        reportProfileAbuseType.AbuseTypeName = item.Name;
                        db.ReportProfileAbuseTypes.Add(reportProfileAbuseType);
                    }
                    #endregion
                    //Upload file lên cloud
                    if (httpFile.Count > 0)
                    {
                        List<string> listFileKey = httpFile.AllKeys.ToList();
                        ProfileAttachmentModel profileAttachmentModel;
                        ProfileAttachment profileAttachment;
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            profileAttachmentModel = model.ListProfileAttachment.FirstOrDefault(r => r.Id.Equals(listFileKey[i]));
                            profileAttachmentModel.Path = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;

                            profileAttachment = new ProfileAttachment()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ReportProfileId = reportProfile.Id,
                                Name = profileAttachmentModel.Name,
                                Path = profileAttachmentModel.Path,
                                Size = profileAttachmentModel.Size,
                                Extension = profileAttachmentModel.Extension,
                                Description = profileAttachmentModel.Description,
                                UploadBy = model.CreateBy,
                                UploadDate = DateTime.Now,
                            };
                            db.ProfileAttachments.Add(profileAttachment);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ReportProfileBusiness.CreateProfile", ex.Message, model);

                    //Xóa file
                    foreach (var attachment in model.ListProfileAttachment)
                    {
                        if (string.IsNullOrEmpty(attachment.Path))
                            Task.Run(async () =>
                            {
                                await AzureStorageUploadFiles.GetInstance().DeleteFileAsync(attachment.Path);
                            });
                    }
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            AddReportHistory(reportProfile, 1);
            db.SaveChanges();
            return Id;
        }
        public string UpdateProfile(ReportProfileModel model, HttpFileCollection httpFile)
        {
            ReportProfile reportProfile = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    string fullAddress = "";
                    try
                    {
                        var provinceName = db.Provinces.FirstOrDefault(u => u.Id.Equals(model.ProvinceId)).Name;
                        var districtName = db.Districts.FirstOrDefault(u => u.Id.Equals(model.DistrictId)).Name;
                        var wardName = db.Wards.FirstOrDefault(u => u.Id.Equals(model.WardId)).Name;
                        fullAddress = wardName + " - " + districtName + " - " + provinceName;
                    }
                    catch (Exception)
                    { }
                    //Thông tin hồ sơ trường hợp
                    reportProfile.InformationSources = model.InformationSources;
                    reportProfile.ReceptionTime = model.ReceptionTime;
                    reportProfile.ReceptionDate = model.ReceptionDate;
                    reportProfile.ChildName = model.ChildName;
                    reportProfile.ChildBirthdate = model.ChildBirthdate;
                    reportProfile.Gender = model.Gender;
                    reportProfile.Age = model.Age;
                    reportProfile.CaseLocation = model.CaseLocation;
                    reportProfile.WardId = model.WardId;
                    reportProfile.DistrictId = model.DistrictId;
                    reportProfile.ProvinceId = model.ProvinceId;
                    reportProfile.CurrentHealth = model.CurrentHealth;
                    reportProfile.SequelGuess = model.SequelGuess;
                    reportProfile.FatherName = model.FatherName;
                    reportProfile.FatherAge = model.FatherAge;
                    reportProfile.FatherJob = model.FatherJob;
                    reportProfile.MotherName = model.MotherName;
                    reportProfile.MotherAge = model.MotherAge;
                    reportProfile.MotherJob = model.MotherJob;
                    reportProfile.FamilySituation = model.FamilySituation;
                    reportProfile.PeopleCare = model.PeopleCare;
                    reportProfile.Support = model.Support;
                    reportProfile.ProviderName = model.ProviderName;
                    reportProfile.ProviderPhone = model.ProviderPhone;
                    reportProfile.ProviderAddress = model.ProviderAddress;
                    reportProfile.ProviderNote = model.ProviderNote;
                    reportProfile.SeverityLevel = model.SeverityLevel;
                    reportProfile.FullAddress = fullAddress;
                    reportProfile.UpdateBy = model.CreateBy;
                    reportProfile.UpdateDate = DateTime.Now;
                    reportProfile.SourceNote = model.SourceNote;
                    reportProfile.WordTitle = model.WordTitle;
                    reportProfile.SummaryCase = model.SummaryCase;
                    reportProfile.TypeOther = model.TypeOther;
                    if (string.IsNullOrEmpty(reportProfile.Code))
                    {
                        #region[them phần sinh mã ca]
                        var code = db.CodeReports.FirstOrDefault();
                        var dateNow = DateTime.Now;
                        if (dateNow.Year != code.Year)
                        {
                            //khác năm thì đổi  năm và số reset về 1
                            code.Year = dateNow.Year;
                            code.Number = 1;
                        }
                        else
                        {
                            //cùng năm thì tăng số lên
                            code.Number = code.Number + 1;
                        }
                        reportProfile.Code = code.Year + code.Number.Value.ToString("D6");
                        #endregion
                    }
                    #region[them loại hình xâm phạm]
                    var reportProfileAbuseTypeOld = db.ReportProfileAbuseTypes.Where(u => u.ReportProfileId.Equals(model.Id)).ToList();
                    if (reportProfileAbuseTypeOld.Count > 0)
                    {
                        db.ReportProfileAbuseTypes.RemoveRange(reportProfileAbuseTypeOld);
                    }
                    ReportProfileAbuseType reportProfileAbuseType;
                    foreach (var item in model.ListAbuseType)
                    {
                        reportProfileAbuseType = new ReportProfileAbuseType();
                        reportProfileAbuseType.Id = Guid.NewGuid().ToString();
                        reportProfileAbuseType.ReportProfileId = reportProfile.Id;
                        reportProfileAbuseType.AbuseTypeId = item.Id;
                        reportProfileAbuseType.AbuseTypeName = item.Name;
                        db.ReportProfileAbuseTypes.Add(reportProfileAbuseType);
                    }
                    #endregion
                    #region[xóa file nào bị xóa]
                    if (model.ListProfileAttachmentUpdate == null)
                    {
                        model.ListProfileAttachmentUpdate = new List<ProfileAttachmentModel>();
                    }
                    var lstIdFile = model.ListProfileAttachmentUpdate.Select(u => u.Id).ToList();
                    var allFile = db.ProfileAttachments.Where(u => u.ReportProfileId.Equals(model.Id)).ToList();
                    foreach (var item in allFile)
                    {
                        if (!lstIdFile.Contains(item.Id))
                        {
                            db.ProfileAttachments.Remove(item);
                        }
                    }

                    #endregion
                    //Upload file lên cloud
                    if (httpFile.Count > 0)
                    {
                        List<string> listFileKey = httpFile.AllKeys.ToList();
                        ProfileAttachmentModel profileAttachmentModel;
                        ProfileAttachment profileAttachment;
                        for (int i = 0; i < httpFile.Count; i++)
                        {
                            profileAttachmentModel = model.ListProfileAttachment.FirstOrDefault(r => r.Id.Equals(listFileKey[i]));
                            profileAttachmentModel.Path = Task.Run(async () =>
                            {
                                return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                            }).Result;

                            profileAttachment = new ProfileAttachment()
                            {
                                Id = Guid.NewGuid().ToString(),
                                ReportProfileId = reportProfile.Id,
                                Name = profileAttachmentModel.Name,
                                Path = profileAttachmentModel.Path,
                                Size = profileAttachmentModel.Size,
                                Extension = profileAttachmentModel.Extension,
                                Description = profileAttachmentModel.Description,
                                UploadBy = model.CreateBy,
                                UploadDate = DateTime.Now,
                            };
                            db.ProfileAttachments.Add(profileAttachment);
                        }
                    }

                    db.SaveChanges();
                    trans.Commit();

                    if (model.IsExport)
                    {
                        return ExportWordForm1(model.Id, model.CreateBy);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ReportProfileBusiness.UpdateProfile", ex.Message, model);
                    //Xóa file
                    foreach (var attachment in model.ListProfileAttachment)
                    {
                        if (string.IsNullOrEmpty(attachment.Path))
                            Task.Run(async () =>
                            {
                                await AzureStorageUploadFiles.GetInstance().DeleteFileAsync(attachment.Path);
                            });
                    }
                    trans.Rollback();
                    throw new Exception(ex.Message);
                }
            }
            return string.Empty;
        }
        public SearchResultObject<ReportProfileSearchResult> GetList(ReportProfileSearchCondition searchCondition, LoginProfileModel userInfo)
        {
            SearchResultObject<ReportProfileSearchResult> searchResult = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 where a.IsDelete == false
                                 select new ReportProfileSearchResult()
                                 {
                                     Id = a.Id,
                                     Code = a.Code,
                                     InformationSources = a.InformationSources,
                                     ReceptionTime = a.ReceptionTime,
                                     ReceptionDate = a.ReceptionDate,
                                     ChildName = a.ChildName,
                                     ChildBirthdate = a.ChildBirthdate,
                                     Gender = a.Gender,
                                     Age = a.Age,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     CaseLocation = a.CaseLocation,
                                     FullAddress = a.FullAddress,
                                     TypeOther = a.TypeOther,
                                     ProviderName = a.ProviderName,
                                     ProviderPhone = a.ProviderPhone,
                                     StatusStep1 = a.StatusStep1,
                                     StatusStep2 = a.StatusStep2,
                                     StatusStep3 = a.StatusStep3,
                                     StatusStep4 = a.StatusStep4,
                                     StatusStep5 = a.StatusStep5,
                                     StatusStep6 = a.StatusStep6,
                                     SeverityLevel = a.SeverityLevel,
                                     FinishDate = a.FinishDate,
                                     IsPublish = a.IsPublish,
                                     ListAbuse = (from ar in db.ReportProfileAbuseTypes
                                                  where ar.ReportProfileId.Equals(a.Id)
                                                  select new ComboboxResult
                                                  { Id = ar.AbuseTypeId, Name = ar.AbuseTypeName }).ToList()
                                 }).AsQueryable();
                if (searchCondition.Gender != null)
                {
                    listmodel = listmodel.Where(r => r.Gender == searchCondition.Gender.Value);
                }
                if (!string.IsNullOrEmpty(searchCondition.Age))
                {
                    try
                    {
                        var ageArray = searchCondition.Age.Split(';');
                        int ageFrom = int.Parse(ageArray[0]);
                        int ageTo = int.Parse(ageArray[1]);
                        listmodel = listmodel.Where(r => r.Age > ageFrom && r.Age < ageTo);
                    }
                    catch (Exception)
                    { }
                }
                if (searchCondition.InformationSources != null)
                {
                    listmodel = listmodel.Where(r => r.InformationSources == searchCondition.InformationSources.Value);
                }
                #region
                if (searchCondition.StatusStep1)
                {
                    listmodel = listmodel.Where(r => r.StatusStep1 == true);
                }
                if (searchCondition.StatusStep2)
                {
                    listmodel = listmodel.Where(r => r.StatusStep2 == true);
                }
                if (searchCondition.StatusStep3)
                {
                    listmodel = listmodel.Where(r => r.StatusStep3 == true);
                }
                if (searchCondition.StatusStep4)
                {
                    listmodel = listmodel.Where(r => r.StatusStep4 == true);
                }
                if (searchCondition.StatusStep5)
                {
                    listmodel = listmodel.Where(r => r.StatusStep5 == true);
                }
                if (searchCondition.StatusStep6)
                {
                    listmodel = listmodel.Where(r => r.StatusStep6 == true);
                }
                #endregion
                if (searchCondition.SeverityLevel != null)
                {
                    listmodel = listmodel.Where(r => r.SeverityLevel == searchCondition.SeverityLevel.Value);
                }
                if (!string.IsNullOrEmpty(searchCondition.ChildName))
                {
                    listmodel = listmodel.Where(r => r.ChildName.ToLower().Contains(searchCondition.ChildName.ToLower()) || r.Code.ToLower().Contains(searchCondition.ChildName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.ProviderName))
                {
                    listmodel = listmodel.Where(r => r.ProviderName.ToLower().Contains(searchCondition.ProviderName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CaseLocation))
                {
                    listmodel = listmodel.Where(r => r.CaseLocation.ToLower().Contains(searchCondition.CaseLocation.ToLower()));
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
                if (!string.IsNullOrEmpty(searchCondition.AbuseId))
                {
                    listmodel = listmodel.Where(r => r.ListAbuse.Where(rr => rr.Id.Equals(searchCondition.AbuseId)).Count() > 0);
                }
                #region[loc du lieu theo tai khoan]
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.WardId.Equals(userInfo.WardId));
                }
                else if (userInfo.Type == Constants.LevelArea)
                {
                    listmodel = listmodel.Where(u => u.DistrictId.Equals(userInfo.DistrictId) && u.IsPublish == true);
                    if (!string.IsNullOrEmpty(searchCondition.WardId))
                    {
                        listmodel = listmodel.Where(u => u.WardId.Equals(searchCondition.WardId));
                    }
                }
                else if (userInfo.Type == Constants.LevelOffice)
                {
                    listmodel = listmodel.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId) && u.IsPublish == true);
                    if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                    {
                        listmodel = listmodel.Where(u => u.DistrictId.Equals(searchCondition.DistrictId));
                    }
                    if (!string.IsNullOrEmpty(searchCondition.WardId))
                    {
                        listmodel = listmodel.Where(u => u.WardId.Equals(searchCondition.WardId));
                    }
                }
                else if (userInfo.Type == Constants.LevelAdmin)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                    if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                    {
                        listmodel = listmodel.Where(u => u.ProvinceId.Equals(searchCondition.ProvinceId));
                    }
                    if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                    {
                        listmodel = listmodel.Where(u => u.DistrictId.Equals(searchCondition.DistrictId));
                    }
                    if (!string.IsNullOrEmpty(searchCondition.WardId))
                    {
                        listmodel = listmodel.Where(u => u.WardId.Equals(searchCondition.WardId));
                    }
                }

                #endregion
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.TotalItemStatus6 = listmodel.Where(u => u.StatusStep6 == true).Select(u => u.Id).Count();
                searchResult.TotalItemStatus1 = searchResult.TotalItem - searchResult.TotalItemStatus6;
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                foreach (var item in searchResult.ListResult)
                {
                    item.InformationSourcesView = Common.GenSource(item.InformationSources);
                    item.SeverityLevelView = Common.GenLevel(item.SeverityLevel);
                    item.GenderView = Common.GenGender(item.Gender.Value);
                }
                if (searchCondition.Export == 1)
                {
                    searchResult.PathFile = ExportExcel(listmodel.OrderByDescending(u => u.ReceptionDate).ToList(), searchCondition);
                }
                else
                {
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }

        public SearchResultObject<ReportProfileSearchResult> GetListForwards(ReportProfileSearchCondition searchCondition, LoginProfileModel userInfo)
        {
            SearchResultObject<ReportProfileSearchResult> searchResult = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 where a.IsDelete == false && a.IsPublish == true
                                 join b in db.ReportForwards.AsNoTracking() on a.Id equals b.ReportProfileId
                                 select new ReportProfileSearchResult()
                                 {
                                     Id = b.Id,
                                     Code = a.Code,
                                     InformationSources = a.InformationSources,
                                     ReceptionTime = a.ReceptionTime,
                                     ReceptionDate = a.ReceptionDate,
                                     CreateDate = b.CreateDate,
                                     ChildName = a.ChildName,
                                     ChildBirthdate = a.ChildBirthdate,
                                     Gender = a.Gender,
                                     Age = a.Age,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     CaseLocation = a.CaseLocation,
                                     FullAddress = a.FullAddress,
                                     //  CurrentHealth = a.CurrentHealth,
                                     TypeOther = a.TypeOther,
                                     ProviderName = a.ProviderName,
                                     ProviderPhone = a.ProviderPhone,
                                     StatusStep1 = a.StatusStep1,
                                     StatusStep2 = a.StatusStep2,
                                     StatusStep3 = a.StatusStep3,
                                     StatusStep4 = a.StatusStep4,
                                     StatusStep5 = a.StatusStep5,
                                     StatusStep6 = a.StatusStep6,
                                     ForwardLevel = b.ForwardLevel,
                                     ForwardNote = b.ForwardNote,
                                     SeverityLevel = a.SeverityLevel,
                                     FinishDate = a.FinishDate,
                                     IsPublish = a.IsPublish,
                                     Status = b.Status,
                                     ListAbuse = (from ar in db.ReportProfileAbuseTypes
                                                  where ar.ReportProfileId.Equals(a.Id)
                                                  select new ComboboxResult
                                                  { Id = ar.AbuseTypeId, Name = ar.AbuseTypeName }).ToList()
                                 }).AsQueryable();
                if (searchCondition.Gender != null)
                {
                    listmodel = listmodel.Where(r => r.Gender == searchCondition.Gender.Value);
                }
                if (!string.IsNullOrEmpty(searchCondition.Age))
                {
                    try
                    {
                        var ageArray = searchCondition.Age.Split(';');
                        int ageFrom = int.Parse(ageArray[0]);
                        int ageTo = int.Parse(ageArray[1]);
                        listmodel = listmodel.Where(r => r.Age > ageFrom && r.Age < ageTo);
                    }
                    catch (Exception)
                    { }
                }
                if (searchCondition.InformationSources != null)
                {
                    listmodel = listmodel.Where(r => r.InformationSources == searchCondition.InformationSources.Value);
                }
                #region
                if (searchCondition.StatusStep1)
                {
                    listmodel = listmodel.Where(r => r.StatusStep1 == true);
                }
                if (searchCondition.StatusStep2)
                {
                    listmodel = listmodel.Where(r => r.StatusStep2 == true);
                }
                if (searchCondition.StatusStep3)
                {
                    listmodel = listmodel.Where(r => r.StatusStep3 == true);
                }
                if (searchCondition.StatusStep4)
                {
                    listmodel = listmodel.Where(r => r.StatusStep4 == true);
                }
                if (searchCondition.StatusStep5)
                {
                    listmodel = listmodel.Where(r => r.StatusStep5 == true);
                }
                if (searchCondition.StatusStep6)
                {
                    listmodel = listmodel.Where(r => r.StatusStep6 == true);
                }
                #endregion
                if (searchCondition.SeverityLevel != null)
                {
                    listmodel = listmodel.Where(r => r.SeverityLevel == searchCondition.SeverityLevel.Value);
                }
                if (!string.IsNullOrEmpty(searchCondition.ChildName))
                {
                    listmodel = listmodel.Where(r => r.ChildName.ToLower().Contains(searchCondition.ChildName.ToLower()) || r.Code.ToLower().Contains(searchCondition.ChildName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.ProviderName))
                {
                    listmodel = listmodel.Where(r => r.ProviderName.ToLower().Contains(searchCondition.ProviderName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CaseLocation))
                {
                    listmodel = listmodel.Where(r => r.CaseLocation.ToLower().Contains(searchCondition.CaseLocation.ToLower()));
                }
                //if (!string.IsNullOrEmpty(searchCondition.CurrentHealth))
                //{
                //    listmodel = listmodel.Where(r => r.CurrentHealth.ToLower().Contains(searchCondition.CurrentHealth.ToLower()));
                //}
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
                if (!string.IsNullOrEmpty(searchCondition.AbuseId))
                {
                    listmodel = listmodel.Where(r => r.ListAbuse.Where(rr => rr.Id.Equals(searchCondition.AbuseId)).Count() > 0);
                }
                #region[loc du lieu theo tai khoan]
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.WardId.Equals(userInfo.WardId));
                }
                else if (userInfo.Type == Constants.LevelArea)
                {
                    listmodel = listmodel.Where(u => u.DistrictId.Equals(userInfo.DistrictId));
                    listmodel = listmodel.Where(u => u.ForwardLevel == Constants.LevelArea);
                    if (!string.IsNullOrEmpty(searchCondition.WardId))
                    {
                        listmodel = listmodel.Where(u => u.WardId.Equals(searchCondition.WardId));
                    }
                }
                else if (userInfo.Type == Constants.LevelOffice)
                {
                    listmodel = listmodel.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId));
                    listmodel = listmodel.Where(u => u.ForwardLevel == Constants.LevelOffice);
                    if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                    {
                        listmodel = listmodel.Where(u => u.DistrictId.Equals(searchCondition.DistrictId));
                    }
                    if (!string.IsNullOrEmpty(searchCondition.WardId))
                    {
                        listmodel = listmodel.Where(u => u.WardId.Equals(searchCondition.WardId));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                    {
                        listmodel = listmodel.Where(u => u.ProvinceId.Equals(searchCondition.ProvinceId));
                    }
                    if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                    {
                        listmodel = listmodel.Where(u => u.DistrictId.Equals(searchCondition.DistrictId));
                    }
                    if (!string.IsNullOrEmpty(searchCondition.WardId))
                    {
                        listmodel = listmodel.Where(u => u.WardId.Equals(searchCondition.WardId));
                    }
                }
                #endregion
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.TotalItemStatus6 = listmodel.Where(u => u.StatusStep6 == true).Select(u => u.Id).Count();
                searchResult.TotalItemStatus1 = searchResult.TotalItem - searchResult.TotalItemStatus6;
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                foreach (var item in searchResult.ListResult)
                {
                    item.InformationSourcesView = Common.GenSource(item.InformationSources);
                    item.SeverityLevelView = Common.GenLevel(item.SeverityLevel.Value);
                    item.GenderView = Common.GenGender(item.Gender.Value);
                }
                if (searchCondition.Export == 1)
                {
                    searchResult.PathFile = ExportExcelForward(listmodel.OrderByDescending(u => u.CreateDate).ToList(), searchCondition);
                }
                else
                {
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }
        public string ExportExcel(List<ReportProfileSearchResult> list, ReportProfileSearchCondition searchCondition)
        {
            //string lang = "Vi";
            //if (HttpContext.Current.Request.Cookies["culture"] != null)
            //{
            //    lang = HttpContext.Current.Request.Cookies["culture"].Value;
            //}
            string pathTemplate = "/Template/ReportProfileList.xlsx";
            string pathExport = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.ReportProfile_ListView) + ".xlsx";
            try
            {

                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(pathTemplate));
                IWorksheet sheet = workbook.Worksheets[0];
                IRange rangeValue = sheet.FindFirst("<TitleSub>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<TitleSub>", Resource.Resource.DayFrom_Title + " " + searchCondition.DateFrom + " " + Resource.Resource.DayTo_Title.ToLower() + " " + searchCondition.DateTo);

                IRange rangeValue2 = sheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<Title>", Resource.Resource.ReportProfile_ListView.ToUpper());

                IRange rangeValue3 = sheet.FindFirst("<ChildName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<ChildName>", Resource.Resource.ReportProfile_ChildName);

                IRange rangeValue4 = sheet.FindFirst("<Gender>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<Gender>", Resource.Resource.ReportProfile_Gender);

                IRange rangeValue5 = sheet.FindFirst("<Age>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Age>", Resource.Resource.ReportProfile_Age);

                IRange rangeValue6 = sheet.FindFirst("<Soure>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Soure>", Resource.Resource.ReportProfile_InformationSources);

                IRange rangeValue7 = sheet.FindFirst("<Abuse>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<Abuse>", Resource.Resource.ReportProfile_AbuseType);

                IRange rangeValue8 = sheet.FindFirst("<Level>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Level>", Resource.Resource.ReportProfile_SeverityLevel);

                // IRange rangeValue9 = sheet.FindFirst("<Health>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                // rangeValue9.Text = rangeValue9.Text.Replace("<Health>", Resource.Resource.ReportProfile_CurrentHealth);

                //IRange rangeValue10 = sheet.FindFirst("<Note>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                //  rangeValue10.Text = rangeValue10.Text.Replace("<Note>", Resource.Resource.NoteForward_Title);

                IRange rangeValue11 = sheet.FindFirst("<Address>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue11.Text = rangeValue11.Text.Replace("<Address>", Resource.Resource.ReportProfile_Address);

                IRange rangeValue12 = sheet.FindFirst("<Local>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue12.Text = rangeValue12.Text.Replace("<Local>", Resource.Resource.ReportProfile_ReportAddress);

                IRange rangeValue13 = sheet.FindFirst("<Date>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue13.Text = rangeValue13.Text.Replace("<Date>", Resource.Resource.ReportProfile_DateInbox);

                IRange rangeValue14 = sheet.FindFirst("<ProviderName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue14.Text = rangeValue14.Text.Replace("<ProviderName>", Resource.Resource.ProviderName_Title);

                IRange rangeValue15 = sheet.FindFirst("<Phone>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue15.Text = rangeValue15.Text.Replace("<Phone>", Resource.Resource.Phone_Title);

                IRange rangeValue16 = sheet.FindFirst("<Status>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue16.Text = rangeValue16.Text.Replace("<Status>", Resource.Resource.Label_Status);

                int total = list.Count;
                if (total == 0)
                {
                    IRange rangeValueDaTa = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    rangeValueDaTa.Text = rangeValueDaTa.Text.Replace("<Data>", (string.Empty));
                }
                if (total > 0)
                {
                    int index = 1;
                    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    if (total > 1)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, total - 1, ExcelInsertOptions.FormatAsBefore);
                    }
                    var listExport = (from a in list
                                      select new
                                      {
                                          Index = index++,
                                          stt = ((a.StatusStep1 == true ? "■" : "□") + (a.StatusStep2 == true ? "■" : "□") + (a.StatusStep3 == true ? "■" : "□") + (a.StatusStep4 == true ? "■" : "□") + (a.StatusStep5 == true ? "■" : "□") + (a.StatusStep6 == true ? "■" : "□")),
                                          a.ChildName,
                                          a5 = Common.GenGender(a.Gender.Value),
                                          a.Age,
                                          a0 = Common.GenSource(a.InformationSources),
                                          a1 = (a.ListAbuse.Where(u => u.Id.Contains(Constants.Abuse5)).Count() > 0) ? (a.TypeOther) : (string.Join(", ", a.ListAbuse.Select(u => u.Name))),
                                          a3 = Common.GenLevel(a.SeverityLevel.Value),
                                          //  a.CurrentHealth,
                                          a.FullAddress,
                                          a.CaseLocation,
                                          a2 = a.ReceptionDate.Value.ToString("dd/MM/yyyy"),
                                          a.ProviderName,
                                          a.ProviderPhone,
                                      }).ToList();
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 2, iRangeData.Row + total - 1, 2].CellStyle.Font.Size = 50;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 13].Borders.Color = ExcelKnownColors.Black;
                    sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 13].CellStyle.WrapText = true;

                }
                workbook.SaveAs(HttpContext.Current.Server.MapPath(pathExport));
            }
            catch (Exception ex)
            { }
            return pathExport;
        }
        public string ExportExcelForward(List<ReportProfileSearchResult> list, ReportProfileSearchCondition searchCondition)
        {
            string pathTemplate = "/Template/ReportProfileForwardList.xlsx";
            string pathExport = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.ReportProfile_Forward_ListView) + ".xlsx";
            try
            {

                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(pathTemplate));
                IWorksheet sheet = workbook.Worksheets[0];
                IRange rangeValue = sheet.FindFirst("<TitleSub>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<TitleSub>", Resource.Resource.DayFrom_Title + " " + searchCondition.DateFrom + " " + Resource.Resource.DayTo_Title.ToLower() + " " + searchCondition.DateTo);

                IRange rangeValue2 = sheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<Title>", Resource.Resource.ReportProfile_Forward_ListView.ToUpper());

                IRange rangeValue3 = sheet.FindFirst("<ChildName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<ChildName>", Resource.Resource.ReportProfile_ChildName);

                IRange rangeValue4 = sheet.FindFirst("<Gender>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<Gender>", Resource.Resource.ReportProfile_Gender);

                IRange rangeValue5 = sheet.FindFirst("<Age>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Age>", Resource.Resource.ReportProfile_Age);

                IRange rangeValue6 = sheet.FindFirst("<Soure>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Soure>", Resource.Resource.ReportProfile_InformationSources);

                IRange rangeValue7 = sheet.FindFirst("<Abuse>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<Abuse>", Resource.Resource.ReportProfile_AbuseType);

                IRange rangeValue8 = sheet.FindFirst("<Level>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Level>", Resource.Resource.ReportProfile_SeverityLevel);

                //  IRange rangeValue9 = sheet.FindFirst("<Health>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                //   rangeValue9.Text = rangeValue9.Text.Replace("<Health>", Resource.Resource.ReportProfile_CurrentHealth);

                IRange rangeValue10 = sheet.FindFirst("<Note>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue10.Text = rangeValue10.Text.Replace("<Note>", Resource.Resource.NoteForward_Title);

                IRange rangeValue11 = sheet.FindFirst("<Address>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue11.Text = rangeValue11.Text.Replace("<Address>", Resource.Resource.ReportProfile_Address);

                IRange rangeValue12 = sheet.FindFirst("<Local>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue12.Text = rangeValue12.Text.Replace("<Local>", Resource.Resource.ReportProfile_ReportAddress);

                IRange rangeValue13 = sheet.FindFirst("<Date>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue13.Text = rangeValue13.Text.Replace("<Date>", Resource.Resource.ReportProfile_DateInbox);

                IRange rangeValue133 = sheet.FindFirst("<CreateDate>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue133.Text = rangeValue133.Text.Replace("<CreateDate>", Resource.Resource.DateForward_Title);

                IRange rangeValue14 = sheet.FindFirst("<ProviderName>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue14.Text = rangeValue14.Text.Replace("<ProviderName>", Resource.Resource.ProviderName_Title);

                IRange rangeValue15 = sheet.FindFirst("<Phone>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue15.Text = rangeValue15.Text.Replace("<Phone>", Resource.Resource.Phone_Title);

                IRange rangeValue16 = sheet.FindFirst("<Status>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue16.Text = rangeValue16.Text.Replace("<Status>", Resource.Resource.Label_Status);

                int total = list.Count;
                if (total == 0)
                {
                    IRange rangeValueDaTa = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    rangeValueDaTa.Text = rangeValueDaTa.Text.Replace("<Data>", (string.Empty));
                }
                if (total > 0)
                {
                    int index = 1;
                    IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                    if (total > 1)
                    {
                        sheet.InsertRow(iRangeData.Row + 1, total - 1, ExcelInsertOptions.FormatAsBefore);
                    }
                    var listExport = (from a in list
                                      select new
                                      {
                                          Index = index++,
                                          stt = ((a.StatusStep1 == true ? "■" : "□") + (a.StatusStep2 == true ? "■" : "□") + (a.StatusStep3 == true ? "■" : "□") + (a.StatusStep4 == true ? "■" : "□") + (a.StatusStep5 == true ? "■" : "□") + (a.StatusStep6 == true ? "■" : "□")),
                                          a.ChildName,
                                          a5 = Common.GenGender(a.Gender.Value),
                                          a.Age,
                                          a0 = Common.GenSource(a.InformationSources),
                                          a1 = (a.ListAbuse.Where(u => u.Id.Contains(Constants.Abuse5)).Count() > 0) ? (a.TypeOther) : (string.Join(", ", a.ListAbuse.Select(u => u.Name))),
                                          a3 = Common.GenLevel(a.SeverityLevel.Value),
                                          //  a.CurrentHealth,
                                          a.ForwardNote,
                                          a.FullAddress,
                                          a.CaseLocation,
                                          a2 = a.ReceptionDate.Value.ToString("dd/MM/yyyy"),
                                          a32 = a.CreateDate.Value.ToString("dd/MM/yyyy"),
                                          a.ProviderName,
                                          a.ProviderPhone,
                                      }).ToList();
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                    sheet.Range[iRangeData.Row, 2, iRangeData.Row + total - 1, 2].CellStyle.Font.Size = 50;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 5].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 15].Borders.Color = ExcelKnownColors.Black;
                    sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 15].CellStyle.WrapText = true;

                }
                workbook.SaveAs(HttpContext.Current.Server.MapPath(pathExport));
            }
            catch (Exception ex)
            { }
            return pathExport;
        }
        public void AddReportHistory(ReportProfile model, int StatusProcess)
        {
            try
            {
                var dateNow = DateTime.Now;
                ReportHistory reportHistory = db.ReportHistories.FirstOrDefault(u => u.ReportProfileId.Equals(model.Id) && u.StatusProcess == StatusProcess && (u.CreateDate.Day == dateNow.Day && u.CreateDate.Month == dateNow.Month && u.CreateDate.Year == dateNow.Year));
                if (reportHistory == null)
                {
                    reportHistory = new ReportHistory();
                    reportHistory.Id = Guid.NewGuid().ToString();
                    reportHistory.ReportProfileId = model.Id;
                    reportHistory.Name = model.ChildName;
                    reportHistory.Code = model.Code;
                    reportHistory.Age = model.Age;
                    reportHistory.FullAddress = model.FullAddress;
                    reportHistory.LocalAddress = model.CaseLocation;
                    reportHistory.CreateDate = dateNow;
                    reportHistory.WardId = model.WardId;
                    reportHistory.StatusProcess = StatusProcess;
                    reportHistory.Abuse = string.Join(";", db.ReportProfileAbuseTypes.Where(u => u.ReportProfileId.Equals(model.Id)).Select(u => u.AbuseTypeName).ToList());
                    db.ReportHistories.Add(reportHistory);
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.AddReportHistory", ex.Message, model);
            }
        }
        public void PublishReport(string id)
        {
            var data = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            try
            {
                data.IsPublish = true;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.PublishReport", ex.Message, data);
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            #region[cache notify]
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    //địa phương duyệt- lấy tk trung ương
                    var userNotify = db.Users.Where(u => u.Type == Constants.LevelAdmin && u.IsDisable == false).ToList();
                    SendMailAndNotify(data, userNotify, " " + Resource.Resource.Label_Add + " ", false, string.Empty);
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.PublishReport -- update Cache", ex.Message, data);
            }
            #endregion

        }

        public void ReOpenCase(string id)
        {
            var data = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            try
            {
                data.StatusStep6 = false;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileBusiness.ReOpenCase", ex.Message, data);
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
        }

        public void DeleteCase(string profileId)
        {
            var reportProfile = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(profileId));
            if (reportProfile == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var supportPlan = (from r in db.SupportPlants
                                       where r.ReportProfileId.Equals(profileId)
                                       select r);
                    var supportPlanId = supportPlan.Select(r => r.Id).ToList();

                    // xóa dữ liệu bảng EstimateCostAttachment
                    var listAttach = (from r in db.EstimateCostAttachments
                                      where supportPlanId.Contains(r.SupportPlantId)
                                      select r);
                    db.EstimateCostAttachments.RemoveRange(listAttach);
                    db.SupportPlants.RemoveRange(supportPlan);

                    var caseVerification = from r in db.CaseVerifications
                                           where r.ReportProfileId.Equals(profileId)
                                           select r;
                    db.CaseVerifications.RemoveRange(caseVerification);

                    var evaluationFirsts = from r in db.EvaluationFirsts
                                           where r.ReportProfileId.Equals(profileId)
                                           select r;
                    db.EvaluationFirsts.RemoveRange(evaluationFirsts);

                    var supportAffter = from r in db.SupportAfterStatus
                                        where r.ReportProfileId.Equals(profileId)
                                        select r;
                    db.SupportAfterStatus.RemoveRange(supportAffter);

                    var reportForward = from r in db.ReportForwards
                                        where r.ReportProfileId.Equals(profileId)
                                        select r;
                    db.ReportForwards.RemoveRange(reportForward);


                    var profileAttachment = from r in db.ProfileAttachments
                                            where r.ReportProfileId.Equals(profileId)
                                            select r;
                    db.ProfileAttachments.RemoveRange(profileAttachment);

                    var reportHistory = from r in db.ReportHistories
                                        where r.ReportProfileId.Equals(profileId)
                                        select r;
                    db.ReportHistories.RemoveRange(reportHistory);

                    var reportAbuseType = from r in db.ReportProfileAbuseTypes
                                          where r.ReportProfileId.Equals(profileId)
                                          select r;
                    db.ReportProfileAbuseTypes.RemoveRange(reportAbuseType);

                    db.ReportProfiles.Remove(reportProfile);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    LogUtils.ExceptionLog("ReportProfileBusiness.DeleteCase", ex.Message, reportProfile);
                    throw new Exception(Resource.Resource.ErroProcess_Title);
                }
            }
        }

        public void FinishReport(string id)
        {
            var data = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            if (data.StatusStep2 == false || data.StatusStep3 == false || data.StatusStep4 == false || data.StatusStep5 == false)
            {
                throw new Exception(Resource.Resource.Not_Finish);
            }
            try
            {
                data.FinishDate = DateTime.Now;
                data.FinishNote = "";
                data.StatusStep6 = true;
                AddReportHistory(data, 6);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            #region[cache notify]
            try
            {
                var createBy = System.Web.HttpContext.Current.User.Identity.Name;
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(createBy);
                List<User> userNotify = new List<User>();
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    userNotify = db.Users.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId) && (u.Type == Constants.LevelArea || u.Type == Constants.LevelOffice) && u.IsDisable == false).ToList();
                }
                SendMailAndNotify(data, userNotify, " " + Resource.Resource.Label_Close, false, string.Empty);
            }
            catch (Exception)
            { }
            #endregion
        }

        public void CloseReport(string id)
        {
            var data = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            try
            {
                data.IsDelete = true;
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            #region[cache notify]
            try
            {
                var createBy = System.Web.HttpContext.Current.User.Identity.Name;
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(createBy);
                List<User> userNotify = new List<User>();
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    userNotify = db.Users.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId) && (u.Type == Constants.LevelArea || u.Type == Constants.LevelOffice) && u.IsDisable == false).ToList();
                }
                SendMailAndNotify(data, userNotify, " " + Resource.Resource.Label_Close, false, string.Empty);
            }
            catch (Exception)
            { }
            #endregion
        }
        //cap nhat
        public ReportProfileModel GetInfo(string id)
        {
            var data = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            try
            {
                ReportProfileModel model = new ReportProfileModel();
                model.Id = data.Id;
                model.Code = data.Code;
                model.InformationSources = data.InformationSources;
                model.ReceptionTime = data.ReceptionTime;
                model.ReceptionDate = data.ReceptionDate;
                model.ChildName = data.ChildName;
                model.ChildBirthdate = data.ChildBirthdate;
                model.Gender = data.Gender;
                model.Age = data.Age;
                model.CaseLocation = data.CaseLocation;
                model.WardId = data.WardId;
                model.DistrictId = data.DistrictId;
                model.ProvinceId = data.ProvinceId;
                model.FullAddress = data.FullAddress;
                model.CurrentHealth = data.CurrentHealth;
                model.SequelGuess = data.SequelGuess;
                model.FatherName = data.FatherName;
                model.FatherAge = data.FatherAge;
                model.FatherJob = data.FatherJob;
                model.MotherName = data.MotherName;
                model.MotherAge = data.MotherAge;
                model.MotherJob = data.MotherJob;
                model.FamilySituation = data.FamilySituation;
                model.PeopleCare = data.PeopleCare;
                model.Support = data.Support;
                model.ProviderName = data.ProviderName;
                model.ProviderPhone = data.ProviderPhone;
                model.ProviderAddress = data.ProviderAddress;
                model.ProviderNote = data.ProviderNote;
                model.StatusStep1 = data.StatusStep1;
                model.StatusStep2 = data.StatusStep2;
                model.StatusStep3 = data.StatusStep3;
                model.StatusStep4 = data.StatusStep4;
                model.StatusStep5 = data.StatusStep5;
                model.StatusStep6 = data.StatusStep6;
                model.SeverityLevel = data.SeverityLevel;
                model.FinishNote = data.FinishNote;
                model.FinishDate = data.FinishDate;
                model.SourceNote = data.SourceNote;
                model.WordTitle = data.WordTitle;
                model.TypeOther = data.TypeOther;
                model.SummaryCase = data.SummaryCase;
                model.ListAbuseType = (from a in db.ReportProfileAbuseTypes
                                       where a.ReportProfileId.Equals(id)
                                       select new ComboboxResult
                                       { Id = a.AbuseTypeId, Name = a.AbuseTypeName }).ToList();
                model.ListProfileAttachment = (from a in db.ProfileAttachments
                                               where a.ReportProfileId.Equals(id)
                                               orderby a.UploadDate
                                               select new ProfileAttachmentModel
                                               { Id = a.Id, Name = a.Name, Size = a.Size, Path = a.Path, UploadDateRoot = a.UploadDate }).ToList();
                foreach (var item in model.ListProfileAttachment)
                {
                    item.UploadDate = item.UploadDateRoot.Value.ToString("dd/MM/yyyy");
                }
                return model;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //xu ly truong hop
        public EvaluationFirstModel GetEvaluationFirstModel(string reportProfileId)
        {
            EvaluationFirstModel model = new EvaluationFirstModel();
            var data = db.EvaluationFirsts.FirstOrDefault(u => u.ReportProfileId.Equals(reportProfileId));
            if (data != null)
            {
                model.Id = data.Id;
                model.ReportProfileId = data.ReportProfileId;
                model.PerformingDate = data.PerformingDate;
                model.LevelHarm = data.LevelHarm;
                model.LevelHarmContinue = data.LevelHarmContinue;
                model.TotalLevelHigh = data.TotalLevelHigh;
                model.TotalLevelAverage = data.TotalLevelAverage;
                model.TotalLevelLow = data.TotalLevelLow;
                model.AbilityProtectYourself = data.AbilityProtectYourself;
                model.AbilityReceiveSupport = data.AbilityReceiveSupport;
                model.TotalAbilityHigh = data.TotalAbilityHigh;
                model.TotalAbilityAverage = data.TotalAbilityAverage;
                model.TotalAbilityLow = data.TotalAbilityLow;
                model.Result = data.Result;
                model.UnitProvideCare = data.UnitProvideCare;
                model.UnitProvideLiving = data.UnitProvideLiving;
                model.ServiceProvideLiving = data.ServiceProvideLiving;
                model.ServiceProvideCare = data.ServiceProvideCare;
                model.LevelHarmNote = data.LevelHarmNote;
                model.LevelHarmContinueNote = data.LevelHarmContinueNote;
                model.AbilityProtectYourselfNote = data.AbilityProtectYourselfNote;
                model.AbilityReceiveSupportNote = data.AbilityReceiveSupportNote;
            }
            return model;
        }
        public string UpdateEvaluationFirstModel(EvaluationFirstModel data)
        {
            try
            {
                var reportProfile = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(data.ReportProfileId));
                bool IsAdd = false;
                var model = db.EvaluationFirsts.FirstOrDefault(u => u.ReportProfileId.Equals(data.ReportProfileId));
                if (model == null)
                {
                    IsAdd = true;
                    model = new EvaluationFirst();

                }
                model.ReportProfileId = data.ReportProfileId;
                model.PerformingDate = data.PerformingDate;
                model.LevelHarm = data.LevelHarm;
                model.LevelHarmContinue = data.LevelHarmContinue;
                model.TotalLevelHigh = data.TotalLevelHigh;
                model.TotalLevelAverage = data.TotalLevelAverage;
                model.TotalLevelLow = data.TotalLevelLow;
                model.AbilityProtectYourself = data.AbilityProtectYourself;
                model.AbilityReceiveSupport = data.AbilityReceiveSupport;
                model.TotalAbilityHigh = data.TotalAbilityHigh;
                model.TotalAbilityAverage = data.TotalAbilityAverage;
                model.TotalAbilityLow = data.TotalAbilityLow;
                model.Result = data.Result;
                model.UnitProvideCare = data.UnitProvideCare;
                model.UnitProvideLiving = data.UnitProvideLiving;
                model.UpdateBy = data.CreateBy;
                model.ServiceProvideLiving = data.ServiceProvideLiving;
                model.ServiceProvideCare = data.ServiceProvideCare;
                model.LevelHarmNote = data.LevelHarmNote;
                model.LevelHarmContinueNote = data.LevelHarmContinueNote;
                model.AbilityProtectYourselfNote = data.AbilityProtectYourselfNote;
                model.AbilityReceiveSupportNote = data.AbilityReceiveSupportNote;
                model.UpdateDate = DateTime.Now;
                if (IsAdd)
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.CreateBy = data.CreateBy;
                    model.CreateDate = DateTime.Now;
                    db.EvaluationFirsts.Add(model);
                }
                reportProfile.StatusStep2 = true;
                AddReportHistory(reportProfile, 2);
                db.SaveChanges();

                if (data.IsExport)
                {
                    return ExportWordForm2(model.ReportProfileId, model.CreateBy);
                }
            }
            catch (Exception)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            return string.Empty;
        }
        public CaseVerificationModel GetCaseVerificationModel(string reportProfileId)
        {
            CaseVerificationModel model = new CaseVerificationModel();
            var data = db.CaseVerifications.FirstOrDefault(u => u.ReportProfileId.Equals(reportProfileId));
            if (data != null)
            {
                model.Id = data.Id;
                model.ReportProfileId = data.ReportProfileId;
                model.PerformingDate = data.PerformingDate;
                model.PerformingBy = data.PerformingBy;
                model.Condition = data.Condition;
                model.FamilySituation = data.FamilySituation;
                model.CurrentQualityCareOK = data.CurrentQualityCareOK;
                model.CurrentQualityCareNG = data.CurrentQualityCareNG;
                model.PeopleCareFuture = data.PeopleCareFuture;
                model.FutureQualityCareOK = data.FutureQualityCareOK;
                model.FutureQualityCareNG = data.FutureQualityCareNG;
                model.LevelHarm = data.LevelHarm;
                model.LevelApproach = data.LevelApproach;
                model.LevelDevelopmentEffect = data.LevelDevelopmentEffect;
                model.LevelCareObstacle = data.LevelCareObstacle;
                model.LevelNoGuardian = data.LevelNoGuardian;
                model.TotalLevelHigh = data.TotalLevelHigh;
                model.TotalLevelAverage = data.TotalLevelAverage;
                model.TotalLevelLow = data.TotalLevelLow;
                model.AbilityProtectYourself = data.AbilityProtectYourself;
                model.AbilityKnowGuard = data.AbilityKnowGuard;
                model.AbilityEstablishRelationship = data.AbilityEstablishRelationship;
                model.AbilityRelyGuard = data.AbilityRelyGuard;
                model.AbilityHelpOthers = data.AbilityHelpOthers;
                model.TotalAbilityHigh = data.TotalAbilityHigh;
                model.TotalAbilityAverage = data.TotalAbilityAverage;
                model.TotalAbilityLow = data.TotalAbilityLow;
                model.Result = data.Result;
                model.Extend = data.Extend;
                model.ProblemIdentify = data.ProblemIdentify;
                model.ChildAspiration = data.ChildAspiration;
                model.FamilyAspiration = data.FamilyAspiration;
                model.ServiceNeeds = data.ServiceNeeds;
                model.LevelHarmNote = data.LevelHarmNote;
                model.LevelApproachNote = data.LevelApproachNote;
                model.LevelDevelopmentEffectNote = data.LevelDevelopmentEffectNote;
                model.LevelCareObstacleNote = data.LevelCareObstacleNote;
                model.LevelNoGuardianNote = data.LevelNoGuardianNote;
                model.AbilityProtectYourselfNote = data.AbilityProtectYourselfNote;
                model.AbilityKnowGuardNote = data.AbilityKnowGuardNote;
                model.AbilityEstablishRelationshipNote = data.AbilityEstablishRelationshipNote;
                model.AbilityRelyGuardNote = data.AbilityRelyGuardNote;
                model.AbilityHelpOthersNote = data.AbilityHelpOthersNote;
            }
            return model;
        }
        public string UpdateCaseVerificationModel(CaseVerificationModel data)
        {
            try
            {
                var reportProfile = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(data.ReportProfileId));
                bool IsAdd = false;
                var model = db.CaseVerifications.FirstOrDefault(u => u.ReportProfileId.Equals(data.ReportProfileId));
                if (model == null)
                {
                    IsAdd = true;
                    model = new CaseVerification();
                }
                // model.Id = data.Id;
                model.ReportProfileId = data.ReportProfileId;
                model.PerformingDate = data.PerformingDate;
                model.PerformingBy = data.PerformingBy;
                model.Condition = data.Condition;
                model.FamilySituation = data.FamilySituation;
                model.CurrentQualityCareOK = data.CurrentQualityCareOK;
                model.CurrentQualityCareNG = data.CurrentQualityCareNG;
                model.PeopleCareFuture = data.PeopleCareFuture;
                model.FutureQualityCareOK = data.FutureQualityCareOK;
                model.FutureQualityCareNG = data.FutureQualityCareNG;
                model.LevelHarm = data.LevelHarm;
                model.LevelApproach = data.LevelApproach;
                model.LevelDevelopmentEffect = data.LevelDevelopmentEffect;
                model.LevelCareObstacle = data.LevelCareObstacle;
                model.LevelNoGuardian = data.LevelNoGuardian;
                model.TotalLevelHigh = data.TotalLevelHigh;
                model.TotalLevelAverage = data.TotalLevelAverage;
                model.TotalLevelLow = data.TotalLevelLow;
                model.AbilityProtectYourself = data.AbilityProtectYourself;
                model.AbilityKnowGuard = data.AbilityKnowGuard;
                model.AbilityEstablishRelationship = data.AbilityEstablishRelationship;
                model.AbilityRelyGuard = data.AbilityRelyGuard;
                model.AbilityHelpOthers = data.AbilityHelpOthers;
                model.TotalAbilityHigh = data.TotalAbilityHigh;
                model.TotalAbilityAverage = data.TotalAbilityAverage;
                model.TotalAbilityLow = data.TotalAbilityLow;
                model.Result = data.Result;
                model.Extend = data.Extend;
                model.ProblemIdentify = data.ProblemIdentify;
                model.ChildAspiration = data.ChildAspiration;
                model.FamilyAspiration = data.FamilyAspiration;
                model.ServiceNeeds = data.ServiceNeeds;
                model.UpdateBy = data.CreateBy;
                model.UpdateDate = DateTime.Now;
                model.LevelHarmNote = data.LevelHarmNote;
                model.LevelApproachNote = data.LevelApproachNote;
                model.LevelDevelopmentEffectNote = data.LevelDevelopmentEffectNote;
                model.LevelCareObstacleNote = data.LevelCareObstacleNote;
                model.LevelNoGuardianNote = data.LevelNoGuardianNote;
                model.AbilityProtectYourselfNote = data.AbilityProtectYourselfNote;
                model.AbilityKnowGuardNote = data.AbilityKnowGuardNote;
                model.AbilityEstablishRelationshipNote = data.AbilityEstablishRelationshipNote;
                model.AbilityRelyGuardNote = data.AbilityRelyGuardNote;
                model.AbilityHelpOthersNote = data.AbilityHelpOthersNote;
                if (IsAdd)
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.CreateBy = data.CreateBy;
                    model.CreateDate = DateTime.Now;
                    db.CaseVerifications.Add(model);
                }
                reportProfile.StatusStep3 = true;
                AddReportHistory(reportProfile, 3);
                db.SaveChanges();
                if (data.IsExport)
                {
                    return ExportWordForm3(model.ReportProfileId);
                }
            }
            catch (Exception)
            {

                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            return string.Empty;
        }
        public SupportAfterStatusModel GetSupportAfterStatusModel(string id, string reportProfileId)
        {
            SupportAfterStatusModel model = new SupportAfterStatusModel();
            var data = db.SupportAfterStatus.OrderByDescending(u => u.CreateDate).FirstOrDefault(u => u.ReportProfileId.Equals(reportProfileId) && (string.IsNullOrEmpty(id) || id.Equals(u.Id)));
            if (data != null)
            {
                model.Id = data.Id;
                model.ReportProfileId = data.ReportProfileId;
                model.PerformingDate = data.PerformingDate;
                model.PerformingBy = data.PerformingBy;
                model.LevelHarm = data.LevelHarm;
                model.LevelApproach = data.LevelApproach;
                model.LevelCareObstacle = data.LevelCareObstacle;
                model.TotalLevelHigh = data.TotalLevelHigh;
                model.TotalLevelAverage = data.TotalLevelAverage;
                model.TotalLevelLow = data.TotalLevelLow;
                model.AbilityProtectYourself = data.AbilityProtectYourself;
                model.AbilityKnowGuard = data.AbilityKnowGuard;
                model.AbilityHelpOthers = data.AbilityHelpOthers;
                model.TotalAbilityHigh = data.TotalAbilityHigh;
                model.TotalAbilityAverage = data.TotalAbilityAverage;
                model.TotalAbilityLow = data.TotalAbilityLow;
                model.Result = data.Result;

                model.SupportAfterTitle = data.SupportAfterTitle;
                model.LevelHarmNote = data.LevelHarmNote;
                model.LevelApproachNote = data.LevelApproachNote;
                model.LevelCareObstacleNote = data.LevelCareObstacleNote;
                model.AbilityProtectYourselfNote = data.AbilityProtectYourselfNote;
                model.AbilityKnowGuardNote = data.AbilityKnowGuardNote;
                model.AbilityHelpOthersNote = data.AbilityHelpOthersNote;
            }
            try
            {
                var childName = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(reportProfileId)).ChildName;
                model.ChildName = childName;
            }
            catch (Exception)
            { }

            return model;
        }
        public OutputModel UpdateSupportAfterStatusModel(SupportAfterStatusModel data)
        {
            OutputModel output = new OutputModel();
            string Id = "";
            try
            {
                var reportProfile = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(data.ReportProfileId));
                if (string.IsNullOrEmpty(data.Id))
                {
                    data.Id = string.Empty;
                }
                bool IsAdd = false;
                var model = db.SupportAfterStatus.FirstOrDefault(u => u.Id.Equals(data.Id));
                if (model == null)
                {
                    Id = data.Id;
                    IsAdd = true;
                    model = new SupportAfterStatu();
                }
                // model.Id = data.Id;
                model.ReportProfileId = data.ReportProfileId;
                model.PerformingDate = data.PerformingDate;
                model.PerformingBy = data.PerformingBy;
                model.LevelHarm = data.LevelHarm;
                model.LevelApproach = data.LevelApproach;
                model.LevelCareObstacle = data.LevelCareObstacle;
                model.TotalLevelHigh = data.TotalLevelHigh;
                model.TotalLevelAverage = data.TotalLevelAverage;
                model.TotalLevelLow = data.TotalLevelLow;
                model.AbilityProtectYourself = data.AbilityProtectYourself;
                model.AbilityKnowGuard = data.AbilityKnowGuard;
                model.AbilityHelpOthers = data.AbilityHelpOthers;
                model.TotalAbilityHigh = data.TotalAbilityHigh;
                model.TotalAbilityAverage = data.TotalAbilityAverage;
                model.TotalAbilityLow = data.TotalAbilityLow;
                model.Result = data.Result;
                model.UpdateBy = data.CreateBy;
                model.SupportAfterTitle = data.SupportAfterTitle;
                model.LevelHarmNote = data.LevelHarmNote;
                model.LevelApproachNote = data.LevelApproachNote;
                model.LevelCareObstacleNote = data.LevelCareObstacleNote;
                model.AbilityProtectYourselfNote = data.AbilityProtectYourselfNote;
                model.AbilityKnowGuardNote = data.AbilityKnowGuardNote;
                model.AbilityHelpOthersNote = data.AbilityHelpOthersNote;
                model.UpdateDate = DateTime.Now;
                if (IsAdd)
                {
                    model.Id = Guid.NewGuid().ToString();
                    Id = model.Id;
                    model.CreateBy = data.CreateBy;
                    model.CreateDate = DateTime.Now;
                    db.SupportAfterStatus.Add(model);
                }
                reportProfile.StatusStep5 = true;
                AddReportHistory(reportProfile, 5);
                db.SaveChanges();
                if (data.IsExport)
                {
                    output.Path = ExportWordForm5(model.Id, model.ReportProfileId, model.CreateBy);
                }
                output.Id = Id;
            }
            catch (Exception)
            {

                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            return output;
        }
        public SupportPlantModel GetSupportPlantModel(string id, string reportProfileId)
        {
            SupportPlantModel model = new SupportPlantModel();
            var data = db.SupportPlants.OrderByDescending(u => u.CreateDate).FirstOrDefault(u => u.ReportProfileId.Equals(reportProfileId) && (string.IsNullOrEmpty(id) || id.Equals(u.Id)));
            if (data != null)
            {
                model.Id = data.Id;
                model.ReportProfileId = data.ReportProfileId;
                model.PlantDate = data.PlantDate;
                model.TitlePlant = data.TitlePlant;
                model.IsEstimateCost = data.IsEstimateCost;
                model.ActionNote = data.ActionNote;
                model.TargetNote = data.TargetNote;

                model.OrganizationActivities = data.OrganizationActivities;
                if (string.IsNullOrEmpty(model.OrganizationActivities))
                {
                    model.ListOrganizationActivities = new List<OrganizationActivitiesModel>();
                }
                else
                {
                    model.ListOrganizationActivities = JsonConvert.DeserializeObject<List<OrganizationActivitiesModel>>(data.OrganizationActivities);
                }
                model.ListProfileAttachment = (from a in db.EstimateCostAttachments
                                               where a.SupportPlantId.Equals(model.Id)
                                               select new ProfileAttachmentModel
                                               { Id = a.Id, Name = a.Name, Size = a.Size, Path = a.Path, UploadDateRoot = a.UploadDate }).ToList();
                foreach (var item in model.ListProfileAttachment)
                {
                    item.UploadDate = item.UploadDateRoot.Value.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                model.ListProfileAttachment = new List<ProfileAttachmentModel>();
                model.ListProfileAttachmentUpdate = new List<ProfileAttachmentModel>();
                model.ListOrganizationActivities = new List<OrganizationActivitiesModel>();
            }
            return model;
        }
        public OutputModel UpdateSupportPlantModel(SupportPlantModel data, HttpFileCollection httpFile)
        {
            OutputModel output = new OutputModel();
            string Id = "";
            if (string.IsNullOrEmpty(data.Id))
            {
                data.Id = string.Empty;
            }
            try
            {
                var reportProfile = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(data.ReportProfileId));
                var model = db.SupportPlants.FirstOrDefault(u => u.Id.Equals(data.Id));
                if (model == null)
                {
                    model = new SupportPlant();
                    model.ReportProfileId = data.ReportProfileId;
                    model.PlantDate = data.PlantDate;

                    model.OrganizationActivities = JsonConvert.SerializeObject(data.ListOrganizationActivities);
                    model.CreateBy = data.CreateBy;
                    model.TitlePlant = data.TitlePlant;
                    model.IsEstimateCost = data.IsEstimateCost;
                    model.TargetNote = data.TargetNote;
                    model.ActionNote = data.ActionNote;
                    model.CreateDate = DateTime.Now;
                    model.UpdateBy = data.CreateBy;
                    model.UpdateDate = DateTime.Now;
                    model.Id = Guid.NewGuid().ToString();
                    model.IsRemind75 = false;
                    model.IsRemind90 = false;
                    db.SupportPlants.Add(model);
                    #region[upload file]
                    if (data.IsEstimateCost == true)
                    {
                        if (httpFile.Count > 0)
                        {
                            List<string> listFileKey = httpFile.AllKeys.ToList();
                            ProfileAttachmentModel profileAttachmentModel;
                            EstimateCostAttachment estimateCostAttachment;
                            for (int i = 0; i < httpFile.Count; i++)
                            {
                                profileAttachmentModel = data.ListProfileAttachment.FirstOrDefault(r => r.Id.Equals(listFileKey[i]));
                                profileAttachmentModel.Path = Task.Run(async () =>
                                {
                                    return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                                }).Result;

                                estimateCostAttachment = new EstimateCostAttachment()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SupportPlantId = model.Id,
                                    Name = profileAttachmentModel.Name,
                                    Path = profileAttachmentModel.Path,
                                    Size = profileAttachmentModel.Size,
                                    Extension = profileAttachmentModel.Extension,
                                    Description = profileAttachmentModel.Description,
                                    UploadBy = model.CreateBy,
                                    UploadDate = DateTime.Now,
                                };
                                db.EstimateCostAttachments.Add(estimateCostAttachment);
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    model.ReportProfileId = data.ReportProfileId;
                    model.PlantDate = data.PlantDate;

                    model.OrganizationActivities = JsonConvert.SerializeObject(data.ListOrganizationActivities);
                    model.UpdateBy = data.CreateBy;
                    model.UpdateDate = DateTime.Now;
                    model.TitlePlant = data.TitlePlant;
                    model.IsEstimateCost = data.IsEstimateCost;
                    model.TargetNote = data.TargetNote;
                    model.ActionNote = data.ActionNote;
                    #region[xóa file nào bị xóa]
                    if (data.ListProfileAttachmentUpdate == null)
                    {
                        data.ListProfileAttachmentUpdate = new List<ProfileAttachmentModel>();
                    }
                    var lstIdFile = data.ListProfileAttachmentUpdate.Select(u => u.Id).ToList();
                    var allFile = db.EstimateCostAttachments.Where(u => u.SupportPlantId.Equals(model.Id)).ToList();
                    foreach (var item in allFile)
                    {
                        if (!lstIdFile.Contains(item.Id))
                        {
                            db.EstimateCostAttachments.Remove(item);
                        }
                    }

                    #endregion
                    #region[upload file]
                    if (data.IsEstimateCost == true)
                    {
                        if (httpFile.Count > 0)
                        {
                            List<string> listFileKey = httpFile.AllKeys.ToList();
                            ProfileAttachmentModel profileAttachmentModel;
                            EstimateCostAttachment estimateCostAttachment;
                            for (int i = 0; i < httpFile.Count; i++)
                            {
                                profileAttachmentModel = data.ListProfileAttachment.FirstOrDefault(r => r.Id.Equals(listFileKey[i]));
                                profileAttachmentModel.Path = Task.Run(async () =>
                                {
                                    return await AzureStorageUploadFiles.GetInstance().UploadFileAsync(httpFile[i], httpFile[i].FileName, Constants.FolderReportProfile);
                                }).Result;

                                estimateCostAttachment = new EstimateCostAttachment()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    SupportPlantId = model.Id,
                                    Name = profileAttachmentModel.Name,
                                    Path = profileAttachmentModel.Path,
                                    Size = profileAttachmentModel.Size,
                                    Extension = profileAttachmentModel.Extension,
                                    Description = profileAttachmentModel.Description,
                                    UploadBy = model.CreateBy,
                                    UploadDate = DateTime.Now,
                                };
                                db.EstimateCostAttachments.Add(estimateCostAttachment);
                            }
                        }
                    }
                    else
                    {
                        //xóa hết file
                        var fileRomote = db.EstimateCostAttachments.Where(u => u.SupportPlantId.Equals(model.Id));
                        if (fileRomote.Count() > 0)
                        {
                            db.EstimateCostAttachments.RemoveRange(fileRomote);
                        }
                    }
                    #endregion
                }

                Id = model.Id;
                reportProfile.StatusStep4 = true;
                AddReportHistory(reportProfile, 4);
                db.SaveChanges();
                if (data.IsExport)
                {
                    output.Path = ExportWordForm4(model.Id, model.ReportProfileId);
                }
                output.Id = Id;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            return output;
        }
        public void UpdateStatusReportForword(string id)
        {
            try
            {
                var data = db.ReportForwards.FirstOrDefault(u => u.Id.Equals(id));
                if (data != null)
                {
                    if (data.Status.Equals("1"))
                    {
                        data.Status = "0";
                    }
                    else
                    {
                        data.Status = "1";
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { }
        }
        public List<ComboboxResult> GenTitleStep5View(string reportProfileId)
        {
            List<ComboboxResult> model = new List<ComboboxResult>();
            var data = db.SupportAfterStatus.Where(u => u.ReportProfileId.Equals(reportProfileId)).ToList();
            if (data.Count > 0)
            {
                model = (from a in data
                         orderby a.CreateDate
                         select new ComboboxResult
                         {
                             Id = a.Id,
                             Name = a.SupportAfterTitle,
                             PId = a.PerformingBy,
                             Exten = a.PerformingDate.Value.ToString("dd/MM/yyyy"),
                         }).ToList();
            }
            return model;
        }
        public List<ComboboxResult> GenTitleStep4View(string reportProfileId)
        {
            List<ComboboxResult> model = new List<ComboboxResult>();
            var data = db.SupportPlants.Where(u => u.ReportProfileId.Equals(reportProfileId)).ToList();
            if (data.Count > 0)
            {
                model = (from a in data
                         orderby a.CreateDate
                         select new ComboboxResult
                         {
                             Id = a.Id,
                             Name = a.TitlePlant,
                             // PId = a.PerformingBy,
                             Exten = a.PlantDate.Value.ToString("dd/MM/yyyy"),
                         }).ToList();
            }
            return model;
        }
        public void DeleteSupportAfter(string id)
        {
            try
            {
                var data = db.SupportAfterStatus.FirstOrDefault(u => u.Id.Equals(id));
                if (data != null)
                {
                    db.SupportAfterStatus.Remove(data);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { }
        }
        public void DeleteSupportPlant(string id)
        {
            try
            {
                var data = db.SupportPlants.FirstOrDefault(u => u.Id.Equals(id));
                if (data != null)
                {
                    db.SupportPlants.Remove(data);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            { }
        }
        public void SendForward(ReportForwardModel model)
        {
            var createBy = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(createBy);
            var data = db.ReportProfiles.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            ReportForward reportForward = new ReportForward();
            try
            {

                reportForward.Id = Guid.NewGuid().ToString();
                reportForward.CreateBy = createBy;
                reportForward.CreateDate = DateTime.Now;
                reportForward.ReportProfileId = model.Id;
                reportForward.ForwardNote = model.ForwardNote;
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    reportForward.ForwardLevel = Constants.LevelArea;
                }
                else if (userInfo.Type == Constants.LevelArea)
                {
                    reportForward.ForwardLevel = Constants.LevelOffice;
                }
                db.ReportForwards.Add(reportForward);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title);
            }
            #region[cache notify]
            try
            {
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                List<User> userNotify = new List<User>();
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    userNotify = db.Users.Where(u => u.DistrictId.Equals(userInfo.DistrictId) && u.Type == Constants.LevelArea && u.IsDisable == false).ToList();
                }
                else if (userInfo.Type == Constants.LevelArea)
                {
                    userNotify = db.Users.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId) && u.Type == Constants.LevelOffice && u.IsDisable == false).ToList();
                }
                SendMailAndNotify(data, userNotify, " " + Resource.Resource.Label_Forward, true, reportForward.Id);
            }
            catch (Exception)
            { }
            #endregion
        }

        public void SendMailAndNotify(ReportProfile reportProfile, List<User> userNotify, string actionTitle, bool IsViewForward, string reportForwardId)
        {
            var abuse = db.ReportProfileAbuseTypes.Where(u => u.ReportProfileId.Equals(reportProfile.Id)).Select(u => u.AbuseTypeName).ToList();
            string levelView = Common.GenLevel(reportProfile.SeverityLevel.Value);
            MailModel mailModel;
            string mailSend = ConfigurationManager.AppSettings["MailSend"];
            string mailPass = ConfigurationManager.AppSettings["MailPass"];
            string title = "";
            string content = "";
            string objJson = "";
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var userId = System.Web.HttpContext.Current.User.Identity.Name;
                    RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                    var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                    NotifyModel notifyModel;
                    var dateNow = DateTime.Now;
                    string ageStr = "";
                    if (reportProfile.Age != null)
                    {
                        ageStr = " (" + reportProfile.Age + " " + Resource.Resource.ReportProfile_Age + ")";
                    }
                    title = "Trường hợp " + actionTitle + " trẻ: " + reportProfile.ChildName + ageStr + ", xâm hại: " + string.Join(";", abuse) + ", mức nghiêm trọng: " + levelView;
                    content += "<p><b>" + "Thông tin trẻ bị xâm hại" + ":</b></p>";
                    content += "<p>" + "Họ và tên trẻ" + ": " + reportProfile.ChildName + "</p>";
                    content += "<p>" + "Tuổi" + ": " + reportProfile.Age + "</p>";
                    content += "<p>" + "Loại hình xâm hại" + ": " + string.Join(";", abuse) + "</p>";
                    content += "<p>" + "Mức độ nghiêm trọng" + ": " + levelView + "</p>";
                    content += "<p>" + "Địa chỉ" + ": " + reportProfile.FullAddress + "</p>";
                    content += "<p>" + "Thời gian gửi" + ": " + dateNow.ToString("dd/MM/yyyy HH:mm") + "</p>";
                    string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                    TimeSpan ts = new TimeSpan(24 * 30, 0, 0);

                    Notify notify = new Notify();
                    foreach (var item in userNotify)
                    {
                        notifyModel = new NotifyModel();
                        notifyModel.Id = Guid.NewGuid().ToString();
                        notifyModel.Addres = reportProfile.FullAddress;
                        notifyModel.CreateDate = dateNow;
                        notifyModel.Status = Constants.NotViewNotification;
                        notifyModel.Title = title;
                        notifyModel.Link = "/ReportProfile/ReportDetail/" + reportProfile.Id;
                        if (IsViewForward)
                        {
                            notifyModel.Link = "/ReportProfile/ReportDetail/" + reportProfile.Id + "?view=" + reportForwardId;
                        }

                        // Lưu Notify vào Cache
                        redisService.Add(cacheNotify + notifyModel.Id, notifyModel, ts);

                        // Lưu Notify vào Database
                        notify = new Notify()
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = item.Id,
                            NotifyKey = notifyModel.Id,
                            CreateDate = DateTime.Now
                        };

                        db.Notifies.Add(notify);

                        mailModel = new MailModel();
                        mailModel.Title = title;
                        mailModel.Content = content;
                        mailModel.MailInbox = item.Email;
                        objJson = JsonConvert.SerializeObject(mailModel);
                        AddQuieAsync(objJson);
                    }

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("ReportProfileBusiness.SendMailAndNotify", ex.Message, reportProfile);
                }
            }
        }

        public string DetailForward(string id)
        {
            var data = db.ReportForwards.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw new Exception(Resource.Resource.ErroNotFound_Title);
            }
            try
            {

                return data.ForwardNote;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void AddQuieAsync(string objJson)
        {
            try
            {
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue messageQueue = queueClient.GetQueueReference(mailservicequeue);
                // Create a message to add to the queue.
                CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(objJson);
                // Async enqueue the message.
                messageQueue.AddMessageAsync(cloudQueueMessage);
            }
            catch (Exception ex)
            {
            }
        }
        public ReportForward GetForwardInfo(string id)
        {
            try
            {
                var data = db.ReportForwards.FirstOrDefault(u => u.Id.Equals(id));
                return data;
            }
            catch (Exception)
            { return null; }
        }

        /// <summary>
        /// Xuất biểu mẫu 01 step 01
        /// </summary>
        public string ExportWordForm1(string profileId, string createBy)
        {
            try
            {
                ReportProfileModel reportProfile = GetInfo(profileId);

                string templatePath = "/Template/Mau1.docx";
                string outPath = $"/Template/Export/Mau01_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.docx";
                WordDocument wordDocument = new WordDocument(HttpContext.Current.Server.MapPath(templatePath), FormatType.Docx);
                wordDocument.Open(HttpContext.Current.Server.MapPath(templatePath));

                var itemWard = db.Wards.FirstOrDefault(r => r.Id.Equals(reportProfile.WardId));
                var users = db.Users.FirstOrDefault(r => r.Id.Equals(createBy));

                FindReplaceContent(wordDocument, "<WardType>", itemWard != null ? itemWard.Type.ToUpper() : "");
                FindReplaceContent(wordDocument, "<WardNameUp>", itemWard != null ? itemWard.Name.ToUpper() : "");
                FindReplaceContent(wordDocument, "<WardName>", itemWard != null ? itemWard.Name : "");
                FindReplaceContent(wordDocument, "<DateNow>", DateTimeUtils.ConvertDateText(reportProfile.ReceptionDate.Value).ToLower());
                FindReplaceContent(wordDocument, "<WordTitle>", reportProfile.WordTitle.ToUpper());

                string[] sourceType = { "Điện thoại", "Gặp trực tiếp", "Người khác báo" };
                FindReplaceContent(wordDocument, "<SourceType>", sourceType[reportProfile.InformationSources]);
                FindReplaceContent(wordDocument, "<SourceNote>", reportProfile.SourceNote);
                FindReplaceContent(wordDocument, "<ReceptionTime>", reportProfile.ReceptionTime.Replace(":", "h"));
                FindReplaceContent(wordDocument, "<ReceptionDate>", DateTimeUtils.ConvertDateText(reportProfile.ReceptionDate));
                FindReplaceContent(wordDocument, "<ChildName>", reportProfile.ChildName);

                FindReplaceContent(wordDocument, "<ChildBirthdate>", reportProfile.ChildBirthdate != null ? DateTimeUtils.ConvertDateToDDMMYYYY(reportProfile.ChildBirthdate) : "            ");
                FindReplaceContent(wordDocument, "<Age>", reportProfile.Age != null ? reportProfile.Age.ToString() : string.Empty);

                FindReplaceContent(wordDocument, "<Gender>", Common.GenGender(reportProfile.Gender.Value));
                FindReplaceContent(wordDocument, "<CaseLocation>", reportProfile.CaseLocation);
                FindAppendHTML(wordDocument, "<CurrentHealth>", reportProfile.CurrentHealth);
                FindAppendHTML(wordDocument, "<SequelGuess>", reportProfile.SequelGuess);
                FindReplaceContent(wordDocument, "<FatherName>", reportProfile.FatherName);
                FindReplaceContent(wordDocument, "<FatherAge>", reportProfile.FatherAge.HasValue ? (DateTime.Now.Year - reportProfile.FatherAge).ToString() : "");
                FindReplaceContent(wordDocument, "<FatherJob>", reportProfile.FatherJob);
                FindReplaceContent(wordDocument, "<MotherName>", reportProfile.MotherName);
                FindReplaceContent(wordDocument, "<MotherAge>", reportProfile.MotherAge.HasValue ? (DateTime.Now.Year - reportProfile.MotherAge).ToString() : "");
                FindReplaceContent(wordDocument, "<MotherJob>", reportProfile.MotherJob);
                FindAppendHTML(wordDocument, "<FamilySituation>", reportProfile.FamilySituation);
                FindAppendHTML(wordDocument, "<TakeCare>", reportProfile.PeopleCare);
                FindAppendHTML(wordDocument, "<Support>", reportProfile.Support);
                FindReplaceContent(wordDocument, "<ProviderName>", reportProfile.ProviderName);
                FindReplaceContent(wordDocument, "<ProviderPhone>", reportProfile.ProviderPhone);
                FindReplaceContent(wordDocument, "<ProviderAddress>", reportProfile.ProviderAddress);
                FindReplaceContent(wordDocument, "<ProviderNote>", reportProfile.ProviderNote);
                FindReplaceContent(wordDocument, "<OfficerName>", users.FullName);
                wordDocument.Save(HttpContext.Current.Server.MapPath(outPath));

                return outPath;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        /// <summary>
        ///  Find replate nội dung
        /// </summary>
        /// <param name="wordDocument"></param>
        /// <param name="textFind"></param>
        /// <param name="textReplace"></param>
        private void FindReplaceContent(WordDocument wordDocument, string textFind, string textReplace)
        {
            TextSelection textSelections = wordDocument.Find(textFind, false, true);
            if (textSelections != null)
            {
                WTextRange textRange = textSelections.GetAsOneRange();
                textRange.Text = textReplace;
            }
        }

        /// <summary>
        ///  Find replate nội dung
        /// </summary>
        /// <param name="wordDocument"></param>
        /// <param name="textFind"></param>
        /// <param name="textReplace"></param>
        private void FindReplaceAllContent(WordDocument wordDocument, string textFind, string textReplace)
        {
            TextSelection[] selections = wordDocument.FindAll(textFind, false, true);
            if (selections != null)
            {
                foreach (var itemSelections in selections)
                {
                    WTextRange textRange = itemSelections.GetAsOneRange();
                    textRange.Text = textReplace;
                }
            }
        }

        /// <summary>
        ///  Find replate nội dung
        /// </summary>
        /// <param name="wordDocument"></param>
        /// <param name="textFind"></param>
        /// <param name="textReplace"></param>
        /// <summary>
        ///  Find replate nội dung
        /// </summary>
        /// <param name="wordDocument"></param>
        /// <param name="textFind"></param>
        /// <param name="textReplace"></param>
        private void FindAppendHTML(WordDocument wordDocument, string textFind, string html)
        {
            TextSelection textSelections = wordDocument.Find(textFind, false, true);
            if (textSelections != null)
            {
                WParagraph textRange = textSelections.GetAsOneRange().OwnerParagraph;
                textRange.Text = textRange.Text.Replace(textFind, string.Empty);
                textRange.AppendHTML(html);
            }
        }

        public object GetContentEdit(string id)
        {
            var data = (from a in db.ReportProfiles.AsNoTracking()
                        where a.Id.Equals(id)
                        select new
                        {
                            CurrentHealth = a.CurrentHealth,
                            SequelGuess = a.SequelGuess,
                            FamilySituation = a.FamilySituation,
                            Support = a.Support,
                            SummaryCase = a.SummaryCase
                        }
                        ).FirstOrDefault();
            return data;
        }

        /// Xuất biểu mẫu 02 step 02
        public string ExportWordForm2(string profileId, string createBy)
        {
            try
            {
                EvaluationFirstModel evaluationFirst = GetEvaluationFirstModel(profileId);
                var users = db.Users.FirstOrDefault(r => r.Id.Equals(createBy));

                string templatePath = "/Template/Mau2.docx";
                string outPath = $"/Template/Export/Mau02_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.docx";
                WordDocument wordDocument = new WordDocument(HttpContext.Current.Server.MapPath(templatePath), FormatType.Docx, XHTMLValidationType.None);
                wordDocument.Open(HttpContext.Current.Server.MapPath(templatePath));

                FindReplaceContent(wordDocument, "<EvPerformingDate>", DateTimeUtils.ConvertDateText(evaluationFirst.PerformingDate).ToLower());
                FindReplaceContent(wordDocument, "<EvLevelHarmNote>", evaluationFirst.LevelHarmNote != null ? "(" + evaluationFirst.LevelHarmNote + ")" : "");
                FindReplaceContent(wordDocument, "<EvLevelHarmContinueNote>", evaluationFirst.LevelHarmContinueNote != null ? "(" + evaluationFirst.LevelHarmContinueNote + ")" : "");
                FindReplaceContent(wordDocument, "<EvTotalHigh>", evaluationFirst.TotalLevelHigh.ToString());
                FindReplaceContent(wordDocument, "<EvTotalAvg>", evaluationFirst.TotalLevelAverage.ToString());
                FindReplaceContent(wordDocument, "<EvTotalLevelLow>", evaluationFirst.TotalLevelLow.ToString());
                FindReplaceContent(wordDocument, "<EvAbilityProtectYourselfNote>", evaluationFirst.AbilityProtectYourselfNote != null ? "(" + evaluationFirst.AbilityProtectYourselfNote + ")" : "");
                FindReplaceContent(wordDocument, "<EvAbilityReceiveSupportNote>", evaluationFirst.AbilityReceiveSupportNote != null ? "(" + evaluationFirst.AbilityReceiveSupportNote + ")" : "");
                FindReplaceContent(wordDocument, "<EvAbiHig>", evaluationFirst.TotalAbilityHigh.ToString());
                FindReplaceContent(wordDocument, "<EvAbiAvg>", evaluationFirst.TotalAbilityAverage.ToString());
                FindReplaceContent(wordDocument, "<EvAbiLow>", evaluationFirst.TotalAbilityLow.ToString());
                FindReplaceContent(wordDocument, "<EvResult>", evaluationFirst.Result != null ? evaluationFirst.Result : "");

                FindReplaceContent(wordDocument, "<PlaceService>", evaluationFirst.ServiceProvideLiving);
                FindReplaceContent(wordDocument, "<PlaceProvider>", evaluationFirst.UnitProvideLiving);
                FindReplaceContent(wordDocument, "<HealthService>", evaluationFirst.ServiceProvideCare);
                FindReplaceContent(wordDocument, "<HealthProvider>", evaluationFirst.UnitProvideCare);

                string[] level = { "Thấp", "Trung bình", "Cao" };

                FindReplaceContent(wordDocument, "<LevelHarm>", GetLevel(level, evaluationFirst.LevelHarm));
                FindReplaceContent(wordDocument, "<LevelHarmContinue>", GetLevel(level, evaluationFirst.LevelHarmContinue));
                FindReplaceContent(wordDocument, "<LevelAbilityProtectYourself>", GetLevel(level, evaluationFirst.AbilityProtectYourself));
                FindReplaceContent(wordDocument, "<LevelAbilityReceiveSupport>", GetLevel(level, evaluationFirst.AbilityReceiveSupport));
                FindReplaceContent(wordDocument, "<OfficerName>", users.FullName);

                wordDocument.Save(HttpContext.Current.Server.MapPath(outPath));

                return outPath;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        /// Xuất biểu mẫu 03 step 03
        public string ExportWordForm3(string profileId)
        {
            CaseVerificationModel caseVerificationModel = GetCaseVerificationModel(profileId);
            ReportProfileModel reportProfile = GetInfo(profileId);
            try
            {
                string templatePath = "/Template/Mau3.docx";
                string outPath = $"/Template/Export/Mau03_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.docx";
                WordDocument wordDocument = new WordDocument(HttpContext.Current.Server.MapPath(templatePath), FormatType.Docx);
                wordDocument.Open(HttpContext.Current.Server.MapPath(templatePath));

                FindReplaceContent(wordDocument, "<Name>", reportProfile.ChildName);
                FindReplaceContent(wordDocument, "<CaPerformingDate>", DateTimeUtils.ConvertDateText(caseVerificationModel.PerformingDate).ToLower());
                FindReplaceContent(wordDocument, "<CaPerformingBy>", caseVerificationModel.PerformingBy);
                FindReplaceContent(wordDocument, "<CaCondition>", caseVerificationModel.Condition);
                FindReplaceContent(wordDocument, "<CaFamilySituation>", caseVerificationModel.FamilySituation);
                FindReplaceContent(wordDocument, "<CaCurrentQualityCareOK>", caseVerificationModel.CurrentQualityCareOK != null ? caseVerificationModel.CurrentQualityCareOK : "");
                FindReplaceContent(wordDocument, "<CaCurrentQualityCareNG>", caseVerificationModel.CurrentQualityCareNG != null ? caseVerificationModel.CurrentQualityCareNG : "");
                FindReplaceContent(wordDocument, "<CaPeopleCareFuture>", caseVerificationModel.PeopleCareFuture);
                FindReplaceContent(wordDocument, "<CaFutureQualityCareOK>", caseVerificationModel.FutureQualityCareOK != null ? caseVerificationModel.FutureQualityCareOK : "");
                FindReplaceContent(wordDocument, "<CaFutureQualityCareNG>", caseVerificationModel.FutureQualityCareNG != null ? caseVerificationModel.FutureQualityCareNG : "");
                FindReplaceContent(wordDocument, "<CaLevelHarmNote>", caseVerificationModel.LevelHarmNote != null ? "(" + caseVerificationModel.LevelHarmNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaLevelApproachNote>", caseVerificationModel.LevelApproachNote != null ? "(" + caseVerificationModel.LevelApproachNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaLevelDevelopmentEffectNote>", caseVerificationModel.LevelDevelopmentEffectNote != null ? "(" + caseVerificationModel.LevelDevelopmentEffectNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaLevelCareObstacleNote>", caseVerificationModel.LevelCareObstacleNote != null ? "(" + caseVerificationModel.LevelCareObstacleNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaLevelNoGuardianNote>", caseVerificationModel.LevelNoGuardianNote != null ? "(" + caseVerificationModel.LevelNoGuardianNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaAbilityProtectYourselfNote>", caseVerificationModel.AbilityProtectYourselfNote != null ? "(" + caseVerificationModel.AbilityProtectYourselfNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaAbilityKnowGuardNote>", caseVerificationModel.AbilityKnowGuardNote != null ? "(" + caseVerificationModel.AbilityKnowGuardNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaAbilityEstablishRelationshipNote>", caseVerificationModel.AbilityEstablishRelationshipNote != null ? caseVerificationModel.AbilityEstablishRelationshipNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaAbilityRelyGuardNote>", caseVerificationModel.AbilityRelyGuardNote != null ? "(" + caseVerificationModel.AbilityRelyGuardNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaAbilityHelpOthersNote>", caseVerificationModel.AbilityHelpOthersNote != null ? "(" + caseVerificationModel.AbilityHelpOthersNote + ")" : "");
                FindReplaceContent(wordDocument, "<CaResult>", caseVerificationModel.Result != null ? caseVerificationModel.Result : "");
                FindReplaceContent(wordDocument, "<CaProblemIdentify>", caseVerificationModel.ProblemIdentify != null ? caseVerificationModel.ProblemIdentify : "");
                FindReplaceContent(wordDocument, "<CaChildAspiration>", caseVerificationModel.ChildAspiration != null ? caseVerificationModel.ChildAspiration : "");
                FindReplaceContent(wordDocument, "<CaFamilyAspiration>", caseVerificationModel.FamilyAspiration != null ? caseVerificationModel.FamilyAspiration : "");
                FindReplaceContent(wordDocument, "<CaServiceNeeds>", caseVerificationModel.ServiceNeeds != null ? caseVerificationModel.ServiceNeeds : "");

                FindReplaceContent(wordDocument, "<CaTotalLevelHigh>", caseVerificationModel.TotalLevelHigh.ToString());
                FindReplaceContent(wordDocument, "<CaTotalLevelAverage>", caseVerificationModel.TotalLevelAverage.ToString());
                FindReplaceContent(wordDocument, "<CaTotalLevelLow>", caseVerificationModel.TotalLevelLow.ToString());
                FindReplaceContent(wordDocument, "<CaTotalAbilityHigh>", caseVerificationModel.TotalAbilityHigh.ToString());
                FindReplaceContent(wordDocument, "<CaTotalAbilityAverage>", caseVerificationModel.TotalAbilityAverage.ToString());
                FindReplaceContent(wordDocument, "<CaTotalAbilityLow>", caseVerificationModel.TotalAbilityLow.ToString());

                string[] level = { "Thấp", "Trung bình", "Cao" };

                FindReplaceContent(wordDocument, "<CaLevelHarm>", GetLevel(level, caseVerificationModel.LevelHarm));
                FindReplaceContent(wordDocument, "<CaLevelApproach>", GetLevel(level, caseVerificationModel.LevelApproach));
                FindReplaceContent(wordDocument, "<CaLevelCareObstacle>", GetLevel(level, caseVerificationModel.LevelCareObstacle));
                FindReplaceContent(wordDocument, "<CaLevelNoGuardian>", GetLevel(level, caseVerificationModel.LevelNoGuardian));
                FindReplaceContent(wordDocument, "<CaAbilityProtectYourself>", GetLevel(level, caseVerificationModel.AbilityProtectYourself));
                FindReplaceContent(wordDocument, "<CaAbilityKnowGuard>", GetLevel(level, caseVerificationModel.AbilityKnowGuard));
                FindReplaceContent(wordDocument, "<CaAbilityEstablishRelationship>", GetLevel(level, caseVerificationModel.AbilityEstablishRelationship));
                FindReplaceContent(wordDocument, "<CaAbilityRelyGuard>", GetLevel(level, caseVerificationModel.AbilityRelyGuard));
                FindReplaceContent(wordDocument, "<CaAbilityHelpOthers>", GetLevel(level, caseVerificationModel.AbilityHelpOthers));
                FindReplaceContent(wordDocument, "<CaLevelDevelopmentEffect>", GetLevel(level, caseVerificationModel.LevelDevelopmentEffect));

                wordDocument.Save(HttpContext.Current.Server.MapPath(outPath));

                return outPath;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        private string GetLevel(string[] level, int? index)
        {
            string result = string.Empty;
            try
            {
                result = level[index.Value].ToString();
            }
            catch
            {
                // Nothing todo
            }

            return result;
        }

        /// Xuất biểu mẫu 04 step 04
        public string ExportWordForm4(string id, string profileId)
        {
            SupportPlantModel supportPlantModel = GetSupportPlantModel(id, profileId);
            ReportProfileModel reportProfile = GetInfo(profileId);
            try
            {
                string templatePath = "/Template/Mau4.docx";
                string outPath = $"/Template/Export/Mau04_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.docx";
                WordDocument wordDocument = new WordDocument(HttpContext.Current.Server.MapPath(templatePath), FormatType.Docx);
                wordDocument.Open(HttpContext.Current.Server.MapPath(templatePath));

                var itemWard = db.Wards.FirstOrDefault(r => r.Id.Equals(reportProfile.WardId));

                WTable table = wordDocument.GetTableByFindText("<dt>");
                wordDocument.NTSReplaceFirst("<dt>", "");
                WTableRow templateRow;
                WTableRow row;
                int index = 1;
                templateRow = table.Rows[1].Clone();
                foreach (var e in supportPlantModel.ListOrganizationActivities)
                {
                    //     outPath = string.Empty;
                    if (index > 1)
                    {
                        table.Rows.Insert(index, templateRow.Clone());
                    }
                    row = table.Rows[index];
                    row.Cells[0].Paragraphs[0].Text = (index).ToString();
                    row.Cells[1].Paragraphs[0].Text = e.Name != null ? e.Name : "";
                    row.Cells[2].Paragraphs[0].Text = e.UserName != null ? e.UserName : "";
                    row.Cells[3].Paragraphs[0].Text = e.UserOther != null ? e.UserOther : "";
                    row.Cells[4].Paragraphs[0].Text = e.DateAction;
                    index++;
                }

                FindReplaceContent(wordDocument, "<WardType>", itemWard != null ? itemWard.Type.ToUpper() : "");
                FindReplaceContent(wordDocument, "<WardNameUp>", itemWard != null ? itemWard.Name.ToUpper() : "");
                FindReplaceContent(wordDocument, "<WardName>", itemWard != null ? itemWard.Name : "");
                FindReplaceContent(wordDocument, "<DateNow>", DateTimeUtils.ConvertDateText(supportPlantModel.PlantDate).ToLower());
                FindReplaceContent(wordDocument, "<Name>", reportProfile.ChildName);
                FindReplaceContent(wordDocument, "<PlanName>", supportPlantModel.TitlePlant);
                FindReplaceContent(wordDocument, "<TargetNote>", supportPlantModel.TargetNote);
                FindAppendHTML(wordDocument, "<ActionNote>", supportPlantModel.ActionNote);

                // Trường hợp sử dụng kinh phí
                if (supportPlantModel.IsEstimateCost.HasValue && supportPlantModel.IsEstimateCost.Value)
                {
                    FindAppendHTML(wordDocument, "<Quotation>", "(Dự toán kinh phí chi tiết kèm theo)./.");
                }
                else
                {
                    FindAppendHTML(wordDocument, "<Quotation>", "Không sử dụng kinh phí cho các hoạt động này.");
                }

                wordDocument.Save(HttpContext.Current.Server.MapPath(outPath));

                return outPath;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        /// Xuất biểu mẫu 05 step 05
        public string ExportWordForm5(string id, string profileId, string creatby)
        {
            SupportAfterStatusModel supportAfterStatusModel = GetSupportAfterStatusModel(id, profileId);
            ReportProfileModel reportProfile = GetInfo(profileId);
            try
            {
                string templatePath = "/Template/Mau5.docx";
                string outPath = $"/Template/Export/Mau06_{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.docx";
                WordDocument wordDocument = new WordDocument(HttpContext.Current.Server.MapPath(templatePath), FormatType.Docx);
                wordDocument.Open(HttpContext.Current.Server.MapPath(templatePath));

                var itemWard = db.Wards.FirstOrDefault(r => r.Id.Equals(reportProfile.WardId));
                var users = db.Users.FirstOrDefault(r => r.Id.Equals(creatby));

                FindReplaceContent(wordDocument, "<WardType>", itemWard != null ? itemWard.Type.ToUpper() : "");
                FindReplaceContent(wordDocument, "<WardNameUp>", itemWard != null ? itemWard.Name.ToUpper() : "");
                FindReplaceContent(wordDocument, "<WardName>", itemWard != null ? itemWard.Name : "");
                FindReplaceContent(wordDocument, "<PerformingDate>", DateTimeUtils.ConvertDateText(supportAfterStatusModel.PerformingDate).ToLower());
                FindReplaceContent(wordDocument, "<Name>", reportProfile.ChildName);
                FindReplaceContent(wordDocument, "<OfficerName>", users.FullName);
                FindReplaceContent(wordDocument, "<LevelHarmNote>", supportAfterStatusModel.LevelHarmNote != null ? "(" + supportAfterStatusModel.LevelHarmNote + ")" : string.Empty);
                FindReplaceContent(wordDocument, "<LevelApproachNote>", supportAfterStatusModel.LevelApproachNote != null ? "(" + supportAfterStatusModel.LevelApproachNote + ")" : string.Empty);
                FindReplaceContent(wordDocument, "<LevelCareObstacleNote>", supportAfterStatusModel.LevelCareObstacleNote != null ? "(" + supportAfterStatusModel.LevelCareObstacleNote + ")" : string.Empty);
                FindReplaceContent(wordDocument, "<AbilityProtectYourselfNote>", supportAfterStatusModel.AbilityProtectYourselfNote != null ? "(" + supportAfterStatusModel.AbilityProtectYourselfNote + ")" : string.Empty);
                FindReplaceContent(wordDocument, "<AbilityKnowGuardNote>", supportAfterStatusModel.AbilityKnowGuardNote != null ? "(" + supportAfterStatusModel.AbilityKnowGuardNote + ")" : string.Empty);
                FindReplaceContent(wordDocument, "<AbilityHelpOthersNote>", supportAfterStatusModel.AbilityHelpOthersNote != null ? "(" + supportAfterStatusModel.AbilityHelpOthersNote + ")" : string.Empty);
                FindReplaceContent(wordDocument, "<Result>", supportAfterStatusModel.Result);
                FindReplaceContent(wordDocument, "<TotalLevelHigh>", supportAfterStatusModel.TotalLevelHigh.ToString());
                FindReplaceContent(wordDocument, "<TotalLevelAverage>", supportAfterStatusModel.TotalLevelAverage.ToString());
                FindReplaceContent(wordDocument, "<TotalLevelLow>", supportAfterStatusModel.TotalLevelLow.ToString());
                FindReplaceContent(wordDocument, "<TotalAbilityHigh>", supportAfterStatusModel.TotalAbilityHigh.ToString());
                FindReplaceContent(wordDocument, "<TotalAbilityAverage>", supportAfterStatusModel.TotalAbilityAverage.ToString());
                FindReplaceContent(wordDocument, "<TotalAbilityLow>", supportAfterStatusModel.TotalAbilityLow.ToString());

                string[] level = { "Thấp", "Trung bình", "Cao" };
                FindReplaceContent(wordDocument, "<LevelHarm>", GetLevel(level, supportAfterStatusModel.LevelHarm));
                FindReplaceContent(wordDocument, "<LevelApproach>", GetLevel(level, supportAfterStatusModel.LevelApproach));
                FindReplaceContent(wordDocument, "<LevelCareObstacle>", GetLevel(level, supportAfterStatusModel.LevelCareObstacle));
                FindReplaceContent(wordDocument, "<AbilityHelpOthers>", GetLevel(level, supportAfterStatusModel.AbilityHelpOthers));
                FindReplaceContent(wordDocument, "<AbilityKnowGuard>", GetLevel(level, supportAfterStatusModel.AbilityKnowGuard));
                FindReplaceContent(wordDocument, "<AbilityProtectYourself>", GetLevel(level, supportAfterStatusModel.AbilityProtectYourself));

                wordDocument.Save(HttpContext.Current.Server.MapPath(outPath));
                return outPath;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
        }

        public static WTable GetTableByFindText(WordDocument document, string textFind)
        {
            var text = document.Find(textFind, false, true);
            WTextRange a = text.GetAsOneRange();
            Entity entity = a.Owner;
            while (!(entity is WTable))
            {
                if (entity.Owner != null)
                {
                    entity = entity.Owner;
                }
                else
                    break;
            }

            if (entity is WTable)
            {
                return entity as WTable;
            }
            else
            {
                return null;
            }
        }
    }
}
