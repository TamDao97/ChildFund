//
//  ChangePasswordViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ChangePasswordViewModel: BaseViewModel {
    var password: String = ""
    var newPassword: String = ""
    var confirmNewPassword: String = ""
    
    let successChangePasswordTitle = Strings.successChangePasswordTitle
    
    var passwordErrorTitle: String {
        if password.isEmpty {
            return Strings.passwordEmptyError
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
        return passwordErrorTitle.isEmpty
            && newPasswordErrorTitle.isEmpty
            && confirmNewPasswordErrorTitle.isEmpty
    }
    
    func updateNewPassword(completion: @escaping (ResultStatus) -> Void) {
        let model = ChangePasswordModel(id: Setting.userId.value,
                                        oldPassword: password,
                                        newPassword: newPassword,
                                        confirmNewPassword: confirmNewPassword)
        
        ChangePasswordService.updateNewPassword(model: model) { [weak self] result in
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
