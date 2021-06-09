using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Homes
{
   public class HomesResultModel
    {
        public string Status { get; set; }
        public string NationId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string NationName { get; set; }
        public string LeaningStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? Birthday { get; set; }
        public int Gender { get; set; }
    }
}
