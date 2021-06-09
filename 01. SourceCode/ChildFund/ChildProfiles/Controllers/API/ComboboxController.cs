using ChildProfiles.Business.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChildProfiles.Controllers.API
{
    [RoutePrefix("api/Combobox")]
    public class ComboboxController : ApiController
    {
        ComboboxDA _data = new ComboboxDA();
        /// <summary>
        /// tôn giáo
        /// </summary>
        /// <returns></returns>
        [Route("GetGeligion")]
        [HttpGet]
        public HttpResponseMessage GetGeligion()
        {
            try
            {
                var result = _data.GetGeligionCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        /// <summary>
        /// dân tộc
        /// </summary>
        /// <returns></returns>
        [Route("GetNation")]
        [HttpGet]
        public HttpResponseMessage GetNation()
        {
            try
            {
                var result = _data.GetNationCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("GetAreaUser")]
        [HttpGet]
        public HttpResponseMessage GetAreaUser()
        {
            try
            {
                var result = _data.GetAreaUserCBB();
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
        [Route("GetProvinceByArea")]
        [HttpGet]
        public HttpResponseMessage GetProvinceByArea(string areaUserId)
        {
            try
            {
                var result = _data.GetProvinceByArea(areaUserId);
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
        /// <param name="Id">Id vùng</param>
        /// <returns></returns>
        [Route("GetDistrictByArea")]
        [HttpGet]
        public HttpResponseMessage GetDistrictByArea(string areaDistrictId)
        {
            try
            {
                var result = _data.GetDistrictByArea(areaDistrictId);
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
        /// <param name="Id">Id vùng</param>
        /// <returns></returns>
        [Route("GetWardByArea")]
        [HttpGet]
        public HttpResponseMessage GetWardByArea(string areaWardId)
        {
            try
            {
                var result = _data.GetWardByArea(areaWardId);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Mối quan hệ trong gia đình
        /// </summary>
        /// <returns></returns>
        [Route("GetRelationship")]
        [HttpGet]
        public HttpResponseMessage GetRelationship()
        {
            try
            {
                var result = _data.RelationshipCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Danh sách nghề nghiệp
        /// </summary>
        /// <returns></returns>
        [Route("GetJob")]
        [HttpGet]
        public HttpResponseMessage GetJob()
        {
            try
            {
                var result = _data.GetJobCBB();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Danh sách Thon xom theo id xa
        /// </summary>
        /// <returns></returns>
        [Route("GetVillageByWrad")]
        [HttpGet]
        public HttpResponseMessage GetVillageByWrad(string id)
        {
            try
            {
                var result = _data.GetVillageByWrad(id);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        /// <summary>
        /// Danh sách nội dung báo cáo
        /// </summary>
        /// <returns></returns>
        [Route("GetReportContent")]
        [HttpGet]
        public HttpResponseMessage GetReportContent()
        {
            try
            {
                var result = _data.GetReportContent();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Danh sách nội dung báo cáo
        /// </summary>
        /// <returns></returns>
        [Route("GetSchool")]
        [HttpGet]
        public HttpResponseMessage GetSchool()
        {
            try
            {
                var result = _data.GetSchool();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
