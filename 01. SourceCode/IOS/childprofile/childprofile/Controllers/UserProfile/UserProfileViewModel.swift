//
//  UserProfileViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import UIKit

class UserProfileViewModel: BaseViewModel {
    var userProfileModel: UserProfileModel?
    var successMessage = Strings.userSuccessUpdate
    
    var id: String = ""
    var name: String = ""
    var dateOfBirth: String = ""
    var phoneNumber: String = ""
    var email: String = ""
    var imagePath: String = ""
    var gender: Int = Gender.male.rawValue
    var identifyNumber: String = ""
    var address: String = ""
    
    var userImageData: Data?
    
    var genderSegmentIndex: Int {
       return gender == 0 ? 1 : 0
    }
    
    var usernameErrorTitle: String {
        if name.isEmpty {
            return Strings.nameEmptyError
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
    
    func get(completion: @escaping (ResultStatus) -> Void) {
        userImageData = nil
        UserProfileService.get() { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success(let userProfileModel):
                self.userProfileModel = userProfileModel
                self.setValueFromModel()
                
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
    
    func update(completion: @escaping (ResultStatus) -> Void) {
        let userProfileModel = UserProfileModel(id: id,
                                                name: name,
                                                dateOfBirth: dateOfBirth,
                                                phoneNumber: phoneNumber,
                                                email: email,
                                                imagePath: imagePath,
                                                gender: gender,
                                                identifyNumber: identifyNumber,
                                                address: address)
        UserProfileService.update(image: userImageData, model: userProfileModel) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success(let userProfileModel):
                if let userProfileModel = userProfileModel {
                    Setting.imagePath.value = userProfileModel.imagePath ?? ""
                }
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
    
    private func setValueFromModel() {
        guard let model = userProfileModel else { return }
        id = model.id
        name = model.name ?? ""
        dateOfBirth = DateHelper.stringFromServerDateToLocalDate(model.dateOfBirth ?? "")
        phoneNumber = model.phoneNumber ?? ""
        email = model.email ?? ""
        imagePath = model.imagePath ?? ""
        gender = model.gender ?? Gender.male.rawValue
        identifyNumber = model.identifyNumber ?? ""
        address = model.address ?? ""
    }
    
    func setImageData(image: UIImage) {
        userImageData = image.jpeg(.medium)
    }
}
