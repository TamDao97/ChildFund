using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationHub.Model.Model.ReportProfile
{
    public class CaseVerificationModel
    {
        public string Id { get; set; }
        public string ReportProfileId { get; set; }
        public Nullable<System.DateTime> PerformingDate { get; set; }
        public string PerformingBy { get; set; }
        public string Condition { get; set; }
        public string FamilySituation { get; set; }
        public string CurrentQualityCareOK { get; set; }
        public string CurrentQualityCareNG { get; set; }
        public string PeopleCareFuture { get; set; }
        public string FutureQualityCareOK { get; set; }
        public string FutureQualityCareNG { get; set; }
        public Nullable<int> LevelHarm { get; set; }
        public Nullable<int> LevelApproach { get; set; }
        public Nullable<int> LevelDevelopmentEffect { get; set; }
        public Nullable<int> LevelCareObstacle { get; set; }
        public Nullable<int> LevelNoGuardian { get; set; }//thua
        public Nullable<int> TotalLevelHigh { get; set; }
        public Nullable<int> TotalLevelAverage { get; set; }
        public Nullable<int> TotalLevelLow { get; set; }
        public Nullable<int> AbilityProtectYourself { get; set; }
        public Nullable<int> AbilityKnowGuard { get; set; }
        public Nullable<int> AbilityEstablishRelationship { get; set; }
        public Nullable<int> AbilityRelyGuard { get; set; }
        public Nullable<int> AbilityHelpOthers { get; set; }//thua
        public Nullable<int> TotalAbilityHigh { get; set; }
        public Nullable<int> TotalAbilityAverage { get; set; }
        public Nullable<int> TotalAbilityLow { get; set; }
        public string Result { get; set; }
        public string ProblemIdentify { get; set; }
        public string ChildAspiration { get; set; }
        public string FamilyAspiration { get; set; }
        public string ServiceNeeds { get; set; }
        public string CreateBy { get; set; }
        public string LevelHarmNote { get; set; }
        public string LevelApproachNote { get; set; }
        public string LevelDevelopmentEffectNote { get; set; }
        public string LevelCareObstacleNote { get; set; }
        public string LevelNoGuardianNote { get; set; }
        public string AbilityProtectYourselfNote { get; set; }
        public string AbilityKnowGuardNote { get; set; }
        public string AbilityEstablishRelationshipNote { get; set; }
        public string AbilityRelyGuardNote { get; set; }
        public string AbilityHelpOthersNote { get; set; }
        public string Extend { get; set; }
        public bool IsExport { get; set; }
    }
}
