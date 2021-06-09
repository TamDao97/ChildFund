//
//  CreateChildProfileViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class CreateChildProfileViewModel: BaseViewModel {
    var childProfileModel: ChildProfileModel?
    
    var successMessage = Strings.insertChildProfileSuccess
    var updateSuccessMessage = Strings.updateChildProfileSuccess
    
    var childImageData: Data?
    var childProfileId: String = ""
    var isEditMode: Bool {
        return !childProfileId.isEmpty
    }
    
    var title: String {
        return isEditMode ? ScreenTitle.updateChildProfile : ScreenTitle.createChildProfile
    }
    
    func get(completion: @escaping (ResultStatus) -> Void) {
        ChildProfileService.get(id: childProfileId) { [weak self] result in
            guard let self = self else { return }
            switch result {
            case .success(let childProfileModel):
                self.childProfileModel = childProfileModel
                
                dump(childProfileModel)
                
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }

    func insert(completion: @escaping (ResultStatus) -> Void) {
        guard let childProfileModel = self.childProfileModel else {
            return
        }
        
        dump(childProfileModel)
        
        ChildProfileService.insert(image: childImageData, model: childProfileModel) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success:
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
    
    func update(completion: @escaping (ResultStatus) -> Void) {
        guard let childProfileModel = self.childProfileModel else {
            return
        }
        
        dump(childProfileModel)
        
        ChildProfileService.update(image: childImageData, model: childProfileModel) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success:
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
    
    func setModelFromChildViewModel(step1ViewModel: Step1ViewModel,
                                     step2ViewModel: Step2ViewModel,
                                     step3ViewModel: Step3ViewModel,
                                     step4ViewModel: Step4ViewModel) {
        guard let model = childProfileModel else { return }
        
        if !isEditMode {
            model.processStatus = Constants.profileNew
            model.createBy = Setting.userId.value
        }
        model.userLever = Setting.userLevel.value
        model.updateBy = Setting.userId.value
        
        childImageData = step1ViewModel.childImageData
        model.employeeName = step1ViewModel.employeeName
        model.imagePath = step1ViewModel.imagePath
        model.infoDate = step1ViewModel.infoDate
        model.programCode = step1ViewModel.programCode
        model.childCode = step1ViewModel.childCode
        model.orderNumber = step1ViewModel.orderNumber
        model.ethnicId = step1ViewModel.nationId
        model.religionId = step1ViewModel.religionId
        model.provinceId = step1ViewModel.provinceId
        model.districtId = step1ViewModel.districtId
        model.wardId = step1ViewModel.wardId
        model.address = step1ViewModel.address
        
        model.name = step2ViewModel.childName
        model.nickName = step2ViewModel.nickName
        model.gender = step2ViewModel.gender
        model.dateOfBirth = step2ViewModel.dateOfBirth
        model.leaningStatus = step2ViewModel.learningStatus
        model.classInfo = step2ViewModel.classInfo
        model.favouriteSubjectModel?.otherValue = step2ViewModel.differentSubject
        model.favouriteSubjectModel?.otherValue2 = step2ViewModel.bestSubject
        model.learningCapacityModel?.otherValue = step2ViewModel.achievement
        model.houseworkModel?.otherValue = step2ViewModel.workOther
        model.healthModel?.otherValue = step2ViewModel.healthOther
        model.personalityModel?.otherValue = step2ViewModel.personalityOther
        model.hobbyModel?.otherValue = step2ViewModel.hobbieOther
        model.dreamModel?.otherValue = step2ViewModel.dreamOther
        
        model.notLivingWithParentModel?.otherValue = step3ViewModel.notLiveParent
        model.livingWithOtherModel?.otherValue = step3ViewModel.liveWhoOther
        model.letterWriteModel?.otherValue = step3ViewModel.liveWriteLetterOther
        model.listFamilyMember = step3ViewModel.listFamilyMember
        
        model.houseTypeModel?.otherValue = step4ViewModel.typeHousingOther
        model.houseRoofModel?.otherValue = step4ViewModel.roofMaterialOther
        model.houseWallModel?.otherValue = step4ViewModel.wallMaterialsOther
        model.houseFloorModel?.otherValue = step4ViewModel.floorMaterialsOther
        model.waterSourceUseModel?.otherValue = step4ViewModel.waterOther
        model.familyType = step4ViewModel.familyType
        model.numberPet = step4ViewModel.pet
        model.totalIncome = step4ViewModel.totalIncome
    }
}
