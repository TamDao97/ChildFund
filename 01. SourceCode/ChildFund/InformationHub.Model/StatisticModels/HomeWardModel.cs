using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
  public  class HomeWardModel
    {
        public string SDateFrom { get; set; }
        public string SDateTo { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string UserId { get; set; }
        public int Type { get; set; }
        public int? Month { get; set; }
        public int? Quatar { get; set; }
        public int? Year { get; set; }
        public int? MonthFrom { get; set; }
        public int? MonthTo { get; set; }
        public int Export { get; set; }
    }
}
