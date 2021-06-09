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
using ChildProfiles.Model.Question;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.Question;
using ChildProfiles.Model.Model.CacheModel;

namespace ChildProfiles.Controllers
{
    public class QuestionController : Controller
    {
        QuestionBusiness _business = new QuestionBusiness();
        // GET: Question
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListSurvey(QuestionSearchCondition modelSearch)
        {
            SearchResultObject<QuestionSearchResult> list = new SearchResultObject<QuestionSearchResult>();
            try
            {
                ViewBag.Index = 0;
                var currPage = modelSearch.PageNumber - 1;
                list = _business.SearchSurvey(modelSearch);
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
        public ActionResult ChangeStatus(string id)
        {
            try
            {
                _business.ChangeStatus(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateSurvey()
        {
            return View();
        }
        public ActionResult UpdateSurvey(string id)
        {
            try
            {
                var data = _business.GetInfo(id,false);
                return View(data);
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
                _business.DeleteSurvey(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create(SurveyModel model)
        {
            try
            {
                model.CreateBy = System.Web.HttpContext.Current.User.Identity.Name;
                _business.CreateSurvey(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Update(SurveyModel model)
        {
            try
            {
                _business.UpdateSurvey(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GenListAnser(AnswerModelTemp model)
        {
            return PartialView(model);
        }
        public ActionResult GenAnserOther(string type, string idq)
        {
            ViewBag.idq = idq;
            ViewBag.type = type;
            return PartialView();
        }
        public ActionResult GenAnser(string type, string idq)
        {
            ViewBag.idq = idq;
            ViewBag.type = type;
            return PartialView();
        }
        public ActionResult GenQuestion(string nameGroup)
        {
            ViewBag.nameGroup = nameGroup;
            return PartialView();
        }
        public ActionResult GenGroupQuestion()
        {
            return PartialView();
        }
        public ActionResult GenPartHtml(string type)
        {
            ViewBag.type = type;
            return PartialView();
        }

        //su dung update
        public ActionResult GenGroupQuestionUpdate(List<GroupQuestionModel> ListGroupQuestion)
        {
            return PartialView(ListGroupQuestion);
        }
        public ActionResult GenQuestionUpdate(string nameGroup, List<QuestionsModel> ListQuestion)
        {
            ViewBag.nameGroup = nameGroup;
            return PartialView(ListQuestion);
        }
        //xem chi tiết
        public ActionResult DetailSurvey(string id)
        {
            try
            {
                var data = _business.GetInfo(id,true);

                return View(data);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public ActionResult GenGroupQuestionDetail(List<GroupQuestionModel> ListGroupQuestion)
        {
            return PartialView(ListGroupQuestion);
        }
        public ActionResult GenQuestionDetail(string nameGroup, List<QuestionsModel> ListQuestion)
        {
            ViewBag.nameGroup = nameGroup;
            return PartialView(ListQuestion);
        }
        public ActionResult GenListAnserDetail(AnswerModelTemp model)
        {
            return PartialView(model);
        }
    }
}