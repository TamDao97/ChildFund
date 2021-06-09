using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.SearchResults
{
    public class FileAttachSearchResult
    {
        public string Id { get; set; }
        public string ChildId { get; set; }
        public string Name { get; set; }
        public Nullable<int> Size { get; set; }
        public string Parth { get; set; }
        public string ParthThumbnail { get; set; }
        public string Type { get; set; }
    }
}
