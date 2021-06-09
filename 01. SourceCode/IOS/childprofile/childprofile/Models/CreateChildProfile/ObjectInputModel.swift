//
//  ObjectInputModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ObjectInputModel: Codable {
    var id: String
    var isCheck: Bool
    var name: String
    var nameEng: String
    var value: String
    var otherName: String
    var otherValue: String
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case isCheck = "Check"
        case name = "Name"
        case nameEng = "NameEN"
        case value = "Value"
        case otherName = "OtherName"
        case otherValue = "OtherValue"
    }
    
    required init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.id = try container.decodeIfPresent(String.self, forKey: .id) ?? ""
        self.isCheck = try container.decodeIfPresent(Bool.self, forKey: .isCheck) ?? false
        self.name = try container.decodeIfPresent(String.self, forKey: .name) ?? ""
        self.nameEng = try container.decodeIfPresent(String.self, forKey: .nameEng) ?? ""
        self.value = try container.decodeIfPresent(String.self, forKey: .value) ?? ""
        self.otherName = try container.decodeIfPresent(String.self, forKey: .otherName) ?? ""
        self.otherValue = try container.decodeIfPresent(String.self, forKey: .otherValue) ?? ""
    }
}
