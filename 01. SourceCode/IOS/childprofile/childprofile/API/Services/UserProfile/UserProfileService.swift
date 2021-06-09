//
//  UserProfileService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct UserProfileService {
    static func get(completion: @escaping (ApiResponse<UserProfileModel>) -> Void) {
        childProfileProvider.request(.getUserProfile) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    let userProfileModel = try response.map(UserProfileModel.self)
                    completion(.success(userProfileModel))
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
    
    static func update(image: Data?, model: UserProfileModel, completion: @escaping (ApiResponse<UserProfileModel?>) -> Void) {
        childProfileProvider.request(.saveUserProfile(image: image, content: model)) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    let userProfileModel = try? response.map(UserProfileModel.self)
                    completion(.success(userProfileModel))
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
