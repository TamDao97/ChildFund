using ChildProfiles.Business;
using ChildProfiles.Business.Business;
using ChildProfiles.Common;
using ChildProfiles.Model;
using ChildProfiles.Model.Model.FliesLibrary;
using NTS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChildProfiles.Controllers.FilesLibrary
{
    public class ImageLibraryController : Controller
    {
        private ImageLibraryDA DA = new ImageLibraryDA();
        // GET: ImageLibrary
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewListUpload()
        {
            ViewBag.Title = "Xem danh sách ảnh";
            string id = "";
            if (Request["id"] != null)
            {
                id = Request["id"].ToString();
                ShareImageModel model = DA.GetInfoFileAttachImage(id);
                ViewBag.ContentFolder = string.Format("{0} {1}", model.Content, model.UploadDate.ToString("dd/MM/yyyy"));
                ViewBag.ItemUploadId = model.Id;
                return View(model.Files);
            }
            else
            {
                throw new Exception("Không tồn tại danh mục ảnh trong hệ thống");
            }
        }

        public ActionResult DownloadAllImage(string id)
        {
            try
            {
                return Json(new { ok = true, mess = DA.DownloadAllImage(id) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult DownloadImage(ImageActionModel model)
        {
            try
            {
                return Json(new { ok = true, mess = DA.DownloadImage(model.UserUploadId, model.ImageId) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteImage(ImageActionModel model)
        {
            try
            {
                DA.DeleteImage(model);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ListUploadImage(UploadImageSearchModel searchModel)
        {
            try
            {
                var currPage = searchModel.PageNumber - 1;
                SearchResultObject<ShareImageModel> list = DA.GetListUpload(searchModel);
                ViewBag.Index = (currPage * searchModel.PageSize);
                ViewBag.TotalItem = list.TotalItem;
                ViewBag.PageSize = searchModel.PageSize;
                if (list.TotalItem > searchModel.PageSize)
                {
                    ViewBag.pages = NTS.Common.Utils.Common.PhanTrang(searchModel.PageSize, currPage, list.TotalItem, "");
                }
                return PartialView(list.ListResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteItemUpload(string id)
        {
            try
            {
                DA.DeleteItemUpload(id);
                return Json(new { ok = true, mess = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ViewImageChildByYear(int year, string wardId)
        {
            var ward = new ComboboxDA().GetWardById(wardId);
            ViewBag.navName = ward.Name;
            ViewBag.year = year;
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            var userLever = userInfo.UserLever;
            if (userLever.Equals(NTS.Common.Constants.LevelArea) && !string.IsNullOrEmpty(userInfo.DistrictId))
            {
                ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=area'>" + year + "</a></li>";
            }
            else if (userLever.Equals(NTS.Common.Constants.LevelArea) && string.IsNullOrEmpty(userInfo.DistrictId))
            {
                var province = new ComboboxDA().GetDistrictByWardId(wardId);
                ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=province'>" + year + "</a></li>";
                ViewBag.navSub = "<li><a href='/ImageLibrary/ViewImageChildLevelByYear?year=" + year + "&type=area&&areaId=" + province.Id + "'>" + province.Name + "</a></li>";
            }
            else if (userLever.Equals(NTS.Common.Constants.LevelOffice) || userLever.Equals(NTS.Common.Constants.LevelAdmin))
            {
                var dist = new ComboboxDA().GetDistrictByWardId(wardId);
                var pro = new ComboboxDA().GetProvinceById(dist.ProvinceId);

                ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=office'>" + year + "</a></li>";
                ViewBag.navSub = "<li><a href='/ImageLibrary/ViewImageChildLevelByYear?year=" + year + "&type=province&&areaId=" + pro.Id + "'>" + pro.Name + "</a></li>" + "<li><a href='/ImageLibrary/ViewImageChildLevelByYear?year=" + year + "&type=area&&areaId=" + dist.Id + "'>" + year + "-" + dist.Name + "</a></li>";

            }
            var list = new ChildProfileBusiness().GetImageChildByYear(userId, year, wardId);
            return View(list);
        }
        public ActionResult ViewImageChildLevelByYear(int year, string type, string areaId)
        {
            ViewBag.year = year;
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            var userLever = userInfo.UserLever;
            var list = new ChildProfileBusiness().ViewImageChildLevelByYear(year, type, areaId);
            List<ImageChildByYearLevelView> listImg = new List<ImageChildByYearLevelView>();
            if (type.Equals("province"))
            {
                var province = new ComboboxDA().GetProvinceById(areaId);
                ViewBag.navName = province.Name;
                //gen nav
                if (userLever.Equals(NTS.Common.Constants.LevelArea) && !string.IsNullOrEmpty(userInfo.DistrictId))
                {
                    ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=area'>" + year + "</a></li>";
                }
                else if (userLever.Equals(NTS.Common.Constants.LevelArea) && string.IsNullOrEmpty(userInfo.DistrictId))
                {
                    ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=province'>" + province.Name + "</a></li>";
                }
                else
                {
                    ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=office'>" + year + "</a></li>";
                }
                //tỉnh xem nhóm theo huyện
                listImg = list.GroupBy(p => p.DistrictId, (key, g) => new ImageChildByYearLevelView { AreaId = key, List = g.ToList() }).ToList();
                var area = DA.GetListD(listImg.Select(u => u.AreaId).ToList());
                foreach (var item in listImg)
                {
                    item.AreaName = area.FirstOrDefault(u => u.Id.Equals(item.AreaId))?.Name;
                }
            }
            else
            {
                //gen nav
                var province = new ComboboxDA().GetProvinceByDistrictId(areaId);
                var dist = new ComboboxDA().GetDistrictById(areaId);
                ViewBag.navName = dist.Name;
                if (province != null)
                {
                    if (userLever.Equals(NTS.Common.Constants.LevelArea) && string.IsNullOrEmpty(userInfo.DistrictId))
                    {
                        ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=province'>" + year + "</a></li>";
                    }
                    else
                  if (userLever.Equals(NTS.Common.Constants.LevelArea) && !string.IsNullOrEmpty(userInfo.DistrictId))
                    {
                        ViewBag.navSub = "<li><a href='/ImageLibrary/ViewImageChildLevelByYear?year=" + year + "&type=province&areaId=" + dist.Id + "'>" + dist.Name + "</a></li>";
                        ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=area'>" + year + "</a></li>";
                    }

                    else
                    {
                        ViewBag.nav = "<li><a href='/ImageLibrary/ViewImageChildRegionalByYear?year=" + year + "&view=office'>" + year + "</a></li>";
                        ViewBag.navSub = "<li><a href='/ImageLibrary/ViewImageChildLevelByYear?year=" + year + "&type=province&areaId=" + province.Id + "'>" + province.Name + "</a></li>";
                    }

                }
                else
                {
                    ViewBag.nav = "#";
                }
                //huyện xem nhóm theo xã
                listImg = list.GroupBy(p => p.WardId, (key, g) => new ImageChildByYearLevelView { AreaId = key, List = g.ToList() }).ToList();
                var area = DA.GetListW(listImg.Select(u => u.AreaId).ToList());
                foreach (var item in listImg)
                {
                    item.AreaName = area.FirstOrDefault(u => u.Id.Equals(item.AreaId))?.Name;
                }
            }
            ViewBag.type = type;
            return View(listImg);
        }
        public ActionResult ViewImageChildRegionalByYear(int year, string view)
        {
            ViewBag.year = year;
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            var list = new ChildProfileBusiness().GetImageChildByYear(userId, year, "");
            List<ImageChildByYearLevelView> listImg = new List<ImageChildByYearLevelView>();
            if (view.Equals("province"))
            {//nhóm theo tỉnh
                listImg = list.GroupBy(p => p.DistrictId, (key, g) => new ImageChildByYearLevelView { AreaId = key, List = g.ToList() }).ToList();
                var area = DA.GetListD(listImg.Select(u => u.AreaId).ToList());
                foreach (var item in listImg)
                {
                    item.AreaName = area.FirstOrDefault(u => u.Id.Equals(item.AreaId))?.Name;
                }
            }
            else if (view.Equals("area"))
            {//nhóm theo huyện
                listImg = list.GroupBy(p => p.WardId, (key, g) => new ImageChildByYearLevelView { AreaId = key, List = g.ToList() }).ToList();
                var area = DA.GetListW(listImg.Select(u => u.AreaId).ToList());
                foreach (var item in listImg)
                {
                    item.AreaName = area.FirstOrDefault(u => u.Id.Equals(item.AreaId))?.Name;
                }
            }
            else
            {//admin
                listImg = list.GroupBy(p => p.ProvinceId, (key, g) => new ImageChildByYearLevelView { AreaId = key, List = g.ToList() }).ToList();
                var area = DA.GetListP(listImg.Select(u => u.AreaId).ToList());
                foreach (var item in listImg)
                {
                    item.AreaName = area.FirstOrDefault(u => u.Id.Equals(item.AreaId))?.Name;
                }
            }
            ViewBag.view = view;
            return View(listImg);
        }
        public ActionResult ViewImageChild()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            ViewBag.UserLever = userInfo.UserLever;
            ViewBag.WardId = userInfo.WardId;
            ViewBag.AreaDistrictId = userInfo.DistrictId;

            var list = new ChildProfileBusiness().GetImageChild(userId);
            var results = list.GroupBy(p => p.Year, (key, g) => new ImageChildView { Year = key, List = g.ToList() }).ToList();
            return View(results);
        }


    }
}