using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.SearchCondition
{
    public class SearchConditionBase
    {
        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string OrderBy { get; set; }

        public bool OrderType { get; set; }
        public string Lang { get; set; }
    }
}
