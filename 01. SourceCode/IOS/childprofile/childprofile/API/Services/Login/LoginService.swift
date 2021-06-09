//
//  LoginService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct LoginService {
    static func authenticate(model: LoginModel, completion: @escaping (ApiResponse<LoginProfileModel>) -> Void) {
        childProfileProvider.request(.login(model: model)) { result in 
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    let loginProfileModel = try response.map(LoginProfileModel.self)
                    completion(.success(loginProfileModel))
                } catch {
                    completion(.failure(error.localizedDescription))
                    log("Error: \(error.localizedDescription)")
                }
            case let .failure(error):
                completion(.failure(error.localizedDescription))
                log("Error: \(error.localizedDescription)")
            }
        }
    }
}
