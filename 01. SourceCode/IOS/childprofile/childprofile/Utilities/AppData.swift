//
//  AppData.swift
//  childprofile
//
//  Created by Thanh Luu on 1/28/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

struct AppData {
    static var shared = AppData()
    private init() {}
    
    func getSessionData() {
        ComboboxService.getSessionData(request: .getRelationShip) { result in
            switch result {
            case .success(let value):
                Setting.relationShip.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getGeligion) { result in
            switch result {
            case .success(let value):
                Setting.religion.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getNation) { result in
            switch result {
            case .success(let value):
                Setting.nation.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getJob) { result in
            switch result {
            case .success(let value):
                Setting.job.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getProvinceByArea) { result in
            switch result {
            case .success(let value):
                Setting.province.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getDistrictByArea) { result in
            switch result {
            case .success(let value):
                Setting.district.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getWardByArea) { result in
            switch result {
            case .success(let value):
                Setting.ward.value = value
            case .failure(let error):
                log(error)
            }
        }
    }
}
