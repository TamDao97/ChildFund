//
//  ListContentViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/31/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class CallDialogViewController: BaseViewController {
    
    weak var containerViewController: HomeViewController?
    
    @IBAction func backAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBAction func call111Action(_ sender: Any) {
        call(number: Constants.childSOSNumber)
    }
    
    @IBAction func call113Action(_ sender: Any) {
        call(number: Constants.policeNumber)
    }
    
    @IBAction func call115Action(_ sender: Any) {
        call(number: Constants.ambulanceNumber)
    }
    
    private func call(number: String) {
        let url = URL(string: "tel://\(number)")
        url?.open()
    }
}
