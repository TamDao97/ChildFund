//
//  UIImage+Extension.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

extension UIImage {
    enum JPEGQuality: CGFloat {
        case lowest  = 0
        case low     = 0.25
        case medium  = 0.5
        case high    = 0.75
        case highest = 1
    }
    
    func jpeg(_ jpegQuality: JPEGQuality) -> Data? {
        return jpegData(compressionQuality: jpegQuality.rawValue)
    }
}
