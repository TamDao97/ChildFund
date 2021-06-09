using SwipeSafe.Business.Authorize;
using SwipeSafe.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SwipeSafe.Controllers.API
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
    }
}
