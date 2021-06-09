using Microsoft.AspNet.SignalR;
using SwipeSafe.Business;
using SwipeSafe.Model.Model.CacheModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwipeSafe.Controllers.Shared
{
    public class SharedController : Controller
    {
        ReportBusiness _buss = new ReportBusiness();

        public ActionResult MenuLeft()
        {
            return PartialView();
        }
        public ActionResult GenNotify()
        {
            List<NotifyModel> lst = new List<NotifyModel>();
            try
            {
                var userId = "Admin";
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
                var userId = "Admin";
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