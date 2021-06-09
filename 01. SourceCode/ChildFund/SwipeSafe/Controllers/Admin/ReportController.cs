using SwipeSafe.Model.SearchResults;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Utils;
using SwipeSafe.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using SwipeSafe.Model.Repositories;
using SwipeSafe.Business;

namespace SwipeSafe.Controllers.Admin
{
    public class ReportController : Controller
    {
        ReportBusiness _buss = new ReportBusiness();
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListReport(ReportSearchCondition modelSearch)
        {
            SearchResultObject<ReportSearchResult> list = new SearchResultObject<ReportSearchResult>();
            try
            {
                var currPage = modelSearch.PageNumber - 1;
                list = _buss.SearchReport(modelSearch);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = modelSearch.PageSize;
                if (list.TotalItem > modelSearch.PageSize)
                {
                    ViewBag.pages = Common.PhanTrang(modelSearch.PageSize, currPage, list.TotalItem, "");
                }
                return PartialView(list.ListResult);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public ActionResult DeleteReport(ReportSearchResult model)
        {
            try
            {
                _buss.DeleteReport(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Detail(string id)
        {
            var data = _buss.Detail(id);
            return View(data);
        }
        public ActionResult GenVideo(string link)
        {
            ViewBag.link = link;
            return PartialView();
        }
    }
}