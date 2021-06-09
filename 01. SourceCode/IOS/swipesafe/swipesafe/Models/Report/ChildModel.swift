//
//  ChildModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ChildModel: Encodable {
    var id: String = ""
    var reportId: String = ""
    var name: String = ""
    var gender: String = ""
    var age: Int?
    var birthDay: String = ""
    var level: String = ""
    var address: String = ""
    var provinceId: String = ""
    var districtId: String = ""
    var wardId: String = ""
    var fullAddress: String = ""
    var dateAction: String = ""
    var abuses: [ChildAbuseModel] = []
    
    var genderName: String = ""
    var levelName: String = ""
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
        case level = "Level"
        case address = "Address"
        case provinceId = "ProvinceId"
        case districtId = "DistrictId"
        case wardId = "WardId"
        case fullAddress = "FullAddress"
        case dateAction = "DateAction"
        case abuses = "ListAbuse"
    }
}
