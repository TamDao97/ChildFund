using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.ChildProfileModels
{
    public class ChildProfileSearchCondition : SearchConditionBase
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string ChildCode { get; set; }
        public string ProgramCode { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string DateFromByADO { get; set; }
        public string DateToByADO { get; set; }
        public string DateFromByHNO { get; set; }
        public string DateToByHNO { get; set; }
        public string CreateBy { get; set; }
        public string UserId { get; set; }
        public string Level { get; set; }
        public string Status { get; set; }
        public int Export { get; set; }
        public string Address { get; set; }
    }
    public class ChildProfileExport
    {
        public bool IsProvince { get; set; }
        public List<string> ListCheck { get; set; }
    }
    public class ChildProfileExportResult
    {
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public string ChildCode { get; set; }
        public string ProgramCode { get; set; }
        public string Name { get; set; }
        public string ProvinceName { get; set; }
        public string SaleforceID { get; set; }
        public string ImageSignaturePath { get; set; }
        // public string DistrictName { get; set; }
    }
}
