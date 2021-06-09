using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
    public class StatisticByMostAbuseModel
    {
        public List<ProvinceByMostAbuse> LstProvince { get; set; }
        public List<DistrictByMostAbuse> LstDistrict { get; set; }
        public List<WardByMostAbuse> LstWard { get; set; }
        public List<string> LstAbuse { get; set; }
    }
    public class ProvinceByMostAbuse
    {
        public int Index { get; set; }
        public string LableName { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Total { get; set; }
    }
    public class WardByMostAbuse
    {
        public int Index { get; set; }
        public string LableName { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Total { get; set; }
    }
    public class DistrictByMostAbuse
    {
        public int Index { get; set; }
        public string LableName { get; set; }
        public int Count1 { get; set; }
        public int Count2 { get; set; }
        public int Count3 { get; set; }
        public int Count4 { get; set; }
        public int Count5 { get; set; }
        public int Total { get; set; }
    }
}
