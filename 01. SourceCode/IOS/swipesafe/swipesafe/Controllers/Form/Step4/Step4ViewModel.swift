//
//  Step4ViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/24/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class Step4ViewModel {
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
    var phone: String = ""
    var mail: String = ""
    var address: String = ""
    var relationShip = ""
    var relationShipName = ""
    var provinceId: String = ""
    var provinceName: String = ""
    var districtId: String = ""
    var districtName: String = ""
    var wardId: String = ""
    var wardName: String = ""
    
    var type: String = ""
    var isInvisible: Bool {
        if type == ReporterType.invisible {
            return true
        }
        return false
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
    
    var fullAddress: String {
        var fullAddress = ""
        if address != "" {
            fullAddress += address + " - "
        }
        fullAddress += wardName + " - " + districtName + " - " + provinceName
        
        return fullAddress
    }
    
    var reporterModel: ReporterModel {
        let reporterModel = ReporterModel()
        reporterModel.type = type
        if type == ReporterType.invisible {
            return reporterModel
        }
        reporterModel.name = name
        reporterModel.provinceId = provinceId
        reporterModel.districtId = districtId
        reporterModel.wardId = wardId
        reporterModel.address = address
        reporterModel.fullAddress = fullAddress
        reporterModel.phone = phone
        reporterModel.email = mail
        reporterModel.relationShip = relationShip
        reporterModel.relationShipName = relationShipName
        reporterModel.gender = gender
        reporterModel.genderName = genderName
        return reporterModel
    }
    
    var errorFormMessage: String {
        guard type != ReporterType.invisible else {
            return ""
        }
        
        // TODO: Remove validate here
//        if name.isEmpty {
//            return Strings.Step1.nameEmptyErrorTitle
//        }
//
//        if !mail.isEmpty && !mail.isValidEmail {
//            return Strings.Step4.emailInValidErrorTitle
//        }
//
//        if districtId.isEmpty {
//            return Strings.Step1.districtEmptyErrorTitle
//        }
//
//        if provinceId.isEmpty {
//            return Strings.Step1.provinceEmptyErrorTitle
//        }
//
//        if wardId.isEmpty {
//            return Strings.Step1.wardEmptyErrorTitle
//        }
        
        return ""
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
        guard let wardGroupData = wardGroupDataSource.first(where: { $0.districtId == districtId }) else {
            return
        }
        wardDataSource = wardGroupData.listWard
    }
    
    func updateType(isInvisible: Bool) {
        type = isInvisible ? ReporterType.invisible : ReporterType.open
    }
    
    func updateValueFromAppData() {
        let reporterModel = AppData.shared.getReporterInfo()
        name = reporterModel.name
        gender = reporterModel.gender
        phone = reporterModel.phone
        mail = reporterModel.email
        address = reporterModel.address
        relationShip = reporterModel.relationShip
        provinceId = reporterModel.provinceId
        districtId = reporterModel.districtId
        wardId = reporterModel.wardId
        type = reporterModel.type
    }
    
    func updateReporterModel() {
        AppData.shared.updateInfoReporter(from: reporterModel)
    }
}
