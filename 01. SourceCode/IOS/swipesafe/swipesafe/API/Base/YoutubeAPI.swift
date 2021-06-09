//
//  YoutubeAPI.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import Moya

let youtubeProvider = MoyaProvider<YoutubeAPI>()

enum YoutubeAPI {
    case getPlaylistItems(part: String, limit: Int, playlistId: String)
}

extension YoutubeAPI: TargetType {
    var baseURL: URL {
        guard let url = URL(string: AppConfigs.youtubeUrl) else {
            fatalError("FAILED: \(AppConfigs.youtubeUrl)")
        }
        return url
    }
    
    var path: String {
        switch self {
        case .getPlaylistItems:
            return "playlistItems"
        }
    }
    
    var method: Moya.Method {
        switch self {
        case .getPlaylistItems:
            return .get
        }
    }
    
    var sampleData: Data {
        return Data()
    }
    
    var task: Task {
        switch self {
        case let .getPlaylistItems(part, limit, playlistId):
            let parameters: [String: Any] = [
                "part": part,
                "maxResults": limit,
                "playlistId": playlistId,
                "key": AppConfigs.youtubeToken
            ]
            return .requestParameters(parameters: parameters, encoding: URLEncoding.queryString)
        }
    }
    
    var headers: [String : String]? {
        return nil
    }
}
