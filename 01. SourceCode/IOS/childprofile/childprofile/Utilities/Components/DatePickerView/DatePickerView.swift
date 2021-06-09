//
//  DatePickerView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

class DatePickerView: NibView {
    @IBOutlet weak var valueTextField: UITextField!
    @IBOutlet weak var selectButton: UIButton!
    
    let localDateFormatter = DateFormatter(dateFormat: DateFormatString.ddMMyyyy)
    
    var date: Date?
    
    var serverDateString: String? {
        return date?.string(from: DateFormatString.shortDateFormat)
    }
    
    override func xibSetup() {
        super.xibSetup()
        setupView()
    }
    
    func config(with date: Date?) {
        self.date = date
        
        if let date = self.date {
            valueTextField.text = localDateFormatter.string(from: date)
        } else {
            valueTextField.text = ""
        }
    }
    
    private func setupView() {
        selectButton.addTarget(self, action: #selector(showPicker), for: .touchUpInside)
    }
    
    @objc private func showPicker() {
        guard let topViewController = UIApplication.topViewController() else {
            return
        }
        DateSelectViewController.show(from: topViewController, date: date ?? Date()) { [weak self] date in
            guard let self = self else { return }
            self.date = date
            self.valueTextField.text = self.localDateFormatter.string(from: date)
        }
    }
}
