//
//  ShareImageService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/26/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ShareImageService {
    static func uploadImage(images: [Data], model: UserUploadImageModel,completion: @escaping (ApiResponse<Any?>) -> Void) {
        childProfileProvider.request(.uploadImage(images: images, model: model)) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    completion(.success(nil))
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
