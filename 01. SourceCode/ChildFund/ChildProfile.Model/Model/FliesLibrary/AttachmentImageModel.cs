using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.FliesLibrary
{
    public class AttachmentImageModel:BaseModel
    { 
        public string Id { get; set; }
        public string ShareImageId { get; set; }
        public string Name { get; set; }
        public int? FileType { get; set; }
        public string ImagePath { get; set; }
        public string ImageThumbnailPath { get; set; }
        public int? SizeBase { get; set; }
        public string UploadBy { get; set; }
        public bool isDelete { get; set; }
        public DateTime UploadDate { get; set; }
    }
    public class UploadAttachFileModel
    {
        public string Id { get; set; }
        public List<string> ListIdRemote { get; set; }
    }
}
