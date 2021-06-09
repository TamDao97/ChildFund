using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
    public class ObjectInputModel
    {
        /// <summary>
        /// Id 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Trạng thái check cho trường hợp chọn
        /// </summary>
        public bool Check { get; set; }

        /// <summary>
        /// Tên hiển thị tiếng việt
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tên hiển thị tiếng anh
        /// </summary>
        public string NameEN { get; set; }

        /// <summary>
        /// Giá trị cho trường hợp là nhập text
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Lable giá trị khác
        /// </summary>
        public string OtherName { get; set; }

        /// <summary>
        /// Nội dung giá trị khác
        /// </summary>
        public string OtherValue { get; set; }

        /// <summary>
        /// Câu hỏi yes/no
        /// </summary>
        public string Question { get; set; }
        public bool YesValue { get; set; }
        public bool NoValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Enabled { get; set; }
    }
}
