//
//  ComboboxService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ComboboxService {
    static func getSessionData(request: ChildProfileAPI, completion: @escaping (ApiResponse<String>) -> Void) {
        childProfileProvider.request(request) { result in
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
