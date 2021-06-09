using InformationHub.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.GroupUser
{
    public class GroupUserSearchCondition : SearchConditionBase
    {
        public bool? IsDisable { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
    }
}
