//
//  ConfirmForwardPasswordViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ConfirmForwardPasswordViewModel: BaseViewModel {
    var confirmKey: String = ""
    var newPassword: String = ""
    var confirmNewPassword: String = ""
    
    let successChangePasswordTitle = Strings.successChangePasswordTitle
    
    var model: ForwardPasswordModel?
    
    var confirmKeyKeyErrorTitle: String {
        if confirmKey.isEmpty {
            return Strings.confirmKeyEmptyError
        }
        
        return ""
    }
    
    var newPasswordErrorTitle: String {
        if newPassword.isEmpty {
            return Strings.passwordEmptyError
        }
        
        return ""
    }
    
    var confirmNewPasswordErrorTitle: String {
        if confirmNewPassword.isEmpty {
            return Strings.passwordEmptyError
        }
        
        if newPassword != confirmNewPassword {
            return Strings.twoNewPasswordNotMatchError
        }
        
        return ""
    }
    
    var isFormValid: Bool {
        return confirmKeyKeyErrorTitle.isEmpty
            && newPasswordErrorTitle.isEmpty
            && confirmNewPasswordErrorTitle.isEmpty
    }
    
    func request(completion: @escaping (ResultStatus) -> Void) {
        model?.confirmKey = confirmKey
        model?.newPassword = newPassword
        model?.confirmNewPassword = confirmNewPassword
        
        guard let model = self.model else { return }
        
        ConfirmForwardPasswordService.request(model: model) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success:
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
}
