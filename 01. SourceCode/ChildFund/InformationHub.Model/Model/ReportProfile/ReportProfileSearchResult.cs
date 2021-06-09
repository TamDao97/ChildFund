using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ReportProfileSearchResult
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public int InformationSources { get; set; }
        public string InformationSourcesView { get; set; }
        public string ReceptionTime { get; set; }
        public Nullable<System.DateTime> ReceptionDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string ChildName { get; set; }
        public Nullable<System.DateTime> ChildBirthdate { get; set; }
        public Nullable<int> Gender { get; set; }
        public string GenderView { get; set; }
        public Nullable<int> Age { get; set; }
        public string CaseLocation { get; set; }
        public string FullAddress { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        // public string CurrentHealth { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderNote { get; set; }
        public bool? StatusStep1 { get; set; }
        public bool? StatusStep2 { get; set; }
        public bool? StatusStep3 { get; set; }
        public bool? StatusStep4 { get; set; }
        public bool? StatusStep5 { get; set; }
        public bool? StatusStep6 { get; set; }
        public bool? IsDelete { get; set; }
        public bool? IsPublish { get; set; }
        public Nullable<int> SeverityLevel { get; set; }
        public string SeverityLevelView { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string ForwardNote { get; set; }
        public string AbuseNote { get; set; }
        public string AbuseTitle { get; set; }
        public string ForwardLevel { get; set; }
        public string Status { get; set; }
        public string TypeOther { get; set; }
        public List<ComboboxResult> ListAbuse { get; set; }
    }
}
