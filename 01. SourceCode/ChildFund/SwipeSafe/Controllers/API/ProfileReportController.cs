using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using SwipeSafe.Business.ProfileReport;
using SwipeSafe.Model.ProfileReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SwipeSafe.Controllers.API
{
    [RoutePrefix("api/ProfileReport")]
    public class ProfileReportController : ApiController
    {
        ProfileReportBusiness _buss = new ProfileReportBusiness();

        [Route("SearchProfileChild")]
        [HttpPost]
        public HttpResponseMessage SearchProfileChild(ProfileChildSearchCondition model)
        {
            try
            {
                var result = _buss.SearchProfileChild(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddProfileChild")]
        [HttpPost]
        public HttpResponseMessage AddProfileChild()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ProfileChildModel model = JsonConvert.DeserializeObject<ProfileChildModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                _buss.AddProfileChild(model, httpFile);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                string notifyContent = "Tạo mới ca bị xâm hại trẻ: " + model.ChildName.ToUpper() + " / Địa chỉ: " + model.FullAddress + " (" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ")";
                List<string> listUserId = _buss.GetListUserIdByNotify(model.ProvinceId, model.DistrictId, model.WardId, model.CreateBy);
                foreach (string id in listUserId)
                {
                    hubContext.Clients.All.GetNotify(id, notifyContent);
                }

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("DetailProcessingContent")]
        [HttpPost]
        public HttpResponseMessage DetailProcessingContent(string id)
        {
            try
            {
                var result = _buss.DetailProcessingContent(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddProcessingContent")]
        [HttpPost]
        public HttpResponseMessage AddProcessingContent(ProcessingContentModel model)
        {
            try
            {
                _buss.AddProcessingContent(model);
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetProcessingContentByProfile")]
        [HttpPost]
        public HttpResponseMessage GetProcessingContentByProfile(string id)
        {
            try
            {
                var result = _buss.GetProcessingContentByProfile(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetNotify")]
        [HttpPost]
        public HttpResponseMessage GetNotify(string id)
        {
            try
            {
                var result = _buss.GetNotify(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("CountNotify")]
        [HttpPost]
        public HttpResponseMessage CountNotify(string id)
        {
            try
            {
                var result = _buss.CountNotify(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("TickNotify")]
        [HttpPost]
        public HttpResponseMessage TickNotify(string userId, string notifyId)
        {
            try
            {
                _buss.TickNotify(userId, notifyId);
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
