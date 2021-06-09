//
//  ForwardPasswordService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ForwardPasswordService {
    static func request(model: ForwardPasswordModel, completion: @escaping (ApiResponse<String>) -> Void) {
        childProfileProvider.request(.forwardPassword(model: model)) { result in
            switch result {
            case let .success(response):
                do {
                    let content = try response.mapString()
                    guard response.statusCode == StatusCode.success.rawValue else {
                        completion(.failure(content))
                        return
                    }
                    completion(.success(content))
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

