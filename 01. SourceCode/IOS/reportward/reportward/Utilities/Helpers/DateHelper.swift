//
//  DateHelper.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

struct DateHelper {
    static func convert(dateString: String, fromFormat: String, toFormat: String) -> String {
        let fromDateFomatter = DateFormatter(dateFormat: fromFormat)
        guard let serverDate = fromDateFomatter.date(from: dateString) else {
            return ""
        }
        let toDateFormatter = DateFormatter(dateFormat: toFormat)
        return toDateFormatter.string(from: serverDate)
    }
    
    static func stringFromServerDateToLocalDate(_ serverDateString: String) -> String {
        return convert(dateString: serverDateString,
                       fromFormat: DateFormatString.serverDateFormat,
                       toFormat: DateFormatString.ddMMyyyy)
    }
    
    static func stringFromLocalDateToShortDate(_ localDateString: String) -> String {
        return convert(dateString: localDateString,
                       fromFormat: DateFormatString.ddMMyyyy,
                       toFormat: DateFormatString.shortDateFormat)
    }
    
    static func stringFromShortDateToLocalDate(_ serverDateString: String) -> String {
        return convert(dateString: serverDateString,
                       fromFormat: DateFormatString.shortDateFormat,
                       toFormat: DateFormatString.ddMMyyyy)
    }
    
    static func stringFromServerDateToShortDate(_ serverDateString: String) -> String {
        return convert(dateString: serverDateString,
                       fromFormat: DateFormatString.serverDateFormat,
                       toFormat: DateFormatString.shortDateFormat)
    }
    
    static func dateFromShortDate(_ serverDateString: String) -> Date? {
        let serverDateFomatter = DateFormatter(dateFormat: DateFormatString.shortDateFormat)
        return serverDateFomatter.date(from: serverDateString)
    }
}
