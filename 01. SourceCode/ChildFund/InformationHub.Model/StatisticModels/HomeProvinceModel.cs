using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
  public  class HomeProvinceModel
    {
        public string Id { get; set; }
        public bool? StatusStep6 { get; set; }
        public DateTime ReceptionDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string AbuseName { get; set; }
        public List<string> AbuseIds = new List<string>();
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public bool? IsPublish { get; set; }
    }
    public class HomeProvinceLeftModel
    {
        public string Name  { get; set; }
        public int CountAll  { get; set; }
        public int CountByAbuse1  { get; set; }
        public int CountByAbuse2  { get; set; }
        public int CountByAbuse3  { get; set; }
        public int CountByAbuse4  { get; set; }
        public int CountByAbuse5 { get; set; }

    }
    public class HomeProvinceItemModel
    {
        public string AbuseId { get; set; }
        public string LableName { get; set; }
        public int Count { get; set; }
        public int CountBefore { get; set; }
        public double Percen { get; set; }
        public double PercenChart { get; set; }
    }
    public class HomeChartItemModelByQuatar
    {
      //  public int ProcessingStatus { get; set; }
        public string LableName { get; set; }
        public List<int> ListCount { get; set; }
        public string ListStr { get; set; }
    }
    public class HomeChartItemModelByMonthYear
    {
        public string DateSearch { get; set; }
        public string LableName { get; set; }
        public int CountStatus0 { get; set; }
        public int CountStatus1 { get; set; }
        public int CountStatus2 { get; set; }
        public int CountStatus3 { get; set; }
        public int CountStatus4 { get; set; }
    }

    public class ChartReportWardModel
    {
        public List<int> Count { get; set; }
        public string AbuseType { get; set; }
        public string AbuseId { get; set; }
    }
    public class TableReportWardModel
    {
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Percent1 { get; set; }
        public int Percent2 { get; set; }
        public int Percent3 { get; set; }
        public int Percent4 { get; set; }
        public int Percent5 { get; set; }
        public int Year { get; set; }
        public int CountAll { get; set; }
    }
    public class ReportWardModel
    {
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }
        public string UserId { get; set; }
        public int Export { get; set; }
    }
}
