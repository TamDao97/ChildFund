using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.SearchResults
{
    public class SearchResultObject<T>
    {
        public int TotalItem { get; set; }

        public List<T> ListResult = new List<T>();

        public string PathFile { get; set; }
    }
}
