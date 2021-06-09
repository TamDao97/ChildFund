using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.CacheModel
{
    public class NotifyModel
    {
        public DateTime CreateDate { get; set; }
        public string Id { get; set; }
        public string IdPlan { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Addres { get; set; }
        public string Status { get; set; }
        public string Link { get; set; }
    }
    public class NotifySearchModel
    {
        public int? PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
