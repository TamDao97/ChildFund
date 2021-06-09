using ChildProfiles.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.AreaUser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ChildProfiles.Common;
using System.Web.Script.Serialization;
using ChildProfiles.Model.Entity;

namespace ChildProfiles.Controllers
{
    public class AreaUserController : Controller
    {
        AreaUserBusiness _business = new AreaUserBusiness();
        // GET: AreaUser
        public ActionResult Index()
        {
            return View();
        }
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

        public ActionResult CreateAreaUser()
        {
            return View();
        }
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

        public ActionResult SaveVillage(Village model)
        {
            try
            {
                var data = _business.SaveVillage(model);
                return Json(new { ok = true, mess = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteVillage(string id)
        {
            try
            {
                _business.DeleteVillage(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}