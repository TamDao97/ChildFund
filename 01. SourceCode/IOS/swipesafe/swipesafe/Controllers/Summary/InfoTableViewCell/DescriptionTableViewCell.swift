//
//  DescriptionTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/25/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class DescriptionTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!

    @IBOutlet weak var descriptionLabel: UILabel!
    
    weak var delegate: SelectUpdateReportDelegate?
    
    func config(title: String) {
        titleLabel.text = SummaryContentType.description.title
        descriptionLabel.text = title
    }
    
    @IBAction func updateAction(_ sender: Any) {
        delegate?.updateAction(type: .description, index: 0)
    }
}
