using ChildProfiles.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Document
{
    public class DocumentLibrarySearchCondition : SearchConditionBase
    {
        public string Id { get; set; }
        public string DocumentTyeid { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Size { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public bool? IsDisplay { get; set; }
        public string UploadBy { get; set; }
        public string UploadDateFrom { get; set; }
        public string UploadDateTo { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
