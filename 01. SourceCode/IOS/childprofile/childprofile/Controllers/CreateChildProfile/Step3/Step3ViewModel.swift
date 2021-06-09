//
//  Step3ViewModel.swift
//  childprofile
//
//  Created by Thanh Luu on 1/20/19.
//  Copyright © 2019 childfund. All rights reserved.
//

import Foundation

class Step3ViewModel {
    var listFamily: [ObjectInputModel] = []
    var listLiveParent: [ObjectInputModel] = []
    var listNotLiveParent: [ObjectInputModel] = []
    var notLiveParent: String = ""
    var listLiveWho: [ObjectInputModel] = []
    var liveWhoOther: String = ""
    var listWhowWriteLetter: [ObjectInputModel] = []
    var liveWriteLetterOther: String = ""
    var listFamilyMember: [FamilyMemberModel] = []
    
    let relationShipDataSource: [ComboboxItemModel] = ComboboxItemModel.getSessionData(from: Setting.relationShip.value)
}
