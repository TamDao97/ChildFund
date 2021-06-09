//
//  ApiResponse.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

enum ApiResponse<Value> {
    case success(Value)
    case failure(String?)
}

enum StatusCode: Int {
    case success = 200
}
