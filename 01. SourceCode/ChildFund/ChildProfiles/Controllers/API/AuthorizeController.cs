using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ChildProfiles.Controllers.API
{
    [RoutePrefix("api/Authorize")]
    public class AuthorizeController : ApiController
    {
        private AuthorizeBusiness authorizeBusiness = new AuthorizeBusiness();

        [Route("Login")]
        [HttpPost]
        public HttpResponseMessage Login(LoginModel loginModel)
        {
            try
            {
                LoginProfileModel loginProfileModel = authorizeBusiness.Login(loginModel);
                return Request.CreateResponse(HttpStatusCode.OK, loginProfileModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// get thông tin hồ sơ
        /// </summary>
        /// <returns></returns>
        [Route("GetProfileUser")]
        [HttpGet]
        public HttpResponseMessage GetProfileUser(string id)
        {
            try
            {
                ProfileUserModel loginProfileModel = authorizeBusiness.GetProfileUser(id);
                return Request.CreateResponse(HttpStatusCode.OK, loginProfileModel);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật hồ sơ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateProfileUser")]
        public HttpResponseMessage UpdateProfileUser()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ProfileUserModel model = JsonConvert.DeserializeObject<ProfileUserModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                var result = authorizeBusiness.UpdateProfileUser(model, httpFile);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ChangePasswordUser")]
        public HttpResponseMessage ChangePasswordUser(ChangePasswordUserModel model)
        {
            try
            {
                bool result = authorizeBusiness.ChangePasswordUser(model);
                return Request.CreateResponse(HttpStatusCode.OK, result.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ForwardPassword")]
        public HttpResponseMessage ForwardPassword(ForwardPasswordModel model)
        {
            try
            {
                string userId = authorizeBusiness.ForwardPassword(model);
                return Request.CreateResponse(HttpStatusCode.OK, userId);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ConfirmForwardPassword")]
        public HttpResponseMessage ConfirmForwardPassword(ForwardPasswordModel model)
        {
            try
            {
                bool result = authorizeBusiness.ConfirmForwardPassword(model);
                return Request.CreateResponse(HttpStatusCode.OK, result.ToString());
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
