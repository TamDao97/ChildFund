//
//  LoginProfileModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct LoginProfileModel: Decodable {
    var id: String
    var name: String?
    var username: String?
    var userLevel: String?
    var areaUserid: String?
    var areaDistrictId: String?
    var isDisable: Bool?
    var listRoles: [String]?
    var imagePath: String?
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case username = "UserName"
        case userLevel = "UserLever"
        case areaUserid = "AreaUserId"
        case areaDistrictId = "AreaDistrictId"
        case isDisable = "IsDisable"
        case listRoles = "ListRoles"
        case imagePath = "ImagePath"
    }
}
