using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.GroupUser
{
    public class GroupUserSearchResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public string TypeView { get; set; }
        public bool IsDisable { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
