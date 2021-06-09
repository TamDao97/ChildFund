using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model
{
    public class BaseSearch
    {
        public int PageSize { get; set; } 
        public int PageNumber { get; set; } 
        public string OrderBy { get; set; }
        public bool OrderType { get; set; } 
        public DateTime? DateFrom { get; set; } 
        public DateTime? DateTo { get; set; }
    }
}
