using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.CacheModel;
using ChildProfiles.Model.Model.ChildProfileModels;
using ChildProfiles.Model.Model.FliesLibrary;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChildProfiles.Business
{
    public class ChildProfileUpdateBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        public SearchResultObject<ChildProfileSearchResult> SearchChildProfileProvince(ChildProfileSearchCondition searchCondition)
        {
            SearchResultObject<ChildProfileSearchResult> searchResult = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                var listmodel = (from a in db.ChildProfileUpdates.AsNoTracking()
                                 where !a.ProcessStatus.Equals(Constants.CreateNew)
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 join g in db.Ethnics.AsNoTracking() on a.EthnicId equals g.Id
                                 join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into asc
                                 from asc1 in asc.DefaultIfEmpty()
                                 join h in db.Users.AsNoTracking() on a.UpdateBy equals h.Id
                                 orderby a.UpdateDate
                                 select new ChildProfileSearchResult()
                                 {
                                     Avata = a.ImageThumbnailPath,
                                     Id = a.Id,
                                     Name = a.Name,
                                     School = asc1 != null ? asc1.SchoolName : a.SchoolOtherName,
                                     ReligionName = ac1 != null ? ac1.Name : "",
                                     ProgramCode = a.ProgramCode,
                                     NationName = g.Name,
                                     Status = a.ProcessStatus,
                                     ChildCode = a.ChildCode,
                                     ProvinceId = a.ProvinceId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     Address = a.FullAddress,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender == Constants.Male ? "Nam" : "Nữ",
                                     CreateBy = h.Name,
                                     UpdateDate = a.UpdateDate,
                                     CreateDate = a.UpdateDate,
                                     ApproveDate = a.AreaApproverDate,
                                     SalesforceID = a.SaleforceId,
                                     Handicap = a.Handicap.HasValue ? (bool)a.Handicap : false,
                                     HealthHandicap = (a.Health.Contains("\"Id\":\"04\",\"Check\":true") || a.Health.Contains("\"Check\":true,\"Id\":\"04\"")) ? true : false,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.ProgramCode))
                {
                    listmodel = listmodel.Where(r => r.ProgramCode.ToLower().Contains(searchCondition.ProgramCode));
                }
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()) || r.ChildCode.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(searchCondition.CreateBy.ToLower()));
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
                    try
                    {
                        var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                        listmodel = listmodel.Where(r => r.UpdateDate >= dateFrom);
                    }
                    catch (Exception)
                    { }

                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    try
                    {
                        var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                        listmodel = listmodel.Where(r => r.UpdateDate <= dateTo);
                    }
                    catch (Exception)
                    { }

                }

                searchResult.ListId = listmodel.Select(r => r.Id).ToList();

                if (searchCondition.Export == 1)
                {
                    var listAll = listmodel.ToList();
                    searchResult.TotalItem = listAll.Count;
                    searchResult.ListResult = listAll.Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                    searchResult.PathFile = ExportProfile(listAll, true);
                }
                else
                {
                    searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                    searchResult.ListResult = listmodel.Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileUpdateBusiness.SearchChildProfileProvince", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public SearchResultObject<ChildProfileSearchResult> SearchChildProfileWard(ChildProfileSearchCondition searchCondition)
        {
            SearchResultObject<ChildProfileSearchResult> searchResult = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(searchCondition.UserId);

                var listmodel = (from a in db.ChildProfileUpdates.AsNoTracking()
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 join g in db.Ethnics.AsNoTracking() on a.EthnicId equals g.Id
                                 join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into asc
                                 from asc1 in asc.DefaultIfEmpty()
                                 join h in db.Users.AsNoTracking() on a.UpdateBy equals h.Id

                                 select new ChildProfileSearchResult()
                                 {
                                     Avata = a.ImageThumbnailPath,
                                     Id = a.Id,
                                     Name = a.Name,
                                     School = asc1 != null ? asc1.SchoolName : a.SchoolOtherName,
                                     ReligionName = ac1 != null ? ac1.Name : "",
                                     ProgramCode = a.ProgramCode,
                                     NationName = g.Name,
                                     Status = a.ProcessStatus,
                                     ChildCode = a.ChildCode,
                                     ProvinceId = a.ProvinceId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     Address = a.FullAddress,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender == Constants.Male ? "Nam" : "Nữ",
                                     CreateDate = a.UpdateDate,
                                     CreateBy = h.Name,
                                     Handicap = a.Handicap.HasValue ? (bool)a.Handicap : false,
                                     HealthHandicap = (a.Health.Contains("\"Id\":\"04\",\"Check\":true") || a.Health.Contains("\"Check\":true,\"Id\":\"04\"")) ? true : false,
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.ProgramCode))
                {
                    listmodel = listmodel.Where(r => r.ProgramCode.ToLower().Contains(searchCondition.ProgramCode));
                }
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()) || r.ChildCode.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.CreateBy))
                {
                    listmodel = listmodel.Where(r => r.CreateBy.ToLower().Contains(searchCondition.CreateBy.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(searchCondition.ProvinceId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(searchCondition.DistrictId));
                }
                else
                {
                    List<string> lstDistrictId = new List<string>();
                    if (!string.IsNullOrEmpty(searchCondition.UserId))
                    {
                        lstDistrictId = (from a in db.AreaUsers.AsNoTracking()
                                         where a.Id.Equals(userInfo.AreaUserId)
                                         join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                         where (string.IsNullOrEmpty(userInfo.DistrictId) || c.DistrictId.Equals(userInfo.DistrictId))
                                         select c.DistrictId).ToList();
                    }
                    listmodel = listmodel.Where(r => lstDistrictId.Contains(r.DistrictId));
                }
                if (!string.IsNullOrEmpty(searchCondition.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(searchCondition.WardId));
                }
                else
                {
                    List<string> lstWardId = new List<string>();
                    if (!string.IsNullOrEmpty(searchCondition.UserId))
                    {
                        lstWardId = (from a in db.AreaUsers.AsNoTracking()
                                     where a.Id.Equals(userInfo.AreaUserId)
                                     join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                     join d in db.AreaWards.AsNoTracking() on c.Id equals d.AreaDistrictId
                                     select d.WardId).ToList();
                    }
                    listmodel = listmodel.Where(r => lstWardId.Contains(r.WardId));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    try
                    {
                        var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                        listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                    }
                    catch (Exception)
                    {
                    }
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
                if (searchCondition.Export == 1)
                {
                    var listAll = listmodel.ToList();
                    searchResult.TotalItem = listAll.Count;
                    searchResult.ListResult = listAll.Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                    searchResult.PathFile = ExportProfile(listAll, false);
                }
                else
                {
                    searchResult.ListId = listmodel.Select(u => u.Id).ToList();
                    searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                    searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileUpdateBusiness.SearchChildProfileWard", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public void DeleteChildProfile(ChildProfileModel model)
        {
            var checkChild = db.ChildProfileUpdates.FirstOrDefault(u => u.Id.Equals(model.Id));
            if (checkChild == null)
            {
                throw new Exception("Hồ sơ đã bị xóa bởi người dùng khác");
            }
            try
            {
                db.ChildProfileUpdates.Remove(checkChild);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileUpdateBusiness.DeleteChildProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public void ConfimProfile(ChildProfileModel model)
        {
            if (!model.Id.Equals("-1"))
            {//nếu là duyệt 1 thì đưa id vào list
                model.SelectId = new List<string>();
                model.SelectId.Add(model.Id);
            }
            var chilSelect = db.ChildProfileUpdates.Where(u => model.SelectId.Contains(u.Id)).ToList();
            var listIdProfile = chilSelect.Select(u => u.ChildProfileId).ToList();
            var listProfile = db.ChildProfiles.Where(u => listIdProfile.Contains(u.Id)).ToList();
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.CreateBy);
            var userNotify = db.Users.Where(u => u.UserLever.Equals(Constants.LevelOffice)).ToList();
            try
            {
                string address = string.Empty;
                foreach (var checkChild in chilSelect)
                {
                    if (checkChild.ProcessStatus.Equals(Constants.CreateNew))
                    {
                        checkChild.ProcessStatus = Constants.ApproverArea;
                        checkChild.AreaApproverDate = DateTime.Now;
                        checkChild.AreaApproverId = model.CreateBy;

                        #region[lưu cache notify]
                        RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                        string image = "";
                        var imageChild = db.ImageChildHistories.FirstOrDefault(u => u.ChildProfileId.Equals(model.Id));
                        if (imageChild != null)
                        {
                            image = imageChild.ImageThumbnailPath;
                        }

                        ImageChildHistory imageChildHistory = new ImageChildHistory
                        {
                            Id = Guid.NewGuid().ToString(),
                            ChildProfileId = checkChild.ChildProfileId,
                            ImagePath = checkChild.ImagePath,
                            ImageThumbnailPath = checkChild.ImageThumbnailPath,
                            UploadBy = checkChild.UpdateBy,
                            UploadDate = DateTime.Now
                        };
                        db.ImageChildHistories.Add(imageChildHistory);

                        //địa phương duyệt- lấy tk trung ương
                        NotifyModel notifyModel;
                        var dateNow = DateTime.Now;
                        address = checkChild.FullAddress;// addressModel.WardName + ", " + addressModel.DistrictName + ", " + addressModel.ProvinceName;

                        string isSendEmail = ConfigurationManager.AppSettings["IsSendEmail"];
                        if (isSendEmail.ToLower().Equals("true"))
                        {
                            TimeSpan ts = new TimeSpan(24 * 30, 0, 0);
                            string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                            foreach (var item in userNotify)
                            {
                                notifyModel = new NotifyModel();
                                notifyModel.Image = image;
                                notifyModel.Id = Guid.NewGuid().ToString();
                                notifyModel.Addres = address;
                                notifyModel.CreateDate = DateTime.Now;
                                notifyModel.Status = Constants.NotViewNotification;
                                notifyModel.Title = "Hồ sơ cập nhật: <b>" + checkChild.ChildCode + "-" + checkChild.Name + "</b> từ cán bộ <b>" + userInfo.Name + "</b>";
                                notifyModel.Link = "/ProfilesUpdate/CompareProfile/" + checkChild.Id + "";
                                redisService.Add(cacheNotify + item.Id + ":" + notifyModel.Id, notifyModel, ts);
                            }
                        }
                        #endregion
                    }
                    else if (checkChild.ProcessStatus.Equals(Constants.ApproverArea) && (userInfo.UserLever.Equals(Constants.LevelOffice) || userInfo.UserLever.Equals(Constants.LevelAdmin)))
                    {
                        #region[cập nhật lại cho bảng hồ sơ]
                        var childProfile = listProfile.FirstOrDefault(u => u.Id.Equals(checkChild.ChildProfileId));
                        if (childProfile != null)
                        {
                            childProfile.EmployeeName = childProfile.EmployeeName;
                            childProfile.ProgramCode = checkChild.ProgramCode;
                            childProfile.ProvinceId = checkChild.ProvinceId;
                            childProfile.DistrictId = checkChild.DistrictId;
                            childProfile.WardId = checkChild.WardId;
                            childProfile.Address = checkChild.Address;
                            childProfile.FullAddress = checkChild.FullAddress;
                            childProfile.ChildCode = checkChild.ChildCode;
                            childProfile.SchoolId = checkChild.SchoolId;
                            childProfile.SchoolOtherName = checkChild.SchoolOtherName;
                            childProfile.EthnicId = checkChild.EthnicId;
                            childProfile.ReligionId = checkChild.ReligionId;
                            childProfile.Name = checkChild.Name;
                            childProfile.NickName = checkChild.NickName;
                            childProfile.Gender = checkChild.Gender;
                            childProfile.DateOfBirth = checkChild.DateOfBirth;
                            childProfile.LeaningStatus = checkChild.LeaningStatus;
                            childProfile.ClassInfo = checkChild.ClassInfo;
                            childProfile.FavouriteSubject = checkChild.FavouriteSubject;
                            childProfile.LearningCapacity = checkChild.LearningCapacity;
                            childProfile.Housework = checkChild.Housework;
                            childProfile.Health = checkChild.Health;
                            childProfile.Personality = checkChild.Personality;
                            childProfile.Hobby = checkChild.Hobby;
                            childProfile.Dream = checkChild.Dream;
                            childProfile.FamilyMember = checkChild.FamilyMember;
                            childProfile.LivingWithParent = checkChild.LivingWithParent;
                            childProfile.NotLivingWithParent = checkChild.NotLivingWithParent;
                            childProfile.LivingWithOther = checkChild.LivingWithOther;
                            childProfile.LetterWrite = checkChild.LetterWrite;
                            childProfile.HouseType = checkChild.HouseType;
                            childProfile.HouseRoof = checkChild.HouseRoof;
                            childProfile.HouseWall = checkChild.HouseWall;
                            childProfile.HouseFloor = checkChild.HouseFloor;
                            childProfile.UseElectricity = checkChild.UseElectricity;
                            childProfile.SchoolDistance = checkChild.SchoolDistance;
                            childProfile.ClinicDistance = checkChild.ClinicDistance;
                            childProfile.WaterSourceDistance = checkChild.WaterSourceDistance;
                            childProfile.WaterSourceUse = checkChild.WaterSourceUse;
                            childProfile.RoadCondition = checkChild.RoadCondition;
                            childProfile.IncomeFamily = checkChild.IncomeFamily;
                            childProfile.HarvestOutput = checkChild.HarvestOutput;
                            childProfile.NumberPet = checkChild.NumberPet;
                            childProfile.FamilyType = checkChild.FamilyType;
                            childProfile.TotalIncome = checkChild.TotalIncome;
                            childProfile.IncomeSources = checkChild.IncomeSources;
                            childProfile.IncomeOther = checkChild.IncomeOther;
                            childProfile.AreaApproverDate = checkChild.AreaApproverDate;
                            childProfile.AreaApproverId = checkChild.AreaApproverId;

                            childProfile.OfficeApproveBy = model.CreateBy;
                            childProfile.OfficeApproveDate = DateTime.Now;
                            childProfile.ProcessStatus = Constants.ApproveOffice;
                            childProfile.ConsentName = checkChild.ConsentName;
                            childProfile.ConsentRelationship = checkChild.ConsentRelationship;
                            childProfile.ConsentVillage = checkChild.ConsentVillage;
                            childProfile.ConsentWard = checkChild.ConsentWard;
                            childProfile.SiblingsJoiningChildFund = checkChild.SiblingsJoiningChildFund;
                            childProfile.Malformation = checkChild.Malformation;
                            childProfile.Orphan = checkChild.Orphan;
                            childProfile.EmployeeTitle = checkChild.EmployeeTitle;
                            childProfile.SaleforceId = checkChild.SaleforceId;
                            childProfile.Handicap = checkChild.Handicap;

                            if (!string.IsNullOrEmpty(checkChild.ImagePath))
                            {
                                childProfile.ImagePath = checkChild.ImagePath;
                            }
                            if (!string.IsNullOrEmpty(checkChild.ImageThumbnailPath))
                            {
                                childProfile.ImageThumbnailPath = checkChild.ImageThumbnailPath;
                            }
                            if (!string.IsNullOrEmpty(checkChild.ImageSignaturePath))
                            {
                                childProfile.ImageSignaturePath = checkChild.ImageSignaturePath;
                            }
                            if (!string.IsNullOrEmpty(checkChild.ImageSignatureThumbnailPath))
                            {
                                childProfile.ImageSignatureThumbnailPath = checkChild.ImageSignatureThumbnailPath;
                            }

                            childProfile.UpdateDate = checkChild.UpdateDate;
                            childProfile.UpdateBy = checkChild.UpdateBy;

                            childProfile.StoryContent = new ChildProfileBusiness().GenStory(childProfile.Id, false);
                            var img = db.ImageChildHistories.OrderByDescending(u => u.UploadDate).FirstOrDefault(u => u.ChildProfileId.Equals(childProfile.Id));
                            ImageChildHistory imageChildHistory = new ImageChildHistory
                            {
                                Id = Guid.NewGuid().ToString(),
                                ChildProfileId = checkChild.ChildProfileId,
                                ImagePath = checkChild.ImagePath,
                                ImageThumbnailPath = checkChild.ImageThumbnailPath,
                                UploadBy = checkChild.UpdateBy,
                                UploadDate = DateTime.Now
                            };
                            db.ImageChildHistories.Add(imageChildHistory);

                        }
                        db.ChildProfileUpdates.Remove(checkChild);
                        #endregion
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileUpdateBusiness.ConfimProfile", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }

        public string ExportProfile(List<ChildProfileSearchResult> list, bool IsProvince)
        {
            string pathTemplate = "/Template/ProfileUpdateProvince.xlsx";
            string pathExport = "/Template/Export/Danh-Sach-Ho-So-Cap-Nhat.xlsx";

            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;
            IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(pathTemplate));
            IWorksheet sheet = workbook.Worksheets[0];
            IRange rangeValue = sheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
            rangeValue.Text = rangeValue.Text.Replace("<Title>", "");
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
                if (IsProvince)
                {
                    var listExport = (from a in list
                                      select new
                                      {
                                          Index = index++,
                                          a.Name,
                                          a.Gender,
                                          d1 = a.DateOfBirth.ToString("dd/MM/yyyy"),
                                          a.NationName,
                                          a.Address,
                                          a.ProgramCode,
                                          a.ChildCode,
                                          a.CreateBy,
                                          d2 = a.CreateDate != null ? a.CreateDate.Value.ToString("dd/MM/yyyy") : "",
                                          d3 = a.Status.Equals(Constants.ApproveOffice) ? "Đã duyệt" : "Chưa duyệt",
                                          a.SalesforceID
                                      }).ToList();
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders.Color = ExcelKnownColors.Black;
                    sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 12].CellStyle.WrapText = true;
                }
                else
                {
                    var listExport = (from a in list
                                      select new
                                      {
                                          Index = index++,
                                          a.Name,
                                          a.Gender,
                                          d1 = a.DateOfBirth.ToString("dd/MM/yyyy"),
                                          a.NationName,
                                          a.Address,
                                          a.ProgramCode,
                                          a.ChildCode,
                                          a.CreateBy,
                                          d2 = a.CreateDate != null ? a.CreateDate.Value.ToString("dd/MM/yyyy") : "",
                                          d3 = a.Status.Equals(Constants.CreateNew) ? "Chưa duyệt" : "Đã duyệt",
                                          a.SalesforceID
                                      }).ToList();
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    sheet.Range[iRangeData.Row, 1, iRangeData.Row + total - 1, 12].Borders.Color = ExcelKnownColors.Black;
                    sheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 12].CellStyle.WrapText = true;
                }
            }
            workbook.SaveAs(HttpContext.Current.Server.MapPath(pathExport));
            return pathExport;
        }

        public string ExportStorySelect(ChildProfileExport model)
        {
            if (model.ListCheck == null)
            {
                model.ListCheck = new List<string>();
            }
            string result = "";
            try
            {
                var listmodel = (from a in db.ChildProfileUpdates.AsNoTracking()
                                 where model.ListCheck.Contains(a.Id)
                                 join c in db.Provinces.AsNoTracking() on a.ProvinceId equals c.Id
                                 join d in db.Districts.AsNoTracking() on a.DistrictId equals d.Id
                                 orderby a.UpdateDate descending
                                 select new ChildProfileExportResult()
                                 {
                                     ProgramCode = a.ProgramCode,
                                     ChildCode = a.ChildCode,
                                     SaleforceID = a.SaleforceId,
                                     Name = a.Name,
                                     ProvinceName = c.Name,
                                     //  DistrictName = d.Name,
                                     CreateDate = a.UpdateDate,
                                     Image = a.ImagePath,
                                     ImageSignaturePath = a.ImageSignaturePath
                                 }).ToList();
                if (listmodel.Count == 0)
                {
                    throw new Exception("Không có câu chuyện của trẻ!");
                }
                result = ExportStory(listmodel);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.ExportStorySelect", ex.Message, model);
                throw new Exception("Không có câu chuyện của trẻ!");
            }
            return result;
        }

        public string ExportStory(List<ChildProfileExportResult> listAll)
        {
            var list = listAll.Where(u => u.ProgramCode.StartsWith(Constants.ChildCode199)).ToList();
            var list213 = listAll.Where(u => u.ProgramCode.StartsWith(Constants.ChildCode213)).ToList();
            List<AttachmentImageModel> lstFile = new List<AttachmentImageModel>();
            AttachmentImageModel itemFile;
            string pathExport = "";
            string result = "";
            try
            {
                var dateView = DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + "-Story";
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView));
                if (list.Count > 0)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Narrative"));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Photo"));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Consent"));
                }
                if (list213.Count > 0)
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/213/Photo"));
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/213/Consent"));
                }
                foreach (var item in list)
                {
                    pathExport = HttpContext.Current.Server.MapPath("/Template/Export/" + dateView + "/199/Narrative/" + Common.ConvertNameToTag(item.ChildCode) + ".txt");
                    using (StreamWriter sw = new StreamWriter(pathExport))
                    {
                        sw.WriteLine(item.Content);
                    }
                    itemFile = new AttachmentImageModel();
                    itemFile.ImagePath = pathExport;
                    itemFile.Name = item.ChildCode + item.Name;
                    lstFile.Add(itemFile);
                }
                //tai anh
                if (list.Count > 0)
                {
                    var urls = list.Select(e => new AttachmentImageModel
                    {
                        ImagePath = e.Image,
                        Name = e.Name,
                        Code = e.ChildCode
                    }).ToList();
                    new ImageLibraryDA().DownLoadImgProfileToServer(urls, "/Template/Export/" + dateView + "/199/Photo/", false, false);
                    var urlImageSignature = list.Select(a => new AttachmentImageModel
                    {
                        ImagePath = a.ImageSignaturePath,
                        Name = a.ChildCode,
                        Code = a.ChildCode
                    }).ToList();
                    if (urlImageSignature.Count > 0)
                    {
                        new ImageLibraryDA().DownLoadImgProfileToServer(urlImageSignature, "/Template/Export/" + dateView + "/199/Consent/", false, true);
                    }
                }
                if (list213.Count > 0)
                {
                    itemFile = new AttachmentImageModel();
                    itemFile.ImagePath = ExportCSV(list213, dateView);
                    itemFile.Name = "CSV213";
                    lstFile.Add(itemFile);
                    var urls213 = list213.Select(e => new AttachmentImageModel
                    {
                        ImagePath = e.Image,
                        Code = e.ChildCode,
                        Name = e.Name
                    }).ToList();
                    new ImageLibraryDA().DownLoadImgProfileToServer(urls213, "/Template/Export/" + dateView + "/213/Photo/", true, false);
                    var urlImageSignature = list213.Select(a => new AttachmentImageModel
                    {
                        ImagePath = a.ImageSignaturePath,
                        Name = a.ChildCode,
                        Code = a.ChildCode
                    }).ToList();
                    if (urlImageSignature.Count > 0)
                    {
                        new ImageLibraryDA().DownLoadImgProfileToServer(urlImageSignature, "/Template/Export/" + dateView + "/213/Consent/", false, true);
                    }
                }
                string folder = "~/Template/Export/" + dateView;
                string fileReturn = "~/Template/Export/" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + "/";
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(fileReturn));
                result = new ImageLibraryDA().ZipFileForder(folder, fileReturn, "Ho-So");
            }
            catch (Exception ex)
            { LogUtils.ExceptionLog("ChildProfileBusiness.ExportStory", ex.Message, ""); }
            return result;
        }

        public string ExportCSV(List<ChildProfileExportResult> list, string dateView)
        {
            //string filePathResult = "/Template/Export/" + dateView + "/Story213/Story/" + NTS.Common.Utils.Common.ConvertNameToTag(list[0].DistrictName) + ".csv";
            string filePathResult = "/Template/Export/" + dateView + "/213/Ho-So.csv";
            try
            {
                #region[xuất file]
                string filePath = "/Template/Export/Data" + dateView + ".xlsx";
                // Khỏi tạo bảng excel
                ExcelEngine excelEngine = new ExcelEngine();
                IApplication application = excelEngine.Excel;
                IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath("/Template/TemplateCSV.xlsx"));
                IWorksheet sheet = workbook.Worksheets[0];
                int total = list.Count;
                var listExport = (from a in list
                                  select new
                                  {
                                      a1 = a.SaleforceID,
                                      a2 = a.Content,
                                      a3 = a.CreateDate.ToString("dd/MM/yyyy")
                                  }).ToList();
                IRange iRangeData = sheet.FindFirst("<Data>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                if (total == 0)
                {
                    iRangeData.Text = iRangeData.Text.Replace("<Data>", "");
                }
                else
                {
                    sheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                }
                workbook.SaveAs(HttpContext.Current.Server.MapPath(filePath));
                ConverFile(filePath, filePathResult);
                #endregion
            }
            catch (Exception)
            { }
            return HttpContext.Current.Server.MapPath(filePathResult);
        }

        public void ConverFile(string filePathSource, string filePathResult)
        {
            #region[xuất file]
            //Initialize ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                //Initialize Application
                IApplication application = excelEngine.Excel;
                //Set default version for application
                application.DefaultVersion = ExcelVersion.Excel2013;
                //Open a workbook to be export as CSV
                IWorkbook workbook = application.Workbooks.Open(HttpContext.Current.Server.MapPath(filePathSource));
                //Accessing first worksheet in the workbook
                IWorksheet worksheet = workbook.Worksheets[0];
                //Save the workbook to csv format
                worksheet.SaveAs(HttpContext.Current.Server.MapPath(filePathResult), ",");
            }
            #endregion
        }

        public List<ChildProfileModel> CompareProfile(string id)
        {
            var listVillage = db.Villages.AsNoTracking().ToList();

            List<ChildProfileModel> lstResult = new List<ChildProfileModel>();
            //Thông tin childprofile bản cập nhật
            var childProfileUpdates = (from a in db.ChildProfileUpdates.AsNoTracking()
                                       where a.Id.Equals(id)
                                       join b in db.Ethnics.AsNoTracking() on a.EthnicId equals b.Id
                                       join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                       from ac1 in ac.DefaultIfEmpty()
                                       join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into asc
                                       from asc1 in asc.DefaultIfEmpty()
                                       join d in db.Provinces.AsNoTracking() on a.ProvinceId equals d.Id
                                       join e in db.Districts.AsNoTracking() on a.DistrictId equals e.Id
                                       join f in db.Wards.AsNoTracking() on a.WardId equals f.Id
                                       select new ChildProfileModel
                                       {
                                           Id = a.Id,
                                           ChildProfileId = a.ChildProfileId,
                                           Name = a.Name,
                                           Status = a.ProcessStatus,
                                           Avatar = a.ImageThumbnailPath,
                                           ReligionId = ac1 != null ? ac1.Name : "",
                                           EthnicId = b.Name,
                                           ProcessStatus = a.ProcessStatus,
                                           ProgramCode = a.ProgramCode,
                                           SchoolId = asc1 != null ? asc1.SchoolName : a.SchoolOtherName,
                                           ChildCode = !string.IsNullOrEmpty(a.ChildCode) ? a.ChildCode : string.Empty,
                                           ProvinceId = !string.IsNullOrEmpty(d.Name) ? d.Name : string.Empty,
                                           DistrictId = !string.IsNullOrEmpty(e.Name) ? e.Name : string.Empty,
                                           WardId = !string.IsNullOrEmpty(f.Name) ? f.Name : string.Empty,
                                           Address = !string.IsNullOrEmpty(a.Address) ? a.Address : string.Empty,
                                           EmployeeName = !string.IsNullOrEmpty(a.EmployeeName) ? a.EmployeeName : string.Empty,
                                           InfoDate = a.InfoDate,
                                           DateOfBirth = a.DateOfBirth,
                                           Gender = a.Gender,
                                           NickName = !string.IsNullOrEmpty(a.NickName) ? a.NickName : string.Empty,
                                           LeaningStatus = !string.IsNullOrEmpty(a.LeaningStatus) ? a.LeaningStatus : string.Empty,
                                           ClassInfo = !string.IsNullOrEmpty(a.ClassInfo) ? a.ClassInfo : string.Empty,
                                           FavouriteSubject = !string.IsNullOrEmpty(a.FavouriteSubject) ? a.FavouriteSubject : string.Empty,
                                           LearningCapacity = !string.IsNullOrEmpty(a.LearningCapacity) ? a.LearningCapacity : string.Empty,
                                           Housework = !string.IsNullOrEmpty(a.Housework) ? a.Housework : string.Empty,
                                           Health = !string.IsNullOrEmpty(a.Health) ? a.Health : string.Empty,
                                           Personality = !string.IsNullOrEmpty(a.Personality) ? a.Personality : string.Empty,
                                           Hobby = !string.IsNullOrEmpty(a.Hobby) ? a.Hobby : string.Empty,
                                           Dream = !string.IsNullOrEmpty(a.Dream) ? a.Dream : string.Empty,
                                           FamilyMember = !string.IsNullOrEmpty(a.FamilyMember) ? a.FamilyMember : string.Empty,
                                           LivingWithParent = !string.IsNullOrEmpty(a.LivingWithParent) ? a.LivingWithParent : string.Empty,
                                           NotLivingWithParent = !string.IsNullOrEmpty(a.NotLivingWithParent) ? a.NotLivingWithParent : string.Empty,
                                           LivingWithOther = !string.IsNullOrEmpty(a.LivingWithOther) ? a.LivingWithOther : string.Empty,
                                           LetterWrite = !string.IsNullOrEmpty(a.LetterWrite) ? a.LetterWrite : string.Empty,
                                           HouseType = !string.IsNullOrEmpty(a.HouseType) ? a.HouseType : string.Empty,
                                           HouseRoof = !string.IsNullOrEmpty(a.HouseRoof) ? a.HouseRoof : string.Empty,
                                           HouseWall = !string.IsNullOrEmpty(a.HouseWall) ? a.HouseWall : string.Empty,
                                           HouseFloor = !string.IsNullOrEmpty(a.HouseFloor) ? a.HouseFloor : string.Empty,
                                           UseElectricity = !string.IsNullOrEmpty(a.UseElectricity) ? a.UseElectricity : string.Empty,
                                           SchoolDistance = !string.IsNullOrEmpty(a.SchoolDistance) ? a.SchoolDistance : string.Empty,
                                           ClinicDistance = !string.IsNullOrEmpty(a.ClinicDistance) ? a.ClinicDistance : string.Empty,
                                           WaterSourceDistance = !string.IsNullOrEmpty(a.WaterSourceDistance) ? a.WaterSourceDistance : string.Empty,
                                           WaterSourceUse = !string.IsNullOrEmpty(a.WaterSourceUse) ? a.WaterSourceUse : string.Empty,
                                           RoadCondition = !string.IsNullOrEmpty(a.RoadCondition) ? a.RoadCondition : string.Empty,
                                           IncomeFamily = !string.IsNullOrEmpty(a.IncomeFamily) ? a.IncomeFamily : string.Empty,
                                           HarvestOutput = !string.IsNullOrEmpty(a.HarvestOutput) ? a.HarvestOutput : string.Empty,
                                           NumberPet = !string.IsNullOrEmpty(a.NumberPet) ? a.NumberPet : string.Empty,
                                           FamilyType = !string.IsNullOrEmpty(a.FamilyType) ? a.FamilyType : string.Empty,
                                           TotalIncome = !string.IsNullOrEmpty(a.TotalIncome) ? a.TotalIncome : string.Empty,
                                           IncomeOther = !string.IsNullOrEmpty(a.IncomeOther) ? a.IncomeOther : string.Empty,
                                           ImagePath = a.ImagePath,
                                           ImageThumbnailPath = a.ImageThumbnailPath,
                                           ImageSignaturePath = a.ImageSignaturePath,
                                           UpdateDate = a.UpdateDate,
                                           Handicap = a.Handicap,
                                           ConsentName = !string.IsNullOrEmpty(a.ConsentName) ? a.ConsentName : string.Empty,
                                           ConsentRelationship = !string.IsNullOrEmpty(a.ConsentRelationship) ? a.ConsentRelationship : string.Empty,
                                           ConsentVillage = !string.IsNullOrEmpty(a.ConsentVillage) ? a.ConsentVillage : string.Empty,
                                           ConsentWard = !string.IsNullOrEmpty(a.ConsentWard) ? a.ConsentWard : string.Empty,
                                           SiblingsJoiningChildFund = !string.IsNullOrEmpty(a.SiblingsJoiningChildFund) ? a.SiblingsJoiningChildFund : string.Empty
                                       }).FirstOrDefault();
            if (childProfileUpdates == null)
            {
                throw new Exception("Hồ sơ đã bị xóa bởi người dùng khách");
            }

            if (childProfileUpdates.SchoolId == null)
            {
                childProfileUpdates.SchoolId = "";
            }

            var newVillage = listVillage.Where(r => r.Id.Equals(childProfileUpdates.Address)).FirstOrDefault();
            if (newVillage != null)
            {
                childProfileUpdates.Address = newVillage.Name;
            }

            childProfileUpdates.ConvertObjectJsonToModel();


            try
            {
                //Thông tin childprofile bản trước
                var childProfiles = (from a in db.ChildProfiles.AsNoTracking()
                                     where a.Id.Equals(childProfileUpdates.ChildProfileId)
                                     join b in db.Ethnics.AsNoTracking() on a.EthnicId equals b.Id
                                     join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id into ac
                                     from ac1 in ac.DefaultIfEmpty()
                                     join s in db.Schools.AsNoTracking() on a.SchoolId equals s.Id into asc
                                     from asc1 in asc.DefaultIfEmpty()
                                     join d in db.Provinces.AsNoTracking() on a.ProvinceId equals d.Id
                                     join e in db.Districts.AsNoTracking() on a.DistrictId equals e.Id
                                     join f in db.Wards.AsNoTracking() on a.WardId equals f.Id
                                     select new ChildProfileModel
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Status = a.ProcessStatus,
                                         Avatar = a.ImageThumbnailPath,
                                         ReligionId = ac1 != null ? ac1.Name : "",
                                         EthnicId = b.Name,
                                         ProcessStatus = a.ProcessStatus,
                                         ProgramCode = a.ProgramCode,
                                         SchoolId = asc1 != null ? asc1.SchoolName : a.SchoolOtherName,
                                         ChildCode = !string.IsNullOrEmpty(a.ChildCode) ? a.ChildCode : string.Empty,
                                         ProvinceId = !string.IsNullOrEmpty(d.Name) ? d.Name : string.Empty,
                                         DistrictId = !string.IsNullOrEmpty(e.Name) ? e.Name : string.Empty,
                                         WardId = !string.IsNullOrEmpty(f.Name) ? f.Name : string.Empty,
                                         Address = !string.IsNullOrEmpty(a.Address) ? a.Address : string.Empty,
                                         EmployeeName = !string.IsNullOrEmpty(a.EmployeeName) ? a.EmployeeName : string.Empty,
                                         InfoDate = a.InfoDate,
                                         DateOfBirth = a.DateOfBirth,
                                         Gender = a.Gender,
                                         NickName = !string.IsNullOrEmpty(a.NickName) ? a.NickName : string.Empty,
                                         LeaningStatus = !string.IsNullOrEmpty(a.LeaningStatus) ? a.LeaningStatus : string.Empty,
                                         ClassInfo = !string.IsNullOrEmpty(a.ClassInfo) ? a.ClassInfo : string.Empty,
                                         FavouriteSubject = !string.IsNullOrEmpty(a.FavouriteSubject) ? a.FavouriteSubject : string.Empty,
                                         LearningCapacity = !string.IsNullOrEmpty(a.LearningCapacity) ? a.LearningCapacity : string.Empty,
                                         Housework = !string.IsNullOrEmpty(a.Housework) ? a.Housework : string.Empty,
                                         Health = !string.IsNullOrEmpty(a.Health) ? a.Health : string.Empty,
                                         Personality = !string.IsNullOrEmpty(a.Personality) ? a.Personality : string.Empty,
                                         Hobby = !string.IsNullOrEmpty(a.Hobby) ? a.Hobby : string.Empty,
                                         Dream = !string.IsNullOrEmpty(a.Dream) ? a.Dream : string.Empty,
                                         FamilyMember = !string.IsNullOrEmpty(a.FamilyMember) ? a.FamilyMember : string.Empty,
                                         LivingWithParent = !string.IsNullOrEmpty(a.LivingWithParent) ? a.LivingWithParent : string.Empty,
                                         NotLivingWithParent = !string.IsNullOrEmpty(a.NotLivingWithParent) ? a.NotLivingWithParent : string.Empty,
                                         LivingWithOther = !string.IsNullOrEmpty(a.LivingWithOther) ? a.LivingWithOther : string.Empty,
                                         LetterWrite = !string.IsNullOrEmpty(a.LetterWrite) ? a.LetterWrite : string.Empty,
                                         HouseType = !string.IsNullOrEmpty(a.HouseType) ? a.HouseType : string.Empty,
                                         HouseRoof = !string.IsNullOrEmpty(a.HouseRoof) ? a.HouseRoof : string.Empty,
                                         HouseWall = !string.IsNullOrEmpty(a.HouseWall) ? a.HouseWall : string.Empty,
                                         HouseFloor = !string.IsNullOrEmpty(a.HouseFloor) ? a.HouseFloor : string.Empty,
                                         UseElectricity = !string.IsNullOrEmpty(a.UseElectricity) ? a.UseElectricity : string.Empty,
                                         SchoolDistance = !string.IsNullOrEmpty(a.SchoolDistance) ? a.SchoolDistance : string.Empty,
                                         ClinicDistance = !string.IsNullOrEmpty(a.ClinicDistance) ? a.ClinicDistance : string.Empty,
                                         WaterSourceDistance = !string.IsNullOrEmpty(a.WaterSourceDistance) ? a.WaterSourceDistance : string.Empty,
                                         WaterSourceUse = !string.IsNullOrEmpty(a.WaterSourceUse) ? a.WaterSourceUse : string.Empty,
                                         RoadCondition = !string.IsNullOrEmpty(a.RoadCondition) ? a.RoadCondition : string.Empty,
                                         IncomeFamily = !string.IsNullOrEmpty(a.IncomeFamily) ? a.IncomeFamily : string.Empty,
                                         HarvestOutput = !string.IsNullOrEmpty(a.HarvestOutput) ? a.HarvestOutput : string.Empty,
                                         NumberPet = !string.IsNullOrEmpty(a.NumberPet) ? a.NumberPet : string.Empty,
                                         FamilyType = !string.IsNullOrEmpty(a.FamilyType) ? a.FamilyType : string.Empty,
                                         TotalIncome = !string.IsNullOrEmpty(a.TotalIncome) ? a.TotalIncome : string.Empty,
                                         IncomeOther = !string.IsNullOrEmpty(a.IncomeOther) ? a.IncomeOther : string.Empty,
                                         ImagePath = a.ImagePath,
                                         ImageThumbnailPath = a.ImageThumbnailPath,
                                         ImageSignaturePath = a.ImageSignaturePath,
                                         UpdateDate = a.UpdateDate,
                                         Handicap = a.Handicap,
                                         ConsentName = !string.IsNullOrEmpty(a.ConsentName) ? a.ConsentName : string.Empty,
                                         ConsentRelationship = !string.IsNullOrEmpty(a.ConsentRelationship) ? a.ConsentRelationship : string.Empty,
                                         ConsentVillage = !string.IsNullOrEmpty(a.ConsentVillage) ? a.ConsentVillage : string.Empty,
                                         ConsentWard = !string.IsNullOrEmpty(a.ConsentWard) ? a.ConsentWard : string.Empty,
                                         SiblingsJoiningChildFund = !string.IsNullOrEmpty(a.SiblingsJoiningChildFund) ? a.SiblingsJoiningChildFund : string.Empty
                                     }).FirstOrDefault();
                if (childProfiles == null)
                {
                    throw new Exception("Hồ sơ đã bị xóa bởi người dùng khách");
                }

                if (childProfiles.SchoolId == null)
                {
                    childProfiles.SchoolId = "";
                }
                var oldVillage = listVillage.Where(r => r.Id.Equals(childProfiles.Address)).FirstOrDefault();
                if (oldVillage != null)
                {
                    childProfiles.Address = oldVillage.Name;
                }

                childProfiles.ConvertObjectJsonToModel();

                lstResult.Add(childProfiles);
                lstResult.Add(childProfileUpdates);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileUpdateBusiness.CompareProfile", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return lstResult;
        }

        public string SaveChangeCode(string Id, string programCode)
        {
            string userId = HttpContext.Current.User.Identity.Name;
            var childProfileUpdate = db.ChildProfileUpdates.Find(Id);

            if (childProfileUpdate != null)
            {
                childProfileUpdate.ProgramCode = programCode;
                childProfileUpdate.UpdateBy = userId;
                childProfileUpdate.UpdateDate = DateTime.Now;
                db.SaveChanges();

                return childProfileUpdate.Id;
            }

            return string.Empty;
        }

        public string ExportProfileSelect(ChildProfileExport model)
        {
            if (model.ListCheck == null)
            {
                model.ListCheck = new List<string>();
            }
            string result = "";
            try
            {
                var listmodel = (from a in db.ChildProfileUpdates.AsNoTracking()
                                 where model.ListCheck.Contains(a.Id)
                                 //&& a.IsDelete == Constants.IsUse
                                 join c in db.Religions.AsNoTracking() on a.ReligionId equals c.Id
                                 //join d in db.Provinces.AsNoTracking() on a.ProvinceId equals d.Id
                                 //join e in db.Districts.AsNoTracking() on a.DistrictId equals e.Id
                                 //join f in db.Wards.AsNoTracking() on a.WardId equals f.Id
                                 join g in db.Ethnics.AsNoTracking() on a.EthnicId equals g.Id
                                 join h in db.Users.AsNoTracking() on a.UpdateBy equals h.Id
                                 //join i in db.Users.AsNoTracking() on a.AreaApproverId equals i.Id into ai
                                 //from ai1 in ai.DefaultIfEmpty()
                                 //join j in db.Users.AsNoTracking() on a.OfficeApproveBy equals j.Id into aj
                                 //from aj1 in aj.DefaultIfEmpty()
                                 join x in db.Schools.AsNoTracking() on a.SchoolId equals x.Id into xx
                                 from xxs in xx.DefaultIfEmpty()
                                 orderby a.UpdateDate descending
                                 select new ChildProfileSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     ReligionName = c.Name,
                                     ProgramCode = a.ProgramCode,
                                     NationName = g.Name,
                                     Status = a.ProcessStatus,
                                     ChildCode = a.ChildCode,
                                     Address = a.FullAddress,
                                     DateOfBirth = a.DateOfBirth,
                                     Gender = a.Gender == Constants.Male ? "Nam" : "Nữ",
                                     ApproveDate = a.AreaApproverDate,
                                     //ApproverName = ai1 != null ? ai1.Name : "",
                                     //OfficeApproveDate = a.OfficeApproveDate,
                                     //OfficeApproveBy = aj1 != null ? aj1.Name : "",
                                     CreateDate = a.UpdateDate,
                                     CreateBy = h.Name,
                                     //StoryContent = a.StoryContent,
                                     SchoolName = xxs.SchoolName,
                                     SalesforceID = a.SaleforceId
                                 }).ToList();
                result = ExportProfile(listmodel, model.IsProvince);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ChildProfileBusiness.ExportProfileSelect", ex.Message, model);
            }
            return result;
        }
    }
}
