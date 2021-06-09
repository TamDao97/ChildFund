//
//  StyleTextField.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/9/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class StyleTextField: UITextField {
    var size: CGFloat {
        return 17
    }
    
    var weight: UIFont.Weight {
        return .regular
    }
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        configTextField()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        configTextField()
    }
    
    func configTextField() {
        font = UIFont.systemFont(ofSize: size, weight: weight)
    }
}

class ContentTextField: StyleTextField {
    override var size: CGFloat {
        return 17
    }
    
    override var weight: UIFont.Weight {
        return .regular
    }
}
