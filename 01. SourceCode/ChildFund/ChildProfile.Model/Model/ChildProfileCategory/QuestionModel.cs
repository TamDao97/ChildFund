using ChildProfiles.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
   public class QuestionModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string OtherAnswer { get; set; }
        public string MoreAnswer { get; set; }
        public List<CheckBoxModel> Answers { get; set; } 
    }
}
