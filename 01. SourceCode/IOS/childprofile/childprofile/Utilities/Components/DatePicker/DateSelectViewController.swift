//
//  DatePickerViewController.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class DateSelectViewController: UIViewController {
    var pickerToolbar = InputToolbar()
    let datePicker = UIDatePicker()
    let textField = UITextField()
    
    var date: Date = Date()
    
    typealias datePickerHandler = (Date) -> Void
    var doneHandler: datePickerHandler?
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        setupView()
        configPickerView()
    }
    
    override func viewWillAppear(_ animated: Bool) {
        super.viewWillAppear(animated)
        datePicker.date = date
        textField.becomeFirstResponder()
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
        
        datePicker.backgroundColor = .white
        datePicker.datePickerMode = .date
        datePicker.locale = Locale(identifier: "vi")
        datePicker.calendar = Calendar(identifier: .gregorian)
        
        view.addSubview(textField)
        textField.inputView = datePicker
        textField.inputAccessoryView = pickerToolbar
        textField.autocorrectionType = .no
    }
    
    @objc private func pickerDoneAction() {
        textField.resignFirstResponder()
        doneHandler?(datePicker.date)
        self.dismiss(animated: true, completion: nil)
    }
    
    @objc private func pickerCancelAction() {
        textField.resignFirstResponder()
        self.dismiss(animated: true, completion: nil)
    }
    
    static func show(from viewController: UIViewController, date: Date = Date(), doneHandler: @escaping datePickerHandler) {
        let datePickerViewController = DateSelectViewController()
        datePickerViewController.modalPresentationStyle = .overCurrentContext
        datePickerViewController.modalTransitionStyle = .crossDissolve
        datePickerViewController.date = date
        datePickerViewController.doneHandler = doneHandler
        viewController.present(datePickerViewController, animated: true, completion: nil)
    }
}
