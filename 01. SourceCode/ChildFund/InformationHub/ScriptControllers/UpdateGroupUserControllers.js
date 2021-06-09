var modelGroupUser =
{
    Id: '',
    Name: '',
    IsDisable: true,
    Type: '',
    Description: '',
    Code: '',
    CreateBy: '',
    CreateDate: '',
    UpdateBy: '',
    UpdateDate: '',
    ListPermission: []
};

function InitModel(obj) {
    var data = JSON.parse(obj);
    modelGroupUser.Id = data.Id;
    modelGroupUser.Type = data.Type;
    $('#statusSearch').val('0');
    if (data.IsDisable === true) {
        $('#statusSearch').val('1');
    }
    $('#type').val(data.Type);
    SetCheckAll();
}
function CheckAllPermission() {
    var checkItem = document.querySelector('#checkAllData').checked;
    if (checkItem) {
        $('tbody input:checkbox[class=itemp]').each(function () {
            $(this).prop('checked', true);
        });
    } else {
        $('tbody input:checkbox[class=itemp]').each(function () {
            $(this).prop('checked', false);

        });
    }
}
function SetCheckAll() {
    var allItem = 0;
    var allItemCheck = 0;
    var itemValue;
    $('#list_data input').each(function () {
        allItem++;
        itemValue = $(this).prop('checked');
        if (itemValue === true) {
            allItemCheck++;
        }
    });
    if (allItem === allItemCheck) {
        $('#checkAllData').prop('checked', true);
    } else {
        $('#checkAllData').prop('checked', false);
    }
}
function GetAllPermission() {
    OpenWaiting();
    $('#checkAllData').prop('checked', false);
    $.post("/GroupUser/GetListPermission?type=" + $('#type').val(), function (result) {
        CloseWaiting();
        $("#list_data").html(result);
    });
}

function CheckPermission(id) {
    SetCheckAll();
}

function GetListPermissionInit(type, groupUserId) {
    $.post("/GroupUser/GetListPermissionUpdate?groupUserId=" + groupUserId + "&type=" + type, function (result) {
        $("#list_data").html(result);
    });
}


function UpdateGroupUser() {
    OpenWaiting();
    var result = ValidateGroupUser();
    if (result === true) {
        $.ajax({
            url: "/GroupUser/Update",
            data: modelGroupUser,
            cache: false,
            type: "POST",
            success: function (data) {
                if (data.ok === true) {
                    sessionStorage.setItem("keyMess", GetNotifyByKey('Update_groupUser'));
                    window.location = '/GroupUser/Index';
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Add_Location'), { timeOut: 5000 });
                CloseWaiting();
            }

        });

    }
}

function ValidateGroupUser() {
    var itemValue = '';
    modelGroupUser.ListPermission = [];
    $('#list_data input:checked').each(function () {
        itemValue = $(this).attr('value');
        modelGroupUser.ListPermission.push(itemValue);
    });
    modelGroupUser.Name = $('#Name').val();
    modelGroupUser.Code = $('#groupUserCode').val();
    modelGroupUser.Description = $('#Description').val();

    modelGroupUser.Type = $('#type').val();
    IsdisableValue = $('#statusSearch').val();
    if (IsdisableValue === '0') {
        modelGroupUser.IsDisable = false;
    }
    if (modelGroupUser.Name === '') {
        toastr.error(GetNotifyByKey('Enter_Groupname'), { timeOut: 5000 });
        return false;
    }
    if (modelGroupUser.Code === '') {
        toastr.error(GetNotifyByKey('Enter_Groupcode'), { timeOut: 5000 });
        return false;
    }
    else { return true; }

}