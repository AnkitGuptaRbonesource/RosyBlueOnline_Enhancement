var CtrlResetPassword = function () {
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
                var oldPass = $('#txtOldPassword').val();
                var newPass = $('#txtNewPassword').val();
                objLRS.ResetPassword(oldPass, newPass).then(function (data) {
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


        $('#btnCancel').click(function (e) {
            e.preventDefault(); 
            objLRS.Backtodashboard().then(function (data) {
               
                if (data !== null) { 
                    if (data == 3) { 
                         
                     location.href = '/Dashboard/Customer';
                       
                    }
                    if (data == 2) {
                        location.href = '/Dashboard/Admin';
                    }
                    if (data == 8) {
                        location.href = '/Dashboard';
                    }
                    if (data == 9) {
                        location.href = '/Dashboard';
                    }

                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "error" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "", type: "error" });
            });

        });

    }



    var ClearForm = function () {
        $('#txtOldPassword').val('');
        $('#txtNewPassword').val('');
        $('#txtConfirmNewPassword').val('');
    }

    var SetValidation = function () {
        $frmResetPassword = $('#frmResetPassword');
        ValRP = $frmResetPassword.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                oldpassword: {
                    required: true
                },
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
    CtrlResetPassword.init();
});