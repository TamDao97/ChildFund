//
//  PickerView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/13/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

@IBDesignable
class ComboboxView: NibView {
    @IBOutlet weak var valueTextField: UITextField!
    @IBOutlet weak var selectButton: UIButton!
    
    var dataSource: [String] = []
    var selectedIndex = -1
    
    var completion: ((Int) -> Void)?
    
    @IBInspectable var isEnable: Bool = true {
        didSet {
            self.isUserInteractionEnabled = isEnable
        }
    }
    
    override func xibSetup() {
        super.xibSetup()
        setupView()
    }
    
    func config(with dataSource: [String], selectedIndex: Int = -1, placeHolder: String = "", isEnable: Bool = true) {
        self.dataSource = dataSource
        self.selectedIndex = selectedIndex
        self.valueTextField.placeholder = placeHolder
        self.isUserInteractionEnabled = isEnable
        
        guard selectedIndex < dataSource.count && selectedIndex >= 0 else {
            valueTextField.text = ""
            return
        }
        
        valueTextField.text = dataSource[selectedIndex]
    }
    
    func show() {
        showPicker()
    }
    
    func refresh() {
        selectedIndex = -1
        valueTextField.text = ""
    }
    
    private func setupView() {
        selectButton.addTarget(self, action: #selector(showPicker), for: .touchUpInside)
    }
    
    @objc private func showPicker() {
        guard let topViewController = UIApplication.topViewController() else {
            return
        }
        PickerViewController.show(from: topViewController, values: dataSource, selectedIndex: selectedIndex) { [weak self] selectedIndex in
            guard
                let self = self,
                self.selectedIndex != selectedIndex
            else { return }
            
            self.selectedIndex = selectedIndex
            
            guard selectedIndex < self.dataSource.count && selectedIndex >= 0 else {
                self.valueTextField.text = ""
                return
            }
            self.valueTextField.text = self.dataSource[selectedIndex]
            self.completion?(selectedIndex)
        }
    }
}
