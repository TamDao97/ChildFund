//
//  BaseTableViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class BaseTableViewController: UITableViewController {
    override func viewDidLoad() {
        super.viewDidLoad()
        setupView()
        setupTitle()
        hideKeyboardWhenTappedAround()
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
