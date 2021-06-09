using ChildProfiles.Model.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Question
{
   public class GroupQuestionModel
    {
        public string Id { get; set; }
        public string SurveyId { get; set; }
        public string Name { get; set; }
        public List<QuestionsModel> ListQuestion { get; set; }
    }
}
