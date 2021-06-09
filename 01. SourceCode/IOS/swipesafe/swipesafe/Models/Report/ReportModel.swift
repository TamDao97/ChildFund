//
//  ReportModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ReportModel: Encodable {
    var id: String = ""
    var name: String = ""
    var provinceId: String = ""
    var districtId: String = ""
    var wardId: String = ""
    var address: String = ""
    var fullAddress: String = ""
    var phone: String = ""
    var email: String = ""
    var relationShip: String = ""
    var birthDay: String = ""
    var gender: String = ""
    var type: String = ""
    var status: String = "0"
    
    var description: String = ""
    
    var childs: [ChildModel] = []
    var prisoners: [PrisonerModel] = []
    
    var genderName: String = ""
    var relationShipName: String = ""
    var provinceName: String = ""
    var districtName: String = ""
    var wardName: String = ""
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case provinceId = "ProvinceId"
        case districtId = "DistrictId"
        case wardId = "WardId"
        case address = "Address"
        case fullAddress = "FullAddress"
        case phone = "Phone"
        case email = "Email"
        case relationShip = "RelationShip"
        case birthDay = "BirthDay"
        case gender = "Gender"
        case type = "Type"
        case status = "Status"
        
        case description = "Description"
        
        case childs = "ListChild"
        case prisoners = "ListPrisoner"
    }
    
    func updateValue(from reporterModel: ReporterModel) {
        id = reporterModel.id
        name = reporterModel.name
        provinceId = reporterModel.provinceId
        districtId = reporterModel.districtId
        wardId = reporterModel.wardId
        address = reporterModel.address
        fullAddress = reporterModel.fullAddress
        phone = reporterModel.phone
        email = reporterModel.email
        relationShip = reporterModel.relationShip
        relationShipName = reporterModel.relationShipName
        birthDay = reporterModel.birthDay
        gender = reporterModel.gender
        genderName = reporterModel.genderName
        type = reporterModel.type
        status = reporterModel.status
    }
    
    func getReporterInfo() -> ReporterModel {
        let reporterModel = ReporterModel()
        reporterModel.id = id
        reporterModel.name = name
        reporterModel.provinceId = provinceId
        reporterModel.districtId = districtId
        reporterModel.wardId = wardId
        reporterModel.address = address
        reporterModel.fullAddress = fullAddress
        reporterModel.phone = phone
        reporterModel.email = email
        reporterModel.relationShip = relationShip
        reporterModel.relationShipName = relationShipName
        reporterModel.birthDay = birthDay
        reporterModel.gender = gender
        reporterModel.genderName = genderName
        reporterModel.type = type
        reporterModel.status = status
        return reporterModel
    }
}
