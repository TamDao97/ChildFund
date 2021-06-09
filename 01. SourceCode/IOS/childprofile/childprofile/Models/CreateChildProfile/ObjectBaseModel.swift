//
//  ObjectBaseModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ObjectBaseModel: Codable {
    var listObject: [ObjectInputModel]
    var otherName: String
    var otherValue: String
    var otherName2: String
    var otherValue2: String
    
    enum CodingKeys: String, CodingKey {
        case listObject = "ListObject"
        case otherName = "OtherName"
        case otherValue = "OtherValue"
        case otherName2 = "OtherName2"
        case otherValue2 = "OtherValue2"
    }
    
    required init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.listObject = try container.decodeIfPresent([ObjectInputModel].self, forKey: .listObject) ?? []
        self.otherName = try container.decodeIfPresent(String.self, forKey: .otherName) ?? ""
        self.otherValue = try container.decodeIfPresent(String.self, forKey: .otherValue) ?? ""
        self.otherName2 = try container.decodeIfPresent(String.self, forKey: .otherName2) ?? ""
        self.otherValue2 = try container.decodeIfPresent(String.self, forKey: .otherValue2) ?? ""
    }
}
