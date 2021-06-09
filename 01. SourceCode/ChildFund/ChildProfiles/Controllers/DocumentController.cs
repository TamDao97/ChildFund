using ChildProfiles;
using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Controllers.Base;
using ChildProfiles.Model;
using ChildProfiles.Model.Document;
using ChildProfiles.Model.DocumentType;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers
{
    public class DocumentController : BaseController
    {
        DocumentBussiness _bussiness = new DocumentBussiness();
        // GET: Document
        public ActionResult Index()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.Type = userInfo.UserLever;
            return View();
        }

        public ActionResult ListDocument(DocumentLibrarySearchCondition modelSearch)
        {
            SearchResultObject<DocumentLibrarySearchResult> list = new SearchResultObject<DocumentLibrarySearchResult>();
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                ViewBag.Type = userInfo.UserLever;
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                string userid = HttpContext.User.Identity.Name;
                list = _bussiness.SearchDocument(modelSearch);
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

        public ActionResult Delete(DocumentLibraryModel model)
        {
            try
            {
                _bussiness.DeleteDocumentLibrary(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Create()
        {
            try
            {
                var modelJson = System.Web.HttpContext.Current.Request.Form["modelDocumentCreate"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                DocumentLibraryModel modelDocumentCreate = JsonConvert.DeserializeObject<DocumentLibraryModel>(modelJson, dateTimeConverter);
                modelDocumentCreate.UploadBy = HttpContext.User.Identity.Name;
                modelDocumentCreate.UpdateBy = HttpContext.User.Identity.Name;
                _bussiness.CreateDocumentLibrary(modelDocumentCreate, httpFile);
                return Json(new { Ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateDocument()
        {
            return View();
        }

        public ActionResult UpdateDocument(string id)
        {
            try
            {

                var data = _bussiness.GetDocumentLibById(id);
                ViewBag.data = JsonConvert.SerializeObject(data); ;
                return View(data);

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public ActionResult GetDocumentLibInfo(string id)
        {
            try
            {
                var data = _bussiness.GetDocumentLibById(id);
                return Json(new { ok = true, mess = "", data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Update()
        {
            try
            {
                var modelJson = System.Web.HttpContext.Current.Request.Form["modelDocumentUpdate"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                DocumentLibraryModel model = JsonConvert.DeserializeObject<DocumentLibraryModel>(modelJson, dateTimeConverter);
                model.UpdateBy = System.Web.HttpContext.Current.User.Identity.Name;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                _bussiness.UpdateDocument(model, httpFile);
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadFile(string id)
        {
            try
            {

                string path = _bussiness.DownloadFile(id);
                return Json(new { Ok = true, path = path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //DocumentType
        public ActionResult ListDocumentType(DocumentTypeSearchCondition modelSearch)
        {
            SearchResultObject<DocumentTypeSearchResult> list = new SearchResultObject<DocumentTypeSearchResult>();
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                ViewBag.Type = userInfo.UserLever;
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                string userid = HttpContext.User.Identity.Name;
                list = _bussiness.SearchDocumentType(modelSearch);
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

        public ActionResult DeleteDocumentType(DocumentTypeModel model)
        {
            try
            {
                _bussiness.DeleteDocumentType(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CreateDocumentType(DocumentTypeModel model)
        {
            try
            {
                model.CreateBy = HttpContext.User.Identity.Name;
                model.UpdateBy = HttpContext.User.Identity.Name;
                _bussiness.CreateDocumentType(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateDocumentType(DocumentTypeModel model)
        {
            try
            {
                model.UpdateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _bussiness.UpdateDocumentType(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}