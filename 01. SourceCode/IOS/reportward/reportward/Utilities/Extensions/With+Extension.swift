//
//  With+Extension.swift
//  childprofile
//
//  Created by Thanh Luu on 1/10/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

protocol With {}

extension With where Self: Any {
    @discardableResult
    func with(_ block: (Self) -> Void) -> Self {
        block(self)
        return self
    }
}

extension NSObject: With {}
