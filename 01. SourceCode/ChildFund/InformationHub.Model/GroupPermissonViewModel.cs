using InformationHub.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class GroupPermissonViewModel
    {
        public string Index { get; set; }
        public string GroupUserId { get; set; }
        public string GroupFunctionId { get; set; }
        public string PermissionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ItemChecked { get; set; }
        public List<Permission> ListPermission { get; set; }
    }
}
