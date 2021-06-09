using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeSafe.Model.SwipeSafeModel
{
    public class ReportModel
    {
        /// <summary>
        /// Thông tin người báo cáo
        /// </summary>
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Relationship { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// Danh sách trẻ bị xâm hại
        /// </summary>
        public List<ChildModel> ListChild { get; set; }

        /// <summary>
        /// Danh sách nghi phạm
        /// </summary>
        public List<PrisonerModel> ListPrisoner { get; set; }
    }
}
