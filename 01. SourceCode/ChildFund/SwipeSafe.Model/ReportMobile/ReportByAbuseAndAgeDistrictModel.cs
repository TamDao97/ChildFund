﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ReportByAbuseAndAgeDistrictModel
    {
        public string AbuseName { get; set; }
        public string AbuseId { get; set; }
        public int? Age { get; set; }
        public int Count { get; set; }
        public List<int?> ListType { get; set; }
    }
    public class ReportByAbuseAndAgeDistrictSearch
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
    }
}
