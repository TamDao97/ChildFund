//
//  SkillTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

struct SkillCellViewModel {
    let title: String
    let date: String
    let content: String
    let image: UIImage
    let htmlFileName: String
}

class SkillTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var dateLabel: UILabel!
    @IBOutlet weak var contentLabel: UILabel!
    @IBOutlet weak var skillImageView: UIImageView!
    
    func configure(viewModel: SkillCellViewModel) {
        titleLabel.text = viewModel.title
        dateLabel.text = viewModel.date
        contentLabel.text = viewModel.content
        skillImageView.image = viewModel.image
    }
}
