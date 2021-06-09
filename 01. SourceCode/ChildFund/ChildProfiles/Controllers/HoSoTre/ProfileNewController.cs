using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using ChildProfiles.Model.Model.FliesLibrary;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NTS.Common;
using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.ProfileNew
{
    public class ProfileNewController : Controller
    {
        ChildProfileBusiness _business = new ChildProfileBusiness();
        ChildProfilePrintBusiness _printBusiness = new ChildProfilePrintBusiness();
        // GET: ProfileNew
        [HttpPost]
        public ActionResult AddProfileNew()
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var modelJson = Request.Form["Model"];
                ChildProfileModel model = JsonConvert.DeserializeObject<ChildProfileModel>(modelJson, dateTimeConverter);
                model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                _business.AddChildProfile(model, httpFile);

                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FormProfileNew()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.Name = userInfo.Name;
            //ViewBag.ProvinceId = userInfo.ProvinceId;
            ViewBag.Title = "Thêm hồ sơ mới/Enrol new children";
            return View(_business.GetInfoChildProfileApproved(""));
        }
        public ActionResult FormUpdateProfiles()
        {
            ViewBag.Title = "Cập nhật hồ sơ/RAM updates";
            try
            {
                string id = "";
                if (Request["id"] != null)
                {
                    id = Request["id"].ToString();
                    var data = _business.GetInfoChildProfileApproved(id);
                    return View(data);
                    //return View();
                }
                else
                {
                    throw new Exception("Không tồn tại hồ sơ trong hệ thống");
                }
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }



        }

        [HttpPost]
        public ActionResult UpdateChildProfile()
        {
            try
            {
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                var modelJson = Request.Form["Model"];
                ChildProfileModel model = JsonConvert.DeserializeObject<ChildProfileModel>(modelJson, dateTimeConverter);
                model.UpdateBy = System.Web.HttpContext.Current.User.Identity.Name;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                string outPath = _business.UpdateChildProfile(model, httpFile);

                return Json(new { ok = true, Path = outPath, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
        public ActionResult ProfileConfim()
        {

            ViewBag.Type = "Confim";
            return View();
        }
        [MyAuthorize]
        public ActionResult ProfileProvince()
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
            ViewBag.Type = "Province";
            return View();
        }
        public ActionResult GetProfileProvince(ChildProfileSearchCondition modelSearch)
        {
            // var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            // var modelJson = Request.Form["Model"];
            // ChildProfileSearchCondition modelSearch = JsonConvert.DeserializeObject<ChildProfileSearchCondition>(modelJson, dateTimeConverter);

            SearchResultObject<ChildProfileSearchResult> list = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                ViewBag.Index = 0;
                ViewBag.Type = modelSearch.Type;
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
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.UserLever = userInfo.UserLever;
            return View();
        }
        public ActionResult GetProfileWard(ChildProfileSearchCondition modelSearch)
        {
            //  var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //  var modelJson = Request.Form["Model"];
            //  ChildProfileSearchCondition modelSearch = JsonConvert.DeserializeObject<ChildProfileSearchCondition>(modelJson, dateTimeConverter);

            SearchResultObject<ChildProfileSearchResult> list = new SearchResultObject<ChildProfileSearchResult>();
            try
            {
                ViewBag.Index = 0;
                modelSearch.UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(modelSearch.UserId);
                modelSearch.Level = userInfo.UserLever;
                ViewBag.UserLever = userInfo.UserLever;
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

        //xuất excel
        public ActionResult ExportProfileSelect(ChildProfileExport model)
        {
            try
            {
                var rs = _business.ExportProfileSelect(model);
                return Json(new { ok = true, mess = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportChildProfile(ChildProfileExport model)
        {
            try
            {
                var rs = _printBusiness.ExportChildProfile(model);
                return Json(new { ok = true, mess = rs }, JsonRequestBehavior.AllowGet);
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

        [MyAuthorize]
        public ActionResult DetailProfile(string id)
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
                var data = _business.DetailProfile(id);
                return View(data);
            }
            catch (Exception)
            { }
            return View();
        }

        [MyAuthorize]
        public ActionResult DetailProfileConfim(string id)
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
                var data = _business.DetailProfile(id);
                return View(data);
            }
            catch (Exception)
            { }
            return View();
        }

        public ActionResult UpdateStory(ChildProfileModel model)
        {
            try
            {
                _business.UpdateStory(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ResetStory(ChildProfileModel model)
        {
            try
            {
                model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                var content = _business.ResetStory(model);
                return Json(new { ok = true, mess = content }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ViewImage()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.UserLever = userInfo.UserLever;
            ViewBag.Title = "Danh sách ảnh báo cáo chuyển biến";
            string id = "";
            if (Request["id"] != null)
            {
                id = Request["id"].ToString();

                return View(_business.GetImageByChildId(id));
            }
            else
            {
                throw new Exception("Không tồn tại ảnh trẻ trong hệ thống");
            }
        }
        public ActionResult DeleteImageChild(ImageActionModel model)
        {
            try
            {
                _business.DeleteImageChild(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DownImageChild(ImageActionModel model)
        {
            try
            {
                var data = new ImageLibraryDA().DownImageChild(model.ImageId);
                return Json(new { ok = true, mess = data }, JsonRequestBehavior.AllowGet);
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
                    _business.ImportProfile(createBy, httpFile[0]);
                }
                return Json(new { ok = true }, JsonRequestBehavior.AllowGet);
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
                var data = new ImageLibraryDA().DownTemplate();
                return Json(new { ok = true, mess = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
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
    }
}