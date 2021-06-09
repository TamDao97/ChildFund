using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Common;
using ChildProfiles.Model;
using ChildProfiles.Model.Model.ChildStory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.Story
{
    public class StorySampleController : Controller
    {
        SampleStoryDA DA = new SampleStoryDA();
        UserBusiness _user = new UserBusiness();
        // GET: StorySample
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListStorySample(SampleStorySearchModel searchModel)
        {
            try
            {
                ViewBag.Index=0;
                var currPage = searchModel.PageNumber - 1;
                SearchResultObject<SampleStoryModel> list = DA.GetListSampleStory(searchModel);
                ViewBag.Index = (currPage * searchModel.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = searchModel.PageSize;
                if(list.ListResult.Count>0)
                {
                    foreach (var item in list.ListResult)
                    {
                        if (!string.IsNullOrEmpty(item.CreateBy))
                        {
                            var rs = _user.GetById(item.CreateBy);
                            if (rs != null)
                            {
                                item.CreateBy = rs.UserName;
                            }
                        }
                        if (!string.IsNullOrEmpty(item.UpdateBy))
                        {
                            var rs = _user.GetById(item.UpdateBy);
                            if (rs != null)
                            {
                                item.UpdateBy = rs.UserName;
                            }
                        }
                    }
                }
                
                if (list.TotalItem > searchModel.PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(searchModel.PageSize, currPage, list.TotalItem, "");
                }
                //ViewBag.HtmlPager = HtmlPager.GetPage("", searchModel.PageNumber, searchModel.PageSize, list.TotalItem);
                return PartialView(list.ListResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult FormStoryTemplate(string id)
        {
            try
            {
                SampleStoryModel model = DA.GetInfoTemplate(id);
                model.Category = DA.GetCategoryTemplate();
                return PartialView(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public ActionResult UpdateTemplate(SampleStoryModel model)
        {
            try
            {
                string userid = HttpContext.User.Identity.Name;
                model.UpdateBy = userid;
                DA.UpdateTemplate(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddTemplate(SampleStoryModel model)
        {
            try
            {
                string userid = HttpContext.User.Identity.Name;
                model.CreateBy = userid;
                DA.AddTemplate(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ChangeStatus(string id, string status)
        {
            try
            {
                DA.UpdateStatus(id,status);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteSampleStory(string id)
        {
            try
            {
                DA.DeleteTemplate(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}