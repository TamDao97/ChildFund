using Microsoft.AspNet.SignalR;
using NTS.Common.Utils;
using SwipeSafe.Business.ProfileReport;
using SwipeSafe.Model.ProfileReport;
using SwipeSafe.Model.SearchCondition;
using SwipeSafe.Model.SearchResults;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SwipeSafe.Controllers.ProfileReport
{
    public class ProfileReportController : Controller
    {
        ProfileReportBusiness _buss = new ProfileReportBusiness();
        // GET: ProfileReport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListProfileReport(ProfileChildSearchCondition modelSearch)
        {
            SearchResultObject<ProfileChildSearchResult> list = new SearchResultObject<ProfileChildSearchResult>();
            try
            {
                var currPage = modelSearch.PageNumber - 1;
                list = _buss.SearchProfileChild(modelSearch);
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

        public ActionResult DeleteReport(string Id)
        {
            try
            {
                _buss.DeleteProfileChild(Id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MoveReportToProfile(string Id)
        {
            try
            {
                var result = _buss.MoveReportToProfile(Id);
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                        string notifyContent = "Tạo mới ca bị xâm hại trẻ: " + item.Name.ToUpper() + " / Địa chỉ: " + item.FullAddress + " (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ")";
                        List<string> listUserId = _buss.GetListUserIdByNotify(item.ProvinceId, item.DistrictId, item.WardId, "");
                        foreach (string id in listUserId)
                        {
                            hubContext.Clients.All.GetNotify(id, notifyContent);
                        }
                    }
                }
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detail(string id)
        {
            var data = _buss.DetailProfileChild(id);
            return View(data);
        }
        public ActionResult GetContent(string id)
        {
            var data = _buss.GetContent(id);
            return PartialView(data);
        }
        public ActionResult GenVideo(string link)
        {
            ViewBag.link = link;
            return PartialView();
        }


        public ActionResult SendContent(ProcessingContentModel model)
        {
            try
            {
                _buss.SendContent(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}