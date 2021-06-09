using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{

    public class StatisticByGenderModelChart
    {
      public  List<int> countTotal { get; set; }
        public string Label { get; set; }

    }



    public class StatisticGenderWardModel
    {
        public string Type { get; set; }
        public int CountNam { get; set; }
        public int CountNu { get; set; }
        public int CountKhong { get; set; }
        public DateTime? CreateDate { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public int? Gender { get; set; }
    }

}
