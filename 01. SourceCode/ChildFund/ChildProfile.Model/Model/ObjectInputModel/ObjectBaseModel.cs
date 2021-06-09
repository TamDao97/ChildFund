using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
    public class ObjectBaseModel
    {
        public List<ObjectInputModel> ListObject { get; set; }
        /// <summary>
        /// Tên nội dung khác
        /// </summary>
        public string OtherName { get; set; }
        /// <summary>
        /// Giá trị khác
        /// </summary>
        public string OtherValue { get; set; }

        /// <summary>
        /// Tên nội dung khác nữa
        /// </summary>
        public string OtherName2 { get; set; }
        /// <summary>
        /// Giá khác nữa
        /// </summary>
        public string OtherValue2 { get; set; }
    }
}
