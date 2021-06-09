//
//  BaseViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 12/25/18.
//  Copyright © 2018 childfund. All rights reserved.
//

import UIKit

class BaseViewController: UIViewController {
    override func viewDidLoad() {
        super.viewDidLoad()
        setupView()
        setupTitle()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        refreshView()
    }
    
    deinit {
        log("\(self.className) DE-INIT")
    }
    
    func setupView() {}
    func setupTitle() {}
    func refreshView() {}
}
