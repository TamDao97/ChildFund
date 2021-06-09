//
//  PickerViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/18/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class PickerViewController: UIViewController {
    var pickerToolbar = InputToolbar()
    
    let pickerView = UIPickerView()
    let textField = UITextField()
    
    var values: [String] = []
    var selectedIndex = -1
    
    typealias pickerHandler = (Int) -> Void
    var doneHandler: pickerHandler?
    
    override func viewDidLoad() {
        super.viewDidLoad()
    
        setupView()
        configPickerView()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        textField.becomeFirstResponder()
        
        let selectedIndex = self.selectedIndex >= 0 ? self.selectedIndex : 0
        pickerView.selectRow(selectedIndex, inComponent: 0, animated: true)
    }
    
    private func setupView() {
        view.backgroundColor = UIColor.black.opacity(0.2)
        let tap: UITapGestureRecognizer = UITapGestureRecognizer(target: self, action: #selector(pickerCancelAction))
        tap.cancelsTouchesInView = false
        view.addGestureRecognizer(tap)
    }
    
    private func configPickerView() {
        pickerToolbar.doneCompletionHandler = pickerDoneAction
        pickerToolbar.cancelCompletionHandler = pickerCancelAction
        
        pickerView.dataSource = self
        pickerView.delegate = self
        pickerView.backgroundColor = .white
        
        view.addSubview(textField)
        textField.inputView = pickerView
        textField.inputAccessoryView = pickerToolbar
        textField.autocorrectionType = .no
    }
    
    @objc private func pickerDoneAction() {
        textField.resignFirstResponder()
        selectedIndex = pickerView.selectedRow(inComponent: 0)
        doneHandler?(selectedIndex)
        self.dismiss(animated: true, completion: nil)
    }
    
    @objc private func pickerCancelAction() {
        textField.resignFirstResponder()
        self.dismiss(animated: true, completion: nil)
    }
    
    static func show(from viewController: UIViewController, values: [String], selectedIndex: Int = -1, doneHandler: @escaping pickerHandler) {
        let pickerViewController = PickerViewController()
        pickerViewController.modalPresentationStyle = .overCurrentContext
        pickerViewController.modalTransitionStyle = .crossDissolve
        pickerViewController.values = values
        pickerViewController.selectedIndex = selectedIndex
        pickerViewController.doneHandler = doneHandler
        viewController.present(pickerViewController, animated: true, completion: nil)
    }
}

extension PickerViewController: UIPickerViewDataSource, UIPickerViewDelegate {
    func numberOfComponents(in pickerView: UIPickerView) -> Int {
        return 1
    }
    
    func pickerView(_ pickerView: UIPickerView, numberOfRowsInComponent component: Int) -> Int {
        return values.count
    }
    
    func pickerView(_ pickerView: UIPickerView, titleForRow row: Int, forComponent component: Int) -> String? {
        return values[row]
    }
}
