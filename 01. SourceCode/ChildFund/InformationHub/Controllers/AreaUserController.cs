using InformationHub.Business;
using InformationHub.Model;
using InformationHub.Model.AreaUser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using InformationHub.Common;
using System.Web.Script.Serialization;
using InformationHub.Model.SearchResults;

namespace InformationHub.Controllers
{
    public class AreaUserController : BaseController
    {
        AreaUserBusiness _business = new AreaUserBusiness();
        // GET: AreaUser
        [MyAuthorize(Roles = "C0017")]
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize(Roles = "C0017")]
        public ActionResult ListAreaUser(AreaUserSearchCondition modelSearch)
        {
            SearchResultObject<AreaUserSearchResult> list = new SearchResultObject<AreaUserSearchResult>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.SearchAreaUser(modelSearch);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = modelSearch.PageSize;
                if (list.TotalItem > modelSearch.PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(modelSearch.PageSize, currPage, list.TotalItem, "");
                }
                return PartialView(list.ListResult);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [MyAuthorize(Roles = "C0018")]
        public ActionResult CreateAreaUser()
        {
            return View();
        }
        [MyAuthorize(Roles = "C0019")]
        public ActionResult UpdateAreaUser(string id)
        {
            try
            {
                var data = _business.GetInfo(id);
                ViewBag.data = JsonConvert.SerializeObject(data); ;
                return View(data);

            }
            catch (Exception ex)
            {
                return View();
            }

        }
        [MyAuthorize(Roles = "C0020")]
        public ActionResult Delete(AreaUserModel model)
        {
            try
            {
                _business.DeleteAreaUser(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [MyAuthorize(Roles = "C0018")]
        public ActionResult Create(AreaUserModel model)
        {
            try
            {
              //  model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _business.CreateAreaUser(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [MyAuthorize(Roles = "C0019")]
        public ActionResult Update(AreaUserModel model)
        {
            try
            {
                _business.UpdateAreaUser(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}