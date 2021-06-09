using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.SearchResults;
using InformationHub.Model.StatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NTS.Common;


namespace InformationHub.Controllers
{
    public class StatisticByGenderController : BaseController
    {
        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0027")]
        public ActionResult Index()
        {
            string userName = HttpContext.User.Identity.Name;
            var user = _business.FindUser(userName);
            ViewBag.provinceId = user.ProvinceId;
            ViewBag.districtId = user.DistrictId;
            ViewBag.wardId = user.WardId;
            ViewBag.type = user.Type;
            ViewBag.dateTimeNow = DateTime.Now;
            return View();
        }
        [MyAuthorize(Roles = "C0027")]
        public ActionResult ListReportProfileByGender(StatisticSearchCondition modelSearch)
        {
            try
            {
                var data = _business.SearchStatisticByGender(modelSearch);

                return Json(new { ok = true, PathFile = data.PathFile, lstChart = data.LstChart, lstTable = data.LstTable, lstAbuse = data.LstAbuse }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {

                return Json(new { ok = false, mess = exx.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
