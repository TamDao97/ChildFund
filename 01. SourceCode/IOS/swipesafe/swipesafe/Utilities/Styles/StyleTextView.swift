//
//  StyleTextView.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/9/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class StyleTextView: UITextView {
    var size: CGFloat {
        return 17
    }
    
    var weight: UIFont.Weight {
        return .regular
    }
    
    override init(frame: CGRect, textContainer: NSTextContainer?) {
        super.init(frame: frame, textContainer: textContainer)
        configTextView()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        configTextView()
    }
    
    func configTextView() {
        font = UIFont.systemFont(ofSize: size, weight: weight)
    }
}

class ContentTextView: StyleTextView {
    override var size: CGFloat {
        return 20
    }
    
    override var weight: UIFont.Weight {
        return .medium
    }
}
