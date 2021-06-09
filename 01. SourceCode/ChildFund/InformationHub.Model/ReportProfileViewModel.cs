using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model
{
    public class ReportProfileViewModel
    {
        public ReportProfileModel ReportProfile { get; set; }
        public List<ItemCombobox> ListAbuseType { get; set; }
        public List<ItemCombobox> ListProvince { get; set; }
        public List<ItemCombobox> ListDistrict { get; set; }
        public List<ItemCombobox> ListWard { get; set; }
        public List<ItemCombobox> ListJob { get; set; }
    }
}
