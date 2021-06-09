using ChildProfiles.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model.Model.Api;
using ChildProfiles.Model.Model.ChildProfileModels;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ChildProfiles.Controllers.API
{
    [RoutePrefix("api/ChildProfiles")]
    public class ChildProfilesController : ApiController
    {
        private ChildProfileBusiness _business = new ChildProfileBusiness();

        [Route("SearchChilldProfile")]
        [HttpPost]
        public HttpResponseMessage SearchChilldProfile(ChildProfileSearchCondition model)
        {
            try
            {
                var result = _business.SearchChildProfileMobiles(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        ///Lấy thông tin trên giao diện
        /// </summary>
        /// <returns></returns>
        [Route("GetInfoChildProfile")]
        [HttpGet]
        public HttpResponseMessage GetInfoChildProfile(string id)
        {
            try
            {
                ChildProfileModel result = _business.GetInfoChildProfile(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Thêm mới hồ sơ
        /// </summary>
        /// <returns></returns>
        [Route("AddChildProfile")]
        [HttpPost]
        public HttpResponseMessage AddChildProfile()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ChildProfileModel model = JsonConvert.DeserializeObject<ChildProfileModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                _business.AddChildProfile(model, httpFile);
                if (!string.IsNullOrEmpty(model.UserLever) && model.UserLever.Equals(Constants.LevelTeacher))
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                    hubContext.Clients.All.GetNotify();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ hồ sơ
        /// </summary>
        /// <returns></returns>
        [Route("UpdateChildProfile")]
        [HttpPost]
        public HttpResponseMessage UpdateChildProfile()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ChildProfileModel model = JsonConvert.DeserializeObject<ChildProfileModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                _business.UpdateChildProfile(model, httpFile);
                if (!string.IsNullOrEmpty(model.UserLever) && model.UserLever.Equals(Constants.LevelTeacher))
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                    hubContext.Clients.All.GetNotify();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddReportProfile")]
        [HttpPost]
        public HttpResponseMessage AddReportProfile()
        {
            try
            {
                //ReportProfileBusiness reportProfileBusiness = new ReportProfileBusiness();
                //var result = reportProfileBusiness.AddReportProfile(model);
                //var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                //hubContext.Clients.All.GetNotify();
                //return Request.CreateResponse(HttpStatusCode.OK, result.ToString());

                var modelJson = HttpContext.Current.Request.Form["Model"];
                ReportProfilesModel model = JsonConvert.DeserializeObject<ReportProfilesModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                ReportProfileBusiness reportProfileBusiness = new ReportProfileBusiness();
                reportProfileBusiness.AddReportProfile(model, httpFile);
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
                hubContext.Clients.All.GetNotify();
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("SaveVillage")]
        [HttpPost]
        public HttpResponseMessage SaveVillage(Village model)
        {
            try
            {
                AreaUserBusiness areaUserBusiness = new AreaUserBusiness();
                var result = areaUserBusiness.SaveVillage(model);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("DeleteVillage")]
        [HttpPost]
        public HttpResponseMessage DeleteVillage(string id)
        {
            try
            {
                AreaUserBusiness areaUserBusiness = new AreaUserBusiness();
                areaUserBusiness.DeleteVillage(id);
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("AddImageChilByYear")]
        [HttpPost]
        public HttpResponseMessage AddImageChilByYear()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ImageChildByYear model = JsonConvert.DeserializeObject<ImageChildByYear>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                _business.AddImageChilByYear(model, httpFile);
                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Lấy hồ sơ 
        /// </summary>
        /// <param name="ListId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetChildProfiles")]
        public HttpResponseMessage GetChildProfiles(ChildProfileDownloadSearch childProfileDownloadSearch)
        {
            try
            {
                var result = _business.GetChildProfiles(childProfileDownloadSearch);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
