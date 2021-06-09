//
//  UIApplication+Extension.swift
//  childprofile
//
//  Created by Thanh Luu on 1/18/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

extension UIApplication {
    class func topViewController(base: UIViewController? = (UIApplication.shared.delegate as? AppDelegate)?.window?.rootViewController) -> UIViewController? {
        if let navigationController = base as? UINavigationController {
            return topViewController(base: navigationController.visibleViewController)
        }
        if let tabBarController = base as? UITabBarController {
            if let selected = tabBarController.selectedViewController {
                return topViewController(base: selected)
            }
        }
        if let presented = base?.presentedViewController {
            return topViewController(base: presented)
        }
        return base
    }
}
