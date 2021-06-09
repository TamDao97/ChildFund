//
//  CheckboxTableViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/16/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class CheckboxTableViewController: UITableViewController {
    var childAbuseModels: [ChildAbuseModel] = []
    
    var contentHeight: CGFloat {
        tableView.layoutIfNeeded()
        return tableView.contentSize.height
    }
    
    convenience init(childAbuseModels: [ChildAbuseModel]) {
        self.init()
        self.childAbuseModels = childAbuseModels
    }

    override func viewDidLoad() {
        super.viewDidLoad()
        
        tableView.separatorColor = .clear
        tableView.register(CheckboxTableViewCell.self, forCellReuseIdentifier: CheckboxTableViewCell.className)
    }
    
    func reload(with data: [ChildAbuseModel]) {
        childAbuseModels = data
        tableView.reloadData()
    }

    // MARK: - Table view data source

    override func tableView(_ tableView: UITableView, numberOfRowsInSection section: Int) -> Int {
        return childAbuseModels.count
    }
    
    override func tableView(_ tableView: UITableView, cellForRowAt indexPath: IndexPath) -> UITableViewCell {
        let cell = tableView.dequeueReusableCell(withIdentifier: CheckboxTableViewCell.className, for: indexPath) as! CheckboxTableViewCell
        cell.childAbuseModel = childAbuseModels[indexPath.row]
        return cell
    }
    
    // MARK: - Table view delegate
    
    override func tableView(_ tableView: UITableView, heightForRowAt indexPath: IndexPath) -> CGFloat {
        return CheckboxTableViewCell.defaultCellHeight
    }
}
