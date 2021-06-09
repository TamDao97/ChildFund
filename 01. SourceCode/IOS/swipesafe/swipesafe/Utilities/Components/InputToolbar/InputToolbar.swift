//
//  PickerToolbar.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class InputToolbar: UIToolbar {
    override init(frame: CGRect) {
        super.init(frame: frame)
        setupView()
    }
    
    required init?(coder aDecoder: NSCoder) {
        super.init(coder: aDecoder)
        setupView()
    }
    
    var doneCompletionHandler: (() -> Void)?
    var cancelCompletionHandler: (() -> Void)?
    
    
    private func setupView() {
        let doneBarButton = UIBarButtonItem(title: Strings.ok, style: .plain, target: self, action: #selector(doneAction))
        let flexibleSpace = UIBarButtonItem(barButtonSystemItem: .flexibleSpace, target: nil, action: nil)
        let cancelBarButton = UIBarButtonItem(title: Strings.cancel, style: .plain, target: self, action: #selector(cancelAction))
        items = [cancelBarButton, flexibleSpace, doneBarButton]
        barTintColor = .groupTableViewBackground
        sizeToFit()
    }
    
    @objc private func doneAction() {
        doneCompletionHandler?()
    }
    
    @objc private func cancelAction() {
        cancelCompletionHandler?()
    }
}
