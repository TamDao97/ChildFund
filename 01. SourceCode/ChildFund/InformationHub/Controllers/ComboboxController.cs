using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Common;
using InformationHub.Model;
using InformationHub.Model.AreaUser;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationHub.Controllers.Combobox
{
    public class ComboboxController : BaseController
    {
        ComboboxBusiness _data = new ComboboxBusiness();

        public ActionResult ProvinceCBB()
        {
            return PartialView(_data.GetProvinceCBB());
        }
        public ActionResult DistrictCBB(string Id)
        {
            return PartialView(_data.GetDistrictCBB(Id));
        }
        public ActionResult WardCBB(string Id)
        {
            return PartialView(_data.GetWardCBB(Id));
        }
        public ActionResult GroupUserCBB(int type)
        {
            return PartialView(_data.GetGroupUserCBB(type));
        }
        public ActionResult GetAllAbuseTypeUpdate(List<ComboboxResult> list)
        {
            ViewBag.list = list;
            return PartialView(_data.GetAllAbuseType());
        }
    
        public ActionResult GetAllAbuseType(string type)
        {
            ViewBag.type = type;
            return PartialView(_data.GetAllAbuseType());
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
        public ActionResult UserAreaCBB()
        {
            return PartialView(_data.GetUserAreaCBB());
        }
        public ActionResult DistrictAreaCBB(string id)
        {
            return PartialView(_data.GetDistrictAreaCBB(id));
        }
        public ActionResult WardAreaCBB(string id)
        {
            return PartialView(_data.GetWardAreaCBB(id));
        }
        public ActionResult DocumentTyeCBB()
        {
            return PartialView(_data.GetDocumentTyeCBB());
        }

        public ActionResult WardByUserId()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            return PartialView(_data.WardByUserId(userId));
        }
        public ActionResult DistrictByWardId(string wardId)
        {
            return PartialView(_data.DistrictByWardId(wardId));
        }
        public ActionResult ProvinceByWardId(string wardId)
        {
            return PartialView(_data.ProvinceByWardId(wardId));
        }

        public ActionResult GetAddressByUser()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.userInfo = userInfo;
            var listProvince = _data.GetProvinceCBB();
            List<ComboboxResult> listDistrict = new List<ComboboxResult>();
            List<ComboboxResult> listWard = new List<ComboboxResult>();
            if (userInfo.Type == Constants.LevelAdmin )
            {
                listDistrict = new List<ComboboxResult>();
                listWard = new List<ComboboxResult>();
            }
            else if (userInfo.Type == Constants.LevelOffice)
            {
                listDistrict = _data.GetDistrictCBB(userInfo.ProvinceId); ;
            }
            else if (userInfo.Type == Constants.LevelArea)
            {
                listDistrict = _data.GetDistrictCBB(userInfo.ProvinceId); ;
                listWard = _data.GetWardCBB(userInfo.DistrictId);
            }
            else
            {
                listDistrict = _data.GetDistrictCBB(userInfo.ProvinceId); ;
                listWard = _data.GetWardCBB(userInfo.DistrictId); 
            }
            ViewBag.listDistrict = listDistrict;
            ViewBag.listWard = listWard;
            return PartialView(listProvince);
        }


        //tìm kiếm theo dk truyền sang của ds sự vụ
        public ActionResult GetWardByWardId(string wardId)
        {
            return PartialView(_data.GetWardByWardId(wardId));
        }
        public ActionResult GetDistrictByWardId(string wardId)
        {
            return PartialView(_data.GetDistrictByWardId(wardId));
        }

        public ActionResult GetDistrictByDistrictId(string districtId)
        {
            return PartialView(_data.GetDistrictByDistrictId(districtId));
        }
    }
}