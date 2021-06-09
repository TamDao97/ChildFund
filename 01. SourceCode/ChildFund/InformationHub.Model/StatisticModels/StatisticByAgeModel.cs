using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ResultStatisticByAge
    {
        public List<StatisticByAgeModel> ListStatisticByAgeModel { get; set; }
        public List<LoaiHinh> AgeValue { get; set; }
        public List<ChartAgeModel> ChartData { get; set; }
        public List<ChartAgeModel> ChartRightData { get; set; }
        public string PathFile { get; set; }
    }
    public class StatisticByAgeModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public int? Age { get; set; }
        public int LableName { get; set; }
        public string AgeValue { get; set; }
        public bool? IsPublish { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Count6 { get; set; }
        public int Count7 { get; set; }
        public int Count8 { get; set; }
        public int Count9 { get; set; }
        //public DateTime? CreateDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
    }
    public class LoaiHinh
    {
        public string LableName { get; set; }
        public int ValueFrom { get; set; }
        public int ValueTo { get; set; }
        public string AgeValue { get; set; }
    }
    public class AbuseTypeByAge
    {
        public string TypeName { get; set; }
        public List<int> Count { get; set; }
    }
    public class ChartAgeModel
    {
        public string Age { get; set; }
        public double Percent { get; set; }
    }
}
