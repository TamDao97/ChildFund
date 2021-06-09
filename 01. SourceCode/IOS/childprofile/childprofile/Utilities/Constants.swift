//
//  Constants.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

struct Constants {
    static let maxPasswordLength = 5
    static let profileNew = "0"
    
    //Id fix anh chị em ruột;
    static let relationshipYoungerSister = "R0006"    //Em gái
    static let relationshipOlderSister = "R0008"    //Chị
    static let relationshipYoungerBrother = "R0009"//Em trai
    static let relationshipOlderBrother = "R0010"    //Anh
    
    //cấu hình trạng thái hs trẻ
    static let profilesNew = "0"
    static let profilesConfimedLevel1 = "1"
    static let profilesConfimedLevel2 = "2"
    
    //cấu hình trạng thái báo cáo
    static let reportProfilesNew = "0"
    static let reportProfilesConfimedLevel1 = "1"
    static let reportProfilesConfimedLevel2 = "2"
    
    static let profilesIsDelete = true
    static let profilesIsUse = false
}

enum ResultStatus {
    case success
    case failed
}

enum Gender: Int {
    case female = 0
    case male
    
    var title: String {
        switch self {
        case .male:
            return "Nam"
        default:
            return "Nữ"
        }
    }
}

struct DateFormatString {
    static let yyyyMMdd = "yyyy/MM/dd"
    static let fullDateFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'"
    static let serverDateFormat = "yyyy-MM-dd'T'HH:mm:ss"
    static let shortDateFormat = "yyyy-MM-dd"
    static let extendedTimezone = "yyyy-MM-dd'T'HH:mm:ssXXX"
    static let ddMMyyyy = "dd/MM/yyyy"
}
