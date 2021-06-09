using ChildProfiles.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.SurveyResult;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChildProfiles.Controllers.API
{
    [RoutePrefix("api/SurveyResult")]
    public class SurveyResultController : ApiController
    {
        private SurveyResultBusiness _business = new SurveyResultBusiness();

        [Route("SearchSurveyResult")]
        [HttpPost]
        public HttpResponseMessage SearchSurveyResult(SurveyResultSearchCondition modelSearch)
        {
            try
            {
                var result = _business.SearchSurveyResult(modelSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetInfoSurveyResult")]
        [HttpGet]
        public HttpResponseMessage GetInfoSurveyResult(string id)
        {
            try
            {
                SurveyResultModel result = _business.GetInfo(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddSurveyResult")]
        [HttpPost]
        public HttpResponseMessage AddSurveyResult(SurveyResultModel model)
        {
            try
            {
                _business.CreateSurveyResult(model);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Request.CreateResponse(HttpStatusCode.OK, model.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}