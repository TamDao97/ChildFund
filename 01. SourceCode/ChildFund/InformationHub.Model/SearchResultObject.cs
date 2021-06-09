using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.SearchResults
{
    public class SearchResultObject<T>
    {
        public int TotalItem { get; set; }

        public List<T> ListResult = new List<T>();
        public List<string> ListId = new List<string>();

        public string PathFile { get; set; }
        //exten
        public int TotalItemStatus6 { get; set; }
        public int TotalItemStatus1 { get; set; }
    }
}
