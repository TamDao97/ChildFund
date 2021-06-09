using ChildProfiles.Model.Model.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.SurveyResult
{
    public class SurveyResultModel
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string SurveyName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ContentResult { get; set; }
        public List<GroupQuestionModel> ListGroupQuestion { get; set; }
    }
}
