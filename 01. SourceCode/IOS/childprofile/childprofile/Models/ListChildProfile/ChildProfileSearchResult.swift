//
//  ChildProfileSearchResult.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ChildProfileSearchResult: Decodable {
    var avatar: String
    var id: String
    var name: String
    var religionName: String
    var programCode: String
    var nationName: String
    var status: String
    var childCode: String
    var provinceId: String
    var districtId: String
    var wardId: String
    var address: String
    var dateOfBirth: String
    var gender: String
    var approveDate: String
    var approveName: String
    var createBy: String
    var createDate: String
    
    enum CodingKeys: String, CodingKey {
        case avatar = "Avata"
        case id = "Id"
        case name = "Name"
        case religionName = "ReligionName"
        case programCode = "ProgramCode"
        case nationName = "NationName"
        case status = "Status"
        case childCode = "ChildCode"
        case provinceId = "ProvinceId"
        case districtId = "DistrictId"
        case wardId = "WardId"
        case address = "Address"
        case dateOfBirth = "DateOfBirth"
        case gender = "Gender"
        case approveDate = "ApproveDate"
        case approveName = "ApproverName"
        case createBy = "CreateBy"
        case createDate = "CreateDate"
    }
    
    init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.avatar = try container.decodeIfPresent(String.self, forKey: .avatar) ?? ""
        self.id = try container.decodeIfPresent(String.self, forKey: .id) ?? ""
        self.name = try container.decodeIfPresent(String.self, forKey: .name) ?? ""
        self.religionName = try container.decodeIfPresent(String.self, forKey: .religionName) ?? ""
        self.programCode = try container.decodeIfPresent(String.self, forKey: .programCode) ?? ""
        self.nationName = try container.decodeIfPresent(String.self, forKey: .nationName) ?? ""
        self.status = try container.decodeIfPresent(String.self, forKey: .status) ?? ""
        self.childCode = try container.decodeIfPresent(String.self, forKey: .childCode) ?? ""
        self.provinceId = try container.decodeIfPresent(String.self, forKey: .provinceId) ?? ""
        self.districtId = try container.decodeIfPresent(String.self, forKey: .districtId) ?? ""
        self.wardId = try container.decodeIfPresent(String.self, forKey: .wardId) ?? ""
        self.address = try container.decodeIfPresent(String.self, forKey: .address) ?? ""
        self.dateOfBirth = try container.decodeIfPresent(String.self, forKey: .dateOfBirth) ?? ""
        self.gender = try container.decodeIfPresent(String.self, forKey: .gender) ?? ""
        self.approveDate = try container.decodeIfPresent(String.self, forKey: .approveDate) ?? ""
        self.approveName = try container.decodeIfPresent(String.self, forKey: .approveName) ?? ""
        self.createBy = try container.decodeIfPresent(String.self, forKey: .createBy) ?? ""
        self.createDate = try container.decodeIfPresent(String.self, forKey: .createDate) ?? ""
    }
}
