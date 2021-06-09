//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChildProfiles.Model.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChildProfileUpdate
    {
        public string Id { get; set; }
        public string ChildProfileId { get; set; }
        public System.DateTime InfoDate { get; set; }
        public string EmployeeName { get; set; }
        public string ProgramCode { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string ChildCode { get; set; }
        public string SchoolId { get; set; }
        public string SchoolOtherName { get; set; }
        public string EthnicId { get; set; }
        public string ReligionId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public int Gender { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string LeaningStatus { get; set; }
        public string ClassInfo { get; set; }
        public string FavouriteSubject { get; set; }
        public string LearningCapacity { get; set; }
        public string Housework { get; set; }
        public string Health { get; set; }
        public string Personality { get; set; }
        public string Hobby { get; set; }
        public string Dream { get; set; }
        public string FamilyMember { get; set; }
        public string LivingWithParent { get; set; }
        public string NotLivingWithParent { get; set; }
        public string LivingWithOther { get; set; }
        public string LetterWrite { get; set; }
        public string HouseType { get; set; }
        public string HouseRoof { get; set; }
        public string HouseWall { get; set; }
        public string HouseFloor { get; set; }
        public string UseElectricity { get; set; }
        public string SchoolDistance { get; set; }
        public string ClinicDistance { get; set; }
        public string WaterSourceDistance { get; set; }
        public string WaterSourceUse { get; set; }
        public string RoadCondition { get; set; }
        public string IncomeFamily { get; set; }
        public string HarvestOutput { get; set; }
        public string NumberPet { get; set; }
        public string FamilyType { get; set; }
        public string TotalIncome { get; set; }
        public string IncomeSources { get; set; }
        public string IncomeOther { get; set; }
        public string ImagePath { get; set; }
        public string ImageThumbnailPath { get; set; }
        public string ProcessStatus { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ConsentName { get; set; }
        public string ConsentRelationship { get; set; }
        public string ConsentVillage { get; set; }
        public string ConsentWard { get; set; }
        public string SiblingsJoiningChildFund { get; set; }
        public string Malformation { get; set; }
        public string Orphan { get; set; }
        public Nullable<int> EmployeeTitle { get; set; }
        public string ImageSignaturePath { get; set; }
        public string ImageSignatureThumbnailPath { get; set; }
        public string SaleforceId { get; set; }
        public Nullable<bool> Handicap { get; set; }
        public Nullable<int> ImageSize { get; set; }
        public Nullable<System.DateTime> AreaApproverDate { get; set; }
        public string AreaApproverId { get; set; }
    
        public virtual ChildProfile ChildProfile { get; set; }
    }
}
