using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.ReportProfile
{
   public class SupportPlantModel
    {
        public string Id { get; set; }
        public string ReportProfileId { get; set; }
        public Nullable<System.DateTime> PlantDate { get; set; }
        public string OrganizationActivities { get; set; }
        public string CreateBy { get; set; }
        public string TitlePlant { get; set; }
        public string TargetNote { get; set; }
        public string ActionNote { get; set; }
        public bool? IsEstimateCost { get; set; }
        public List<OrganizationActivitiesModel> ListOrganizationActivities { get; set; }
        public List<ProfileAttachmentModel> ListProfileAttachment { get; set; }
        public List<ProfileAttachmentModel> ListProfileAttachmentUpdate { get; set; }
        public bool IsExport { get; set; }
    }
    public class OutputModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
    }
}
