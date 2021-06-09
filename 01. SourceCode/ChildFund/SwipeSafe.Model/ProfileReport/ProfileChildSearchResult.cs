using SwipeSafe.Model.Repositories;
using SwipeSafe.Model.SwipeSafeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ProfileChildSearchResult
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
        public IEnumerable<ProfileChildAbuseModel> FormAbuse { get; set; }

        public string FormAbuseName { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }

        public int ProcessingStatus { get; set; }
        public string ProcessingName { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public string ReceptionDateView { get; set; }

        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }
        public string ProviderAddress { get; set; }
        public string ProviderNote { get; set; }
        public List<ProcessingContentModel> ListProcessingContent { get; set; }
    }
}
