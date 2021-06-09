using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.Model.SystemConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.Config
{
    public class SystemConfigController : Controller
    {
        private readonly SystemConfigBusiness systemConfigBusiness = new SystemConfigBusiness();
        private ComboboxDA comboboxDA = new ComboboxDA();

        // GET: Config
        public ActionResult ConfigSchool()
        {
            return View();
        }

        public PartialViewResult GetListSchool(SchoolSearchCondition searchModel)
        {
            SearchResultObject<SchoolModel> result = new SearchResultObject<SchoolModel>();
            result = systemConfigBusiness.GetListSchool(searchModel);

            int currPage = searchModel.PageNumber - 1;
            ViewBag.totalItem = result.TotalItem;
            ViewBag.PageSize = searchModel.PageSize;

            if (result.TotalItem > searchModel.PageSize)
            {
                ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(searchModel.PageSize, currPage, result.TotalItem, "");
            }

            return PartialView(result.ListResult);
        }

        public bool Save(SchoolModel model)
        {
            bool isSuccess = false;

            isSuccess = systemConfigBusiness.Save(model);
            return isSuccess;
        }

        public PartialViewResult ShowCreateForm()
        {
            SchoolModel model = new SchoolModel();
            string provinceId, districtId;
            provinceId = comboboxDA.GetListProvinceById().FirstOrDefault()?.Id;
            ViewBag.listProvince = comboboxDA.GetListProvinceById();
            ViewBag.listDistrict = comboboxDA.GetListDistrictByProviceId(provinceId);
            districtId = comboboxDA.GetListDistrictByProviceId(provinceId).FirstOrDefault()?.Id;
            ViewBag.listWard = comboboxDA.GetListWardByDistrictId(districtId);

            return PartialView("_CreateSchool", model);
        }

        public PartialViewResult ShowEditForm(string id)
        {
            SchoolModel model = systemConfigBusiness.GetDetailSchool(id);
            ViewBag.listProvince = comboboxDA.GetListProvinceById();
            ViewBag.listDistrict = comboboxDA.GetListDistrictByProviceId(model.ProvinceId);
            ViewBag.listWard = comboboxDA.GetListWardByDistrictId(model.DistrictId);

            return PartialView("_CreateSchool", model);
        }

        public bool Delete(string Id)
        {
            return systemConfigBusiness.Delete(Id);
        }
    }
}