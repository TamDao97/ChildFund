using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
    public class StatisticByLocationModel
    {
        public string AreaId {get;set;}
        public string AbuseName { get; set; }
        public string AbuseId { get; set; }
        public string LableName { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public bool? IsPublish { get; set; }
    }
    public class LocationChartModel
    {
        public string TypeName { get; set; }
        public List<int> Count { get; set; }
    }
    public class ReturnLocationModel
    {
        public List<StatisticByLocationModel> LstTable { get; set; }
        public List<LocationChartModel> LstChart { get; set; }
        public List<string> ListLocation { get; set; }
        public List<string> LstType { get; set; }
        public string PathFile { get; set; }
    }
}
