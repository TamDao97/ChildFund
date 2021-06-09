//
//  MainViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/4/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class MainViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    @IBAction func goToReportScreenAction(_ sender: Any) {
        containerViewController?.openChildViewController(menuItem: .report)
    }
}
