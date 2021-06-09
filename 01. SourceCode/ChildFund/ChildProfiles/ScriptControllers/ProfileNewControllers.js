
$('.datepicker').datepicker({
    format: 'dd/mm/yyyy'
});

//Danh sách thành viên gia đình sống / không sống cùng trẻ
var _arrLiveWithChildObject = [];
var _arrUnLiveWithChildObject = [];

var dataProfile = new FormData();
var dateNow = new Date();
// them moi
var model = {
    UserLever: '2',
    Id: '',
    InfoDate: '',
    EmployeeName: '',
    EmployeeTitle: '',
    ProgramCode: '',
    ProvinceId: '',
    DistrictId: '',
    WardId: '0',
    Address: '',
    ChildCode: '',
    SchoolId: '',
    EthnicId: '',
    ReligionId: '',
    Name: '',
    NickName: '',
    Gender: '',
    DateOfBirth: '',
    LeaningStatus: '',
    ClassInfo: '',
    FavouriteSubject: '',
    LearningCapacity: '',
    Housework: '',
    Health: '',
    Personality: '',
    Hobby: '',
    Dream: '',
    FamilyMember: '',
    LivingWithParent: '',
    NotLivingWithParent: '',
    LivingWithOther: '',
    LetterWrite: '',
    HouseType: '',
    HouseRoof: '',
    HouseWall: '',
    HouseFloor: '',
    UseElectricity: '',
    SchoolDistance: '',
    WaterSourceDistance: '',
    WaterSourceUse: '',
    RoadCondition: '',
    IncomeFamily: '',
    HarvestOutput: '',
    NumberPet: '',
    FamilyType: '',
    TotalIncome: '',
    IncomeOther: '',
    StoryContent: '',
    ImagePath: '',
    ImageThumbnailPath: '',

    ConsentName: '',
    ConsentRelationship: '',
    ConsentVillage: '',
    ConsentWard: '',
    SiblingsJoiningChildFund: '',
    Malformation: '',
    Orphan: '',
    Handicap: '',
    SalesforceID: ''
};
var modelFamilyInfo = [];
function Create(isContinue) {
    dataProfile = new FormData();
    if (ValidateFormNew()) {
        $('#loader_id').addClass("loader");
        document.getElementById("overlay").style.display = "block";
        dataProfile.append("Model", JSON.stringify(model));
        $.ajax({
            url: "/ProfileNew/AddProfileNew",
            data: dataProfile,
            type: "POST",
            cache: false,
            processData: false,
            contentType: false,
            dataType: 'json',
            success: function (data) {
                if (data.ok === true) {
                    if (isContinue === true) {
                        toastr.success("Thêm hồ sơ thành công!/Create successfully", { timeOut: 5000 });
                        $('#loader_id').removeClass("loader");
                        document.getElementById("overlay").style.display = "none";
                        ResetModel();
                    } else {
                        sessionStorage.setItem("keyMess", "Thêm hồ sơ thành công!/Create successfully");
                        window.location.href = '/ProfileNew/ProfileWard';
                    }
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                $('#loader_id').removeClass("loader");
                document.getElementById("overlay").style.display = "none";
            },
            error: function (reponse) {
                toastr.error("Đã xảy ra lỗi!", { timeOut: 5000 });
            }
        });
    }
}
function CountGender(vl) {
    if (vl !== -1) {
        var relationship = $('#relationship_' + vl).val();
        if (relationship === 'R0001' || relationship === 'R0002' || relationship === 'R0004' || relationship === 'R0009' || relationship === 'R0010' || relationship === 'R0013' || relationship === 'R0014') {
            document.querySelector('#gender_' + vl).checked = true;
        } else {
            document.querySelector('#gender2_' + vl).checked = true;
        }
    }
    var countMale = 0;
    var countFeMale = 0;
    $('#familyTable tbody tr').each(function () {
        var id = '';
        var vlItem = '';
        $(this).find("select").each(function () {
            id = $(this).attr('id');
            vlItem = $(this).val();

            if (id.includes('relationship')) {
                if (vlItem === 'R0010' || vlItem === 'R0009') {
                    countMale++;
                }
                if (vlItem === 'R0006' || vlItem === 'R0008') {
                    countFeMale++;
                }
            }

        });
    });

    $('#countMale').html(countMale);
    $('#countFeMale').html(countFeMale);
    RefreshDataOnTable();
    SetCheckboxLivingWithParent();
}
function ValidateFormNew() {
    model.EmployeeName = $('#employeeName').val();
    model.EmployeeTitle = $('#employeeTitle').val();
    model.InfoDate = $('#infoDate').val();
    if (model.InfoDate !== '') {
        model.InfoDate = ProcessDate($('#infoDate').val());
    }
    model.ProgramCode = $('#programCode').val();
    model.ChildCode = $('#childCode').val();
    model.SchoolId = $('#SchoolId').val();
    model.EthnicId = $('#ethnicId').val();
    model.ReligionId = $('#religionId').val();
    model.ProvinceId = $('#provinceId').val();
    model.DistrictId = $('#districtId').val();
    model.WardId = $('#wardId').val();
    model.Address = $('#villageId').val();
    model.Gender = $("input[name='gender']:checked").val();
    model.DateOfBirth = $('#DateOfBirth').val();
    if (model.DateOfBirth !== '') {
        model.DateOfBirth = ProcessDate($('#DateOfBirth').val());
    }
    model.Name = $('#nameChild').val();
    model.NickName = $('#nickName').val();
    $('#tbLeaning input:checkbox').each(function () {
        if ($(this).is(':checked')) {
            if ($(this).attr('value') == '13') {
                model.Handicap = true;
            } else {
                model.LeaningStatus = $(this).attr('value');
            }
        }
    });
    if (model.LeaningStatus === '15') {
        model.ClassInfo = $('#classInfo').val();
    }
    if (model.LeaningStatus === '16') {
        model.ClassInfo = $('#classInfo2').val();
    }
    if (model.LeaningStatus === '17') {
        model.ClassInfo = $('#classInfo3').val();
    }

    GetDataSubjectCapacity();
    GetDataFamilyWorkHealth();
    GetDataPersonals();
    GetDataSpecialSituation();
    GetDataHouseCondition();
    GetDataOtherCondition();
    GetDataRoad();
    //Bổ sung cam kết gia đình
    GetDataSpecialInformation();

    var isBool = CheckDuplicate();
    if (isBool) {
        toastr.error("Thông tin về các thành viên trong gia đình trẻ không được trùng/ Infomation about child’s family not duplicate !", { timeOut: 5000 }); return false;
    }

    var validateInfo = GetDataFamilyInfo();
    if (validateInfo !== 1 && validateInfo !== 2 && validateInfo !== 3) {
        model.FamilyMember = JSON.stringify(validateInfo);
    } else {
        if (validateInfo === 1) {
            toastr.error("Họ tên thành viên không được để trống!/Family member's name can not be empty!", { timeOut: 5000 }); return false;
        } else if (validateInfo === 3) {
            toastr.error("Sai định dạng năm!/Wrong year format!", { timeOut: 5000 }); return false;
        } else {
            toastr.error("Mối quan hệ không được để trống!/Relationship can not be empty!", { timeOut: 5000 }); return false;
        }

    }


    var files = $("#FileUpload").get(0).files;
    if (files.length > 0) {
        dataProfile.append("Avatar", files[0]);
    }

    var fileSignature = $("#FileUploadSignature").get(0).files;
    if (fileSignature.length > 0) {
        dataProfile.append("ImageSignature", fileSignature[0]);
    }

    if (model.InfoDate === '') {
        toastr.error("Nhập ngày thu thập thông tin!/Enter date of information collection!", { timeOut: 5000 }); return false;
    } else if (model.TeachName === '') {
        toastr.error("Nhập tên cán bộ phụ trách!/Enter name of information collector!", { timeOut: 5000 }); return false;
    } else if (model.ProgramCode === '') {
        toastr.error("Nhập mã chương trình!/Enter program code!", { timeOut: 5000 }); return false;
    } else if (model.ProvinceId === '') {
        toastr.error("Nhập tỉnh thành!/Enter province!", { timeOut: 5000 }); return false;
    } else if (model.DistrictId === '') {
        toastr.error("Nhập huyện!/Enter district!", { timeOut: 5000 }); return false;
    } else if (model.ChildCode === '') {
        toastr.error("Nhập mã số trẻ!/Enter Child Number !", { timeOut: 5000 }); return false;
    }
    else if (model.ProgramCode.startsWith("199") === false && model.ProgramCode.startsWith("213") === false) {
        toastr.error("Mã chương trình phải bắt đầu là 199 hoặc 213!/Program code must be started with 199 or 213!", { timeOut: 5000 }); return false;
    }

    else if (model.Name === '') {
        toastr.error("Nhập họ tên trẻ!/Enter child's name!", { timeOut: 5000 }); return false;
    }
    else if (model.DateOfBirth === '') {
        toastr.error("Nhập ngày sinh trẻ!/Enter child's date of birth!", { timeOut: 5000 }); return false;
    }
    else if (model.LeaningStatus === '') {
        toastr.error("Chọn tình trạng học của trẻ!/Choose child's learning status!", { timeOut: 5000 }); return false;
    }

    return true;
}

function ResetModel() {
    // $('#teachName').val('');
    $('#infoDate').val($('#hid_dateTo').val());
    // $('#programCode').val('');
    $('#childCode').val('');
    $('#orderNumber').val('');
    $('#ethnicId').val('');
    $('#religionId').val('');
    $('#districtId').val('');
    $('#wardId').val('');
    $('#address').val('');
    $('#imgPreview').attr("src", "/img/avatar.png");
    $('#nameChild').val('');
    $('#nickName').val('');
    $('#DateOfBirth').val('');
    $('#SalesforceID').val('');
    document.querySelector('#gender').checked = true;
    $('#tbLeaning input:checkbox').each(function () {
        vlItem = $(this).attr('value');
        $(this).prop('checked', false);
    });
}

function InitModel() {
    ChangeProvince();
    SetVillage();
}

function SetVillage() {
    var village = document.getElementById("villageId");
    var villageSelected = village.options[village.selectedIndex].text;

    $('#consentVillage').val(villageSelected);
}

function SetWard() {
    var ward = document.getElementById("wardId");
    var wardSelected = ward.options[ward.selectedIndex].text;

    $('#consentWard').val(wardSelected);
}

function AddRowTableFamilyInfo() {
    modelFamilyInfo = GetDataFamilyInfoPost();
    $.post("/ProfileCategory/FamilyInfo?typeAction=add&IdProfile=&model=" + JSON.stringify(modelFamilyInfo), function (result) {
        $("#divfamilyInfo").html(result);
        //modelFamilyInfo = JSON.parse($('#strDataTableFamilyInfo').val());
        CountGender(-1);
    });
}

function DeleteRow(index) {
    $('#familyTable tbody tr').each(function (i, item) {
        var stt = parseInt($(this).children('td').slice(0, 1).html().trim());
        if (stt == index) {
            item.remove();
            return;
        }
    });
    //document.getElementById("familyTable").getElementsByTagName('tbody')[0].deleteRow(index);
    CountGender(-1);
}

function ChangeProvince() {
    var comboboxid = $('#provinceId').val();
    $.post("/Combobox/DistrictByUser?Id=" + $('#provinceId').val(), function (result) {
        $("#districtId").html(result);
        ChangeDistrict();
    });
}

function ChangeDistrict() {
    $.post("/Combobox/WardByUser?Id=" + $('#districtId').val(), function (result) {
        $("#wardId").html(result);
        ChangeWard();
    });
}

function ChangeWard() {
    $.post("/Combobox/GetVillageByWrad?id=" + $('#wardId').val(), function (result) {
        $("#villageId").html(result);
        SetVillage();
        SetWard();
    });
    $.post("/Combobox/SchoolCBB?id=" + $('#wardId').val(), function (result) {
        $("#SchoolId").html(result);
    });
}

function ShowManageVillage() {
    $("#txtVillageName").val("");
    $("#txtVillageId").val("");
    $('#modalManageVillage').modal({
        show: 'true'
    });
}

function EditVillage(item) {
    $("#txtVillageName").val(item.Name);
    $("#txtVillageId").val(item.Id);
}

function DeleteVillage(id) {
    $.ajax({
        url: "/AreaUser/DeleteVillage?id=" + id,
        type: "POST",
        dataType: 'json',
        success: function (data) {
            $("#txtVillageName").val("");
            $("#txtVillageId").val("");
            $.post("/Combobox/GetVillageByWrad?id=" + $('#wardId').val(), function (result) {
                $("#villageId").html(result);
            });
            $.post("/Combobox/GetListVillage?id=" + $('#wardId').val(), function (result) {
                $("#listVillage").html(result);
            });
        }
    });
}

function SaveVillage() {
    var dataModel = {
        Id: $("#txtVillageId").val(),
        WardId: $("#wardId").val(),
        Name: $("#txtVillageName").val()
    };
    $.ajax({
        url: "/AreaUser/SaveVillage",
        data: dataModel,
        type: "POST",
        dataType: 'json',
        success: function (data) {
            $("#txtVillageName").val("");
            $("#txtVillageId").val("");
            $.post("/Combobox/GetVillageByWrad?id=" + $('#wardId').val(), function (result) {
                $("#villageId").html(result);
            });
            $.post("/Combobox/GetListVillage?id=" + $('#wardId').val(), function (result) {
                $("#listVillage").html(result);
            });
        }
    });
}

function ChangeLeaningStatus(value) {
    var vlItem = ''; var check = false;
    $('#tbLeaning input:checkbox').each(function () {
        if (value != '13') {
            vlItem = $(this).attr('value');
            if (vlItem == value) {
                check = $(this).is(":checked");
            }
            else if (vlItem != value && vlItem != '13') {
                $(this).prop('checked', false);
            }
        }
    });
    if (value == '15') {
        if (check == true) {
            $('#classInfo').prop('disabled', false);
            $('#classInfo2').val('');
            $('#classInfo2').prop('disabled', true);
            $('#classInfo3').val('');
            $('#classInfo3').prop('disabled', true);
        } else {
            $('#classInfo').prop('disabled', true);
            $('#classInfo').val('');
        }
    } else
        if (value == '16') {
            if (check == true) {
                $('#classInfo2').prop('disabled', false);
                $('#classInfo').val('');
                $('#classInfo').prop('disabled', true);
                $('#classInfo3').val('');
                $('#classInfo3').prop('disabled', true);
            } else {
                $('#classInfo2').prop('disabled', true);
                $('#classInfo2').val('');
            }
        } else if (value == '17') {
            if (check == true) {
                $('#classInfo3').prop('disabled', false);
                $('#classInfo').val('');
                $('#classInfo').prop('disabled', true);
                $('#classInfo2').val('');
                $('#classInfo2').prop('disabled', true);
            } else {
                $('#classInfo3').prop('disabled', true);
                $('#classInfo3').val('');
            }
        }
        else {
            $('#classInfo').val('');
            $('#classInfo2').val('');
            $('#classInfo3').val('');
            $('#classInfo3').prop('disabled', true);
            $('#classInfo2').prop('disabled', true);
            $('#classInfo').prop('disabled', true);
        }
}

function ChangeCheckbox(value, table, name, elem) {
    var vlItem = ''; var str = table + ' input:checkbox';
    var namec = '';
    $(str).each(function () {
        vlItem = $(this).attr('value');
        namec = $(this).attr("name");
        if (vlItem != value && namec == name) {
            $(this).prop('checked', false);
        }
    });

    RefreshDataOnTable();

    $('#familyTable tbody tr').each(function () {
        $(this).remove();
    });

    var ListRelationShip = [];
    var objClone = {};
    var objExisted = null;
    var objAdd = {
        Name: '',
        Dateb: '',
        Job: '',
        RelationshipId: '',
        LiveWithChild: 1
    };

    //Trường hợp check có bố/mẹ
    if (elem.checked) {
        if (value == 01 && name == 'cklivingWithParent') {
            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0001');
            if (!objExisted) {
                objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0001');

                if (!objExisted) {
                    objAdd.RelationshipId = 'R0001';
                    objClone = Object.assign({}, objAdd);
                    _arrLiveWithChildObject.push(objClone)
                } else {
                    objExisted.LiveWithChild = 1;
                    objClone = Object.assign({}, objExisted);
                    _arrLiveWithChildObject.push(objClone);
                }
                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0001');
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0007');
            if (!objExisted) {
                objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0007');

                if (!objExisted) {
                    objAdd.RelationshipId = 'R0007';
                    objClone = Object.assign({}, objAdd);
                    _arrLiveWithChildObject.push(objClone);
                } else {
                    objExisted.LiveWithChild = 1;
                    objClone = Object.assign({}, objExisted);
                    _arrLiveWithChildObject.push(objClone);
                }
                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0007');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 04 && name == 'cklivingWithParent') {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0001');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0001');
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0007');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0007');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 01 && name == 'cklivingWithOther') {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0004');
            if (!objExisted) {
                objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0004');

                if (!objExisted) {
                    objAdd.RelationshipId = 'R0004';
                    objClone = Object.assign({}, objAdd);
                    _arrLiveWithChildObject.push(objClone);
                } else {
                    objExisted.LiveWithChild = 1;
                    objClone = Object.assign({}, objExisted);
                    _arrLiveWithChildObject.push(objClone);
                }

                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0004');
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0005');
            if (!objExisted) {
                objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0005');

                if (!objExisted) {
                    objAdd.RelationshipId = 'R0005';
                    objClone = Object.assign({}, objAdd);
                    _arrLiveWithChildObject.push(objClone);
                } else {
                    objExisted.LiveWithChild = 1;
                    objClone = Object.assign({}, objExisted);
                    _arrLiveWithChildObject.push(objClone);
                }

                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0005');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 02) {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0004');
            if (!objExisted) {
                objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0004');

                if (!objExisted) {
                    objAdd.RelationshipId = 'R0004';
                    objClone = Object.assign({}, objAdd);
                    _arrLiveWithChildObject.push(objClone)
                } else {
                    objExisted.LiveWithChild = 1;
                    objClone = Object.assign({}, objExisted);
                    _arrLiveWithChildObject.push(objClone);
                }

                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0004');
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0005');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0005');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 03) {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0005');
            if (!objExisted) {
                objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0005');

                if (!objExisted) {
                    objAdd.RelationshipId = 'R0005';
                    objClone = Object.assign({}, objAdd);
                    _arrLiveWithChildObject.push(objClone);
                } else {
                    objExisted.LiveWithChild = 1;
                    objClone = Object.assign({}, objExisted);
                    _arrLiveWithChildObject.push(objClone);
                }
                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0005');
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0004');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0004');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    } else {
        if (value == 01 && name == 'cklivingWithParent') {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0001');
            if (objExisted) {
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0001');
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0007');
            if (objExisted) {
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0007');
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 04 && name == 'cklivingWithParent') {

            objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0001');
            if (objExisted) {
                objExisted.LiveWithChild = 1;
                objClone = Object.assign({}, objExisted);
                _arrLiveWithChildObject.push(objClone);
                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0001');
            }

            objExisted = _arrUnLiveWithChildObject.find(r => r.RelationshipId == 'R0007');
            if (objExisted) {
                objExisted.LiveWithChild = 1;
                objClone = Object.assign({}, objExisted);
                _arrLiveWithChildObject.push(objClone);
                _arrUnLiveWithChildObject = _arrUnLiveWithChildObject.filter(r => r.RelationshipId != 'R0007');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 01 && name == 'cklivingWithOther') {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0004');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0004');
            }

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0005');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0005');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 02) {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0004');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0004');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        } else if (value == 03) {

            objExisted = _arrLiveWithChildObject.find(r => r.RelationshipId == 'R0005');
            if (objExisted) {
                objExisted.LiveWithChild = 0;
                objClone = Object.assign({}, objExisted);
                _arrUnLiveWithChildObject.push(objClone);
                _arrLiveWithChildObject = _arrLiveWithChildObject.filter(r => r.RelationshipId != 'R0005');
            }

            ListRelationShip = _arrLiveWithChildObject.concat(_arrUnLiveWithChildObject);

            $.ajax({
                url: '/ProfileCategory/FamilyInfoRow',
                data: JSON.stringify(ListRelationShip),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    $("#familyTable tbody").append(data);
                    CountGender(-1);
                },
                error: function (error) {
                    console.log(error);
                }
            });
        }
    }
}

function GetDataSubjectCapacity() {
    var favouriteSubject = JSON.parse($('#hidfavouriteSubjectModel').val());
    var learningCapacity = JSON.parse($('#hidlearningCapacityModel').val());
    var vlItem = ''; var name = '';
    $('#tbfavouriteSubjectModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        favouriteSubject.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'Subject') {
                e.Check = true;
            }
        });
    });
    $('#tblearningCapacityModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        learningCapacity.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'Capacity') {
                e.Check = true;
            }
        });
    });

    favouriteSubject.OtherValue = $('#OtherSubject').val();
    favouriteSubject.OtherValue2 = $('#BestSubject').val();
    learningCapacity.OtherValue = $('#learningCapacityOther').val();
    model.FavouriteSubject = JSON.stringify(favouriteSubject);
    model.LearningCapacity = JSON.stringify(learningCapacity);
}

function GetDataFamilyWorkHealth() {
    var housework = JSON.parse($('#hidhouseworkModel').val());
    var health = JSON.parse($('#hidhealthModel').val());
    var vlItem = ''; var name = '', vlOther = '';
    $('#tbhouseworkModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        housework.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'FamilyWork') {
                e.Check = true;
            }
        });
    });
    $('#tbhealthModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        health.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'Health') {
                e.Check = true;
                e.OtherValue = $('#healthItemOther_' + vlItem).val();
            }
        });
    });

    housework.OtherValue = $('#familyWorkOther').val();
    health.OtherValue = $('#healthOther').val();
    model.Housework = JSON.stringify(housework);
    model.Health = JSON.stringify(health);
}

function GetDataPersonals() {
    var personality = JSON.parse($('#hidpersonalityModel').val());
    var hobby = JSON.parse($('#hidhobbyModel').val());
    var dream = JSON.parse($('#hiddreamModel').val());
    var vlItem = ''; var name = '';
    $('#tbpersonalityModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        personality.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckPersonalit') {
                e.Check = true;
            }
        });
    });
    $('#tbhobbyModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        hobby.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckHobby') {
                e.Check = true;
            }
        });
    });
    $('#tbdreamModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        dream.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckDream') {
                e.Check = true;
            }
        });
    });
    personality.OtherValue = $('#PersonalitOther').val();
    hobby.OtherValue = $('#HobbyOther').val();
    dream.OtherValue = $('#DreamOther').val();
    model.Personality = JSON.stringify(personality);
    model.Hobby = JSON.stringify(hobby);
    model.Dream = JSON.stringify(dream);
}

function GetDataSpecialSituation() {
    var livingWithParent = JSON.parse($('#hidlivingWithParentModel').val());
    var livingWithOther = JSON.parse($('#hidlivingWithOtherModel').val());
    var letterWrite = JSON.parse($('#hidletterWriteModel').val());
    var notLivingWithParent = JSON.parse($('#hidnotLivingWithParentModel').val());
    var vlItem = ''; var name = '', vlOther = '';
    $('#tblivingWithParentModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        livingWithParent.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'cklivingWithParent') {
                e.Check = true;
            }
        });
    });
    $('#tblivingWithOtherModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        livingWithOther.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'cklivingWithOther') {
                e.Check = true;
            }
        });
    });
    $('#tbletterWriteModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        letterWrite.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckletterWrite') {
                e.Check = true;
            }
        });
    });
    $('#tbnotLivingWithParentModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        notLivingWithParent.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'cknotLivingWithParent') {
                e.Check = true;
                e.OtherValue = $('#notLivingWithParent_' + vlItem).val();
            }
        });
    });
    livingWithOther.OtherValue = $('#livingWithOther').val();
    letterWrite.OtherValue = $('#letterWriteOther').val();
    model.LivingWithParent = JSON.stringify(livingWithParent);
    model.LivingWithOther = JSON.stringify(livingWithOther);
    model.LetterWrite = JSON.stringify(letterWrite);
    model.NotLivingWithParent = JSON.stringify(notLivingWithParent);
}

function GetDataHouseCondition() {
    var houseType = JSON.parse($('#hidhouseTypeModel').val());
    var houseRoof = JSON.parse($('#hidhouseRoofModel').val());
    var houseWall = JSON.parse($('#hidhouseWallModel').val());
    var houseFloor = JSON.parse($('#hidhouseFloorModel').val());
    var vlItem = ''; var name = '';
    $('#tbhouseTypeModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        houseType.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckhouseType') {
                e.Check = true;
            }
        });
    });
    $('#tbhouseRoofModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        houseRoof.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckhouseRoof') {
                e.Check = true;
            }
        });
    });
    $('#tbhouseWallModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        houseWall.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckhouseWall') {
                e.Check = true;
            }
        });
    });
    $('#tbhouseFloorModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        houseFloor.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckhouseFloor') {
                e.Check = true;
            }
        });
    });

    houseType.OtherValue = $('#houseTypeOther').val();
    houseRoof.OtherValue = $('#houseRoofOther').val();
    houseWall.OtherValue = $('#houseWallOther').val();
    houseFloor.OtherValue = $('#houseFloorOther').val();

    model.HouseType = JSON.stringify(houseType);
    model.HouseRoof = JSON.stringify(houseRoof);
    model.HouseWall = JSON.stringify(houseWall);
    model.HouseFloor = JSON.stringify(houseFloor);
    return;
}

function GetDataOtherCondition() {
    var useElectricity = JSON.parse($('#hiduseElectricityModel').val());
    var schoolDistance = JSON.parse($('#hidschoolDistanceModel').val());
    var clinicDistance = JSON.parse($('#hidclinicDistanceModel').val());
    var waterSourceDistance = JSON.parse($('#hidwaterSourceDistanceModel').val());
    var waterSourceUse = JSON.parse($('#hidwaterSourceUseModel').val());
    var vlItem = ''; var name = '';
    $('#tbuseElectricityModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        useElectricity.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckuseElectricity') {
                e.Check = true;
            }
        });
    });
    $('#tbschoolDistanceModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        schoolDistance.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckschoolDistance') {
                e.Check = true;
            }
        });
    });
    $('#tbclinicDistanceModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        clinicDistance.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckclinicDistance') {
                e.Check = true;
            }
        });
    });
    $('#tbwaterSourceDistanceModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        waterSourceDistance.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckwaterSourceDistance') {
                e.Check = true;
            }
        });
    });
    $('#tbwaterSourceUseModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        waterSourceUse.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckwaterSourceUse') {
                e.Check = true;
            }
        });
    });

    waterSourceUse.OtherValue = $('#waterSourceUseOther').val();

    model.UseElectricity = JSON.stringify(useElectricity);
    model.SchoolDistance = JSON.stringify(schoolDistance);
    model.ClinicDistance = JSON.stringify(clinicDistance);
    model.WaterSourceDistance = JSON.stringify(waterSourceDistance);
    model.WaterSourceUse = JSON.stringify(waterSourceUse);
}

function GetDataRoad() {
    var roadCondition = JSON.parse($('#hidroadConditionModel').val());
    var incomeFamily = JSON.parse($('#hidincomeFamilyModel').val());
    var harvestOutput = JSON.parse($('#hidharvestOutputModel').val());
    var incomeOther = JSON.parse($('#hidincomeOtherModel').val());
    var numberPet = JSON.parse($('#hidnumberPetModel').val());
    var vlItem = ''; var name = '';

    $('#tbharvestOutputModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        harvestOutput.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckharvestOutput') {
                e.Check = true;
            }
        });
    });

    $('#tbnumberPetModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        numberPet.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'cknumberPet') {
                e.Check = true;
            }
        });
    });


    $('#tbroadConditionModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        roadCondition.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckroadCondition') {
                e.Check = true;
            }
        });
    });
    $('#tbincomeOtherModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        name = $(this).attr("name");
        incomeOther.ListObject.forEach(function (e) {
            if (e.Id === vlItem && name === 'ckincomeOther') {
                e.Check = true;
            }
        });
    });

    $('#tbincomeFamilyModel input:text').each(function () {
        vlItem = $(this).attr('name');
        incomeFamily.ListObject.forEach(function (e) {
            if (e.Id === vlItem) {
                e.Value = $('#incomeFamilyModel_' + vlItem).val();
            }
        });
    });

    //$('#tbharvestOutputModel input:text').each(function () {
    //    vlItem = $(this).attr('name');
    //    harvestOutput.ListObject.forEach(function (e) {
    //        if (e.Id === vlItem) {
    //            e.Value = $('#harvestOutputModel_' + vlItem).val();
    //        }
    //    });
    //});

    model.FamilyType = $('#familyType').val();
    //model.NumberPet = $('#numberPet').val();
    model.TotalIncome = $('#totalIncome').val();
    incomeOther.OtherValue = $('#incomeOtherDetail').val();
    harvestOutput.OtherValue = $('#harvestOutputOther').val();
    numberPet.OtherValue = $('#numberPetOther').val();

    model.RoadCondition = JSON.stringify(roadCondition);
    model.IncomeFamily = JSON.stringify(incomeFamily);
    model.HarvestOutput = JSON.stringify(harvestOutput);
    model.NumberPet = JSON.stringify(numberPet);
    model.IncomeOther = JSON.stringify(incomeOther);
}

function GetDataSpecialInformation() {
    var siblingsJoiningChildFund = JSON.parse($('#hidsiblingsJoiningChildFundModel').val());
    var malformation = JSON.parse($('#hidmalformationModel').val());
    var orphan = JSON.parse($('#hidorphanModel').val());

    var vlItem = '';
    $('#tbsiblingsJoiningChildFundModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        siblingsJoiningChildFund.ListObject.forEach(function (e) {
            if (e.Id === vlItem) {
                e.Check = true;
                if (e.OtherName != "" && e.OtherName != null) {
                    e.OtherValue = $("#tbsiblingsJoiningChildFundModel #txtNumber").val();
                }
            }
        });
    });

    $('#tbmalformationModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        malformation.ListObject.forEach(function (e) {
            if (e.Id === vlItem) {
                e.Check = true;
            }
        });
    });

    $('#tborphanModel input:checked').each(function () {
        vlItem = $(this).attr('value');
        orphan.ListObject.forEach(function (e) {
            if (e.Id === vlItem) {
                e.Check = true;
            }
        });
    });

    model.ConsentName = $('#consentName').val();
    model.ConsentRelationship = $('#consentRelationship').val();
    model.ConsentVillage = $('#consentVillage').val();
    model.ConsentWard = $('#consentWard').val();
    model.SiblingsJoiningChildFund = JSON.stringify(siblingsJoiningChildFund);
    model.Malformation = JSON.stringify(malformation);
    model.Orphan = JSON.stringify(orphan);
}

function ProcessDate(date) {
    //var dateTemp = date.split('-');
    //var rs = '';
    //if (dateTemp[0].length === 4) {
    //    rs = dateTemp[2] + '/' + dateTemp[1] + '/' + dateTemp[0];
    //} else {
    //    rs = dateTemp[0] + '/' + dateTemp[1] + '/' + dateTemp[2];
    //}
    return date;
}
function GetDataFamilyInfo() {
    var erroValidate = 0;
    var datainfo = [];
    $('#familyTable tbody tr').each(function () {
        var id = ''; var obj = {};
        var vlItem = '';
        $(this).find("input[type='text']").each(function () {
            id = $(this).attr('id');
            vlItem = $(this).val();
            if (id.includes('name')) {
                if (vlItem === '') {
                    erroValidate = 1;
                    return false;
                }
                obj.Name = vlItem;
            }
            if (id.includes('relationship')) {
                if (vlItem === '') {
                    erroValidate = 2;
                    return false;
                }
                obj.RelationshipId = vlItem;
            }

            if (id.includes('dateb')) {
                if (vlItem !== '') {
                    if (isNaN(vlItem)) {
                        erroValidate = 3;
                    } else if (vlItem.length !== 4) {
                        erroValidate = 3;
                    } else { obj.DateOfBirth = "01/01/" + vlItem; }
                } else {
                    obj.DateOfBirth = null;
                }
            }
        });

        obj.RelationshipId = $(this).find('select[name="relationship"]').val();
        obj.Job = $(this).find('select[name="job"]').val();

        $(this).find("input[type='radio']").each(function () {
            id = $(this).attr('id');
            vlItem = $(this).attr('value');
            if (id.includes('gender') && $(this).is(':checked')) {
                obj.Gender = vlItem;
            }
            if (id.includes('live') && $(this).is(':checked')) {
                obj.LiveWithChild = vlItem;
            }
        });

        datainfo.push(obj);
    });
    if (erroValidate > 0) {
        return erroValidate;
    } else {
        return datainfo;
    }

}
function GetDataFamilyInfoPost() {
    var datainfo = [];
    $('#familyTable tbody tr').each(function () {
        var id = ''; var obj = {};
        var vlItem = '';
        $(this).find("input[type='text']").each(function () {
            id = $(this).attr('id');
            vlItem = $(this).val();
            if (id.includes('name')) {
                obj.Name = vlItem;
            }
            if (id.includes('relationship')) {
                obj.RelationshipId = vlItem;
            }
            //if (id.includes('job')) {
            //    obj.Job = vlItem;
            //}
            if (id.includes('dateb')) {
                if (vlItem !== '') {
                    if (isNaN(vlItem)) {
                        erroValidate = 3;
                    } else if (vlItem.length !== 4) {
                        erroValidate = 3;
                    } else { obj.DateOfBirth = "01/01/" + vlItem; }
                } else {
                    obj.DateOfBirth = null;
                }
            }

        });
        //xử lý date mr.Hiep về chung 1 định dang dd/MM/yyyy
        //var dateTemp = $(this).find("input[type='date']").val();
        //if (dateTemp !== '') {
        //    obj.DateOfBirth = ProcessDate(dateTemp);
        //} else {
        //    obj.DateOfBirth = null;
        //}

        obj.RelationshipId = $(this).find('select[name="relationship"]').val();
        obj.Job = $(this).find('select[name="job"]').val();
        $(this).find("input[type='radio']").each(function () {
            id = $(this).attr('id');
            vlItem = $(this).attr('value');
            if (id.includes('gender') && $(this).is(':checked')) {
                obj.Gender = vlItem;
            }
            if (id.includes('live') && $(this).is(':checked')) {
                obj.LiveWithChild = vlItem;
            }
        });

        datainfo.push(obj);
    });
    return datainfo;
}

function uploadImageSignature() {
    $('#FileUploadSignature').trigger('click');
    $('#FileUploadSignature').change(function () {
        var data = new FormData();
        let reader = new FileReader();
        var files = $("#FileUploadSignature").get(0).files;
        if (files.length > 0) {
            data.append("ImageSignature", files[0]);
            reader.readAsDataURL(files[0]);
            reader.onload = () => {
                $('#imgPreviewSignature').attr("src", reader.result);
            };
        }
    });
}
function clearImageSignature() {
    IsRemoteImgSignature = true;
    $('#imgPreviewSignature').attr("src", "/img/avatar.png");
}

//Bổ sung phần logic mới trên giao diện thêm mới
function validateInput(classLevel, elemt) {
    setTimeout(function () { validateClass(classLevel, elemt); }, 1500);
}

function validateClass(classLevel, elemt) {
    var listPrimary = ['1', '2', '3', '4', '5'];
    var listSecondary = ['6', '7', '8', '9'];
    var listHigh = ['10', '11', '12'];

    var fisrtCharacter = elemt.value.trim().charAt(0);
    var secondCharacter = elemt.value.trim().substr(0, 2);
    if (fisrtCharacter && secondCharacter) {
        if (classLevel == 'primary') {
            if (listPrimary.indexOf(fisrtCharacter) === -1) {
                elemt.value = '';
                toastr.error("Thông tin lớp nhập vào chưa đúng với bậc Tiểu học.", { timeOut: 3000 });
            }
        } else if (classLevel == 'secondary') {
            if (listSecondary.indexOf(fisrtCharacter) === -1) {
                elemt.value = '';
                toastr.error("Thông tin lớp nhập vào chưa đúng với bậc Trung học cơ sở.", { timeOut: 3000 });
            }
        } else if (classLevel == 'high') {
            if (listHigh.indexOf(secondCharacter) === -1) {
                elemt.value = '';
                toastr.error("Thông tin lớp nhập vào chưa đúng với bậc Trung học phổ thông.", { timeOut: 3000 });
            }
        }
    } else
        toastr.error("Mời nhập vào thông tin.", { timeOut: 3000 });
}

function CheckDuplicate() {
    var boolean = false;
    var ListRelationShipId = [];
    $('[name="relationship"]').each(function (index, item) {
        ListRelationShipId.push(item.value);
    });

    for (var i = 0; i < ListRelationShipId.length - 1; i++) {
        for (var j = i + 1; j < ListRelationShipId.length; j++) {
            var valI = ListRelationShipId[i];
            var valJ = ListRelationShipId[j];
            if (valI == 'R0001' || valI == 'R0015') {
                boolean = (valJ == 'R0001' || valJ == 'R0015');
            } else if (valI == 'R0007' || valI == 'R0016') {
                boolean = (valJ == 'R0007' || valJ == 'R0016');
            } else {
                boolean = valI == valJ;
            }

            if (boolean)
                return boolean;
        }
    }
    return boolean;
}

function SetCheckboxLivingWithParent() {
    var _arrRelationship = [];
    $('#familyTable tbody tr').each(function () {
        var id = '';
        var vlItem = '';
        var RelationshipId = '';
        var LiveWithChild = '';
        RelationshipId = $(this).find('select[name="relationship"]').val();
        $(this).find("input[type='radio']").each(function () {
            id = $(this).attr('id');
            vlItem = $(this).attr('value');
            if (id.includes('live') && $(this).is(':checked')) {
                LiveWithChild = vlItem;
            }
        });

        if (LiveWithChild == 1) {
            _arrRelationship.push(RelationshipId);
        }
    });

    if (_arrRelationship.indexOf('R0001') !== -1 && _arrRelationship.indexOf('R0007') !== -1) {
        $('#tbnotLivingWithParentModel tbody tr').each(function () {
            $(this).find('input[type="checkbox"]').each(function () {
                $(this).attr('disabled', true);
            });
        });
        $('[name="cklivingWithParent"]').each(function (index, item) {
            item.checked = item.value == 01 ? true : false;
        });
    } else {
        $('#tbnotLivingWithParentModel tbody tr').each(function () {
            $(this).find('input[type="checkbox"]').each(function () {
                $(this).attr('disabled', false);
            });
        });
        $('[name="cklivingWithParent"]').each(function (index, item) {
            item.checked = item.value == 04 ? true : false;
        });
    }

    if (_arrRelationship.indexOf('R0004') !== -1 && _arrRelationship.indexOf('R0005') !== -1) {
        $('[name="cklivingWithOther"]').each(function (index, item) {
            item.checked = item.value == 01 ? true : false;
        });
    } else if (_arrRelationship.indexOf('R0004') !== -1) {
        $('[name="cklivingWithOther"]').each(function (index, item) {
            item.checked = item.value == 02 ? true : false;
        });
    } else if (_arrRelationship.indexOf('R0005') !== -1) {
        $('[name="cklivingWithOther"]').each(function (index, item) {
            item.checked = item.value == 03 ? true : false;
        });
    } else {
        $('[name="cklivingWithOther"]').each(function (index, item) {
            item.checked = false;
        });
    }

    if (_arrRelationship.indexOf('R0001') != -1 && _arrRelationship.indexOf('R0007') != -1) {
        bothParentsDisable = true;
    }
    else if (_arrRelationship.indexOf('R0001') != -1) {
        fatherDisable = true;
    }
    else if (_arrRelationship.indexOf('R0007') != -1) {
        motherDisable = true;
    }

    $('#tbnotLivingWithParentModel tbody tr').each(function (i, item) {
        var checkValue = $(this).find('input[type="checkbox"]').val();

        if (_arrRelationship.indexOf('R0001') != -1 && _arrRelationship.indexOf('R0007') != -1) {
            $('[name="cknotLivingWithParent"]').each(function () {
                if ($(this).val() == 01) {
                    $(this).attr('checked', false);
                    $(this).attr('disabled', true);
                }
            });
        } else if (_arrRelationship.indexOf('R0001') != -1) {
            $('[name="cknotLivingWithParent"]').each(function () {
                if ($(this).val() == 01 || $(this).val() == 02 || $(this).val() == 04 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 09) {
                    $(this).attr('checked', false);
                    $(this).attr('disabled', true);
                }
            });
        } else if (_arrRelationship.indexOf('R0007') != -1) {
            $('[name="cknotLivingWithParent"]').each(function () {
                if ($(this).val() == 01 || $(this).val() == 03 || $(this).val() == 05 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 10) {
                    $(this).attr('checked', false);
                    $(this).attr('disabled', true);
                }
            });
        }
    });
}

function RefreshDataOnTable() {
    _arrLiveWithChildObject = [];
    _arrUnLiveWithChildObject = [];

    $('#familyTable tbody tr').each(function () {
        var id = '';
        var RelationshipId = '';
        var LiveWithChild = '';
        var Name = $(this).find('input[name="name"]').val();
        var Dateb = $(this).find('input[name="dateb"]').val();
        var Job = $(this).find('select[name="job"]').val();
        RelationshipId = $(this).find('select[name="relationship"]').val();
        $(this).find("input[type='radio']").each(function (index, item) {
            id = $(this).attr('id');
            if (id.includes('live') && item.checked) {
                LiveWithChild = item.value;
            }
        });

        var obj = {
            Name: Name,
            Dateb: Dateb,
            Job: Job,
            RelationshipId: RelationshipId,
            LiveWithChild: LiveWithChild
        };

        if (LiveWithChild == 1 && !_arrLiveWithChildObject.find(r => r.RelationshipId == RelationshipId)) {
            _arrLiveWithChildObject.push(obj);
        } else if (LiveWithChild != 1 && !_arrUnLiveWithChildObject.find(r => r.RelationshipId == RelationshipId)) {
            _arrUnLiveWithChildObject.push(obj);
        }
    });
}

$('#tbnotLivingWithParentModel input:checkbox').change(function () {
    RefreshDataOnTable();
    var checkValue = $(this).val();
    var isChecked = $(this).prop('checked');

    if (checkValue == 01) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() != 01) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 02) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() == 01 || $(this).val() == 04 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 09) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 03) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() == 01 || $(this).val() == 05 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 10) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 04) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() == 01 || $(this).val() == 02 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 09) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 05) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() == 01 || $(this).val() == 03 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 10) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 06) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() != 06) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 07) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() != 07) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 08) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() != 08) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 09) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() == 01 || $(this).val() == 04 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 02) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    } else if (checkValue == 10) {
        $('#tbnotLivingWithParentModel input:checkbox').each(function () {
            if ($(this).val() == 01 || $(this).val() == 05 || $(this).val() == 06 || $(this).val() == 07 || $(this).val() == 08 || $(this).val() == 03) {
                $(this).prop('checked', false);
                $(this).prop('disabled', isChecked)
            }
        });
    }



});