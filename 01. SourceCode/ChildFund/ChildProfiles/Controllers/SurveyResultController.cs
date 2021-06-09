using ChildProfiles.Business;
using ChildProfiles.Model;
using System.Web.Mvc;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.SurveyResult;
using System;
using ChildProfiles.Model.Model.Question;
using Newtonsoft.Json;
using System.Linq;
using ChildProfiles.Business.Business;
using ChildProfiles.Model.Question;
using System.Collections.Generic;

namespace ChildProfiles.Controllers
{
    public class SurveyResultController : Controller
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        SurveyResultBusiness _surveyResultBusiness = new SurveyResultBusiness();
        public ActionResult Index(string id)
        {
            var surveyName = db.Surveys.Where(i => i.Id.Equals(id)).Select(i => i.Name);
            ViewBag.id = id;
            ViewBag.name = surveyName;
            return View();
        }
        public ActionResult ListSurveyResult(SurveyResultSearchCondition modelSearch)
        {
            SearchResultObject<SurveyResultModel> list = new SearchResultObject<SurveyResultModel>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _surveyResultBusiness.SearchSurveyResult(modelSearch);
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
        public ActionResult GetSurveyResultInfo(string id)
        {
            try
            {
                var result = _surveyResultBusiness.GetInfo(id);
                return View(result);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public ActionResult Delete(SurveyResult model)
        {
            try
            {
                _surveyResultBusiness.DeleteSurveyResult(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenListAnswerResult(AnswerModelTemp model)
        {
            return PartialView(model);
        }
        public ActionResult GenQuestionResult(string nameGroup, List<QuestionsModel> ListQuestion)
        {
            ViewBag.nameGroup = nameGroup;
            return PartialView(ListQuestion);
        }
        public ActionResult GenGroupQuestionResult(List<GroupQuestionModel> ListGroupQuestion)
        {
            return PartialView(ListGroupQuestion);
        }
    }
}
