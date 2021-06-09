//
//  ConfirmForwardPasswordViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ConfirmForwardPasswordViewController: BaseViewController {
    @IBOutlet weak var confirmKeyTextField: UITextField!
    @IBOutlet weak var newPasswordTextField: UITextField!
    @IBOutlet weak var confirmNewPasswordTextField: UITextField!
    @IBOutlet weak var confirmKeyErrorLabel: UILabel!
    @IBOutlet weak var confirmNewPasswordLabel: UILabel!
    @IBOutlet weak var newPasswordErrorLabel: UILabel!

    var viewModel = ConfirmForwardPasswordViewModel()
    weak var loginViewController: LoginViewController?
    
    override func setupView() {
        setupTextField()
    }
    
    @IBAction func exitButtonWasTouched(_ sender: Any) {
        dismiss(animated: true, completion: nil)
    }
    
    @IBAction func updateButtonWasTouched(_ sender: Any) {
        guard
            let confirmKey = confirmKeyTextField.text,
            let newPassword = newPasswordTextField.text,
            let confirmNewPassword = confirmNewPasswordTextField.text
            else {
                return
        }
        
        viewModel.confirmKey = confirmKey
        viewModel.newPassword = newPassword
        viewModel.confirmNewPassword = confirmNewPassword
        
        confirmKeyErrorLabel.text = viewModel.confirmKeyKeyErrorTitle
        newPasswordErrorLabel.text = viewModel.newPasswordErrorTitle
        confirmNewPasswordLabel.text = viewModel.confirmNewPasswordErrorTitle
        
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
            
            let successMessage = self.viewModel.successChangePasswordTitle
            
            self.dismiss(animated: true, completion: {
                self.loginViewController?.showMessage(title: successMessage)
            })
        }
    }
}

extension ConfirmForwardPasswordViewController {
    private func setupTextField() {
        confirmKeyTextField.becomeFirstResponder()
        
        confirmKeyTextField.delegate = self
        newPasswordTextField.delegate = self
        confirmNewPasswordTextField.delegate = self
    }
}

extension ConfirmForwardPasswordViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        if confirmKeyTextField.isFirstResponder {
            newPasswordTextField.becomeFirstResponder()
        } else if newPasswordTextField.isFirstResponder {
            confirmNewPasswordTextField.becomeFirstResponder()
        } else {
            confirmNewPasswordTextField.resignFirstResponder()
        }
        return true
    }
}
