//
//  ChildProfileTableViewCell.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

protocol ChildProfileCellDelegate: class {
    func report(index: Int)
    func edit(index: Int)
}

class ChildProfileTableViewCell: UITableViewCell {
    @IBOutlet weak var avatarImageView: UIImageView!
    @IBOutlet weak var codeLabel: UILabel!
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var addressLabel: UILabel!
    @IBOutlet weak var statusLabel: UILabel!
    
    weak var delegate: ChildProfileCellDelegate?
    var index = 0
    
    func config(with model: ChildProfileCellViewModel, index: Int) {
        self.index = index
        avatarImageView.setImage(urlString: model.imageUrl)
        codeLabel.text = model.code
        nameLabel.text = model.name
        addressLabel.text = model.address
        statusLabel.text = model.status
    }
    
    @IBAction func editProfileButtonWasTouched(_ sender: Any) {
        delegate?.edit(index: index)
    }
    
    @IBAction func reportButtonWasTouched(_ sender: Any) {
        delegate?.report(index: index)
    }
}
