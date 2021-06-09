using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.ChildProfileModels
{
    public class ChildProfileSearchResult
    {
        public string Avata { get; set; }
        public string Id { get; set; }
        public string childId { get; set; }
        public string Name { get; set; }
        public string ReligionName { get; set; }
        public string ProgramCode { get; set; }
        public string NationName { get; set; }
        public string School { get; set; }
        public string Status { get; set; }
        public string ChildCode { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string SalesforceID { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string ApproverName { get; set; }
        public Nullable<System.DateTime> OfficeApproveDate { get; set; }
        public string OfficeApproveBy { get; set; }
        public string CreateBy { get; set; }
        public string UserId { get; set; }
        public string StoryContent { get; set; }
        public string SchoolName { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool Handicap { get; set; }
        public bool HealthHandicap { get; set; }
    }
}
