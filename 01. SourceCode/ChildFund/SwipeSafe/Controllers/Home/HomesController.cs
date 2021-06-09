
using Microsoft.AspNet.SignalR;
using NTS.Common;
using SwipeSafe;
using SwipeSafe.Business;
using SwipeSafe.Model.Model.CacheModel;
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