//
//  Step2ViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class Step2ViewModel {
    var prisonerIndex: Int?
    
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
    var phone: String = ""
    var address: String = ""
    var relationShip = ""
    var relationShipName = ""
    var provinceId: String = ""
    var provinceName: String = ""
    var districtId: String = ""
    var districtName: String = ""
    var wardId: String = ""
    var wardName: String = ""
    
    var isMatchChildAddress: Bool = false
    
    var fullAddress: String {
        var fullAddress = ""
        if address != "" {
            fullAddress += address + " - "
        }
        fullAddress += wardName + " - " + districtName + " - " + provinceName
        
        return fullAddress
    }
    
    var prisonerModel: PrisonerModel {
        let prisonerModel = PrisonerModel()
        prisonerModel.name = name
        prisonerModel.gender = gender
        prisonerModel.age = age
        prisonerModel.birthDay = birthday
        prisonerModel.phone = phone
        prisonerModel.relationShip = relationShip
        prisonerModel.provinceId = provinceId
        prisonerModel.districtId = districtId
        prisonerModel.wardId = wardId
        prisonerModel.address = address
        prisonerModel.fullAddress = fullAddress
        
        prisonerModel.relationShipName = relationShipName
        prisonerModel.genderName = genderName
        prisonerModel.provinceName = provinceName
        prisonerModel.districtName = districtName
        prisonerModel.wardName = wardName
        return prisonerModel
    }
    
    let relationShipDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.relationShip.value)
    let provinceDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.province.value)
    let districtAllDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.district.value)
    let wardGroupDataSource: [WarGroupModel] = WarGroupModel.getSessionData(from: Setting.ward.value)
    
    var districtDataSource: [ComboboxItemModel] = []
    var wardDataSource: [ComboboxItemModel] = []
    
    var relationTitles: [String] {
        return relationShipDataSource.map { $0.value }
    }
    
    var provinceTitles: [String] {
        return provinceDataSource.map { $0.value }
    }
    
    var districtTitles: [String] {
        return districtDataSource.map { $0.value }
    }
    
    var wardTitles: [String] {
        return wardDataSource.map { $0.value }
    }
    
    var childs: [ChildModel] {
        return AppData.shared.getChilds()
    }
    
    var childTitles: [String] {
        return childs.map { $0.name == "" ? Strings.Step1.unknownChildName : $0.name }
    }
    
    var prisonerInfoCellViewModels: [InfoCellViewModel] {
        let prisoners = AppData.shared.getPrisoner()
            .enumerated().map { InfoCellViewModel(index: $0, title: $1.name == "" ? Strings.Step1.unknownChildName : $1.name) }
        return prisoners
    }
    
    var prisonerInfoViewIsHidden: Bool {
        if AppData.shared.getPrisoner().count <= 1 && prisonerIndex == nil {
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
        let validFieldNumber = [name, gender, birthday, phone, relationShip, provinceId, districtId, wardId, address].filter { !$0.isEmpty }.count
        
        if validFieldNumber == 0 {
            return Strings.Step2.emptyErrorTitle
        }
        
        return ""
    }
    
    func resetToFirstPrisoner() {
        updateValue(by: 0)
    }
    
    func updateNextPrisonerIndex() {
        let infoPrisonerCount = AppData.shared.getPrisoner().count
        prisonerIndex = infoPrisonerCount == 0 ? nil : infoPrisonerCount
    }
    
    func updateValue(by prisonerIndex: Int) {
        guard let prisonerModel = AppData.shared.getPrisoner(at: prisonerIndex) else { return }
        
        self.prisonerIndex = prisonerIndex
        
        name = prisonerModel.name
        gender = prisonerModel.gender
        age = prisonerModel.age
        phone = prisonerModel.phone
        birthday = prisonerModel.birthDay
        address = prisonerModel.address
        relationShip = prisonerModel.relationShip
        provinceId = prisonerModel.provinceId
        districtId = prisonerModel.districtId
        wardId = prisonerModel.wardId
        relationShipName = prisonerModel.relationShipName
        genderName = prisonerModel.genderName
        provinceName = prisonerModel.provinceName
        districtName = prisonerModel.districtName
        wardName = prisonerModel.wardName
    }
    
    func removePrisonerModelFromAppData(at prisonerIndex: Int) {
        AppData.shared.removeReportPrisonerModel(at: prisonerIndex)
    }
    
    func updateReportPrisonerModel() {
        AppData.shared.updateReportPrisonelModel(prisonerModel, at: prisonerIndex)
    }
}
