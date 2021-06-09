using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Question
{
    public class QuestionSearchCondition : SearchConditionBase
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
