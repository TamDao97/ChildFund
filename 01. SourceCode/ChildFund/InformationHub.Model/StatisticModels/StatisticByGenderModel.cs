using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class StatisticByGenderModel
    {
        public string TypeId { get; set; }
        public string Type { get; set; }
        public int CountNam { get; set; }
        public int CountNu { get; set; }
        public int CountKhong { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public int? Gender { get; set; }
        public bool? IsPublish { get; set; }
    }
    public class GenderChartModel
    {
        public List<int> Count { get; set; }
        public string Label { get; set; }
    }
    public class ListGender
    {
        public int Gender { get; set; }
        public string GenderType { get; set; }
    }
    public class GenderResultModel
    {
        public List<StatisticByGenderModel> LstTable { get; set; }
        public List<GenderChartModel> LstChart { get; set; }
        public List<string> LstAbuse { get; set; }
        public string PathFile { get; set; }
    }
}
