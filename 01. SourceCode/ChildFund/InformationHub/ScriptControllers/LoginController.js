﻿$(function () {
    $("#btnLogin").click(function (event) {
        Login();
    });

    $("body").keydown(function (event) {
        if (event.keyCode === 13) {
            Login();
        }
    });
    function Login() {
        var modelLogin = {
            UserName: $("#UserName").val(),
            Password: $("#Password").val()
        };
        if (ValidateForm(modelLogin)) {
            return;
        }
        OpenWaiting();
        $.post("/Authorize/Login", modelLogin, function (result) {
            if (result.Ok) {
                if (result.type === '3') {
                    window.location.href = "/StatisticHome/HomeWard";
                } else {
                    window.location.href = "/StatisticHome/HomeProvince";
                }
            }
            else {
                CloseWaiting();
                toastr.error(result.Message, { timeOut: 500 });
            }
        });
    }

    $("#forwardPassword").click(function () {
        window.location.href = "/Authorize/ForwardPassword";
    });

    $("#btnForwardPassword").click(function (event) {
        event.preventDefault();
        $("#btnForwardPassword").prop('disabled', true);
        var forwardPasswordModel = {
            UserName: '',
            Email:''
        };
        GetModelFormFix('ForwardPassword_', forwardPasswordModel);
        if (ValidateFormFix('ForwardPassword_', forwardPasswordModel)) {
            $("#btnForwardPassword").prop('disabled', false);
            return;
        }

        $.post("/Authorize/ForwardPassword", forwardPasswordModel, function (result) {
            if (result.Ok) {
                window.location.href = "/Authorize/ConfirmForwardPassword";
            }
            else {
                $("#btnForwardPassword").prop('disabled', false);
                toastr.error(result.Message, { timeOut: 500 });
            }
        });
        //$(this).prop('disabled', true);
    });

    $("#btnConfirmForwardPassword").click(function () {
        var forwardPasswordModel = {
            ConfirmKey: '',
            PasswordNew: '',
            ConfirmPasswordNew:''
        };
        GetModelFormFix('ConfirmForwardPassword_', forwardPasswordModel);
        if (ValidateFormFix('ConfirmForwardPassword_', forwardPasswordModel)) {
            return;
        }

        $.post("/Authorize/ConfirmForwardPassword", forwardPasswordModel, function (result) {
            if (result.Ok) {
                toastr.success(GetNotifyByKey('Update_passwordNew'), { timeOut: 5000 });
                setTimeout(function () { window.location.href = "/Authorize/Logout"; }, 500);
            }
            else {
                toastr.error(result.Message, { timeOut: 500 });
            }
        });
    });
});