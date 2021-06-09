//
//  Playlist.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct PlaylistService {
    static func getPlaylist(part: String, limit: Int, playlistId: String, completion: @escaping (ApiResponse<[PlaylistItem]>) -> Void) {
        youtubeProvider.request(.getPlaylistItems(part: part, limit: limit, playlistId: playlistId)) { result in
            switch result {
            case let .success(response):
                do {
                    guard response.statusCode == StatusCode.success.rawValue else {
                        let errorMessage = try response.mapString()
                        completion(.failure(errorMessage))
                        return
                    }
                    let playlistInfo = try response.map(PlaylistInfo.self)
                    completion(.success(playlistInfo.items))
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
