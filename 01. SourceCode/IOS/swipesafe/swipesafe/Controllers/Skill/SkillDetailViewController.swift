//
//  SkillDetailViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import WebKit

class SkillDetailViewController: BaseViewController {
    var content: SkillCellViewModel?
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var webView: UIWebView!
    
    override func setupView() {
        loadFileUrl()
    }
    
    override func setupTitle() {
        titleLabel.text = content?.title
    }
    
    private func loadFileUrl() {
        guard
            let content = content,
            let url = Bundle.main.url(forResource: content.htmlFileName, withExtension: "html")
        else { return }
        let urlRequest = URLRequest(url: url)
        webView.loadRequest(urlRequest)
    }
}
