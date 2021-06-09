
using NTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTS.Common;
using NTS.Common.Utils;
using ChildProfiles.Model.AreaUser;
using ChildProfiles.Model.Entity;
using ChildProfiles.Model;
using ChildProfiles.Model.Model.Homes;
using ChildProfiles.Model.Model.CacheModel;
using NTS.Caching;
using System.Configuration;
using ChildProfiles.Business.Business;
using Syncfusion.XlsIO;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.ExcelChartToImageConverter;
using System.Web;

namespace ChildProfiles.Business
{
    public class HomesBusiness
    {
        private ChildProfileEntities db = new ChildProfileEntities();
        public List<HomesResultModel> SearchProfiles(HomesModel searchCondition, LoginProfileModel userInfo)
        {
            List<HomesResultModel> searchResult = new List<HomesResultModel>();
            try
            {
                var listmodel = (from a in db.ChildProfiles.AsNoTracking()
                                 where a.IsDelete == Constants.IsUse
                                 && a.UpdateDate.Year == searchCondition.Year
                                 && (string.IsNullOrEmpty(searchCondition.ProvinceId) || a.ProvinceId.Equals(searchCondition.ProvinceId))
                                 join b in db.Ethnics.AsNoTracking() on a.EthnicId equals b.Id
                                 select new HomesResultModel()
                                 {
                                     Status = a.ProcessStatus,
                                     NationId = a.EthnicId,
                                     DistrictId = a.DistrictId,
                                     WardId = a.WardId,
                                     NationName = b.Name,
                                     LeaningStatus = a.LeaningStatus,
                                     CreateDate = a.CreateDate,
                                     Gender = a.Gender,
                                     Birthday = a.DateOfBirth
                                 }).AsQueryable();
                if (!string.IsNullOrEmpty(searchCondition.DistrictId))
                {
                    listmodel = listmodel.Where(r => r.DistrictId.Equals(searchCondition.DistrictId));
                }
                else
                {
                    if (!userInfo.UserLever.Equals(Constants.LevelAdmin) && !userInfo.UserLever.Equals(Constants.LevelOffice))
                    {
                        List<string> lstDistrictId = new List<string>();
                        if (!string.IsNullOrEmpty(searchCondition.UserId))
                        {
                            lstDistrictId = (from a in db.AreaUsers.AsNoTracking()
                                             where a.Id.Equals(userInfo.AreaUserId)
                                             join c in db.AreaDistricts.AsNoTracking() on a.Id equals c.AreaUserId
                                             where (string.IsNullOrEmpty(userInfo.DistrictId) || c.DistrictId.Equals(userInfo.DistrictId))
                                             select c.DistrictId).ToList();
                        }
                        listmodel = listmodel.Where(r => lstDistrictId.Contains(r.DistrictId));
                    }
                }
                searchResult = listmodel.ToList();

            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.SearchProfiles", ex.Message, searchCondition);
                throw new Exception(ErrorMessage.ERR001, ex.InnerException);
            }
            return searchResult;
        }

        public List<NotifyModel> GetNotify(string userId)
        {
            RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
            List<NotifyModel> lst = new List<NotifyModel>();
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                lst = redisService.GetContains(cacheNotify + userId + ":*");
            }
            catch (Exception ex)
            { LogUtils.ExceptionLog("DocumentBussiness.GetNotify", ex.Message, userId); }
            return lst;
        }

        public void DeleteNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + System.Web.HttpContext.Current.User.Identity.Name + ":" + id;
                redisService.Remove(key);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.DeleteNotify", ex.Message, id);
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }
        public void TickNotify(string id)
        {
            try
            {
                string cacheNotify = ConfigurationManager.AppSettings["cacheNotify"] + "NotifyInfo:";
                RedisService<NotifyModel> redisService = RedisService<NotifyModel>.GetInstance();
                var key = cacheNotify + System.Web.HttpContext.Current.User.Identity.Name + ":" + id;
                NotifyModel notify = redisService.Get<NotifyModel>(key);
                notify.Status = Constants.ViewNotification;
                redisService.Replace(key, notify);
            }
            catch (Exception ex)
            {
                LogUtils.ExceptionLog("DocumentBussiness.TickNotify", ex.Message, id);
                throw new Exception("Xảy ra lỗi vui lòng thử lại");
            }
        }

        public List<Ward> GetWard(List<string> WardId)
        {
            var ward = db.Wards.AsNoTracking().Where(i => WardId.Contains(i.Id)).ToList();
            return ward;
        }

        public string ExportReport(List<LearningModel> lstLearningModel, List<LearningModel> dataNation, List<LearningModel> dataGender, List<AgeModel> listAge, List<int> lstprofileConfim, List<int> lstprofileUnConfim)
        {

            string pathExport = "/Template/Export/school-status";
            string FullPath = HttpContext.Current.Server.MapPath(pathExport + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(pathExport + ".pdf");
            string pathTemplate = HttpContext.Current.Server.MapPath("~/Template/SchoolStatus.xlsx");
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                IWorksheet worksheet2 = workbook.Worksheets[1];
                IWorksheet worksheet3 = workbook.Worksheets[2];
                IWorksheet worksheet4 = workbook.Worksheets[3];
                IWorksheet worksheet5 = workbook.Worksheets[4];


                #region[biểu đồ sheet1]
                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in lstLearningModel
                                  select new
                                  {
                                      a1 = a.Name,
                                      a.Percen,
                                      a.Count,
                                  }).ToList();
                var count = listExport.Count();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 3].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + count - 1, 3].CellStyle.WrapText = true;
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = Syncfusion.XlsIO.ExcelChartType.Pie;
                //Assign data
                chart.DataRange = worksheet["A28:B" + (28 + lstLearningModel.Count) + ""];
                chart.IsSeriesInRows = false;
                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = "HỒ SƠ TRẺ ĐI HỌC/BY SCHOOL STATUS";
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
                #endregion
                #region[biểu đồ sheet2]
                IRange iRangeData2 = worksheet2.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet2.InsertRow(iRangeData2.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport2 = (from a in dataNation
                                   select new
                                   {
                                       a1 = a.Name,
                                       a.Percen,
                                       a.Count,
                                   }).ToList();
                var count2 = listExport2.Count();
                worksheet2.ImportData(listExport2, iRangeData2.Row, iRangeData2.Column, false);
                worksheet2.Range[iRangeData2.Row - 1, 1, iRangeData2.Row + count2 - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range[iRangeData2.Row - 1, 1, iRangeData2.Row + count2 - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range[iRangeData2.Row - 1, 1, iRangeData2.Row + count2 - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range[iRangeData2.Row - 1, 1, iRangeData2.Row + count2 - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet2.Range[iRangeData2.Row - 1, 1, iRangeData2.Row + count2 - 1, 3].Borders.Color = ExcelKnownColors.Black;
                worksheet2.Range[iRangeData2.Row - 1, 1, iRangeData2.Row + count2 - 1, 3].CellStyle.WrapText = true;
                IChartShape chart2 = worksheet2.Charts.Add();
                chart2.ChartType = Syncfusion.XlsIO.ExcelChartType.Pie;
                //Assign data
                chart2.DataRange = worksheet2["A28:B" + (28 + dataNation.Count) + ""];
                chart2.IsSeriesInRows = false;
                //Apply chart2 elements
                //Set chart2 Title
                chart2.ChartTitle = "HỒ SƠ TRẺ THEO XÃ/BY COMMUNE";
                //Set Legend
                chart2.HasLegend = true;
                chart2.Legend.Position = ExcelLegendPosition.Bottom;
                //Set Datalabels
                IChartSerie serie2 = chart2.Series[0];
                serie2.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie2.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.BestFit;
                //Positioning the chart2 in the worksheet
                chart2.TopRow = 3;
                chart2.LeftColumn = 1;
                chart2.RightColumn = 6;
                chart2.BottomRow = 25;
                #endregion

                #region[biểu đồ sheet3]
                IRange iRangeData3 = worksheet3.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet3.InsertRow(iRangeData3.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport3 = (from a in dataGender
                                   select new
                                   {
                                       a1 = a.Name,
                                       a.Percen,
                                       a.Count,
                                   }).ToList();
                var count3 = listExport3.Count();
                worksheet3.ImportData(listExport3, iRangeData3.Row, iRangeData3.Column, false);
                worksheet3.Range[iRangeData3.Row - 1, 1, iRangeData3.Row + count3 - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet3.Range[iRangeData3.Row - 1, 1, iRangeData3.Row + count3 - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet3.Range[iRangeData3.Row - 1, 1, iRangeData3.Row + count3 - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet3.Range[iRangeData3.Row - 1, 1, iRangeData3.Row + count3 - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet3.Range[iRangeData3.Row - 1, 1, iRangeData3.Row + count3 - 1, 3].Borders.Color = ExcelKnownColors.Black;
                worksheet3.Range[iRangeData3.Row - 1, 1, iRangeData3.Row + count3 - 1, 3].CellStyle.WrapText = true;
                IChartShape chart3 = worksheet3.Charts.Add();
                chart3.ChartType = Syncfusion.XlsIO.ExcelChartType.Pie;
                //Assign data
                chart3.DataRange = worksheet3["A28:B" + (28 + dataGender.Count) + ""];
                chart3.IsSeriesInRows = false;
                //Apply chart2 elements
                //Set chart2 Title
                chart3.ChartTitle = "HỒ SƠ TRẺ THEO GIỚI TÍNH/BY GENDER";
                //Set Legend
                chart3.HasLegend = true;
                chart3.Legend.Position = ExcelLegendPosition.Bottom;
                //Set Datalabels
                IChartSerie serie3 = chart3.Series[0];
                serie3.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie3.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.BestFit;
                //Positioning the chart2 in the worksheet
                chart3.TopRow = 3;
                chart3.LeftColumn = 1;
                chart3.RightColumn = 6;
                chart3.BottomRow = 25;
                #endregion

                #region[biểu đồ sheet4]
                IRange iRangeData4 = worksheet4.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet4.InsertRow(iRangeData4.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport4 = (from a in listAge
                                   select new
                                   {
                                       a1 = a.Name,
                                       a.Percen,
                                       a.Count,
                                   }).ToList();
                var count4 = listExport4.Count();
                worksheet4.ImportData(listExport4, iRangeData4.Row, iRangeData4.Column, false);
                worksheet4.Range[iRangeData4.Row - 1, 1, iRangeData4.Row + count4 - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet4.Range[iRangeData4.Row - 1, 1, iRangeData4.Row + count4 - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet4.Range[iRangeData4.Row - 1, 1, iRangeData4.Row + count4 - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet4.Range[iRangeData4.Row - 1, 1, iRangeData4.Row + count4 - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet4.Range[iRangeData4.Row - 1, 1, iRangeData4.Row + count4 - 1, 3].Borders.Color = ExcelKnownColors.Black;
                worksheet4.Range[iRangeData4.Row - 1, 1, iRangeData4.Row + count4 - 1, 3].CellStyle.WrapText = true;
                IChartShape chart4 = worksheet4.Charts.Add();
                chart4.ChartType = Syncfusion.XlsIO.ExcelChartType.Pie;
                //Assign data
                chart4.DataRange = worksheet4["A28:B" + (28 + listAge.Count) + ""];
                chart4.IsSeriesInRows = false;
                //Apply chart2 elements
                //Set chart2 Title
                chart4.ChartTitle = "HỒ SƠ TRẺ THEO TUỔI/BY AGE";
                //Set Legend
                chart4.HasLegend = true;
                chart4.Legend.Position = ExcelLegendPosition.Bottom;
                //Set Datalabels
                IChartSerie serie4 = chart4.Series[0];
                serie4.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie4.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.BestFit;
                //Positioning the chart2 in the worksheet
                chart4.TopRow = 3;
                chart4.LeftColumn = 1;
                chart4.RightColumn = 6;
                chart4.BottomRow = 25;
                #endregion
                #region[biểu đồ sheet5]

                List<string> lstName = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                List<LearningModel> model = new List<LearningModel>();
                LearningModel item;
                for (int i = 0; i < lstName.Count; i++)
                {
                    item = new LearningModel();
                    item.Name = lstName[i];
                    item.Count = lstprofileUnConfim[i];
                    item.CountConfim = lstprofileConfim[i];
                    model.Add(item);
                }
                var year = DateTime.Now;
                IRange rangeValue = worksheet5.FindFirst("<year>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<year>", year.Year + "");
                IRange rangeValue2 = worksheet5.FindFirst("<year2>", ExcelFindType.Text, ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<year2>", year.Year + "");

                IRange iRangeData5 = worksheet5.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                worksheet5.InsertRow(iRangeData5.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport5 = (from a in model
                                   select new
                                   {
                                       a1 = a.Name,
                                       a.CountConfim,
                                       a.Count,
                                   }).ToList();
                var count5 = listExport5.Count();
                worksheet5.ImportData(listExport5, iRangeData5.Row, iRangeData5.Column, false);
                worksheet5.Range[iRangeData5.Row - 1, 1, iRangeData5.Row + count5 - 1, 3].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet5.Range[iRangeData5.Row - 1, 1, iRangeData5.Row + count5 - 1, 3].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet5.Range[iRangeData5.Row - 1, 1, iRangeData5.Row + count5 - 1, 3].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet5.Range[iRangeData5.Row - 1, 1, iRangeData5.Row + count5 - 1, 3].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet5.Range[iRangeData5.Row - 1, 1, iRangeData5.Row + count5 - 1, 3].Borders.Color = ExcelKnownColors.Black;
                worksheet5.Range[iRangeData5.Row - 1, 1, iRangeData5.Row + count5 - 1, 3].CellStyle.WrapText = true;

                //Initialize chart
                IChartShape chart5 = worksheet5.Charts.Add();
                chart5.ChartType = Syncfusion.XlsIO.ExcelChartType.Column_Clustered;

                //Assign data
                chart5.DataRange = worksheet5["A28:C40"];
                chart5.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart5.ChartTitle = "CHILD PROFILE DATA IN " + year.Year;

                //Set Legend
                chart5.HasLegend = true;
                chart5.Legend.Position = ExcelLegendPosition.Bottom;

                //Positioning the chart2 in the worksheet
                chart5.TopRow = 3;
                chart5.LeftColumn = 1;
                chart5.RightColumn = 6;
                chart5.BottomRow = 25;
                #endregion
                workbook.SaveAs(FullPath);
                workbook.Close();
                pathExport += ".xlsx";
                return pathExport;
            }
        }


    }
}
