var CtrlBlockSite = function () {
    var objSvc = null; $frmBlockSite = null;
    var valBlockSite = null;

    var OnLoad = function () {
        SetValidation();
        objSvc = new LoginRegistrationService();
    }

    var RegisterEvent = function () {
        $(document).on('click', '#btnUpdate', function (e) {
            e.preventDefault();
            if ($frmBlockSite.valid()) {
                uiApp.BlockUI();
                objSvc.BlockSite($('#txtPassword').val()).then(function (data) {
                    if (data.IsSuccess) {
                        uiApp.Alert({ container: '#uiPanel1', message: "Block site updated", type: "success" });
                        setInterval(function () {
                            location.reload();
                        }, 3000)
                    } else {
                        uiApp.Alert({ container: '#uiPanel1', message: "Block site not updated", type: "warning" });
                    }
                    uiApp.UnBlockUI();
                }, function (error) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Problem in updating block site", type: "danger" });
                    uiApp.UnBlockUI();
                });
            }
        });
    };

    var SetValidation = function () {
        $frmBlockSite = $('#frmBlockSite');
        valBlockSite = $frmBlockSite.validate({
            keypress: true,
            onfocusout: false,
            rules: {
                password: {
                    required: true
                }
            }
        });
    };


    return {
        init: function () {
            OnLoad();
            RegisterEvent();
        }
    }
}();
$(document).ready(function () {
    CtrlBlockSite.init();
});
