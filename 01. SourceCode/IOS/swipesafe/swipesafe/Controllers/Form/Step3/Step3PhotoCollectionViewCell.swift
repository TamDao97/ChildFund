//
//  Step3PhotoCollectionViewCell.swift
//  swipesafe
//
//  Created by Thanh Luu on 3/24/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

protocol Step3PhotoCollectionViewCellDelegate: class {
    func removePhoto(at index: Int)
}

class Step3PhotoCollectionViewCell: UICollectionViewCell {
    @IBOutlet weak var photoImageView: UIImageView!
    @IBOutlet weak var removePhotoButton: UIButton!
    @IBOutlet weak var videoImageView: UIImageView!
    
    weak var delegate: Step3PhotoCollectionViewCellDelegate?
    
    func config(image: UIImage, index: Int, isVideo: Bool = false) {
        photoImageView.image = image
        removePhotoButton.tag = index
        videoImageView.isHidden = !isVideo
    }

    @IBAction func removePhotoAction(_ sender: UIButton) {
        delegate?.removePhoto(at: sender.tag)
    }
}
