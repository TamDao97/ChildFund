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

function GetAllPermission() {
    OpenWaiting();
    var type = $('#type').val();
    $('#checkAllData').prop('checked', false);
    $.post("/GroupUser/GetListPermission?type=" + type, function (result) {
        CloseWaiting();
        $("#list_data").html(result);
    });
}

function CreateGroupUser(isContinue) {
    dataProfile = new FormData();
    if (ValidateGroupUser()) {
        OpenWaiting();
        $.ajax({
            url: "/GroupUser/Create",
            data: JSON.stringify(modelGroupUser),
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            success: function (data) {
                if (data.ok === true) {
                    toastr.success(GetNotifyByKey('Add_Group'), { timeOut: 5000 });
                    if (isContinue === true) {
                        window.location.href = '/GroupUser/CreateGroupUser';
                    } else {
                        window.location.href = '/GroupUser/index';
                    }
                } else {
                    toastr.error(data.mess, { timeOut: 5000 });
                }
                CloseWaiting();
            },
            error: function (reponse) {
                toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
            }
        });
    }
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
function CheckPermission(id) {
    SetCheckAll();
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