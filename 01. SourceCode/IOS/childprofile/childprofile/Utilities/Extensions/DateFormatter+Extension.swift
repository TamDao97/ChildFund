//
//  DateFormatter+Extension.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

extension DateFormatter {
    convenience init(dateFormat: String) {
        self.init()
        self.calendar = Calendar(identifier: .gregorian)
        self.dateFormat = dateFormat
    }
}
