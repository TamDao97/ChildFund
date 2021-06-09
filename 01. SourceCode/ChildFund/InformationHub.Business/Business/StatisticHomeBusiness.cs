using InformationHub.Model;
using InformationHub.Model.Repositories;
using InformationHub.Model.StatisticModels;
using NTS.Common;
using NTS.Common.Utils;
using NTS.Utils;

//using Spire.Xls;
using Syncfusion.ExcelChartToImageConverter;
using Syncfusion.ExcelToPdfConverter;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace InformationHub.Business.Business
{
    public class StatisticHomeBusiness
    {
        private InformationHubEntities db = new InformationHubEntities();

        public List<HomeProvinceModel> SearchHomeProvince(LoginProfileModel userInfo, int year)
        {
            List<HomeProvinceModel> searchResult = new List<HomeProvinceModel>();
            try
            {
                var model = (from a in db.ReportProfiles.AsNoTracking()
                             where a.ReceptionDate.Value.Year == year && a.IsDelete == false && a.IsPublish == true
                             select new HomeProvinceModel
                             {
                                 Id = a.Id,
                                 StatusStep6 = a.StatusStep6,// a.ProcessingStatus,
                                 ReceptionDate = a.ReceptionDate.Value,
                                 AbuseIds = (from r in db.ReportProfileAbuseTypes
                                            where r.ReportProfileId.Equals(a.Id)
                                            select r.AbuseTypeId).ToList(),
                                 ProvinceId = a.ProvinceId,
                                 DistrictId = a.DistrictId,
                                 FinishDate = a.FinishDate,
                                 WardId = a.WardId,
                             }).AsQueryable();
                if (userInfo.Type == Constants.LevelOffice)
                {
                    model = model.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId));
                }
                if (userInfo.Type == Constants.LevelArea)
                {
                    model = model.Where(u => u.DistrictId.Equals(userInfo.DistrictId));
                }
                searchResult = model.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }
        
        public List<HomeProvinceModel> GetReportWard(ReportWardModel model)
        {
            var userInfo = new AuthorizeBusiness().GetCacheLoginProfile(model.UserId);
            List<HomeProvinceModel> searchResult = new List<HomeProvinceModel>();
            try
            {
                var modelResult = (from a in db.ReportProfiles.AsNoTracking()
                                   where a.IsDelete == false
                                   select new HomeProvinceModel
                                   {
                                       Id = a.Id,
                                       ReceptionDate = a.ReceptionDate.Value,
                                       AbuseIds = (from r in db.ReportProfileAbuseTypes
                                                   where r.ReportProfileId.Equals(a.Id)
                                                   select r.AbuseTypeId).ToList(),
                                       WardId = a.WardId,
                                       DistrictId = a.DistrictId,
                                       ProvinceId = a.ProvinceId,
                                       IsPublish = a.IsPublish,
                                       FinishDate = a.FinishDate,
                                   }).AsQueryable();

                if (model.FromYear != null)
                {
                    modelResult = modelResult.Where(u => u.ReceptionDate.Year >= model.FromYear);
                }
                if (model.ToYear != null)
                {
                    modelResult = modelResult.Where(u => u.ReceptionDate.Year <= model.ToYear);
                }
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    modelResult = modelResult.Where(u => u.WardId.Equals(userInfo.WardId));
                }
                else if (userInfo.Type == Constants.LevelArea)
                {
                    modelResult = modelResult.Where(u => u.DistrictId.Equals(userInfo.DistrictId) && u.IsPublish == true);
                }
                else if (userInfo.Type == Constants.LevelOffice)
                {
                    modelResult = modelResult.Where(u => u.ProvinceId.Equals(userInfo.ProvinceId) && u.IsPublish == true);
                }
                else if (userInfo.Type == Constants.LevelAdmin)
                {
                    modelResult = modelResult.Where(u => u.IsPublish == true);
                }
                searchResult = modelResult.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }

            return searchResult;
        }
        public List<ReportProfileSearchResult> HomeWard(LoginProfileModel userInfo)
        {
            List<ReportProfileSearchResult> searchResult = new List<ReportProfileSearchResult>();
            try
            {
                var dateNow = DateTime.Now;
                var modelResult = (from a in db.ReportProfiles.AsNoTracking()
                                   where a.ReceptionDate.Value.Year == dateNow.Year && a.IsDelete == false
                                   select new ReportProfileSearchResult()
                                   {
                                       Id = a.Id,
                                       InformationSources = a.InformationSources,
                                       ChildName = a.ChildName,
                                       ChildBirthdate = a.ChildBirthdate,
                                       Gender = a.Gender,
                                       Age = a.Age,
                                       FullAddress = a.FullAddress,
                                       SeverityLevel = a.SeverityLevel,
                                       ReceptionDate = a.ReceptionDate,
                                       WardId = a.WardId,
                                       ListAbuse = (from ar in db.ReportProfileAbuseTypes
                                                    where ar.ReportProfileId.Equals(a.Id)
                                                    select new ComboboxResult
                                                    { Id = ar.AbuseTypeId, Name = ar.AbuseTypeName }).ToList()
                                   }).AsQueryable();
                if (userInfo.Type == Constants.LevelTeacher)
                {
                    modelResult = modelResult.Where(u => u.WardId.Equals(userInfo.WardId));
                }
                searchResult = modelResult.OrderByDescending(u => u.ReceptionDate).ToList();
                foreach (var item in searchResult)
                {
                    item.InformationSourcesView = Common.GenSource(item.InformationSources);
                    item.SeverityLevelView = Common.GenLevel(item.SeverityLevel);
                    item.GenderView = Common.GenGender(item.Gender);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }
        //ham mơi dung syncfusion
        public string ExportReportFile(List<TableReportWardModel> list, ReportWardModel model)
        {
            string title = "(" + Resource.Resource.Year_Title + " " + model.FromYear + " - " + model.ToYear + ")";

            // var dateNow = DateTime.Now.ToString("yyyyMM");
            string rs = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.ReportProfile_Report_Title);
            string FullPath = HttpContext.Current.Server.MapPath(rs + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(rs + ".pdf");
            string pathFileTemplate = HttpContext.Current.Server.MapPath("~/Template/ReportWardTemp.xlsx");

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathFileTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.ReportProfile_Report_Title.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<TitleSub>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<TitleSub>", title);

                IRange rangeValue3 = worksheet.FindFirst("<ReportLeft>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<ReportLeft>", Resource.Resource.ReportProfile_ByAbuse);

                IRange rangeValue5 = worksheet.FindFirst("<Year>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Year>", Resource.Resource.Year_Title);

                IRange rangeValue6 = worksheet.FindFirst("<Type1>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue6.Text = rangeValue6.Text.Replace("<Type1>", Resource.Resource.Abuse_Type_01);

                IRange rangeValue7 = worksheet.FindFirst("<Type2>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue7.Text = rangeValue7.Text.Replace("<Type2>", Resource.Resource.Abuse_Type_02);

                IRange rangeValue8 = worksheet.FindFirst("<Type3>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue8.Text = rangeValue8.Text.Replace("<Type3>", Resource.Resource.Abuse_Type_03);

                IRange rangeValue9 = worksheet.FindFirst("<Type4>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue9.Text = rangeValue9.Text.Replace("<Type4>", Resource.Resource.Abuse_Type_04);

                IRange rangeValue9a = worksheet.FindFirst("<Type5>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue9a.Text = rangeValue9a.Text.Replace("<Type5>", Resource.Resource.Other_Type);

                IRange rangeValue10 = worksheet.FindFirst("<CountAll>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue10.Text = rangeValue10.Text.Replace("<CountAll>", Resource.Resource.Count_Title);

                #region[bang trai]
                int total = list.Count;
                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                //             worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in list
                                  select new
                                  {
                                      a1 = a.Year.ToString(),
                                      a.Count1,
                                      a.Count2,
                                      a.Count3,
                                      a.Count4,
                                      a.Count5,
                                      a.CountAll
                                  }).ToList();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 7].CellStyle.WrapText = true;
                #endregion

                #region[bieu do duong]
                //IChartShape chart2 = worksheet.Charts.Add();
                //chart2.ChartType = Syncfusion.XlsIO.ExcelChartType.Line;
                //chart2.DataRange = worksheet["A25:E" + (25 + total) + ""];
                //chart2.IsSeriesInRows = false;                //Set Chart Title
                //chart2.ChartTitle = Resource.Resource.ReportProfile_ChartStatus_Title;
                //chart2.HasLegend = true;
                //chart2.Legend.Position = ExcelLegendPosition.Bottom;                //Set Datalabels
                //IChartSerie serie1 = chart2.Series[0];
                //serie1.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //serie1.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //chart2.TopRow = 5;
                //chart2.LeftColumn = 1;
                //chart2.BottomRow = 22;
                //chart2.RightColumn = 7;
                //Initialize chart
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = ExcelChartType.Line;

                //Assign data
                chart.DataRange = worksheet["A25:F" + (25 + total) + ""];
                chart.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = InformationHub.Resource.Resource.Report_Statistic_Title;

                //Set Legend
                chart.HasLegend = true;
                chart.Legend.Position = ExcelLegendPosition.Bottom;

                //Set Datalabels
                IChartSerie serie1 = chart.Series[0];
                //IChartSerie serie2 = chart.Series[1];
                //IChartSerie serie3 = chart.Series[2];

                serie1.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //       serie2.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //       serie3.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie1.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //      serie2.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //      serie3.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;

                //Positioning the chart in the worksheet
                chart.TopRow = 5;
                chart.LeftColumn = 1;
                chart.BottomRow = 23;
                chart.RightColumn = 9;
                #endregion
                workbook.SaveAs(FullPath);
                workbook.Close();
                if (model.Export == 2)
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
        public List<ReportHistory> GetReportHistory(LoginProfileModel userInfo, HomeWardModel model)
        {
            List<ReportHistory> searchResult = new List<ReportHistory>();
            try
            {
                var searchModel = (from u in db.ReportHistories.AsNoTracking() where u.WardId.Equals(userInfo.WardId) orderby u.CreateDate descending select u).AsQueryable();
                if (model.DateFrom != null)
                {
                    searchModel = searchModel.Where(u => u.CreateDate >= model.DateFrom);
                }
                if (model.DateTo != null)
                {
                    searchModel = searchModel.Where(u => u.CreateDate <= model.DateTo);
                }
                searchResult = searchModel.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(Resource.Resource.ErroProcess_Title, ex.InnerException);
            }
            return searchResult;
        }

        //bieu d theo năm
        public string ExportReportByYear(List<TableReportWardModel> list, ReportWardModel model)
        {
            string title = "(" + Resource.Resource.Year_Title + " " + model.FromYear + " - " + model.ToYear + ")";

            // var dateNow = DateTime.Now.ToString("yyyyMM");
            string rs = "/Template/Export/" + Common.ConvertNameToTag(Resource.Resource.ReportProfile_Report_Title);
            string FullPath = HttpContext.Current.Server.MapPath(rs + ".xlsx");
            string FullPathPDF = HttpContext.Current.Server.MapPath(rs + ".pdf");
            string pathFileTemplate = HttpContext.Current.Server.MapPath("~/Template/ReportByYearTemp.xlsx");

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = Syncfusion.XlsIO.ExcelVersion.Excel2016;
                application.ChartToImageConverter = new ChartToImageConverter();
                //Open existing workbook with data entered
                IWorkbook workbook = application.Workbooks.Open(pathFileTemplate, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                IRange rangeValue = worksheet.FindFirst("<Title>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue.Text = rangeValue.Text.Replace("<Title>", Resource.Resource.ReportProfile_Report_Title.ToUpper());

                IRange rangeValue2 = worksheet.FindFirst("<TitleSub>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue2.Text = rangeValue2.Text.Replace("<TitleSub>", title);

                IRange rangeValue3 = worksheet.FindFirst("<ReportLeft>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue3.Text = rangeValue3.Text.Replace("<ReportLeft>", Resource.Resource.ReportByYear_Title);

                IRange rangeValue5 = worksheet.FindFirst("<Year>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue5.Text = rangeValue5.Text.Replace("<Year>", Resource.Resource.Year_Title);

                IRange rangeValue10 = worksheet.FindFirst("<CountAll>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                rangeValue10.Text = rangeValue10.Text.Replace("<CountAll>", Resource.Resource.ReportProfile_NumberChartStatus_Title);

                #region[bang trai]
                int total = list.Count;
                IRange iRangeData = worksheet.FindFirst("<Data>", ExcelFindType.Text, Syncfusion.XlsIO.ExcelFindOptions.MatchCase);
                //             worksheet.InsertRow(iRangeData.Row + 1, 3, ExcelInsertOptions.FormatAsBefore);
                var listExport = (from a in list
                                  select new
                                  {
                                      a1 = a.Year.ToString(),
                                      a.CountAll
                                  }).ToList();
                worksheet.ImportData(listExport, iRangeData.Row, iRangeData.Column, false);
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 2].Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 2].Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 2].Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 2].Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 2].Borders.Color = ExcelKnownColors.Black;
                worksheet.Range[iRangeData.Row - 1, 1, iRangeData.Row + total - 1, 2].CellStyle.WrapText = true;
                #endregion

                #region[bieu do duong]
                IChartShape chart = worksheet.Charts.Add();
                chart.ChartType = ExcelChartType.Line;

                //Assign data
                chart.DataRange = worksheet["A25:B" + (25 + total) + ""];
                chart.IsSeriesInRows = false;

                //Apply chart elements
                //Set Chart Title
                chart.ChartTitle = InformationHub.Resource.Resource.Report_Statistic_Title;

                //Set Legend
                chart.HasLegend = true;
                chart.Legend.Position = ExcelLegendPosition.Bottom;

                //Set Datalabels
                IChartSerie serie1 = chart.Series[0];
                //IChartSerie serie2 = chart.Series[1];
                //IChartSerie serie3 = chart.Series[2];

                serie1.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //       serie2.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                //       serie3.DataPoints.DefaultDataPoint.DataLabels.IsValue = true;
                serie1.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //      serie2.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;
                //      serie3.DataPoints.DefaultDataPoint.DataLabels.Position = ExcelDataLabelPosition.Center;

                //Positioning the chart in the worksheet
                chart.TopRow = 5;
                chart.LeftColumn = 1;
                chart.BottomRow = 23;
                chart.RightColumn = 9;
                #endregion
                workbook.SaveAs(FullPath);
                workbook.Close();
                if (model.Export == 2)
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
    }
}



