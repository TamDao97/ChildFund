using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
    public class StatisticByProcessingModel
    {
        public string AreaId {get;set;}
        public string LableName { get; set; }
        public string AbuseTypeName { get; set; }
        public string AbuseId { get; set; }
        public bool StatusStep1 { get; set; }
        public bool StatusStep2 { get; set; }
        public bool StatusStep3 { get; set; }
        public bool StatusStep4 { get; set; }
        public bool StatusStep5 { get; set; }
        public bool StatusStep6 { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public bool? IsPublish { get; set; }
    }
    public class ProcessingStatus
    {
        public int Status { get; set; }
        public string StatusLable { get; set; }
    }
    public class ChartModel
    {
        public List<int> Count { get; set; }
        public string Lable { get; set; }
    }
    public class ReturnProcessingModel
    {
        public List<StatisticByProcessingModel> LstTable { get; set; }
        public List<ChartModel> LstChart { get; set; }
        public List<string> ListLocation { get; set; }
        public string PathFile { get; set; }
    }
}
