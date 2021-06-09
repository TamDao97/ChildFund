using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.SwipeSafeModel
{
    public class ChildModel
    {
        public string Id { get; set; }
        public string ReportId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Level { get; set; }
        public string Address { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string FullAddress { get; set; }
        public Nullable<System.DateTime> DateAction { get; set; }
        
        /// <summary>
        /// Hành vi xam hại
        /// </summary>
        public List<ChildAbuseModel> ListAbuse { get; set; }
    }
}
