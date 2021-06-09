//
//  ReportService.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import DKImagePickerController

struct ReportService {
    static func create(assetListData: [(type: DKAssetType, data: Data)], model: ReportModel,completion: @escaping (ApiResponse<Any?>) -> Void) {
        swipeSafeProvider.request(.addReport(images: assetListData, model: model)) { result in
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
