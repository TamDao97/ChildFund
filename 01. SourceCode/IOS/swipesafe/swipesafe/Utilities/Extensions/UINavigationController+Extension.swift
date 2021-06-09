//
//  UINavigationController+Extension.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

extension UINavigationController {
    func updateAppearance(backgroundColor: UIColor = .white, titleColor: UIColor = .black) {
        navigationBar.barTintColor = backgroundColor
        
        let textAttributes = [NSAttributedString.Key.foregroundColor: titleColor]
        navigationBar.titleTextAttributes = textAttributes
    }
}


