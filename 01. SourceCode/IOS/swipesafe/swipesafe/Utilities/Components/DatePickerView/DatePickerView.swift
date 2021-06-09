//
//  DatePickerView.swift
//  childprofile
//
//  Created by Thanh Luu on 1/19/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import UIKit

@IBDesignable
class DatePickerView: NibView {
    @IBOutlet weak var valueTextField: UITextField!
    @IBOutlet weak var selectButton: UIButton!
    
    var completion: (() -> Void)?
    
    var localDateFormatter: DateFormatter {
        if isShowDateAndTime {
            return DateFormatter(dateFormat: DateFormatString.ddMMyyyyHHmm)
        }
        return DateFormatter(dateFormat: DateFormatString.ddMMyyyy)
    }
    
    var datePickerMode: UIDatePicker.Mode = .date
    var minimumDate: Date?
    var maximumDate: Date?
    
    @IBInspectable var isShowDateAndTime: Bool = false {
        didSet {
            datePickerMode = isShowDateAndTime ? .dateAndTime : .date
        }
    }
    
    var date: Date?
    
    var serverDateString: String? {
        if isShowDateAndTime {
            return date?.string(from: DateFormatString.serverDateFormat)
        }
        return date?.string(from: DateFormatString.shortDateFormat)
    }
    
    override func xibSetup() {
        super.xibSetup()
        setupView()
    }
    
    func config(with date: Date?, minimumDate: Date? = nil, maximumDate: Date? = nil) {
        self.date = date
        self.maximumDate = maximumDate
        self.minimumDate = minimumDate
        
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
        DateSelectViewController.show(from: topViewController, datePickerMode: datePickerMode, date: date ?? Date(), minimumDate: minimumDate, maximumDate: maximumDate) { [weak self] date in
            guard let self = self else { return }
            self.updateDate(date)
            self.completion?()
        }
    }
    
    func updateDate(_ date: Date?) {
        self.date = date
        if let date = date {
            self.valueTextField.text = self.localDateFormatter.string(from: date)
        } else {
            self.valueTextField.text = ""
        }
    }
}
