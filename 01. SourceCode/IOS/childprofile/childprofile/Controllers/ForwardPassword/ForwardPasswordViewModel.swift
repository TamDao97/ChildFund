//
//  ForwardPasswordViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ForwardPasswordViewModel: BaseViewModel {
    var username: String = ""
    var email: String = ""
    
    var userId: String = ""
    
    var nextModel: ForwardPasswordModel {
        return ForwardPasswordModel(id: userId,
                                    username: username,
                                    email: email)
    }
    
    var usernameErrorTitle: String {
        if username.isEmpty {
            return Strings.usernameEmptyError
        }
        
        return ""
    }
    
    var emailErrorTitle: String {
        if email.isEmpty {
            return Strings.emailEmptyError
        }
        
        if !email.isValidEmail {
            return Strings.emailWrongFormat
        }
        
        return ""
    }
    
    var isFormValid: Bool {
        return usernameErrorTitle.isEmpty && emailErrorTitle.isEmpty
    }
    
    func request(completion: @escaping (ResultStatus) -> Void) {
        let model = ForwardPasswordModel(username: username, email: email)
        ForwardPasswordService.request(model: model) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success(let userId):
                self.userId = userId.replacingOccurrences(of: "\"", with: "")
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
}
