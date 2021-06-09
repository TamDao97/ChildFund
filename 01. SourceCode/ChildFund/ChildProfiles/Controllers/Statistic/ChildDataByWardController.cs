using ChildProfiles.Business.Business;
using ChildProfiles.Model;
using ChildProfiles.Model.ChildProfileModels;
using ChildProfiles.Model.Model.Statistic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.Statistic
{
    public class ChildDataByWardController : Controller
    {

        private readonly ChildDataByWardBusiness childDataByWardBusiness = new ChildDataByWardBusiness();

        // GET: ChildDataByWard
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(ChildDataByWardSearchModel modelSearch)
        {
            SearchResultObject<ChildDataByWardModel> list = new SearchResultObject<ChildDataByWardModel>();
            try
            {
                ViewBag.Index = 0;
                modelSearch.UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(modelSearch.UserId);
                modelSearch.Level = userInfo.UserLever;
                ViewBag.UserLever = userInfo.UserLever;
                var currPage = modelSearch.PageNumber - 1;
                list = childDataByWardBusiness.StatisticChildDataByWard(modelSearch);
                ViewBag.Index = (currPage * modelSearch.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = modelSearch.PageSize;
                ViewBag.PathFile = list.PathFile;

                if (list.TotalItem > modelSearch.PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(modelSearch.PageSize, currPage, list.TotalItem, "");
                }
                return PartialView("ChilDataByWardTable", list.ListResult);
            }
            catch (Exception ex)
            {
                return PartialView();
            }
        }
    }
}