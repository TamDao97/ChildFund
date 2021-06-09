using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.ChildProfileModels
{
    public class ChildProfileDownloadSearch
    {
        public List<string> ListChildProfileId { get; set; }
        public ChildProfileDownloadSearch()
        {
            ListChildProfileId = new List<string>();    
        }
    }
}
