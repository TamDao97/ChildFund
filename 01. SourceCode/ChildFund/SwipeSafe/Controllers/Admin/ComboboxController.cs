using SwipeSafe.Model.SearchCondition;
using SwipeSafe.Model.SearchResults;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using SwipeSafe.Business;

namespace SwipeSafe.Controllers.Admin
{
    public class ComboboxController : Controller
    {
        ComboboxBusiness _buss = new ComboboxBusiness();
        public ActionResult GetFormsAbuses()
        {
            var list = _buss.GetFormsAbuses();
            return PartialView(list);
        }
        public ActionResult GetProvince()
        {
            var list = _buss.GetProvinceCBB();
            return PartialView(list);
        }
        public ActionResult GetDistrict(string id)
        {
            var list = _buss.GetDistrictCBB(id);
            return PartialView(list);
        }
        public ActionResult GetWard(string id)
        {
            var list = _buss.GetWardCBB(id);
            return PartialView(list);
        }
    }
}