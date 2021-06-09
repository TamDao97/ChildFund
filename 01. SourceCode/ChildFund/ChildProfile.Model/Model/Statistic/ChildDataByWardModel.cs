using ChildProfiles.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Statistic
{
    public class ChildDataByWardModel
    {
        public string WardName { get; set; }
        public int ChildTotal { get; set; }
        public List<ChildProfile> ChildData { get; set; }
        public int? Size { get; set; }
        public string SizeString { get; set; }
    }
}
