//
//  Constants.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

struct Constants {
    
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
