using SwipeSafe.Model.SwipeSafeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.SearchResults
{
    public class ChildSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public int? Age { get; set; }
        public string Level { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public Nullable<System.DateTime> DateAction { get; set; }
        public List<ChildAbuseModel> FormAbuse { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
    }
}
