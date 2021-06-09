using SwipeSafe.Business.StatisticReport;
using SwipeSafe.Model.ProfileReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SwipeSafe.Controllers.API
{
    [RoutePrefix("api/StatisticReport")]
    public class StatisticReportController : ApiController
    {
       private StatisticReportBusiness _buss = new StatisticReportBusiness();

        //báo cáo thống kê cho mobile
        [Route("ReportByCountWard")]
        [HttpPost]
        public HttpResponseMessage ReportByCountWard(ReportByCountWardSearch model)
        {
            try
            {
                var rs = _buss.ReportByCountWard(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAbuseWard")]
        [HttpPost]
        public HttpResponseMessage ReportByAbuseWard(ReportByAbuseWardSearch model)
        {
            try
            {
                var rs = _buss.ReportByAbuseWard(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //cap huyen

        [Route("ReportByAreaDistrict")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaDistrict(ReportByAreaDistrictSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaDistrict(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAbuseAndTypeDistrict")]
        [HttpPost]
        public HttpResponseMessage ReportByAbuseAndTypeDistrict(ReportByAbuseAndTypeDistrictSearch model)
        {
            try
            {
                var rs = _buss.ReportByAbuseAndTypeDistrict(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAreaAndTypeDistrict")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaAndTypeDistrict(ReportByAreaAndTypeDistrictSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaAndTypeDistrict(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAreaAndAgeDistrict")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaAndAgeDistrict(ReportByAbuseAndAgeDistrictSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaAndAgeDistrict(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAreaAndGenderDistrict")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaAndGenderDistrict(ReportByAbuseAndGenderDistrictSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaAndGenderDistrict(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //cap tinh
        [Route("ReportByAreaProvince")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaProvince(ReportByAreaProvinceSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaProvince(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAbuseAndTypeProvince")]
        [HttpPost]
        public HttpResponseMessage ReportByAbuseAndTypeProvince(ReportByAbuseAndTypeProvinceSearch model)
        {
            try
            {
                var rs = _buss.ReportByAbuseAndTypeProvince(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAreaAndTypeProvince")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaAndTypeProvince(ReportByAreaAndTypeProvinceSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaAndTypeProvince(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAreaAndAgeProvince")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaAndAgeProvince(ReportByAbuseAndAgeProvinceSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaAndAgeProvince(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("ReportByAreaAndGenderProvince")]
        [HttpPost]
        public HttpResponseMessage ReportByAreaAndGenderProvince(ReportByAbuseAndGenderProvinceSearch model)
        {
            try
            {
                var rs = _buss.ReportByAreaAndGenderProvince(model);
                return Request.CreateResponse(HttpStatusCode.OK, rs);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
