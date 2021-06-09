using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Statistic
{
    public class ChildDataByWardSearchModel : SearchConditionBase
    {
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string UserId { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
    }
}
