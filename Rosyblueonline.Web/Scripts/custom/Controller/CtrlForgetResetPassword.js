﻿var CtrlForgetResetPassword = function () {
    var $frmResetPassword = null;
    var ValRP = null;
    var objLRS = new LoginRegistrationService();
    var OnLoad = function () {
        SetValidation();
    }

    var RegisterEvent = function () {
        $('#btnSubmit').click(function (e) {
            e.preventDefault();
            if ($frmResetPassword.valid()) {
                var code = myApp.getUrlData('v');
                var lid = myApp.getUrlData('l');
                var newPass = $('#txtNewPassword').val();
                objLRS.ResetForgetPassword(code, lid, newPass).then(function (data) {
                    if (data.IsSuccess == true && data.Result > 0) {
                        ClearForm();
                        uiApp.Alert({ container: '#uiPanel1', message: "Password updated", type: "success" });
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                    }
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in updating password", type: "error" });
                });
            }
        });
    }

    var ClearForm = function () {
        $('#txtNewPassword').val('');
        $('#txtConfirmPassword').val('');
    }

    var SetValidation = function () {
        $frmResetPassword = $('#frmResetPassword');
        ValRP = $frmResetPassword.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                password: {
                    required: true,
                    PasswordPattern: true
                },
                confirmPassword: {
                    required: true,
                    ComparePassword: true
                },
            }
        });

        $.validator.addMethod('PasswordPattern', function (value) {
            return value.match(/(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}/);
        }, "<p>Password must contain the following:\
            <ul><li>A <b>lowercase</b> letter</li>\
            <li>A <b>capital (uppercase)</b> letter</li>\
            <li>A <b>number</b></li>\
            <li>Minimum <b>8 characters</b></li></ul>\
            </p>");

        $.validator.addMethod('ComparePassword', function (value) {
            return value === $('#txtNewPassword').val();
        }, "Passwords do not match");
    };

    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlForgetResetPassword.init();
});