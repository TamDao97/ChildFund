//
//  Constants.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

struct Constants {
    static let maximumChildAge = 18
    
    static let childSOSNumber = "111"
    static let policeNumber = "113"
    static let ambulanceNumber = "115"
    
    static let childOrganizationLink = "http://treem.gov.vn/"
    static let childFundLink = "http://www.childfund.org.vn/"
    static let unicefLink = "https://www.unicef.org/"
    
    static let maxReportSize = 100.0
}

enum ResultStatus {
    case success
    case failed
}

enum Gender: Int {
    case female = 0
    case male
    case unknown
    
    var title: String {
        switch self {
        case .male:
            return "Nam"
        case .female:
            return "Nữ"
        default:
            return "Không biết"
        }
    }
}

enum Level: Int {
    case first = 0
    case second
    case third
    
    var title: String {
        switch self {
        case .first:
            return "Cần quan tâm"
        case .second:
            return "Nghiêm trọng"
        case .third:
            return "Rất nghiêm trọng"
        }
    }
}

enum ReporterType {
    static let invisible = "1"
    static let open = "0"
}

struct DateFormatString {
    static let yyyyMMdd = "yyyy/MM/dd"
    static let fullDateFormat = "yyyy-MM-dd'T'HH:mm:ss'Z'"
    static let serverDateFormat = "yyyy-MM-dd'T'HH:mm:ss"
    static let shortDateFormat = "yyyy-MM-dd"
    static let extendedTimezone = "yyyy-MM-dd'T'HH:mm:ssXXX"
    static let ddMMyyyy = "dd/MM/yyyy"
    static let ddMMyyyyHHmm = "dd/MM/yyyy HH:mm"
    static let yyyyMMddLine = "yyyy-MM-dd"
}
