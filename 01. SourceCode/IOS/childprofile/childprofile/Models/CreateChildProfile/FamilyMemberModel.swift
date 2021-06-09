//
//  FamilyMemberModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class FamilyMemberModel: Codable {
    var id: String
    var name: String
    var code: String
    var createBy: String
    var createDate: String
    var updateBy: String
    var updateDate: String
    var childId: String
    var dateOfBirthView: String
    var dateOfBirth: String
    var relationshipId: String
    var relationshipName: String
    var gender: Int
    var job: String
    var jobName: String
    var liveWithChild: Int
    var position: Int?
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case code = "Code"
        case createBy = "CreateBy"
        case createDate = "CreateDate"
        case updateBy = "UpdateBy"
        case updateDate = "UpdateDate"
        case childId = "ChildId"
        case dateOfBirthView = "DateOfBirthView"
        case dateOfBirth = "DateOfBirth"
        case relationshipId = "RelationshipId"
        case relationshipName = "RelationshipName"
        case gender = "Gender"
        case job = "Job"
        case jobName = "JobName"
        case liveWithChild = "LiveWithChild"
        case position = "Position"
    }
    
    init() {
        id = ""
        name = ""
        code = ""
        createBy = ""
        createDate = ""
        updateBy = ""
        updateDate = ""
        childId = ""
        dateOfBirthView = ""
        dateOfBirth = ""
        relationshipId = ""
        relationshipName = ""
        gender = Gender.male.rawValue
        job = ""
        jobName = ""
        liveWithChild = LiveWithChild.yes.rawValue
        position = nil
    }
    
    required init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.id = try container.decodeIfPresent(String.self, forKey: .id) ?? ""
        self.name = try container.decodeIfPresent(String.self, forKey: .name) ?? ""
        self.code = try container.decodeIfPresent(String.self, forKey: .code) ?? ""
        self.createBy = try container.decodeIfPresent(String.self, forKey: .createBy) ?? ""
        self.createDate = try container.decodeIfPresent(String.self, forKey: .createDate) ?? ""
        self.updateBy = try container.decodeIfPresent(String.self, forKey: .updateBy) ?? ""
        self.updateDate = try container.decodeIfPresent(String.self, forKey: .updateDate) ?? ""
        self.childId = try container.decodeIfPresent(String.self, forKey: .childId) ?? ""
        self.dateOfBirthView = try container.decodeIfPresent(String.self, forKey: .dateOfBirthView) ?? ""
        self.dateOfBirth = try container.decodeIfPresent(String.self, forKey: .dateOfBirth) ?? ""
        self.relationshipId = try container.decodeIfPresent(String.self, forKey: .relationshipId) ?? ""
        self.relationshipName = try container.decodeIfPresent(String.self, forKey: .relationshipName) ?? ""
        self.relationshipName = try container.decodeIfPresent(String.self, forKey: .relationshipName) ?? ""
        self.gender = try container.decodeIfPresent(Int.self, forKey: .gender) ?? Gender.male.rawValue
        self.job = try container.decodeIfPresent(String.self, forKey: .job) ?? ""
        self.jobName = try container.decodeIfPresent(String.self, forKey: .jobName) ?? ""
        self.liveWithChild = try container.decodeIfPresent(Int.self, forKey: .liveWithChild) ?? 0
        self.position = try container.decodeIfPresent(Int.self, forKey: .position)
    }
}
