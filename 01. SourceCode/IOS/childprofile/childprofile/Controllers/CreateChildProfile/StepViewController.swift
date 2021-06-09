//
//  StepViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

protocol StepForm {
    func updateFormToViewModel() -> Bool
}

class StepViewController: BaseTableViewController, StepForm {
    func updateFormToViewModel() -> Bool {
        return true
    }
}

extension StepViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}
