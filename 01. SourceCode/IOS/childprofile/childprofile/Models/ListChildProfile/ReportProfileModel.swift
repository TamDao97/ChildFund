//
//  ReportProfileModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/26/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ReportProfileModel: Codable {
    var id: String?
    var name: String?
    var code: String?
    var createBy: String?
    var createDate: String?
    var updateBy: String?
    var updateDate: String?
    
    var content: String?
    var childProfileId: String?
    var status: String?
    var isDelete: Bool?
    
    init(content: String, childProfileId: String, status: String, isDelete: Bool, createBy: String, updateBy: String) {
        self.content = content
        self.childProfileId = childProfileId
        self.status = status
        self.isDelete = isDelete
        self.createBy = createBy
        self.updateBy = updateBy
    }
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case code = "Code"
        case createBy = "CreateBy"
        case createDate = "CteateDate"
        case updateBy = "UpdateBy"
        case updateDate = "UpdateDate"
        case content = "Content"
        case childProfileId = "ChildProfileId"
        case status = "Status"
        case isDelete = "IsDelete"
    }
}
