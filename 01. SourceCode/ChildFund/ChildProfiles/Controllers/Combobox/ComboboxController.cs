using ChildProfiles.Business.Business;
using ChildProfiles.Common;
using ChildProfiles.Controllers.Base;
using ChildProfiles.Model;
using ChildProfiles.Model.AreaUser;
using ChildProfiles.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.Combobox
{
    public class ComboboxController : BaseController
    {
        ComboboxDA _data = new ComboboxDA();
        /// <summary>
        /// tôn giáo
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [OutputCache(Duration = 864000)]
        public ActionResult ReligionCBB()
        {
            return PartialView(_data.GetGeligionCBB());
        }
        [ChildActionOnly]
        [OutputCache(Duration = 864000)]
        public ActionResult JobCBB()
        {
            return PartialView(_data.GetJobCBB());
        }
        /// <summary>
        /// dân tộc
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [OutputCache(Duration = 864000)]
        public ActionResult NationCBB()
        {
            return PartialView(_data.GetNationCBB());
        }

        public ActionResult AreaUserCBB()
        {
            return PartialView(_data.GetAreaUserCBB());
        }
        [ChildActionOnly]
        [OutputCache(Duration = 864000)]
        public ActionResult ProvinceCBB()
        {
            return PartialView(_data.GetProvinceCBB());
        }

        public ActionResult ImageCreateByCBB()
        {
            return PartialView(_data.ImageCreateByCBB());
        }

        public ActionResult DistrictCBB(string Id)
        {
            return PartialView(_data.GetDistrictCBB(Id));
        }

        public ActionResult WardCBB(string Id)
        {
            return PartialView(_data.GetWardCBB(Id));
        }
        [ChildActionOnly]
        [OutputCache(Duration = 864000)]
        public ActionResult RelationshipCBB()
        {
            return PartialView(_data.RelationshipCBB());
        }

        public ActionResult AreaUser()
        {
            return PartialView(_data.GetAllAreaUser());
        }

        public ActionResult AreaDistrict(string Id)
        {
            return PartialView(_data.GetAreaDistrict(Id));
        }

        public ActionResult AreaWard(string Id)
        {
            return PartialView(_data.GetAreaWard(Id));
        }

        public ActionResult DepartmentCBB()
        {
            return PartialView(_data.GetAllDepartment());
        }

        public ActionResult ProvinceByUser()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            return PartialView(_data.GetProvinceByUser(userInfo.AreaUserId));
        }
        public ActionResult DistrictByUser(string Id)
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            return PartialView(_data.GetDistrictByUser(Id, userInfo.DistrictId));
        }

        public ActionResult WardByUser(string Id)
        {
            return PartialView(_data.GetWardByUser(Id));
        }

        public ActionResult DistrictArea(AreaUserModel model)
        {
            if (model.ListDistrict == null)
            {
                model.ListDistrict = new List<string>();
            }
            ViewBag.ListDistrict = model.ListDistrict;
            return PartialView(_data.GetDistrictCBB(model.ProvinceId));
        }
        public ActionResult WardArea(AreaUserModel model)
        {
            if (model.ListWard == null)
            {
                model.ListWard = new List<string>();
            }
            ViewBag.ListWard = model.ListWard;
            return PartialView(_data.GetWardCBB(model.DistrictId));
        }

        [HttpPost]
        public JsonResult GetArea()
        {
            return Json(_data.GetAllAreaUser());
        }

        [HttpPost]
        public JsonResult GetDistrictByAreaId(String Id)
        {
            return Json(_data.GetAreaDistrict(Id));
        }

        //địa ban trang chủ
        public ActionResult GetAllAreaHome()
        {
            return PartialView(_data.GetAllAreaHome());
        }

        public ActionResult GetAreaDistrictHome(string Id)
        {
            return PartialView(_data.GetAreaDistrictHome(Id));
        }

        //danh sách survey
        public ActionResult GetAllSurvey()
        {
            return PartialView(_data.GetAllSurvey());
        }

        /// <summary>
        /// Danh sách Thon xom theo id xa
        /// </summary>
        public ActionResult GetVillageByWrad(string id)
        {
            return PartialView(_data.GetVillageByWrad(id));
        }

        /// <summary>
        /// Danh sách Thon xom theo id xa
        /// </summary>
        public ActionResult GetListVillage(string id)
        {
            return PartialView(_data.GetVillageByWrad(id));
        }

        public ActionResult DocumentTyeCBB()
        {
            return PartialView(_data.GetDocumentTyeCBB());
        }

        public ActionResult SchoolCBB(string id)
        {
            return PartialView(_data.GetSchoolWard(id));
        }

        public JsonResult GetListProvinceById()
        {
            return Json(_data.GetListProvinceById());
        }

        public JsonResult GetListDistrictByProviceId(string id)
        {
            return Json(_data.GetListDistrictByProviceId(id));
        }

        public JsonResult GetListWardByDistrictId(string id)
        {
            return Json(_data.GetListWardByDistrictId(id));
        }
    }
}