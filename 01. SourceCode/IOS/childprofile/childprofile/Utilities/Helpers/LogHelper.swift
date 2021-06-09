//
//  LogHelper.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation

public func log<T>(_ object: T, _ file: String = #file, _ function: String = #function, _ line: Int = #line) {
    let fileName = ((file as NSString).lastPathComponent as NSString).deletingPathExtension
    let currentDateString = Date().string(from: DateFormatString.fullDateFormat)
    print("\(currentDateString) | \(fileName) | line: \(line) | \(object)")
}
