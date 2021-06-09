//
//  URL+Extension.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/3/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

extension URL {
    func open() {
        guard UIApplication.shared.canOpenURL(self) else { return }
        if #available(iOS 10, *) {
            UIApplication.shared.open(self)
        } else {
            UIApplication.shared.openURL(self)
        }
    }
}

