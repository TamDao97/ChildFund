using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.SearchResults
{
    public class ReportSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Relationship { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string StatusView { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public int CountChild { get; set; }
        public int CountPrisoner { get; set; }
        public List<PrisonerSearchResult> ListPrisonerSearchResult { get; set; }
        public List<ChildSearchResult> ListChildSearchResult { get; set; }
        public List<FileAttachSearchResult> ListFile { get; set; }

    }
}
