//
//  Date+Extension.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

extension Date {
    func string(from format: String) -> String {
        let dateFormatter = DateFormatter()
        dateFormatter.locale = Locale(identifier: "en_US_POSIX")
        dateFormatter.calendar = Calendar(identifier: .gregorian)
        dateFormatter.dateFormat = format
        return dateFormatter.string(from: self)
    }
}
