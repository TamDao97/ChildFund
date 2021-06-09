using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InformationHub.Model
{
    public class ReportProfileModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public List<ComboboxResult> ListAbuseType { get; set; }
        public int InformationSources { get; set; }
        public string ReceptionTime { get; set; }
        public Nullable<System.DateTime> ReceptionDate { get; set; }
        public string ChildName { get; set; }
        public Nullable<System.DateTime> ChildBirthdate { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<int> Age { get; set; }
        public string CaseLocation { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public string FullAddress { get; set; }
        public string CurrentHealth { get; set; }
        public string SequelGuess { get; set; }
        public string FatherName { get; set; }
        public Nullable<int> FatherAge { get; set; }
        public string FatherJob { get; set; }
        public string MotherName { get; set; }
        public Nullable<int> MotherAge { get; set; }
        public string MotherJob { get; set; }
        public string FamilySituation { get; set; }
        public string PeopleCare { get; set; }
        public string Support { get; set; }
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
        public Nullable<int> SeverityLevel { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string FinishNote { get; set; }
        public string WordTitle { get; set; }
        public string SourceNote { get; set; }
        public string SummaryCase { get; set; }
        public string TypeOther { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }

        /// <summary>
        /// Danh sách tài liệu đính kèm
        /// </summary>
        public List<ProfileAttachmentModel> ListProfileAttachment { get; set; }
        public List<ProfileAttachmentModel> ListProfileAttachmentUpdate { get; set; }

        public bool IsExport { get; set; }
    }
    public class ReportForwardModel
    {
        public string Id { get; set; }
        public string ForwardNote { get; set; }
    }
}
