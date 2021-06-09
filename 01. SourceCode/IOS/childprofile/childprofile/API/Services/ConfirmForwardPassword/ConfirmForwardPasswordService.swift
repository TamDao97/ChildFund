//
//  ConfirmForwardPasswordService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ConfirmForwardPasswordService {
    static func request(model: ForwardPasswordModel, completion: @escaping (ApiResponse<Any?>) -> Void) {
        childProfileProvider.request(.confirmForwardPassword(model: model)) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let content = try response.mapString()
                        completion(.failure(content))
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

