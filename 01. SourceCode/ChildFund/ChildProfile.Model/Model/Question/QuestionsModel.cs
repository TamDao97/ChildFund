using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Question
{
    public class QuestionsModel 
    {
        public string Id { get; set; }
        public string GroupQuestionId { get; set; }
        public string Contents { get; set; }//noi dung cau hoi
        public string Result { get; set; }//noi dung dap an
        public string Type { get; set; }//0 la cau hoi dang chon, 1 la cau hoi dang van ban
        public List<AnswerModel> ListAnswer { get; set; }

    }
}
