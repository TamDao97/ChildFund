
var modelGroupUserSearch =
{
    Name: '',
    IsDisable: null,
    Type: '',
    PageSize: 10,
    PageNumber: 1,
    OrderBy: 'CreateDate',
    OrderType: false
};

function RefreshGroupUser() {
    $('#nameSearch').val('');
    $('#statusSearch').val('');
    $('#typeSearch').val('');

    modelGroupUserSearch.PageSize = 10;
    modelGroupUserSearch.PageNumber = 1;
    SearchGroupUser();
}

function GetGroupUserModelSearch() {
    modelGroupUserSearch.Name = $('#nameSearch').val();
    var IsDisable = $('#statusSearch').val();
    modelGroupUserSearch.IsDisable = null;
    if (IsDisable !== '') {
        if (IsDisable === '1') {
            modelGroupUserSearch.IsDisable = true;
        } else {
            modelGroupUserSearch.IsDisable = false;
        }
    }
    modelGroupUserSearch.Type = $('#typeSearch').val();
}

function SearchGroupUser() {
    var keyMess = sessionStorage.getItem("keyMess");
    if (keyMess !== null && keyMess !== '') {
        toastr.success(keyMess, { timeOut: 5000 });
        sessionStorage.removeItem("keyMess");
    }
    GetGroupUserModelSearch();
    OpenWaiting();
    $.post("/GroupUser/ListGroupUser", modelGroupUserSearch, function (result) {
        modelGroupUserSearch.PageNumber = 1;
        $("#list_data").html(result);
        CloseWaiting();
    });
}

$("#nameSearch").keydown(function (event) {
    if (event.keyCode === 13) {
        SearchGroupUser();
        return false;
    }
});

function LockGroup(id) {
    $.ajax({
        url: "/GroupUser/LockGroup?id=" + id,
        type: "POST",
        cache: false,
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('Lock_GroupUser'), { timeOut: 5000 });
                SearchGroupUser();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}
function UnLockGroup(id) {
    $.ajax({
        url: "/GroupUser/UnLockGroup?id=" + id,
        type: "POST",
        cache: false,
        success: function (data) {
            if (data.ok === true) {
                toastr.success(GetNotifyByKey('UnLock_GroupUser'), { timeOut: 5000 });
                SearchGroupUser();
            } else {
                toastr.error(data.mess, { timeOut: 5000 });
            }
        },
        error: function (response) {
            toastr.error(GetNotifyByKey('Error_Process'), { timeOut: 5000 });
        },
    });
}

function ChangeSize() {
    modelGroupUserSearch.PageNumber = 1;
    modelGroupUserSearch.PageSize = $('#pageSize').val();
    SearchGroupUser();
}
function phantrang(PageNumber) {
    modelGroupUserSearch.PageNumber = PageNumber;
    SearchGroupUser();
}
