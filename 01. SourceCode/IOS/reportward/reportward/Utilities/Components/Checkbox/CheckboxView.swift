//
//  CheckboxView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/10/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

@IBDesignable
class CheckboxView: NibView {
    @IBOutlet weak var imageView: UIImageView!
    @IBOutlet weak var titleLabel: UILabel!
    
    static let selectedImageName = "selected_checkbox"
    static let unSelectedImageName = "unselected_checkbox"
    static let fadeDuration: CFTimeInterval = 0.15
    
    var selectedCompletion: (() -> Void)?
    var value: String = ""
    
    var result: String? {
        return isSelected ? value : nil
    }
    
    @IBInspectable var title: String = "" {
        didSet {
            titleLabel.text = title
        }
    }
    
    @IBInspectable var isSelected: Bool = false {
        didSet {
            imageView.image = isSelected
                ? CheckboxView.selectedImageName.image
                : CheckboxView.unSelectedImageName.image
            let transition: CATransition = CATransition.init()
            transition.duration = CheckboxView.fadeDuration
            transition.timingFunction = CAMediaTimingFunction.init(name: .easeInEaseOut)
            transition.type = .fade
            imageView.layer.add(transition, forKey: nil)
        }
    }
    
    @IBAction func toggleButtonWasTouched(_ sender: Any) {
        isSelected = !isSelected
        selectedCompletion?()
    }
}
