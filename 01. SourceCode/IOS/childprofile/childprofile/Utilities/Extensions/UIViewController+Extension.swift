//
//  UIViewController+Extension.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import Foundation
import UIKit

extension UIViewController {
    static func instance(identifier: String = "", with storyboardName: String = "") -> Self {
        var identifier = identifier
        if identifier == "" {
            identifier = self.className
        }
        
        var storyboardName = storyboardName
        if storyboardName == "" {
            storyboardName = self.className
        }
        return instantiateFromStoryboard(self, identifier: identifier, storyboardName: storyboardName)
    }
    
    private static func instantiateFromStoryboard<T: UIViewController>(_: T.Type, identifier: String, storyboardName: String) -> T {
        let storyboard = UIStoryboard(name: storyboardName, bundle: Bundle.main)
        guard let scene = storyboard.instantiateViewController(withIdentifier: identifier) as? T else {
            fatalError("ViewController with identifier \(storyboardName), not found in \(storyboardName) Storyboard.")
        }
        return scene
    }
    
    func getImagePickerController(type: UIImagePickerController.SourceType) -> UIImagePickerController? {
        guard UIImagePickerController.isSourceTypeAvailable(type) else {
            return nil
        }
        let imagePicker = UIImagePickerController()
        imagePicker.sourceType = type;
        imagePicker.allowsEditing = type == .camera ? true : false
        imagePicker.modalPresentationStyle = .popover
        return imagePicker
    }
}

extension UIViewController {
    func add(childViewController: UIViewController, containerView: UIView) {
        addChild(childViewController)
        
        containerView.addSubview(childViewController.view)
        childViewController.view.frame = containerView.bounds
        childViewController.view.autoresizingMask = [.flexibleWidth, .flexibleHeight]
    
        childViewController.didMove(toParent: self)
    }
    
    func remove(childViewController: UIViewController) {
        childViewController.willMove(toParent: nil)
        childViewController.view.removeFromSuperview()
        childViewController.removeFromParent()
    }
}

extension UIViewController {
    func hideKeyboardWhenTappedAround() {
        let tap: UITapGestureRecognizer = UITapGestureRecognizer(target: self, action: #selector(UIViewController.closeKeyboard))
        tap.cancelsTouchesInView = false
        view.addGestureRecognizer(tap)
    }
    
    @objc func closeKeyboard() {
        view.endEditing(true)
    }
}
