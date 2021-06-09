//
//  LoginViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class LoginViewController: BaseViewController {
    @IBOutlet weak var usernameTextField: UITextField!
    @IBOutlet weak var passwordTextField: UITextField!
    @IBOutlet weak var usernameErrorLabel: UILabel!
    @IBOutlet weak var passwordErrorLabel: UILabel!
    
    @IBOutlet weak var loadingIndicatorView: UIActivityIndicatorView!
    
    @IBOutlet weak var loginButton: UIButton!
    
    var viewModel = LoginViewModel()
    
    var isLoading: Bool = false {
        didSet {
            if isLoading {
                loginButton.isEnabled = false
                loginButton.setTitle(Strings.empty, for: .normal)
                loadingIndicatorView.isHidden = false
            } else {
                loginButton.isEnabled = true
                loginButton.setTitle(Strings.loginButton, for: .normal)
                loadingIndicatorView.isHidden = true
            }
        }
    }
    
    override func setupView() {
        setupTextField()
    }

    override func setupTitle() {
        title = ScreenTitle.login
        usernameErrorLabel.text = ""
        passwordErrorLabel.text = ""
    }
    
    @IBAction func loginButtonWasTouched(_ sender: Any) {
        guard
            let username = usernameTextField.text,
            let password = passwordTextField.text
            else {
                return
        }
        
        viewModel.username = username
        viewModel.password = password
        
        usernameErrorLabel.text = viewModel.usernameErrorTitle
        passwordErrorLabel.text = viewModel.passwordErrorTitle
        
        guard viewModel.isFormValid else {
            return
        }
        
        isLoading = true
        viewModel.login() { [weak self] result in
            guard let self = self else { return }
            self.isLoading = false
            
            guard result == .success else {
                self.showMessage(title: self.viewModel.errorResponseContent)
                return
            }
            
            AppData.shared.getSessionData()
            
            let mainViewController = MainViewController.instance()
            let mainNavigationController = UINavigationController(rootViewController: mainViewController)
            self.present(mainNavigationController, animated: true, completion: nil)
        }
    }
    
    
    @IBAction func forgotPasswordButtonWasTouched(_ sender: Any) {
        let forwardViewController = ForwardPasswordViewController.instance()
        forwardViewController.loginViewController = self
        present(forwardViewController, animated: true, completion: nil)
    }
}

extension LoginViewController {
    private func setupTextField() {
        usernameTextField.delegate = self
        passwordTextField.delegate = self
    }
}

extension LoginViewController: UITextFieldDelegate {
    func textFieldShouldReturn(_ textField: UITextField) -> Bool {
        if usernameTextField.isFirstResponder {
            passwordTextField.becomeFirstResponder()
        } else {
            passwordTextField.resignFirstResponder()
        }
        return true
    }
}
