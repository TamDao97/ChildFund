using NTS.Utils;
using System;
using System.Linq;
using NTS.Common.Utils;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model;
using ChildProfiles.Model.SurveyResult;
using ChildProfiles.Model.Model.Question;
using ChildProfiles.Model.Question;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ChildProfiles.Business
{
    public class SurveyResultBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        public SearchResultObject<SurveyResultModel> SearchSurveyResult(SurveyResultSearchCondition searchCondition)
        {
            SearchResultObject<SurveyResultModel> searchResult = new SearchResultObject<SurveyResultModel>();
            try
            {
                var listmodel = (from a in db.SurveyResults.AsNoTracking()
                                 join b in db.Surveys.AsNoTracking() on a.SurveyId equals b.Id into ab
                                 from ab1 in ab.DefaultIfEmpty()
                                 join c in db.Users.AsNoTracking() on a.UserId equals c.Id into ac
                                 from ac1 in ac.DefaultIfEmpty()
                                 select new SurveyResultModel()
                                 {
                                     Id = a.Id,
                                     SurveyName = ab1.Name,
                                     SurveyId = ab1.Id,
                                     UserName = ac1.Name,
                                     UserId = ac1.Id,
                                     CreateDate = a.CreateDate,
                                     ContentResult = a.ContentResult
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.SurveyId))
                {
                    listmodel = listmodel.Where(r => r.SurveyId.ToLower() == searchCondition.SurveyId.ToLower());
                }
                if (!string.IsNullOrEmpty(searchCondition.UserName))
                {
                    listmodel = listmodel.Where(r => r.UserName.ToLower().Contains(searchCondition.UserName.ToLower()));
                }
                if (!string.IsNullOrEmpty(searchCondition.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(searchCondition.DateFrom);
                    listmodel = listmodel.Where(r => r.CreateDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(searchCondition.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(searchCondition.DateTo);
                    listmodel = listmodel.Where(r => r.CreateDate <= dateTo);
                }
                searchResult.TotalItem = listmodel.Select(u => u.Id).Count();
                searchResult.ListResult = SQLHelpper.OrderBy(listmodel, searchCondition.OrderBy, searchCondition.OrderType).Skip((searchCondition.PageNumber - 1) * searchCondition.PageSize).Take(searchCondition.PageSize).ToList();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("SurveyResultBusiness.SearchSurveyResult", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }
        public SurveyResultModel GetInfo(string id)
        {
            SurveyResultModel result = new SurveyResultModel();
            try
            {
                var model = (from a in db.SurveyResults.AsNoTracking()
                             where a.Id.Equals(id)
                             join b in db.Surveys.AsNoTracking() on a.SurveyId equals b.Id into ab
                             from ab1 in ab.DefaultIfEmpty()
                             join c in db.Users.AsNoTracking() on a.UserId equals c.Id into ac
                             from ac1 in ac.DefaultIfEmpty()
                             select new SurveyResultModel()
                             {
                                 Id = a.Id,
                                 SurveyId = a.SurveyId,
                                 SurveyName = ab1.Name,
                                 UserId = a.UserId,
                                 UserName = ac1.Name,
                                 ContentResult = a.ContentResult,
                                 CreateDate = a.CreateDate
                             }).AsQueryable();
                if (model == null)
                {
                    throw new Exception("Chủ đề đã bị xóa bởi người dùng khác");
                }
                result.SurveyName = model.Select(i => i.SurveyName).First();
                result.UserName = model.Select(i => i.UserName).First();
                result.CreateDate = model.Select(i => i.CreateDate).First();
                result.ListGroupQuestion = JsonConvert.DeserializeObject<List<GroupQuestionModel>>(model.Select(i => i.ContentResult).First());
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("SurveyResultBusiness.GetInfo", ex.Message, id);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return result;
        }
        public void CreateSurveyResult(SurveyResultModel modelSurvey)
        {
            using (var trans = db.Database.BeginTransaction())
            {
                try
                {
                    var model = new SurveyResult();
                    model.Id = Guid.NewGuid().ToString();
                    model.SurveyId = modelSurvey.SurveyId;
                    model.UserId = modelSurvey.UserId;
                    model.CreateDate = DateTime.Now;
                    model.ContentResult = JsonConvert.SerializeObject(modelSurvey.ListGroupQuestion);
                    db.SurveyResults.Add(model);

                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    LogUtils.ExceptionLog("SurveyResultBusiness.CreateSurveyResult", ex.Message, modelSurvey);
                    trans.Rollback();
                    throw new Exception(ErrorMessage.ERR001, ex.InnerException);
                }
            }
        }
        public void DeleteSurveyResult(SurveyResult model)
        {
            try
            {
                var data = db.SurveyResults.FirstOrDefault(u => u.Id.Equals(model.Id));
                if (data == null)
                {
                    throw new Exception("Survey Result này đã được xóa khỏi hệ thống");
                }
                //delete
                db.SurveyResults.Remove(data);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("SurveyResultBusiness.DeleteSurveyResult", ex.Message, model);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
        }
    }
}
