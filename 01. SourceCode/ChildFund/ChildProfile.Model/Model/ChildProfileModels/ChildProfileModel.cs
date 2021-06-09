using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildProfiles.Model.ChildProfileModels
{
    public class ChildProfileModel
    {
        public bool? Handicap { get; set; }
        public string Description { get; set; }
        public bool IsExportWord { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public string ChildProfileId { get; set; }
        public string Avatar { get; set; }
        public DateTime InfoDate { get; set; }
        public string EmployeeName { get; set; }
        public string SaleforceID { get; set; }
        public int? EmployeeTitle { get; set; }
        public string ProgramCode { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public string VillageName { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string ChildCode { get; set; }
        public string SchoolId { get; set; }
        public string SchoolOtherName { get; set; }
        public string EthnicId { get; set; }
        public string ReligionId { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string AreaApproverNotes { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
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
        public string StoryContent { get; set; }
        public string ImagePath { get; set; }
        public string ImageThumbnailPath { get; set; }
        public string AreaApproverId { get; set; }
        public DateTime AreaApproverDate { get; set; }
        public string OfficeApproveBy { get; set; }
        public DateTime OfficeApproveDate { get; set; }
        public string ProcessStatus { get; set; }
        public bool IsDelete { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ReportProfileId { get; set; }
        public string Content { get; set; }
        public List<ImageHistory> ListImage { get; set; }
        /// <summary>
        /// UserLever dùng cho notify
        /// </summary>
        public string UserLever { get; set; }

        /// <summary>
        /// Người dùng xóa ảnh
        /// </summary>
        public bool IsRemoveImage { get; set; }
        public List<string> SelectId { get; set; }
        public ObjectBaseModel FavouriteSubjectModel { get; set; }
        public ObjectBaseModel LearningCapacityModel { get; set; }
        public ObjectBaseModel HouseworkModel { get; set; }
        public ObjectBaseModel HealthModel { get; set; }
        public ObjectBaseModel PersonalityModel { get; set; }
        public ObjectBaseModel HobbyModel { get; set; }
        public ObjectBaseModel DreamModel { get; set; }
        public List<FamilyMemberModel> ListFamilyMember { get; set; }
        public ObjectBaseModel LivingWithParentModel { get; set; }
        public ObjectBaseModel NotLivingWithParentModel { get; set; }
        public ObjectBaseModel LivingWithOtherModel { get; set; }
        public ObjectBaseModel LetterWriteModel { get; set; }
        public ObjectBaseModel HouseTypeModel { get; set; }
        public ObjectBaseModel HouseRoofModel { get; set; }
        public ObjectBaseModel HouseWallModel { get; set; }
        public ObjectBaseModel HouseFloorModel { get; set; }
        public ObjectBaseModel UseElectricityModel { get; set; }
        public ObjectBaseModel SchoolDistanceModel { get; set; }
        public ObjectBaseModel ClinicDistanceModel { get; set; }
        public ObjectBaseModel WaterSourceDistanceModel { get; set; }
        public ObjectBaseModel WaterSourceUseModel { get; set; }
        public ObjectBaseModel RoadConditionModel { get; set; }
        public ObjectBaseModel IncomeFamilyModel { get; set; }
        public ObjectBaseModel HarvestOutputModel { get; set; }
        public ObjectBaseModel NumberPetModel { get; set; }
        public ObjectBaseModel IncomeOtherModel { get; set; }

        public string ConsentName { get; set; }
        public string ConsentRelationship { get; set; }
        public string ConsentVillage { get; set; }
        public string ConsentWard { get; set; }
        public string SiblingsJoiningChildFund { get; set; }
        public string Malformation { get; set; }
        public string Orphan { get; set; }
        public ObjectBaseModel SiblingsJoiningChildFundModel { get; set; }
        public ObjectBaseModel MalformationModel { get; set; }
        public ObjectBaseModel OrphanModel { get; set; }
        public string ImageSignaturePath { get; set; }
        public string ImageSignatureThumbnailPath { get; set; }

        /// <summary>
        /// Convert string json to model
        /// </summary>
        public void ConvertObjectJsonToModel()
        {
            try
            {
                if (!string.IsNullOrEmpty(this.FavouriteSubject))
                {
                    this.FavouriteSubjectModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/FavouriteSubject.json");
                    try
                    {
                        ObjectBaseModel favouriteSubjectModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.FavouriteSubject);
                        //Cập nhật list nếu 2 list khác nhau
                        if (favouriteSubjectModel != null && this.FavouriteSubjectModel.ListObject != null && favouriteSubjectModel.ListObject != null
                            && this.FavouriteSubjectModel.ListObject.Count != favouriteSubjectModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.FavouriteSubjectModel.ListObject)
                            {
                                foreach (var itemInput in this.FavouriteSubjectModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.FavouriteSubjectModel = favouriteSubjectModel;
                        }
                    }
                    catch { }
                    this.FavouriteSubject = string.Empty;
                }
                else
                {
                    this.FavouriteSubjectModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/FavouriteSubject.json");
                }

                if (!string.IsNullOrEmpty(this.LearningCapacity))
                {
                    this.LearningCapacityModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LearningCapacity.json");
                    try
                    {
                        ObjectBaseModel learningCapacityModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.LearningCapacity);
                        //Cập nhật list nếu 2 list khác nhau
                        if (learningCapacityModel != null && this.LearningCapacityModel.ListObject != null && learningCapacityModel.ListObject != null
                            && this.LearningCapacityModel.ListObject.Count != learningCapacityModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.LearningCapacityModel.ListObject)
                            {
                                foreach (var itemInput in this.LearningCapacityModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.LearningCapacityModel = learningCapacityModel;
                        }
                    }
                    catch { }
                    this.LearningCapacity = string.Empty;
                }
                else
                {
                    this.LearningCapacityModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LearningCapacity.json");
                }

                if (!string.IsNullOrEmpty(this.Housework))
                {
                    this.HouseworkModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Housework.json");
                    try
                    {
                        var houseworkModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Housework);
                        //Cập nhật list nếu 2 list khác nhau
                        if (houseworkModel != null && this.HouseworkModel.ListObject != null && houseworkModel.ListObject != null
                            && this.HouseworkModel.ListObject.Count != houseworkModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HouseworkModel.ListObject)
                            {
                                foreach (var itemInput in this.HouseworkModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HouseworkModel = houseworkModel;
                        }
                    }
                    catch { }
                    this.Housework = string.Empty;
                }
                else
                {
                    this.HouseworkModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Housework.json");
                }

                if (!string.IsNullOrEmpty(this.Health))
                {
                    this.HealthModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Health.json");
                    try
                    {
                        var healthModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Health);
                        //Cập nhật list nếu 2 list khác nhau
                        if (healthModel != null && this.HealthModel.ListObject != null && healthModel.ListObject != null
                            && this.HealthModel.ListObject.Count != healthModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HealthModel.ListObject)
                            {
                                foreach (var itemInput in this.HealthModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HealthModel = healthModel;
                        }
                    }
                    catch { }
                    this.Health = string.Empty;
                }
                else
                {
                    this.HealthModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Health.json");
                }

                if (!string.IsNullOrEmpty(this.Personality))
                {
                    this.PersonalityModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Personality.json");
                    try
                    {
                        var personalityModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Personality);
                        //Cập nhật list nếu 2 list khác nhau
                        if (personalityModel != null && this.PersonalityModel.ListObject != null && personalityModel.ListObject != null
                            && this.PersonalityModel.ListObject.Count != personalityModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.PersonalityModel.ListObject)
                            {
                                foreach (var itemInput in this.PersonalityModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.PersonalityModel = personalityModel;
                        }
                    }
                    catch { }
                    this.Personality = string.Empty;
                }
                else
                {
                    this.PersonalityModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Personality.json");
                }

                if (!string.IsNullOrEmpty(this.Hobby))
                {
                    this.HobbyModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Hobby.json");
                    try
                    {
                        var hobbyModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Hobby);
                        //Cập nhật list nếu 2 list khác nhau
                        if (hobbyModel != null && this.HobbyModel.ListObject != null && hobbyModel.ListObject != null
                            && this.HobbyModel.ListObject.Count != hobbyModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HobbyModel.ListObject)
                            {
                                foreach (var itemInput in this.HobbyModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HobbyModel = hobbyModel;
                        }
                    }
                    catch { }
                    this.Hobby = string.Empty;
                }
                else
                {
                    this.HobbyModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Hobby.json");
                }

                if (!string.IsNullOrEmpty(this.Dream))
                {
                    this.DreamModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Dream.json");
                    try
                    {
                        var dreamModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Dream);
                        //Cập nhật list nếu 2 list khác nhau
                        if (dreamModel != null && this.DreamModel.ListObject != null && dreamModel.ListObject != null
                            && this.DreamModel.ListObject.Count != dreamModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.DreamModel.ListObject)
                            {
                                foreach (var itemInput in this.DreamModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.DreamModel = dreamModel;
                        }
                    }
                    catch { }
                    this.Dream = string.Empty;
                }
                else
                {
                    this.DreamModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Dream.json");
                }

                if (!string.IsNullOrEmpty(this.FamilyMember))
                {
                    try
                    {
                        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                        this.ListFamilyMember = JsonConvert.DeserializeObject<List<FamilyMemberModel>>(this.FamilyMember, dateTimeConverter);
                    }
                    catch
                    {
                        this.ListFamilyMember = new List<FamilyMemberModel>();
                    }
                    // this.FamilyMember = string.Empty;
                }
                else
                {
                    this.ListFamilyMember = new List<FamilyMemberModel>();
                }

                if (!string.IsNullOrEmpty(this.LivingWithParent))
                {
                    this.LivingWithParentModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LivingWithParent.json");
                    try
                    {
                        var livingWithParentModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.LivingWithParent);
                        //Cập nhật list nếu 2 list khác nhau
                        if (livingWithParentModel != null && this.LivingWithParentModel.ListObject != null && livingWithParentModel.ListObject != null
                            && this.LivingWithParentModel.ListObject.Count != livingWithParentModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.LivingWithParentModel.ListObject)
                            {
                                foreach (var itemInput in this.LivingWithParentModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.LivingWithParentModel = livingWithParentModel;
                        }
                    }
                    catch { }
                    this.LivingWithParent = string.Empty;
                }
                else
                {
                    this.LivingWithParentModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LivingWithParent.json");
                }

                if (!string.IsNullOrEmpty(this.NotLivingWithParent))
                {
                    this.NotLivingWithParentModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/NotLivingWithParent.json");
                    try
                    {
                        var notLivingWithParentModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.NotLivingWithParent);
                        //Cập nhật list nếu 2 list khác nhau
                        if (notLivingWithParentModel != null && this.NotLivingWithParentModel.ListObject != null && notLivingWithParentModel.ListObject != null
                            && this.NotLivingWithParentModel.ListObject.Count != notLivingWithParentModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.NotLivingWithParentModel.ListObject)
                            {
                                foreach (var itemInput in this.NotLivingWithParentModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.NotLivingWithParentModel = notLivingWithParentModel;
                        }
                    }
                    catch { }
                    this.NotLivingWithParent = string.Empty;
                }
                else
                {
                    this.NotLivingWithParentModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/NotLivingWithParent.json");
                }

                if (!string.IsNullOrEmpty(this.LivingWithOther))
                {
                    this.LivingWithOtherModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LivingWithOther.json");
                    try
                    {
                        var livingWithOtherModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.LivingWithOther);
                        //Cập nhật list nếu 2 list khác nhau
                        if (livingWithOtherModel != null && this.LivingWithOtherModel.ListObject != null && livingWithOtherModel.ListObject != null
                            && this.LivingWithOtherModel.ListObject.Count != livingWithOtherModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.LivingWithOtherModel.ListObject)
                            {
                                foreach (var itemInput in this.LivingWithOtherModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.LivingWithOtherModel = livingWithOtherModel;
                        }
                    }
                    catch { }
                    this.LivingWithOther = string.Empty;
                }
                else
                {
                    this.LivingWithOtherModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LivingWithOther.json");
                }

                if (!string.IsNullOrEmpty(this.LetterWrite))
                {
                    this.LetterWriteModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LetterWrite.json");
                    try
                    {
                        var letterWriteModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.LetterWrite);
                        //Cập nhật list nếu 2 list khác nhau
                        if (letterWriteModel != null && this.LetterWriteModel.ListObject != null && letterWriteModel.ListObject != null
                            && this.LetterWriteModel.ListObject.Count != letterWriteModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.LetterWriteModel.ListObject)
                            {
                                foreach (var itemInput in this.LetterWriteModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.LetterWriteModel = letterWriteModel;
                        }
                    }
                    catch { }
                    this.LetterWrite = string.Empty;
                }
                else
                {
                    this.LetterWriteModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/LetterWrite.json");
                }

                if (!string.IsNullOrEmpty(this.HouseType))
                {
                    this.HouseTypeModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseType.json");
                    try
                    {
                        var houseTypeModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.HouseType);
                        //Cập nhật list nếu 2 list khác nhau
                        if (houseTypeModel != null && this.HouseTypeModel.ListObject != null && houseTypeModel.ListObject != null
                            && this.HouseTypeModel.ListObject.Count != houseTypeModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HouseTypeModel.ListObject)
                            {
                                foreach (var itemInput in this.HouseTypeModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HouseTypeModel = houseTypeModel;
                        }
                    }
                    catch { }
                    this.HouseType = string.Empty;
                }
                else
                {
                    this.HouseTypeModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseType.json");
                }

                if (!string.IsNullOrEmpty(this.HouseRoof))
                {
                    this.HouseRoofModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseRoof.json");
                    try
                    {
                        var houseRoofModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.HouseRoof);
                        //Cập nhật list nếu 2 list khác nhau
                        if (houseRoofModel != null && this.HouseRoofModel.ListObject != null && houseRoofModel.ListObject != null
                            && this.HouseRoofModel.ListObject.Count != houseRoofModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HouseRoofModel.ListObject)
                            {
                                foreach (var itemInput in this.HouseRoofModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HouseRoofModel = houseRoofModel;
                        }
                    }
                    catch { }
                    this.HouseRoof = string.Empty;
                }
                else
                {
                    this.HouseRoofModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseRoof.json");
                }

                if (!string.IsNullOrEmpty(this.HouseWall))
                {
                    this.HouseWallModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseWall.json");
                    try
                    {
                        var houseWallModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.HouseWall);
                        //Cập nhật list nếu 2 list khác nhau
                        if (houseWallModel != null && this.HouseWallModel.ListObject != null && houseWallModel.ListObject != null
                            && this.HouseWallModel.ListObject.Count != houseWallModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HouseWallModel.ListObject)
                            {
                                foreach (var itemInput in this.HouseWallModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HouseWallModel = houseWallModel;
                        }
                    }
                    catch { }
                    this.HouseWall = string.Empty;
                }
                else
                {
                    this.HouseWallModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseWall.json");
                }

                if (!string.IsNullOrEmpty(this.HouseFloor))
                {
                    this.HouseFloorModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseFloor.json");
                    try
                    {
                        var houseFloorModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.HouseFloor);
                        //Cập nhật list nếu 2 list khác nhau
                        if (houseFloorModel != null && this.HouseFloorModel.ListObject != null && houseFloorModel.ListObject != null
                            && this.HouseFloorModel.ListObject.Count != houseFloorModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HouseFloorModel.ListObject)
                            {
                                foreach (var itemInput in this.HouseFloorModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HouseFloorModel = houseFloorModel;
                        }
                    }
                    catch { }
                    this.HouseFloor = string.Empty;
                }
                else
                {
                    this.HouseFloorModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HouseFloor.json");
                }

                if (!string.IsNullOrEmpty(this.UseElectricity))
                {
                    this.UseElectricityModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/UseElectricity.json");
                    try
                    {
                        var useElectricityModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.UseElectricity);
                        //Cập nhật list nếu 2 list khác nhau
                        if (useElectricityModel != null && this.UseElectricityModel.ListObject != null && useElectricityModel.ListObject != null
                            && this.UseElectricityModel.ListObject.Count != useElectricityModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.UseElectricityModel.ListObject)
                            {
                                foreach (var itemInput in this.UseElectricityModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.UseElectricityModel = useElectricityModel;
                        }
                    }
                    catch { }
                    this.UseElectricity = string.Empty;
                }
                else
                {
                    this.UseElectricityModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/UseElectricity.json");
                }

                if (!string.IsNullOrEmpty(this.SchoolDistance))
                {
                    this.SchoolDistanceModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/SchoolDistance.json");
                    try
                    {
                        var schoolDistanceModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.SchoolDistance);
                        //Cập nhật list nếu 2 list khác nhau
                        if (schoolDistanceModel != null && this.SchoolDistanceModel.ListObject != null && schoolDistanceModel.ListObject != null
                            && this.SchoolDistanceModel.ListObject.Count != schoolDistanceModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.SchoolDistanceModel.ListObject)
                            {
                                foreach (var itemInput in this.SchoolDistanceModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.SchoolDistanceModel = schoolDistanceModel;
                        }
                    }
                    catch { }
                    this.SchoolDistance = string.Empty;
                }
                else
                {
                    this.SchoolDistanceModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/SchoolDistance.json");
                }

                if (!string.IsNullOrEmpty(this.ClinicDistance))
                {
                    this.ClinicDistanceModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/ClinicDistance.json");
                    try
                    {
                        var clinicDistanceModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.ClinicDistance);
                        //Cập nhật list nếu 2 list khác nhau
                        if (clinicDistanceModel != null && this.ClinicDistanceModel.ListObject != null && clinicDistanceModel.ListObject != null
                            && this.ClinicDistanceModel.ListObject.Count != clinicDistanceModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.ClinicDistanceModel.ListObject)
                            {
                                foreach (var itemInput in this.ClinicDistanceModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.ClinicDistanceModel = clinicDistanceModel;
                        }
                    }
                    catch { }
                    this.ClinicDistance = string.Empty;
                }
                else
                {
                    this.ClinicDistanceModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/ClinicDistance.json");
                }

                if (!string.IsNullOrEmpty(this.WaterSourceDistance))
                {
                    this.WaterSourceDistanceModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/WaterSourceDistance.json");
                    try
                    {
                        var waterSourceDistanceModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.WaterSourceDistance);
                        //Cập nhật list nếu 2 list khác nhau
                        if (waterSourceDistanceModel != null && this.WaterSourceDistanceModel.ListObject != null && waterSourceDistanceModel.ListObject != null
                            && this.WaterSourceDistanceModel.ListObject.Count != waterSourceDistanceModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.WaterSourceDistanceModel.ListObject)
                            {
                                foreach (var itemInput in this.WaterSourceDistanceModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.WaterSourceDistanceModel = waterSourceDistanceModel;
                        }
                    }
                    catch { }
                    this.WaterSourceDistance = string.Empty;
                }
                else
                {
                    this.WaterSourceDistanceModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/WaterSourceDistance.json");
                }

                if (!string.IsNullOrEmpty(this.WaterSourceUse))
                {
                    this.WaterSourceUseModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/WaterSourceUse.json");
                    try
                    {
                        var waterSourceUseModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.WaterSourceUse);
                        //Cập nhật list nếu 2 list khác nhau
                        if (waterSourceUseModel != null && this.WaterSourceUseModel.ListObject != null && waterSourceUseModel.ListObject != null
                            && this.WaterSourceUseModel.ListObject.Count != waterSourceUseModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.WaterSourceUseModel.ListObject)
                            {
                                foreach (var itemInput in this.WaterSourceUseModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.WaterSourceUseModel = waterSourceUseModel;
                        }
                    }
                    catch { }
                    this.WaterSourceUse = string.Empty;
                }
                else
                {
                    this.WaterSourceUseModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/WaterSourceUse.json");
                }

                if (!string.IsNullOrEmpty(this.RoadCondition))
                {
                    this.RoadConditionModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/RoadCondition.json");
                    try
                    {
                        var roadConditionModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.RoadCondition);
                        //Cập nhật list nếu 2 list khác nhau
                        if (roadConditionModel != null && this.RoadConditionModel.ListObject != null && roadConditionModel.ListObject != null
                            && this.RoadConditionModel.ListObject.Count != roadConditionModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.RoadConditionModel.ListObject)
                            {
                                foreach (var itemInput in this.RoadConditionModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.RoadConditionModel = roadConditionModel;
                        }
                    }
                    catch { }
                    this.RoadCondition = string.Empty;
                }
                else
                {
                    this.RoadConditionModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/RoadCondition.json");
                }

                if (!string.IsNullOrEmpty(this.IncomeFamily))
                {
                    this.IncomeFamilyModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/IncomeFamily.json");
                    try
                    {
                        var incomeFamilyModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.IncomeFamily);
                        //Cập nhật list nếu 2 list khác nhau
                        if (incomeFamilyModel != null && this.IncomeFamilyModel.ListObject != null && incomeFamilyModel.ListObject != null
                            && this.IncomeFamilyModel.ListObject.Count != incomeFamilyModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.IncomeFamilyModel.ListObject)
                            {
                                foreach (var itemInput in this.IncomeFamilyModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.IncomeFamilyModel = incomeFamilyModel;
                        }
                    }
                    catch { }
                    this.IncomeFamily = string.Empty;
                }
                else
                {
                    this.IncomeFamilyModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/IncomeFamily.json");
                }

                if (!string.IsNullOrEmpty(this.HarvestOutput))
                {
                    this.HarvestOutputModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HarvestOutput.json");
                    try
                    {
                        ObjectBaseModel harvestOutputModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.HarvestOutput);
                        //Cập nhật list nếu 2 list khác nhau
                        if (harvestOutputModel != null && this.HarvestOutputModel.ListObject != null && harvestOutputModel.ListObject != null
                            && this.HarvestOutputModel.ListObject.Count != harvestOutputModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.HarvestOutputModel.ListObject)
                            {
                                foreach (var itemInput in this.HarvestOutputModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.HarvestOutputModel = harvestOutputModel;
                        }
                    }
                    catch { }
                    this.HarvestOutput = string.Empty;
                }
                else
                {
                    this.HarvestOutputModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/HarvestOutput.json");
                }

                if (!string.IsNullOrEmpty(this.NumberPet))
                {
                    this.NumberPetModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/NumberPet.json");
                    try
                    {
                        var numberPetModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.NumberPet);
                        //Cập nhật list nếu 2 list khác nhau
                        if (numberPetModel != null && this.NumberPetModel.ListObject != null && numberPetModel.ListObject != null
                            && this.NumberPetModel.ListObject.Count != numberPetModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.NumberPetModel.ListObject)
                            {
                                foreach (var itemInput in this.NumberPetModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.NumberPetModel = numberPetModel;
                        }
                    }
                    catch { }
                    this.NumberPet = string.Empty;
                }
                else
                {
                    this.NumberPetModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/NumberPet.json");
                }

                if (!string.IsNullOrEmpty(this.IncomeOther))
                {
                    this.IncomeOtherModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/IncomeOther.json");
                    try
                    {
                        var incomeOtherModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.IncomeOther);
                        //Cập nhật list nếu 2 list khác nhau
                        if (incomeOtherModel != null && this.IncomeOtherModel.ListObject != null && incomeOtherModel.ListObject != null
                            && this.IncomeOtherModel.ListObject.Count != incomeOtherModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.IncomeOtherModel.ListObject)
                            {
                                foreach (var itemInput in this.IncomeOtherModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.IncomeOtherModel = incomeOtherModel;
                        }
                    }
                    catch { }
                    this.IncomeOther = string.Empty;
                }
                else
                {
                    this.IncomeOtherModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/IncomeOther.json");
                }

                if (!string.IsNullOrEmpty(this.SiblingsJoiningChildFund))
                {
                    this.SiblingsJoiningChildFundModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/SiblingsJoiningChildFund.json");
                    try
                    {
                        var siblingsJoiningChildFundModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.SiblingsJoiningChildFund);
                        //Cập nhật list nếu 2 list khác nhau
                        if (siblingsJoiningChildFundModel != null && this.SiblingsJoiningChildFundModel.ListObject != null && siblingsJoiningChildFundModel.ListObject != null
                            && this.SiblingsJoiningChildFundModel.ListObject.Count != siblingsJoiningChildFundModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.SiblingsJoiningChildFundModel.ListObject)
                            {
                                foreach (var itemInput in this.SiblingsJoiningChildFundModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.SiblingsJoiningChildFundModel = siblingsJoiningChildFundModel;
                        }
                    }
                    catch { }
                    this.SiblingsJoiningChildFund = string.Empty;
                }
                else
                {
                    this.SiblingsJoiningChildFundModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/SiblingsJoiningChildFund.json");
                }

                if (!string.IsNullOrEmpty(this.Malformation))
                {
                    this.MalformationModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Malformation.json");
                    try
                    {
                        var malformationModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Malformation);
                        //Cập nhật list nếu 2 list khác nhau
                        if (malformationModel != null && this.MalformationModel.ListObject != null && malformationModel.ListObject != null
                            && this.MalformationModel.ListObject.Count != malformationModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.MalformationModel.ListObject)
                            {
                                foreach (var itemInput in this.MalformationModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.MalformationModel = malformationModel;
                        }
                    }
                    catch { }
                    this.Malformation = string.Empty;
                }
                else
                {
                    this.MalformationModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Malformation.json");
                }

                if (!string.IsNullOrEmpty(this.Orphan))
                {
                    this.OrphanModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Orphan.json");
                    try
                    {
                        var orphanModel = JsonConvert.DeserializeObject<ObjectBaseModel>(this.Orphan);
                        //Cập nhật list nếu 2 list khác nhau
                        if (orphanModel != null && this.OrphanModel.ListObject != null && orphanModel.ListObject != null
                            && this.OrphanModel.ListObject.Count != orphanModel.ListObject.Count)
                        {
                            foreach (var itemTemp in this.OrphanModel.ListObject)
                            {
                                foreach (var itemInput in this.OrphanModel.ListObject)
                                {
                                    if (itemTemp.Id.Equals(itemInput.Id))
                                    {
                                        itemTemp.Value = itemInput.Value;
                                        itemTemp.OtherValue = itemInput.OtherValue;
                                        itemTemp.Check = itemInput.Check;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.OrphanModel = orphanModel;
                        }
                    }
                    catch { }
                    this.Orphan = string.Empty;
                }
                else
                {
                    this.OrphanModel = ConvertFileJson<ObjectBaseModel>.ReadFile("~/JsonData/Orphan.json");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Convert model to string json
        /// </summary>
        public void ConvertObjectModelToJson()
        {
            try
            {
                if (this.FavouriteSubjectModel != null)
                {
                    this.FavouriteSubject = JsonConvert.SerializeObject(this.FavouriteSubjectModel);
                }

                if (this.LearningCapacityModel != null)
                {
                    this.LearningCapacity = JsonConvert.SerializeObject(this.LearningCapacityModel);
                }

                if (this.HouseworkModel != null)
                {
                    this.Housework = JsonConvert.SerializeObject(this.HouseworkModel);
                }

                if (this.HealthModel != null)
                {
                    this.Health = JsonConvert.SerializeObject(this.HealthModel);
                }

                if (this.PersonalityModel != null)
                {
                    this.Personality = JsonConvert.SerializeObject(this.PersonalityModel);
                }

                if (this.HobbyModel != null)
                {
                    this.Hobby = JsonConvert.SerializeObject(this.HobbyModel);
                }

                if (this.DreamModel != null)
                {
                    this.Dream = JsonConvert.SerializeObject(this.DreamModel);
                }

                if (this.ListFamilyMember != null)
                {
                    this.FamilyMember = JsonConvert.SerializeObject(this.ListFamilyMember);
                }

                if (this.LivingWithParentModel != null)
                {
                    this.LivingWithParent = JsonConvert.SerializeObject(this.LivingWithParentModel);
                }

                if (this.NotLivingWithParentModel != null)
                {
                    this.NotLivingWithParent = JsonConvert.SerializeObject(this.NotLivingWithParentModel);
                }

                if (this.LivingWithOtherModel != null)
                {
                    this.LivingWithOther = JsonConvert.SerializeObject(this.LivingWithOtherModel);
                }

                if (this.LetterWriteModel != null)
                {
                    this.LetterWrite = JsonConvert.SerializeObject(this.LetterWriteModel);
                }

                if (this.HouseTypeModel != null)
                {
                    this.HouseType = JsonConvert.SerializeObject(this.HouseTypeModel);
                }

                if (this.HouseRoofModel != null)
                {
                    this.HouseRoof = JsonConvert.SerializeObject(this.HouseRoofModel);
                }

                if (this.HouseWallModel != null)
                {
                    this.HouseWall = JsonConvert.SerializeObject(this.HouseWallModel);
                }

                if (this.HouseFloorModel != null)
                {
                    this.HouseFloor = JsonConvert.SerializeObject(this.HouseFloorModel);
                }

                if (this.UseElectricityModel != null)
                {
                    this.UseElectricity = JsonConvert.SerializeObject(this.UseElectricityModel);
                }

                if (this.SchoolDistanceModel != null)
                {
                    this.SchoolDistance = JsonConvert.SerializeObject(this.SchoolDistanceModel);
                }

                if (this.ClinicDistanceModel != null)
                {
                    this.ClinicDistance = JsonConvert.SerializeObject(this.ClinicDistanceModel);
                }

                if (this.WaterSourceDistanceModel != null)
                {
                    this.WaterSourceDistance = JsonConvert.SerializeObject(this.WaterSourceDistanceModel);
                }

                if (this.WaterSourceUseModel != null)
                {
                    this.WaterSourceUse = JsonConvert.SerializeObject(this.WaterSourceUseModel);
                }

                if (this.RoadConditionModel != null)
                {
                    this.RoadCondition = JsonConvert.SerializeObject(this.RoadConditionModel);
                }

                if (this.IncomeFamilyModel != null)
                {
                    this.IncomeFamily = JsonConvert.SerializeObject(this.IncomeFamilyModel);
                }

                if (this.HarvestOutputModel != null)
                {
                    this.HarvestOutput = JsonConvert.SerializeObject(this.HarvestOutputModel);
                }

                if (this.NumberPetModel != null)
                {
                    this.NumberPet = JsonConvert.SerializeObject(this.NumberPetModel);
                }

                if (this.IncomeOtherModel != null)
                {
                    this.IncomeOther = JsonConvert.SerializeObject(this.IncomeOtherModel);
                }

                if (this.SiblingsJoiningChildFundModel != null)
                {
                    this.SiblingsJoiningChildFund = JsonConvert.SerializeObject(this.SiblingsJoiningChildFundModel);
                }

                if (this.MalformationModel != null)
                {
                    this.Malformation = JsonConvert.SerializeObject(this.MalformationModel);
                }

                if (this.OrphanModel != null)
                {
                    this.Orphan = JsonConvert.SerializeObject(this.OrphanModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
