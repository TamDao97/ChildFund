//
//  ChildProfileAPI.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import Moya

let childProfileProvider = MoyaProvider<ChildProfileAPI>()

enum ChildProfileAPI {
    
}

extension ChildProfileAPI: TargetType {
    var baseURL: URL {
        guard let url = URL(string: AppConfigs.apiUrl) else {
            fatalError("FAILED: \(AppConfigs.apiUrl)")
        }
        return url
    }
    
    var path: String {
        return ""
    }
    
    var method: Moya.Method {
        return .get
    }
    
    var sampleData: Data {
        return Data()
    }
    
    var task: Task {
        return .requestPlain
    }
    
    var headers: [String : String]? {
        return nil
    }
}
