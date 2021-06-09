//
//  ChangePasswordViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class ChangePasswordViewController: BaseViewController {
    @IBOutlet weak var passwordTextField: UITextField!
    @IBOutlet weak var newPasswordTextField: UITextField!
    @IBOutlet weak var confirmNewPasswordTextField: UITextField!
    
    @IBOutlet weak var passwordErrorLabel: UILabel!
    @IBOutlet weak var newPasswordErrorLabel: UILabel!
    @IBOutlet weak var confirmNewPasswordLabel: UILabel!
    
    var viewModel = ChangePasswordViewModel()
    
    override func setupTitle() {
        title = ScreenTitle.changePassword
    }
    
    override func setupView() {
        setupTextField()
    }
    
    override func refreshView() {
        parent?.title = title
    }
    
    @IBAction func updateButtonWasTouched(_ sender: Any) {
        guard
            let password = passwordTextField.text,
            let newPassword = newPasswordTextField.text,
            let confirmNewPassword = confirmNewPasswordTextField.text
        else {
            return
        }
        
        viewModel.password = password
        viewModel.newPassword = newPassword
        viewModel.confirmNewPassword = confirmNewPassword
        
        passwordErrorLabel.text = viewModel.passwordErrorTitle
        newPasswordErrorLabel.text = viewModel.newPasswordErrorTitle
        confirmNewPasswordLabel.text = viewModel.confirmNewPasswordErrorTitle
        
        guard viewModel.isFormValid else {
            return
        }
        
        showHUD()
        viewModel.updateNewPassword() { [weak self] result in
            guard let self = self else { return }
            self.dimissHUD()
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            self.showMessage(title: self.viewModel.successChangePasswordTitle)
            
            guard let mainViewController = self.parent as? MainViewController else {
                return
            }
            mainViewController.logout()
        }
    }
}

extension ChangePasswordViewController {
    private func setupTextField() {
        passwordTextField.becomeFirstResponder()
        
        passwordTextField.delegate = self
        newPasswordTextField.delegate = self
        confirmNewPasswordTextField.delegate = self
    }
}

extension ChangePasswordViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        if passwordTextField.isFirstResponder {
            newPasswordTextField.becomeFirstResponder()
        } else if newPasswordTextField.isFirstResponder {
            confirmNewPasswordTextField.becomeFirstResponder()
        } else {
            confirmNewPasswordTextField.resignFirstResponder()
        }
        return true
    }
}
