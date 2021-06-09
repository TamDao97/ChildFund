using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ReportByCountWardModel
    {
        public int Year { get; set; }
        public int Count { get; set; }
    }
    public class ReportByCountWardSearch
    {
        public int YearFrom { get; set; }
        public int YearTo { get; set; }
        public string WardId { get; set; }
    }
}
