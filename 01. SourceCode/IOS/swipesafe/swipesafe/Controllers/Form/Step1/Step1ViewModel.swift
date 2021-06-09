//
//  Step1ViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/15/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class Step1ViewModel {
    var childIndex: Int?
    
    var name: String = ""
    var gender: String = "" {
        didSet {
            guard
                !gender.isEmpty,
                let genderValue = Int(gender),
                let genderType = Gender(rawValue: genderValue)
                else { return }
            genderName = genderType.title
        }
    }
    var genderName: String = ""
    var birthday: String = ""
    var age: Int?
    var level: String = "" {
        didSet {
            guard
                !level.isEmpty,
                let levelValue = Int(level),
                let levelType = Level(rawValue: levelValue)
                else { return }
            levelName = levelType.title
        }
    }
    var levelName: String = ""
    var address: String = ""
    var provinceId: String = ""
    var provinceName: String = ""
    var districtId: String = ""
    var districtName: String = ""
    var wardId: String = ""
    var wardName: String = ""
    var dateAction: String = ""
    
    var abuses: [ChildAbuseModel] {
        return childAbuseDataSource.filter { $0.isCheck }
    }
    
    var fullAddress: String {
        var fullAddress = ""
        if address != "" {
            fullAddress += address + " - "
        }
        fullAddress += wardName + " - " + districtName + " - " + provinceName
        
        return fullAddress
    }
    
    var childModel: ChildModel {
        let childModel = ChildModel()
        childModel.name = name
        childModel.gender = gender
        childModel.age = age
        childModel.birthDay = birthday
        childModel.level = level
        childModel.address = address
        childModel.provinceId = provinceId
        childModel.districtId = districtId
        childModel.wardId = wardId
        childModel.fullAddress = fullAddress
        childModel.dateAction = dateAction
        childModel.abuses = abuses
        childModel.genderName = genderName
        childModel.levelName = levelName
        childModel.provinceName = provinceName
        childModel.districtName = districtName
        childModel.wardName = wardName
        return childModel
    }
    
    let provinceDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.province.value)
    let districtAllDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.district.value)
    let wardGroupDataSource: [WarGroupModel] = WarGroupModel.getSessionData(from: Setting.ward.value)
    var childAbuseDataSource: [ChildAbuseModel] = ChildAbuseModel.getSessionData(from: Setting.formAbuse.value)
    
    var districtDataSource: [ComboboxItemModel] = []
    var wardDataSource: [ComboboxItemModel] = []
    
    var provinceTitles: [String] {
        return provinceDataSource.map { $0.value }
    }
    
    var districtTitles: [String] {
        return districtDataSource.map { $0.value }
    }
    
    var wardTitles: [String] {
        return wardDataSource.map { $0.value }
    }
    
    var childInfoCellViewModels: [InfoCellViewModel] {
        let childs = AppData.shared.getChilds().enumerated().map {
            InfoCellViewModel(index: $0,
                              title: $1.name == "" ? Strings.Step1.unknownChildName : $1.name)
        }
        return childs
    }
    
    var childInfoViewIsHidden: Bool {
        if AppData.shared.getChilds().count <= 1 && childIndex == nil {
            return true
        }
        return false
    }
    
    func getAreaIdFromName(provinceName: String, districtName: String, wardName: String) -> (String, String, String) {
        let provinceId = provinceDataSource.filter { provinceName.contains($0.value) }.first?.id ?? ""
        let districtId = districtAllDataSource.filter { districtName.contains($0.value) }.first?.id ?? ""
        var wardId = ""
        if let wardGroupData = wardGroupDataSource.first(where: { $0.districtId == districtId }) {
            wardId = wardGroupData.listWard.filter { wardName.contains($0.value) }.first?.id ?? ""
        }
        
        return (provinceId, districtId, wardId)
    }
    
    func setDistrictDataSource(provinceId: String) {
        districtDataSource = districtAllDataSource.filter { $0.parentId == provinceId }
    }
    
    func setWarDataSource(districtId: String?) {
        guard
            let districtId = districtId,
            let wardGroupData = wardGroupDataSource.first(where: { $0.districtId == districtId })
        else {
            wardDataSource = []
            return
        }
        wardDataSource = wardGroupData.listWard
    }
    
    var errorFormMessage: String {
        if level.isEmpty {
            return Strings.Step1.levelEmptyErrorTitle
        }
        
        if districtId.isEmpty {
            return Strings.Step1.districtEmptyErrorTitle
        }
        
        if provinceId.isEmpty {
            return Strings.Step1.provinceEmptyErrorTitle
        }
        
        if wardId.isEmpty {
            return Strings.Step1.wardEmptyErrorTitle
        }
        
        if dateAction.isEmpty {
            return Strings.Step1.dateEmptyErrorTitle
        }
        
        if abuses.isEmpty {
            return Strings.Step1.abuseEmptyErrorTitle
        }
        
        return ""
    }
    
    func resetToFirstChild() {
        updateValue(by: 0)
    }
    
    func updateNextChildIndex() {
        let infoChildCount = AppData.shared.getChilds().count
        childIndex = infoChildCount == 0 ? nil : infoChildCount
    }
    
    func updateValue(by childIndex: Int) {
        guard let childModel = AppData.shared.getChild(at: childIndex) else { return }
        
        self.childIndex = childIndex
        
        name = childModel.name
        gender = childModel.gender
        age = childModel.age
        birthday = childModel.birthDay
        level = childModel.level
        address = childModel.address
        provinceId = childModel.provinceId
        districtId = childModel.districtId
        wardId = childModel.wardId
        dateAction = childModel.dateAction
        genderName = childModel.genderName
        levelName = childModel.levelName
        provinceName = childModel.provinceName
        districtName = childModel.districtName
        wardName = childModel.wardName
        
        childAbuseDataSource.forEach { childAbuse in
            if childModel.abuses.contains(childAbuse) {
                childAbuse.isCheck = true
            } else {
                childAbuse.isCheck = false
            }
        }
    }
    
    func removeChildModelFromAppData(at childIndex: Int) {
        AppData.shared.removeReportChildModel(at: childIndex)
    }
    
    func updateChildReportModel() {
        if childIndex == nil {
            childIndex = 0
        }
        AppData.shared.updateReportChildModel(childModel, at: childIndex)
    }
}
