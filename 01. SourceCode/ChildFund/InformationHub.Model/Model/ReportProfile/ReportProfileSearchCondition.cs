using InformationHub.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ReportProfileSearchCondition : SearchConditionBase
    {
        public string Age { get; set; }
        public int? Gender { get; set; }
        public int? InformationSources { get; set; }
        public string ChildName { get; set; }
        public string ProviderName { get; set; }
        public string CaseLocation { get; set; }
        public string CurrentHealth { get; set; }
        public string AbuseId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public bool StatusStep1 { get; set; }
        public bool StatusStep2 { get; set; }
        public bool StatusStep3 { get; set; }
        public bool StatusStep4 { get; set; }
        public bool StatusStep5 { get; set; }
        public bool StatusStep6 { get; set; }
        public int? SeverityLevel { get; set; }
        public int Export { get; set; }
        public string UserId { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
    }
}
