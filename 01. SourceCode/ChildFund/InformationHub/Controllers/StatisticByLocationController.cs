using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model.StatisticModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticByLocationController : BaseController
    {

        StatisticBusiness _business = new StatisticBusiness();

        [MyAuthorize(Roles = "C0028")]
        public ActionResult Index()
        {
            string userId = HttpContext.User.Identity.Name;
            var user = _business.FindUser(userId);
            ViewBag.type = user.Type;
            var abuse = new ComboboxBusiness().GetAllAbuseType();
            return View(abuse);
        }
        [MyAuthorize(Roles = "C0028")]
        public ActionResult ListReportProfileByLocation(StatisticSearchCondition modelSearch)
        {
            ReturnLocationModel list = new ReturnLocationModel();
            try
            {
                list = _business.SearchStatisticByLocation(modelSearch);
                var count = list.LstTable.Count;
                int listLeftCount = count / 2;
                if (count % 2 != 0)
                {
                    listLeftCount++;
                }
                var listLeft = list.LstTable.Skip(0).Take(listLeftCount).ToList();
                var listRight = list.LstTable.Skip(listLeftCount).Take(count).ToList();

                return Json(new { ok = true, lstType = list.LstType, lstTableLeft = listLeft, lstTableRight = listRight, lstChart = list.LstChart, lstLocation = list.ListLocation, PathFile = list.PathFile }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}