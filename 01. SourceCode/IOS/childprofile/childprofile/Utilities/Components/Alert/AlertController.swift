//
//  AlertController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/22/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class AlertController: NSObject {
    static let shared = AlertController()
    private override init() {}
    var alertController: UIAlertController?
    
    func showErrorMessage(message: String, completionHandler: @escaping () -> Void) {
        if self.alertController != nil {
            self.alertController?.dismiss(animated: false, completion: nil)
            self.alertController = nil
        }
        self.alertController = UIAlertController(title: nil, message: message, preferredStyle: .alert)
        let okAction = UIAlertAction(title: Strings.ok, style: .cancel, handler: { (action) in
            self.alertController = nil
            completionHandler()
        })
        self.alertController?.addAction(okAction)
        UIApplication.topViewController()?.present(alertController!, animated: true, completion: nil)
    }
    
    func showConfirmMessage(message: String, confirm: String, cancel: String, isDelete: Bool = false, completionHandler: @escaping (_ confirm: Bool) -> Void) {
        if self.alertController != nil {
            self.alertController?.dismiss(animated: false, completion: nil)
            self.alertController = nil
        }
        self.alertController = UIAlertController(title: nil, message: message, preferredStyle: .alert)
        let confirmStyle: UIAlertAction.Style = (isDelete ? .destructive : .default)
        let confirmAction = UIAlertAction(title: confirm, style: confirmStyle, handler: { (action) in
            self.alertController = nil
            completionHandler(true)
        })
        let cancelAction = UIAlertAction(title: cancel, style: .cancel, handler: { (action) in
            self.alertController = nil
            completionHandler(false)
        })
        self.alertController?.addAction(cancelAction)
        self.alertController?.addAction(confirmAction)
        UIApplication.topViewController()?.present(alertController!, animated: true, completion: nil)
    }
}
