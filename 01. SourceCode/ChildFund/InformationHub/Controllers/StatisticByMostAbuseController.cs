using InformationHub.Business.Business;
using InformationHub.Model.StatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticByMostAbuseController : BaseController
    {
        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0032")]
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize(Roles = "C0032")]
        public ActionResult StatisticWitMostAbuse(StatisticSearchCondition modelSearch)
        {
            StatisticByMostAbuseModel list = new StatisticByMostAbuseModel();
            try
            {
                list = _business.StatisticWitMostAbuse(modelSearch);
                ViewBag.Index = 0;

                return Json(new { ok = true, lstProvince = list.LstProvince, lstWard = list.LstWard, lstDistrict = list.LstDistrict, lstAbuse = list.LstAbuse }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}