using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.ReportProfile
{
    public class EvaluationFirstModel
    {
        public string Id { get; set; }
        public string ReportProfileId { get; set; }
        public Nullable<System.DateTime> PerformingDate { get; set; }
        public Nullable<int> LevelHarm { get; set; }
        public Nullable<int> LevelHarmContinue { get; set; }
        public Nullable<int> TotalLevelHigh { get; set; }
        public Nullable<int> TotalLevelAverage { get; set; }
        public Nullable<int> TotalLevelLow { get; set; }
        public Nullable<int> AbilityProtectYourself { get; set; }
        public Nullable<int> AbilityReceiveSupport { get; set; }
        public Nullable<int> TotalAbilityHigh { get; set; }
        public Nullable<int> TotalAbilityAverage { get; set; }
        public Nullable<int> TotalAbilityLow { get; set; }
        public string Result { get; set; }
        public string UnitProvideLiving { get; set; }
        public string UnitProvideCare { get; set; }
        public string CreateBy { get; set; }
        public string ServiceProvideLiving { get; set; }
        public string ServiceProvideCare { get; set; }
        public string LevelHarmNote { get; set; }
        public string LevelHarmContinueNote { get; set; }
        public string AbilityProtectYourselfNote { get; set; }
        public string AbilityReceiveSupportNote { get; set; }
        public bool IsExport { get; set; }
    }
}
