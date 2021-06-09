//
//  UserUploadImageModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/27/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct UserUploadImageModel: Encodable {
    var content: String
    var userId: String
    
    enum CodingKeys: String, CodingKey {
        case content = "Content"
        case userId = "UserId"
    }
}
