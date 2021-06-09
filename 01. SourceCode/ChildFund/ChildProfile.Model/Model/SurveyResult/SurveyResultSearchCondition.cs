using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.SurveyResult
{
    public class SurveyResultSearchCondition : SearchConditionBase
    {
        public string SurveyId { get; set; }
        public string UserName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
