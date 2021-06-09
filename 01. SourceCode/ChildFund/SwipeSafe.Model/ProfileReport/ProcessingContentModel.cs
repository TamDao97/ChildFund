using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ProcessingContentModel
    {
        public string Id { get; set; }
        public string ProfileChildId { get; set; }
        public string Content { get; set; }
        public DateTime ProcessingDate { get; set; }
        public string ProcessingDateView { get; set; }
        public string ProcessingBy { get; set; }
    }
}
