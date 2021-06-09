using ChildProfiles.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.FliesLibrary;
using ChildProfiles.Model.ReportProfileModel;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.HoSoTre
{
    public class ReportProfileController : Controller
    {
        // GET: ReportProfile
        ReportProfileBusiness _business = new ReportProfileBusiness();
        //cấp tỉnh
        [MyAuthorize]
        public ActionResult ReportProvince()
        {
            return View();
        }
        public ActionResult GetReportProvince(ReportProfileSearchCondition modelSearch)
        {
            //  var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            // var modelJson = Request.Form["Model"];
            // ReportProfileSearchCondition modelSearch = JsonConvert.DeserializeObject<ReportProfileSearchCondition>(modelJson, dateTimeConverter);
            SearchResultObject<ReportProfileSearchResult> list = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.SearchReportProfileProvince(modelSearch);
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
            catch (Exception)
            {
                return PartialView();
            }

        }

        // cấp địa phương
        [MyAuthorize]
        public ActionResult ReportWard()
        {
            return View();
        }
        public ActionResult GetReportWard(ReportProfileSearchCondition modelSearch)
        {
            //  var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
            //  var modelJson = Request.Form["Model"];
            // ReportProfileSearchCondition modelSearch = JsonConvert.DeserializeObject<ReportProfileSearchCondition>(modelJson, dateTimeConverter);
            modelSearch.UserId = System.Web.HttpContext.Current.User.Identity.Name;

            SearchResultObject<ReportProfileSearchResult> list = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.SearchReportProfileWard(modelSearch);
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
            catch (Exception)
            {
                return PartialView();
            }

        }

        //dùng chung
        public ActionResult Download(ReportProfile model)
        {
            try
            {
                var rs = _business.GetFileDownload(model);
                return Json(new { ok = true, mess = rs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(ReportProfile model)
        {
            try
            {
                _business.DeleteReportProfile(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ConfimProvince(ReportProfile model)
        {
            try
            {
                model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _business.ConfimReportProfile(model);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UploadAttachFile(string id)
        {
            try
            {
                var modelJson = System.Web.HttpContext.Current.Request.Form["model"];
                UploadAttachFileModel model = JsonConvert.DeserializeObject<UploadAttachFileModel>(modelJson);
                var createBy = System.Web.HttpContext.Current.User.Identity.Name;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                _business.UploadAttachFile(httpFile, model, createBy);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetAttachFile(string id)
        {
            try
            {
                var rs = _business.GetAttachFile(id);
                return Json(new { ok = true, data = rs, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetReportProfile(string id)
        {
            try
            {
                var rs = _business.DetailProfile(id);
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                List<FamilyMemberModel> familyMember = new List<FamilyMemberModel>();
                if (!string.IsNullOrEmpty(rs.FamilyMember))
                {
                    var familyMemberDb = JsonConvert.DeserializeObject<List<FamilyMemberModel>>(rs.FamilyMember, dateTimeConverter);
                    var familyMemberFa = familyMemberDb.FirstOrDefault(u => u.RelationshipId.Equals("R0001"));
                    var familyMemberMo = familyMemberDb.FirstOrDefault(u => u.RelationshipId.Equals("R0007"));
                    if (familyMemberFa == null)
                    {
                        familyMemberFa = new FamilyMemberModel();
                    }
                    familyMember.Add(familyMemberFa);
                    if (familyMemberMo == null)
                    {
                        familyMemberMo = new FamilyMemberModel();
                    }
                    familyMember.Add(familyMemberMo);
                }

                return Json(new { ok = true, data = rs, mess = "", familyMember = familyMember }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}