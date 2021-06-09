//
//  RightTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

struct RightCellViewModel {
    var id: Int
    let title: String
    let date: String
    let fileUrl: String
    
    var fileName: String {
        return "File\(id).pdf"
    }
}

class RightTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var dateLabel: UILabel!
    
    @IBOutlet weak var statusImageView: UIImageView!
    
    func configure(viewModel: RightCellViewModel) {
        titleLabel.text = viewModel.title
        dateLabel.text = viewModel.date
    }
}
