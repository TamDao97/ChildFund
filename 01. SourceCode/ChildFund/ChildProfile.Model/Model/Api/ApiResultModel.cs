using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.Model.Api
{
    public class ApiResultModel<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<T> Data { get; set; }
    }
}
