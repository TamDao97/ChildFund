//
//  MenuViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/4/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

enum MenuItem {
    case report
    case library
    case phone
    case link
    case appInfo
}

class MenuViewController: BaseViewController {
    @IBOutlet weak var menuView: UIView!
    @IBOutlet weak var leadingMenuViewConstraint: NSLayoutConstraint!
    
    weak var callerViewController: HomeViewController?

    override func viewDidAppear(_ animated: Bool) {
        super.viewDidAppear(animated)
        
        setMenuState(isOpen: true)
    }
    
    @IBAction func openReportAction(_ sender: Any) {
        DispatchQueue.main.async { [weak self] in
            self?.callerViewController?.openChildViewController(menuItem: .report)
        }
        setMenuState(isOpen: false)
    }
    
    @IBAction func openLibraryAction(_ sender: Any) {
        callerViewController?.openChildViewController(menuItem: .library)
        setMenuState(isOpen: false)
    }
    
    @IBAction func openPhoneAction(_ sender: Any) {
        callerViewController?.openChildViewController(menuItem: .phone)
        setMenuState(isOpen: false)
    }
    
    @IBAction func openLinkAction(_ sender: Any) {
        callerViewController?.openChildViewController(menuItem: .link)
        setMenuState(isOpen: false)
    }
    
    @IBAction func openAppInfoAction(_ sender: Any) {
        callerViewController?.openChildViewController(menuItem: .appInfo)
        setMenuState(isOpen: false)
    }
    
    @IBAction func dimissAction(_ sender: Any) {
        setMenuState(isOpen: false)
    }
    
    private func setMenuState(isOpen: Bool) {
        leadingMenuViewConstraint.constant = isOpen ? -12 : -menuView.frame.width
        
        UIView.animate(withDuration: 0.5, animations: { [weak self] in
            guard let self = self else { return }
            self.view.layoutIfNeeded()
        }, completion: { [weak self] _ in
            guard !isOpen else { return }
            self?.dismiss(animated: true, completion: nil)
        })
    }
}
