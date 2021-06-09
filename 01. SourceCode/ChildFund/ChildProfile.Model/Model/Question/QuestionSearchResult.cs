using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Question
{
    public class QuestionSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IsPublish { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int CountResult { get; set; }

    }
}
