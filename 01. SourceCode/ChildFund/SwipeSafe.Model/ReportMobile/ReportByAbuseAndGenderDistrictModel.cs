
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ReportByAbuseAndGenderDistrictModel
    {
        public string AbuseName { get; set; }
        public string AbuseId { get; set; }
        public int? Gender { get; set; }
        public int Count { get; set; }
        public List<int?> ListType { get; set; }
    }
    public class ReportByAbuseAndGenderDistrictSearch
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
    }
}
