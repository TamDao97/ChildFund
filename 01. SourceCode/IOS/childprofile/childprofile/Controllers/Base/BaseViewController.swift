//
//  BaseViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/6/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class BaseViewController: UIViewController {
    override func viewDidLoad() {
        super.viewDidLoad()
        resetParentNavigationBar()
        setupView()
        setupTitle()
        setupNavigationBar()
        hideKeyboardWhenTappedAround()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        refreshView()
    }
    
    deinit {
        log("\(self.className) DE-INIT")
    }
    
    func resetParentNavigationBar() {
        parent?.navigationItem.rightBarButtonItem = nil
    }
    
    func setupView() {}
    func setupNavigationBar() {}
    func setupTitle() {}
    func refreshView() {}
}
