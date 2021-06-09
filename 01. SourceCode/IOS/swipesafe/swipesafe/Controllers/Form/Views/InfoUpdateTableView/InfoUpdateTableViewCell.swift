//
//  InfoUpdateTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

protocol InfoUpdateTableViewCellDelegate: class {
    func update(index: Int)
    func remove(index: Int)
}

class InfoUpdateTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    weak var delegate: InfoUpdateTableViewCellDelegate?
    static let defaultCellHeight: CGFloat = 44
    
    var infoCellViewModel: InfoCellViewModel? {
        didSet {
            guard let model = infoCellViewModel else { return }
            titleLabel.text = model.title
        }
    }
    
    @IBAction func minusAction(_ sender: Any) {
        guard let index = infoCellViewModel?.index else { return }
        delegate?.remove(index: index)
    }
    
    @IBAction func editAction(_ sender: Any) {
        guard let index = infoCellViewModel?.index else { return }
        delegate?.update(index: index)
    }
}
