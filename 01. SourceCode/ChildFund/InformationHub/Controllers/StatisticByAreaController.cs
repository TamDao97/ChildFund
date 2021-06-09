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
    public class StatisticByAreaController : BaseController
    {
        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0029")]
        public ActionResult Index()
        {
            string userId = HttpContext.User.Identity.Name;
            var user = _business.FindUser(userId);
            ViewBag.provinceId = user.ProvinceId;
            ViewBag.districtId = user.DistrictId;
            ViewBag.province = user.ProvinceName;
            ViewBag.district = user.DistrictName;
            ViewBag.type = user.Type;
            return View();
        }
        [MyAuthorize(Roles = "C0029")]
        public ActionResult ListReportProfileByArea(StatisticSearchCondition modelSearch)
        {
            ReturnModel list = new ReturnModel();
            try
            {
                string userName = HttpContext.User.Identity.Name;
                var user = _business.FindUser(userName);
                ViewBag.type = user.Type;
                list = _business.SearchStatisticByArea(modelSearch, user.Type);
                
                return Json(new { ok = true, lstChart = list.LstChart, lstTable = list.LstTable, lstArea = list.LstArea }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {

                return Json(new { ok = false, mess = exx.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}