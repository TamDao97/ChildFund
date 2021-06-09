//
//  AbuseDetailTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class AbuseDetailTableViewCell: UITableViewCell {
    static let defaultHeight: CGFloat = 33

    @IBOutlet weak var titleLabel: UILabel!
    
    func config(title: String) {
        titleLabel.text = title
    }
}
