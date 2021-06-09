using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
    public class StatisticSearchCondition
    {
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int FromYear { get; set; }
        public int ToYear { get; set; }
        public int Export { get; set; }
        public int ClickYear { get; set; }
        public string AbuseId { get; set; }
    }
}
