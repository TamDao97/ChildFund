using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model.CacheModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InformationHub.Common;
namespace InformationHub.Controllers.Shared
{
    public class SharedController : BaseController
    {
        public ActionResult SetLanguage(string lang)
        {
            new LanguageManagement().SetLanguage(lang);
            return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult MenuLeft()
        {
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo != null)
                {
                    ViewBag.Type = userInfo.Type;
                }
                else
                {
                    ViewBag.Type = -1;
                }

            }
            catch (Exception)
            {
            }
            return PartialView();

        }

        public ActionResult GenNotify()
        {
            HomesBusiness _buss = new HomesBusiness();
            List<NotifyModel> lst = new List<NotifyModel>();
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                lst = _buss.GetNotify(userId).ToList();
                ViewBag.counAll = lst.Count();
                ViewBag.countNotify = lst.Where(u => u.Status.Equals("0")).ToList().Count;
                lst = lst.OrderByDescending(u => u.CreateDate).Take(5).ToList();
            }
            catch (Exception)
            {
            }
            return PartialView(lst);
        }
    }
}