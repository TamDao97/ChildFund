//
//  SearchViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class SearchViewController: BaseViewController {
    @IBOutlet weak var codeTextField: UITextField!
    @IBOutlet weak var nameTextField: UITextField!
    @IBOutlet weak var addressTextField: UITextField!
    
    var viewModel = SearchViewModel()
    var completion: ((SearchViewModel) -> Void)?
    
    static func show(from viewController: UIViewController, viewModel: SearchViewModel, completion: @escaping (SearchViewModel) -> Void) {
        let searchViewController = SearchViewController.instance()
        searchViewController.viewModel = viewModel
        searchViewController.completion = completion
        viewController.present(searchViewController, animated: true, completion: nil)
    }
    
    override func setupView() {
        // Form
        codeTextField.delegate = self
        nameTextField.delegate = self
        addressTextField.delegate = self
        codeTextField.becomeFirstResponder()
        
        // Bind viewModel
        codeTextField.text = viewModel.code
        nameTextField.text = viewModel.name
        addressTextField.text = viewModel.address
    }
    
    @IBAction func closeButtonWasTouched(_ sender: Any) {
        dismiss(animated: true, completion: nil)
    }
    
    
    @IBAction func searchButtonWasTouched(_ sender: Any) {
        viewModel.code = codeTextField.text ?? ""
        viewModel.name = nameTextField.text ?? ""
        viewModel.address = addressTextField.text ?? ""
        
        completion?(viewModel)
        dismiss(animated: true, completion: nil)
    }
}

extension SearchViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        if codeTextField.isFirstResponder {
            nameTextField.becomeFirstResponder()
        } else if nameTextField.isFirstResponder {
            addressTextField.becomeFirstResponder()
        } else {
            addressTextField.resignFirstResponder()
        }
        return true
    }
}
