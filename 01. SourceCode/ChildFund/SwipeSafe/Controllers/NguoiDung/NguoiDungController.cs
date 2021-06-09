using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NTS.Common.Utils;
using System.Web.Security;
using SwipeSafe.Business.User;
using SwipeSafe.UserModels;
using SwipeSafe.Model.SearchResults;

namespace ChildProfiles.Controllers.NguoiDung
{
    public class NguoiDungController : Controller
    {
        UserBusiness _userBusiness = new UserBusiness();
        // GET: User
 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListUser(UserSearchCondition modelSearch)
        {
            SearchResultObject<UserSearchResult> list = new SearchResultObject<UserSearchResult>();
            try
            {
                ViewBag.Index = 0;
                string userid = HttpContext.User.Identity.Name;
                ViewBag.UserName = userid;
                var currPage = modelSearch.PageNumber - 1;
                list = _userBusiness.SearchUser(modelSearch);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = modelSearch.PageSize;
                ViewBag.Type = modelSearch.Type;
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

        public ActionResult DeleteUser(UserModel model)
        {
            try
            {
                _userBusiness.DeleteUser(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateUser()
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var modelJson = System.Web.HttpContext.Current.Request.Form["model"];
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.None
                };
                UserModel model = JsonConvert.DeserializeObject<UserModel>(modelJson, dateTimeConverter);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                _userBusiness.CreateUser(model, httpFile);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    
        public ActionResult UpdateUser()
        {
            try
            {
                var modelJson = System.Web.HttpContext.Current.Request.Form["model"];
                JsonConvert.DefaultSettings = () => new JsonSerializerSettings
                {
                    DateParseHandling = DateParseHandling.None
                };
                UserModel model = JsonConvert.DeserializeObject<UserModel>(modelJson, new JsonSerializerSettings
                {
                    Error = delegate (object sender, ErrorEventArgs args)
                    {
                        args.ErrorContext.Handled = true;
                    },
                    Converters = {
                        new IsoDateTimeConverter() }
                });
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                _userBusiness.UpdateUser(model, httpFile);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetUserInfo(string id)
        {
            try
            {
                var data = _userBusiness.GetUserInfo(id);
                return Json(new { ok = true, mess = "", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        
        public ActionResult ResetPassword(string id, string password)
        {
            try
            {
                _userBusiness.ResetPassword(id, password);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
       
        public ActionResult ChangeStatus(string id)
        {
            try
            {
                _userBusiness.ChangeStatus(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}