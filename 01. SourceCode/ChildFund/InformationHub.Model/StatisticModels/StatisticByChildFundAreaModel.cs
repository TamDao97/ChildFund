using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
    public class StatisticByChildFundAreaModel
    {
        public string AreaId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string AbuseName { get; set; }
        public string AbuseId { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Total { get; set; }
        public bool? IsPublish { get; set; }
    }
    public class ChildFundAreaChartModel
    {
        public List<int> Count { get; set; }
        public string Lable { get; set; }
    }
    public class ReturnChildFundAreaModel
    {
        public List<StatisticByChildFundAreaModel> LstTable { get; set; }
        public List<ChildFundAreaChartModel> LstChart { get; set; }
        public List<string> LstArea { get; set; }
    }
}
