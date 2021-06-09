//
//  PlaylistItem.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct PlaylistInfo: Codable {
    let kind, etag: String
    let pageInfo: PageInfo
    let items: [PlaylistItem]
}

struct PlaylistItem: Codable {
    let kind: ItemKind
    let etag, id: String
    let snippet: Snippet
}

enum ItemKind: String, Codable {
    case youtubePlaylistItem = "youtube#playlistItem"
}

struct Snippet: Codable {
    let publishedAt: String
    let channelID: ChannelID
    let title, description: String
    let thumbnails: Thumbnails?
    let channelTitle: ChannelTitle
    let playlistID: PlaylistID
    let position: Int
    let resourceID: ResourceID
    
    enum CodingKeys: String, CodingKey {
        case publishedAt
        case channelID = "channelId"
        case title, description, thumbnails, channelTitle
        case playlistID = "playlistId"
        case position
        case resourceID = "resourceId"
    }
}

enum ChannelID: String, Codable {
    case ucLxaUpKb2QYs6UKqvaWvW = "UCLxaUpKb2qYs6_uKqvaWv-w"
}

enum ChannelTitle: String, Codable {
    case vtv7Kids = "VTV7 KIDS"
}

enum PlaylistID: String, Codable {
    case pl8BUH3S0Jon4T3Pvg2VLGPy2YXwNWey3I = "PL8bUH3S0Jon4T3pvg2vLGPy2yXwNWey3I"
}

struct ResourceID: Codable {
    let kind: ResourceIDKind
    let videoID: String
    
    enum CodingKeys: String, CodingKey {
        case kind
        case videoID = "videoId"
    }
}

enum ResourceIDKind: String, Codable {
    case youtubeVideo = "youtube#video"
}

struct Thumbnails: Codable {
    let thumbnailsDefault, medium, high, standard: Default
    let maxres: Default
    
    enum CodingKeys: String, CodingKey {
        case thumbnailsDefault = "default"
        case medium, high, standard, maxres
    }
}

struct Default: Codable {
    let url: String
    let width, height: Int
}

struct PageInfo: Codable {
    let totalResults, resultsPerPage: Int
}
