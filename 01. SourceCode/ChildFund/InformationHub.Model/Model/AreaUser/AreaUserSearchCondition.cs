using InformationHub.Model.SearchCondition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.AreaUser
{
    public class AreaUserSearchCondition : SearchConditionBase
    {
        public string Name { get; set; }
        public bool? IsActivate { get; set; }
        public string Manager { get; set; }
    }
}
