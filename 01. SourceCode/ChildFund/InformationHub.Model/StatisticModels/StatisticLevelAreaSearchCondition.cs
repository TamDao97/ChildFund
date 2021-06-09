using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.StatisticModels
{
    public class StatisticLevelAreaSearchCondition
    {
        /// <summary>
        /// Loại thống kê theo ngày,tháng,năm,quý
        /// </summary>
        public string StatisticType { get; set; }
        /// <summary>
        /// Tỉnh
        /// </summary>
        public string ProvinceId { get; set; }
        /// <summary>
        /// Huyện
        /// </summary>
        public string DistrictId { get; set; }
        /// <summary>
        /// Xã
        /// </summary>
        public string WardId { get; set; }
        /// <summary>
        /// Từ ngày
        /// </summary>
        public string DateFrom { get; set; }
        /// <summary>
        ///  Đến ngày
        /// </summary>
        public string DateTo { get; set; }
        /// <summary>
        /// Loại xuất file
        /// </summary>
        public int Export { get; set; }
    }
}
