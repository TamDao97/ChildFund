//
//  WarGroupModel.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct WarGroupModel: Decodable {
    var districtId: String
    var listWard: [ComboboxItemModel]
    
    enum CodingKeys: String, CodingKey {
        case districtId = "DistrictId"
        case listWard = "ListWard"
    }
    
    init(from decoder: Decoder) throws {
        let container = try decoder.container(keyedBy: CodingKeys.self)
        self.districtId = try container.decodeIfPresent(String.self, forKey: .districtId) ?? ""
        self.listWard = try container.decodeIfPresent([ComboboxItemModel].self, forKey: .listWard) ?? []
    }
    
    static func getSessionData(from string: String) -> [WarGroupModel] {
        do {
            guard let data = string.data(using: .utf8) else {
                return []
            }
            
            let values = try JSONDecoder().decode([WarGroupModel].self, from: data)
            return values
        } catch {
            print(error)
        }
        return []
    }
}
