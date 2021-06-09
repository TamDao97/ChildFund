using InformationHub.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InformationHub.Model;
using Newtonsoft.Json;
using NTS.Common;
using System.Net;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using InformationHub.Model.SearchResults;
using InformationHub.Model.Model.ReportProfile;
using Microsoft.AspNet.SignalR;
using InformationHub.Business.Business;
using System.ComponentModel.DataAnnotations;
using InformationHub.Common;
using NTS.Common.Utils;

namespace InformationHub.Controllers
{
    public class ReportProfileController : BaseController
    {
        //Khởi tạo các business
        private ReportProfileBusiness _business = new ReportProfileBusiness();

        //danh sách
        [MyAuthorize(Roles = "C0010")]
        public ActionResult Index()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.Type = userInfo.Type;
            ViewBag.Title = Resource.Resource.ReportProfile_List;
            return View();
        }
        //danh sách chuyển cấp trên
        [MyAuthorize(Roles = "C0016")]
        public ActionResult Forward()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.Type = userInfo.Type;
            ViewBag.Title = Resource.Resource.ReportProfile_Forward_List;
            return View();
        }

        //chuyển cấp trên
        [MyAuthorize(Roles = "C0015")]
        public ActionResult SendForward(ReportForwardModel model)
        {
            try
            {
                _business.SendForward(model);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //  [MyAuthorize(Roles = "C0010")]
        public ActionResult GetList(ReportProfileSearchCondition modelSearch)
        {
            var checkPermission = LoginCommon.CheckPermission("C0010");
            if (!checkPermission)
            {
                return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
            }
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            if (userInfo == null)
            {
                return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
            }

            SearchResultObject<ReportProfileSearchResult> list = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                ViewBag.Type = userInfo.Type;
                modelSearch.UserId = userId;
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.GetList(modelSearch, userInfo);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.TotalItemStatus6 = list.TotalItemStatus6;
                ViewBag.TotalItemStatus1 = list.TotalItemStatus1;
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
                return Json(new { Ok = false, Message = Resource.Resource.ErroProcess_Title }, JsonRequestBehavior.AllowGet);
            }
        }
        //     [MyAuthorize(Roles = "C0016")]
        public ActionResult GetListForward(ReportProfileSearchCondition modelSearch)
        {
            SearchResultObject<ReportProfileSearchResult> list = new SearchResultObject<ReportProfileSearchResult>();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0016");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.Type = userInfo.Type;
                modelSearch.UserId = userId;
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.GetListForwards(modelSearch, userInfo);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.TotalItemStatus6 = list.TotalItemStatus6;
                ViewBag.TotalItemStatus1 = list.TotalItemStatus1;
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
        // thêm
        [MyAuthorize(Roles = "C0011")]
        public ActionResult CreateProfileView()
        {
            ViewBag.reviewName = InformationHub.Common.LoginCommon.GetUserName();
            ViewBag.Title = Resource.Resource.ReportProfile_Create;
            ViewBag.evaluationFirstModel = _business.GetEvaluationFirstModel("-1");
            ViewBag.caseVerificationModel = _business.GetCaseVerificationModel("-1");
            ReportProfileModel model = new ReportProfileModel();
            return View(model);
        }

        /// <summary>
        /// Tạo mới sự vụ
        /// </summary>
        /// <returns></returns>
        /// [MyAuthorize(Roles = "C0011")]
        /// 
        [ValidateInput(false)]
        // [MyAuthorize(Roles = "C0011")]
        public ActionResult CreateProfile()
        {
            ReportProfileModel model = new ReportProfileModel();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0011");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }

                var modelJson = System.Web.HttpContext.Current.Request.Unvalidated.Form["model"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                model = JsonConvert.DeserializeObject<ReportProfileModel>(modelJson, dateTimeConverter);
                model.CreateBy = userId;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                var Id = _business.CreateProfile(model, httpFile);
                return Json(new { Ok = true, Id = Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileController.CreateProfile", ex.Message, model);
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //cap nhât
        [MyAuthorize(Roles = "C0012")]
        public ActionResult UpdateProfileView(string id)
        {
            ViewBag.Title = Resource.Resource.ReportProfile_Update;
            var data = _business.GetInfo(id);
            ViewBag.listFile = JsonConvert.SerializeObject(data.ListProfileAttachment);
            return View(data);
        }
        [ValidateInput(false)]
        public ActionResult UpdateProProfile()
        {
            ReportProfileModel model = new ReportProfileModel();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0011");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }

                var modelJson = System.Web.HttpContext.Current.Request.Unvalidated.Form["model"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                model = JsonConvert.DeserializeObject<ReportProfileModel>(modelJson, dateTimeConverter);
                model.CreateBy = userId;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                string outPath = _business.UpdateProfile(model, httpFile);
                return Json(new { Ok = true, Path = outPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileController.UpdateProProfile", ex.Message, model);
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //view update truong hợp
        public ActionResult GenViewFile(FileModel model)
        {
            if (model.ListProfileAttachment == null)
            {
                model.ListProfileAttachment = new List<ProfileAttachmentModel>();
            }
            return PartialView(model.ListProfileAttachment);
        }
        public ActionResult GenViewFileSup(FileModel model)
        {
            if (model.ListProfileAttachment == null)
            {
                model.ListProfileAttachment = new List<ProfileAttachmentModel>();
            }
            return PartialView(model.ListProfileAttachment);
        }
        public ActionResult GenViewFileUpdate(FileModel model)
        {
            if (model.ListProfileAttachment == null)
            {
                model.ListProfileAttachment = new List<ProfileAttachmentModel>();
            }
            return PartialView(model.ListProfileAttachment);
        }
        public ActionResult GenViewFileUpdateSub(FileModel model)
        {
            if (model.ListProfileAttachment == null)
            {
                model.ListProfileAttachment = new List<ProfileAttachmentModel>();
            }
            return PartialView(model.ListProfileAttachment);
        }
        [MyAuthorize(Roles = "C0014")]
        public ActionResult PublishReport(string id)
        {
            try
            {
                _business.PublishReport(id);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyAuthorize(Roles = "C0001")]
        public ActionResult ReOpenCase(string id)
        {
            try
            {
                _business.ReOpenCase(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [MyAuthorize(Roles = "C0001")]
        public ActionResult DeleteCase(string id)
        {
            try
            {
                _business.DeleteCase(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult FinishReport(string id)
        {
            try
            {
                _business.FinishReport(id);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Json(new { Ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CloseReport(string id)
        {
            try
            {
                _business.CloseReport(id);
                return Json(new { Ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //xua lý truong hợp
        [MyAuthorize(Roles = "C0012")]
        public ActionResult ReportProcess(string id)
        {
            ViewBag.reviewName = InformationHub.Common.LoginCommon.GetUserName();
            ViewBag.Title = Resource.Resource.ReportProfile_Process;
            var data = _business.GetInfo(id);
            ViewBag.listFile = JsonConvert.SerializeObject(data.ListProfileAttachment);
            ViewBag.evaluationFirstModel = _business.GetEvaluationFirstModel(id);
            ViewBag.caseVerificationModel = _business.GetCaseVerificationModel(id);
            return View(data);
        }

        [ValidateInput(false)]
        [MyAuthorize(Roles = "C0012")]
        public ActionResult UpdateProcessTab2()
        {
            EvaluationFirstModel modelEv = new EvaluationFirstModel();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0012");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }

                var modelJsonEv = System.Web.HttpContext.Current.Request.Unvalidated.Form["modelEv"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                modelEv = JsonConvert.DeserializeObject<EvaluationFirstModel>(modelJsonEv, dateTimeConverter);
                modelEv.CreateBy = userId;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                string outPath = _business.UpdateEvaluationFirstModel(modelEv);
                return Json(new { Ok = true, Path = outPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileController.UpdateProcessTab2", ex.Message, modelEv);
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [MyAuthorize(Roles = "C0012")]
        public ActionResult UpdateProcessTab3()
        {
            CaseVerificationModel modelCa = new CaseVerificationModel();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0012");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var modelJsonCa = System.Web.HttpContext.Current.Request.Unvalidated.Form["modelCa"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                modelCa = JsonConvert.DeserializeObject<CaseVerificationModel>(modelJsonCa, dateTimeConverter);
                modelCa.CreateBy = userId;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                string outPath = _business.UpdateCaseVerificationModel(modelCa);
                return Json(new { Ok = true, Path = outPath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileController.UpdateProcessTab3", ex.Message, modelCa);

                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [MyAuthorize(Roles = "C0012")]
        public ActionResult UpdateProcessTab4()
        {
            SupportPlantModel modelSup = new SupportPlantModel();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0012");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var modelJsonSup = System.Web.HttpContext.Current.Request.Unvalidated.Form["modelSup"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                modelSup = JsonConvert.DeserializeObject<SupportPlantModel>(modelJsonSup, dateTimeConverter);
                modelSup.CreateBy = userId;
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                var output = _business.UpdateSupportPlantModel(modelSup, httpFile);
                return Json(new { Ok = true, Path = output.Path, Id = output.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileController.UpdateProcessTab4", ex.Message, modelSup);

                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [ValidateInput(false)]
        [MyAuthorize(Roles = "C0012")]
        public ActionResult UpdateProcessTab5()
        {
            SupportAfterStatusModel model = new SupportAfterStatusModel();
            try
            {
                var checkPermission = LoginCommon.CheckPermission("C0012");
                if (!checkPermission)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                if (userInfo == null)
                {
                    return Json(new { Ok = false, Message = Resource.Resource.OutUser_Erros }, JsonRequestBehavior.AllowGet);
                }
                var modelJson = System.Web.HttpContext.Current.Request.Unvalidated.Form["modelSupAf"];
                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                model = JsonConvert.DeserializeObject<SupportAfterStatusModel>(modelJson, dateTimeConverter);
                model.CreateBy = userId;
                var output = _business.UpdateSupportAfterStatusModel(model);
                return Json(new { Ok = true, Path = output.Path, Id = output.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("ReportProfileController.UpdateProcessTab5", ex.Message, model);

                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenActionTable()
        {
            return PartialView();
        }
        public ActionResult GenListActionTable(List<OrganizationActivitiesModel> model)
        {
            return PartialView(model);
        }

        public ActionResult DeleteSupportAfter(string id)
        {
            try
            {
                _business.DeleteSupportAfter(id);
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult DeleteSupportPlant(string id)
        {
            try
            {
                _business.DeleteSupportPlant(id);
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReportDetail(string id)
        {
            string viewRequest = Request["view"];
            if (viewRequest != null)
            {
                ViewBag.viewMore = true;
                ViewBag.ForwardInfo = _business.GetForwardInfo(viewRequest);
            }
            else
            {
                ViewBag.ForwardInfo = null;
                ViewBag.viewMore = false; ;
            }
            ViewBag.Title = Resource.Resource.ReportProfile_Detail;
            var data = _business.GetInfo(id);
            ViewBag.evaluationFirstModel = _business.GetEvaluationFirstModel(id);
            ViewBag.caseVerificationModel = _business.GetCaseVerificationModel(id);
            return View(data);
        }
        public ActionResult DetailForward(string id)
        {
            try
            {
                var note = _business.DetailForward(id);
                return Json(new { ok = true, note = note, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GenListActionTableDetail(List<OrganizationActivitiesModel> model)
        {
            return PartialView(model);
        }
        public ActionResult GenViewFileUpdateSubDetail(FileModel model)
        {
            if (model.ListProfileAttachment == null)
            {
                model.ListProfileAttachment = new List<ProfileAttachmentModel>();
            }
            return PartialView(model.ListProfileAttachment);
        }

        public ActionResult ViewContent(ViewContentModel model)
        {
            var ViewContent = NTS.Common.Utils.Common.StripHtml(model.Note, false, false, false, false, false, null);
            return Json(new { Ok = true, ViewContent = ViewContent }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewContentPopup()
        {
            return PartialView();
        }
        public ActionResult ViewContentPopupDetail()
        {
            return PartialView();
        }
        public ActionResult GetContentEdit(string id)
        {
            try
            {
                var data = _business.GetContentEdit(id);
                return Json(new { Ok = true, obj = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateStatusReportForword(string id)
        {
            try
            {
                _business.UpdateStatusReportForword(id);
                return Json(new { Ok = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region[gen các view con tab 4-5]
        public ActionResult GenStep5DetailView(string id, string reportProfileId)
        {
            var supportAfterStatusModel = _business.GetSupportAfterStatusModel(id, reportProfileId);
            return PartialView(supportAfterStatusModel);
        }
        public ActionResult GenStep5View(string id, string reportProfileId)
        {
            ViewBag.reviewName5 = InformationHub.Common.LoginCommon.GetUserName();
            var supportAfterStatusModel = _business.GetSupportAfterStatusModel(id, reportProfileId);
      
            return PartialView(supportAfterStatusModel);
        }
        public ActionResult GenTitleStep5View(string reportProfileId)
        {
            var model = _business.GenTitleStep5View(reportProfileId);
            return PartialView(model);
        }
        public ActionResult GenTitleStep5DetailView(string reportProfileId)
        {
            var model = _business.GenTitleStep5View(reportProfileId);
            return PartialView(model);
        }
        public ActionResult GenTitleStep4View(string reportProfileId)
        {
            var model = _business.GenTitleStep4View(reportProfileId);
            return PartialView(model);
        }
        public ActionResult GenStep4View(string id, string reportProfileId)
        {
            var supportPlantModel = _business.GetSupportPlantModel(id, reportProfileId);
            ViewBag.supportPlantModel = supportPlantModel;
            ViewBag.listFileSup = JsonConvert.SerializeObject(supportPlantModel.ListProfileAttachment);
            return PartialView();
        }

        public ActionResult GenTitleStep4DetailView(string reportProfileId)
        {
            var model = _business.GenTitleStep4View(reportProfileId);
            return PartialView(model);
        }
        public ActionResult GenStep4DetailView(string id, string reportProfileId)
        {
            var supportPlantModel = _business.GetSupportPlantModel(id, reportProfileId);
            ViewBag.supportPlantModel = supportPlantModel;
            ViewBag.listFileSup = JsonConvert.SerializeObject(supportPlantModel.ListProfileAttachment);
            return PartialView();
        }
        #endregion

        public ActionResult ExportWordForm1(string profileId)
        {
            try
            {
                string createBy = System.Web.HttpContext.Current.User.Identity.Name;
                var Path = _business.ExportWordForm1(profileId, createBy);
                return Json(new { Ok = true, Path = Path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportWordForm2(string profileId)
        {
            try
            {
                string createBy = System.Web.HttpContext.Current.User.Identity.Name;
                var Path = _business.ExportWordForm2(profileId, createBy);
                return Json(new { Ok = true, Path = Path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportWordForm3(string profileId)
        {
            try
            {
                string createBy = System.Web.HttpContext.Current.User.Identity.Name;
                var Path = _business.ExportWordForm3(profileId);
                return Json(new { Ok = true, Path = Path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportWordForm4(string profileId, string id)
        {
            try
            {
                var Path = _business.ExportWordForm4(id, profileId);
                return Json(new { Ok = true, Path = Path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExportWordForm5(string profileId, string id)
        {
            try
            {
                string createBy = System.Web.HttpContext.Current.User.Identity.Name;
                var Path = _business.ExportWordForm5(id, profileId, createBy);
                return Json(new { Ok = true, Path = Path }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Ok = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}