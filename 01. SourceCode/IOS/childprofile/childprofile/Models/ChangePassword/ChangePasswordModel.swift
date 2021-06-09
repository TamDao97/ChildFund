//
//  ChangePasswordModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ChangePasswordModel: Encodable {
    var id: String
    var oldPassword: String
    var newPassword: String
    var confirmNewPassword: String
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case oldPassword = "PasswordOld"
        case newPassword = "PasswordNew"
        case confirmNewPassword = "ConfirmPasswordNew"
    }
}
