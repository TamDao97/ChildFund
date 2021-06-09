using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ChildProcessingContentModel
    {
        public string ChildName { get; set; }
        public string FullAddress { get; set; }
        public List<ProcessingContentModel> ListProcessingContent { get; set; }
    }
}
