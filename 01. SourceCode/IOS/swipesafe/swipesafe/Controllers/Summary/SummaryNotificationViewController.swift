//
//  SummaryNotificationViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/9/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class SummaryNotificationViewController: BaseViewController {
    weak var homeViewController: HomeViewController?
    
    @IBAction func goToHomeAction(_ sender: Any) {
        dismiss(animated: true, completion: nil)
        homeViewController?.resetToDefaultViewController()
    }
    
    @IBAction func createReportAgainAction(_ sender: Any) {
        dismiss(animated: true, completion: nil)
        homeViewController?.openChildViewController(menuItem: .report)
    }
}
