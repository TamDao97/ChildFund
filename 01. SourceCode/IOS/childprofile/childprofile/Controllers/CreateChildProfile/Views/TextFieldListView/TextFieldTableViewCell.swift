//
//  TextFieldTableViewCell.swift
//  childprofile
//
//  Created by Thanh Luu on 1/27/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class TextFieldTableViewCell: UITableViewCell {
    var objectInputModel: ObjectInputModel? {
        didSet {
            guard let model = objectInputModel else {
                return
            }
            
            titleLabel.text = model.name
            contentTextField.text = model.value
            contentTextField.delegate = self
        }
    }
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var contentTextField: UITextField!
    
    func configure(with model: ObjectInputModel) {
        objectInputModel = model
    }
}

extension TextFieldTableViewCell: UITextFieldDelegate {
    func textFieldDidEndEditing(_ textField: UITextField) {
        guard let text = textField.text else { return }
        objectInputModel?.value = text
    }
    
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}
