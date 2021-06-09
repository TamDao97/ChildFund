using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.ProfileReport
{
    public class ProfileChildAbuseModel
    {
        public string Id { get; set; }
        public string ProfileChildId { get; set; }
        public string AbuseId { get; set; }
        public string AbuseName { get; set; }
    }
}
