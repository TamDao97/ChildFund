//
//  String+Extension.swift
//  childprofile
//
//  Created by Thanh Luu on 1/10/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

extension String {
    var image: UIImage {
        return UIImage(named: self) ?? UIImage()
    }
    
    var isValidEmail: Bool {
        let emailRegEx = "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,64}"
        let emailTest = NSPredicate(format:"SELF MATCHES %@", emailRegEx)
        return emailTest.evaluate(with: self)
    }
}
