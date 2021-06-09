//
//  ChildViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

protocol StepForm {
    func updateFormToViewModel() -> Bool
}

class StepViewController: BaseViewController, StepForm {
    weak var containerViewController: FormViewController?
    
    func updateFormToViewModel() -> Bool {
        return true
    }
}

class StepTableViewController: BaseTableViewController, StepForm {
    func updateFormToViewModel() -> Bool {
        return true
    }
}

extension StepTableViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        textField.resignFirstResponder()
        return true
    }
}
