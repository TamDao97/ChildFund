using SwipeSafe.Model.SwipeSafeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ProfileChildModel
    {
        public string Id { get; set; }
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
        public int ProcessingStatus { get; set; }
        public Nullable<int> SeverityLevel { get; set; }
        public Nullable<System.DateTime> FinishDate { get; set; }
        public string FinishNote { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public string ClosedNote { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ReportId { get; set; }

        /// <summary>
        /// Hành vi xam hại
        /// </summary>
        public List<ChildAbuseModel> ListAbuse { get; set; }
    }
}
