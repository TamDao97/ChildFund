using InformationHub.Business.Business;
using InformationHub.Model.StatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticByChildFundAreaController : BaseController
    {
        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0033")]
        public ActionResult Index()
        {
            string userId = HttpContext.User.Identity.Name;
            var user = _business.FindUser(userId);
            ViewBag.type = user.Type;
            return View();
        }
        [MyAuthorize(Roles = "C0033")]
        public ActionResult ListReportProfileByChildFundArea(StatisticSearchCondition modelSearch)
        {
            ReturnChildFundAreaModel list = new ReturnChildFundAreaModel();
            try
            {
                list = _business.SearchStatisticByChildFundArea(modelSearch);

                return Json(new { ok = true, lstChart = list.LstChart, lstTable = list.LstTable, lstArea = list.LstArea }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exx)
            {

                return Json(new { ok = false, mess = exx.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}