//
//  VideoItemTableViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 4/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

struct PlaylistItemViewModel {
    let id: String
    let title: String
    let date: String
    let thumbnail: String
}

class VideoItemTableViewCell: UITableViewCell {
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var dateLabel: UILabel!
    @IBOutlet weak var videoImageView: UIImageView!
    
    func configure(viewModel: PlaylistItemViewModel) {
        titleLabel.text = viewModel.title
        dateLabel.text = viewModel.date
        videoImageView.setImage(urlString: viewModel.thumbnail)
    }
}
