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
    // User Feature
    case login(model: LoginModel)
    case changePassword(model: ChangePasswordModel)
    case forwardPassword(model: ForwardPasswordModel)
    case confirmForwardPassword(model: ForwardPasswordModel)
    case getUserProfile
    case saveUserProfile(image: Data?, content: UserProfileModel)
    case uploadImage(images: [Data], model: UserUploadImageModel)
    
    // Data combobox
    case getRelationShip
    case getGeligion
    case getNation
    case getProvinceByArea
    case getDistrictByArea
    case getWardByArea
    case getJob
    
    // Child profile
    case getChildProfile(id: String)
    case addChildProfile(image: Data?, content: ChildProfileModel)
    case updateChildProfile(image: Data?, content: ChildProfileModel)
    case searchChildProfile(model: ChildProfileSearchCondition)
    case addReportProfile(model: ReportProfileModel)
}

extension ChildProfileAPI: TargetType {
    var baseURL: URL {
        guard let url = URL(string: AppConfigs.apiUrl) else {
            fatalError("FAILED: \(AppConfigs.apiUrl)")
        }
        return url
    }
    
    var path: String {
        switch self {
        case .login:
            return "api/Authorize/Login"
        case .changePassword:
            return "api/Authorize/ChangePasswordUser"
        case .forwardPassword:
            return "api/Authorize/ForwardPassword"
        case .confirmForwardPassword:
            return "api/Authorize/ConfirmForwardPassword"
        case .getUserProfile:
            return "api/Authorize/GetProfileUser"
        case .saveUserProfile:
            return "api/Authorize/UpdateProfileUser"
        case .getRelationShip:
            return "api/Combobox/GetRelationship"
        case .getGeligion:
            return "api/Combobox/GetGeligion"
        case .getNation:
            return "api/Combobox/GetNation"
        case .getProvinceByArea:
            return "api/Combobox/GetProvinceByArea"
        case .getDistrictByArea:
            return "api/Combobox/GetDistrictByArea"
        case .getWardByArea:
            return "api/Combobox/GetWardByArea"
        case .getJob:
            return "api/Combobox/GetJob"
        case .getChildProfile:
            return "api/ChildProfiles/GetInfoChildProfile"
        case .addChildProfile:
            return "api/ChildProfiles/AddChildProfile"
        case .updateChildProfile:
            return "api/ChildProfiles/UpdateChildProfile"
        case .searchChildProfile:
            return "api/ChildProfiles/SearchChilldProfile"
        case .addReportProfile:
            return "api/ChildProfiles/AddReportProfile"
        case .uploadImage:
            return "api/ImageLibrary/UploadImage"
        }
    }
    
    var method: Moya.Method {
        switch self {
        case .login,
             .changePassword,
             .forwardPassword,
             .confirmForwardPassword,
             .saveUserProfile,
             .addChildProfile,
             .updateChildProfile,
             .searchChildProfile,
             .addReportProfile,
             .uploadImage:
            return .post
        case .getUserProfile,
             .getRelationShip,
             .getGeligion,
             .getNation,
             .getProvinceByArea,
             .getDistrictByArea,
             .getWardByArea,
             .getJob,
             .getChildProfile:
            return .get
        }
    }
    
    var sampleData: Data {
        return Data()
    }
    
    var task: Task {
        switch self {
        case let .login(model):
            return .requestJSONEncodable(model)
        case let .changePassword(model):
            return .requestJSONEncodable(model)
        case let .forwardPassword(model):
            return .requestJSONEncodable(model)
        case let .confirmForwardPassword(model):
            return .requestJSONEncodable(model)
        case .getUserProfile:
            let parameters = ["id": Setting.userId.value]
            return .requestParameters(parameters: parameters, encoding: URLEncoding.queryString)
        case .getNation,
             .getGeligion,
             .getRelationShip,
             .getJob:
            return .requestPlain
        case .getProvinceByArea:
            let parameters = ["areaUserId": Setting.areaId.value]
            return .requestParameters(parameters: parameters, encoding: URLEncoding.queryString)
        case .getDistrictByArea,
             .getWardByArea:
            let parameters = [
                "areaDistrictId": Setting.areaDistrictId.value
            ]
            return .requestParameters(parameters: parameters, encoding: URLEncoding.queryString)
        case let .saveUserProfile(image, userProfileModel):
            let datas = createFormData(image: image, model: userProfileModel)
            return .uploadMultipart(datas)
        case let .addChildProfile(image, childProfileModel),
             let .updateChildProfile(image, childProfileModel):
            let datas = createFormData(image: image, model: childProfileModel)
            return .uploadMultipart(datas)
        case let .getChildProfile(id):
            let parameters = [
                "id": id
            ]
            return .requestParameters(parameters: parameters, encoding: URLEncoding.queryString)
        case let .searchChildProfile(model):
            return .requestJSONEncodable(model)
        case let .addReportProfile(model):
            return .requestJSONEncodable(model)
        case let .uploadImage(images, userUploadImageModel):
            let datas = createFormData(images: images, model: userUploadImageModel)
            return .uploadMultipart(datas)
        }
    }
    
    var headers: [String : String]? {
        return nil
    }
}

// Helpers
extension ChildProfileAPI {
    private func createFormData<T: Encodable>(image: Data?, model: T) -> [MultipartFormData] {
        var datas: [MultipartFormData] = []
        if let image = image {
            let imageData = MultipartFormData(provider: .data(image), name: "file", fileName: "file", mimeType: "image/jpg")
            datas.append(imageData)
        }
        
        var stringData = Data()
        if let jsonData = try? JSONEncoder().encode(model) {
            stringData = jsonData
        }
        
        let contentData = MultipartFormData(provider: .data(stringData), name: "Model")
        datas.append(contentData)
        return datas
    }
    
    private func createFormData<T: Encodable>(images: [Data], model: T) -> [MultipartFormData] {
        var datas: [MultipartFormData] = []
        for (index, image) in images.enumerated() {
            let orderNumber = index + 1
            let imageData = MultipartFormData(provider: .data(image), name: "file\(orderNumber)", fileName: "file\(orderNumber)", mimeType: "image/jpg")
            datas.append(imageData)
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
