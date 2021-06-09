using SwipeSafe.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ProfileChildSearchCondition : SearchConditionBase
    {
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string Name { get; set; }
        public int? ProcessingStatus { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
