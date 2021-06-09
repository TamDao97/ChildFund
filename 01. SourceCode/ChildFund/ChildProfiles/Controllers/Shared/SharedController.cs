using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Model.Model.CacheModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.Shared
{
    public class SharedController : Controller
    {
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
                    ViewBag.UserLever = userInfo.UserLever;
                }
                else
                {
                    ViewBag.UserLever = "";
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

        public PartialViewResult EditCode(string Id, string programCode)
        {
            ViewBag.Id = Id;
            ViewBag.programCode = programCode;
            return PartialView("_PartialEditCodeView");
        }
    }
}