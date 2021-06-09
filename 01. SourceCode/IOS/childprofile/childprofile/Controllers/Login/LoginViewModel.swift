//
//  LoginViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class LoginViewModel: BaseViewModel {
    var username: String = ""
    var password: String = ""
    
    var usernameErrorTitle: String {
        if username.isEmpty {
            return Strings.usernameEmptyError
        }
        
        return ""
    }
    
    var passwordErrorTitle: String {
        if password.isEmpty {
            return Strings.passwordEmptyError
        }
        
        if password.count <= Constants.maxPasswordLength {
            return Strings.passwordLengthError
        }
    
        return ""
    }
    
    var isFormValid: Bool {
        return usernameErrorTitle.isEmpty && passwordErrorTitle.isEmpty
    }
    
    func login(completion: @escaping (ResultStatus) -> Void) {
        let model = LoginModel(username: username, password: password)
        LoginService.authenticate(model: model) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success(let loginProfileModel):
                
                Setting.isLogin.value = true
                Setting.userId.value = loginProfileModel.id
                Setting.areaId.value = loginProfileModel.areaUserid ?? ""
                Setting.areaDistrictId.value = loginProfileModel.areaDistrictId ?? ""
                Setting.userLevel.value = loginProfileModel.userLevel ?? ""
                Setting.userFullName.value = loginProfileModel.name ?? ""
                Setting.imagePath.value = loginProfileModel.imagePath ?? ""
                
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
}
