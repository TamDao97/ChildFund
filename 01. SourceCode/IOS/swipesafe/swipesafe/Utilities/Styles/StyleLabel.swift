//
//  StyleLabel.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/9/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class StyleLabel: UILabel {
    var size: CGFloat {
        return 17
    }
    
    var weight: UIFont.Weight {
        return .regular
    }
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        configLabel()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        configLabel()
    }
    
    func configLabel() {
        font = UIFont.systemFont(ofSize: size, weight: weight)
    }
}

class BoldLabel: StyleLabel {
    override var size: CGFloat {
        return 20
    }
    
    override var weight: UIFont.Weight {
        return .medium
    }
}

class ContentLabel: StyleLabel {
    override var size: CGFloat {
        return 17
    }
    
    override var weight: UIFont.Weight {
        return .regular
    }
}

class InfoTitleLabel: StyleLabel {
    override var size: CGFloat {
        return 17
    }
    
    override var weight: UIFont.Weight {
        return .regular
    }
}

class ContentTitleLabel: StyleLabel {
    override var size: CGFloat {
        return 18
    }
    
    override var weight: UIFont.Weight {
        return .medium
    }
}


