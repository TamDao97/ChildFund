//
//  ChildProfileService.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct ChildProfileService {
    static func insert(image: Data?, model: ChildProfileModel, completion: @escaping (ApiResponse<Any?>) -> Void) {
        childProfileProvider.request(.addChildProfile(image: image, content: model)) { result in
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
    
    static func update(image: Data?, model: ChildProfileModel, completion: @escaping (ApiResponse<Any?>) -> Void) {
        childProfileProvider.request(.updateChildProfile(image: image, content: model)) { result in
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
    
    static func get(id: String, completion: @escaping (ApiResponse<ChildProfileModel>) -> Void) {
        childProfileProvider.request(.getChildProfile(id: id)) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    let childProfileModel = try response.map(ChildProfileModel.self)
                    completion(.success(childProfileModel))
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
    
    static func search(searchModel: ChildProfileSearchCondition, completion: @escaping (ApiResponse<SearchResult>) -> Void) {
        childProfileProvider.request(.searchChildProfile(model: searchModel)) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    let result = try response.map(SearchResult.self)
                    completion(.success(result))
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
    
    static func addReport(reportProfileModel: ReportProfileModel, completion: @escaping (ApiResponse<Any?>) -> Void) {
        childProfileProvider.request(.addReportProfile(model: reportProfileModel)) { result in
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
