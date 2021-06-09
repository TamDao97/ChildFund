using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ReportByAreaProvinceModel
    {
        public string DistrictName { get; set; }
        public string DistrictId { get; set; }
        public int Count { get; set; }
    }
    public class ReportByAreaProvinceSearch
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
    }
}
