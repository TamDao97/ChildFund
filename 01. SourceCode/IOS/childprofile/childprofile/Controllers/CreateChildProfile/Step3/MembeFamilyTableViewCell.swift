//
//  MembeFamilyTableViewCell.swift
//  childprofile
//
//  Created by Thanh Luu on 1/22/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

struct FamilyMemberCellViewModel {
    let name: String
    let gender: String
    let relation: String
    let birthday: String
    let liveWithChildren: String
}

protocol FamilyMemberCellDelegate: class {
    func delete(index: Int)
    func edit(index: Int)
}

class MembeFamilyTableViewCell: UITableViewCell {

    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var genderLabel: UILabel!
    @IBOutlet weak var relationLabel: UILabel!
    @IBOutlet weak var birthDayLabel: UILabel!
    @IBOutlet weak var liveWithChildLabel: UILabel!
    
    weak var delegate: FamilyMemberCellDelegate?
    var index = 0
    
    func configure(viewModel: FamilyMemberCellViewModel, index: Int) {
        self.index = index
        nameLabel.text = viewModel.name
        genderLabel.text = viewModel.gender
        relationLabel.text = viewModel.relation
        birthDayLabel.text = viewModel.birthday
        liveWithChildLabel.text = viewModel.liveWithChildren
    }
    
    @IBAction func deleteButtonWasTouched(_ sender: Any) {
        delegate?.delete(index: index)
    }
    
    @IBAction func editButtonWasTouched(_ sender: Any) {
        delegate?.edit(index: index)
    }
}
