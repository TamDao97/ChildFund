//
//  Step2ViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

enum LearningStatus: Int {
    case childhood = 11
    case outSchool
    case handicap
    case preSchool
    case primarySchool
    case secondarySchool
}

class Step2ViewModel {
    var childName: String = ""
    var nickName: String = ""
    var gender: Int = Gender.male.rawValue
    var dateOfBirth: String = ""
    var learningStatus: String = ""
    var classInfo: String = ""
    var listSubject: [ObjectInputModel] = []
    var differentSubject: String = ""
    var bestSubject: String = ""
    var listLearningCapacity: [ObjectInputModel] = []
    var achievement: String = ""
    var listHouseWork: [ObjectInputModel] = []
    var workOther: String = ""
    var listHealth: [ObjectInputModel] = []
    var healthOther: String = ""
    var listPersonality: [ObjectInputModel] = []
    var personalityOther: String = ""
    var listHobbie: [ObjectInputModel] = []
    var hobbieOther: String = ""
    var listDream: [ObjectInputModel] = []
    var dreamOther: String = ""
    
    let primaryClassDataSource: [ComboboxItemModel] = Array(1...5).map { ComboboxItemModel(id: "\($0)", value: "\($0)") }
    let secondaryClassDataSource: [ComboboxItemModel] = Array(6...9).map { ComboboxItemModel(id: "\($0)", value: "\($0)") }
    
    
    var childNameErrorTitle: String {
        return childName.isEmpty ? Strings.emptyError : ""
    }
    
    var dateOfBirthErrorTitle: String {
        return dateOfBirth.isEmpty ? Strings.emptyError : ""
    }
    
    var learningStatusErrorTitle: String {
        return learningStatus.isEmpty ? Strings.emptyError : ""
    }
    
    var isFormValid: Bool {
        return childNameErrorTitle.isEmpty
            && dateOfBirthErrorTitle.isEmpty
            && learningStatusErrorTitle.isEmpty
    }
}
