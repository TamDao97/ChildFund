//
//  VideoItemViewController.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit
import YoutubePlayer_in_WKWebView

class VideoItemViewController: BaseViewController {
    weak var containerViewController: HomeViewController?
    
    @IBAction func closeAction(_ sender: Any) {
        containerViewController?.popViewController()
    }
    
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var playerView: WKYTPlayerView!
    var videoTitle = ""
    var videoId: String?
    
    override func setupView() {
        guard let videoId = videoId else { return }
        playerView.load(withVideoId: videoId)
    }
    
    override func setupTitle() {
        titleLabel.text = videoTitle
    }
}
