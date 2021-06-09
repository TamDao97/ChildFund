//
//  MemberFamilyViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/21/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

enum LiveWithChild: Int {
    case no = 0
    case yes
    
    var title: String {
        switch self {
        case .yes:
            return "Có"
        case .no:
            return "Không"
        }
    }
}

class MemberFamilyViewModel {
    var name: String = ""
    var dateOfBirth: String = ""
    var relationShipId: String = ""
    var gender: Int = Gender.male.rawValue
    var jobId: String = ""
    var liveWithChild: Int = LiveWithChild.yes.rawValue
    var editModel: FamilyMemberModel?
    
    var isEditMode: Bool {
        return editModel != nil
    }
    
    let relationShipDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.relationShip.value)
    let jobDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.job.value)
    
    var nameErrorTitle: String {
        return name.isEmpty ? Strings.emptyError : ""
    }
    
    var relationShipErrorTitle: String {
        return relationShipId.isEmpty ? Strings.emptyError : ""
    }
    
    var isFormValid: Bool {
        return nameErrorTitle.isEmpty && relationShipErrorTitle.isEmpty
    }
    
    func config(editModel: FamilyMemberModel) {
        self.editModel = editModel
        
        name = editModel.name
        dateOfBirth = editModel.dateOfBirth
        relationShipId = editModel.relationshipId
        gender = editModel.gender
        jobId = editModel.job
        liveWithChild = editModel.liveWithChild
    }
    
    func updateEditModel() {
        guard let model = editModel else { return }
        bindToModel(model)
    }
    
    func getInsertModel() -> FamilyMemberModel {
        let model = FamilyMemberModel()
        bindToModel(model)
        return model
    }
    
    private func bindToModel(_ model: FamilyMemberModel) {
        model.name = name
        model.dateOfBirth = dateOfBirth
        model.relationshipId = relationShipId
        model.gender = gender
        model.job = jobId
        model.jobName = Utilities.getName(from: jobDataSource, with: jobId)
        model.liveWithChild = liveWithChild
    }
}
