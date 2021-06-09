using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.ReportProfile
{
    public class SupportAfterStatusModel
    {
        public string Id { get; set; }
        public string ReportProfileId { get; set; }
        public Nullable<System.DateTime> PerformingDate { get; set; }
        public string PerformingBy { get; set; }
        public Nullable<int> LevelHarm { get; set; }
        public Nullable<int> LevelApproach { get; set; }
        public Nullable<int> LevelCareObstacle { get; set; }
        public Nullable<int> TotalLevelHigh { get; set; }
        public Nullable<int> TotalLevelAverage { get; set; }
        public Nullable<int> TotalLevelLow { get; set; }
        public Nullable<int> AbilityProtectYourself { get; set; }
        public Nullable<int> AbilityKnowGuard { get; set; }
        public Nullable<int> AbilityHelpOthers { get; set; }
        public Nullable<int> TotalAbilityHigh { get; set; }
        public Nullable<int> TotalAbilityAverage { get; set; }
        public Nullable<int> TotalAbilityLow { get; set; }
        public string Result { get; set; }
        public string CreateBy { get; set; }
        public string SupportAfterTitle { get; set; }
        public string LevelHarmNote { get; set; }
        public string LevelApproachNote { get; set; }
        public string LevelCareObstacleNote { get; set; }
        public string AbilityProtectYourselfNote { get; set; }
        public string AbilityKnowGuardNote { get; set; }
        public string AbilityHelpOthersNote { get; set; }
        public bool IsExport { get; set; }
        public string ChildName { get; set; }
    }
}
