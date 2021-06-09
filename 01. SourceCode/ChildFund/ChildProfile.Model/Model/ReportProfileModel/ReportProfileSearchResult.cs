using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.ReportProfileModel
{
    public class ReportProfileSearchResult
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ChildCode { get; set; }
        public string ProgramCode { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string ApproverId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ApproveDate { get; set; }
        public string CreateBy { get; set; }
        public string Description { get; set; }
        public string AreaApproverNotes { get; set; }
        public int CountFile { get; set; }

        public bool Handicap { get; set; }
        public bool HealthHandicap { get; set; }

    }
}
