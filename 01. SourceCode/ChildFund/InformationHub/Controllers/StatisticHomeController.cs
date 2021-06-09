using InformationHub.Business;
using InformationHub.Business.Business;
using InformationHub.Model;
using InformationHub.Model.Repositories;
using InformationHub.Model.StatisticModels;
using NTS.Common;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace InformationHub.Controllers
{
    public class StatisticHomeController : BaseController
    {
        StatisticHomeBusiness _business = new StatisticHomeBusiness();
        ComboboxBusiness _combb = new ComboboxBusiness();
        [MyAuthorize]
        public ActionResult HomeProvince()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            int  thisYear = DateTime.Now.Year;

            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            if (userInfo == null || userInfo.Type == Constants.LevelTeacher)
            {
                Response.Redirect("/Authorize/Login");
            }
            #region[dữ liệu ban đầu]
            var abuseType = _combb.GetAllAbuseType();
            var thisYeardata = _business.SearchHomeProvince(userInfo, thisYear);
            var lastYearData = _business.SearchHomeProvince(userInfo, thisYear - 1);

            List<string> listLable = new List<string>();
            for (int i = 1; i <= 12; i++)
            {
                listLable.Add("T" + i);
            }
            ViewBag.abuseType = abuseType;
            ViewBag.type = userInfo.Type;
            ViewBag.listLable = string.Join(";", listLable);
            ViewBag.year = thisYear;
            #endregion
            #region[bảng bên trái]
            List<HomeProvinceLeftModel> lstHomeProvinceLeftModel = new List<HomeProvinceLeftModel>();
            HomeProvinceLeftModel itemHomeProvinceLeftModel;
            if (userInfo.Type == Constants.LevelAdmin)
            {
                var listProvinceId = thisYeardata.Select(u => u.ProvinceId).ToList();
                var listProvince = new ComboboxBusiness().GetProvinceByListId(listProvinceId);
                foreach (var item in listProvince)
                {
                    itemHomeProvinceLeftModel = new HomeProvinceLeftModel();
                    itemHomeProvinceLeftModel.Name = item.Name;
                    itemHomeProvinceLeftModel.CountAll = thisYeardata.Where(u => u.ProvinceId.Equals(item.Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse1 = thisYeardata.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[0].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse2 = thisYeardata.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[1].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse3 = thisYeardata.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[2].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse4 = thisYeardata.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[3].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse5 = thisYeardata.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[4].Id)).Count();
                    lstHomeProvinceLeftModel.Add(itemHomeProvinceLeftModel);
                }
            }            
            else
            {
                var listWardId = thisYeardata.Select(u => u.WardId).ToList();
                var listWard = new ComboboxBusiness().GetWardByListId(listWardId, userInfo.DistrictId);
                foreach (var item in listWard)
                {
                    itemHomeProvinceLeftModel = new HomeProvinceLeftModel();
                    itemHomeProvinceLeftModel.Name = item.Name;
                    itemHomeProvinceLeftModel.CountAll = thisYeardata.Where(u => u.WardId.Equals(item.Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse1 = thisYeardata.Where(u => u.WardId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[0].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse2 = thisYeardata.Where(u => u.WardId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[1].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse3 = thisYeardata.Where(u => u.WardId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[2].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse4 = thisYeardata.Where(u => u.WardId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[3].Id)).Count();
                    itemHomeProvinceLeftModel.CountByAbuse5 = thisYeardata.Where(u => u.WardId.Equals(item.Id) && u.AbuseIds.Contains(abuseType[4].Id)).Count();
                    lstHomeProvinceLeftModel.Add(itemHomeProvinceLeftModel);
                }
            }
            ViewBag.lstHomeProvinceLeftModel = lstHomeProvinceLeftModel;
            #endregion
            #region[thống kê bên trên]
            List<HomeProvinceItemModel> listCount = new List<HomeProvinceItemModel>();
            int counAll = thisYeardata.Count;
            int totalThisYear = 0;
            int totalLastYear = 0;
            double PercenChart = 0;
            foreach (var item in abuseType)
            {
                totalThisYear = thisYeardata.Where(u => u.AbuseIds.Contains(item.Id)).Count();
                totalLastYear = lastYearData.Where(u => u.AbuseIds.Contains(item.Id)).Count();
                if (totalLastYear != 0)
                {
                    PercenChart = Math.Round((double)(totalThisYear * 100) / totalLastYear, 0);
                }else
                {
                    PercenChart = 0;
                }

                listCount.Add(new HomeProvinceItemModel { LableName = item.Name, AbuseId = item.Id, Count = totalThisYear, PercenChart = PercenChart, CountBefore = totalLastYear, Percen = PercenChart, });

            }
            ViewBag.listCount = listCount;
            #endregion

            #region[biểu đồ phải]
            List<HomeChartItemModelByQuatar> listChartCount = new List<HomeChartItemModelByQuatar>();
            listChartCount.Add(new HomeChartItemModelByQuatar { LableName = Resource.Resource.Report_AddNew_Title });
            listChartCount.Add(new HomeChartItemModelByQuatar { LableName = Resource.Resource.Report_Closed_Title });

            listChartCount[0].ListCount = new List<int>();
            listChartCount[1].ListCount = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                int openCases = thisYeardata.Where(u => u.ReceptionDate.Month == i).Count();
                int closedCases = thisYeardata.Where(u => u.StatusStep6 == true && u.FinishDate.HasValue && u.FinishDate.Value.Month == i).Count();
                listChartCount[0].ListCount.Add(openCases);
                listChartCount[1].ListCount.Add(closedCases);
            }

            listChartCount[0].ListStr = string.Join(";", listChartCount[0].ListCount);
            listChartCount[1].ListStr = string.Join(";", listChartCount[1].ListCount);

            #endregion
            return View(listChartCount);
        }
        public ActionResult GetChartRightByProvinceId(string provinceId)
        {
            try
            {
                var userId = System.Web.HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);

                List<string> listLable = new List<string>();
                for (int i = 1; i <= 12; i++)
                {
                    listLable.Add("T" + i);
                }
                ViewBag.type = userInfo.Type;
                ViewBag.listLable = string.Join(";", listLable);
                var dateNow = DateTime.Now;

                var abuseType = _combb.GetAllAbuseType();
                ViewBag.year = dateNow.Year;
                var data = _business.SearchHomeProvince(userInfo, dateNow.Year);
                if (!string.IsNullOrEmpty(provinceId))
                {
                    data = data.Where(u => u.ProvinceId.Equals(provinceId)).ToList();
                }
                var result = data.GroupBy(test => test.Id)
                       .Select(grp => grp.First())
                       .ToList();
                List<HomeProvinceModel> listByStatusResult;
                List<HomeChartItemModelByQuatar> listChartCount = new List<HomeChartItemModelByQuatar>();
                // listChartCount.Add(new HomeChartItemModelByQuatar { ProcessingStatus = 0, LableName = Resource.Resource.ReportProfile_Status0 });
                listChartCount.Add(new HomeChartItemModelByQuatar { LableName = Resource.Resource.ReportProfile_Status4 });

                listChartCount[0].ListCount = new List<int>();
                for (int i = 1; i <= 12; i++)
                {
                    listChartCount[0].ListCount.Add(result.Where(u => u.ReceptionDate.Month == i).Count());
                }
                listChartCount[0].ListStr = string.Join(";", listChartCount[0].ListCount);

                listChartCount[1].ListCount = new List<int>();
                listByStatusResult = result.Where(u => u.StatusStep6 == true).ToList();
                for (int i = 1; i <= 12; i++)
                {
                    listChartCount[1].ListCount.Add(listByStatusResult.Where(u => u.ReceptionDate.Month == i).Count());
                }
                listChartCount[1].ListStr = string.Join(";", listChartCount[1].ListCount);

                return Json(new { ok = true, listLable = listLable, listChartCount = listChartCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [MyAuthorize]
        public ActionResult HomeWard()
        {
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            if (userInfo == null || userInfo.Type != Constants.LevelTeacher)
            {
                Response.Redirect("/Authorize/Login");
            }
            var data = _business.HomeWard(userInfo);
            return View(data);
        }
        [MyAuthorize]
        public ActionResult GetHomeWard(HomeWardModel model)
        {
            if (!string.IsNullOrEmpty(model.SDateTo))
            {
                model.DateTo = DateTimeUtils.ConvertDateToStr(model.SDateTo);
            }
            if (!string.IsNullOrEmpty(model.SDateFrom))
            {
                model.DateFrom = DateTimeUtils.ConvertDateFromStr(model.SDateFrom);
            }
            var dateNow = DateTime.Now;
            var userId = System.Web.HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);

            ViewBag.ListReportHistory = _business.GetReportHistory(userInfo, model);
            return PartialView();
        }
        [MyAuthorize(Roles = "C0034")]
        public ActionResult ReportWard()
        {
            return View();
        }
        [MyAuthorize(Roles = "C0034")]
        public ActionResult GetReportWard(ReportWardModel model)
        {
            try
            {
                if (model.FromYear == null)
                {
                    var dateNow = DateTime.Now;
                    model.ToYear = dateNow.Year;
                    model.FromYear = dateNow.AddYears(-10).Year;
                }
                model.UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var data = _business.GetReportWard(model);

                List<int> listYear = new List<int>();
                for (int i = model.FromYear.Value; i <= model.ToYear; i++)
                {
                    int year = i;
                    listYear.Add(year);
                };
                listYear = listYear.OrderBy(i => i).ToList();
                var abuseType = _combb.GetAllAbuseType().OrderBy(i => i.Id);
                List<TableReportWardModel> tableData = new List<TableReportWardModel>();
                int counAll = data.Count();
                for (int i = model.FromYear.Value; i <= model.ToYear; i++)
                {
                    TableReportWardModel itemTableData = new TableReportWardModel();
                    //itemTableData.Count1 = data.Where(u => u.AbuseId.Equals("AT01") && u.ReceptionDate.Year == i).Count();
                    //itemTableData.Count2 = data.Where(u => u.AbuseId.Equals("AT02") && u.ReceptionDate.Year == i).Count();
                    //itemTableData.Count3 = data.Where(u => u.AbuseId.Equals("AT03") && u.ReceptionDate.Year == i).Count();
                    //itemTableData.Count4 = data.Where(u => u.AbuseId.Equals("AT04") && u.ReceptionDate.Year == i).Count();
                    //itemTableData.Count5 = data.Where(u => u.AbuseId.Equals("AT05") && u.ReceptionDate.Year == i).Count();
                    itemTableData.Percent1 = counAll != 0 ? (itemTableData.Count1 * 100 / counAll) : 100;
                    itemTableData.Percent2 = counAll != 0 ? (itemTableData.Count2 * 100 / counAll) : 100;
                    itemTableData.Percent3 = counAll != 0 ? (itemTableData.Count3 * 100 / counAll) : 100;
                    itemTableData.Percent4 = counAll != 0 ? (itemTableData.Count4 * 100 / counAll) : 100;
                    itemTableData.Percent5 = counAll != 0 ? (itemTableData.Count5 * 100 / counAll) : 100;
                    itemTableData.Year = i;
                    itemTableData.CountAll = data.Where(u => u.ReceptionDate.Year == i).GroupBy(u => u.Id).Count();
                    tableData.Add(itemTableData);
                }

                List<ChartReportWardModel> chartData = new List<ChartReportWardModel>();
                var chartCount = 0;
                foreach (var item in abuseType)
                {
                    ChartReportWardModel itemChartData = new ChartReportWardModel();
                    List<int> countList = new List<int>();
                    for (int i = model.FromYear.Value; i <= model.ToYear; i++)
                    {
                       // chartCount = data.Where(u => u.AbuseId.Equals(item.Id) && u.ReceptionDate.Year == i).Count();
                        countList.Add(chartCount);
                    }
                    itemChartData.AbuseType = item.Name;
                    itemChartData.AbuseId = item.Id;
                    itemChartData.Count = countList;
                    chartData.Add(itemChartData);
                }

                var PathFile = "";
                if (model.Export != 0)
                {
                    PathFile = _business.ExportReportFile(tableData, model); ;
                }
                else
                {
                    PathFile = "";
                }
                return Json(new { ok = true, listYear = listYear, tabledata = tableData, PathFile = PathFile, chartData = chartData, Chart_Title = Resource.Resource.Chart_Title }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReportByYear()
        {
            return View();
        }

        public ActionResult GetReportByYear(ReportWardModel model)
        {
            try
            {
                if (model.FromYear == null)
                {
                    var dateNow = DateTime.Now;
                    model.ToYear = dateNow.Year;
                    model.FromYear = dateNow.AddYears(-10).Year;
                }
                model.UserId = System.Web.HttpContext.Current.User.Identity.Name;
                var data = _business.GetReportWard(model);

                List<int> listYear = new List<int>();
                for (int i = model.FromYear.Value; i <= model.ToYear; i++)
                {
                    int year = i;
                    listYear.Add(year);
                };
                listYear = listYear.OrderBy(i => i).ToList();
                //  var abuseType = _combb.GetAllAbuseType().OrderBy(i => i.Id);
                List<TableReportWardModel> tableData = new List<TableReportWardModel>();
                int counAll = data.Count();
                for (int i = model.FromYear.Value; i <= model.ToYear; i++)
                {
                    TableReportWardModel itemTableData = new TableReportWardModel();
                    itemTableData.Year = i;
                    itemTableData.CountAll = data.Where(u => u.ReceptionDate.Year == i).GroupBy(u => u.Id).Count();
                    tableData.Add(itemTableData);
                }

                List<ChartReportWardModel> chartData = new List<ChartReportWardModel>();
                var chartCount = 0;
                //  foreach (var item in abuseType)
                // {
                ChartReportWardModel itemChartData = new ChartReportWardModel();
                List<int> countList = new List<int>();
                for (int i = model.FromYear.Value; i <= model.ToYear; i++)
                {
                    chartCount = data.Where(u => u.ReceptionDate.Year == i).Count();
                    countList.Add(chartCount);
                }
                itemChartData.AbuseType = InformationHub.Resource.Resource.Chart_Title;
                itemChartData.AbuseId = "-1";
                itemChartData.Count = countList;
                chartData.Add(itemChartData);
                //}

                var PathFile = "";
                if (model.Export != 0)
                {
                    PathFile = _business.ExportReportByYear(tableData, model); ;
                }
                else
                {
                    PathFile = "";
                }
                return Json(new { ok = true, listYear = listYear, tabledata = tableData, PathFile = PathFile, chartData = chartData, Chart_Title = Resource.Resource.Chart_Title }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, mess = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}