using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
   public class SubjectCapacityModel
    {
        public string Index { get; set; }
        public string IndexC1 { get; set; }
        public bool CheckC1 { get; set; }
        public string IndexC2 { get; set; }
        public bool CheckC2 { get; set; }

        public string NameSubjectC1 { get; set; }
        public string NameSubjectC2 { get; set; }
        public string NameCapacity { get; set; }
        public bool CheckC3 { get; set; }
    }
}
