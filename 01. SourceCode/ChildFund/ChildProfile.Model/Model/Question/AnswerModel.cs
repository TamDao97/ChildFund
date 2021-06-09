using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Question
{
    public class AnswerModel
    {
        public string Id { get; set; }
        public string Type { get; set; }//0 la loai cau hoi bt, 1 la loai cau hoi khac
        public string Other { get; set; }//noi dung cau hoi khac
        public string Answer { get; set; }//dap an
        public bool Check { get; set; }//ket qua dc chon
        public int? CountSelect { get; set; }//dem ket qua dc chon
        public List<ComboboxResult> ListUser { get; set; }
    }

    public class AnswerModelTemp
    {
        //gianh cho them moi
        public List<AnswerModel> lstReult { get; set; }
        public string type { get; set; }
        public string idq { get; set; }
    }
}
