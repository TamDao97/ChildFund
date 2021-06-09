//
//  AppData+SessionData.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/17/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

extension AppData {
    func getSessionData() {
        ComboboxService.getSessionData(request: .getFormAbuse) { result in
            switch result {
            case .success(let value):
                Setting.formAbuse.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getDistrict) { result in
            switch result {
            case .success(let value):
                Setting.district.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getProvince) { result in
            switch result {
            case .success(let value):
                Setting.province.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getWard) { result in
            switch result {
            case .success(let value):
                Setting.ward.value = value
            case .failure(let error):
                log(error)
            }
        }
        
        ComboboxService.getSessionData(request: .getRelationShip) { result in
            switch result {
            case .success(let value):
                Setting.relationShip.value = value
            case .failure(let error):
                log(error)
            }
        }
    }
}
