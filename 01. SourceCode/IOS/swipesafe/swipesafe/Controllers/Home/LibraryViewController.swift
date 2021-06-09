//
//  LibraryViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/3/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class LibraryViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBAction func goToChildrenThingsAction(_ sender: Any) {
        let childrenThingsViewController = ChildrenThingsViewController.storyboardInstance(identifier: ChildrenThingsViewController.className,
                                                                                           with: HomeViewController.className)
        childrenThingsViewController.containerViewController = containerViewController
        containerViewController?.pushViewController(childrenThingsViewController)
    }
    
    @IBAction func goToRightsAction(_ sender: Any) {
        let rightViewController = RightViewController.storyboardInstance(identifier: RightViewController.className,
                                                                         with: HomeViewController.className)
        rightViewController.containerViewController = containerViewController
        containerViewController?.pushViewController(rightViewController)
    }
}

class ChildrenThingsViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBAction func goToSkillAction(_ sender: Any) {
        let skillViewController = SkillViewController.storyboardInstance(identifier: SkillViewController.className,
                                                                         with: HomeViewController.className)
        skillViewController.containerViewController = containerViewController
        containerViewController?.pushViewController(skillViewController)
    }
    
    @IBAction func goToEducationAction(_ sender: Any) {
        let listVideoViewController = ListVideoViewController.storyboardInstance(identifier: ListVideoViewController.className,
                                                                         with: HomeViewController.className)
        listVideoViewController.containerViewController = containerViewController
        containerViewController?.pushViewController(listVideoViewController)
    }
}
