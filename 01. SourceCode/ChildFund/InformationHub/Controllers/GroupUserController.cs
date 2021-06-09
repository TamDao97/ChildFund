using InformationHub.Business.Business;
using InformationHub.Model.Model.Function;
using InformationHub.Model.Model.GroupUser;
using InformationHub.Model.SearchResults;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class GroupUserController : BaseController
    {
        GroupUserBussiness _bussiness = new GroupUserBussiness();
        // GET: GroupUser
        [MyAuthorize(Roles = "C0006")]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize(Roles = "C0006")]
        public ActionResult ListGroupUser(GroupUserSearchCondition modelSearch)
        {
            SearchResultObject<GroupUserSearchResult> list = new SearchResultObject<GroupUserSearchResult>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                string userid = HttpContext.User.Identity.Name;
                list = _bussiness.SearchGroupUser(modelSearch);
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
                //throw new Exception(ex.Message);
                return PartialView();
            }
        }

        [MyAuthorize(Roles = "C0009")]
        public ActionResult LockGroup(string id)
        {
            try
            {
                _bussiness.LockGroup(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyAuthorize(Roles = "C0009")]
        public ActionResult UnLockGroup(string id)
        {
            try
            {
                _bussiness.UnLockGroup(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: GroupUser
        [MyAuthorize(Roles = "C0007")]
        public ActionResult CreateGroupUser()
        {
            return View();
        }

        public ActionResult GetListPermission(string type)
        {
            List<FunctionModel> list = new List<FunctionModel>();
            try
            {
                ViewBag.Index = 0;
                ViewBag.GroupUserId = type;
                list = _bussiness.GetAllFunction(type);
                ViewBag.TotalItem = list.Where(u => !u.Index.Equals("0")).Count();
                return PartialView(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [MyAuthorize(Roles = "C0007")]
        public ActionResult Create(GroupUserModel model)
        {
            try
            {
                model.CreateBy = HttpContext.User.Identity.Name;
                model.UpdateBy = HttpContext.User.Identity.Name;
                //  model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _bussiness.CreateGroupUser(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyAuthorize(Roles = "C0008")]
        public ActionResult UpdateGroupUser(string id)
        {
            try
            {

                var data = _bussiness.GetGroupUserById(id);
                ViewBag.data = JsonConvert.SerializeObject(data); ;
                return View(data);

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult GetGroupUserInfo(string id)
        {
            try
            {
                var data = _bussiness.GetGroupUserById(id);
                return Json(new { ok = true, mess = "", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetListPermissionUpdate(string groupUserId, string type)
        {
            try
            {
                ViewBag.Index = 0;
                var list = _bussiness.GetListPermissionUpdate(groupUserId, type);
                ViewBag.TotalItem = list.Where(u => !u.Index.Equals("0")).Count();
                return PartialView(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [MyAuthorize(Roles = "C0008")]
        public ActionResult Update(GroupUserModel model)
        {
            try
            {
                model.UpdateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _bussiness.UpdateGroupUser(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}