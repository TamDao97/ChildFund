using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Common;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.HoSoTre
{
    public class ProfilesUpdateController : Controller
    {

        ChildProfileUpdateBusiness _business = new ChildProfileUpdateBusiness();
        // GET: ProfilesUpdate

        [MyAuthorize]
        public ActionResult CompareProfile(string id)
        {
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var backUrl = "";
                if (userInfo.UserLever.Equals(Constants.LevelArea))
                {
                    backUrl = "ProfileWard";
                }
                else
                {
                    backUrl = "ProfileProvince";
                }
                ViewBag.backUrl = backUrl;
                var lstResult = _business.CompareProfile(id);
                return View(lstResult);
            }
            catch (Exception ex)
            { return View(); }
        }
        public ActionResult Delete(ChildProfileModel model)
        {
            try
            {
                _business.DeleteChildProfile(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ConfimProfile(ChildProfileModel model)
        {
            try
            {
                model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _business.ConfimProfile(model);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        //danh sách hồ sơ cấp tỉnh
        [MyAuthorize]
        public ActionResult ProfileProvince()
        {
            return View();
        }
        public ActionResult GetProfileProvince(ChildProfileSearchCondition modelSearch)
        {
            //var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //var modelJson = Request.Form["Model"];
            // ChildProfileSearchCondition modelSearch = JsonConvert.DeserializeObject<ChildProfileSearchCondition>(modelJson, dateTimeConverter);

            SearchResultObject<ChildProfileSearchResult> list = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.SearchChildProfileProvince(modelSearch);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = modelSearch.PageSize;
                ViewBag.PathFile = list.PathFile;
                ViewBag.ListId = string.Join(";", list.ListId);
                if (list.TotalItem > modelSearch.PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(modelSearch.PageSize, currPage, list.TotalItem, "");
                }
                return PartialView(list.ListResult);
            }
            catch (Exception)
            {
                return PartialView();
            }

        }

        //danh sách hồ sơ địa phương
        [MyAuthorize]
        public ActionResult ProfileWard()
        {
            return View();
        }
        public ActionResult GetProfileWard(ChildProfileSearchCondition modelSearch)
        {
            // var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //  var modelJson = Request.Form["Model"];
            // ChildProfileSearchCondition modelSearch = JsonConvert.DeserializeObject<ChildProfileSearchCondition>(modelJson, dateTimeConverter);

            SearchResultObject<ChildProfileSearchResult> list = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                ViewBag.Index = 0;
                modelSearch.UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.SearchChildProfileWard(modelSearch);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = modelSearch.PageSize;
                ViewBag.PathFile = list.PathFile;
                ViewBag.ListId = string.Join(";", list.ListId);
                if (list.TotalItem > modelSearch.PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(modelSearch.PageSize, currPage, list.TotalItem, "");
                }
                return PartialView(list.ListResult);
            }
            catch (Exception)
            {
                return PartialView();
            }

        }

        public ActionResult SaveChangeCode(string Id, string programCode)
        {
            try
            {
                string data = _business.SaveChangeCode(Id, programCode);
                return Json(new { ok = true, mess = data });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportStorySelect(ChildProfileExport model)
        {
            try
            {
                var rs = _business.ExportStorySelect(model);
                return Json(new { ok = true, mess = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}