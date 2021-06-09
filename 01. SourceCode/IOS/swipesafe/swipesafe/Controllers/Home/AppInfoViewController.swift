//
//  AppInfoViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/11/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class AppInfoViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func backAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
}
