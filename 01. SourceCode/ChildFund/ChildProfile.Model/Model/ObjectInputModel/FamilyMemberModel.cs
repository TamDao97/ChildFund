using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model
{
    public class FamilyMemberModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên thành viên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        public string Dateb { get; set; }

        /// <summary>
        /// Mối quan hệ
        /// </summary>
        public string RelationshipId { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Nghề nghiệp
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// Sống với trẻ
        /// </summary>
        public int LiveWithChild { get; set; }
    }
}
