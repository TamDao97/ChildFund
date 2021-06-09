//
//  UITableView+Extension.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

extension UITableView {
    func scrollToTop() {
        let firstIndexPath = IndexPath(row: 0, section: 0)
        scrollToRow(at: firstIndexPath, at: .top, animated: true)
    }
}


