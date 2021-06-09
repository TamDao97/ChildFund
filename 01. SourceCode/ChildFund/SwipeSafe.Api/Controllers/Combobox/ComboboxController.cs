using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NTS.Api.Controllers.Combobox
{
    [RoutePrefix("api/ComboboxCommon")]
    public class ComboboxController : ApiController
    {
        //private readonly ComboboxBusiness _Business = new ComboboxBusiness();

        //[Route("GetListBanking")]
        //[HttpPost]
        //public HttpResponseMessage GetListBanking()
        //{
        //    try
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListGroupModule")]
        //[HttpPost]
        //public HttpResponseMessage GetListGroupModule()
        //{
        //    try
        //    {
        //        var result = _Business.GetListGroupModule();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListParentMaterialGroup")]
        //[HttpPost]
        //public HttpResponseMessage GetListParentMaterialGroup(SwipeSafeMGModel model)
        //{
        //    try
        //    {
        //        var result = _Business.GetListParentMaterialGroup(model);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListMaterialGroup")]
        //[HttpPost]
        //public HttpResponseMessage GetListMaterialGroup()
        //{
        //    try
        //    {
        //        var result = _Business.GetListMaterialGroup();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListManufacture")]
        //[HttpPost]
        //public HttpResponseMessage GetListManufacture()
        //{
        //    try
        //    {
        //        var result = _Business.GetListManufacture();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListDepartment")]
        //[HttpPost]
        //public HttpResponseMessage GetListDepartment()
        //{
        //    try
        //    {
        //        var result = _Business.GetListDepartment();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetGroupModule")]
        //[HttpPost]
        //public HttpResponseMessage GetGroupModule()
        //{
        //    try
        //    {
        //        var result = _Business.GetGroupModule();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListJobPosition")]
        //[HttpPost]
        //public HttpResponseMessage GetListJobPosition()
        //{
        //    try
        //    {
        //        var result = _Business.GetListJobPosition();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListQualification")]
        //[HttpPost]
        //public HttpResponseMessage GetListQualification()
        //{
        //    try
        //    {
        //        var result = _Business.GetListQualification();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListGroupUsers")]
        //[HttpPost]
        //public HttpResponseMessage GetListGroupUsers()
        //{
        //    try
        //    {
        //        var result = _Business.GetListGroupUsers();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[Route("GetListModulesByGroup")]
        //[HttpPost]
        //public HttpResponseMessage GetListModulesByGroup(string groupId)
        //{
        //    try
        //    {
        //        var result = _Business.GetListModulesByGroup(groupId);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[Route("GetListProject")]
        //[HttpPost]
        //public HttpResponseMessage GetListProject()
        //{
        //    try
        //    {
        //        var result = _Business.GetListProject();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GenCodeModuleError")]
        //[HttpPost]
        //public HttpResponseMessage GenCodeModuleError()
        //{
        //    try
        //    {
        //        var result = _Business.GenCodeModuleError();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListModuleErrorTypesParent")]
        //[HttpPost]
        //public HttpResponseMessage GetListModuleErrorTypesParent(int type)
        //{
        //    try
        //    {
        //        var result = _Business.GetListModuleErrorTypesParent(type);
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[Route("GetListManagers")]
        //[HttpPost]
        //public HttpResponseMessage GetListManagers()
        //{
        //    try
        //    {
        //        var result = _Business.GetListManagers();
        //        return Request.CreateResponse(HttpStatusCode.OK, result);
        //    } catch(Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
    }
}
