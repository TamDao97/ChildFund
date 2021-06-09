using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ProfileAttachmentModel
    {
        public string Id { get; set; }
       // public string ReportProfileId { get; set; }
      //  public string SupportPlantId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public Nullable<int> Size { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string UploadBy { get; set; }
        public string UploadDate { get; set; }
        public DateTime? UploadDateRoot { get; set; }
    }
    public class FileModel
    {
        public string Name { get; set; }
        public List<ProfileAttachmentModel> ListProfileAttachment { get; set; }
    }
}
