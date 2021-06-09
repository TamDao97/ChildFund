using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Model.Model.CacheModel;
using ChildProfiles.Model.Model.Homes;
using Microsoft.AspNet.SignalR;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.Home
{
    public class HomesController : Controller
    {
        HomesBusiness _buss = new HomesBusiness();
        [MyAuthorize]
        public ActionResult Index()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            if (!string.IsNullOrEmpty(userId))
            {
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                ViewBag.UserLever = userInfo.UserLever;
            }
            return View();
        }
        public ActionResult GetData(HomesModel model)
        {
            try
            {
                model.UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.UserId);

                var dataProfiles = _buss.SearchProfiles(model, userInfo);
                var listWardId = dataProfiles.Select(i => i.WardId).ToList();
                var listWardData = _buss.GetWard(listWardId);
                var dateNow = DateTime.Now;
                //var dataProfilesUpdate = _buss.SearchProfilesUpdate(model);
                #region[xử lý biểu đồ cột bên phải]
                List<int> lstprofile = new List<int>();
                List<int> lstprofileConfim = new List<int>();
                List<int> lstprofileUnConfim = new List<int>();
                int countItem = 0;
                int countItemConfim = 0;
                int countItemUnConfim = 0;
                string status = Constants.ApproverArea;
                string statusOff = Constants.ApproveOffice;
                if (userInfo.UserLever.Equals("0") || userInfo.UserLever.Equals("1"))
                {
                    status = Constants.ApproveOffice;
                    statusOff = Constants.ApproveOffice;
                }
                for (int i = 1; i < 13; i++)
                {
                    countItem = dataProfiles.Where(u => u.CreateDate.Year == model.Year && u.CreateDate.Month == i).Count();
                    lstprofile.Add(countItem);
                    countItemConfim = dataProfiles.Where(u => u.CreateDate.Year == model.Year && u.CreateDate.Month == i && (u.Status.Equals(status) || u.Status.Equals(statusOff))).Count();
                    lstprofileConfim.Add(countItemConfim);
                    countItemUnConfim = countItem - countItemConfim;
                    lstprofileUnConfim.Add(countItemUnConfim);
                }
                #endregion
                #region[đếm số tổng trên cùng]
                var countProfile = dataProfiles.Count();
                var countConfim = dataProfiles.Where(u => (u.Status.Equals(status) || u.Status.Equals(statusOff))).Select(u => u.Status).Count();
                var dataCount = new { countConfim = countConfim, countProfile = countProfile, countUnConfim = countProfile - countConfim };
                #endregion
                #region[biểu độ đi học]
                List<LearningModel> lstLearningModel = new List<LearningModel>();
                LearningModel iemAdd;
                for (int i = 11; i < 17; i++)
                {
                    iemAdd = new LearningModel();
                    iemAdd.KeyValue = i.ToString();
                    lstLearningModel.Add(iemAdd);
                }
                var dataLearning = dataProfiles.GroupBy(r => r.LeaningStatus).Select(s => new LearningModel { Count = s.Count(), KeyValue = s.FirstOrDefault().LeaningStatus }).ToList();
                double countLearning = dataLearning.Sum(u => u.Count);
                foreach (var item in lstLearningModel)
                {
                    iemAdd = dataLearning.FirstOrDefault(u => u.KeyValue.Equals(item.KeyValue));
                    if (iemAdd != null)
                    {
                        item.Count = iemAdd.Count;
                        item.Name = GenName(item.KeyValue);
                        if (countLearning != 0)
                        {
                            item.Percen = ((double)iemAdd.Count * 100) / countLearning;
                            item.Percen = Math.Round(item.Percen, 1);
                        }
                        else
                        {
                            item.Percen = 0;
                        }

                    }
                    else
                    {
                        item.Count = 0;
                        item.Name = GenName(item.KeyValue);
                        item.Percen = 0;
                    }
                }
                #endregion
                #region[biểu độ theo xã]
                var dataNation = dataProfiles.GroupBy(r => r.WardId).Select(s => new LearningModel { Count = s.Count(), KeyValue = s.FirstOrDefault().WardId }).OrderByDescending(s => s.Count).ToList();
                double countNation = dataNation.Sum(u => u.Count);
                for (int i = 0; i < dataNation.Count; i++)
                {
                    dataNation[i].Name = listWardData.Where(s => s.Id.Equals(dataNation[i].KeyValue)).Select(s => s.Name).FirstOrDefault();
                    if (countNation != 0)
                    {
                        dataNation[i].Percen = ((double)dataNation[i].Count * 100) / countNation;
                        dataNation[i].Percen = Math.Round(dataNation[i].Percen, 1);
                    }
                    else
                    {
                        dataNation[i].Percen = 0;
                    }

                }
                #endregion
                #region[tuổi]
                var listAge = new List<int>();
                TimeSpan timeSp;
                foreach (var item in dataProfiles)
                {
                    if (item.Birthday != null)
                    {
                        timeSp = dateNow - item.Birthday.Value;
                        listAge.Add((int)(timeSp.TotalDays / 365));
                    }
                    else
                    {
                        listAge.Add(int.MaxValue);
                    }
                }
                List<AgeModel> lstAgegModel = new List<AgeModel>
                {
                    new AgeModel{ Name = "Nhỏ hơn 7 tuổi/Less than 7 years old", AgeBegin=0,AgeEnd=6 },
                    new AgeModel{ Name = "Từ 7-11 tuổi/From 7-11 years old", AgeBegin=7,AgeEnd=11 },
                    new AgeModel{ Name = "Từ 12-15 tuổi/From 12-15 years old", AgeBegin=12,AgeEnd=15 },
                    new AgeModel{ Name = "Khác/Other", AgeBegin=16,AgeEnd=int.MaxValue },
                };
                foreach (var item in lstAgegModel)
                {
                    item.Count = listAge.Where(u => u >= item.AgeBegin && u <= item.AgeEnd).Count();
                    if (countProfile != 0)
                    {
                        item.Percen = ((double)item.Count * 100) / countProfile;
                        item.Percen = Math.Round(item.Percen, 1);
                    }
                    else
                    {
                        item.Percen = 0;
                    }

                }
                #endregion
                #region[biểu độ theo giới tính]
                var dataGender = dataProfiles.GroupBy(r => r.Gender).Select(s => new LearningModel { Count = s.Count(), KeyValue = s.FirstOrDefault().Gender.ToString() }).OrderByDescending(s => s.Count).ToList();
                double countGender = dataGender.Sum(u => u.Count);
                for (int i = 0; i < dataGender.Count; i++)
                {
                    if (dataGender[i].KeyValue.Equals("1"))
                    {
                        dataGender[i].Name = "Nam/Male";
                    }
                    else
                    {
                        dataGender[i].Name = "Nữ/Female";
                    }
                    if (countGender != 0)
                    {
                        dataGender[i].Percen = ((double)dataGender[i].Count * 100) / countGender;
                        dataGender[i].Percen = Math.Round(dataGender[i].Percen, 1);
                    }
                    else
                    {
                        dataGender[i].Percen = 0;
                    }

                }
                var itemGender = new { Male = dataProfiles.Where(u => u.Gender == 1).Count(), FeMale = dataProfiles.Where(u => u.Gender != 1).Count() };
                #endregion
                var fileUrl = string.Empty;
                if (model.Export == 1)
                {
                    fileUrl = _buss.ExportReport(lstLearningModel, dataNation, dataGender, lstAgegModel, lstprofileConfim, lstprofileUnConfim);
                }
                return Json(new { ok = true, fileUrl = fileUrl, dataCount = dataCount, dataLearning = lstLearningModel, dataNation = dataNation, lstprofile = lstprofile, lstprofileConfim = lstprofileConfim, lstprofileUnConfim = lstprofileUnConfim, itemGender = itemGender, dataGender = dataGender, lstAgegModel = lstAgegModel }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { ok = false, mess = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public string GenName(string LeaningStatus)
        {
            string rs = "";
            try
            {
                switch (LeaningStatus)
                {
                    case Constants.LeaningDropout:
                        rs = "Bỏ học/Drop-out";
                        break;
                    case Constants.LeaningKindergarten:
                        rs = "Mẫu giáo/Attending kindergarten";
                        break;
                    case Constants.LeaningPrimarySchool:
                        rs = "Tiểu học/Primary school";
                        break;
                    case Constants.LeaningHighSchool:
                        rs = "Trung học/Secondary school";
                        break;
                    case Constants.LeaningHandicapped:
                        rs = "Khuyết tật/Disability";
                        break;
                    case Constants.LeaningChildhood:
                        rs = "Còn nhỏ/Too young";
                        break;
                    default:
                        break;

                }
            }
            catch (Exception)
            { }
            return rs;
        }

        public ActionResult Notify()
        {
            return View();
        }
        public ActionResult GetNotify(NotifySearchModel model)
        {
            List<NotifyModel> list = new List<NotifyModel>();
            try
            {
                var PageNumber = model.PageNumber;
                int PageSize = model.PageSize;
                if (PageNumber == null)
                {
                    PageNumber = 1;
                }
                int currPage = PageNumber.Value - 1;
                ViewBag.Index = (currPage * PageSize);
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                list = _buss.GetNotify(userId).OrderByDescending(u => u.CreateDate).ToList();
                var countAll = list.Count;
                ViewBag.PageSize = model.PageSize;
                ViewBag.countAll = countAll;
                list = list.Skip(currPage * PageSize).Take(PageSize).ToList();
                if (countAll > PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(PageSize, currPage, countAll, "");
                }
            }
            catch (Exception)
            {
            }
            return PartialView(list);
        }

        public ActionResult DeleteNotify(string id)
        {
            try
            {
                _buss.DeleteNotify(id);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult TickNotify(string id)
        {
            try
            {
                _buss.TickNotify(id);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}