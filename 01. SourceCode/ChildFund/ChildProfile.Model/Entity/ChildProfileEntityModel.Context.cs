﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChildProfileEntities : DbContext
    {
        public ChildProfileEntities()
            : base("name=ChildProfileEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<AreaDistrict> AreaDistricts { get; set; }
        public virtual DbSet<AreaUser> AreaUsers { get; set; }
        public virtual DbSet<AreaWard> AreaWards { get; set; }
        public virtual DbSet<AttachFileReport> AttachFileReports { get; set; }
        public virtual DbSet<AttachmentImage> AttachmentImages { get; set; }
        public virtual DbSet<ChildProfile> ChildProfiles { get; set; }
        public virtual DbSet<ChildProfileUpdate> ChildProfileUpdates { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<DocumentLibrary> DocumentLibraries { get; set; }
        public virtual DbSet<DocumentTye> DocumentTyes { get; set; }
        public virtual DbSet<Ethnic> Ethnics { get; set; }
        public virtual DbSet<ImageChildByYear> ImageChildByYears { get; set; }
        public virtual DbSet<ImageChildHistory> ImageChildHistories { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<ReportContent> ReportContents { get; set; }
        public virtual DbSet<ReportProfile> ReportProfiles { get; set; }
        public virtual DbSet<School> Schools { get; set; }
        public virtual DbSet<ShareImage> ShareImages { get; set; }
        public virtual DbSet<StoryTemplate> StoryTemplates { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyResult> SurveyResults { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Village> Villages { get; set; }
        public virtual DbSet<Ward> Wards { get; set; }
    }
}
