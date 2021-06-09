//
//  ForwardPasswordModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ForwardPasswordModel: Encodable {
    var id: String
    var username: String
    var email: String
    var confirmKey: String
    var newPassword: String
    var confirmNewPassword: String
    
    init(id: String = "", username: String, email: String, confirmKey: String = "", newPassword: String = "", confirmNewPassword: String = "") {
        self.id = id
        self.username = username
        self.email = email
        self.confirmKey = confirmKey
        self.newPassword = newPassword
        self.confirmNewPassword = confirmNewPassword
    }
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case username = "UserName"
        case email = "Email"
        case confirmKey = "ConfirmKey"
        case newPassword = "PasswordNew"
        case confirmNewPassword = "ConfirmPasswordNew"
    }
}
