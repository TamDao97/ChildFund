using SwipeSafe.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SwipeSafe.Controllers.API
{
    [RoutePrefix("api/Combobox")]
    public class ComboboxController : ApiController
    {
        ComboboxBusiness _buss = new ComboboxBusiness();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id vùng</param>
        /// <returns></returns>
        [Route("GetFormsAbuse")]
        [HttpGet]
        public HttpResponseMessage GetFormsAbuse()
        {
            try
            {
                var result = _buss.GetFormsAbuses();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetRelationship")]
        [HttpGet]
        public HttpResponseMessage GetRelationship()
        {
            try
            {
                var result = _buss.GetRelationshipCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id vùng</param>
        /// <returns></returns>
        [Route("GetAllProvince")]
        [HttpGet]
        public HttpResponseMessage GetProvince()
        {
            try
            {
                var result = _buss.GetProvinceCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Get All District
        /// </summary>
        /// <returns></returns>
        [Route("GetAllDistrict")]
        [HttpGet]
        public HttpResponseMessage GetDistrict()
        {
            try
            {
                var result = _buss.GetDistrictCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get All Ward
        /// </summary>
        /// <returns></returns>
        [Route("GetAllWard")]
        [HttpGet]
        public HttpResponseMessage GetWard()
        {
            try
            {
                var result = _buss.GetWardCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
