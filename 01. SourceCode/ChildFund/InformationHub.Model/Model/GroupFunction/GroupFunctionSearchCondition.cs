using InformationHub.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.GroupFunction
{
    class GroupFunctionSearchCondition: SearchConditionBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
