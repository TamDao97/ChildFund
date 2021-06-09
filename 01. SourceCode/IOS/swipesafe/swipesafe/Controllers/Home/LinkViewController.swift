//
//  LinkViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/3/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class LinkViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func backAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    private func gotoWeb(link: String) {
        if let url = URL(string: link) {
            UIApplication.shared.open(url, options: [:])
        }
    }
}

class LinkTableViewController: BaseTableViewController {
    private let links = [
        "http://treem.gov.vn/",
        "http://www.childfund.org.vn/",
        "http://treemviet.vn/",
        "https://www.unicef.org/vietnam/vi",
        "https://plan-international.org/vietnam",
        "https://www.wvi.org/vi/vi%E1%BB%87t-nam",
        "https://vietnam.savethechildren.net/",
        "https://hagarinternational.org/vietnam/",
        "http://goodneighbors.vn/"
    ]
    
    override func tableView(_ tableView: UITableView, didSelectRowAt indexPath: IndexPath) {
        tableView.deselectRow(at: indexPath, animated: true)
        gotoWeb(link: links[indexPath.row])
    }
    
    private func gotoWeb(link: String) {
        if let url = URL(string: link) {
            UIApplication.shared.open(url, options: [:])
        }
    }
}
