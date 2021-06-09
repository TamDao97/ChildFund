//
//  AppDelegate.swift
//  childprofile
//
//  Created by Thanh Luu on 1/2/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import IQKeyboardManagerSwift

@UIApplicationMain
class AppDelegate: UIResponder, UIApplicationDelegate {
    var window: UIWindow?

    func application(_ application: UIApplication, didFinishLaunchingWithOptions launchOptions: [UIApplication.LaunchOptionsKey: Any]?) -> Bool {
        setupCommon()
        setNavigationBarApperance()
        setRootViewController()
        return true
    }

    func applicationWillResignActive(_ application: UIApplication) {}
    func applicationDidEnterBackground(_ application: UIApplication) {}
    func applicationWillEnterForeground(_ application: UIApplication) {}
    func applicationDidBecomeActive(_ application: UIApplication) {}
    func applicationWillTerminate(_ application: UIApplication) {}
}

extension AppDelegate {
    private func setRootViewController() {
        let rootViewController = Setting.isLogin.value
            ? MainViewController.instance()
            : UserProfileViewController.instance()
        let navigationController = UINavigationController(rootViewController: rootViewController)
        window = UIWindow(frame: UIScreen.main.bounds)
        window?.rootViewController = navigationController
        window?.makeKeyAndVisible()
    }
    
    private func setNavigationBarApperance() {
        UINavigationBar.appearance().with {
            $0.isTranslucent = false
            $0.barStyle = .black
            $0.barTintColor = Colors.primary
            $0.tintColor = .white
            $0.titleTextAttributes = [.foregroundColor: UIColor.white]
        }
    }
    
    private func setupCommon() {
        IQKeyboardManager.shared.enable = true
        IQKeyboardManager.shared.enableAutoToolbar = false
        
        let disableIQKeyboardViewController = [Step1TableViewController.self,
                                               Step2TableViewController.self,
                                               Step3TableViewController.self,
                                               Step4TableViewController.self]
        IQKeyboardManager.shared.disabledDistanceHandlingClasses.append(contentsOf: disableIQKeyboardViewController)
    }
}
