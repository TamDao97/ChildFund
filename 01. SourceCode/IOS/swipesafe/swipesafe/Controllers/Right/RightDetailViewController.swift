//
//  RightDetailViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class RightDetailViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    var content: RightCellViewModel?
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var webView: UIWebView!
    
    override func setupView() {
        loadFileUrl()
    }
    
    override func setupTitle() {
        titleLabel.text = content?.title
    }
    
    private func loadFileUrl() {
        guard let content = content else { return }
        let urlRequest = URLRequest(url: content.fileName.documentUrl)
        webView.loadRequest(urlRequest)
    }
}
