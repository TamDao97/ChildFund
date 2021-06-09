using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Homes
{
   public class HomesModel
    {
        public int Year { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public string UserId { get; set; }
        public int Export { get; set; }
    }
    public class LearningModel
    {
        public int Count { get; set; }
        public int CountConfim { get; set; }
        public string Name { get; set; }
        public string KeyValue { get; set; }
        public Double Percen { get; set; }
    }
    public class AgeModel
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public int AgeBegin  { get; set; }
        public int AgeEnd  { get; set; }
        public Double Percen { get; set; }
    }
}
