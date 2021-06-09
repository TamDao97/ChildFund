//
//  LocationHelper.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/30/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import CoreLocation

struct LocationHelper {
    static func isLocationPermissionGranted() -> Bool? {
        guard CLLocationManager.locationServicesEnabled() else {
            return false
        }
        
        switch CLLocationManager.authorizationStatus() {
        case .notDetermined:
            return nil
        case .authorizedAlways, .authorizedWhenInUse:
            return true
        default:
            return false
        }
    }
    
    static func goToLocationSetting() {
        if let bundleId = Bundle.main.bundleIdentifier,
            let url = URL(string: "\(UIApplication.openSettingsURLString)&path=LOCATION/\(bundleId)") {
            UIApplication.shared.open(url, options: [:], completionHandler: nil)
        }
    }
    
    static func geocode(latitude: Double, longitude: Double, completion: @escaping ((name: String, provinceName: String, districtName: String, wardName: String)?, Error?) -> ())  {
        CLGeocoder().reverseGeocodeLocation(CLLocation(latitude: latitude, longitude: longitude)) { placemarks, error in
            guard
                let placeMark = placemarks?.first,
                error == nil
            else {
                completion(nil, error)
                return
            }
            
            let address = placeMark.name ?? ""
            let provinceName = placeMark.administrativeArea ?? ""
            let districtName = placeMark.subAdministrativeArea ?? ""
            let wardName = placeMark.subLocality ?? ""
            
            completion((address, provinceName, districtName, wardName), nil)
        }
    }
}
