//
//  ChildProfileAPI.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation
import DKImagePickerController
import Moya

let swipeSafeProvider = MoyaProvider<SwipeSafeAPI>()

enum SwipeSafeAPI {
    case getFormAbuse
    
    // Data combobox
    case getProvince
    case getDistrict
    case getWard
    case getRelationShip
    
    // Create report
    case addReport(images: [(type: DKAssetType, data: Data)], model: ReportModel)
}

extension SwipeSafeAPI: TargetType {
    var baseURL: URL {
        guard let url = URL(string: AppConfigs.apiUrl) else {
            fatalError("FAILED: \(AppConfigs.apiUrl)")
        }
        return url
    }
    
    var path: String {
        switch self {
        case .getFormAbuse:
            return "api/Combobox/GetFormsAbuse"
        case .getProvince:
            return "api/Combobox/GetAllProvince"
        case .getDistrict:
            return "api/Combobox/GetAllDistrict"
        case .getWard:
            return "api/Combobox/GetAllWard"
        case .getRelationShip:
            return "api/Combobox/GetRelationship"
        case .addReport:
            return "api/Report/AddReport"
        }
    }
    
    var method: Moya.Method {
        switch self {
        case .getFormAbuse,
             .getProvince,
             .getDistrict,
             .getWard,
             .getRelationShip:
            return .get
        case .addReport:
            return .post
        }
    }
    
    var sampleData: Data {
        return Data()
    }
    
    var task: Task {
        switch self {
        case .getFormAbuse,
             .getProvince,
             .getDistrict,
             .getWard,
             .getRelationShip:
            return .requestPlain
        case let .addReport(assetListData, reportModel):
            let datas = createFormData(assetListData: assetListData, model: reportModel)
            return .uploadMultipart(datas)
        }
    }
    
    var headers: [String : String]? {
        return nil
    }
}

// Helpers
extension SwipeSafeAPI {
    private func createFormData<T: Encodable>(assetListData: [(type: DKAssetType, data: Data)], model: T) -> [MultipartFormData] {
        var datas: [MultipartFormData] = []
        for (index, assetData) in assetListData.enumerated() {
            let orderNumber = index + 1
            let mimeType = assetData.type == .photo ? "image/jpg" : "video/mp4"
            let data = MultipartFormData(provider: .data(assetData.data), name: "file\(orderNumber)", fileName: "file\(orderNumber)", mimeType: mimeType)
            datas.append(data)
        }
        
        var stringData = Data()
        if let jsonData = try? JSONEncoder().encode(model) {
            stringData = jsonData
        }
        
        let contentData = MultipartFormData(provider: .data(stringData), name: "Model")
        datas.append(contentData)
        return datas
    }
}
