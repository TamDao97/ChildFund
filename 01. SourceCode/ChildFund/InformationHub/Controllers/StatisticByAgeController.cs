using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.Repositories;
using InformationHub.Model.SearchResults;
using InformationHub.Model.StatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticByAgeController : BaseController
    {
        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0026")]
        public ActionResult Index()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.provinceId = userInfo.ProvinceId;
            ViewBag.districtId = userInfo.DistrictId;
            ViewBag.wardId = userInfo.WardId;
            ViewBag.type = userInfo.Type;
            ViewBag.dateTimeNow = DateTime.Now;
            return View();
        }
        [MyAuthorize(Roles = "C0026")]
        public ActionResult ListReportProfileByAge(StatisticSearchCondition modelSearch)
        {
            ResultStatisticByAge list = new ResultStatisticByAge();
            try
            {
                list = _business.SearchStatisticByAge(modelSearch);

                return Json(new { ok = true, PathFile = list.PathFile, listTable = list.ListStatisticByAgeModel, ageValue = list.AgeValue, chartData = list.ChartData, chartRightData = list.ChartData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [MyAuthorize(Roles = "C0026")]
        public ActionResult GetRighChart(StatisticSearchCondition modelSearch)
        {
            List<ChartAgeModel> list = new List<ChartAgeModel>();
            try
            {
                list = _business.GetRightChart(modelSearch);

                return Json(new { ok = true, chartRightData = list }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}