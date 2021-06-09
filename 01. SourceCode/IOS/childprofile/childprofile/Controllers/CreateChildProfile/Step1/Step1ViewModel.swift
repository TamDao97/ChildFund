//
//  Step1ViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class Step1ViewModel {
    var employeeName: String = ""
    var infoDate: String = ""
    var programCode: String = ""
    var childCode: String = ""
    var orderNumber: Int?
    var nationId: String = ""
    var provinceId: String = ""
    var districtId: String = ""
    var wardId: String = ""
    var religionId: String = ""
    var address: String = ""
    var imagePath: String = ""
    
    var childImageData: Data?
    
    let nationDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.nation.value)
    let religionDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.religion.value)
    let provinceDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.province.value)
    let districtDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.district.value)
    let wardDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.ward.value)
    let programCodeDataSource: [ComboboxItemModel] = [
        ComboboxItemModel(id: "199", value: "199"),
        ComboboxItemModel(id: "213", value: "213")
    ]
    
    var employeeNameErrorTitle: String {
        return employeeName.isEmpty ? Strings.emptyError : ""
    }
    
    var infoDateErrorTitle: String {
        return infoDate.isEmpty ? Strings.emptyError : ""
    }
    
    var programCodeErrorTitle: String {
        return programCode.isEmpty ? Strings.emptyError : ""
    }
    
    var childCodeErrorTitle: String {
        return childCode.isEmpty ? Strings.emptyError : ""
    }
    
    var orderNumberErrorTitle: String {
        return orderNumber == nil ? Strings.emptyError : ""
    }
    
    var nationErrorTitle: String {
        return nationId.isEmpty ? Strings.emptyError : ""
    }
    
    var provinceErrorTitle: String {
        return provinceId.isEmpty ? Strings.emptyError : ""
    }
    
    var districtErrorTitle: String {
        return districtId.isEmpty ? Strings.emptyError : ""
    }
    
    var wardErrorTitle: String {
        return wardId.isEmpty ? Strings.emptyError : ""
    }
    
    var isFormValid: Bool {
        return employeeNameErrorTitle.isEmpty
            && infoDateErrorTitle.isEmpty
            && programCodeErrorTitle.isEmpty
            && childCodeErrorTitle.isEmpty
            && orderNumberErrorTitle.isEmpty
            && nationErrorTitle.isEmpty
            && provinceErrorTitle.isEmpty
            && districtErrorTitle.isEmpty
            && wardErrorTitle.isEmpty
    }
    
    func setImageData(image: UIImage) {
        childImageData = image.jpeg(.medium)
    }
}
