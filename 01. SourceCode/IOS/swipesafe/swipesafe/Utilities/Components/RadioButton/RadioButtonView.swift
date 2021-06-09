//
//  RadioButtonView.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/9/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

@IBDesignable
class RadioButtonView: NibView {
    @IBOutlet weak var imageView: UIImageView!
    @IBOutlet weak var titleLabel: UILabel!
    
    static let selectedImageName = "selected_radio"
    static let unSelectedImageName = "unselected_radio"
    static let fadeDuration: CFTimeInterval = 0.15
    
    var selectedCompletion: (() -> Void)?
    
    var result: String? {
        return isSelected ? value : nil
    }
    
    @IBInspectable var title: String = "" {
        didSet {
            titleLabel.text = title
        }
    }
    
    @IBInspectable var titleColor: UIColor = .black {
        didSet {
            titleLabel.textColor = titleColor
        }
    }
    
    @IBInspectable var value: String = ""
    
    @IBInspectable var isSelected: Bool = false {
        didSet {
            imageView.image = isSelected
                ? RadioButtonView.selectedImageName.image
                : RadioButtonView.unSelectedImageName.image
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
