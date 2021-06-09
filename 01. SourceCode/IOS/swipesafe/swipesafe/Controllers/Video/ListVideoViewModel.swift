//
//  ListVideoViewModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ListVideoViewModel {
    var errorResponseContent: String?
    var playListItemViewModels: [PlaylistItemViewModel] = []
    
    func getPlaylist(completion: @escaping (ResultStatus) -> Void) {
        let part = "snippet"
        let limit = 25
        let playlistId = "PL8bUH3S0Jon4T3pvg2vLGPy2yXwNWey3I"
        PlaylistService.getPlaylist(part: part, limit: limit, playlistId: playlistId) { [weak self] result in
            guard let self = self else { return }
            
            switch result {
            case .success(let playlistItems):
                self.playListItemViewModels = playlistItems.map({ item -> PlaylistItemViewModel in
                    let id = item.snippet.resourceID.videoID
                    let title = item.snippet.title
                    let date = DateHelper.convert(dateString: String(item.snippet.publishedAt.prefix(10)), fromFormat: DateFormatString.yyyyMMddLine, toFormat: DateFormatString.ddMMyyyy)
                    let thumnail = item.snippet.thumbnails?.thumbnailsDefault.url ?? ""
                    
                    let playListItem = PlaylistItemViewModel(id: id,
                                                             title: title,
                                                             date: date,
                                                             thumbnail: thumnail)
                    return playListItem
                })
                
                completion(.success)
            case .failure(let error):
                self.errorResponseContent = error
                completion(.failed)
            }
        }
    }
}
