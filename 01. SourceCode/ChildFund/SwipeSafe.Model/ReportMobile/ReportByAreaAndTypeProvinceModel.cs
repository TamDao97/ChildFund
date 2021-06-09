using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ReportByAreaAndTypeProvinceModel
    {
        public string WardName { get; set; }
        public string WardId { get; set; }
        public int Type { get; set; }
        public int Count { get; set; }
        public List<int> ListType { get; set; }
    }
    public class ReportByAreaAndTypeProvinceSearch
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
    }
}
