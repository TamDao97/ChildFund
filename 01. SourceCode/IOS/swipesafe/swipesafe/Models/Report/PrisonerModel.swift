//
//  PrisonerModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class PrisonerModel: Encodable {
    var id: String = ""
    var reportId: String = ""
    var name: String = ""
    var gender: String = ""
    var age: Int?
    var birthDay: String = ""
    var phone: String = ""
    var relationShip: String = ""
    var provinceId: String = ""
    var districtId: String = ""
    var wardId: String = ""
    var address: String = ""
    var fullAddress: String = ""
    
    var genderName: String = ""
    var relationShipName: String = ""
    var provinceName: String = ""
    var districtName: String = ""
    var wardName: String = ""
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case reportId = "ReportId"
        case name = "Name"
        case gender = "Gender"
        case age = "Age"
        case birthDay = "BirthDay"
        case phone = "Phone"
        case relationShip = "RelationShip"
        case provinceId = "ProvinceId"
        case districtId = "DistrictId"
        case wardId = "WardId"
        case address = "Address"
        case fullAddress = "FullAddress"
    }
}
