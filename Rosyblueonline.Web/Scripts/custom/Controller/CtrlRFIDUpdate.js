var CtrlRFIDUpdate = function () {

    var objSvc = null;

    var LoadForm = function () {
        objSvc = new RFIDService();
    };

    var RegisterEvent = function () {
        $('#btnRFIDUpdate').click(function (e) {
            e.preventDefault();
            var fd = new FormData();
            fd.append("File", $('#fileUpload').get(0).files[0]);
            objSvc.UpdateRfId(fd).then(function (data) {
                if (data.IsSuccess) {
                    uiApp.Alert({ container: '#uiPanel1', message: "Rfids updated successfully", type: "success" });
                    setTimeout(function () {
                        //location.reload();
                        location.href = '/RFID/Uploadhistory';
                    }, 2000);
                } else {
                    uiApp.Alert({ container: '#uiPanel1', message: data.Message, type: "danger" });
                }
            }, function (error) {
                uiApp.Alert({ container: '#uiPanel1', message: "Problem in adding data", type: "danger" });
            });
        });
    };

    return {
        init: function () {
            LoadForm();
            RegisterEvent();
        }
    }
}();

$(document).ready(function () {
    CtrlRFIDUpdate.init();
});