
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Utils;
using ChildProfiles.Model.AreaUser;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model;
using ChildProfiles.Model.Question;
using ChildProfiles.Model.Model.Question;
using Newtonsoft.Json;
using NTS.Caching;
using System.Configuration;

namespace ChildProfiles.Business
{
    public class QuestionBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        public SearchResultObject<QuestionSearchResult> SearchSurvey(QuestionSearchCondition searchCondition)
        {
            SearchResultObject<QuestionSearchResult> searchResult = new SearchResultObject<QuestionSearchResult>();
            try
            {
                var listmodel = (from a in db.Surveys.AsNoTracking()
                                 select new QuestionSearchResult()
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     OrderNumber = a.OrderNumber,
                                     StartDate = a.StartDate,
                                     EndDate = a.EndDate,
                                     IsPublish = a.IsPublish,
                                     CountResult = db.SurveyResults.Where(u => u.SurveyId.Equals(a.Id)).Select(u => u.Id).Count()
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.Name))
                {
                    listmodel = listmodel.Where(r => r.Name.ToLower().Contains(searchCondition.Name.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.StartDate))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.StartDate);
                    listmodel = listmodel.Where(r => r.StartDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(searchCondition.EndDate))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.EndDate);
                    listmodel = listmodel.Where(r => r.EndDate <= dateTo);
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("QuestionBusiness.SearchSurvey", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        public void ChangeStatus(string id)
        {
            var survey = db.Surveys.FirstOrDefault(u => u.Id.Equals(id));
            if (survey == null)
            {
                throw new Exception("Khảo sát đã bị xóa bởi người dùng khác");
            }
            try
            {
                if (survey.IsPublish.Equals("0"))
                {
                    survey.IsPublish = Constants.ViewNotification;
                }
                else
                {
                    survey.IsPublish = Constants.NotViewNotification;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("QuestionBusiness.ChangeStatus", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
        public void DeleteSurvey(SurveyResult model)
        {
            var check = db.Surveys.FirstOrDefault(u => u.Id.Equals(model.Id));
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    db.Surveys.Remove(check);
                    var surveyResults = db.SurveyResults.Where(u => u.SurveyId.Equals(model.Id));
                    if (surveyResults.Count() > 0)
                    {
                        db.SurveyResults.RemoveRange(surveyResults);
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("QuestionBusiness.DeleteSurvey", ex.Message, model);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }
        public void CreateSurvey(SurveyModel modelSurvey)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    var model = new Survey();
                    model.Id = Guid.NewGuid().ToString();
                    model.Name = modelSurvey.Name;
                    model.IsPublish = modelSurvey.IsPublish;
                    //modelSurvey.ContentResult;
                    model.OrderNumber = modelSurvey.OrderNumber;
                    model.StartDate = DateTimeUtils.ConvertDateFromStr(modelSurvey.StartDate);
                    model.EndDate = DateTimeUtils.ConvertDateToStr(modelSurvey.EndDate);
                    model.CreateBy = modelSurvey.CreateBy;
                    model.UpdateBy = modelSurvey.CreateBy;
                    model.CreateDate = dateNow;
                    model.UpdateDate = dateNow;
                    if (modelSurvey.ListGroupQuestion == null)
                    {
                        modelSurvey.ListGroupQuestion = new List<GroupQuestionModel>();
                    }
                    foreach (var item in modelSurvey.ListGroupQuestion)
                    {
                        item.Id = Guid.NewGuid().ToString();
                        item.SurveyId = model.Id;
                        if (item.ListQuestion == null)
                        {
                            item.ListQuestion = new List<QuestionsModel>();
                        }
                        foreach (var item2 in item.ListQuestion)
                        {
                            item2.Id = Guid.NewGuid().ToString();
                            item2.GroupQuestionId = item.Id;
                        }
                    }
                    model.ContentResult = JsonConvert.SerializeObject(modelSurvey.ListGroupQuestion);
                    db.Surveys.Add(model);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("QuestionBusiness.CreateSurvey", ex.Message, modelSurvey);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }
        public void UpdateSurvey(SurveyModel modelSurvey)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var dateNow = DateTime.Now;
                    var model = db.Surveys.FirstOrDefault(u => u.Id.Equals(modelSurvey.Id));
                    if (model == null)
                    {
                        throw new Exception("Chủ đề đã bị xóa bởi người dùng khác");
                    }
                    model.Name = modelSurvey.Name;
                    model.OrderNumber = modelSurvey.OrderNumber;
                    model.IsPublish = modelSurvey.IsPublish;

                    model.StartDate = DateTimeUtils.ConvertDateFromStr(modelSurvey.StartDate);
                    model.EndDate = DateTimeUtils.ConvertDateToStr(modelSurvey.EndDate);
                    model.CreateBy = modelSurvey.CreateBy;
                    model.UpdateBy = modelSurvey.CreateBy;
                    model.CreateDate = dateNow;
                    model.UpdateDate = dateNow;
                    if (modelSurvey.ListGroupQuestion == null)
                    {
                        modelSurvey.ListGroupQuestion = new List<GroupQuestionModel>();
                    }
                    foreach (var item in modelSurvey.ListGroupQuestion)
                    {
                        if (string.IsNullOrEmpty(item.Id))
                        {
                            item.Id = Guid.NewGuid().ToString();
                        }
                        item.SurveyId = model.Id;
                        if (item.ListQuestion == null)
                        {
                            item.ListQuestion = new List<QuestionsModel>();
                        }
                        foreach (var item2 in item.ListQuestion)
                        {
                            if (string.IsNullOrEmpty(item2.Id))
                            {
                                item2.Id = Guid.NewGuid().ToString();
                            }
                            item2.GroupQuestionId = item.Id;
                        }
                    }
                    model.ContentResult = JsonConvert.SerializeObject(modelSurvey.ListGroupQuestion);
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("QuestionBusiness.UpdateSurvey", ex.Message, modelSurvey);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }
        public SurveyModel GetInfo(string id, bool IsGetCache)
        {
            try
            {
                SurveyModel result = new SurveyModel();
                var model = db.Surveys.FirstOrDefault(u => u.Id.Equals(id));
                if (model == null)
                {
                    throw new Exception("Chủ đề đã bị xóa bởi người dùng khác");
                }
                string KeyCache = ConfigurationManager.AppSettings["cacheQuetion"] + ":" + id + ":";
                QuestionsModel cacheModel;
                result.Id = id;
                result.Name = model.Name;
                result.ContentResult = model.ContentResult;
                result.IsPublish = model.IsPublish;
                result.OrderNumber = model.OrderNumber;
                result.StartDate = model.StartDate.Value.ToString("dd/MM/yyyy");
                result.EndDate = model.EndDate.Value.ToString("dd/MM/yyyy"); ;
                result.ListGroupQuestion = JsonConvert.DeserializeObject<List<GroupQuestionModel>>(model.ContentResult);

                if (IsGetCache)
                {
                    RedisService<QuestionsModel> redisService = RedisService<QuestionsModel>.GetInstance();
                    if (result.ListGroupQuestion == null)
                    {
                        result.ListGroupQuestion = new List<GroupQuestionModel>();
                    }
                    foreach (var item in result.ListGroupQuestion)
                    {
                        if (item.ListQuestion == null)
                        {
                            item.ListQuestion = new List<QuestionsModel>();
                        }
                        foreach (var item2 in item.ListQuestion)
                        {
                            cacheModel = redisService.Get<QuestionsModel>(KeyCache + item2.Id);
                            if (cacheModel != null)
                            {
                                if (item2.ListAnswer == null)
                                {
                                    item2.ListAnswer = new List<AnswerModel>();
                                }
                                for (int i = 0; i < item2.ListAnswer.Count; i++)
                                {
                                    try
                                    {
                                        item2.ListAnswer[i].CountSelect = cacheModel.ListAnswer[i].CountSelect;
                                        item2.ListAnswer[i].ListUser = cacheModel.ListAnswer[i].ListUser;
                                    }
                                    catch (Exception) { }
                                }
                            }
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("QuestionBusiness.GetInfo", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
    }
}
