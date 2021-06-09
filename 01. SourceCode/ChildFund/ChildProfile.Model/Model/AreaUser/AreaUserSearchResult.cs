using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.AreaUser
{
    public class AreaUserSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProvinceName { get; set; }
        public string Description { get; set; }
        public bool IsActivate { get; set; }
        public string Manager { get; set; }
        public int CountDistrict { get; set; }
        public int CountWard { get; set; }

    }
}
