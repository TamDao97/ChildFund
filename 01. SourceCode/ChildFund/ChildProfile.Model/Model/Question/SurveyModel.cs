using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Question
{
  public  class SurveyModel:BaseModel
    {
        public Nullable<int> OrderNumber { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ContentResult { get; set; }
        public string IsPublish { get; set; }
        public List<GroupQuestionModel> ListGroupQuestion { get; set; }
    }
}
