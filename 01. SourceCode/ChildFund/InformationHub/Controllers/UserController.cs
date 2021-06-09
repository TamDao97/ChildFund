using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.SearchCondition;
using InformationHub.Model.SearchResults;
using InformationHub.Model.UserModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NTS.Common;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class UserController : BaseController
    {
        UserBusiness _userBusiness = new UserBusiness();
        // GET: User
        [MyAuthorize(Roles = "C0001")]
        public ActionResult Index()
        {
            var allowedIport = false;
            string createBy = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(createBy);
            if (userInfo != null)
            {
                if (userInfo.UserLever.Equals(Constants.LevelAdmin) || (userInfo.UserLever.Equals(Constants.LevelOffice) && string.IsNullOrEmpty(userInfo.ProvinceId)))
                {
                    allowedIport = true;
                }
            }
            ViewBag.allowedIport = allowedIport;
            return View();
        }

        [MyAuthorize(Roles = "C0001")]
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
                ViewBag.PathFile = list.PathFile;
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

        [MyAuthorize(Roles = "C0002")]
        public ActionResult CreateUserView()
        {
            return View();
        }

        [MyAuthorize(Roles = "C0003")]
        public ActionResult UpdateUserView(string id)
        {
            try
            {
                var data = _userBusiness.GetUserInfo(id);
                ViewBag.data = JsonConvert.SerializeObject(data); ;
                return View(data);
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        [MyAuthorize(Roles = "C0003")]
        public ActionResult UpdateUser()
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var modelJson = Request.Form["Model"];
                UserModel model = JsonConvert.DeserializeObject<UserModel>(modelJson, dateTimeConverter);
                model.UpdateBy = System.Web.HttpContext.Current.User.Identity.Name;
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

        [MyAuthorize(Roles = "C0002")]
        public ActionResult AddUser()
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var modelJson = Request.Form["Model"];
                var model = JsonConvert.DeserializeObject<UserModel>(modelJson, dateTimeConverter);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _userBusiness.CreateUser(model, httpFile);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListPermission(string id, int type)
        {
            List<GroupPermissonViewModel> list = new List<GroupPermissonViewModel>();
            try
            {
                ViewBag.Index = 0;
                ViewBag.GroupUserId = id;
                list = _userBusiness.GetListPermission(id, type);
                ViewBag.TotalItem = list.Where(i => i.PermissionId != null).Count();
                return PartialView(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult ListPermissionUpdate(string groupUserId, string userId, int type)
        {
            try
            {
                ViewBag.Index = 0;
                var list = _userBusiness.GetListPermissionUpdate(groupUserId, userId, type);
                ViewBag.TotalItem = list.Where(i => i.PermissionId != null).Count();
                return PartialView(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [MyAuthorize(Roles = "C0005")]
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

        [MyAuthorize(Roles = "C0004")]
        public ActionResult LockUser(string id)
        {
            try
            {
                _userBusiness.LockUser(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyAuthorize(Roles = "C0004")]
        public ActionResult UnLockUser(string id)
        {
            try
            {
                _userBusiness.UnLockUser(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImportExcel()
        {
            try
            {
                _userBusiness.ImportExcel();
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadTemplate()
        {
            try
            {
                var file = _userBusiness.DownloadTemplate();
                return Json(new { ok = true, PathFile = file, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ImportProfile()
        {
            try
            {
                string createBy = System.Web.HttpContext.Current.User.Identity.Name;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                if (httpFile.Count > 0)
                {
                    _userBusiness.ImportProfile(createBy, httpFile[0]);
                }
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}