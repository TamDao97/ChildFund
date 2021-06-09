//
//  SearchResult.swift
//  childprofile
//
//  Created by Thanh Luu on 1/23/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct SearchResult: Decodable {
    var totalItem: Int
    var listResult: [ChildProfileSearchResult]
    var pathFile: String
    
    enum CodingKeys: String, CodingKey {
        case totalItem = "TotalItem"
        case listResult = "ListResult"
        case pathFile = "PathFile"
    }
    
    init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.totalItem = try container.decodeIfPresent(Int.self, forKey: .totalItem) ?? 0
        self.listResult = try container.decodeIfPresent([ChildProfileSearchResult].self, forKey: .listResult) ?? []
        self.pathFile = try container.decodeIfPresent(String.self, forKey: .pathFile) ?? ""
    }
}
