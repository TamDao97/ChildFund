using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.AreaUser
{
    public class AreaUserModel :BaseModel
    {
        public bool IsActivate { get; set; }
        public string Manager { get; set; }
        public string Description { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public List<string> ListWard { get; set; }
        public List<string> ListDistrict { get; set; }

    }
}
