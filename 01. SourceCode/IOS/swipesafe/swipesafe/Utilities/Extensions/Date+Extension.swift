//
//  Date+Extension.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

extension Calendar {
    static var gregorian = Calendar(identifier: .gregorian)
}

extension Date {
    func string(from format: String) -> String {
        let dateFormatter = DateFormatter()
        dateFormatter.locale = Locale(identifier: "en_US_POSIX")
        dateFormatter.calendar = Calendar(identifier: .gregorian)
        dateFormatter.dateFormat = format
        return dateFormatter.string(from: self)
    }
    
    var year: Int {
        return Calendar.gregorian.component(.year, from: self)
    }
    
    var month: Int {
        return Calendar.gregorian.component(.month, from: self)
    }
    
    var day: Int {
        return Calendar.gregorian.component(.day, from: self)
    }
    
    var hour: Int {
        return Calendar.gregorian.component(.hour, from: self)
    }
    
    var minute: Int {
        return Calendar.gregorian.component(.minute, from: self)
    }
    
    var second: Int {
        return Calendar.gregorian.component(.second, from: self)
    }
    
    var previousYear: Date {
        return Calendar.gregorian.date(byAdding: .year, value: -1, to: self) ?? self
    }
    
    var nextYear: Date {
        return Calendar.gregorian.date(byAdding: .year, value: 1, to: self) ?? self
    }
    
    var previousMonth: Date {
        return Calendar.gregorian.date(byAdding: .month, value: -1, to: self) ?? self
    }
    
    var nextMonth: Date {
        return Calendar.gregorian.date(byAdding: .month, value: 1, to: self) ?? self
    }
    
    var yesterday: Date {
        return Calendar.gregorian.date(byAdding: .day, value: -1, to: self) ?? self
    }
    
    var tomorrow: Date {
        return Calendar.gregorian.date(byAdding: .day, value: 1, to: self) ?? self
    }
    
    var previousHour: Date {
        return Calendar.gregorian.date(byAdding: .hour, value: -1, to: self) ?? self
    }
    
    var nextHour: Date {
        return Calendar.gregorian.date(byAdding: .hour, value: 1, to: self) ?? self
    }
    
    var previousSecond: Date {
        return Calendar.gregorian.date(byAdding: .second, value: -1, to: self) ?? self
    }
    
    var startOfHour: Date {
        let components = Calendar.gregorian.dateComponents([.year, .month, .day, .hour], from: self)
        return Calendar.gregorian.date(from: components) ?? self
    }
    
    var startOfYear: Date {
        let components = Calendar.gregorian.dateComponents([.year], from: self)
        return Calendar.gregorian.date(from: components) ?? self
    }
    
    var endOfYear: Date {
        return startOfYear.nextYear.previousSecond
    }
    
    var startOfMonth: Date {
        let components = Calendar.gregorian.dateComponents([.year, .month], from: self)
        return Calendar.gregorian.date(from: components) ?? self
    }
    
    var endOfMonth: Date {
        return startOfMonth.nextMonth.previousSecond
    }
    
    var startOfDay: Date {
        let components = Calendar.gregorian.dateComponents([.year, .month, .day], from: self)
        return Calendar.gregorian.date(from: components) ?? self
    }
    
    var endOfDay: Date {
        return startOfDay.tomorrow.previousSecond
    }
    
    var isLastDayOfMonth: Bool {
        return tomorrow.month != self.month
    }
    
    var numberOfDaysInMonth: Int {
        return Calendar.gregorian.range(of: .day, in: .month, for: self)?.count ?? 0
    }
    
    func subtract(days: Int) -> Date {
        return Calendar.gregorian.date(byAdding: .day, value: -days, to: self) ?? self
    }
    
    func add(days: Int) -> Date {
        return Calendar.gregorian.date(byAdding: .day, value: days, to: self) ?? self
    }
    
    func subtract(years: Int) -> Date {
        return Calendar.gregorian.date(byAdding: .year, value: -years, to: self) ?? self
    }
}
