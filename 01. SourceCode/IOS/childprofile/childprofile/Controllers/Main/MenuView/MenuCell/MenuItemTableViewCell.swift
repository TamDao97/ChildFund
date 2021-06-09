//
//  MenuItemTableViewCell.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

struct MenuItem {
    let id: Int
    let title: String
    let imageName: String
    let isTitle: Bool
    
    
    init(id: Int, title: String, imageName: String, isTitle: Bool = false) {
        self.id = id
        self.title = title
        self.imageName = imageName
        self.isTitle = isTitle
    }
}

class MenuItemTableViewCell: UITableViewCell {
    @IBOutlet weak var menuImageView: UIImageView!
    @IBOutlet weak var titleLabel: UILabel!
    
    @IBOutlet weak var menuTitleLabel: UILabel!
    @IBOutlet weak var titleView: UIView!
    
    func bind(menuItem: MenuItem) {
        guard !menuItem.isTitle else {
            titleView.isHidden = false
            menuTitleLabel.text = menuItem.title
            return
        }
        
        titleView.isHidden = true
        
        titleLabel.text = menuItem.title
        menuImageView.image = menuItem.imageName.image
    }
}
