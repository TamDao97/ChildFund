using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model.SearchResults;
using InformationHub.Model.StatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticByProcessingController : BaseController
    {
        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0030")]
        public ActionResult Index()
        {
            string userId = HttpContext.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.userInfo = userInfo;
            return View();
        }
        [MyAuthorize(Roles = "C0030")]
        public ActionResult ListReportProfileByProcessing(StatisticSearchCondition modelSearch)
        {
            ReturnProcessingModel list = new ReturnProcessingModel();
            try
            {
                list = _business.SearchStatisticByProcessingStatus(modelSearch);
                var count = list.LstTable.Count;
                int listLeftCount = count / 2;
                if (count % 2 != 0)
                {
                    listLeftCount++;
                }
                var listLeft = list.LstTable.Skip(0).Take(listLeftCount).ToList();
                var listRight = list.LstTable.Skip(listLeftCount).Take(count).ToList();

                return Json(new { ok = true, PathFile = list.PathFile, lstChart = list.LstChart, lstTableLeft = listLeft, lstTableRight = listRight, lstLocation = list.ListLocation }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyAuthorize(Roles = "C0031")]
        public ActionResult ReportByAbuse()
        {
            string userId = HttpContext.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.userInfo = userInfo;
            return View();
        }
        [MyAuthorize(Roles = "C0031")]
        public ActionResult GetReportByAbuse(StatisticSearchCondition modelSearch)
        {
            List<StatisticByProcessingModel> list = new List<StatisticByProcessingModel>();
            try
            {
                // table
                List<StatisticByProcessingModel> lstTable = new List<StatisticByProcessingModel>();
                StatisticByProcessingModel itemTable;
                // chart
                List<ChartModel> lstChart = new List<ChartModel>();
                ChartModel itemChart;
                //processing
                List<ProcessingStatus> status = new List<ProcessingStatus>();
                status.Add(new ProcessingStatus { Status = 1, StatusLable = Resource.Resource.ReportProfile_Status1 });
                status.Add(new ProcessingStatus { Status = 2, StatusLable = Resource.Resource.ReportProfile_Status2 });
                status.Add(new ProcessingStatus { Status = 3, StatusLable = Resource.Resource.ReportProfile_Status3 });
                status.Add(new ProcessingStatus { Status = 4, StatusLable = Resource.Resource.ReportProfile_Status4 });
                status.Add(new ProcessingStatus { Status = 5, StatusLable = Resource.Resource.ReportProfile_Status5 });
                //list status lable
                List<string> lstStatus = new List<string>();

                var data = _business.SearchStatisticByProcessing(modelSearch);
                var abuSeAll = new ComboboxBusiness().GetAllAbuseType();
                foreach (var item in abuSeAll)
                {
                    //table
                    itemTable = new StatisticByProcessingModel();
                    itemTable.AbuseTypeName = item.Name;
                    itemTable.AbuseId = item.Id;
                    itemTable.Count1 = data.Where(u => u.StatusStep1 == true && item.Id.Equals(u.AbuseId)).Count();
                    itemTable.Count2 = data.Where(u => u.StatusStep2 == true && item.Id.Equals(u.AbuseId)).Count();
                    itemTable.Count3 = data.Where(u => u.StatusStep3 == true && item.Id.Equals(u.AbuseId)).Count();
                    itemTable.Count4 = data.Where(u => u.StatusStep4 == true && item.Id.Equals(u.AbuseId)).Count();
                    itemTable.Count5 = data.Where(u => u.StatusStep5 == true && item.Id.Equals(u.AbuseId)).Count();
                    lstTable.Add(itemTable);
                    //chart
                }
                foreach (var item in status)
                {
                    itemChart = new ChartModel();
                    itemChart.Lable = item.StatusLable;
                    itemChart.Count = new List<int>();
                    foreach (var itemsub in abuSeAll)
                    {
                        switch (item.Status)
                        {
                            case 1:
                                itemChart.Count.Add(data.Where(u => u.StatusStep1 == true && itemsub.Id.Equals(u.AbuseId)).Count());
                                break;
                            case 2:
                                itemChart.Count.Add(data.Where(u => u.StatusStep2 == true && itemsub.Id.Equals(u.AbuseId)).Count());
                                break;
                            case 3:
                                itemChart.Count.Add(data.Where(u => u.StatusStep3 == true && itemsub.Id.Equals(u.AbuseId)).Count());
                                break;
                            case 4:
                                itemChart.Count.Add(data.Where(u => u.StatusStep4 == true && itemsub.Id.Equals(u.AbuseId)).Count());
                                break;
                            case 5:
                                itemChart.Count.Add(data.Where(u => u.StatusStep5 == true && itemsub.Id.Equals(u.AbuseId)).Count());
                                break;
                            default:
                                break;
                        }
                    }
                    lstChart.Add(itemChart);
                }
                var PathFile = "";
                if (modelSearch.Export != 0 && modelSearch.Export != 3)
                {
                    PathFile = _business.ExportReportByStatus(lstTable, modelSearch.Export, false, Resource.Resource.ReportProfile_AbuseType.ToLower());
                }
                return Json(new { ok = true, PathFile = PathFile, lstChart = lstChart, lstTable = lstTable }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}