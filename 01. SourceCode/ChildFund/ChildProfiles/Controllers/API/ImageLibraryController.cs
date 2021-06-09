using ChildProfiles.Business.Business;
using ChildProfiles.Model.Model.FliesLibrary;
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
    [RoutePrefix("api/ImageLibrary")]
    public class ImageLibraryController : ApiController
    {

        private ImageLibraryDA _business = new ImageLibraryDA();

        [Route("UploadImage")]
        [HttpPost]
        public HttpResponseMessage UploadImage()
        {
            try
            {
                var modelJson = HttpContext.Current.Request.Form["Model"];
                ShareImageModel model = JsonConvert.DeserializeObject<ShareImageModel>(modelJson);
                HttpFileCollection httpFile = System.Web.HttpContext.Current.Request.Files;

                _business.UploadImage(httpFile, model);
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
