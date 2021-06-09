//
//  ForwardPasswordViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ForwardPasswordViewController: BaseViewController {
    @IBOutlet weak var usernameTextField: UITextField!
    @IBOutlet weak var emailTextField: UITextField!
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var emailErrorLabel: UILabel!
    
    private let viewModel = ForwardPasswordViewModel()
    weak var loginViewController: LoginViewController?
    
    override func setupView() {
        setupTextField()
    }
    
    @IBAction func exitButtonWasTouched(_ sender: Any) {
        dismiss(animated: true, completion: nil)
    }
    
    @IBAction func confirmButtonWasTouched(_ sender: Any) {
        guard
            let username = usernameTextField.text,
            let email = emailTextField.text
            else {
                return
        }
        
        viewModel.username = username
        viewModel.email = email
        
        usernameErrorLabel.text = viewModel.usernameErrorTitle
        emailErrorLabel.text = viewModel.emailErrorTitle
        
        guard viewModel.isFormValid else {
            return
        }
        
        showHUD()
        viewModel.request() { [weak self] result in
            guard let self = self else { return }
            
            self.dimissHUD()
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            let confirmForwardPasswordViewController = ConfirmForwardPasswordViewController.instance()
            confirmForwardPasswordViewController.viewModel.model = self.viewModel.nextModel
            confirmForwardPasswordViewController.loginViewController = self.loginViewController
            
            self.dismiss(animated: true, completion: {
                self.loginViewController?.present(confirmForwardPasswordViewController, animated: true, completion: nil)
            })
        }
    }
}

extension ForwardPasswordViewController {
    private func setupTextField() {
        usernameTextField.becomeFirstResponder()
        
        usernameTextField.delegate = self
        emailTextField.delegate = self
    }
}

extension ForwardPasswordViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        if usernameTextField.isFirstResponder {
            emailTextField.becomeFirstResponder()
        } else {
            emailTextField.resignFirstResponder()
        }
        return true
    }
}
