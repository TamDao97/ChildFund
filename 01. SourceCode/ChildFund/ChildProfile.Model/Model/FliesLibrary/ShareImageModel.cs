using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.FliesLibrary
{
    public class ShareImageModel:BaseModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime UploadDate { get; set; }
        public string UserId { get; set; }
        public bool isDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int ImageNumber { get; set; }
        public List<AttachmentImageModel> Files { get; set; }
    }
}
