//
//  Toast.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import Toast_Swift

extension UIViewController {
    func showMessage(title: String?, position: ToastPosition = .center) {
        UIApplication.shared.keyWindow?.makeToast(title, position: position)
    }
}
