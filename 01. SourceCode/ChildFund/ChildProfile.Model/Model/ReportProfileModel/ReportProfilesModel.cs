using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
    public class ReportProfilesModel : BaseModel
    {
        public string Content { get; set; }
        public string ChildProfileId { get; set; }
        public string ProcessStatus { get; set; }
        public string AreaApproverBy { get; set; }
        public string AreaApproverNotes { get; set; }
        public DateTime? AreaApproverDate { get; set; }
        public string OfficeApproveBy { get; set; }
        public DateTime? OfficeApproveDate { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Description { get; set; }
    }
}
