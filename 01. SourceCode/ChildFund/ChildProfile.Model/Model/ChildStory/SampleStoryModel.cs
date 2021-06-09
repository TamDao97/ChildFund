using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.ChildStory
{
    public class SampleStoryModel : BaseModel
    { 
        public string Type { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public string SalesforceID { get; set; }
        public string Status { get; set; } 
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public List<CategoryModel> Category { get; set; }
        
    }
}
