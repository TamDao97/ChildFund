using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NTS.Common;
using SwipeSafe.Business;
using SwipeSafe.Model.ProfileReport;
using SwipeSafe.Model.SearchResults;
using SwipeSafe.Model.SwipeSafeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SwipeSafe.Controllers.API
{
    [RoutePrefix("api/Report")]
    public class ReportApiController : ApiController
    {
        ReportBusiness _buss = new ReportBusiness();
        [Route("AddReport")]
        [HttpPost]
        public HttpResponseMessage AddReport()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ReportModel model = JsonConvert.DeserializeObject<ReportModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;
                _buss.AddReport(model, httpFile);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
