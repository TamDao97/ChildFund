using InformationHub.Model;
using InformationHub.Model.Repositories;
using InformationHub.Model.SearchResults;
using InformationHub.Model.StatisticModels;
using InformationHub.Model.UserModels;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Utils;
using Syncfusion.ExcelChartToImageConverter;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace InformationHub.Business.Business
{
    public class StatisticBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();
        ComboboxBusiness _combb = new ComboboxBusiness();

        public UserModel FindUser(string userId)
        {
            var user = (from a in db.Users.AsNoTracking()
                        where a.Id.Equals(userId)
                        join b in db.Provinces.AsNoTracking() on a.ProvinceId equals b.Id into ab
                        from ab1 in ab.DefaultIfEmpty()
                        join c in db.Districts.AsNoTracking() on a.DistrictId equals c.Id into ac
                        from ac1 in ac.DefaultIfEmpty()
                        join d in db.Wards.AsNoTracking() on a.WardId equals d.Id into ad
                        from ad1 in ad.DefaultIfEmpty()
                        select new UserModel()
                        {
                            Type = a.Type,
                            DistrictName = ac1.Name,
                            ProvinceName = ab1.Name,
                            DistrictId = ac1.Id,
                            ProvinceId = ab1.Id,
                            WardId = ad1.Id,
                            WardName = ad1.Name
                        }).First();
            return user;
        }

        public ResultStatisticByAge SearchStatisticByAge(StatisticSearchCondition modelSearch)
        {
            ResultStatisticByAge model = new ResultStatisticByAge();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 where a.IsDelete == false
                                 select new StatisticByAgeModel()
                                 {
                                     Id = a.Id,
                                     Age = a.Age,
                                     ReceptionDate = a.ReceptionDate,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     IsPublish = a.IsPublish
                                 }).AsQueryable();
                if (modelSearch != null)
                {
                    listmodel = listmodel.Where(u => u.ReceptionDate.Value.Year <= modelSearch.ToYear && u.ReceptionDate.Value.Year >= modelSearch.FromYear);
                }
                if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(modelSearch.ProvinceId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(modelSearch.DistrictId));
                }
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(modelSearch.WardId));
                }
                if (userInfo.Type!=Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                //
                List<int> listYear = new List<int>();
                for (int i = modelSearch.FromYear; i <= modelSearch.ToYear; i++)
                {
                    int year = i;
                    listYear.Add(year);
                }
                listYear = listYear.OrderByDescending(i => i).ToList();
                //
                List<LoaiHinh> loaihinh = new List<LoaiHinh>();
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age0_3, ValueFrom = -1, ValueTo = 4 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age4_6, ValueFrom = 3, ValueTo = 7 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age7_9, ValueFrom = 6, ValueTo = 10 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age10, ValueFrom = 9, ValueTo = 11 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age11_12, ValueFrom = 10, ValueTo = 13 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age13_14, ValueFrom = 12, ValueTo = 15 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age15_16, ValueFrom = 14, ValueTo = 17 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age16_18, ValueFrom = 16, ValueTo = 19 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.ReportProfile_Unknow, ValueFrom = 0, ValueTo = 0 });
                foreach (var item in loaihinh)
                {
                    item.AgeValue = item.ValueFrom + ";" + item.ValueTo;
                }
                model.AgeValue = loaihinh;
                //
                var listdata = listmodel.ToList();
                double countAll = listdata.Count();
                //
                model.ListStatisticByAgeModel = new List<StatisticByAgeModel>();
                foreach (var item in listYear)
                {
                    var itemModel = new StatisticByAgeModel()
                    {
                        LableName = item,
                        Count1 = listdata.Where(u => u.Age > loaihinh[0].ValueFrom && u.Age < loaihinh[0].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count2 = listdata.Where(u => u.Age > loaihinh[1].ValueFrom && u.Age < loaihinh[1].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count3 = listdata.Where(u => u.Age > loaihinh[2].ValueFrom && u.Age < loaihinh[2].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count4 = listdata.Where(u => u.Age > loaihinh[3].ValueFrom && u.Age < loaihinh[3].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count5 = listdata.Where(u => u.Age > loaihinh[4].ValueFrom && u.Age < loaihinh[4].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count6 = listdata.Where(u => u.Age > loaihinh[5].ValueFrom && u.Age < loaihinh[5].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count7 = listdata.Where(u => u.Age > loaihinh[6].ValueFrom && u.Age < loaihinh[6].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count8 = listdata.Where(u => u.Age > loaihinh[7].ValueFrom && u.Age < loaihinh[7].ValueTo && u.ReceptionDate.Value.Year == item).Count(),
                        Count9 = listdata.Where(u => u.Age == null && u.ReceptionDate.Value.Year == item).Count(),
                    };
                    model.ListStatisticByAgeModel.Add(itemModel);
                }
                //
                List<ChartAgeModel> chartData = new List<ChartAgeModel>();
                for (int i = 0; i < loaihinh.Count; i++)
                {
                    double count = listdata.Where(u => u.Age > loaihinh[i].ValueFrom && u.Age < loaihinh[i].ValueTo).Count();
                    if (i == loaihinh.Count - 1)
                    {
                        count = listdata.Where(u => u.Age == null).Count();
                    }
                    var itemChart = new ChartAgeModel();
                    itemChart.Age = loaihinh[i].LableName;
                    if (countAll != 0)
                    {
                        itemChart.Percent = Math.Round((count * 100 / countAll), 1);
                    };
                    chartData.Add(itemChart);
                }
                model.ChartData = chartData;
                //
                List<ChartAgeModel> chartRightData = new List<ChartAgeModel>();
                for (int i = 0; i < loaihinh.Count; i++)
                {
                    double count = listdata.Where(u => u.Age > loaihinh[i].ValueFrom && u.Age < loaihinh[i].ValueTo && u.ReceptionDate.Value.Year == modelSearch.ClickYear).Count();
                    if (i == loaihinh.Count - 1)
                    {
                        count = listdata.Where(u => u.Age == null && u.ReceptionDate.Value.Year == modelSearch.ClickYear).Count();
                    }
                    var itemChart = new ChartAgeModel();
                    itemChart.Age = loaihinh[i].LableName;
                    if (countAll != 0)
                    {
                        itemChart.Percent = Math.Round((count * 100 / countAll), 1);
                    };
                    chartRightData.Add(itemChart);
                }
                model.ChartRightData = chartRightData;

                if (modelSearch.Export != 0)
                {
                    model.PathFile = ExportExcelByAge(model.ListStatisticByAgeModel, model.ChartData, model.ChartRightData, modelSearch);
                }
                else
                {
                    model.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return model;
        }
        public List<ChartAgeModel> GetRightChart(StatisticSearchCondition modelSearch)
        {
            var userId = HttpContext.Current.User.Identity.Name;
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
            List<ChartAgeModel> model = new List<ChartAgeModel>();
            try
            {
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 where a.IsDelete == false
                                 select new StatisticByAgeModel()
                                 {
                                     Id = a.Id,
                                     Age = a.Age,
                                     ReceptionDate = a.ReceptionDate,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     IsPublish=a.IsPublish
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.ToLower().Contains(modelSearch.WardId.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.ToLower().Contains(modelSearch.DistrictId.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.ToLower().Contains(modelSearch.ProvinceId.ToLower()));
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                //
                List<LoaiHinh> loaihinh = new List<LoaiHinh>();
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age0_3, ValueFrom = -1, ValueTo = 4 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age4_6, ValueFrom = 3, ValueTo = 7 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age7_9, ValueFrom = 6, ValueTo = 10 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age10, ValueFrom = 9, ValueTo = 11 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age11_12, ValueFrom = 10, ValueTo = 13 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age13_14, ValueFrom = 12, ValueTo = 15 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age15_16, ValueFrom = 14, ValueTo = 17 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.Statistic_Age16_18, ValueFrom = 16, ValueTo = 19 });
                loaihinh.Add(new LoaiHinh { LableName = Resource.Resource.ReportProfile_Unknow, ValueFrom = 0, ValueTo = 0 });
                //
                var listdata = listmodel.Where(i => i.ReceptionDate.Value.Year == modelSearch.ClickYear).ToList();
                var countAll = listdata.GroupBy(i => i.Id).Count();
                //
                for (int i = 0; i < loaihinh.Count; i++)
                {
                    double count = listdata.Where(u => u.Age > loaihinh[i].ValueFrom && u.Age < loaihinh[i].ValueTo).Count();
                    if (i == loaihinh.Count - 1)
                    {
                        count = listdata.Where(u => u.Age == null && u.ReceptionDate.Value.Year == modelSearch.ClickYear).Count();
                    }
                    var itemChart = new ChartAgeModel();
                    itemChart.Age = loaihinh[i].LableName;
                    if (countAll != 0)
                    {
                        itemChart.Percent = Math.Round((count * 100 / countAll), 1);
                    };
                    model.Add(itemChart);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return model;
        }

        public GenderResultModel SearchStatisticByGender(StatisticSearchCondition modelSearch)
        {
            GenderResultModel searchResult = new GenderResultModel();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                int countTemp = 0;
                var listmodel = (from a in db.ReportProfiles.AsNoTracking()
                                 join b in db.ReportProfileAbuseTypes.AsNoTracking()
                                 on a.Id equals b.ReportProfileId
                                 where a.IsDelete == false
                                 select new StatisticByGenderModel()
                                 {
                                     Type = b.AbuseTypeName,
                                     TypeId = b.AbuseTypeId,
                                     ReceptionDate = a.ReceptionDate,
                                     WardId = a.WardId,
                                     DistrictId = a.DistrictId,
                                     ProvinceId = a.ProvinceId,
                                     Gender = a.Gender,
                                     IsPublish = a.IsPublish
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(modelSearch.DateFrom);
                    listmodel = listmodel.Where(r => r.ReceptionDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(modelSearch.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(modelSearch.DateTo);
                    listmodel = listmodel.Where(r => r.ReceptionDate <= dateTo);
                }
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(modelSearch.WardId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(modelSearch.DistrictId));
                }
                if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(modelSearch.ProvinceId));
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                var list = listmodel.ToList();
                // table
                List<StatisticByGenderModel> lstTable = new List<StatisticByGenderModel>();
                StatisticByGenderModel itemTable;
                // chart
                List<GenderChartModel> lstChart = new List<GenderChartModel>();
                GenderChartModel itemChart;
                //abusetype
                List<string> lstAbuse = new List<string>();

                var abuSeAll = new ComboboxBusiness().GetAllAbuseType();
                foreach (var item in abuSeAll)
                {
                    //table
                    itemTable = new StatisticByGenderModel();
                    itemTable.Type = item.Name;
                    itemTable.TypeId = item.Id;
                    itemTable.CountNam = list.Where(u => u.Gender == Constants.Male && item.Name.Equals(u.Type)).Count();
                    itemTable.CountNu = list.Where(u => u.Gender == Constants.FeMale && item.Name.Equals(u.Type)).Count();
                    itemTable.CountKhong = list.Where(u => u.Gender == Constants.UnMale && item.Name.Equals(u.Type)).Count();
                    lstTable.Add(itemTable);
                    //abusetype
                    lstAbuse.Add(item.Name);
                }
                // gender
                List<ListGender> lstGender = new List<ListGender>();
                lstGender.Add(new ListGender { Gender = 1, GenderType = Resource.Resource.ReportProfile_Male });
                lstGender.Add(new ListGender { Gender = 2, GenderType = Resource.Resource.ReportProfile_FeMale });
                lstGender.Add(new ListGender { Gender = 0, GenderType = Resource.Resource.Unknow_Gender });
                foreach (var item in lstGender)
                {
                    itemChart = new GenderChartModel();
                    itemChart.Label = item.GenderType;
                    itemChart.Count = new List<int>();
                    foreach (var itemChild in abuSeAll)
                    {
                        countTemp = list.Where(u => u.Gender == item.Gender && u.TypeId.Equals(itemChild.Id)).Count();
                        itemChart.Count.Add(countTemp);
                    }
                    lstChart.Add(itemChart);
                }
                searchResult.LstTable = lstTable;
                searchResult.LstChart = lstChart;
                searchResult.LstAbuse = lstAbuse;
                //excel
                if (modelSearch.Export != 0)
                {
                    searchResult.PathFile = ExportExcelByGender(searchResult.LstTable, modelSearch);
                }
                else
                {
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public ReturnModel SearchStatisticByArea(StatisticSearchCondition modelSearch, string type)
        {
            ReturnModel searchResult = new ReturnModel();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var reportProfiles = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                      join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                      where b.IsDelete == false
                                      select new StatisticByAreaModel()
                                      {
                                          Id = b.Id,
                                          AbuseName = a.AbuseTypeName,
                                          AbuseId = a.AbuseTypeId,
                                          ReceptionDate = b.ReceptionDate,
                                          WardId = b.WardId,
                                          DistrictId = b.DistrictId,
                                          ProvinceId = b.ProvinceId,
                                          IsPublish = b.IsPublish
                                      }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(modelSearch.DateFrom);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(modelSearch.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(modelSearch.DateTo);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate <= dateTo);
                }
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    reportProfiles = reportProfiles.Where(r => r.WardId.ToLower().Contains(modelSearch.WardId.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    reportProfiles = reportProfiles.Where(r => r.DistrictId.ToLower().Contains(modelSearch.DistrictId.ToLower()));
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    reportProfiles = reportProfiles.Where(u => u.IsPublish == true);
                }
                var list = reportProfiles.ToList();
                //area
                List<string> lstArea = new List<string>();
                // chart
                List<AreaChartModel> lstChart = new List<AreaChartModel>();
                AreaChartModel itemChart;
                //table
                List<StatisticByAreaModel> lstTable = new List<StatisticByAreaModel>();
                //abusetype
                var abuSeAll = new ComboboxBusiness().GetAllAbuseType();
                if (type == "1")
                {
                    var districts = db.Districts.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId)).OrderBy(i => i.Name).ToList();
                    var districtsName = districts.Select(i => i.Name).ToList();
                    lstArea.AddRange(districtsName);
                    var reportByDistrict = list.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId));
                    foreach (var district in districts)
                    {
                        var model = new StatisticByAreaModel();
                        model.AreaId = district.Id;
                        model.Name = district.Name;
                        model.Count1 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse1 && district.Id.Equals(u.DistrictId)).Count();
                        model.Count2 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse2 && district.Id.Equals(u.DistrictId)).Count();
                        model.Count3 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse3 && district.Id.Equals(u.DistrictId)).Count();
                        model.Count4 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse4 && district.Id.Equals(u.DistrictId)).Count();
                        model.Total = list.Where(u => u.DistrictId.Equals(district.Id)).GroupBy(u => u.Id).Count();
                        lstTable.Add(model);
                    }

                    foreach (var item in abuSeAll)
                    {
                        itemChart = new AreaChartModel();
                        itemChart.Count = new List<int>();
                        itemChart.Lable = item.Name;
                        foreach (var itemChild in districts)
                        {
                            itemChart.Count.Add(reportByDistrict.Where(u => u.AbuseId.Equals(item.Id) && u.DistrictId.Equals(itemChild.Id)).Count());
                        }
                        lstChart.Add(itemChart);
                    }
                    searchResult.LstChart = lstChart;
                    searchResult.LstTable = lstTable;
                    searchResult.LstArea = lstArea;
                }
                if (type == "2")
                {
                    var wards = db.Wards.Where(u => u.DistrictId.Equals(modelSearch.DistrictId)).OrderBy(i => i.Name).ToList();
                    var wardsName = wards.Select(i => i.Name).ToList();
                    lstArea.AddRange(wardsName);
                    var reportByWard = list.Where(u => u.DistrictId.Equals(modelSearch.DistrictId));
                    foreach (var ward in wards)
                    {
                        var model = new StatisticByAreaModel();
                        model.AreaId = ward.Id;
                        model.Name = ward.Name;
                        model.Count1 = reportByWard.Where(u => u.AbuseId == Constants.Abuse1 && ward.Id.Equals(u.WardId)).Count();
                        model.Count2 = reportByWard.Where(u => u.AbuseId == Constants.Abuse2 && ward.Id.Equals(u.WardId)).Count();
                        model.Count3 = reportByWard.Where(u => u.AbuseId == Constants.Abuse3 && ward.Id.Equals(u.WardId)).Count();
                        model.Count4 = reportByWard.Where(u => u.AbuseId == Constants.Abuse4 && ward.Id.Equals(u.WardId)).Count();
                        model.Total = list.Where(u => u.WardId.Equals(ward.Id)).GroupBy(u => u.Id).Count();
                        lstTable.Add(model);
                    }

                    foreach (var item in abuSeAll)
                    {
                        itemChart = new AreaChartModel();
                        itemChart.Count = new List<int>();
                        itemChart.Lable = item.Name;
                        foreach (var itemChild in wards)
                        {
                            itemChart.Count.Add(reportByWard.Where(u => u.AbuseId.Equals(item.Id) && u.WardId.Equals(itemChild.Id)).Count());
                        }
                        lstChart.Add(itemChart);
                    }
                    searchResult.LstChart = lstChart;
                    searchResult.LstTable = lstTable;
                    searchResult.LstArea = lstArea;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public List<StatisticByProcessingModel> SearchStatisticByProcessing(StatisticSearchCondition modelSearch)
        {
            List<StatisticByProcessingModel> searchResult = new List<StatisticByProcessingModel>();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var listmodel = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                 join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                 where b.CreateDate.Year.Equals(DateTime.Now.Year) && b.IsDelete == false
                                 select new StatisticByProcessingModel()
                                 {
                                     StatusStep1 = b.StatusStep1.Value,
                                     StatusStep2 = b.StatusStep2.Value,
                                     StatusStep3 = b.StatusStep3.Value,
                                     StatusStep4 = b.StatusStep4.Value,
                                     StatusStep5 = b.StatusStep5.Value,
                                     AbuseTypeName = a.AbuseTypeName,
                                     AbuseId = a.AbuseTypeId,
                                     WardId = b.WardId,
                                     DistrictId = b.DistrictId,
                                     ProvinceId = b.ProvinceId,
                                     ReceptionDate = b.ReceptionDate,
                                     IsPublish = b.IsPublish
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(modelSearch.WardId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(modelSearch.DistrictId));
                }
                if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(modelSearch.ProvinceId));
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                searchResult = listmodel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public ReturnLocationModel SearchStatisticByLocation(StatisticSearchCondition modelSearch)
        {
            ReturnLocationModel searchResult = new ReturnLocationModel();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var reportProfiles = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                      join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                      where b.IsDelete == false
                                      select new StatisticByLocationModel()
                                      {
                                          AbuseId = a.AbuseTypeId,
                                          AbuseName = a.AbuseTypeName,
                                          WardId = b.WardId,
                                          DistrictId = b.DistrictId,
                                          ProvinceId = b.ProvinceId,
                                          ReceptionDate = b.ReceptionDate,
                                          IsPublish = b.IsPublish
                                      }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(modelSearch.DateFrom);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(modelSearch.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(modelSearch.DateTo);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate <= dateTo);
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    reportProfiles = reportProfiles.Where(u => u.IsPublish == true);
                }
                var list = reportProfiles.ToList();
                //area
                List<string> lstLocation = new List<string>();
                //abuse type
                List<string> lstType = new List<string>();
                // chart
                List<LocationChartModel> lstChart = new List<LocationChartModel>();
                LocationChartModel itemChart;
                //table
                List<StatisticByLocationModel> lstTable = new List<StatisticByLocationModel>();
                //abusetype
                var abuSeAll = new ComboboxBusiness().GetAllAbuseType();
                lstType.AddRange(abuSeAll.Select(i => i.Name));
                if (string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    var provinces = db.Provinces.OrderBy(i => i.Name).ToList();
                    var provincesName = provinces.Select(i => i.Name).ToList();
                    var proviceIds = provinces.Select(i => i.Id).ToList();
                    lstLocation.AddRange(provincesName);
                    var reportByProvince = list.Where(u => proviceIds.Contains(u.ProvinceId));
                    foreach (var province in provinces)
                    {
                        var model = new StatisticByLocationModel();
                        model.LableName = province.Name;
                        model.AreaId = province.Id;
                        model.Count1 = reportByProvince.Where(u => u.AbuseId == Constants.Abuse1 && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count2 = reportByProvince.Where(u => u.AbuseId == Constants.Abuse2 && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count3 = reportByProvince.Where(u => u.AbuseId == Constants.Abuse3 && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count4 = reportByProvince.Where(u => u.AbuseId == Constants.Abuse4 && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count5 = reportByProvince.Where(u => u.AbuseId == Constants.Abuse5 && province.Id.Equals(u.ProvinceId)).Count();
                        lstTable.Add(model);
                    }

                    foreach (var item in abuSeAll.Where(u => u.Id.Equals(modelSearch.AbuseId)))
                    {
                        itemChart = new LocationChartModel();
                        itemChart.Count = new List<int>();
                        itemChart.TypeName = item.Name;
                        foreach (var itemChild in provinces)
                        {
                            itemChart.Count.Add(reportByProvince.Where(u => u.AbuseId.Equals(item.Id) && u.ProvinceId.Equals(itemChild.Id)).Count());
                        }
                        lstChart.Add(itemChart);
                    }
                    searchResult.LstChart = lstChart;
                    searchResult.LstTable = lstTable;
                    searchResult.ListLocation = lstLocation;
                    searchResult.LstType = lstType;
                }
                else
                {
                    if (string.IsNullOrEmpty(modelSearch.DistrictId))
                    {
                        var districts = db.Districts.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId)).OrderBy(i => i.Name).ToList();
                        var districtsName = districts.Select(i => i.Name).ToList();
                        lstLocation.AddRange(districtsName);
                        var reportByDistrict = list.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId));
                        foreach (var district in districts)
                        {
                            var model = new StatisticByLocationModel();
                            model.LableName = district.Name;
                            model.AreaId = district.Id;
                            model.Count1 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse1 && district.Id.Equals(u.DistrictId)).Count();
                            model.Count2 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse2 && district.Id.Equals(u.DistrictId)).Count();
                            model.Count3 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse3 && district.Id.Equals(u.DistrictId)).Count();
                            model.Count4 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse4 && district.Id.Equals(u.DistrictId)).Count();
                            model.Count5 = reportByDistrict.Where(u => u.AbuseId == Constants.Abuse5 && district.Id.Equals(u.DistrictId)).Count();
                            lstTable.Add(model);
                        }

                        foreach (var item in abuSeAll.Where(u => u.Id.Equals(modelSearch.AbuseId)))
                        {
                            itemChart = new LocationChartModel();
                            itemChart.Count = new List<int>();
                            itemChart.TypeName = item.Name;
                            foreach (var itemChild in districts)
                            {
                                itemChart.Count.Add(reportByDistrict.Where(u => u.AbuseId.Equals(item.Id) && u.DistrictId.Equals(itemChild.Id)).Count());
                            }
                            lstChart.Add(itemChart);
                        }
                        searchResult.LstChart = lstChart;
                        searchResult.LstTable = lstTable;
                        searchResult.ListLocation = lstLocation;
                        searchResult.LstType = lstType;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(modelSearch.WardId))
                        {
                            var wards = db.Wards.Where(u => u.DistrictId.Equals(modelSearch.DistrictId)).OrderBy(i => i.Name).ToList();
                            var wardsName = wards.Select(i => i.Name).ToList();
                            lstLocation.AddRange(wardsName);
                            var reportByWard = list.Where(u => u.DistrictId.Equals(modelSearch.DistrictId));
                            foreach (var ward in wards)
                            {
                                var model = new StatisticByLocationModel();
                                model.LableName = ward.Name;
                                model.AreaId = ward.Id;
                                model.Count1 = reportByWard.Where(u => u.AbuseId == Constants.Abuse1 && ward.Id.Equals(u.WardId)).Count();
                                model.Count2 = reportByWard.Where(u => u.AbuseId == Constants.Abuse2 && ward.Id.Equals(u.WardId)).Count();
                                model.Count3 = reportByWard.Where(u => u.AbuseId == Constants.Abuse3 && ward.Id.Equals(u.WardId)).Count();
                                model.Count4 = reportByWard.Where(u => u.AbuseId == Constants.Abuse4 && ward.Id.Equals(u.WardId)).Count();
                                model.Count5 = reportByWard.Where(u => u.AbuseId == Constants.Abuse5 && ward.Id.Equals(u.WardId)).Count();
                                lstTable.Add(model);
                            }

                            foreach (var item in abuSeAll.Where(u => u.Id.Equals(modelSearch.AbuseId)))
                            {
                                itemChart = new LocationChartModel();
                                itemChart.Count = new List<int>();
                                itemChart.TypeName = item.Name;
                                foreach (var itemChild in wards)
                                {
                                    itemChart.Count.Add(reportByWard.Where(u => u.AbuseId.Equals(item.Id) && u.WardId.Equals(itemChild.Id)).Count());
                                }
                                lstChart.Add(itemChart);
                            }
                            searchResult.LstChart = lstChart;
                            searchResult.LstTable = lstTable;
                            searchResult.ListLocation = lstLocation;
                            searchResult.LstType = lstType;
                        }
                        else
                        {
                            var ward = db.Wards.Where(u => u.Id.Equals(modelSearch.WardId)).FirstOrDefault();
                            if (ward != null)
                            {
                                lstLocation.Add(ward.Name);
                                var reportByWard = list.Where(u => u.WardId.Equals(modelSearch.WardId));
                                var model = new StatisticByLocationModel();
                                model.LableName = ward.Name;
                                model.AreaId = ward.Id;
                                model.Count1 = reportByWard.Where(u => u.AbuseId == Constants.Abuse1 && ward.Id.Equals(u.WardId)).Count();
                                model.Count2 = reportByWard.Where(u => u.AbuseId == Constants.Abuse2 && ward.Id.Equals(u.WardId)).Count();
                                model.Count3 = reportByWard.Where(u => u.AbuseId == Constants.Abuse3 && ward.Id.Equals(u.WardId)).Count();
                                model.Count4 = reportByWard.Where(u => u.AbuseId == Constants.Abuse4 && ward.Id.Equals(u.WardId)).Count();
                                model.Count5 = reportByWard.Where(u => u.AbuseId == Constants.Abuse5 && ward.Id.Equals(u.WardId)).Count();
                                lstTable.Add(model);

                                foreach (var item in abuSeAll.Where(u => u.Id.Equals(modelSearch.AbuseId)))
                                {
                                    itemChart = new LocationChartModel();
                                    itemChart.Count = new List<int>();
                                    itemChart.TypeName = item.Name;
                                    itemChart.Count.Add(reportByWard.Where(u => u.AbuseId.Equals(item.Id) && u.WardId.Equals(ward.Id)).Count());
                                    lstChart.Add(itemChart);
                                }
                                searchResult.LstChart = lstChart;
                                searchResult.LstTable = lstTable;
                                searchResult.ListLocation = lstLocation;
                                searchResult.LstType = lstType;
                            }
                        }
                    }
                }
                //excel
                if (modelSearch.Export != 0 && modelSearch.Export != 3)
                {
                    searchResult.PathFile = ExportExcelByLocation(searchResult.LstTable, modelSearch, abuSeAll);
                }
                else
                {
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public StatisticByMostAbuseModel StatisticWitMostAbuse(StatisticSearchCondition modelSearch)
        {
            StatisticByMostAbuseModel searchResult = new StatisticByMostAbuseModel();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var reportProfiles = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                      join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                      where b.IsDelete == false
                                      select new
                                      {
                                          Id = b.Id,
                                          AbuseName = a.AbuseTypeName,
                                          AbuseId = a.AbuseTypeId,
                                          ProvinceId = b.ProvinceId,
                                          DistrictId = b.DistrictId,
                                          WardId = b.WardId,
                                          ReceptionDate = b.ReceptionDate,
                                          IsPublish = b.IsPublish
                                      }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(modelSearch.DateFrom);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(modelSearch.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(modelSearch.DateTo);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate <= dateTo);
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    reportProfiles = reportProfiles.Where(u => u.IsPublish == true);
                }
                var list = reportProfiles.ToList();
                var listProvinceId = list.Select(u => u.ProvinceId).ToList();
                var listDistrictId = list.Select(u => u.DistrictId).ToList();
                var listWardId = list.Select(u => u.WardId).ToList();
                string parentName = "";
                //abuse
                var abuse = db.AbuseTypes.Select(i => i.Name).ToList();
                //provinces
                var provinces = db.Provinces.Where(u => listProvinceId.Contains(u.Id)).ToList();
                var provinceTable = new List<ProvinceByMostAbuse>();
                //districts
                var districts = db.Districts.Where(u => listDistrictId.Contains(u.Id)).ToList();
                var districtTable = new List<DistrictByMostAbuse>();
                //wards
                var wards = db.Wards.Where(u => listWardId.Contains(u.Id)).ToList();
                var wardTable = new List<WardByMostAbuse>();
                // table province
                foreach (var item in provinces)
                {
                    var itemTable = new ProvinceByMostAbuse()
                    {
                        LableName = item.Name,
                        Count1 = list.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseId == Constants.Abuse1).Count(),
                        Count2 = list.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseId == Constants.Abuse2).Count(),
                        Count3 = list.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseId == Constants.Abuse3).Count(),
                        Count4 = list.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseId == Constants.Abuse4).Count(),
                        Count5 = list.Where(u => u.ProvinceId.Equals(item.Id) && u.AbuseId == Constants.Abuse5).Count(),
                    };
                    itemTable.Total = list.Where(u => u.ProvinceId.Equals(item.Id)).GroupBy(u => u.Id).Count();
                    provinceTable.Add(itemTable);
                }
                // table district
                foreach (var item in districts)
                {
                    parentName = provinces.FirstOrDefault(u => u.Id.Equals(item.ProvinceId)).Name;
                    var itemTable = new DistrictByMostAbuse()
                    {
                        LableName = item.Name + " - " + parentName,
                        Count1 = list.Where(u => u.DistrictId.Equals(item.Id) && u.AbuseId == Constants.Abuse1).Count(),
                        Count2 = list.Where(u => u.DistrictId.Equals(item.Id) && u.AbuseId == Constants.Abuse2).Count(),
                        Count3 = list.Where(u => u.DistrictId.Equals(item.Id) && u.AbuseId == Constants.Abuse3).Count(),
                        Count4 = list.Where(u => u.DistrictId.Equals(item.Id) && u.AbuseId == Constants.Abuse4).Count(),
                        Count5 = list.Where(u => u.DistrictId.Equals(item.Id) && u.AbuseId == Constants.Abuse5).Count(),
                    };
                    itemTable.Total = list.Where(u => u.DistrictId.Equals(item.Id)).GroupBy(u => u.Id).Count();
                    districtTable.Add(itemTable);
                }
                // table ward
                District districtTemp;
                foreach (var item in wards)
                {
                    districtTemp = districts.FirstOrDefault(u => u.Id.Equals(item.DistrictId));
                    parentName = districtTemp.Name + " - " + (provinces.FirstOrDefault(u => u.Id.Equals(districtTemp.ProvinceId)).Name);
                    var itemTable = new WardByMostAbuse()
                    {
                        LableName = item.Name + " - " + parentName,
                        Count1 = list.Where(u => u.WardId.Equals(item.Id) && u.AbuseId == Constants.Abuse1).Count(),
                        Count2 = list.Where(u => u.WardId.Equals(item.Id) && u.AbuseId == Constants.Abuse2).Count(),
                        Count3 = list.Where(u => u.WardId.Equals(item.Id) && u.AbuseId == Constants.Abuse3).Count(),
                        Count4 = list.Where(u => u.WardId.Equals(item.Id) && u.AbuseId == Constants.Abuse4).Count(),
                        Count5 = list.Where(u => u.WardId.Equals(item.Id) && u.AbuseId == Constants.Abuse5).Count(),
                    };
                    itemTable.Total = list.Where(u => u.WardId.Equals(item.Id)).GroupBy(u => u.Id).Count();
                    wardTable.Add(itemTable);
                }
                searchResult.LstProvince = provinceTable.OrderByDescending(i => i.Total).Take(5).ToList();
                searchResult.LstDistrict = districtTable.OrderByDescending(i => i.Total).Take(5).ToList();
                searchResult.LstWard = wardTable.OrderByDescending(i => i.Total).Take(5).ToList();
                searchResult.LstAbuse = abuse;
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }

        public ReturnChildFundAreaModel SearchStatisticByChildFundArea(StatisticSearchCondition modelSearch)
        {
            ReturnChildFundAreaModel searchResult = new ReturnChildFundAreaModel();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var reportProfiles = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                      join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                      where b.IsDelete == false
                                      select new StatisticByChildFundAreaModel()
                                      {
                                          Id = b.Id,
                                          AbuseName = a.AbuseTypeName,
                                          AbuseId = a.AbuseTypeId,
                                          ReceptionDate = b.ReceptionDate,
                                          WardId = b.WardId,
                                          DistrictId = b.DistrictId,
                                          ProvinceId = b.ProvinceId,
                                          IsPublish = b.IsPublish
                                      }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.DateFrom))
                {
                    var dateFrom = DateTimeUtils.ConvertDateFromStr(modelSearch.DateFrom);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate >= dateFrom);
                }
                if (!string.IsNullOrEmpty(modelSearch.DateTo))
                {
                    var dateTo = DateTimeUtils.ConvertDateToStr(modelSearch.DateTo);
                    reportProfiles = reportProfiles.Where(r => r.ReceptionDate <= dateTo);
                }
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    reportProfiles = reportProfiles.Where(r => r.WardId.ToLower().Contains(modelSearch.WardId.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    reportProfiles = reportProfiles.Where(r => r.DistrictId.ToLower().Contains(modelSearch.DistrictId.ToLower()));
                }
                if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    reportProfiles = reportProfiles.Where(r => r.ProvinceId.ToLower().Contains(modelSearch.ProvinceId.ToLower()));
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    reportProfiles = reportProfiles.Where(u => u.IsPublish == true);
                }
                var list = reportProfiles.ToList();
                var reportList = db.ReportProfiles.Select(i => i.Id).ToList();
                //area
                List<string> lstArea = new List<string>();
                // chart
                List<ChildFundAreaChartModel> lstChart = new List<ChildFundAreaChartModel>();
                ChildFundAreaChartModel itemChart;
                //table
                List<StatisticByChildFundAreaModel> lstTable = new List<StatisticByChildFundAreaModel>();
                //abusetype
                var abuSeAll = new ComboboxBusiness().GetAllAbuseType();
                if (string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    var provinceArea = db.AreaUsers.OrderBy(i => i.Name).ToList();
                    var provinceAreaName = provinceArea.Select(i => i.Name).ToList();
                    lstArea.AddRange(provinceAreaName);
                    foreach (var province in provinceArea)
                    {
                        var model = new StatisticByChildFundAreaModel();
                        model.Name = province.Name;
                        model.AreaId = province.ProvinceId;
                        model.Count1 = list.Where(u => u.AbuseId == Constants.Abuse1 && u.ProvinceId.Equals(province.ProvinceId)).Count();
                        model.Count2 = list.Where(u => u.AbuseId == Constants.Abuse2 && u.ProvinceId.Equals(province.ProvinceId)).Count();
                        model.Count3 = list.Where(u => u.AbuseId == Constants.Abuse3 && u.ProvinceId.Equals(province.ProvinceId)).Count();
                        model.Count4 = list.Where(u => u.AbuseId == Constants.Abuse4 && u.ProvinceId.Equals(province.ProvinceId)).Count();
                        model.Count5 = list.Where(u => u.AbuseId == Constants.Abuse5 && u.ProvinceId.Equals(province.ProvinceId)).Count();
                        model.Total = list.Where(u => u.ProvinceId.Equals(province.ProvinceId)).GroupBy(u => u.Id).Count();
                        lstTable.Add(model);
                    }

                    foreach (var item in abuSeAll)
                    {
                        itemChart = new ChildFundAreaChartModel();
                        itemChart.Count = new List<int>();
                        itemChart.Lable = item.Name;
                        foreach (var itemChild in provinceArea)
                        {
                            itemChart.Count.Add(list.Where(u => u.AbuseId.Equals(item.Id) && u.ProvinceId.Equals(itemChild.ProvinceId)).Count());
                        }
                        lstChart.Add(itemChart);
                    }
                    searchResult.LstChart = lstChart;
                    searchResult.LstTable = lstTable;
                    searchResult.LstArea = lstArea;
                }
                else
                {
                    if (string.IsNullOrEmpty(modelSearch.DistrictId))
                    {
                        var districtArea = db.AreaDistricts.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId)).OrderBy(i => i.Name).ToList();
                        var districtAreaName = districtArea.Select(i => i.Name).ToList();
                        lstArea.AddRange(districtAreaName);
                        foreach (var district in districtArea)
                        {
                            var model = new StatisticByChildFundAreaModel();
                            model.Name = district.Name;
                            model.AreaId = district.DistrictId;
                            model.Count1 = list.Where(u => u.AbuseId == Constants.Abuse1 && u.DistrictId.Equals(district.DistrictId)).Count();
                            model.Count2 = list.Where(u => u.AbuseId == Constants.Abuse2 && u.DistrictId.Equals(district.DistrictId)).Count();
                            model.Count3 = list.Where(u => u.AbuseId == Constants.Abuse3 && u.DistrictId.Equals(district.DistrictId)).Count();
                            model.Count4 = list.Where(u => u.AbuseId == Constants.Abuse4 && u.DistrictId.Equals(district.DistrictId)).Count();
                            model.Count5 = list.Where(u => u.AbuseId == Constants.Abuse5 && u.DistrictId.Equals(district.DistrictId)).Count();
                            model.Total = list.Where(u => u.DistrictId.Equals(district.DistrictId)).GroupBy(u => u.Id).Count();
                            lstTable.Add(model);
                        }

                        foreach (var item in abuSeAll)
                        {
                            itemChart = new ChildFundAreaChartModel();
                            itemChart.Count = new List<int>();
                            itemChart.Lable = item.Name;
                            foreach (var itemChild in districtArea)
                            {
                                itemChart.Count.Add(list.Where(u => u.AbuseId.Equals(item.Id) && u.DistrictId.Equals(itemChild.DistrictId)).Count());
                            }
                            lstChart.Add(itemChart);
                        }
                        searchResult.LstChart = lstChart;
                        searchResult.LstTable = lstTable;
                        searchResult.LstArea = lstArea;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(modelSearch.WardId))
                        {
                            var wardArea = db.AreaWards.Where(u => u.DistrictId.Equals(modelSearch.DistrictId)).OrderBy(i => i.Name).ToList();
                            var wardAreaName = wardArea.Select(i => i.Name).ToList();
                            lstArea.AddRange(wardAreaName);
                            foreach (var ward in wardArea)
                            {
                                var model = new StatisticByChildFundAreaModel();
                                model.Name = ward.Name;
                                model.AreaId = ward.WardId;
                                model.Count1 = list.Where(u => u.AbuseId == Constants.Abuse1 && u.WardId.Equals(ward.WardId)).Count();
                                model.Count2 = list.Where(u => u.AbuseId == Constants.Abuse2 && u.WardId.Equals(ward.WardId)).Count();
                                model.Count3 = list.Where(u => u.AbuseId == Constants.Abuse3 && u.WardId.Equals(ward.WardId)).Count();
                                model.Count4 = list.Where(u => u.AbuseId == Constants.Abuse4 && u.WardId.Equals(ward.WardId)).Count();
                                model.Count5 = list.Where(u => u.AbuseId == Constants.Abuse5 && u.WardId.Equals(ward.WardId)).Count();
                                model.Total = list.Where(u => u.WardId.Equals(ward.WardId)).GroupBy(u => u.Id).Count();
                                lstTable.Add(model);
                            }

                            foreach (var item in abuSeAll)
                            {
                                itemChart = new ChildFundAreaChartModel();
                                itemChart.Count = new List<int>();
                                itemChart.Lable = item.Name;
                                foreach (var itemChild in wardArea)
                                {
                                    itemChart.Count.Add(list.Where(u => u.AbuseId.Equals(item.Id) && u.WardId.Equals(itemChild.WardId)).Count());
                                }
                                lstChart.Add(itemChart);
                            }
                            searchResult.LstChart = lstChart;
                            searchResult.LstTable = lstTable;
                            searchResult.LstArea = lstArea;
                        }
                        else
                        {
                            var ward = db.AreaWards.Where(u => u.WardId.Equals(modelSearch.WardId)).FirstOrDefault();
                            if (ward != null)
                            {
                                lstArea.Add(ward.Name);
                                var reportByWard = list.Where(u => u.WardId.Equals(modelSearch.WardId));
                                var model = new StatisticByChildFundAreaModel();
                                model.Name = ward.Name;
                                model.AreaId = ward.WardId;
                                model.Count1 = reportByWard.Where(u => u.AbuseId == Constants.Abuse1).Count();
                                model.Count2 = reportByWard.Where(u => u.AbuseId == Constants.Abuse2).Count();
                                model.Count3 = reportByWard.Where(u => u.AbuseId == Constants.Abuse3).Count();
                                model.Count4 = reportByWard.Where(u => u.AbuseId == Constants.Abuse4).Count();
                                model.Count5 = reportByWard.Where(u => u.AbuseId == Constants.Abuse5).Count();
                                model.Total = reportByWard.GroupBy(u => u.Id).Count();
                                lstTable.Add(model);

                                foreach (var item in abuSeAll)
                                {
                                    itemChart = new ChildFundAreaChartModel();
                                    itemChart.Count = new List<int>();
                                    itemChart.Lable = item.Name;
                                    itemChart.Count.Add(reportByWard.Where(u => u.AbuseId.Equals(item.Id)).Count());
                                    lstChart.Add(itemChart);
                                }
                                searchResult.LstChart = lstChart;
                                searchResult.LstTable = lstTable;
                                searchResult.LstArea = lstArea;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public ReturnProcessingModel SearchStatisticByProcessingStatus(StatisticSearchCondition modelSearch)
        {
            ReturnProcessingModel searchResult = new ReturnProcessingModel();
            try
            {
                var userId = HttpContext.Current.User.Identity.Name;
                var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(userId);
                var listmodel = (from a in db.ReportProfileAbuseTypes.AsNoTracking()
                                 join b in db.ReportProfiles.AsNoTracking() on a.ReportProfileId equals b.Id
                                 where b.CreateDate.Year.Equals(DateTime.Now.Year) && b.IsDelete == false
                                 select new StatisticByProcessingModel()
                                 {
                                     StatusStep1 = b.StatusStep1.Value,
                                     StatusStep2 = b.StatusStep2.Value,
                                     StatusStep3 = b.StatusStep3.Value,
                                     StatusStep4 = b.StatusStep4.Value,
                                     StatusStep5 = b.StatusStep5.Value,
                                     AbuseTypeName = a.AbuseTypeName,
                                     AbuseId = a.AbuseTypeId,
                                     WardId = b.WardId,
                                     DistrictId = b.DistrictId,
                                     ProvinceId = b.ProvinceId,
                                     ReceptionDate = b.ReceptionDate,
                                     IsPublish = b.IsPublish
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(modelSearch.WardId))
                {
                    listmodel = listmodel.Where(r => r.WardId.Equals(modelSearch.WardId));
                }
                if (!string.IsNullOrEmpty(modelSearch.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(modelSearch.DistrictId));
                }
                if (!string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    listmodel = listmodel.Where(r => r.ProvinceId.Equals(modelSearch.ProvinceId));
                }
                if (userInfo.Type != Constants.LevelTeacher)
                {
                    listmodel = listmodel.Where(u => u.IsPublish == true);
                }
                var list = listmodel.ToList();
                // table
                List<StatisticByProcessingModel> lstTable = new List<StatisticByProcessingModel>();
                // chart
                List<ChartModel> lstChart = new List<ChartModel>();
                ChartModel itemChart;
                //processing
                List<ProcessingStatus> status = new List<ProcessingStatus>();
                status.Add(new ProcessingStatus { Status = 1, StatusLable = Resource.Resource.ReportProfile_Status1 });
                status.Add(new ProcessingStatus { Status = 2, StatusLable = Resource.Resource.ReportProfile_Status2 });
                status.Add(new ProcessingStatus { Status = 3, StatusLable = Resource.Resource.ReportProfile_Status3 });
                status.Add(new ProcessingStatus { Status = 4, StatusLable = Resource.Resource.ReportProfile_Status4 });
                status.Add(new ProcessingStatus { Status = 5, StatusLable = Resource.Resource.ReportProfile_Status5 });
                //area
                List<string> lstLocation = new List<string>();
                //
                if (string.IsNullOrEmpty(modelSearch.ProvinceId))
                {
                    var provinces = db.Provinces.OrderBy(i => i.Name).ToList();
                    var provincesName = provinces.Select(i => i.Name).ToList();
                    var proviceIds = provinces.Select(i => i.Id).ToList();
                    lstLocation.AddRange(provincesName);
                    var reportByProvince = list.Where(u => proviceIds.Contains(u.ProvinceId));
                    foreach (var province in provinces)
                    {
                        var model = new StatisticByProcessingModel();
                        model.LableName = province.Name;
                        model.AreaId = province.Id;
                        model.Count1 = reportByProvince.Where(u => u.StatusStep1 == true && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count2 = reportByProvince.Where(u => u.StatusStep2 == true && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count3 = reportByProvince.Where(u => u.StatusStep3 == true && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count4 = reportByProvince.Where(u => u.StatusStep4 == true && province.Id.Equals(u.ProvinceId)).Count();
                        model.Count5 = reportByProvince.Where(u => u.StatusStep5 == true && province.Id.Equals(u.ProvinceId)).Count();
                        lstTable.Add(model);
                    }

                    foreach (var item in status)
                    {
                        itemChart = new ChartModel();
                        itemChart.Count = new List<int>();
                        itemChart.Lable = item.StatusLable;
                        foreach (var itemChild in provinces)
                        {
                            switch (item.Status)
                            {
                                case 1:
                                    itemChart.Count.Add(reportByProvince.Where(u => u.StatusStep1 == true && u.ProvinceId.Equals(itemChild.Id)).Count());
                                    break;
                                case 2:
                                    itemChart.Count.Add(reportByProvince.Where(u => u.StatusStep2 == true && u.ProvinceId.Equals(itemChild.Id)).Count());
                                    break;
                                case 3:
                                    itemChart.Count.Add(reportByProvince.Where(u => u.StatusStep3 == true && u.ProvinceId.Equals(itemChild.Id)).Count());
                                    break;
                                case 4:
                                    itemChart.Count.Add(reportByProvince.Where(u => u.StatusStep4 == true && u.ProvinceId.Equals(itemChild.Id)).Count());
                                    break;
                                case 5:
                                    itemChart.Count.Add(reportByProvince.Where(u => u.StatusStep5 == true && u.ProvinceId.Equals(itemChild.Id)).Count());
                                    break;
                                default:
                                    break;
                            }

                        }
                        lstChart.Add(itemChart);
                    }
                    searchResult.LstChart = lstChart;
                    searchResult.LstTable = lstTable;
                    searchResult.ListLocation = lstLocation;
                }
                else
                {
                    if (string.IsNullOrEmpty(modelSearch.DistrictId))
                    {
                        var districts = db.Districts.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId)).OrderBy(i => i.Name).ToList();
                        var districtsName = districts.Select(i => i.Name).ToList();
                        lstLocation.AddRange(districtsName);
                        var reportByDistrict = list.Where(u => u.ProvinceId.Equals(modelSearch.ProvinceId));
                        foreach (var district in districts)
                        {
                            var model = new StatisticByProcessingModel();
                            model.LableName = district.Name;
                            model.AreaId = district.Id;
                            model.Count1 = reportByDistrict.Where(u => u.StatusStep1 == true && district.Id.Equals(u.DistrictId)).Count();
                            model.Count2 = reportByDistrict.Where(u => u.StatusStep1 == true && district.Id.Equals(u.DistrictId)).Count();
                            model.Count3 = reportByDistrict.Where(u => u.StatusStep1 == true && district.Id.Equals(u.DistrictId)).Count();
                            model.Count4 = reportByDistrict.Where(u => u.StatusStep1 == true && district.Id.Equals(u.DistrictId)).Count();
                            model.Count5 = reportByDistrict.Where(u => u.StatusStep1 == true && district.Id.Equals(u.DistrictId)).Count();
                            lstTable.Add(model);
                        }

                        foreach (var item in status)
                        {
                            itemChart = new ChartModel();
                            itemChart.Count = new List<int>();
                            itemChart.Lable = item.StatusLable;
                            foreach (var itemChild in districts)
                            {
                                switch (item.Status)
                                {
                                    case 1:
                                        itemChart.Count.Add(reportByDistrict.Where(u => u.StatusStep1 == true && u.DistrictId.Equals(itemChild.Id)).Count());
                                        break;
                                    case 2:
                                        itemChart.Count.Add(reportByDistrict.Where(u => u.StatusStep2 == true && u.DistrictId.Equals(itemChild.Id)).Count());
                                        break;
                                    case 3:
                                        itemChart.Count.Add(reportByDistrict.Where(u => u.StatusStep3 == true && u.DistrictId.Equals(itemChild.Id)).Count());
                                        break;
                                    case 4:
                                        itemChart.Count.Add(reportByDistrict.Where(u => u.StatusStep4 == true && u.DistrictId.Equals(itemChild.Id)).Count());
                                        break;
                                    case 5:
                                        itemChart.Count.Add(reportByDistrict.Where(u => u.StatusStep5 == true && u.DistrictId.Equals(itemChild.Id)).Count());
                                        break;
                                    default:
                                        break;
                                }
                            }
                            lstChart.Add(itemChart);
                        }
                        searchResult.LstChart = lstChart;
                        searchResult.LstTable = lstTable;
                        searchResult.ListLocation = lstLocation;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(modelSearch.WardId))
                        {
                            var wards = db.Wards.Where(u => u.DistrictId.Equals(modelSearch.DistrictId)).OrderBy(i => i.Name).ToList();
                            var wardsName = wards.Select(i => i.Name).ToList();
                            lstLocation.AddRange(wardsName);
                            var reportByWard = list.Where(u => u.DistrictId.Equals(modelSearch.DistrictId));
                            foreach (var ward in wards)
                            {
                                var model = new StatisticByProcessingModel();
                                model.LableName = ward.Name;
                                model.AreaId = ward.Id;
                                model.Count1 = reportByWard.Where(u => u.StatusStep1 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count2 = reportByWard.Where(u => u.StatusStep2 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count3 = reportByWard.Where(u => u.StatusStep3 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count4 = reportByWard.Where(u => u.StatusStep4 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count5 = reportByWard.Where(u => u.StatusStep5 == true && ward.Id.Equals(u.WardId)).Count();
                                lstTable.Add(model);
                            }

                            foreach (var item in status)
                            {
                                itemChart = new ChartModel();
                                itemChart.Count = new List<int>();
                                itemChart.Lable = item.StatusLable;
                                foreach (var itemChild in wards)
                                {
                                    switch (item.Status)
                                    {
                                        case 1:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep1 == true && u.WardId.Equals(itemChild.Id)).Count());
                                            break;
                                        case 2:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep2 == true && u.WardId.Equals(itemChild.Id)).Count());
                                            break;
                                        case 3:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep3 == true && u.WardId.Equals(itemChild.Id)).Count());
                                            break;
                                        case 4:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep4 == true && u.WardId.Equals(itemChild.Id)).Count());
                                            break;
                                        case 5:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep5 == true && u.WardId.Equals(itemChild.Id)).Count());
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                lstChart.Add(itemChart);
                            }
                            searchResult.LstChart = lstChart;
                            searchResult.LstTable = lstTable;
                            searchResult.ListLocation = lstLocation;
                        }
                        else
                        {
                            var ward = db.Wards.Where(u => u.Id.Equals(modelSearch.WardId)).FirstOrDefault();
                            if (ward != null)
                            {
                                lstLocation.Add(ward.Name);
                                var reportByWard = list.Where(u => u.WardId.Equals(modelSearch.WardId));
                                var model = new StatisticByProcessingModel();
                                model.LableName = ward.Name;
                                model.AreaId = ward.Id;
                                model.Count1 = reportByWard.Where(u => u.StatusStep1 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count2 = reportByWard.Where(u => u.StatusStep2 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count3 = reportByWard.Where(u => u.StatusStep3 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count4 = reportByWard.Where(u => u.StatusStep4 == true && ward.Id.Equals(u.WardId)).Count();
                                model.Count5 = reportByWard.Where(u => u.StatusStep5 == true && ward.Id.Equals(u.WardId)).Count();
                                lstTable.Add(model);

                                foreach (var item in status)
                                {
                                    itemChart = new ChartModel();
                                    itemChart.Count = new List<int>();
                                    itemChart.Lable = item.StatusLable;
                                    switch (item.Status)
                                    {
                                        case 1:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep1 == true && u.WardId.Equals(ward.Id)).Count());
                                            break;
                                        case 2:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep2 == true && u.WardId.Equals(ward.Id)).Count());
                                            break;
                                        case 3:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep3 == true && u.WardId.Equals(ward.Id)).Count());
                                            break;
                                        case 4:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep4 == true && u.WardId.Equals(ward.Id)).Count());
                                            break;
                                        case 5:
                                            itemChart.Count.Add(reportByWard.Where(u => u.StatusStep5 == true && u.WardId.Equals(ward.Id)).Count());
                                            break;
                                        default:
                                            break;
                                    }

                                    lstChart.Add(itemChart);
                                }
                                searchResult.LstChart = lstChart;
                                searchResult.LstTable = lstTable;
                                searchResult.ListLocation = lstLocation;
                            }
                        }
                    }
                }
                //excel
                if (modelSearch.Export != 0 && modelSearch.Export != 3)
                {
                    searchResult.PathFile = ExportExcelByProcessingStatus(searchResult.LstTable, modelSearch.Export);
                }
                else
                {
                    searchResult.PathFile = "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }

        public string ExportExcelByAge(List<StatisticByAgeModel> list, List<ChartAgeModel> chartdata, List<ChartAgeModel> chartrightdata, StatisticSearchCondition modelSearch)
        {
            string pathExport = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.Statistic_By_Age);
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(pathExport + ".pdf");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/StatisticByAge.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                IWorksheet worksheet1 = workbook.Worksheets[1];

                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.Statistic_By_Age.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<TableTitle>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<TableTitle>", Resource.Resource.Table_By_Age);

                IRange rangeValue3 = worksheet.FindFirst("<Year>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<Year>", Resource.Resource.Year_Title);

                IRange rangeValue4 = worksheet.FindFirst("<Statistic_Age0_3>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<Statistic_Age0_3>", Resource.Resource.Statistic_Age0_3);

                IRange rangeValue5 = worksheet.FindFirst("<Statistic_Age4_6>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Statistic_Age4_6>", Resource.Resource.Statistic_Age4_6);

                IRange rangeValue6 = worksheet.FindFirst("<Statistic_Age7_9>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Statistic_Age7_9>", Resource.Resource.Statistic_Age7_9);

                IRange rangeValue7 = worksheet.FindFirst("<Statistic_Age10>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<Statistic_Age10>", Resource.Resource.Statistic_Age10);

                IRange rangeValue8 = worksheet.FindFirst("<Statistic_Age11_12>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Statistic_Age11_12>", Resource.Resource.Statistic_Age11_12);

                IRange rangeValue9 = worksheet.FindFirst("<Statistic_Age13_14>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue9.Text = rangeValue9.Text.Replace("<Statistic_Age13_14>", Resource.Resource.Statistic_Age13_14);

                IRange rangeValue10 = worksheet.FindFirst("<Statistic_Age15_16>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue10.Text = rangeValue10.Text.Replace("<Statistic_Age15_16>", Resource.Resource.Statistic_Age15_16);

                IRange rangeValue11 = worksheet.FindFirst("<Statistic_Age16_18>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue11.Text = rangeValue11.Text.Replace("<Statistic_Age16_18>", Resource.Resource.Statistic_Age16_18);

                IRange rangeValue12 = worksheet.FindFirst("<ReportProfile_Unknow>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue12.Text = rangeValue12.Text.Replace("<ReportProfile_Unknow>", Resource.Resource.ReportProfile_Unknow);

                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in list
                                  select new
                                  {
                                      a1 = a.LableName.ToString(),
                                      a.Count1,
                                      a.Count2,
                                      a.Count3,
                                      a.Count4,
                                      a.Count5,
                                      a.Count6,
                                      a.Count7,
                                      a.Count8,
                                      a.Count9
                                  }).ToList();
                var count = listExport.Count();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 10].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 10].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 10].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 10].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 10].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 10].CellStyle.WrapText = true;

                IRange iRangeDataChart = worksheet1.FindFirst("<DataChart>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                var datachart = (from a in chartdata
                                 select new
                                 {
                                     a2 = a.Age.ToString(),
                                     a.Percent
                                 }).ToList();
                datachart = datachart.Where(i => i.Percent >= 0).ToList();
                var datachartCount = datachart.Count();
                worksheet1.ImportData(datachart, iRangeDataChart.Row, iRangeDataChart.Column, false);
                //Initialize chart
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = Syncfusion.XlsIO.ExcelChartType.Pie;

                //Assign data
                chart.DataRange = worksheet1["A2:B" + (datachartCount + 1) + ""];
                chart.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = Resource.Resource.ChartAge_Title + " " + modelSearch.FromYear + "-" + modelSearch.ToYear;

                //Set Legend
                chart.HasLegend = true;
                chart.Legend.Position = ExcelLegendPosition.Bottom;

                //Set Datalabels
                IChartSerie serie = chart.Series[0];
                serie.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.BestFit;

                //Positioning the chart in the worksheet
                chart.TopRow = 3;
                chart.LeftColumn = 1;
                chart.RightColumn = 6;
                chart.BottomRow = 25;
                ///////////////////////////////
                IRange iRangeDataChart2 = worksheet1.FindFirst("<DataChartRight>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                var datachart2 = (from a in chartrightdata
                                 select new
                                 {
                                     a2 = a.Age.ToString(),
                                     a.Percent
                                 }).ToList();
                datachart2 = datachart2.Where(i => i.Percent >= 0).ToList();
                var datachartCount2 = datachart2.Count();
                worksheet1.ImportData(datachart2, iRangeDataChart2.Row, iRangeDataChart2.Column, false);
                //Initialize chart
                IChartShape chart2 = worksheet.Charts.Add();
                chart2.ChartType = Syncfusion.XlsIO.ExcelChartType.Pie;

                //Assign data
                chart2.DataRange = worksheet1["D2:E"+(datachartCount2+1)+""];
                chart2.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart2.ChartTitle = Resource.Resource.ChartAge_Title + " " + modelSearch.ClickYear;

                //Set Legend
                chart2.HasLegend = true;
                chart2.Legend.Position = ExcelLegendPosition.Bottom;

                //Set Datalabels
                IChartSerie serie2 = chart2.Series[0];
                serie2.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie2.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.BestFit;

                //Positioning the chart in the worksheet
                chart2.TopRow = 3;
                chart2.LeftColumn = 7;
                chart2.RightColumn = 11;
                chart2.BottomRow = 25;
                workbook.SaveAs(FullPath);
                workbook.Close();

                if (modelSearch.Export == 2)
                {
                    ExcelEngine excelEngine2 = new ExcelEngine();
                    IApplication application2 = excelEngine2.Excel;
                    application2.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                    application2.ChartToImageConverter = new ChartToImageConverter();

                    //Open existing workbook with data entered
                    IWorkbook workbook2 = application2.Workbooks.Open(FullPath, ExcelOpenType.Automatic);
                    IWorksheet worksheet2 = workbook2.Worksheets[0];

                    pathExport += ".pdf";
                    ExcelToPdfConverter converter = new ExcelToPdfConverter(worksheet2);
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument = converter.Convert();
                    pdfDocument.Save(FullPathPDF);
                    pdfDocument.Close();
                    converter.Dispose();
                }
                else
                {
                    pathExport += ".xlsx";
                }
                return pathExport;
            }
        }

        public string ExportExcelByGender(List<StatisticByGenderModel> list, StatisticSearchCondition modelSearch)
        {
            string pathExport = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.Statistic_By_Gender);
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(pathExport + ".pdf");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/StatisticByGender.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];

                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.Statistic_By_Gender.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<TableTitle>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<TableTitle>", Resource.Resource.Table_By_Gender);

                IRange rangeValue3 = worksheet.FindFirst("<Type>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<Type>", Resource.Resource.ReportProfile_AbuseType);

                IRange rangeValue4 = worksheet.FindFirst("<Male>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<Male>", Resource.Resource.ReportProfile_Male);

                IRange rangeValue5 = worksheet.FindFirst("<Female>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Female>", Resource.Resource.ReportProfile_FeMale);

                IRange rangeValue6 = worksheet.FindFirst("<Unknow>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Unknow>", Resource.Resource.ReportProfile_Unknow);

                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in list
                                  select new
                                  {
                                      a.Type,
                                      a.CountNam,
                                      a.CountNu,
                                      a.CountKhong
                                  }).ToList();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + 4, 4].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + 4, 4].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + 4, 4].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + 4, 4].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + 4, 4].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + 4, 4].CellStyle.WrapText = true;

                //Initialize chart
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = Syncfusion.XlsIO.ExcelChartType.Column_Clustered;

                //Assign data
                chart.DataRange = worksheet["A27:D32"];
                chart.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = Resource.Resource.ChartGender_Title + " " + modelSearch.DateFrom + "-" + modelSearch.DateTo;

                //Set Legend
                chart.HasLegend = true;
                chart.Legend.Position = ExcelLegendPosition.Bottom;

                //Positioning the chart in the worksheet
                chart.TopRow = 3;
                chart.LeftColumn = 1;
                chart.RightColumn = 5;
                chart.BottomRow = 25;

                workbook.SaveAs(FullPath);
                workbook.Close();

                if (modelSearch.Export == 2)
                {
                    ExcelEngine excelEngine2 = new ExcelEngine();
                    IApplication application2 = excelEngine2.Excel;
                    application2.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                    application2.ChartToImageConverter = new ChartToImageConverter();

                    //Open existing workbook with data entered
                    IWorkbook workbook2 = application2.Workbooks.Open(FullPath, ExcelOpenType.Automatic);
                    IWorksheet worksheet2 = workbook2.Worksheets[0];

                    pathExport += ".pdf";
                    ExcelToPdfConverter converter = new ExcelToPdfConverter(worksheet2);
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument = converter.Convert();
                    pdfDocument.Save(FullPathPDF);
                    pdfDocument.Close();
                    converter.Dispose();
                }
                else
                {
                    pathExport += ".xlsx";
                }
                return pathExport;
            }
        }

        public string ExportExcelByLocation(List<StatisticByLocationModel> list, StatisticSearchCondition export, List<ComboboxResult> lstAbuse)
        {
            var nameHidd = lstAbuse.FirstOrDefault(u => u.Id.Equals(export.AbuseId)).Name;
            string pathExport = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.Statistic_By_Location);
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(pathExport + ".pdf");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/StatisticByLocation.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                IWorksheet worksheet2 = workbook.Worksheets[1];

                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.Statistic_By_Location.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<TableTitle>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<TableTitle>", Resource.Resource.Table_By_Location);

                IRange rangeValue3 = worksheet.FindFirst("<Location>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<Location>", Resource.Resource.Location);

                IRange rangeValue4 = worksheet.FindFirst("<Abuse_Type_01>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<Abuse_Type_01>", Resource.Resource.Abuse_Type_01);

                IRange rangeValue5 = worksheet.FindFirst("<Abuse_Type_02>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Abuse_Type_02>", Resource.Resource.Abuse_Type_02);

                IRange rangeValue6 = worksheet.FindFirst("<Abuse_Type_03>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Abuse_Type_03>", Resource.Resource.Abuse_Type_03);

                IRange rangeValue7 = worksheet.FindFirst("<Abuse_Type_04>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<Abuse_Type_04>", Resource.Resource.Abuse_Type_04);

                IRange rangeValue7a = worksheet.FindFirst("<Abuse_Type_05>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7a.Text = rangeValue7a.Text.Replace("<Abuse_Type_05>", Resource.Resource.Other_Type);



                IRange rangeValue8 = worksheet2.FindFirst("<Abuse_Type_Hidd>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Abuse_Type_Hidd>", nameHidd);

                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                IRange iRangeData2 = worksheet2.FindFirst("<DataHidd>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in list
                                  select new
                                  {
                                      a.LableName,
                                      a.Count1,
                                      a.Count2,
                                      a.Count3,
                                      a.Count4,
                                      a.Count5
                                  }).ToList();
              
                var count = listExport.Count();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].CellStyle.WrapText = true;
                #region[xuất bảng ẩn]
                switch (export.AbuseId)
                {
                    case Constants.Abuse1:
                        var listExport1 = (from a in list
                                           select new { a.LableName, a.Count1 }).ToList();
                        worksheet2.ImportData(listExport1, iRangeData2.Row, iRangeData2.Column, false);
                        break;
                    case Constants.Abuse2:
                        var listExport2 = (from a in list
                                           select new { a.LableName, a.Count2 }).ToList();
                        worksheet2.ImportData(listExport2, iRangeData2.Row, iRangeData2.Column, false);
                        break;
                    case Constants.Abuse3:
                        var listExport3 = (from a in list
                                           select new { a.LableName, a.Count3 }).ToList();
                        worksheet2.ImportData(listExport3, iRangeData2.Row, iRangeData2.Column, false);
                        break;
                    case Constants.Abuse4:
                        var listExport4 = (from a in list
                                           select new { a.LableName, a.Count4 }).ToList();
                        worksheet2.ImportData(listExport4, iRangeData2.Row, iRangeData2.Column, false);
                        break;
                    default:
                        break;
                }
                #endregion
                //Initialize chart
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = Syncfusion.XlsIO.ExcelChartType.Column_Stacked;

                //Assign data
                int counAll = count + 27;
                chart.DataRange = worksheet2["A1:B" +( count+1) + ""];
          //      chart.DataRange = worksheet[27,1,90,2];
                chart.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = Resource.Resource.ChartLocation_Title;

                //Set Legend
                chart.HasLegend = true;
                chart.Legend.Position = ExcelLegendPosition.Bottom;
                IChartSerie serie1 = chart.Series[0];
                serie1.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie1.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //Positioning the chart in the worksheet
                chart.TopRow = 3;
                chart.LeftColumn = 1;
                chart.RightColumn = 6;
                chart.BottomRow = 25;

                workbook.SaveAs(FullPath);
                workbook.Close();

                if (export.Export == 2)
                {
                    ExcelEngine excelEngine2 = new ExcelEngine();
                    IApplication application2 = excelEngine2.Excel;
                    application2.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                    application2.ChartToImageConverter = new ChartToImageConverter();

                    //Open existing workbook with data entered
                    IWorkbook workbook2 = application2.Workbooks.Open(FullPath, ExcelOpenType.Automatic);
                    IWorksheet worksheet3 = workbook2.Worksheets[0];

                    pathExport += ".pdf";
                    ExcelToPdfConverter converter = new ExcelToPdfConverter(worksheet3);
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument = converter.Convert();
                    pdfDocument.Save(FullPathPDF);
                    pdfDocument.Close();
                    converter.Dispose();
                }
                else
                {
                    pathExport += ".xlsx";
                }
                return pathExport;
            }
        }

        public string ExportReportByStatus(List<StatisticByProcessingModel> lstTable, int Export, bool type, string title)
        {
            int total = lstTable.Count;
            // var dateNow = DateTime.Now.ToString("yyyyMM");
            string rs = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.ReportProfile_Report_Title);
            string FullPath = HttpContext.Current.Server.MapPath(rs + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(rs + ".pdf");
            string pathFileTemplate = HttpContext.Current.Server.MapPath("~/Template/ReportStatusTemp.xlsx");

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathFileTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                IRange rangeValue = worksheet.FindFirst("<Name>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Name>", title.ToUpper());
                //       IRange rangeValue2 = worksheet.FindFirst("<TypeName>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                //       rangeValue2.Text = rangeValue2.Text.Replace("<TypeName>", title);

                IRange rangeValue8 = worksheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Title>", Resource.Resource.Statistic_By.ToUpper());

                IRange rangeValue9 = worksheet.FindFirst("<TableTitle>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue9.Text = rangeValue9.Text.Replace("<TableTitle>", Resource.Resource.Statistic_By);

                IRange rangeValue3 = worksheet.FindFirst("<Type>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<Type>", Resource.Resource.ReportProfile_AbuseType);

                IRange rangeValue4 = worksheet.FindFirst("<ReportProfile_Status5>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<ReportProfile_Status5>", Resource.Resource.ReportProfile_Status5);

                IRange rangeValue5 = worksheet.FindFirst("<ReportProfile_Status1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<ReportProfile_Status1>", Resource.Resource.ReportProfile_Status1);

                IRange rangeValue6 = worksheet.FindFirst("<ReportProfile_Status2>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<ReportProfile_Status2>", Resource.Resource.ReportProfile_Status2);

                IRange rangeValue7 = worksheet.FindFirst("<ReportProfile_Status3>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<ReportProfile_Status3>", Resource.Resource.ReportProfile_Status3);

                IRange rangeValue0 = worksheet.FindFirst("<ReportProfile_Status4>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue0.Text = rangeValue0.Text.Replace("<ReportProfile_Status4>", Resource.Resource.ReportProfile_Status4);

                #region[bang trai]
                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, total - 1, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in lstTable
                                  select new
                                  {
                                      a.AbuseTypeName,
                                      a.Count1,
                                      a.Count2,
                                      a.Count3,
                                      a.Count4,
                                      a.Count5,
                                  }).ToList();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 6].CellStyle.WrapText = true;
                #endregion
                #region[bieu do duong]
                IChartShape chart2 = worksheet.Charts.Add();
                chart2.ChartType = Syncfusion.XlsIO.ExcelChartType.Column_Stacked;

                //Assign data
                chart2.DataRange = worksheet["A29:F33"];
                chart2.IsSeriesInRows = type;

                //Apply chart elements
                //Set Chart Title
                chart2.ChartTitle = Resource.Resource.ReportProfile_ByAbuse;

                //Set Legend
                chart2.HasLegend = true;
                chart2.Legend.Position = ExcelLegendPosition.Bottom;

                //Set Datalabels
                IChartSerie serie1 = chart2.Series[0];
                //  IChartSerie serie2 = chart2.Series[1];
                ///  IChartSerie serie3 = chart2.Series[2];

                serie1.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //     serie2.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //     serie3.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie1.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //   serie2.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //  serie3.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;

                //Positioning the chart in the worksheet
                chart2.TopRow = 5;
                chart2.LeftColumn = 1;
                chart2.BottomRow = 27;
                chart2.RightColumn = 7;
                #endregion
                workbook.SaveAs(FullPath);
                workbook.Close();
                if (Export == 2)
                {
                    ExcelEngine excelEngine2 = new ExcelEngine();
                    IApplication application2 = excelEngine2.Excel;
                    application2.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                    application2.ChartToImageConverter = new ChartToImageConverter();

                    //Open existing workbook with data entered
                    IWorkbook workbook2 = application2.Workbooks.Open(FullPath, ExcelOpenType.Automatic);
                    IWorksheet worksheet2 = workbook2.Worksheets[0];

                    rs += ".pdf";
                    ExcelToPdfConverter converter = new ExcelToPdfConverter(worksheet2);
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument = converter.Convert();
                    pdfDocument.Save(FullPathPDF);
                    pdfDocument.Close();
                    converter.Dispose();
                }
                else
                {
                    rs += ".xlsx";
                }
                return rs;
            }
        }

        public string ExportExcelByProcessingStatus(List<StatisticByProcessingModel> list, int export)
        {
            string pathExport = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.Statistic_By_Process);
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(pathExport + ".pdf");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/StatisticByProcessingStatus.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];

                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.Statistic_By_Process.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<TableTitle>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<TableTitle>", Resource.Resource.Table_By_Process);

                IRange rangeValue3 = worksheet.FindFirst("<Location>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<Location>", Resource.Resource.Location);

                IRange rangeValue4 = worksheet.FindFirst("<ReportProfile_Status5>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue4.Text = rangeValue4.Text.Replace("<ReportProfile_Status5>", Resource.Resource.ReportProfile_Status5);

                IRange rangeValue5 = worksheet.FindFirst("<ReportProfile_Status1>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<ReportProfile_Status1>", Resource.Resource.ReportProfile_Status1);

                IRange rangeValue6 = worksheet.FindFirst("<ReportProfile_Status2>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<ReportProfile_Status2>", Resource.Resource.ReportProfile_Status2);

                IRange rangeValue7 = worksheet.FindFirst("<ReportProfile_Status3>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<ReportProfile_Status3>", Resource.Resource.ReportProfile_Status3);
                IRange rangeValue8 = worksheet.FindFirst("<ReportProfile_Status4>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<ReportProfile_Status4>", Resource.Resource.ReportProfile_Status4);

                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in list
                                  select new
                                  {
                                      a.LableName,
                                      a.Count1,
                                      a.Count2,
                                      a.Count3,
                                      a.Count4,
                                      a.Count5
                                  }).ToList();
                var count = listExport.Count();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 6].CellStyle.WrapText = true;

                //Initialize chart
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = Syncfusion.XlsIO.ExcelChartType.Column_Stacked;

                //Assign data
                int counAll = count + 27;
                chart.DataRange = worksheet["A27:E" + counAll + ""];
                chart.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = Resource.Resource.ChartProcess_Title;

                //Set Legend
                chart.HasLegend = true;
                chart.Legend.Position = ExcelLegendPosition.Bottom;

                //Positioning the chart in the worksheet
                chart.TopRow = 3;
                chart.LeftColumn = 1;
                chart.RightColumn = 7;
                chart.BottomRow = 25;

                workbook.SaveAs(FullPath);
                workbook.Close();

                if (export == 2)
                {
                    ExcelEngine excelEngine2 = new ExcelEngine();
                    IApplication application2 = excelEngine2.Excel;
                    application2.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                    application2.ChartToImageConverter = new ChartToImageConverter();

                    //Open existing workbook with data entered
                    IWorkbook workbook2 = application2.Workbooks.Open(FullPath, ExcelOpenType.Automatic);
                    IWorksheet worksheet2 = workbook2.Worksheets[0];

                    pathExport += ".pdf";
                    ExcelToPdfConverter converter = new ExcelToPdfConverter(worksheet2);
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument = converter.Convert();
                    pdfDocument.Save(FullPathPDF);
                    pdfDocument.Close();
                    converter.Dispose();
                }
                else
                {
                    pathExport += ".xlsx";
                }
                return pathExport;
            }
        }
    }
}
