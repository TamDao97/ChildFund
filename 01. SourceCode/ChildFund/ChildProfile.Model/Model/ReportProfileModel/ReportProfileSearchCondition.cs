using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.ReportProfileModel
{
    public class ReportProfileSearchCondition : SearchConditionBase
    {
        public string Name { get; set; }
        public string ChildCode { get; set; }
        public string Status { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CreateBy { get; set; }
        public string UserId { get; set; }
    }
}
