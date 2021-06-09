//
//  HighlightView.swift
//  swipesafe
//
//  Created by Thanh Luu on 5/7/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class HighlightView: UIView, UIGestureRecognizerDelegate {
    /* Called when the view goes to normal state (set desired appearance) */
    var onNormal = {}
    /* Called when the view goes to pressed state (set desired appearance) */
    var onPressed = {}
    /* Called when the view is released (perform desired action) */
    var onReleased = {}
    
    override init(frame: CGRect) {
        super.init(frame: frame)
        setupView()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        setupView()
    }
    
    func setupView() {
        let recognizer = UILongPressGestureRecognizer(target: self, action: #selector(touched(sender:)))
        recognizer.delegate = self
        recognizer.minimumPressDuration = 0.0
        addGestureRecognizer(recognizer)
        
        onNormal()
    }
    
    @objc func touched(sender: UILongPressGestureRecognizer) {
        if sender.state == .began {
            onPressed()
        } else if sender.state == .ended {
            onNormal()
            onReleased()
        }
    }
}

