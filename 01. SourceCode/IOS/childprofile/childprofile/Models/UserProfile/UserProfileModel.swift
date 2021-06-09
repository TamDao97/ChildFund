//
//  UserProfileModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct UserProfileModel: Codable {
    var id: String
    var name: String?
    var dateOfBirth: String?
    var phoneNumber: String?
    var email: String?
    var imagePath: String?
    var gender: Int?
    var identifyNumber: String?
    var address: String?
    var updateBy: String?
    var updateDate: String?
    
    init(id: String, name: String, dateOfBirth: String, phoneNumber: String, email: String, imagePath: String, gender: Int, identifyNumber: String, address: String) {
        self.id = id
        self.name = name
        self.dateOfBirth = dateOfBirth
        self.phoneNumber = phoneNumber
        self.email = email
        self.imagePath = imagePath
        self.gender = gender
        self.identifyNumber = identifyNumber
        self.address = address
    }
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case dateOfBirth = "DateOfBirth"
        case phoneNumber = "PhoneNumber"
        case email = "Email"
        case imagePath = "ImagePath"
        case gender = "Gender"
        case identifyNumber = "IdentifyNumber"
        case address = "Address"
        case updateBy = "UpdateBy"
        case updateDate = "UpdateDate"
    }
}
