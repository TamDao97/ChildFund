//
//  ChildProfileSearchCondition.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ChildProfileSearchCondition: Encodable {
    var name: String = ""
    var childCode: String = ""
    var provinceId: String = ""
    var districtId: String = ""
    var wardId: String = ""
    var dateFrom: String = ""
    var dateTo: String = ""
    var createBy: String = ""
    var userId: String = ""
    var export: Int = 0
    var address: String = ""
    
    var pageSize: Int = 1
    var pageNumber: Int = 1
    var orderBy: String = ""
    var orderType: Bool = false
    
    init(userId: String, orderType: Bool, orderBy: String, pageNumber: Int, pageSize: Int) {
        self.userId = userId
        self.orderType = orderType
        self.orderBy = orderBy
        self.pageNumber = pageNumber
        self.pageSize = pageSize
    }
    
    enum CodingKeys: String, CodingKey {
        case name = "Name"
        case childCode = "ChildCode"
        case provinceId = "ProvinceId"
        case districtId = "DistrictId"
        case wardId = "WardId"
        case dateFrom = "DateFrom"
        case dateTo = "DateTo"
        case createBy = "CreateBy"
        case userId = "UserId"
        case export = "Export"
        case address = "Address"
        case pageSize = "PageSize"
        case pageNumber = "PageNumber"
        case orderBy = "OrderBy"
        case orderType = "OrderType"
    }
}
