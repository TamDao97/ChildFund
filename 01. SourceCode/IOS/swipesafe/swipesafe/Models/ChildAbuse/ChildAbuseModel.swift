//
//  ChildAbuseModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/16/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class ChildAbuseModel: Codable {
    var id: String
    var name: String
    var abuseId: String
    var abuseName: String
    var isCheck: Bool = false
    
    enum CodingKeys: String, CodingKey {
        case id = "Id"
        case name = "Name"
        case abuseId = "AbuseId"
        case abuseName = "AbuseName"
    }
    
    required init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.id = try container.decodeIfPresent(String.self, forKey: .id) ?? ""
        self.name = try container.decodeIfPresent(String.self, forKey: .name) ?? ""
        self.abuseId = self.id
        self.abuseName = self.name
    }
    
    static func getSessionData(from string: String) -> [ChildAbuseModel] {
        do {
            guard let data = string.data(using: .utf8) else {
                return []
            }
            
            let values = try JSONDecoder().decode([ChildAbuseModel].self, from: data)
            return values
        } catch {
            print(error)
        }
        return []
    }
}

extension ChildAbuseModel: Equatable {
    static func == (lhs: ChildAbuseModel, rhs: ChildAbuseModel) -> Bool {
        return lhs.id == rhs.id
    }
}
