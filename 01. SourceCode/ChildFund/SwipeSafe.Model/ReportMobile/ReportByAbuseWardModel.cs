using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ReportByAbuseWardModel
    {
        public string AbuseName { get; set; }
        public string AbuseId { get; set; }
        public int Count { get; set; }
    }
    public class ReportByAbuseWardSearch
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string WardId { get; set; }
    }
}
