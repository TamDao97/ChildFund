using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.Permission
{
    class PermissionSearchResult
    {
        public string Id { get; set; }
        public string GroupFunctionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool TypeLevel1 { get; set; }
        public bool TypeLevel2 { get; set; }
        public bool TypeLevel3 { get; set; }
        public bool TypeLevel4 { get; set; }
    }
}
