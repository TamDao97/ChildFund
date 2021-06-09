//
//  HUD.swift
//  childprofile
//
//  Created by Thanh Luu on 1/11/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import JGProgressHUD

class HUD {
    static let shared = HUD()
    private init() {}
    
    let loadingTitle = "Vui lòng chờ..."
    
    let view = JGProgressHUD(style: .dark)
    
    func show(in containerView: UIView, title: String? = nil) {
        view.textLabel.text = title ?? loadingTitle
        view.show(in: containerView)
    }
    
    func dimiss() {
        view.dismiss()
    }
}

extension UIViewController {
    func showHUD() {
        HUD.shared.show(in: self.view)
    }
    
    func dimissHUD() {
        HUD.shared.dimiss()
    }
}
