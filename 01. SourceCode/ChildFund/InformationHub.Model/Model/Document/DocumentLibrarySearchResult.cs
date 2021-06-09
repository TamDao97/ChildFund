using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.Document
{
    public class DocumentLibrarySearchResult
    {
        public string Id { get; set; }
        public string DocumentTyeid { get; set; }
        public string DocumentTypeidView { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? Size { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public bool IsDisplay { get; set; }
        public string UploadBy { get; set; }
        public DateTime UploadDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
