//
//  SplashViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/30/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class SplashViewController: BaseViewController {
    @IBOutlet weak var circleImageView: UIImageView!
    
    override func setupView() {
        circleImageView.rotate()
    }
    
    override func refreshView() {
        DispatchQueue.main.asyncAfter(deadline: .now() + 1) { [unowned self] in
            let homeViewController = HomeViewController.storyboardInstance()
            self.present(homeViewController, animated: false, completion: nil)
        }
    }
}
