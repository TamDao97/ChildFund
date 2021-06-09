//
//  LoginModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct LoginModel: Encodable {
    var username: String
    var password: String
    
    enum CodingKeys: String, CodingKey {
        case username = "UserName"
        case password = "Password"
    }
}
