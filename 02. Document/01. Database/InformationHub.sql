/*
Created		9/8/2016
Modified		3/13/2020
Project		
Model			
Company		
Author		
Version		
Database		MS SQL 2005 
*/


Drop table [Notify] 
go
Drop table [ReportForward] 
go
Drop table [ReportHistory] 
go
Drop table [AreaWard] 
go
Drop table [AreaUser] 
go
Drop table [AreaDistrict] 
go
Drop table [ReportProfileAbuseType] 
go
Drop table [DocumentTye] 
go
Drop table [DocumentLibrary] 
go
Drop table [EstimateCostAttachment] 
go
Drop table [SupportPlant] 
go
Drop table [Province] 
go
Drop table [District] 
go
Drop table [Ward] 
go
Drop table [EvaluationFirst] 
go
Drop table [SupportAfterStatus] 
go
Drop table [CaseVerification] 
go
Drop table [ProfileAttachment] 
go
Drop table [AbuseType] 
go
Drop table [ReportProfile] 
go
Drop table [ActivityLog] 
go
Drop table [GroupPermission] 
go
Drop table [Permission] 
go
Drop table [GroupFunction] 
go
Drop table [UserPermission] 
go
Drop table [GroupUser] 
go
Drop table [User] 
go


Create table [User]
(
	[Id] Varchar(36) NOT NULL,
	[GroupUserId] Varchar(36) NOT NULL,
	[UserName] Nvarchar(100) NOT NULL,
	[Type] Char(1) NOT NULL,
	[AreaUserId] Varchar(36) NULL,
	[WardId] Varchar(36) NULL,
	[DistrictId] Varchar(36) NULL,
	[ProvinceId] Varchar(36) NULL,
	[FullAddress] Nvarchar(500) NULL,
	[Password] Nvarchar(36) NOT NULL,
	[PasswordHash] Nvarchar(64) NOT NULL,
	[HomeURL] Varchar(100) NULL,
	[FullName] Nvarchar(150) NULL,
	[Gender] Integer NOT NULL,
	[Birthdate] Datetime NULL,
	[Email] Nvarchar(300) NOT NULL,
	[Phone] Varchar(50) NULL,
	[IdentifyNumber] Varchar(15) NULL,
	[Address] Nvarchar(500) NULL,
	[AvatarPath] Nvarchar(256) NULL,
	[IsDisable] Bit NOT NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go

Create table [GroupUser]
(
	[Id] Varchar(36) NOT NULL,
	[Name] Nvarchar(200) NOT NULL,
	[Type] Integer NOT NULL,
	[IsDisable] Bit Default 0 NOT NULL,
	[HomeURL] Nvarchar(100) Default 0 NOT NULL,
	[Description] Nvarchar(300) NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go

Create table [UserPermission]
(
	[Id] Varchar(36) NOT NULL,
	[UserId] Varchar(36) NOT NULL,
	[PermissionId] Varchar(36) NOT NULL,
Primary Key ([Id])
) 
go

Create table [GroupFunction]
(
	[Id] Varchar(36) NOT NULL,
	[Code] Nvarchar(50) NOT NULL,
	[Name] Nvarchar(100) NOT NULL,
Primary Key ([Id])
) 
go

Create table [Permission]
(
	[Id] Varchar(36) NOT NULL,
	[GroupFunctionId] Varchar(36) NOT NULL,
	[Code] Nvarchar(50) NOT NULL,
	[Name] Nvarchar(100) NOT NULL,
	[TypeLevel1] Bit NOT NULL,
	[TypeLevel2] Bit NOT NULL,
	[TypeLevel3] Bit NOT NULL,
	[TypeLevel4] Bit NOT NULL,
Primary Key ([Id])
) 
go

Create table [GroupPermission]
(
	[Id] Varchar(36) NOT NULL,
	[GroupUserId] Varchar(36) NOT NULL,
	[PermissionId] Varchar(36) NOT NULL,
Primary Key ([Id])
) 
go

Create table [ActivityLog]
(
	[Id] Varchar(36) NOT NULL,
	[UserId] Varchar(36) NOT NULL,
	[LogDate] Datetime NOT NULL,
	[LogContent] Nvarchar(max) NOT NULL,
	[OldValue] Nvarchar(100) NULL,
	[NewValue] Nvarchar(100) NULL,
	[TableName] Varchar(100) NULL,
	[ObjectId] Varchar(36) NULL,
	[LogType] Integer NOT NULL,
Primary Key ([Id])
) 
go

Create table [ReportProfile]
(
	[Id] Varchar(36) NOT NULL,
	[InformationSources] Integer NOT NULL,
	[SourceNote] Nvarchar(500) NULL,
	[ReceptionTime] Varchar(15) NULL,
	[ReceptionDate] Datetime Default 0 NULL,
	[ChildName] Nvarchar(300) Default 0 NOT NULL,
	[ChildBirthdate] Datetime NULL,
	[Gender] Integer NULL,
	[Age] Integer NULL,
	[CaseLocation] Nvarchar(300) NULL,
	[WardId] Varchar(36) NULL,
	[DistrictId] Varchar(36) NULL,
	[ProvinceId] Varchar(36) NULL,
	[FullAddress] Nvarchar(500) NULL,
	[CurrentHealth] Nvarchar(max) NULL,
	[SequelGuess] Nvarchar(max) NULL,
	[FatherName] Nvarchar(150) NULL,
	[FatherAge] Integer NULL,
	[FatherJob] Nvarchar(250) NULL,
	[MotherName] Nvarchar(150) NULL,
	[MotherAge] Integer NULL,
	[MotherJob] Nvarchar(250) NULL,
	[FamilySituation] Nvarchar(max) NULL,
	[PeopleCare] Nvarchar(300) NULL,
	[Support] Nvarchar(max) NULL,
	[ProviderName] Nvarchar(150) NULL,
	[ProviderPhone] Nvarchar(50) NULL,
	[ProviderAddress] Nvarchar(500) NULL,
	[ProviderNote] Nvarchar(500) NULL,
	[SeverityLevel] Integer NULL,
	[FinishDate] Datetime NULL,
	[FinishNote] Nvarchar(500) NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
	[StatusStep1] Bit NULL,
	[StatusStep2] Bit NULL,
	[StatusStep3] Bit NULL,
	[StatusStep4] Bit NULL,
	[StatusStep5] Bit NULL,
	[StatusStep6] Bit NULL,
	[CloseDate] Datetime NULL,
	[IsDelete] Bit NULL,
	[WordTitle] Nvarchar(500) NULL,
	[IsPublish] Bit NULL,
	[SummaryCase] Nvarchar(max) NULL,
Primary Key ([Id])
) 
go

Create table [AbuseType]
(
	[Id] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[Note] Nvarchar(500) NULL,
	[OrderNumber] Integer NOT NULL,
Primary Key ([Id])
) 
go

Create table [ProfileAttachment]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[Name] Nvarchar(300) NOT NULL,
	[Path] Nvarchar(256) NULL,
	[Size] Integer NULL,
	[Extension] Varchar(30) NULL,
	[Description] Nvarchar(500) NULL,
	[UploadBy] Varchar(36) NOT NULL,
	[UploadDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go

Create table [CaseVerification]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[PerformingDate] Datetime NULL,
	[PerformingBy] Varchar(36) NULL,
	[Condition] Nvarchar(500) NULL,
	[FamilySituation] Nvarchar(500) NULL,
	[CurrentQualityCareOK] Nvarchar(500) NULL,
	[CurrentQualityCareNG] Nvarchar(500) NULL,
	[PeopleCareFuture] Nvarchar(500) NULL,
	[FutureQualityCareOK] Nvarchar(500) NULL,
	[FutureQualityCareNG] Nvarchar(500) NULL,
	[LevelHarm] Integer NULL,
	[LevelApproach] Integer NULL,
	[LevelDevelopmentEffect] Integer NULL,
	[LevelCareObstacle] Integer NULL,
	[LevelNoGuardian] Integer NULL,
	[TotalLevelHigh] Integer NULL,
	[TotalLevelAverage] Integer NULL,
	[TotalLevelLow] Integer NULL,
	[AbilityProtectYourself] Integer NULL,
	[AbilityKnowGuard] Integer NULL,
	[AbilityEstablishRelationship] Integer NULL,
	[AbilityRelyGuard] Integer NULL,
	[AbilityHelpOthers] Integer NULL,
	[TotalAbilityHigh] Integer NULL,
	[TotalAbilityAverage] Integer NULL,
	[TotalAbilityLow] Integer NULL,
	[Result] Nvarchar(max) NULL,
	[ProblemIdentify] Nvarchar(500) NULL,
	[ChildAspiration] Nvarchar(500) NULL,
	[FamilyAspiration] Nvarchar(500) NULL,
	[ServiceNeeds] Nvarchar(500) NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
	[LevelHarmNote] Nvarchar(500) NULL,
	[LevelApproachNote] Nvarchar(500) NULL,
	[LevelDevelopmentEffectNote] Nvarchar(500) NULL,
	[LevelCareObstacleNote] Nvarchar(500) NULL,
	[LevelNoGuardianNote] Char(500) NULL,
	[AbilityProtectYourselfNote] Nvarchar(500) NULL,
	[AbilityKnowGuardNote] Nvarchar(500) NULL,
	[AbilityEstablishRelationshipNote] Nvarchar(500) NULL,
	[AbilityRelyGuardNote] Nvarchar(500) NULL,
	[AbilityHelpOthersNote] Nvarchar(500) NULL,
Primary Key ([Id])
) 
go

Create table [SupportAfterStatus]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[PerformingDate] Datetime NULL,
	[PerformingBy] Varchar(36) NULL,
	[LevelHarm] Integer NULL,
	[LevelApproach] Integer NULL,
	[LevelCareObstacle] Integer NULL,
	[TotalLevelHigh] Integer NULL,
	[TotalLevelAverage] Integer NULL,
	[TotalLevelLow] Integer NULL,
	[AbilityProtectYourself] Integer NULL,
	[AbilityKnowGuard] Integer NULL,
	[AbilityHelpOthers] Integer NULL,
	[TotalAbilityHigh] Integer NULL,
	[TotalAbilityAverage] Integer NULL,
	[TotalAbilityLow] Integer NULL,
	[Result] Nvarchar(max) NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
	[SupportAfterTitle] Nvarchar(500) NULL,
	[LevelHarmNote] Nvarchar(500) NULL,
	[LevelApproachNote] Nvarchar(500) NULL,
	[LevelCareObstacleNote] Nvarchar(500) NULL,
	[AbilityProtectYourselfNote] Nvarchar(500) NULL,
	[AbilityKnowGuardNote] Nvarchar(500) NULL,
	[AbilityHelpOthersNote] Nvarchar(500) NULL,
Primary Key ([Id])
) 
go

Create table [EvaluationFirst]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[PerformingDate] Datetime NULL,
	[LevelHarm] Integer NULL,
	[LevelHarmNote] Nvarchar(500) NULL,
	[LevelHarmContinue] Integer NULL,
	[LevelHarmContinueNote] Nvarchar(500) NULL,
	[TotalLevelHigh] Integer NULL,
	[TotalLevelAverage] Integer NULL,
	[TotalLevelLow] Integer NULL,
	[AbilityProtectYourself] Integer NULL,
	[AbilityReceiveSupport] Integer NULL,
	[AbilityProtectYourselfNote] Nvarchar(500) NULL,
	[AbilityReceiveSupportNote] Nvarchar(500) NULL,
	[TotalAbilityHigh] Integer NULL,
	[TotalAbilityAverage] Integer NULL,
	[TotalAbilityLow] Integer NULL,
	[Result] Nvarchar(max) NULL,
	[UnitProvideLiving] Nvarchar(500) NULL,
	[UnitProvideCare] Nvarchar(500) NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
	[ServiceProvideLiving] Nvarchar(500) NULL,
	[ServiceProvideCare] Nvarchar(500) NULL,
Primary Key ([Id])
) 
go

Create table [Ward]
(
	[Id] Varchar(36) NOT NULL,
	[DistrictId] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[Type] Nvarchar(50) NULL,
	[Location] Varchar(50) NULL,
Primary Key ([Id])
) 
go

Create table [District]
(
	[Id] Varchar(36) NOT NULL,
	[ProvinceId] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[Type] Nvarchar(50) NULL,
	[Location] Varchar(50) NULL,
Primary Key ([Id])
) 
go

Create table [Province]
(
	[Id] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[Type] Nvarchar(50) NULL,
Primary Key ([Id])
) 
go

Create table [SupportPlant]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[PlantDate] Datetime NULL,
	[TitlePlant] Nvarchar(250) NOT NULL,
	[TargetNote] Nvarchar(max) NULL,
	[ActionNote] Nvarchar(max) NULL,
	[OrganizationActivities] Nvarchar(max) NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
	[IsEstimateCost] Bit NULL,
Primary Key ([Id])
) 
go

Create table [EstimateCostAttachment]
(
	[Id] Varchar(36) NOT NULL,
	[SupportPlantId] Varchar(36) NOT NULL,
	[Name] Nvarchar(300) NULL,
	[Path] Nvarchar(256) NULL,
	[Size] Integer NULL,
	[Extension] Varchar(30) NULL,
	[Description] Nvarchar(500) NULL,
	[UploadBy] Varchar(36) NOT NULL,
	[UploadDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go

Create table [DocumentLibrary]
(
	[Id] Varchar(36) NOT NULL,
	[DocumentTyeId] Varchar(36) NOT NULL,
	[Name] Nvarchar(300) NOT NULL,
	[FileName] Nvarchar(250) NULL,
	[Path] Nvarchar(256) NULL,
	[Size] Integer NULL,
	[Extension] Varchar(30) NULL,
	[Description] Nvarchar(500) NULL,
	[IsDisplay] Bit NOT NULL,
	[UploadBy] Varchar(36) NOT NULL,
	[UploadDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go

Create table [DocumentTye]
(
	[Id] Varchar(36) NOT NULL,
	[Name] Nvarchar(200) NOT NULL,
	[Description] Nvarchar(300) NULL,
	[IsDisplay] Bit NOT NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[UpdateBy] Varchar(36) NOT NULL,
	[UpdateDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go

Create table [ReportProfileAbuseType]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[AbuseTypeId] Varchar(36) NOT NULL,
	[AbuseTypeName] Nvarchar(150) NULL,
Primary Key ([Id])
) 
go

Create table [AreaDistrict]
(
	[Id] Varchar(36) NOT NULL,
	[DistrictId] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[ProvinceId] Varchar(36) NULL,
	[AreaUserId] Varchar(36) NULL,
	[IsActivate] Bit NOT NULL,
Primary Key ([Id])
) 
go

Create table [AreaUser]
(
	[Id] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[Description] Nvarchar(500) NULL,
	[Manager] Nvarchar(200) NULL,
	[ProvinceId] Varchar(36) NULL,
	[ProvinceName] Nvarchar(150) NULL,
	[IsActivate] Bit NOT NULL,
	[CreateBy] Varchar(36) NULL,
	[CreateDate] Datetime NULL,
	[UpdateBy] Varchar(36) NULL,
	[UpdateDate] Datetime NULL,
Primary Key ([Id])
) 
go

Create table [AreaWard]
(
	[Id] Varchar(36) NOT NULL,
	[AreaDistrictId] Varchar(36) NOT NULL,
	[DistrictId] Varchar(36) NULL,
	[WardId] Varchar(36) NOT NULL,
	[Name] Nvarchar(150) NOT NULL,
	[IsActivate] Bit NOT NULL,
Primary Key ([Id])
) 
go

Create table [ReportHistory]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[Name] Nvarchar(200) NOT NULL,
	[Age] Integer NULL,
	[Abuse] Nvarchar(250) NULL,
	[FullAddress] Nvarchar(300) NULL,
	[StatusProcess] Integer NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[WardId] Varchar(36) NULL,
	[LocalAddress] Nvarchar(500) NULL,
Primary Key ([Id])
) 
go

Create table [ReportForward]
(
	[Id] Varchar(36) NOT NULL,
	[ReportProfileId] Varchar(36) NOT NULL,
	[CreateBy] Varchar(36) NOT NULL,
	[CreateDate] Datetime NOT NULL,
	[ForwardNote] Nvarchar(max) NULL,
	[ForwardLevel] Integer NULL,
	[Status] Char(1) NULL,
Primary Key ([Id])
) 
go

Create table [Notify]
(
	[Id] Varchar(36) NOT NULL,
	[UserId] Varchar(36) NULL,
	[NotifyKey] Varchar(36) NULL,
	[CreateDate] Datetime NOT NULL,
Primary Key ([Id])
) 
go



Alter table [ActivityLog] add  foreign key([UserId]) references [User] ([Id])  on update no action on delete no action 
go
Alter table [UserPermission] add  foreign key([UserId]) references [User] ([Id])  on update no action on delete no action 
go
Alter table [GroupPermission] add  foreign key([GroupUserId]) references [GroupUser] ([Id])  on update no action on delete no action 
go
Alter table [User] add  foreign key([GroupUserId]) references [GroupUser] ([Id])  on update no action on delete no action 
go
Alter table [Permission] add  foreign key([GroupFunctionId]) references [GroupFunction] ([Id])  on update no action on delete no action 
go
Alter table [GroupPermission] add  foreign key([PermissionId]) references [Permission] ([Id])  on update no action on delete no action 
go
Alter table [UserPermission] add  foreign key([PermissionId]) references [Permission] ([Id])  on update no action on delete no action 
go
Alter table [ProfileAttachment] add  foreign key([ReportProfileId]) references [ReportProfile] ([Id])  on update no action on delete no action 
go
Alter table [CaseVerification] add  foreign key([ReportProfileId]) references [ReportProfile] ([Id])  on update no action on delete no action 
go
Alter table [SupportAfterStatus] add  foreign key([ReportProfileId]) references [ReportProfile] ([Id])  on update no action on delete no action 
go
Alter table [EvaluationFirst] add  foreign key([ReportProfileId]) references [ReportProfile] ([Id])  on update no action on delete no action 
go
Alter table [SupportPlant] add  foreign key([ReportProfileId]) references [ReportProfile] ([Id])  on update no action on delete no action 
go
Alter table [ReportProfileAbuseType] add  foreign key([ReportProfileId]) references [ReportProfile] ([Id])  on update no action on delete no action 
go
Alter table [ReportProfileAbuseType] add  foreign key([AbuseTypeId]) references [AbuseType] ([Id])  on update no action on delete no action 
go
Alter table [Ward] add  foreign key([DistrictId]) references [District] ([Id])  on update no action on delete no action 
go
Alter table [District] add  foreign key([ProvinceId]) references [Province] ([Id])  on update no action on delete no action 
go
Alter table [EstimateCostAttachment] add  foreign key([SupportPlantId]) references [SupportPlant] ([Id])  on update no action on delete no action 
go
Alter table [DocumentLibrary] add  foreign key([DocumentTyeId]) references [DocumentTye] ([Id])  on update no action on delete no action 
go
Alter table [AreaWard] add  foreign key([AreaDistrictId]) references [AreaDistrict] ([Id])  on update no action on delete no action 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


/* Roles permissions */


/* Users permissions */


