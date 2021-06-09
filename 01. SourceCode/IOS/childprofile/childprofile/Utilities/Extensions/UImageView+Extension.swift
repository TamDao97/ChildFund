//
//  UImageView+Extension.swift
//  childprofile
//
//  Created by Thanh Luu on 1/12/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import Kingfisher

extension UIImageView {
    func setImage(urlString: String) {
        let url = URL(string: urlString)
        kf.setImage(with: url,
                    options: [.transition(.fade(0.2))]
        )
    }
}
