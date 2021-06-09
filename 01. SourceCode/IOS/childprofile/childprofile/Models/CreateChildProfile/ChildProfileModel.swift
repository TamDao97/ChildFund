//
//  ChildProfileModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ChildProfileModel: Codable {
    var id: String
    var infoDate: String
    var employeeName: String
    var programCode: String
    var provinceId: String
    var districtId: String
    var wardId: String
    var address: String
    var childCode: String
    var orderNumber: Int?
    var ethnicId: String
    var religionId: String
    var name: String
    var nickName: String
    var gender: Int
    var dateOfBirth: String
    var leaningStatus: String
    var classInfo: String
    var favouriteSubjectModel: ObjectBaseModel?
    var learningCapacityModel: ObjectBaseModel?
    var houseworkModel: ObjectBaseModel?
    var healthModel: ObjectBaseModel?
    var personalityModel: ObjectBaseModel?
    var hobbyModel: ObjectBaseModel?
    var dreamModel: ObjectBaseModel?
    var livingWithParentModel: ObjectBaseModel?
    var notLivingWithParentModel: ObjectBaseModel?
    var livingWithOtherModel: ObjectBaseModel?
    var letterWriteModel: ObjectBaseModel?
    var houseTypeModel: ObjectBaseModel?
    var houseRoofModel: ObjectBaseModel?
    var houseWallModel: ObjectBaseModel?
    var houseFloorModel: ObjectBaseModel?
    var useElectricityModel: ObjectBaseModel?
    var schoolDistanceModel: ObjectBaseModel?
    var clinicDistanceModel: ObjectBaseModel?
    var waterSourceDistanceModel: ObjectBaseModel?
    var waterSourceUseModel: ObjectBaseModel?
    var roadConditionModel: ObjectBaseModel?
    var incomeFamilyModel: ObjectBaseModel?
    var harvestOutputModel: ObjectBaseModel?
    var incomeOtherModel: ObjectBaseModel?
    var listFamilyMember: [FamilyMemberModel]
    var numberPet: String
    var familyType: String
    var totalIncome: String
    var storyContent: String
    var imagePath: String
    var imageThumbnailPath: String
    var areaApproverId: String
    var areaApproverDate: String
    var officeApproveBy: String
    var officeApproveDate: String
    var processStatus: String
    var isDelete: Bool
    var createBy: String
    var createDate: String
    var updateBy: String
    var updateDate: String
    var userLever: String
    var isRemoveImage: Bool
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case infoDate = "InfoDate"
        case employeeName = "EmployeeName"
        case programCode = "ProgramCode"
        case provinceId = "ProvinceId"
        case districtId = "DistrictId"
        case wardId = "WardId"
        case address = "Address"
        case childCode = "ChildCode"
        case orderNumber = "OrderNumber"
        case ethnicId = "EthnicId"
        case religionId = "ReligionId"
        case name = "Name"
        case nickName = "NickName"
        case gender = "Gender"
        case dateOfBirth = "DateOfBirth"
        case leaningStatus = "LeaningStatus"
        case classInfo = "ClassInfo"
        case favouriteSubjectModel = "FavouriteSubjectModel"
        case learningCapacityModel = "LearningCapacityModel"
        case houseworkModel = "HouseworkModel"
        case healthModel = "HealthModel"
        case personalityModel = "PersonalityModel"
        case hobbyModel = "HobbyModel"
        case dreamModel = "DreamModel"
        case livingWithParentModel = "LivingWithParentModel"
        case notLivingWithParentModel = "NotLivingWithParentModel"
        case livingWithOtherModel = "LivingWithOtherModel"
        case letterWriteModel = "LetterWriteModel"
        case houseTypeModel = "HouseTypeModel"
        case houseRoofModel = "HouseRoofModel"
        case houseWallModel = "HouseWallModel"
        case houseFloorModel = "HouseFloorModel"
        case useElectricityModel = "UseElectricityModel"
        case schoolDistanceModel = "SchoolDistanceModel"
        case clinicDistanceModel = "ClinicDistanceModel"
        case waterSourceDistanceModel = "WaterSourceDistanceModel"
        case waterSourceUseModel = "WaterSourceUseModel"
        case roadConditionModel = "RoadConditionModel"
        case incomeFamilyModel = "IncomeFamilyModel"
        case harvestOutputModel = "HarvestOutputModel"
        case incomeOtherModel = "IncomeOtherModel"
        case listFamilyMember = "ListFamilyMember"
        case numberPet = "NumberPet"
        case familyType = "FamilyType"
        case totalIncome = "TotalIncome"
        case storyContent = "StoryContent"
        case imagePath = "ImagePath"
        case imageThumbnailPath = "ImageThumbnailPath"
        case areaApproverId = "AreaApproverId"
        case areaApproverDate = "AreaApproverDate"
        case officeApproveBy = "OfficeApproveBy"
        case officeApproveDate = "OfficeApproveDate"
        case processStatus = "ProcessStatus"
        case isDelete = "IsDelete"
        case createBy = "CreateBy"
        case createDate = "CreateDate"
        case updateBy = "UpdateBy"
        case updateDate = "UpdateDate"
        case userLever = "UserLever"
        case isRemoveImage = "IsRemoveImage"
    }
    
    required init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.id = try container.decodeIfPresent(String.self, forKey: .id) ?? ""
        self.infoDate = try container.decodeIfPresent(String.self, forKey: .infoDate) ?? ""
        self.employeeName = try container.decodeIfPresent(String.self, forKey: .employeeName) ?? ""
        self.programCode = try container.decodeIfPresent(String.self, forKey: .programCode) ?? ""
        self.provinceId = try container.decodeIfPresent(String.self, forKey: .provinceId) ?? ""
        self.districtId = try container.decodeIfPresent(String.self, forKey: .districtId) ?? ""
        self.wardId = try container.decodeIfPresent(String.self, forKey: .wardId) ?? ""
        self.address = try container.decodeIfPresent(String.self, forKey: .address) ?? ""
        self.childCode = try container.decodeIfPresent(String.self, forKey: .childCode) ?? ""
        self.orderNumber = try container.decodeIfPresent(Int.self, forKey: .orderNumber)
        self.ethnicId = try container.decodeIfPresent(String.self, forKey: .ethnicId) ?? ""
        self.religionId = try container.decodeIfPresent(String.self, forKey: .religionId) ?? ""
        self.name = try container.decodeIfPresent(String.self, forKey: .name) ?? ""
        self.nickName = try container.decodeIfPresent(String.self, forKey: .nickName) ?? ""
        self.gender = try container.decodeIfPresent(Int.self, forKey: .gender) ?? Gender.male.rawValue
        self.dateOfBirth = try container.decodeIfPresent(String.self, forKey: .dateOfBirth) ?? ""
        self.leaningStatus = try container.decodeIfPresent(String.self, forKey: .leaningStatus) ?? ""
        self.classInfo = try container.decodeIfPresent(String.self, forKey: .classInfo) ?? ""
        self.favouriteSubjectModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .favouriteSubjectModel)
        self.learningCapacityModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .learningCapacityModel)
        self.houseworkModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .houseworkModel)
        self.healthModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .healthModel)
        self.personalityModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .personalityModel)
        self.hobbyModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .hobbyModel)
        self.dreamModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .dreamModel)
        self.livingWithParentModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .livingWithParentModel)
        self.notLivingWithParentModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .notLivingWithParentModel)
        self.livingWithOtherModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .livingWithOtherModel)
        self.letterWriteModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .letterWriteModel)
        self.houseTypeModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .houseTypeModel)
        self.houseRoofModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .houseRoofModel)
        self.houseWallModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .houseWallModel)
        self.houseFloorModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .houseFloorModel)
        self.useElectricityModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .useElectricityModel)
        self.schoolDistanceModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .schoolDistanceModel)
        self.clinicDistanceModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .clinicDistanceModel)
        self.waterSourceDistanceModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .waterSourceDistanceModel)
        self.waterSourceUseModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .waterSourceUseModel)
        self.roadConditionModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .roadConditionModel)
        self.incomeFamilyModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .incomeFamilyModel)
        self.harvestOutputModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .harvestOutputModel)
        self.incomeOtherModel = try container.decodeIfPresent(ObjectBaseModel.self, forKey: .incomeOtherModel)
        self.listFamilyMember = try container.decodeIfPresent([FamilyMemberModel].self, forKey: .listFamilyMember) ?? []
        self.numberPet = try container.decodeIfPresent(String.self, forKey: .numberPet) ?? ""
        self.familyType = try container.decodeIfPresent(String.self, forKey: .familyType) ?? ""
        self.totalIncome = try container.decodeIfPresent(String.self, forKey: .totalIncome) ?? ""
        self.storyContent = try container.decodeIfPresent(String.self, forKey: .storyContent) ?? ""
        self.imagePath = try container.decodeIfPresent(String.self, forKey: .imagePath) ?? ""
        self.imageThumbnailPath = try container.decodeIfPresent(String.self, forKey: .imageThumbnailPath) ?? ""
        self.areaApproverId = try container.decodeIfPresent(String.self, forKey: .areaApproverId) ?? ""
        self.areaApproverDate = try container.decodeIfPresent(String.self, forKey: .areaApproverDate) ?? ""
        self.officeApproveBy = try container.decodeIfPresent(String.self, forKey: .officeApproveBy) ?? ""
        self.officeApproveDate = try container.decodeIfPresent(String.self, forKey: .officeApproveDate) ?? ""
        self.processStatus = try container.decodeIfPresent(String.self, forKey: .processStatus) ?? ""
        self.isDelete = try container.decodeIfPresent(Bool.self, forKey: .isDelete) ?? false
        self.createBy = try container.decodeIfPresent(String.self, forKey: .createBy) ?? ""
        self.createDate = try container.decodeIfPresent(String.self, forKey: .createDate) ?? ""
        self.updateBy = try container.decodeIfPresent(String.self, forKey: .updateBy) ?? ""
        self.updateDate = try container.decodeIfPresent(String.self, forKey: .updateDate) ?? ""
        self.userLever = try container.decodeIfPresent(String.self, forKey: .userLever) ?? ""
        self.isRemoveImage = try container.decodeIfPresent(Bool.self, forKey: .isRemoveImage) ?? false
    }
}
